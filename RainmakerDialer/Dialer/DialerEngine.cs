//#define THUMP

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Threading;
using System.Xml;
using System.Collections;

using System.Timers;

using VoiceElements.Common;
using VoiceElements.Client;
using Rainmaker.Common.DomainModel;

using RainMakerDialer.CampaignWS;

namespace Rainmaker.RainmakerDialer
{
    public class DialerEngine
    {

        #region Static variables

        // A reference to Telephony Server Connection 
        public static TelephonyServer TelephonyServer = null;

        // lock variable
        private static object s_SyncVar = new object();

        // A reference to Log
        public static Log Log = new Log("DialerActivity" + DateTime.Now.ToString("_MMddyyyy") + ".Log");

        public static string LiveAnswerMessage = @"Live.Wav";
        public static string MachineAnswerMessage = @"Machine.Wav";

        // reference to message queue
        public static Queue<string> qMessageQueue = new Queue<string>();

        // reference to campaign queue
        private static Queue<Campaign> qCampaignQueue = new Queue<Campaign>();

        // reference to currently running campaign threads
        public static List<Thread> lstCampaignThreads = new List<Thread>();

        private static bool s_Connected = false;

        public static bool Connected
        {
            get { return DialerEngine.s_Connected; }
        }

        #endregion

        #region Private Variables

        // Campaign polling timer
        private System.Windows.Forms.Timer tmrGetCampaigns = null; 

        // Agent polling timer
        private System.Windows.Forms.Timer tmrGetAgents = null;

        // Admin tasks polling timer
        private System.Windows.Forms.Timer tmrGetAdminRequests = null;

        private long activityId = 0;
        public long ActivityId
        {
            get { return activityId; }
            set { activityId = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public DialerEngine()
        {
        }

        #endregion

        #region Logging

        /// <summary>
        /// Adds Message logged events
        /// </summary>
        public void StartLogging()
        {
            DialerEngine.Log.MessageLogged += new MessageLogged(Log_MessageLogged);
        }

        /// <summary>
        /// Event for message logging
        /// </summary>
        /// <param name="message"></param>
        public void Log_MessageLogged(string message)
        {
            if (Utilities.GetAppSetting("LogToScreen", "yes").Trim() == "yes")
            {
                lock (qMessageQueue)
                {
                    qMessageQueue.Enqueue(message);
                }
            }
        }

        #endregion

        #region Connect/Disconnect Methods

        /// <summary>
        /// Connect To telephony server
        /// </summary>
        /// <param name="telephonyServer"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Connect(string telephonyServer, string secU, string secP)
        {
            tConnect secConn = new tConnect();
            try
            {
                lock (s_SyncVar)
                {
                    System.Net.IPAddress[] ips = System.Net.Dns.GetHostAddresses(telephonyServer);

                    if (ips == null || ips.Length == 0) throw new Exception(
                            "Error: Could not resolve name specified!");

                    string sIpaddress = @"gtcp://" + ips[0].ToString() + ":54331";
                    Log.Write("|DE|Connecting to Voice Elements server at {0}.", sIpaddress);
                    TelephonyServer = new TelephonyServer(sIpaddress, secConn.SecU, secConn.SecP);
                    TelephonyServer.NewCall += new NewCall(m_TelephonyServer_NewCall);
                    TelephonyServer.RegisterDNIS();
                    TelephonyServer.SetSingleClientMode();

                    TelephonyServer.AutoAssignVoiceResources = false;

                    Log.Write("CONNECTED");
                    s_Connected = true;

                    try
                    {
                        this.ActivityId = CampaignAPI.DialerConnected();
                    }
                    catch(Exception ex) 
                    {
                        if (ex.Message.ToLower().IndexOf("unable to connect to the remote server") >= 0)
                        {
                            Log.Write("|DE|Unable to connect, please check setting and location of the VE server.");
                        }
                        else
                        {
                            Log.Write("|DE|Error connecting to VE server.");
                        }
                        Disconnect();
                        return false;
                    }
                }
                return true;
            }
            catch (ElementsException ee)
            {
                TelephonyServer = null;
                Log.WriteException(ee, "Elements Exception Connect");
            }
            catch (Exception ex)
            {
                TelephonyServer = null;
                Log.WriteException(ex, "Exception Connect");
            }
            return false;
        }

        /// <summary>
        /// Disconnect server
        /// </summary>
        public void Disconnect()
        {
            lock (s_SyncVar)
            {
                s_Connected = false;

                if (TelephonyServer != null)
                {
                    try
                    {
                        // stop campaign polling
                        if (tmrGetCampaigns != null)
                        {
                            tmrGetCampaigns.Stop();
                            tmrGetCampaigns = null;
                        }

                        // stop agents pooling
                        if (tmrGetAgents != null)
                        {
                            tmrGetAgents.Stop();
                            tmrGetAgents = null;
                        }

                        // stop running campaigns
                        StopAllCampaigns();

                        // dispose all channels
                        ManagedChannel.Dispose();
                        ManagedAgent.Dispose();

                        CampaignProcess.Dispose();
                    }
                    catch { }

                    try
                    {
                        CampaignAPI.DialerStoped(this.ActivityId);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Updating Dialer start error");
                    }

                    TelephonyServer.NewCall -= new NewCall(m_TelephonyServer_NewCall);
                    TelephonyServer.Dispose();
                    TelephonyServer = null;
                    
                    // wait for a while, Child threads release the resources
                    Thread.Sleep(10);
                    Log.Write("|DE|Disconnected from VE server.");
                }
            }
        }

        #endregion

        #region Newcall Event

        /// <summary>
        /// Event for new call
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_TelephonyServer_NewCall(object sender, NewCallEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "NewCall");
            }
        }

        #endregion

        #region Campaigning Methods

        /// <summary>
        /// Entry point for campaign process
        /// </summary>
        public void Start()
        {
            // This is the main application entry point when the dialer is started **
            try
            {
                if (TelephonyServer != null)
                {
                    try
                    {
                        CampaignAPI.DialerStarted(this.ActivityId);
                    }
                    catch(Exception ex) 
                    {
                        Log.WriteException(ex, "Updating Dialer start error");
                    }

                    // Added GW 10.01.10 to reset any campaigns that somehow locked into run or paused states.
                    CampaignAPI.ResetCampaignsToIdle();

                    try
                    {
                        ManagedChannel.Initialize();
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Channels are not allocated properly please check");
                        Disconnect();
                        return;
                    }
                    GetLoggodInAgents();
                    tmrGetAgents = new System.Windows.Forms.Timer(); 
                    tmrGetAgents.Tick += new EventHandler(Timer_GetAgents); 
                    tmrGetAgents.Interval = Convert.ToInt32(
                            Utilities.GetAppSetting("AgentPollingInterval", "10000"));
                    tmrGetAgents.Enabled = true;

                    GetCampaigns();
                    tmrGetCampaigns = new System.Windows.Forms.Timer(); 
                    tmrGetCampaigns.Tick += new EventHandler(Timer_GetCampaigns);
                    tmrGetCampaigns.Interval = Convert.ToInt32(
                            Utilities.GetAppSetting("CampaignPollingInterval", "10000"));
                    tmrGetCampaigns.Enabled = true;

                    

                    // Added for support of PromptRecorder
                    GetAdminRequests();
                    tmrGetAdminRequests = new System.Windows.Forms.Timer();
                    tmrGetAdminRequests.Tick += new EventHandler(Timer_GetAdminRequests);
                    tmrGetAdminRequests.Interval = Convert.ToInt32(
                            Utilities.GetAppSetting("AdminPollingInterval", "10000"));
                    tmrGetAdminRequests.Enabled = true;
                    Log.Write("|DE|Admin timer interval set to {0}.", tmrGetAdminRequests.Interval);
                }
                else
                {
                    Log.Write("");
                    Log.Write("|DE|Not Connected to VE server.");
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error in Start");
            }
        }

        /// <summary>
        /// Get all loggedin agents and add it to pool
        /// </summary>
        private void GetLoggodInAgents()
        {
            try
            {
                //lock (s_SyncVar)
                //{
                    // Update agents pool
                    List<Agent> agentList = AgentAPI.GetLoggedInAgents();
                    if (agentList != null)
                    {
                        ManagedAgent.InitializeAgents(agentList);
                    }
               // }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error Getting Campaigns");
            }
        }

        /// <summary>
        /// Get Campaigns
        /// </summary>
        private void GetCampaigns()
        {
            try
            {
                Queue<Campaign> campaignQueueFromDB = CampaignAPI.GetAllCampaigns();
                lock (s_SyncVar)
                {
                    if (campaignQueueFromDB.Count != 0)
                    {

                        if (CheckAndUpdateCampaigns(campaignQueueFromDB))
                        {
                            StartCampaignProcess();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error Getting Campaigns");
            }
        }

        /// <summary>
        /// Returns true if new campaigns are added for running, 
        /// Updates currently running campaigns if status changed to pause or idle
        /// </summary>
        /// <param name="campaignQueueFromDB"></param>
        /// <returns></returns>
        private bool CheckAndUpdateCampaigns(Queue<Campaign> campaignQueueFromDB)
        {
            //Log.Write("Check and update campaigns invoked.");
            bool added = false;
            try
            {
                Campaign camp = null;
                while (campaignQueueFromDB.Count != 0)
                {
                    bool bIsRun = false;
                    int threadIndex = 0;
                    lock (lstCampaignThreads)
                    {
                        camp = campaignQueueFromDB.Dequeue();
                        for (int i = 0; i < lstCampaignThreads.Count; i++)
                        {
                            
                            //Log.Write("Campaign thread list contains thread: " + lstCampaignThreads[i].Name + ".");
                            if (lstCampaignThreads[i].Name == camp.ShortDescription)
                            {
                                bIsRun = true;
                                threadIndex = i;
                                break;
                            }
                        }
                    }
                    if (!bIsRun) // currently not running 
                    {
                        if (camp.StatusID == (long)CampaignStatus.Run)
                        {
                            // add to list to start new campaign
                            Log.Write("Engine has determined start, adding to queue for campaign " + camp.CampaignID);
                            Log.Write("|DE|{0}|{1}|Initiating campaign start.", camp.CampaignID, camp.ShortDescription);
                            lock (qCampaignQueue)
                            {
                                qCampaignQueue.Enqueue(camp);
                                added = true;
                            }
                        }
                    }
                    else // running, check status and take appropriate action
                    {
                        camp.StatusID = CampaignAPI.GetCampaignStatus(camp.CampaignID);
                        switch (camp.StatusID)
                        {
                            case (long)CampaignStatus.FlushIdle:
                            case (long)CampaignStatus.Pause:
                            case (long)CampaignStatus.FlushPaused: 
                                if (lstCampaignThreads[threadIndex].Priority != ThreadPriority.Lowest)
                                {
                                    Log.Write("|DE|{0}|{1}|Pause or idled triggered, setting thread priority on {2}.", camp.CampaignID, camp.ShortDescription, lstCampaignThreads[threadIndex].Name);
                                    lock (lstCampaignThreads)
                                    {
                                        lstCampaignThreads[threadIndex].Priority = ThreadPriority.Lowest;
                                    }
                                }
                                break;
                            case (long)CampaignStatus.Completed:
                            case (long)CampaignStatus.Idle:
                                Log.Write("|DE|{0}|{1}|Campaign complete.", camp.CampaignID, camp.ShortDescription);
                                RemoveCampaignFromList(camp.ShortDescription, true);
                                break;
                            case (long)CampaignStatus.Run:
                                try
                                {
                                    if (lstCampaignThreads[threadIndex].Priority == ThreadPriority.Lowest)
                                    {
                                        Log.Write("Engine has determined paused reactivate for thread " + lstCampaignThreads[threadIndex].Name);
                                        Log.Write("|DE|{0}|{1}|Pause reactivate campaign.", camp.CampaignID, camp.ShortDescription);
                                        lock (lstCampaignThreads)
                                        {
                                            lstCampaignThreads[threadIndex].Priority = ThreadPriority.Normal;
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (ex.Message.IndexOf("dead") > 0)
                                    {
                                        //lstCampaignThreads.Remove[threadIndex];
                                        Log.Write("|DE|{0}|{1}|Dead campaign thread found, removing campaign.", camp.CampaignID, camp.ShortDescription);
                                        RemoveCampaignFromList(camp.ShortDescription, true);
                                    }
                                    else
                                        throw;
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error in CheckAndUpdateCampaigns");
            }
            finally
            {
                // Log.Write("CheckAndUpdateCampaigns End");
            }
            return added;
        }

        /// <summary>
        /// Remove campaign from list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stopCampaign"></param>
        public static void RemoveCampaignFromList(string name, bool stopCampaign)
        {
            DialerEngine.Log.Write("Remove campaign start " + name);
            Log.Write("|DE|Remove {0} thread begin.", name);
            try
            {
                lock (lstCampaignThreads)
                {
                    for (int i = 0; i < lstCampaignThreads.Count; i++)
                    {
                        if (lstCampaignThreads[i].Name == name)
                        {
                            if (stopCampaign)
                            {
                                try
                                {
                                    // stop campaigning.  This method will trigger on idle or completed.
                                    Log.Write("|DE|Abort {0} thread begin.", name);
                                    if (lstCampaignThreads[i].IsAlive)
                                        lstCampaignThreads[i].Abort();
                                }
                                catch (Exception ee)
                                {
                                    Log.WriteException(ee, "Error aborting thread.");
                                    //throw ee;
                                }                            
                            }
                            try
                            {
                                lstCampaignThreads[i] = null;
                                lstCampaignThreads.RemoveAt(i);

                                // Remove silent call counts
                                CampaignProcess.RemoveSilentCallCount(name);
                            }
                            catch (Exception ee)
                            {
                                Log.WriteException(ee, "Error in RemoveCampaignFromList RemoveAt");
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error in RemoveCampaignFromList");
                throw ex;
            }
            finally
            {
                //DialerEngine.Log.Write("Remove end");
            }
        }

        /// <summary>
        /// Stop and remove all campaigns
        /// </summary>
        private static void StopAllCampaigns()
        {
            Log.Write("|DE|Stop all campaigns invoked.");
            lock (lstCampaignThreads)
            {
                try
                {
                    if (lstCampaignThreads.Count > 0)
                    {
                        for (int i = 0; i < lstCampaignThreads.Count; i++)
                        {
                            try
                            {
                                // stop campaigning
                                Log.Write("|DE|Campaign thread abort {0}.", lstCampaignThreads[i].Name);
                                if (lstCampaignThreads[i].IsAlive)
                                    lstCampaignThreads[i].Abort();
                                //break;
                            }
                            catch (Exception ex)
                            {
                                Log.WriteException(ex, "Error in StopAllCampaigns");
                            }
                        }
                    }
                    lstCampaignThreads.Clear();

                    CampaignProcess.RemoveSilentCallCount("-ALL-");

                    // Added GW 10.01.10 to reset any campaigns that somehow locked into run or paused states.
                    CampaignAPI.ResetCampaignsToIdle();

                }
                catch (Exception ex1)
                {
                    Log.WriteException(ex1, "Error in StopAllCampaigns Main");
                }
            }
            Log.Write("|DE|Stop all campaigns complete.");
        }

        /// <summary>
        /// Start campaigns for dialing
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        private void StartCampaignProcess()
        {
            Log.Write("|DE|Checking for start times for {0} new campaigns.", qCampaignQueue.Count);
            if (qCampaignQueue.Count == 0)
            {
                // no campaigns
                DialerEngine.Log.Write("No active campaigns found");
                return;
            }

            // clear
            GC.Collect();
            try
            {
                ThreadStart ts = null;

                while (qCampaignQueue.Count != 0)
                {
                    Campaign objCampaign = null;
                    lock (qCampaignQueue)
                    {
                        objCampaign = qCampaignQueue.Dequeue();
                    }
                    // add to running list
                    // m_RunningCampaignIdList.Add(campaign.CampaignID.ToString());

                    DialingParameter objDialParam = CampaignAPI.GetDialParam(objCampaign);
                    OtherParameter objOtherParam = null;
                    bool bStartCampaign = false;

                    CallType callType = CallType.AMCall;
                    DateTime dtStartTime;
                    if (DateTime.Now.Hour >= 12)
                    {
                        callType = CallType.PMCall;
                        dtStartTime = objDialParam.PMDialingStart;
                    }
                    else
                    {
                        dtStartTime = objDialParam.AMDialingStart;
                    }

                    int iCurrHour = DateTime.Now.Hour;
                    int iCurrMinutes = DateTime.Now.Minute;
                    int iDPhour = dtStartTime.Hour;
                    int iDPMinutes = dtStartTime.Minute;

                    //-------------------------------------------------
                    // We ignore time issue for anything
                    // other than unmanned campaigns.
                    //-------------------------------------------------
                    Log.Write("|DE|Campaign - Dialing Mode: {0})", objDialParam.DialingMode.ToString());
                    if (objDialParam.DialingMode != 6)
                    {
                        bStartCampaign = true;
                    }
                    else if (iCurrHour > iDPhour || ((iCurrHour == iDPhour) && (iCurrMinutes >= iDPMinutes)))
                    {
                        bStartCampaign = true;
                    }

                    try
                    {
                        if (bStartCampaign)
                        {
                            DigitalizedRecording digRecording = CampaignAPI.GetDigitizedRecordings(objCampaign);

                            objOtherParam = CampaignAPI.GetOtherParam(objCampaign);

                            CampaignProcess campProcess = new CampaignProcess(objCampaign, objDialParam, objOtherParam);
                            if (digRecording != null)
                            {
                                campProcess.RecordingsPath = digRecording.RecordingFilePath;
                                campProcess.RecordCalls = digRecording.EnableRecording;
                                campProcess.RecordBeep = digRecording.StartWithABeep;
                            }

                            // weekend call checking
                            if ( DateTime.Now.DayOfWeek == DayOfWeek.Saturday ||
                                   DateTime.Now.DayOfWeek == DayOfWeek.Sunday )
                            {
                                callType = CallType.WkendCall;
                            }

                            campProcess.CallType = callType;

                            Log.Write("|DE|Starting campaign '{0}'.", objCampaign.ShortDescription);

                            // Start campaignprocess thread.  Different startup method for normal / unmanned mode
                            if (objDialParam.DialingMode == Convert.ToInt32(DialingMode.Unmanned))
                            {
                                ts = new ThreadStart(campProcess.RunCampaignUnmannedMode);
                            }
                            else
                            {
                                ts = new ThreadStart(campProcess.RunCampaign);
                            }
                            Thread t = new Thread(ts);
                            t.Priority = ThreadPriority.Normal;
                            t.IsBackground = true;
                            t.Name = objCampaign.ShortDescription.ToString();

                            lock (lstCampaignThreads)
                            {
                                lstCampaignThreads.Add(t);
                            }
                            if (objDialParam.DialingMode != Convert.ToInt32(DialingMode.ManualDial))
                            {
                                t.Start();
                            }
                        }
                        else
                        {
                            Log.Write("|DE|Campaign '{0}' not started, outside of schedule range. (Start time = {1}:{2})",
                                objCampaign.ShortDescription, iDPhour.ToString(), iDPMinutes.ToString());
                            
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Error in Starting Campaign " + objCampaign.ShortDescription);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error in StartCampaignProcess");
            }
            finally
            {
                //
            }
        }

        #endregion

        #region Timer Events

        /// <summary>
        /// Timer event for agents polling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eArgs"></param>
        private void Timer_GetAgents(object sender, EventArgs eArgs)
        {
            //Log.Write("Get Logged In Agents from database");
            try
            {
                if (tmrGetAgents != null)
                {
                    // stop timer
                    //tmrGetAgents.Stop();
                    tmrGetAgents.Enabled = false;       

                    if (DialerEngine.Connected)
                    {
                        // Get loggedIn Agents
                        GetLoggodInAgents();
                    }
                    Thread.Sleep(1);                    
                    // start the timer
                    tmrGetAgents.Enabled = true;        
                    //tmrGetAgents.Start();
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Timer_GetAgents");
            }

            //Log.Write("End - Get LoggodIn Agents from database");
        }

        /// <summary>
        /// Timer event for campaigns polling
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="eArgs"></param>
        private void Timer_GetCampaigns(object sender, EventArgs eArgs)
        {
#if THUMP
            Log.Write("|DE|Heartbeat - thump.");
#endif
            try
            {
                if (tmrGetCampaigns != null)
                {
                    // stop timer
                    //tmrGetCampaigns.Stop();
                    tmrGetCampaigns.Enabled = false;  

                    // Get active campaigns
                    GetCampaigns();
                    Thread.Sleep(1);                    
                    // start the timer
                    //tmrGetCampaigns.Start();
                    tmrGetCampaigns.Enabled = true;     
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Timer_GetCampaigns");
            }
            //Log.Write("End - Get campaigns from database");
        }

        #endregion

        #region Admin Tasks - Events & Methods

        private void Timer_GetAdminRequests(object sender, EventArgs eArgs)
        {
            try
            {
                if (tmrGetAdminRequests != null)
                {
                    // stop timer
                    //tmrGetCampaigns.Stop();
                    //tmrGetAdminRequests.Enabled = false;

                    // Get active campaigns
                    GetAdminRequests();
                    Thread.Sleep(1);
                    // start the timer
                    //tmrGetCampaigns.Start();
                    //tmrGetAdminRequests.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Timer_GetAdmin Exception.");
            }
        }

        private void GetAdminRequests()
        {
            try
            {
                //Log.Write("|DE|Admin check.");
                DataSet dsAdminRequests = CampaignAPI.GetAdminRequests();

                if (dsAdminRequests.Tables[0].Rows.Count < 1)
                {
                    // No pending admin tasks
                    dsAdminRequests = null;
                    return;
                }

                foreach (DataRow dr in dsAdminRequests.Tables[0].Rows)
                {
                    DateTime dtSubmitted = Convert.ToDateTime(dr["DateTimeSubmitted"]);
                    TimeSpan ts = (DateTime.Now - dtSubmitted);
                    if (ts.TotalMinutes >= 15)
                    {
                        CampaignAPI.UpdateAdminRequestStatus(Convert.ToInt64(dr["RequestID"]), 3);
                    }
                    else
                    {
                        // Spin off recorder thread
                        string targetNumber = dr["RequestData"].ToString();
                        PromptRecorder recorder = new PromptRecorder(targetNumber);
                        ThreadStart ths = new ThreadStart(recorder.RunScript);

                        Thread t = new Thread(ths);
                        t.Name = "Recorder:" + targetNumber;
                        Log.Write("|DE|{0}|Prompt recorder calling.", targetNumber);

                        t.Start();
                        CampaignAPI.UpdateAdminRequestStatus(Convert.ToInt64(dr["RequestID"]), 2);
                        break;
                    }
                }
                return;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error Getting Admin Requests");
            }
        }

        #endregion
        
    }
}
