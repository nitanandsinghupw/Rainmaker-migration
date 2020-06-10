using System;
using System.Web;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net.Sockets;
using VoiceElements.Client;
using VoiceElements.Common;
using Rainmaker.Common.DomainModel;

namespace Rainmaker.RainmakerDialer
{
    public class Dialer
    {
        #region Variables and properties

        // References the main Log File created at startup
        private static Log Log = DialerEngine.Log;

        // Reference to the managed agent
        private ManagedAgent managedAgent = null;
        public ManagedAgent ManagedAgent
        {
            get { return managedAgent; }
            set
            {
                lock (this)
                {
                    if (m_TerminateCall) throw new Exception("Agent No Longer Needed.");
                    managedAgent = value;
                }
                //Log.Write("ManagedAgent Set");                      
                threadEvent.Set();
            }
        }

        // GW 10.17.10 Reference to owner campaign process
        private CampaignProcess ownerCampaign = null;
        public CampaignProcess OwnerCampaign
        {
            get { return ownerCampaign; }
            set { ownerCampaign = value; }
        }

        // a reference to the ManagedChannel the call arrived on
        private ManagedChannel managedChannel = null;
        public ManagedChannel ManagedChannel
        {
            get { return managedChannel; }
            set { managedChannel = value; }
        }

        private AutoResetEvent threadEvent = new AutoResetEvent(false);

        private bool m_NoAvailableAgents = false;

        public bool NoAvailableAgents
        {
          get { return m_NoAvailableAgents; }
            set
            {
                Log.Write("|DLR|NoAvailableAgents Set to: " + value.ToString());         
                lock (this)
                {
                    m_NoAvailableAgents = value;
                }
                threadEvent.Set();
            }
        }


        // Monroe 2.24.09
        private bool m_TerminateCall = false;

        private bool silentCallAdded = false;

        public bool TerminateCall
        {
            get { return m_TerminateCall; }
            set
            {
                Log.Write("|DLR|Terminate Call Set to: " + value.ToString());           
                lock (this)
                {
                    m_TerminateCall = value;
                }
                threadEvent.Set();
            }
        }


        private bool isDialing = false;

        // a reference to current call details  
        private CampaignDetails callDetails;
        public CampaignDetails CallDetails
        {
            get { return callDetails; }
            set { callDetails = value; }
        }

        // a reference to current campaign
        private Campaign campaign;
        public Campaign Campaign
        {
            get { return campaign; }
            set { campaign = value; }
        }

        // a reference to current campaign's dialingparameted
        private DialingParameter dialParameter;
        public DialingParameter DialParameter
        {
            get { return dialParameter; }
            set { dialParameter = value; }
        }

        // a reference to current campaign's dialingparameted
        private OtherParameter otherParameter;
        public OtherParameter OtherParameter
        {
            get { return otherParameter; }
            set { otherParameter = value; }
        }

        // the files to play, machine message
        private string machineMessage = "";
        public string MachineMessage
        {
            get { return machineMessage; }
            set { machineMessage = value; }
        }

        // the files to play, human message
        private string humanMessage = "";
        public string HumanMessage
        {
            get { return humanMessage; }
            set { humanMessage = value; }
        }

        // the files to play, transfer message
        private string transferMessage = "";
        public string TransferMessage
        {
            get { return transferMessage; }
            set { transferMessage = value; }
        }

        // the files to play, transfer message
        private string holdMessage = "";
        public string HoldMessage
        {
            get { return holdMessage; }
            set { holdMessage = value; }
        }

        // the files to play, silent call message
        private string silentCallMessage = "";
        public string SilentCallMessage
        {
            get { return silentCallMessage; }
            set { silentCallMessage = value; }
        }

        private CallType callType = CallType.AMCall;
        public CallType CallType
        {
            get { return callType; }
            set { callType = value; }
        }

        // Reference to RecordingsPath
        private string recordingsPath = string.Empty;
        public string RecordingsPath
        {
            get { return recordingsPath; }
            set { recordingsPath = value; }
        }

        // Reference to record calls switch GW 09.28.10
        private bool recordCalls = false;
        public bool RecordCalls
        {
            get { return recordCalls; }
            set { recordCalls = value; }
        }

        // Reference to record calls switch GW 09.28.10
        private bool recordBeep = false;
        public bool RecordBeep
        {
            get { return recordBeep; }
            set { recordBeep = value; }
        }

        // a reference to current running query id
        private long queryId;
        public long QueryId
        {
            get { return queryId; }
            set { queryId = value; }
        }

        // a reference to current running query
        private Query query;
        public Query Query
        {
            get { return query; }
            set { query = value; }
        }

        // a reference to campaignstats
        private CampaignStats campStats;
        public CampaignStats CampStats
        {
            get { return campStats; }
            set { campStats = value; }
        }

        private int callInterval;
        public int CallInterval
        {
            get { return callInterval; }
            set { callInterval = value; }
        }

        private bool answeredCall;
        public bool AnsweredCall
        {
            get { return answeredCall; }
            set { answeredCall = value; }
        }

        public DialResult CurrentDialResult;

        //06/12
        private bool m_isTransferedCall = false;
        public bool IsTransferedCall
        {
            get { return m_isTransferedCall; }
            set { m_isTransferedCall = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="campaign"></param>
        public Dialer(Campaign campaign, CampaignProcess ownerCamp)
        {
            // sets current campaign
            this.Campaign = campaign;
            this.ownerCampaign = ownerCamp;
        }

        #endregion
        
        #region Run Script - Manned
        /// <summary>
        /// The main script, Dials the number and routes the call to an agent
        /// </summary>
        public void RunScript()
        {
            // Check for unmanned mode
            if (dialParameter.DialingMode == Convert.ToInt32(DialingMode.Unmanned))
            {
                try
                {
                    RunScriptForUnmannedMode();
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "Unmanned runscript Exception");
                }
                return;
            }

            // Moved to allow finally block to use object
            CampaignQueryStatus campQueryStatus = new CampaignQueryStatus();

            try
            {
                DateTime dtCallStartTime = DateTime.Now;

                // Get a channel for outbound usage from the server...
                ManagedChannel = ManagedChannel.GetOutboundChannel(Campaign.CampaignID);

                // Update Calllist (number of trials)
                CampaignAPI.UpdateCallDetails(Campaign, CallDetails, CallType, QueryId);

                if (ManagedChannel == null || ManagedChannel.ChannelResource == null)
                {
                    // Channels not available, prior checking also implemented this will not happen
                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|No channels available, thread has been started without a channel.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                    return;
                }

                //ChannelResource = ManagedChannel.ChannelResource;
                //VoiceResource = ChannelResource.VoiceResource;

                // Suscribes to the disconnect event to let us know if the caller hangs up the phone.
                try
                {
                    ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                }
                catch { }
                ManagedChannel.ChannelResource.Disconnected += new Disconnected(ChannelResource_Disconnected);

                // Set outbound caller id
                ManagedChannel.ChannelResource.OriginatingPhoneNumber = this.campaign.OutboundCallerID;

                // Set messages variables
                
                string strUploadDirectory;
                string ismultiboxconfig = ConfigurationManager.AppSettings["IsMultiBoxConfig"];
                if (ismultiboxconfig == "yes" || ismultiboxconfig == "Yes" || ismultiboxconfig == "YES")
                {

                    //if its a multibox config use this path
                    strUploadDirectory = ConfigurationManager.AppSettings["UploadPromptsPathMulti"];
                    MachineMessage = HttpContext.Current.Server.MapPath(strUploadDirectory + dialParameter.AnsweringMachineMessage);
                    HumanMessage = HttpContext.Current.Server.MapPath(strUploadDirectory +dialParameter.HumanMessage);
                    SilentCallMessage = HttpContext.Current.Server.MapPath(strUploadDirectory +dialParameter.SilentCallMessage);
                }
                else
                {
                    //if its a single box configuration use this
                    MachineMessage = dialParameter.AnsweringMachineMessage;
                    HumanMessage = dialParameter.HumanMessage;
                    SilentCallMessage = dialParameter.SilentCallMessage;
                }

                bool AnsweringMachineDetectionOn = false;
                try
                {
                    AnsweringMachineDetectionOn = dialParameter.AnsweringMachineDetection;
                    // Added by GW to truly disable detection for testing.
                    if (AnsweringMachineDetectionOn)
                    {
                        ManagedChannel.ChannelResource.CallProgress = CallProgress.AnalyzeCall;
                    }
                    else
                    {
                        ManagedChannel.ChannelResource.CallProgress = CallProgress.WaitForConnect;
                    }
                }
                catch { }

                //Make sure we are routed to the voice resource!
                if (ManagedChannel.ChannelResource is T1Channel)
                {
                    ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                }

                DialResult dr;

                // Added GW 11.07.10 - CPA overrides
                ManagedChannel.ChannelResource.CallProgressTemplate = @"Dialogic\DxCap";
                
                Dictionary<string, int> overrides = new Dictionary<string, int>();
                if (DialParameter.RingSeconds > 6)
                {
                    overrides.Add("ca_noanswer", ((DialParameter.RingSeconds - 6) * 100));
                }
                else
                {
                    overrides.Add("ca_noanswer", (DialParameter.RingSeconds * 100));
                }

                ManagedChannel.ChannelResource.CallProgressOverrides = overrides;

                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|{5}|Dialing for {6} seconds on channel {7}.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, CallDetails.PhoneNum, callDetails.UniqueKey, DialParameter.RingSeconds, ManagedChannel.ChannelResource.DeviceName);
                string strDialString = BuildDialString();

                try
                {

                    lock (this) { isDialing = true; }

                    dr = ManagedChannel.ChannelResource.Dial(strDialString);

                    lock (this) { isDialing = false; }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "General dialing exception.");
                    dr = DialResult.Failed;
                }
                finally
                {
                    ManagedChannel.ChannelResource.CallProgress = CallProgress.WaitForConnect;

                    CampaignProcess.UpdateCampaignDialCount(Campaign.CampaignID, false);
                }

                // Log dial result
                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|{5}|The dial result was {6}.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, callDetails.UniqueKey, dr);

                bool bUpdCompletionStatus = false;
                bool bScheduleIt = false;
                DateTime dtStartTime = DateTime.Now;
                DateTime dtEndTime = DateTime.Now;

                // Moved to allow finally block to use object
                //CampaignQueryStatus campQueryStatus = new CampaignQueryStatus();
                campQueryStatus.QueryID = this.QueryId;
                campQueryStatus.Dials = 1;

                // update result to call list table
                CurrentDialResult = dr;

                try
                {
                    // Update stats and add to calllist
                    callInterval = (int)(DateTime.Now.Subtract(dtCallStartTime).TotalSeconds);
                    answeredCall = (dr == DialResult.Connected || dr == DialResult.HumanDetected
                                    || dr == DialResult.Successful);

                    m_UpdateStatsThread = new Thread(new ThreadStart(UpdateStats));
                    m_UpdateStatsThread.Name = callDetails.PhoneNum + "_Stats";
                    m_UpdateStatsThread.IsBackground = true;
                    m_UpdateStatsThread.Start();
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "Runscript exception spawning stats thread.");
                }

                switch (dr)
                {
                    case DialResult.Connected:
                    case DialResult.HumanDetected:
                    case DialResult.Successful:
                    case DialResult.MachineDetected:
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|System has detected an answer, machine detection set to {5}.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, AnsweringMachineDetectionOn);
                        // If Answering machine detection is enabled    04/24/2010
                        if (dr == DialResult.MachineDetected && AnsweringMachineDetectionOn)
                        {
                            bUpdCompletionStatus = true;

                            campQueryStatus.AnsweringMachine = 1;

                            // Hang up on machines if we didn't specify a message for machines.
                            if (MachineMessage == null) break;

                            try
                            {
                                // Set the MaximumSilence to wait for 3 seconds of silence.  This value is set in deciSeconds (1/10th of a second).
                                ManagedChannel.ChannelResource.VoiceResource.MaximumSilence = 30;

                                // Also set the Maximum Overall Time to wait for the 3 seconds.  (If greeting is longer than 60 seconds, just play the message).
                                // MaximumTime is specified in deciSeconds (1/10th of a second).
                                ManagedChannel.ChannelResource.VoiceResource.MaximumTime = 600;
                                ManagedChannel.ChannelResource.VoiceResource.GetSilence();

                                // Forec the play to run to completion
                                ManagedChannel.ChannelResource.VoiceResource.TerminationDigits = "";

                                if (MachineMessage != "")
                                {
                                    // Play the file.
                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Playing answering machine file '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, MachineMessage);
                                    try
                                    {
                                        ManagedChannel.ChannelResource.VoiceResource.Play(MachineMessage);
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Bad answering machine file encountered: '{5}'. Play Failed", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, MachineMessage);
                                    }
                                }
                                else
                                {
                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Answering machine message not defined or invalid, no message was sent.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.WriteException(ex, "Machine Detected Execution Exception.");
                            }

                            dtEndTime = DateTime.Now;

                            //break;
                        }
                        else
                        {
                            bUpdCompletionStatus = true;
                            threadEvent.Reset();

                            // Request an Agent!
                            Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|{5}|Requesting an agent.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, callDetails.UniqueKey);
                            AgentRequest.GetMeAnAgent(this);

                            //DateTime nextWaitMessage = DateTime.MinValue;
                            DateTime nextWaitMessage = DateTime.Now.AddSeconds(2.0);
                            while (true)
                            {
                                if (m_TerminateCall)
                                {
                                    if (ManagedAgent == null) AgentRequest.CancelMyAgentRequest(this);
                                    break;
                                }

                                if (m_NoAvailableAgents)
                                {

                                    // Add to silent call counts
                                    CampaignProcess.AddSilentCall(Campaign.ShortDescription);

                                    if (m_VoiceResourceThread != null) ManagedChannel.ChannelResource.VoiceResource.Stop();
                                    while (m_VoiceResourceThread != null) Thread.Sleep(100);

                                    // Silent call , play message to customer
                                    string strMessage = Utilities.GetAppSetting("SilentCallMessage", "Sorry, No agent found");
                                    ManagedChannel.ChannelResource.VoiceResource.TerminationDigits = "";


                                    // Silent call, log the details to Database
                                    SilentCall silentCall = new SilentCall();
                                    silentCall.DateTimeofCall = dtStartTime;
                                    silentCall.UniqueKey = CallDetails.UniqueKey;
                                    CampaignAPI.LogSilentCall(Campaign, silentCall);
                                    campQueryStatus.Drops = 1;
                                    dtEndTime = DateTime.Now;
                                    silentCallAdded = true;
                                    // info 04/27
                                    try
                                    {
                                        if (SilentCallMessage != "")
                                        {
                                            // Play the file.
                                            //ManagedChannel.ChannelResource.VoiceResource.TerminationDigits = "";
                                            
                                            Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Playing dropped call file '{0}'", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, SilentCallMessage);

                                            try
                                            {
                                                ManagedChannel.ChannelResource.VoiceResource.Play(SilentCallMessage);
                                            }
                                            catch (Exception)
                                            {
                                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Bad dropped call file encountered: '{0}'. Play failed.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, SilentCallMessage);
                                            }
                                        }
                                    }
                                    catch (Exception ee)
                                    {
                                        Log.Write("Silent call msg play Exception : " + ee.Message);
                                    }

                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Silent call has been determined.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);

                                    break;
                                }

                                // We got an agent!
                                if (ManagedAgent != null) break;

                                if (DateTime.Now > nextWaitMessage)
                                {
                                    if (m_TerminateCall)
                                        break;
                                    if (m_VoiceResourceThread == null)
                                    {
                                        ThreadStart ts = new ThreadStart(PlayWaitMessage);
                                        m_VoiceResourceThread = new Thread(ts);
                                        m_VoiceResourceThread.Name = CallDetails.PhoneNum + ".PlayWait";
                                        m_VoiceResourceThread.IsBackground = true;
                                        m_VoiceResourceThread.Start();


                                        double dWaitMsgDelay = 60.0;
                                        try
                                        {
                                            dWaitMsgDelay = Convert.ToDouble(Utilities.GetAppSetting("CustomerWaitMessageDelay", "60"));
                                        }
                                        catch { }
                                        nextWaitMessage = DateTime.Now.AddSeconds(dWaitMsgDelay);
                                    }
                                }

                                threadEvent.WaitOne(1000, false);

                                //if (m_TerminateCall)
                                //    break;

                                if (!DialerEngine.Connected) TerminateCall = true;

                            }

                            threadEvent.Reset();
                            if (m_TerminateCall || m_NoAvailableAgents)
                            {
                                // If agent arrived after the call was hungup.  Make him available again.
                                campQueryStatus.Drops = 1;
                                try
                                {
                                    if (!silentCallAdded)
                                    {
                                        // Silent call, log the details to Database
                                        SilentCall silentCall = new SilentCall();
                                        silentCall.DateTimeofCall = dtStartTime;
                                        silentCall.UniqueKey = CallDetails.UniqueKey;
                                        CampaignAPI.LogSilentCall(Campaign, silentCall);

                                        try
                                        {
                                            if (!m_TerminateCall && SilentCallMessage != "")
                                            {
                                                // Play the file.
                                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Playing dropped call message '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, SilentCallMessage);
                                                try
                                                {
                                                    ManagedChannel.ChannelResource.VoiceResource.Play(SilentCallMessage);
                                                }
                                                catch (Exception ex)
                                                {
                                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Bad dropped call file encountered: '{0}'. Play failed.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, SilentCallMessage);
                                                }
                                            }
                                        }
                                        catch (Exception ee)
                                        {
                                            Log.Write("Silent call msg play Exception2 : " + ee.Message);
                                        }

                                        Log.Write("Silent Call");
                                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Silent call detected.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                                    }
                                }
                                catch {}
                                if (ManagedAgent != null)
                                {
                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Updating agent status, dialer being set to null.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                                    ManagedAgent.Dialer = null;
                                }
                                break;
                            }

                            if (ManagedAgent == null)
                            {
                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Managed agent is null!.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                            }
                            else
                            {
                                callDetails.AgentID = ManagedAgent.AgentDetails.AgentID.ToString();
                                callDetails.AgentName = ManagedAgent.AgentDetails.AgentName;
                                callDetails.VerificationAgentID = ManagedAgent.AgentDetails.VerificationAgent ? ManagedAgent.AgentDetails.AgentID.ToString() : "";

                                AddAgentToCallDetails(false);

                                if (m_VoiceResourceThread != null) ManagedChannel.ChannelResource.VoiceResource.Stop();
                                while (m_VoiceResourceThread != null) Thread.Sleep(100);

                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Checking call recording switch which is set to {5}.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, ownerCampaign.RecordCalls);

                                if (!m_TerminateCall && ownerCampaign.RecordCalls)
                                {

                                    ThreadStart ts = new ThreadStart(Record);
                                    m_VoiceResourceThread = new Thread(ts);

                                    string timestamp = DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss");
                                    m_VoiceResourceThread.Name = CallDetails.PhoneNum + "_" + timestamp + "O.Record";
                                    m_VoiceResourceThread.IsBackground = true;
                                    m_VoiceResourceThread.Start();
                                }

                                // This is the main loop waiting for a disconnect and while in call
                                while (true)
                                {
                                    // At this point Any Thread event is a disconnect...
                                    if (m_TerminateCall) break;
                                    if (threadEvent.WaitOne(1000, false)) break;
                                    if (!DialerEngine.Connected) TerminateCall = true;
                                }

                                // Stop the record...
                                try
                                {
                                    ManagedChannel.ChannelResource.VoiceResource.Stop();
                                }
                                catch { }

                                ManagedAgent temp = ManagedAgent;
                                if (temp != null)
                                {
                                    temp.Dialer = null;
                                }

                                #region Transfer Call 1.18.11
                                // 06/22 - Call transfer implementation
                                if (IsTransferedCall)
                                {
                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|{5}|Transfer notification received.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, callDetails.UniqueKey);
                                    m_TerminateCall = false;
                                    ManagedAgent = null;
                                    // 06/22 - Transfer the call
                                    try
                                    {
                                        if (OtherParameter.CallTransfer == (int)CallBackOptions.AllowOnSiteCallTransfer)
                                        {
                                            // Transfer to verification agent
                                            Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|{5}|Transfer to verification agent.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, callDetails.UniqueKey);

                                            if (OtherParameter.TransferMessageEnable)
                                            {
                                                transferMessage = OtherParameter.TransferMessage;

                                                if (transferMessage != "")
                                                {
                                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Playing transfer message '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, TransferMessage);
                                                    ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                                                    ManagedChannel.ChannelResource.VoiceResource.TerminationDigits = "";

                                                    try
                                                    {
                                                        ManagedChannel.ChannelResource.VoiceResource.Play(TransferMessage);
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Bad transfer message encountered: '{5}'.  Play has failed.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, TransferMessage);
                                                    }
                                                } 
                                            }

                                            threadEvent.Reset();
                                            // Request an Agent!
                                            Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Requesting a verification agent.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);

                                            AgentRequest.GetMeAnAgent(this); // This is where we get verification agent

                                            DateTime nextHoldMessage = DateTime.Now.AddSeconds(2.0);
                                            while (true)
                                            {
                                                if (m_TerminateCall)
                                                {
                                                    if (ManagedAgent == null) AgentRequest.CancelMyAgentRequest(this);
                                                    break;
                                                }

                                                if (m_NoAvailableAgents)
                                                {

                                                    if (m_VoiceResourceThread != null) ManagedChannel.ChannelResource.VoiceResource.Stop();
                                                    while (m_VoiceResourceThread != null) Thread.Sleep(100);
                                                    break;
                                                }

                                                // We got an agent!
                                                if (ManagedAgent != null) break;

                                                if (DateTime.Now > nextHoldMessage)
                                                {
                                                    if (m_TerminateCall)
                                                        break;

                                                    if (OtherParameter.HoldMessage != "")
                                                    {
                                                        holdMessage = OtherParameter.HoldMessage;
                                                    }

                                                    if (holdMessage != "")
                                                    {
                                                        if (m_VoiceResourceThread == null)
                                                        {
                                                            ThreadStart ts = new ThreadStart(PlayHoldMessage);
                                                            m_VoiceResourceThread = new Thread(ts);
                                                            m_VoiceResourceThread.Name = CallDetails.PhoneNum + ".PlayHold";
                                                            m_VoiceResourceThread.IsBackground = true;
                                                            m_VoiceResourceThread.Start();
                                                            nextHoldMessage = DateTime.Now.AddSeconds(2);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|No hold message defined.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                                                    }
                                                }

                                                threadEvent.WaitOne(1000, false);

                                                if (!DialerEngine.Connected) TerminateCall = true;

                                            }

                                            threadEvent.Reset();

                                            if (m_TerminateCall || m_NoAvailableAgents)
                                            {
                                                // If agent arrived after the call was hungup.  Make him available again.
                                                if (ManagedAgent != null)
                                                {
                                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Updating agent status, setting dialer to null.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                                                    ManagedAgent.Dialer = null;
                                                }
                                                break;
                                            }

                                            if (ManagedAgent == null)
                                            {
                                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Managed agent is null!", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                                            }
                                            else
                                            {

                                                //DateTime agentConnecttime = DateTime.Now;
                                                // Update Calllist with agentid this will show script to the agent

                                                // GW 11.29.10 out for talks being decided on disposition
                                                callDetails.AgentID = ManagedAgent.AgentDetails.AgentID.ToString();
                                                callDetails.AgentName = ManagedAgent.AgentDetails.AgentName;
                                                callDetails.VerificationAgentID = ManagedAgent.AgentDetails.VerificationAgent ? ManagedAgent.AgentDetails.AgentID.ToString() : "";

                                                AddVerificationAgentToCallDetails();

                                                if (m_VoiceResourceThread != null) ManagedChannel.ChannelResource.VoiceResource.Stop();
                                                while (m_VoiceResourceThread != null) Thread.Sleep(100);

                                                // Record the call switch added GW 10.19.10

                                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Second check determines recording switch set to {5}.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, ownerCampaign.RecordCalls);
                                                if (!m_TerminateCall && ownerCampaign.RecordCalls)
                                                {
                                                    ThreadStart ts = new ThreadStart(Record);
                                                    m_VoiceResourceThread = new Thread(ts);

                                                    string timestamp = DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss");
                                                    m_VoiceResourceThread.Name = CallDetails.PhoneNum + "_" + timestamp + "V.Record";
                                                    m_VoiceResourceThread.IsBackground = true;
                                                    m_VoiceResourceThread.Start();
                                                }

                                                while (true)
                                                {
                                                    // At this point Any Thread event is a disconnect...
                                                    if (m_TerminateCall) break;
                                                    if (threadEvent.WaitOne(1000, false)) break;
                                                    if (!DialerEngine.Connected) TerminateCall = true;
                                                }

                                                // Stop the record...
                                                try
                                                {
                                                    ManagedChannel.ChannelResource.VoiceResource.Stop();
                                                }
                                                catch { }

                                                ManagedAgent temp1 = ManagedAgent;
                                                if (temp1 != null)
                                                {
                                                    temp1.Dialer = null;
                                                }
                                            }

                                        }
                                        else if (OtherParameter.CallTransfer == (int)CallBackOptions.AllowOffsiteCallTransfer)
                                        {
                                            // Transfer to offsite number
                                            string offsiteNumberToDial = OtherParameter.StaticOffsiteNumber.Trim();
                                            if (offsiteNumberToDial != "" && offsiteNumberToDial.Length > 6)
                                            {
                                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|{5}|Transfer to static offsite number '{6}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, callDetails.UniqueKey, offsiteNumberToDial);
                                                // *** Set the static offsite number
                                                // dial the number and route him
                                                try
                                                {
                                                    callDetails.OffsiteTransferNumber = offsiteNumberToDial;
                                                    OffsiteDialer m_OffsiteDialer = new OffsiteDialer(managedAgent, ManagedChannel, campaign, callDetails);
                                                    m_OffsiteDialer.RunScript();
                                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Static offsite transfer complete.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                                                    // Call over
                                                    m_OffsiteDialer = null;
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (!(ex is System.Threading.ThreadAbortException))
                                                        Log.WriteException(ex, "Offsite transfer thread exception.");
                                                }
                                            }
                                            else
                                            {
                                                offsiteNumberToDial = CampaignAPI.GetOffsiteTransferNumber(campaign.CampaignDBConnString, callDetails);
                                                if (offsiteNumberToDial != "" && offsiteNumberToDial.Length > 6)
                                                {
                                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Transfer to dynamic offsite number '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, offsiteNumberToDial);

                                                    // dial the number and route him
                                                    try
                                                    {
                                                        callDetails.OffsiteTransferNumber = offsiteNumberToDial;
                                                        OffsiteDialer m_OffsiteDialer = new OffsiteDialer(managedAgent, ManagedChannel, campaign, callDetails);
                                                        m_OffsiteDialer.RunScript();
                                                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Dynamic offsite transfer complete.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);

                                                        // Call over
                                                        m_OffsiteDialer = null;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        if (!(ex is System.Threading.ThreadAbortException))
                                                            Log.WriteException(ex, "Offsite transfer thread exception.");
                                                    }
                                                }
                                                else
                                                {
                                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Transfer to dynamic offsite number '{5}' failed, invalid number.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, offsiteNumberToDial);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Transfer attempted, but not allowed for this campaign.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);

                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.WriteException(ex, "Transfer call exception.");
                                    }
                                }
                                #endregion

                                // Call finished
                                //Signalled, so Terminate the call
                                dtEndTime = DateTime.Now;

                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|{5}|Call complete.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, callDetails.UniqueKey);

                            }
                        }
                        break;

                    case DialResult.NoAnswer:
                        campQueryStatus.NoAnswer = 1;
                        break;

                    case DialResult.Busy:
                        campQueryStatus.Busy = 1;
                        break;

                    case DialResult.Error:
                    case DialResult.Failed:
                        campQueryStatus.Failed = 1;
                        //bScheduleIt = true;
                        //CallDetails.ScheduleDate = DateTime.Now.AddMinutes(DialParameter.ErrorRedialLapse);
                        //Log.WriteWithId(ManagedChannel.Id.ToString(), "Dial Result Error");
                        break;

                    case DialResult.OperatorIntercept:
                        campQueryStatus.OpInt = 1;
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Operator Intercept.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                        break;

                    default:
                        // call failed no answer
                        campQueryStatus.Failed = 1;
                        //bScheduleIt = true;
                        //CallDetails.ScheduleDate = DateTime.Now.AddMinutes(DialParameter.ErrorRedialLapse);
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Unhandled dial result '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, dr);
                        break;
                }

                // update call status to database
                if (bUpdCompletionStatus)
                {
                    //
                    try
                    {
                        System.TimeSpan callDuration = dtEndTime.Subtract(dtStartTime);
                        CallDetails.CallDuration = callDuration.TotalSeconds.ToString();
                        // Update Calllist (completion time)
                        CampaignAPI.UpdateCallCompletion(Campaign, CallDetails);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Call update Exception");
                    }
                }

                if (bScheduleIt)
                {
                    try
                    {
                        CampaignAPI.UpdateCampaignSchedule(Campaign, CallDetails);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Call schedule exception");
                    }
                }

                // Update query status counts
                // 09.12.10 - Moved to finally block due to hangups causing query stats to not be registered.
                // This may be the same needed for the available counts.  Test!


                // Update Available counts  - Moved to finally block 09.12.10

            }
            catch (ElementsException ee)
            {
                // These are Telephony Specific exceptions, such an the caller hanging up the phone during a play or record.
                if (ee is HangupException)
                {
                    // GW 09.29.10 - To allow greying of agent UI button on hangup and beep for hangup
                    CampaignAPI.SetCallHangup(CallDetails.UniqueKey, ManagedAgent.AgentDetails.CampaignDB);
                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Contact hung up.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                }
                else
                    Log.WriteException(ee, "Script Elements Exception");
            }
            catch (ThreadAbortException tex)
            {
                if (DialerEngine.Connected)
                {
                    Log.WriteException(tex, "ThreadAbortException");
                }
                else
                {
                    m_TerminateCall = true;
                }
            }
            catch (Exception ex)
            {
                // This would be a general logic exception, such as a null reference violation or other .NET exception.
                if (DialerEngine.Connected)
                {
                    Log.WriteException(ex, "Script General Exception");
                }
            }
            finally
            {
                // Update query status counts
                // 09.12.10 - Moved to finally block due to hangups causing query stats to not be registered.
                // This may be the same needed for the available counts.  Test!
                try
                {
                    CampaignAPI.UpdateCampaignQueryStats(Campaign, campQueryStatus);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "Query Status update error");
                }


                if (ManagedChannel != null)
                {

                    if (isDialing)
                    {
                        try { ManagedChannel.ChannelResource.StopDial(); }
                        catch { }
                        isDialing = false;
                    }
                    try
                    {
                        if (ManagedChannel.ChannelResource != null)
                        {
                            ManagedChannel.ChannelResource.Disconnect();
                            ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                            // Route to Home VR.
                            ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                        }
                    }
                    catch { }


                    // Add channel back to the list for later use
                    ManagedChannel.ReturnChannel(ManagedChannel, Campaign.CampaignID);
                    Thread myThread = Thread.CurrentThread;
                    ownerCampaign.RemoveCallThreadFromList(myThread.Name);
                }
            }
        }

        private string BuildDialString()
        {
            try
            {
                string dialString = callDetails.PhoneNum;

                switch (dialString.Length)
                {
                    case 7:
                        dialString = string.Format("{0}{1}{2}", dialParameter.SevenDigitPrefix.Trim(), callDetails.PhoneNum, dialParameter.SevenDigitSuffix.Trim());
                        break;
                    case 10:
                        dialString = string.Format("{0}{1}{2}", dialParameter.TenDigitPrefix.Trim(), callDetails.PhoneNum, dialParameter.TenDigitSuffix.Trim());
                        break;
                    default:
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Unhandled number length '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, dialString);
                        break;
                }
                return dialString;
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Exception building dial string.", ex.Message);
                return callDetails.PhoneNum;
            }
        } 
        #endregion

        #region Run Script - Unmanned
        public void RunScriptForUnmannedMode()
        {
            // Moved 09.12.10 to allow finally block to use object
            CampaignQueryStatus campQueryStatus = new CampaignQueryStatus();

            try
            {
                DateTime dtCallStartTime = DateTime.Now;

                // Get a channel for outbound usage from the server...
                ManagedChannel = ManagedChannel.GetOutboundChannel(Campaign.CampaignID);

                // Update Calllist (number of trials)
                CampaignAPI.UpdateCallDetails(Campaign, CallDetails, CallType, QueryId);

                if (ManagedChannel == null || ManagedChannel.ChannelResource == null)
                {
                    // Channels not available, prior checking also implemented this will not happen
                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|No channels available, thread has been started without a channel.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                    return;
                }

                // Suscribes to the disconnect event to let us know if the caller hangs up the phone.
                try
                {
                    ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                }
                catch { }
                ManagedChannel.ChannelResource.Disconnected += new Disconnected(ChannelResource_Disconnected);

                // Set outbound caller id
                ManagedChannel.ChannelResource.OriginatingPhoneNumber = this.campaign.OutboundCallerID;

                // Set messages variables
                MachineMessage = dialParameter.AnsweringMachineMessage;
                HumanMessage = dialParameter.HumanMessage;
                SilentCallMessage = dialParameter.SilentCallMessage;

                bool AnsweringMachineDetectionOn = false;
                try
                {
                    AnsweringMachineDetectionOn = dialParameter.AnsweringMachineDetection;
                    // Added by GW to truly disable detection for testing.
                    if (AnsweringMachineDetectionOn)
                    {
                        ManagedChannel.ChannelResource.CallProgress = CallProgress.AnalyzeCall;
                    }
                    else
                    {
                        ManagedChannel.ChannelResource.CallProgress = CallProgress.WaitForConnect;
                    }
                }
                catch { }

                //Make sure we are routed to the voice resource!
                if (ManagedChannel.ChannelResource is T1Channel)
                {
                    ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                }

                DialResult dr;

                // Added GW 11.07.10 - CPA overrides
                ManagedChannel.ChannelResource.CallProgressTemplate = @"Dialogic\DxCap";

                Dictionary<string, int> overrides = new Dictionary<string, int>();
                if (DialParameter.RingSeconds > 6)
                {
                    overrides.Add("ca_noanswer", ((DialParameter.RingSeconds - 6) * 100));
                }
                else
                {
                    overrides.Add("ca_noanswer", (DialParameter.RingSeconds * 100));
                }

                ManagedChannel.ChannelResource.CallProgressOverrides = overrides;

                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Dialing for {5} seconds on channel {6}.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, CallDetails.PhoneNum, DialParameter.RingSeconds, ManagedChannel.ChannelResource.DeviceName);
                
                string strDialString = BuildDialString();

                bool RecordCalls = false;
                DigitalizedRecording digRecording = CampaignAPI.GetDigitizedRecordings(campaign);
                if (digRecording != null)
                {

                    RecordCalls = digRecording.EnableRecording;
                }
                if (m_VoiceResourceThread != null) ManagedChannel.ChannelResource.VoiceResource.Stop();
                while (m_VoiceResourceThread != null) Thread.Sleep(100);

                Log.Write("|DLR|{0}|{1}|{2}|Checking call recording switch which is set to {3}.", campaign.CampaignID, campaign.ShortDescription, callDetails.PhoneNum, RecordCalls);

                if (RecordCalls)
                {
                    ThreadStart ts = new ThreadStart(Record);
                    m_VoiceResourceThread = new Thread(ts);

                    string timestamp = DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss");
                    m_VoiceResourceThread.Name = CallDetails.PhoneNum + "_" + timestamp + "U.Record";
                    m_VoiceResourceThread.IsBackground = true;
                    m_VoiceResourceThread.Start();
                }

                try
                {

                    lock (this) { isDialing = true; }

                    dr = ManagedChannel.ChannelResource.Dial(strDialString);

                    lock (this) { isDialing = false; }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "General dialing exception.");
                    dr = DialResult.Failed;
                }
                finally
                {
                    ManagedChannel.ChannelResource.CallProgress = CallProgress.WaitForConnect;


                    CampaignProcess.UpdateCampaignDialCount(Campaign.CampaignID, false);
                }

                // Log dial result
                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|The dial result was {5}.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, dr);

                bool bUpdCompletionStatus = false;
                DateTime dtStartTime = DateTime.Now;
                DateTime dtEndTime = DateTime.Now;

                // Moved 01.17.11 to allow finally block to use object
                //CampaignQueryStatus campQueryStatus = new CampaignQueryStatus();
                campQueryStatus.QueryID = this.QueryId;
                campQueryStatus.Dials = 1;

                // update result to call list table
                CurrentDialResult = dr;

                try
                {
                    // Update stats and add to calllist
                    callInterval = (int)(DateTime.Now.Subtract(dtCallStartTime).TotalSeconds);
                    answeredCall = (dr == DialResult.Connected || dr == DialResult.HumanDetected
                                    || dr == DialResult.Successful);

                    m_UpdateStatsThread = new Thread(new ThreadStart(UpdateStats));
                    m_UpdateStatsThread.Name = callDetails.PhoneNum + "_Stats";
                    m_UpdateStatsThread.IsBackground = true;
                    m_UpdateStatsThread.Start();
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "Runscript exception spawning stats thread.");
                }

                switch (dr)
                {
                    case DialResult.Connected:
                    case DialResult.HumanDetected:
                    case DialResult.Successful:
                    case DialResult.MachineDetected:
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|System has detected an answer, machine detection set to {5}.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, AnsweringMachineDetectionOn);
                        bUpdCompletionStatus = true;
                        if (dr == DialResult.MachineDetected && AnsweringMachineDetectionOn)
                        {
                            campQueryStatus.AnsweringMachine = 1;

                            // Hang up on machines if we didn't specify a message for machines.
                            if (MachineMessage == null)
                            {
                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Answering machine detection is on, but no message specified.  Call will be terminated without leaving a message.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                                break;
                            }

                            // Set the MaximumSilence to wait for 3 seconds of silence.  This value is set in deciSeconds (1/10th of a second).
                            ManagedChannel.ChannelResource.VoiceResource.MaximumSilence = 30;

                            // Also set the Maximum Overall Time to wait for the 3 seconds.  (If greeting is longer than 60 seconds, just play the message).
                            // MaximumTime is specified in deciSeconds (1/10th of a second).
                            ManagedChannel.ChannelResource.VoiceResource.MaximumTime = 600;
                            ManagedChannel.ChannelResource.VoiceResource.GetSilence();

                            // Forec the play to run to completion
                            ManagedChannel.ChannelResource.VoiceResource.TerminationDigits = "";

                            if (MachineMessage != null && MachineMessage != "")
                            {
                                // Play the file.
                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Playing answering machine file '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, MachineMessage);
                                try
                                {
                                    ManagedChannel.ChannelResource.VoiceResource.Play(MachineMessage);
                                }
                                catch (Exception ex)
                                {
                                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Bad answering machine file encountered: '{5}'. Play failed", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, MachineMessage);
                                }

                            }
                            else
                            {
                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Answering machine message not defined or invalid, no message was sent.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                            }
                        }
                        else
                        {
                            // Hang up on calls if we didn't specify a message for Human detection.
                            if (HumanMessage == null || HumanMessage == "")
                            {
                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Live contact (human) message not defined or invalid, no message was sent.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                                break;
                            }

                            ManagedChannel.ChannelResource.VoiceResource.TerminationDigits = "";
                            Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Playing live contact file '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, HumanMessage);
                            try
                            {
                                 ManagedChannel.ChannelResource.VoiceResource.Play(HumanMessage);
                            }
                            catch (Exception ex)
                            {
                                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Bad live contact file encountered: '{5}'. Play failed.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, HumanMessage);
                            }
                        }
                        dtEndTime = DateTime.Now;
                        break;

                    case DialResult.NoAnswer:
                        campQueryStatus.NoAnswer = 1;
                        break;

                    case DialResult.Busy:
                        campQueryStatus.Busy = 1;
                        break;

                    case DialResult.Error:
                    case DialResult.Failed:
                        campQueryStatus.Failed = 1;
                        break;

                    case DialResult.OperatorIntercept:
                        campQueryStatus.OpInt = 1;
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Operator Intercept.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                        break;

                    default:
                        // call failed no answer
                        campQueryStatus.Failed = 1;
                        //bScheduleIt = true;
                        //CallDetails.ScheduleDate = DateTime.Now.AddMinutes(DialParameter.ErrorRedialLapse);
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Unhandled dial result '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, dr);
                        break;
                }

                // Stop the record...
                try
                {
                    ManagedChannel.ChannelResource.VoiceResource.Stop();
                }
                catch { }
                if (bUpdCompletionStatus)
                {
                    try
                    {
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Updating call completion.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                        System.TimeSpan callDuration = dtEndTime.Subtract(dtStartTime);
                        CallDetails.CallDuration = callDuration.TotalSeconds.ToString();
                        // Update Calllist (completion time)
                        CampaignAPI.UpdateCallCompletion(Campaign, CallDetails);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Call update Exception");
                    }
                }

            }
            catch (ElementsException ee)
            {
                // These are Telephony Specific exceptions, such an the caller hanging up the phone during a play or record.
                if (ee is HangupException)
                {
                    // GW 09.29.10 - To allow greying of agent UI button on hangup, added beep
                    //CampaignAPI.SetCallHangup(CallDetails.UniqueKey, ManagedAgent.AgentDetails.CampaignDB);
                    try
                    {
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Contact hung up.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);
                    }
                    catch
                    { }
                }
                else
                    Log.WriteException(ee, "Script telephony exception.");
            }
            catch (ThreadAbortException tex)
            {
                if (DialerEngine.Connected)
                {
                    Log.WriteException(tex, "Thread Abort Exception");
                }
                else
                {
                    m_TerminateCall = true;
                }
            }
            catch (Exception ex)
            {
                // This would be a general logic exception, such as a null reference violation or other .NET exception.
                if (DialerEngine.Connected)
                {
                    Log.WriteException(ex, "Script General Exception");
                }
            }
            finally
            {
                // Update query status counts
                // 09.12.10 - Moved to finally block due to hangups causing query stats to not be registered.
                // This may be the same needed for the available counts.
                try
                {
                    CampaignAPI.UpdateCampaignQueryStats(Campaign, campQueryStatus);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "Query Status update error");
                }

                if (ManagedChannel != null)
                {

                    if (isDialing)
                    {
                        try { ManagedChannel.ChannelResource.StopDial(); }
                        catch { }
                        isDialing = false;
                    }
                    try
                    {
                        if (ManagedChannel.ChannelResource != null)
                        {
                            ManagedChannel.ChannelResource.Disconnect();        //JMC
                            ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                            // Route to Home VR.
                            ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                        }
                    }
                    catch { }

                    try
                    {
                        // Add channel back to the list for later use
                        ManagedChannel.ReturnChannel(ManagedChannel, Campaign.CampaignID);
                        Thread myThread = Thread.CurrentThread;
                        ownerCampaign.RemoveCallThreadFromList(myThread.Name);
                    }
                    catch { }
                }
            }
        } 
        #endregion

        #region Private Methods

        private Thread m_VoiceResourceThread = null;

        private void PlayWaitMessage()
        {
            try
            {
                // Play wait message
                //Log.Write("Playing Customer wait Message");
                if (!m_TerminateCall && ManagedAgent == null)
                {
                    if (HumanMessage != "")
                    {
                        // Play the file.
                        ManagedChannel.ChannelResource.VoiceResource.TerminationDigits = "";
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Playing live contact wait file '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, HumanMessage);    
                        try
                        {
                            ManagedChannel.ChannelResource.VoiceResource.Play(HumanMessage);
                        }
                        catch (Exception ex)
                        {
                            Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Bad live contact file encountered: '{5}'. Play failed.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, HumanMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "PlayWaitMessage Exception");
            }
            finally
            {
                m_VoiceResourceThread = null;
            }

        }

        private void PlayTransferMessage()
        {
            try
            {
                // Play wait message
                //Log.Write("Playing Customer wait Message");
                if (!m_TerminateCall && ManagedAgent == null)
                {
                    if (TransferMessage != "")
                    {
                        // Play the file.
                        ManagedChannel.ChannelResource.VoiceResource.TerminationDigits = "";
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Playing transfer file '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, TransferMessage);   
                        try
                        {
                            ManagedChannel.ChannelResource.VoiceResource.Play(TransferMessage);
                        }
                        catch (Exception ex)
                        {
                            Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Bad transfer message encountered: '{5}'.  Play has failed.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, TransferMessage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "PlayTransferMessage Exception");
            }
            finally
            {
                m_VoiceResourceThread = null;
            }

        }

        private void PlayHoldMessage()
        {
            try
            {
                // Play wait message
                //Log.Write("Playing Customer wait Message");
                if (!m_TerminateCall && ManagedAgent == null)
                {
                    if (HoldMessage != "")
                    {
                        // Play the file.
                        ManagedChannel.ChannelResource.VoiceResource.TerminationDigits = "";
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Playing hold message file '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, HoldMessage);
                        try
                        {
                            ManagedChannel.ChannelResource.VoiceResource.Play(HoldMessage);
                        }
                        catch
                        {
                            Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Bad hold message file encountered: '{5}'. Play failed.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, HoldMessage);    
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "PlayHoldMessage Exception");
            }
            finally
            {
                m_VoiceResourceThread = null;
            }

        }

        private void Record()
        {
            try
            {
                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Beginning record.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);   
                // file format : agentid_date_campaignid_calluniqueid
                string sFileName = string.Format("{0}_{1}.wav", DateTime.Now.ToString("MMddyyyy"), CallDetails.PhoneNum);
                if (RecordingsPath == string.Empty)
                {
                    string sDefaultPath = Utilities.GetAppSetting("DefaultRecordingsPath", @"C:\recordings\");
                    RecordingsPath = Utilities.CombinePaths(sDefaultPath, campaign.ShortDescription);
                }
                try
                {
                    Utilities.CreateDirectory(RecordingsPath);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "Recordings Path creation failed");
                }

                if (!m_TerminateCall)
                {

                    ManagedChannel.ChannelResource.VoiceResource.MaximumTime = 600;     // Ten Minutes
                    ManagedChannel.ChannelResource.VoiceResource.TerminationDigits = "";
                    ManagedChannel.ChannelResource.VoiceResource.WipeDigitBuffer();

                    //if (m_VoiceResourceThread != null) ManagedChannel.ChannelResource.VoiceResource.Stop();
                    //while (m_VoiceResourceThread != null) Thread.Sleep(100);
                    if (ManagedChannel.ChannelResource != null && ManagedAgent.ManagedChannel.ChannelResource != null)
                    {
                        if (RecordBeep)
                        {
                            // GW playtone for beep on recording - playing both to emulate amcat, Impact to specify if change.
                            ManagedAgent.PlayBeep();
                        }
                        TerminationCode tc = ManagedChannel.ChannelResource.VoiceResource.RecordConverstation(
                            Utilities.CombinePaths(RecordingsPath, sFileName),
                            ManagedChannel.ChannelResource, ManagedAgent.ManagedChannel.ChannelResource);
                        Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Record complete, termination code '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, tc);
                    }
                    else
                    {
                        Log.Write("Record Error. Channels are null check why this happend");
                    }
                }
                else
                {
                    Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Record error, call terminated before recording began.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);   
                }

            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Record Exception");
            }
            finally
            {
                m_VoiceResourceThread = null;
            }
        }

        private Thread m_UpdateStatsThread = null;
        private void UpdateStats()
        {
            try
            {
                //Log.Write("Updating stats");
                CampStats.AddToACTList(CallInterval);
                CampStats.AddToAnswerList(AnsweredCall);

                //Log.Write("Updating result to call list");
                UpdateResultToCalllist("");
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "UpdateStats Exception");
            }
            finally
            {
                m_UpdateStatsThread = null;
            }
        }

        private void UpdateResultToCalllist(string ResultDesc)
        {
            switch (CurrentDialResult)
            {
                case DialResult.Connected:
                case DialResult.HumanDetected:
                case DialResult.Successful:
                    if (dialParameter.DialingMode == Convert.ToInt32(DialingMode.Unmanned))
                        ResultDesc = "Unmanned Live Contact";
                    if (IsTransferedCall) ResultDesc = "Transferred to Agent";
                    break;
                case DialResult.MachineDetected:
                    if (dialParameter.DialingMode == Convert.ToInt32(DialingMode.Unmanned))
                        ResultDesc = "Unmanned Transferred To Answering Machine";
                    else ResultDesc = "Answering Machine";
                    break;
                case DialResult.NoAnswer:
                    ResultDesc = "No Answer";
                    break;
                case DialResult.Busy:
                    ResultDesc = "Busy";
                    break;
                //case DialResult.Error:
                //    ResultDesc = "Error";
                //    break;
                //case DialResult.Failed:
                //    ResultDesc = "Failed";
                //    break;
                //case DialResult.CadenceBreak:
                //    ResultDesc = "Cadence Break";
                //    break;
                //case DialResult.LoopCurrentDrop:
                //    ResultDesc = "Loop Current Drop";
                //    break;
                //case DialResult.PbxDetected:
                //    ResultDesc = "Pbx Detected";
                //    break;
                //case DialResult.NoRingback:
                //    ResultDesc = "No Ringback";
                //    break;
                //case DialResult.AnalysisStopped:
                //    ResultDesc = "Analysis Stopped";
                //    break;
                //case DialResult.NoDialTone:
                //    ResultDesc = "No DialTone";
                //    break;
                //case DialResult.FaxToneDetected:
                //    ResultDesc = "FaxTone Detected";
                //    break;
                case DialResult.OperatorIntercept:
                    ResultDesc = "Operator Intercept";
                    break;
                default:
                    ResultDesc = "Failed";
                    break;
            }
            if (ResultDesc != "")
            {
                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Updating call result to call list '{5}'.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum, ResultDesc);
                CampaignAPI.AddResultCodeToCallList(Campaign.CampaignDBConnString, CallDetails, ResultDesc);
            }
        }

        //private Thread m_AddAgentToCallDetailsThread = null;
        private void AddAgentToCallDetails(bool isVerification)
        {
            try
            {
                //Log.Write("Add Agent to Calldetails DB");
                CampaignAPI.AddAgentToCallDetail(Campaign, CallDetails, isVerification);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "AddAgentToCallDeatals Exception");
            }
            /* finally
            {
                m_AddAgentToCallDetailsThread = null;
            }*/
            //Log.Write("Add Agent to Calldetails DB End");
        }

        private void AddVerificationAgentToCallDetails()
        {
            try
            {
                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Add verification agent to call details.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);   
                CampaignAPI.AddVerificationAgentToCallDetail(Campaign, CallDetails);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "AddAgentToCallDetails Exception");
            }
            /*finally
            {
                m_AddAgentToCallDetailsThread = null;
            }*/
            //Log.Write("Add Agent to Calldetails DB End");
        } 
        #endregion
 
        #region Events

        /// <summary>
        /// The Disconnected event processing code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChannelResource_Disconnected(object sender, DisconnectedEventArgs e)
        {
            try
            {
                TerminateCall = true;

                if (isDialing)
                {
                    try { ManagedChannel.ChannelResource.StopDial(); }
                    catch { }
                    isDialing = false;
                }

                // Here we will simply write to the log that the caller hung up the phone.
                Log.Write("|DLR|{0}|{1}|{2}|{3}|{4}|Channel disconnect event received.", campaign.CampaignID, campaign.ShortDescription, query.QueryID, query.QueryName, callDetails.PhoneNum);   
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "ChannelResource_Disconnected Exception");
            }
        }
        #endregion
    }

    public class ManualDialer
    {
        #region Variables and Properties
        // References the main Log File created at startup
        private static Log Log = DialerEngine.Log;

        // a reference to current campaign's dialingparameted
        private DialingParameter dialParameter;
        public DialingParameter DialParameter
        {
            get { return dialParameter; }
            set { dialParameter = value; }
        }

        // Reference to the managed agent
        private ManagedAgent managedAgent = null;
        public ManagedAgent ManagedAgent
        {
            get { return managedAgent; }
            set
            {
                lock (this)
                {
                    if (m_TerminateCall) throw new Exception("Agent No Longer Needed.");
                    managedAgent = value;
                }
                threadEvent.Set();
            }
        }

        // a reference to the ManagedChannel the call arrived on
        private ManagedChannel managedChannel = null;
        public ManagedChannel ManagedChannel
        {
            get { return managedChannel; }
            set { managedChannel = value; }
        }

        public AutoResetEvent threadEvent = new AutoResetEvent(false);

        private bool m_TerminateCall = false;

        public bool TerminateCall
        {
            get { return m_TerminateCall; }
            set
            {
                lock (this)
                {
                    m_TerminateCall = value;
                }
                threadEvent.Set();
            }
        }

        private bool isDialing = false;
        private long m_AgentID = 0;
        private string m_AgentName = "";

        // Reference to RecordingsPath
        private string recordingsPath = string.Empty;
        public string RecordingsPath
        {
            get { return recordingsPath; }
            set { recordingsPath = value; }
        }
        private bool recordCalls = false;
        public bool RecordCalls
        {
            get { return recordCalls; }
            set { recordCalls = value; }
        }

        // Reference to record calls switch GW 09.28.10
        private bool recordBeep = false;
        public bool RecordBeep
        {
            get { return recordBeep; }
            set { recordBeep = value; }
        }


        // a reference to current call details  
        private CampaignDetails callDetails;
        public CampaignDetails CallDetails
        {
            get { return callDetails; }
            set { callDetails = value; }
        }

        public DialResult CurrentDialResult; 
        #endregion

        #region Constructor
        /// <summary>
        /// Constuctor, assigns managed agent and call details.
        /// </summary>
        /// <param name="campaign"></param>
        public ManualDialer(ManagedAgent managedAgent, CampaignDetails callDetails)
        {

           
            this.managedAgent = managedAgent;
            this.callDetails = callDetails;
            this.m_AgentName = managedAgent.AgentDetails.AgentName;
            this.m_AgentID = managedAgent.AgentDetails.AgentID;
            
           
        } 
        #endregion

        #region Run Main Dialing Script
        /// <summary>
        /// The main script, Dials the number and routes the call to an agent
        /// </summary>
        public void RunScript()
        {
            try
            {
                Campaign objCampaign = new Campaign();
                objCampaign.CampaignID = ManagedAgent.AgentDetails.CampaignID;
                objCampaign.CampaignDBConnString = ManagedAgent.AgentDetails.CampaignDB;

                // Update Calllist (number of trials)
                CampaignAPI.UpdateCallDetails(objCampaign, CallDetails, CallType.PMCall, 0);

                // Get a channel for outbound usage from the server...
                ManagedChannel = ManagedChannel.GetOutboundChannel(ManagedAgent.AgentDetails.CampaignID);

                if (ManagedChannel == null || ManagedChannel.ChannelResource == null)
                {
                    Log.Write("|MD|{0}|{1}|{2}|{3}|No channels available, thread has been started without a channel.", objCampaign.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum);
                    return;
                }

                // Suscribes to the disconnect event to let us know if the caller hangs up the phone.
                try
                {
                    ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                }
                catch { }
                ManagedChannel.ChannelResource.Disconnected += new Disconnected(ChannelResource_Disconnected);

                Log.Write("|MD|{0}|{1}|{2}|{3}|Dialing on channel {4}.", objCampaign.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum, ManagedChannel.ChannelResource.DeviceName);
                // Dial to number

                // You should set the outbound caller ID of the Campaign here!
                ManagedChannel.ChannelResource.OriginatingPhoneNumber = this.ManagedAgent.AgentDetails.OutboundCallerID;

                 try
                {
                    if (this.ManagedAgent.ManagedChannel.ChannelResource is MsiChannel)
                    {
                        this.ManagedAgent.ManagedChannel.ChannelResource.StopListening();
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "StopListening error");
                }
                 ManagedChannel.ChannelResource.CallProgress = CallProgress.DialOnly; // If CPA needed, add here

                try
                {

                    //Make sure we are routed to the voice resource!
                    if (ManagedChannel.ChannelResource is T1Channel)
                    {
                        ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                    }

                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "ChannelResource T1Channel error");
                }

                // Route early so we can listen to the dialing...
                // ManagedAgent.ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource);
                try
                {
                ManagedAgent.ManagedChannel.ChannelResource.RouteHalf(ManagedChannel.ChannelResource);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "ChannelResource RouteHalf error");
                }


                DialResult dr;

                /* ManagedChannel.ChannelResource.CallProgressTemplate = @"Dialogic\DxCap";
                Dictionary<string, int> overrides = new Dictionary<string, int>();
                overrides.Add("ca_noanswer", 3600);
                ManagedChannel.ChannelResource.CallProgressOverrides = overrides;
                */

                //ManagedChannel.ChannelResource.CallProgressTemplate = @"Dialogic\DxCap";

                
                //string strDialString = BuildDialString();

                lock (this) { isDialing = true; }
                
                
                    dr = ManagedChannel.ChannelResource.Dial(CallDetails.PhoneNum);
                
               
                lock (this) { isDialing = false; }
                
                 try
                {
                ManagedAgent.ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource);
                }
                 catch (Exception ex)
                 {
                     Log.WriteException(ex, "ChannelResource RouteFull error");
                 }
                
                // Log dial result
                Log.Write("|MD|{0}|{1}|{2}|{3}|Dial result '{4}'.", objCampaign.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum, dr);

                bool bUpdCompletionStatus = false;
                //bool bScheduleIt = false;
                DateTime dtStartTime = DateTime.Now;
                DateTime dtEndTime = DateTime.Now;

                switch (dr)
                {
                    case DialResult.Connected:
                    case DialResult.HumanDetected:
                    case DialResult.Successful:
                        Log.Write("|MD|{0}|{1}|{2}|{3}|Routed agent station {4} to {5}.", objCampaign.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum,
                            ManagedAgent.AgentDetails.StationNumber, ManagedChannel.ChannelResource.DeviceName);
                        bUpdCompletionStatus = true;
                        //DateTime agentConnecttime = DateTime.Now;

                        //PlayConnectingCustomer();
                        bool RecordCalls = false;
                        DigitalizedRecording digRecording = CampaignAPI.GetDigitizedRecordings(objCampaign);
                        if (digRecording != null)
                        {
                            
                            RecordCalls = digRecording.EnableRecording;
                        }
                        if (m_VoiceResourceThread != null) ManagedChannel.ChannelResource.VoiceResource.Stop();
                        while (m_VoiceResourceThread != null) Thread.Sleep(100);

                        Log.Write("|DLR|{0}|{1}|{2}|Checking call recording switch which is set to {3}.", objCampaign.CampaignID, ManagedAgent.AgentDetails.ShortDescription, callDetails.PhoneNum, RecordCalls);

                        if (RecordCalls)
                        {
                            ThreadStart ts = new ThreadStart(Record);
                            m_VoiceResourceThread = new Thread(ts);

                            string timestamp = DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss");
                            m_VoiceResourceThread.Name = CallDetails.PhoneNum + "_" + timestamp + "M.Record";
                            m_VoiceResourceThread.IsBackground = true;
                            m_VoiceResourceThread.Start();
                        }

                        Thread t = new Thread(new ThreadStart(HangupCheck));
                        t.IsBackground = true;
                        t.Start();
                        threadEvent.WaitOne();

                        // Call finished
                        dtEndTime = DateTime.Now;
                        // *** Route to self to break route ... bug heard ringing 04.14.11 - May be putting agent back into pool somehow. 08/6/11
                        ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource);
                        Log.Write("|MD|{0}|{1}|{2}|{3}|Call complete.", objCampaign.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum);

                        break;

                    case DialResult.MachineDetected:
                        Log.Write("|MD|{0}|{1}|{2}|{3}|Machine detected, call complete.", objCampaign.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum);
                        break;

                    case DialResult.NoAnswer:
                        Log.Write("|MD|{0}|{1}|{2}|{3}|No answer, call complete.", objCampaign.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum);
                        break;

                    case DialResult.Busy:
                        Log.Write("|MD|{0}|{1}|{2}|{3}|Busy, call complete.", objCampaign.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum);
                        break;

                    case DialResult.Error:
                    case DialResult.Failed:
                        Log.Write("|MD|{0}|{1}|{2}|{3}|Failed or error, call complete.", objCampaign.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum);
                        break;

                    case DialResult.OperatorIntercept:
                        Log.Write("|MD|{0}|{1}|{2}|{3}|Operator Intercept, call complete.", objCampaign.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum);
                        break;

                    default:
                        // call failed no answer
                        Log.Write("|MD|{0}|{1}|{2}|{3}|Unexpected reault '{4}', call complete.", objCampaign.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum, dr);
                        break;
                }

                CurrentDialResult = dr;
                //Log.Write("Updating result to call list");
                UpdateResultToCalllist("");
                // Stop the record...
                try
                {
                    ManagedChannel.ChannelResource.VoiceResource.Stop();
                }
                catch { }
                // update call status to database
                if (bUpdCompletionStatus)
                {
                    //
                    try
                    {
                        System.TimeSpan callDuration = dtEndTime.Subtract(dtStartTime);
                        CallDetails.CallDuration = callDuration.TotalSeconds.ToString();
                        // Update Calllist (completion time)
                        CampaignAPI.UpdateCallCompletion(objCampaign, CallDetails);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Call update Exception");
                    }
                }
            }

            catch (ElementsException ee)
            {
                // These are Telephony Specific exceptions, such an the caller hanging up the phone during a play or record.
                if (ee is HangupException)
                {
                    // GW 09.29.10 - To allow greying of agent UI button on hangup, added beep
                    CampaignAPI.SetCallHangup(CallDetails.UniqueKey, ManagedAgent.AgentDetails.CampaignDB);
                    Log.Write("|MD|{0}|{1}|{2}|{3}|Call recipient hung up.", this.managedAgent.CampaignId, m_AgentID, m_AgentName, callDetails.PhoneNum);
                }
                else
                    Log.WriteException(ee, "Script Elements Exception");
            }
            catch (ThreadAbortException tex)
            {
                if (DialerEngine.Connected)
                {
                    Log.WriteException(tex, "ThreadAbortException");
                }
                else
                {
                    m_TerminateCall = true;
                }
            }
            catch (Exception ex)
            {
                // This would be a general logic exception, such as a null reference violation or other .NET exception.
                if (DialerEngine.Connected)
                {
                    Log.WriteException(ex, "Script General Exception");
                }
            }
            finally
            {

                if (m_VoiceResourceThread != null) ManagedChannel.ChannelResource.VoiceResource.Stop();
                while (m_VoiceResourceThread != null) Thread.Sleep(100);

                if (ManagedAgent.ManagedChannel != null)
                {
                    if (ManagedAgent.ManagedChannel.ChannelResource is MsiChannel)
                    {
                        Log.Write("|MD|{0}|{1}|{2}|Agent channel has stopped listening.", m_AgentID, m_AgentName, callDetails.PhoneNum);

                        ManagedAgent.ManagedChannel.ChannelResource.StopListening();
                    }
                    else
                    {
                        ManagedAgent.ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                    }
                }

                if (ManagedChannel != null)
                {

                    if (isDialing)
                    {
                        try { ManagedChannel.ChannelResource.StopDial(); }
                        catch { }
                        isDialing = false;
                    }

                    try
                    {
                        if (ManagedChannel.ChannelResource != null)
                        {

                            ManagedChannel.ChannelResource.Disconnect();

                            ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                            // Route to Home VR.
                            ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                        }
                    }
                    catch { }

                    // Add channel back to the list for later use
                    ManagedChannel.ReturnChannel(ManagedChannel, ManagedAgent.AgentDetails.CampaignID);
                }
            }
        } 
        #endregion

        #region Private Methods

        private Thread m_VoiceResourceThread = null;

        private void Record()
        {

            
            try
            {
                Log.Write("|MD|{0}|{1}|{2}|Beginning record.", this.ManagedAgent.AgentDetails.CampaignID, this.managedAgent.AgentDetails.ShortDescription, callDetails.PhoneNum);
                // file format : agentid_date_campaignid_calluniqueid
                string sFileName = string.Format("{0}_{1}_{2}_{3}_M.wav", this.ManagedAgent.AgentDetails.CampaignID, this.ManagedAgent.AgentDetails.AgentID, DateTime.Now.ToString("MM-dd-yyyy_HH-mm-ss"), CallDetails.PhoneNum);
                if (RecordingsPath == string.Empty)
                {
                    string sDefaultPath = Utilities.GetAppSetting("DefaultRecordingsPath", @"C:\recordings\");
                    RecordingsPath = Utilities.CombinePaths(sDefaultPath, this.managedAgent.AgentDetails.ShortDescription);
                }
                try
                {
                    Utilities.CreateDirectory(RecordingsPath);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "Recordings Path creation failed");
                }

                if (!m_TerminateCall)
                {

                    ManagedChannel.ChannelResource.VoiceResource.MaximumTime = 600;     // Ten Minutes
                    ManagedChannel.ChannelResource.VoiceResource.TerminationDigits = "";
                    ManagedChannel.ChannelResource.VoiceResource.WipeDigitBuffer();

                    //if (m_VoiceResourceThread != null) ManagedChannel.ChannelResource.VoiceResource.Stop();
                    //while (m_VoiceResourceThread != null) Thread.Sleep(100);
                    if (ManagedChannel.ChannelResource != null && ManagedAgent.ManagedChannel.ChannelResource != null)
                    {
                        if (RecordBeep)
                        {
                            // GW playtone for beep on recording - playing both to emulate amcat, Impact to specify if change.
                            ManagedAgent.PlayBeep();
                        }
                        TerminationCode tc = ManagedChannel.ChannelResource.VoiceResource.RecordConverstation(
                            Utilities.CombinePaths(RecordingsPath, sFileName),
                            ManagedChannel.ChannelResource, ManagedAgent.ManagedChannel.ChannelResource);
                        Log.Write("|DLR|{0}|{1}|{2}|Record complete, termination code '{3}'.", this.ManagedAgent.AgentDetails.CampaignID, this.ManagedAgent.AgentDetails.ShortDescription, callDetails.PhoneNum, tc);
                    }
                    else
                    {
                        Log.Write("Record Error. Channels are null check why this happend");
                    }
                }
                else
                {
                    Log.Write("|DLR|{0}|{1}|{2}|Record error, call terminated before recording began.", this.ManagedAgent.AgentDetails.CampaignID, this.ManagedAgent.AgentDetails.ShortDescription, callDetails.PhoneNum);
                }

            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Record Exception");
            }
            finally
            {
                m_VoiceResourceThread = null;
            }
        }

        public void HangupCheck()
        {
            try
            {
                // *** Speed of hangup GW 09.29.10 
                DateTime nextCheck = DateTime.Now.AddSeconds(1);
                while (!TerminateCall)
                {
                    if (DateTime.Now > nextCheck)
                    {
                        bool isHangup = CampaignAPI.IsCallHangup(CallDetails.UniqueKey, ManagedAgent.AgentDetails.CampaignDB);
                        if (isHangup)
                        {
                            try
                            {
                                TerminateCall = true;
                                Log.Write("|MD|{0}|{1}|{2}|{3}|Call hang up from agent interface.", managedAgent.AgentDetails.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum);
                            }
                            catch { }
                            break;
                        }
                        // GW changed for hangup speed detection 09.29.10
                        // nextCheck = DateTime.Now.AddSeconds(1.0);
                        nextCheck = DateTime.Now.AddMilliseconds(300);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Call Hangup check");
            }
        }



        private void UpdateResultToCalllist(string ResultDesc)
        {
            switch (CurrentDialResult)
            {
                case DialResult.Connected:
                case DialResult.HumanDetected:
                case DialResult.Successful:
                    break;
                case DialResult.MachineDetected:
                    ResultDesc = "Answering Machine";
                    break;
                case DialResult.NoAnswer:
                    ResultDesc = "No Answer";
                    break;
                case DialResult.Busy:
                    ResultDesc = "Busy";
                    break;
                case DialResult.OperatorIntercept:
                    ResultDesc = "Operator Intercept";
                    break;
                default:
                    ResultDesc = "Failed";
                    break;
            }
            if (ResultDesc != "")
            {
                Log.Write("|MD|{0}|{1}|{2}|{3}|Updating call result to call list to '{4}'.", managedAgent.AgentDetails.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum, ResultDesc);
                CampaignAPI.AddResultCodeToCallList(ManagedAgent.AgentDetails.CampaignDB, CallDetails, ResultDesc);
            }
        } 
        #endregion

        #region Events
        /// <summary>
        /// The Disconnected event processing code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChannelResource_Disconnected(object sender, DisconnectedEventArgs e)
        {
            try
            {
                TerminateCall = true;

                if (isDialing)
                {
                    try { ManagedChannel.ChannelResource.StopDial(); }
                    catch { }
                    isDialing = false;
                }

                // Here we will simply write to the log that the caller hung up the phone.
                Log.Write("|MD|{0}|{1}|{2}|{3}|Channel disconnect event received.", managedAgent.AgentDetails.CampaignID, m_AgentID, m_AgentName, callDetails.PhoneNum);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "ChannelResource_Disconnected Exception");
            }
        } 
        #endregion
    }

    public class OffsiteDialer
    {
        #region Variables and Properties
        // References the main Log File created at startup
        private static Log Log = DialerEngine.Log;

        private ManagedAgent managedAgent = null;
        public ManagedAgent ManagedAgent
        {
            get { return managedAgent; }
            set { managedAgent = value; }
            //set
            //{
            //    lock (this)
            //    {
            //        if (m_TerminateCall) throw new Exception("Agent No Longer Needed.");
            //        managedAgent = value;
            //    }
            //    threadEvent.Set();
            //}
        }

        // a reference to the ManagedChannel the call arrived on
        private ManagedChannel managedChannel = null;
        public ManagedChannel ManagedChannel
        {
            get { return managedChannel; }
            set { managedChannel = value; }
        }

        private ManagedChannel transferredManagedChannel = null;
        public ManagedChannel TransferredManagedChannel
        {
            get { return transferredManagedChannel; }
            set { transferredManagedChannel = value; }
        }

        public AutoResetEvent threadEvent = new AutoResetEvent(false);

        private bool m_TerminateCall = false;

        public bool TerminateCall
        {
            get { return m_TerminateCall; }
            set
            {
                lock (this)
                {
                    m_TerminateCall = value;
                }
                threadEvent.Set();
            }
        }

        private bool isDialing = false;
        private long m_AgentID = 0;
        private string m_AgentName = "";

        // a reference to current call details  
        private CampaignDetails callDetails;
        public CampaignDetails CallDetails
        {
            get { return callDetails; }
            set { callDetails = value; }
        }

        // a reference to current campaign  
        private Campaign campaign;
        public Campaign Campaign
        {
            get { return campaign; }
            set { campaign = value; }
        }

        public DialResult CurrentDialResult; 
        #endregion

        #region Constructor
        /// <summary>
        /// Constuctor
        /// </summary>
        /// <param name="campaign"></param>
        public OffsiteDialer(ManagedAgent managedAgent, ManagedChannel transferredChannel, Campaign campaign, CampaignDetails callDetails)
        {
            this.transferredManagedChannel = transferredChannel;
            this.managedAgent = managedAgent;
            this.m_AgentName = managedAgent.AgentDetails.AgentName;
            this.m_AgentID = managedAgent.AgentDetails.AgentID;
            this.callDetails = callDetails;
            this.campaign = campaign;
        } 
        #endregion

        #region Run Dial Script
        /// <summary>
        /// The main script, Dials the number and routes the call to an agent
        /// </summary>
        public void RunScript()
        {
            try
            {
                //Route the transferred channel to itself to break away from agent
                transferredManagedChannel.ChannelResource.RouteFull(transferredManagedChannel.ChannelResource);

                // Get a channel for outbound usage from the server...
                ManagedChannel = ManagedChannel.GetOutboundChannel(campaign.CampaignID);

                if (ManagedChannel == null || ManagedChannel.ChannelResource == null)
                {
                    Log.Write("|OD|{0}|{1}|{2}|{3}|{4}|No channels available, thread has been started without a channel.", campaign.CampaignID, campaign.ShortDescription, m_AgentID, m_AgentName, callDetails.PhoneNum);
                    return;
                }

                // Suscribes to the disconnect event to let us know if the caller hangs up the phone.
                try
                {
                    ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                }
                catch { }
                ManagedChannel.ChannelResource.Disconnected += new Disconnected(ChannelResource_Disconnected);

                Log.Write("|OD|{0}|{1}|{2}|{3}|{4}|Dialing off site to {5} on channel {6}.", campaign.CampaignID, campaign.ShortDescription, m_AgentID, m_AgentName, callDetails.PhoneNum, CallDetails.OffsiteTransferNumber, ManagedChannel.ChannelResource.DeviceName);
                // Dial to number

                //set the outbound caller ID of the Campaign here!
                ManagedChannel.ChannelResource.OriginatingPhoneNumber = campaign.OutboundCallerID;


                //Make sure we are routed to the voice resource!
                if (ManagedChannel.ChannelResource is T1Channel)
                {
                    ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                }

                // Route early so we can listen to the dialing...
                // ManagedAgent.ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource);

                ManagedChannel.ChannelResource.RouteHalf(ManagedChannel.ChannelResource);
                ManagedChannel.ChannelResource.CallProgress = CallProgress.DialOnly; // If CPA needed, add here

                lock (this) { isDialing = true; }

                DialResult dr = ManagedChannel.ChannelResource.Dial(CallDetails.OffsiteTransferNumber);

                lock (this) { isDialing = false; }

                ManagedChannel.ChannelResource.RouteFull(transferredManagedChannel.ChannelResource);

                Log.Write("|OD|{0}|{1}|{2}|{3}|{4}|Offsite routing channel {5} to {6}", campaign.CampaignID, campaign.ShortDescription, m_AgentID, m_AgentName, callDetails.PhoneNum, ManagedChannel.ChannelResource.DeviceName,
                    transferredManagedChannel.ChannelResource.DeviceName);

                // Log dial result
                Log.Write("|OD|{0}|{1}|{2}|{3}|{4}|Dial reseult {5}.", campaign.CampaignID, campaign.ShortDescription, m_AgentID, m_AgentName, callDetails.PhoneNum, dr);
                bool bUpdCompletionStatus = false;
                DateTime dtStartTime = DateTime.Now;
                DateTime dtEndTime = DateTime.Now;

                switch (dr)
                {
                    case DialResult.Connected:
                    case DialResult.HumanDetected:
                    case DialResult.Successful:
                        bUpdCompletionStatus = true;
                        //DateTime agentConnecttime = DateTime.Now;

                        //PlayConnectingCustomer();
                        Thread t = new Thread(new ThreadStart(HangupCheck));
                        t.IsBackground = true;
                        t.Start();
                        threadEvent.WaitOne();

                        // Call finished
                        //Signalled, so Terminate the call
                        dtEndTime = DateTime.Now;

                        Log.Write("|OD|{0}|{1}|{2}|{3}|{4}|Call completed.", campaign.CampaignID, campaign.ShortDescription, m_AgentID, m_AgentName, callDetails.PhoneNum);
                        break;
                    // These may be re-activated if CPA is needed for offsite transfer.
                    //case DialResult.MachineDetected:
                    //    Log.Write("Manual dial machine detected - play some message to agent");
                    //    break;

                    //case DialResult.NoAnswer:
                    //    Log.Write("Manual dial no answer - play some message to agent");
                    //    break;

                    //case DialResult.Busy:
                    //    Log.Write("Manual dial busy - play some message to agent");
                    //    break;

                    //case DialResult.Error:
                    //case DialResult.Failed:
                    //    Log.Write("Manual dial error - play some message to agent");
                    //    break;

                    //case DialResult.OperatorIntercept:
                    //    Log.Write("Manual dial Operatorintercept - play some message to agent");
                    //    break;

                    default:
                        // call failed no answer

                        break;
                }

                CurrentDialResult = dr;
                //Log.Write("Updating result to call list");
                UpdateResultToCalllist("");

                // update call status to database
                if (bUpdCompletionStatus)
                {
                    //
                    try
                    {
                        System.TimeSpan callDuration = dtEndTime.Subtract(dtStartTime);
                        CallDetails.CallDuration = callDuration.TotalSeconds.ToString();
                        // Update Calllist (completion time)
                        CampaignAPI.UpdateCallCompletion(campaign, CallDetails);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Call update Exception");
                    }
                }
            }
            catch (ElementsException ee)
            {
                // These are Telephony Specific exceptions, such an the caller hanging up the phone during a play or record.
                if (ee is HangupException)
                {
                    // GW 09.29.10 - To allow greying of agent UI button on hangup, added beep
                    CampaignAPI.SetCallHangup(CallDetails.UniqueKey, campaign.CampaignDBConnString);
                    Log.Write("|OD|{0}|{1}|{2}|{3}|{4}|Call recipient hung up.", campaign.CampaignID, campaign.ShortDescription, m_AgentID, m_AgentName, callDetails.PhoneNum);
                }
                else
                    Log.WriteException(ee, "Script Elements Exception");
            }
            catch (ThreadAbortException tex)
            {
                if (DialerEngine.Connected)
                {
                    Log.WriteException(tex, "ThreadAbortException");
                }
                else
                {
                    m_TerminateCall = true;
                }
            }
            catch (Exception ex)
            {
                // This would be a general logic exception, such as a null reference violation or other .NET exception.
                if (DialerEngine.Connected)
                {
                    Log.WriteException(ex, "Script General Exception");
                }
            }
            finally
            {
                if (isDialing)
                {
                    try { ManagedChannel.ChannelResource.StopDial(); }
                    catch { }
                    isDialing = false;
                }

                try
                {
                    ManagedChannel.ChannelResource.Disconnect();
                }
                catch { }
                if (ManagedChannel != null)
                {

                    try
                    {
                        if (ManagedChannel.ChannelResource != null)
                        {

                            ManagedChannel.ChannelResource.Disconnect();

                            ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                            // Route to Home VR.
                            ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                        }
                    }
                    catch { }

                    // Add channel back to the list for later use
                    ManagedChannel.ReturnChannel(ManagedChannel, campaign.CampaignID);
                }
                try
                {
                    TransferredManagedChannel.ChannelResource.Disconnect();
                }
                catch { }
            }
        } 
        #endregion

        #region Private Methods
        public void HangupCheck()
        {
            try
            {
                while (!TerminateCall)
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Offsite hangup check exception.");
            }
        }


        private void UpdateResultToCalllist(string ResultDesc)
        {
            // May be re-activated in the future is CPA needed, for now, all results are "Transferred Offsite
            //switch (CurrentDialResult)
            //{
            //    case DialResult.Connected:
            //    case DialResult.HumanDetected:
            //    case DialResult.Successful:
            //        break;
            //    case DialResult.MachineDetected:
            //        ResultDesc = "Answering Machine";
            //        break;
            //    case DialResult.NoAnswer:
            //        ResultDesc = "No Answer";
            //        break;
            //    case DialResult.Busy:
            //        ResultDesc = "Busy";
            //        break;
            //    //case DialResult.Error:
            //    //    ResultDesc = "Error";
            //    //    break;
            //    //case DialResult.Failed:
            //    //    ResultDesc = "Failed";
            //    //    break;
            //    //case DialResult.CadenceBreak:
            //    //    ResultDesc = "Cadence Break";
            //    //    break;
            //    //case DialResult.LoopCurrentDrop:
            //    //    ResultDesc = "Loop Current Drop";
            //    //    break;
            //    //case DialResult.PbxDetected:
            //    //    ResultDesc = "Pbx Detected";
            //    //    break;
            //    //case DialResult.NoRingback:
            //    //    ResultDesc = "No Ringback";
            //    //    break;
            //    //case DialResult.AnalysisStopped:
            //    //    ResultDesc = "Analysis Stopped";
            //    //    break;
            //    //case DialResult.NoDialTone:
            //    //    ResultDesc = "No DialTone";
            //    //    break;
            //    //case DialResult.FaxToneDetected:
            //    //    ResultDesc = "FaxTone Detected";
            //    //    break;
            //    case DialResult.OperatorIntercept:
            //        ResultDesc = "Operator Intercept";
            //        break;
            //    default:
            //        ResultDesc = "Failed";
            //        break;
            //}
            ResultDesc = "Transferred Offsite";
            if (ResultDesc != "")
            {
                Log.Write("|OD|{0}|{1}|{2}|{3}|{4}|Updating call reult to '{5}'.", campaign.CampaignID, campaign.ShortDescription, m_AgentID, m_AgentName, callDetails.PhoneNum, ResultDesc);
                CampaignAPI.AddResultCodeToCallList(campaign.CampaignDBConnString, CallDetails, ResultDesc);
            }
        } 
        #endregion

        #region Events
        /// <summary>
        /// The Disconnected event processing code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ChannelResource_Disconnected(object sender, DisconnectedEventArgs e)
        {
            try
            {
                TerminateCall = true;

                if (isDialing)
                {
                    try { ManagedChannel.ChannelResource.StopDial(); }
                    catch { }
                    isDialing = false;
                }

                // Here we will simply write to the log that the caller hung up the phone.
                Log.Write("|OD|{0}|{1}|{2}|{3}|{4}|Channel disonnect event received.", campaign.CampaignID, campaign.ShortDescription, m_AgentID, m_AgentName, callDetails.PhoneNum);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "ChannelResource_Disconnected Exception");
            }
        } 
        #endregion
    }
}
