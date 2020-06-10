using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Threading;
using Rainmaker.Common.DomainModel;
using RainMakerDialer.Dialer;
using VoiceElements.Common;
using System.Linq;
using System.Diagnostics;

namespace Rainmaker.RainmakerDialer
{
    public class CampaignProcess
    {
        #region Variables and Properties
        // Reference to campaign
        private Campaign objCampaign = null;

        private CampaignMonitor campaignMonitor;

        // Reference to dialingparameters
        private DialingParameter objDialParameter = null;

        // Reference to otherparameters
        private OtherParameter objOtherParameter = null;

        // References the main Log File created at startup
        private static Log Log = DialerEngine.Log;

        // Reference to current dialing count
        private static Dictionary<long, int> dCampaignDialCount = new Dictionary<long, int>();

        // Reference to calltype
        private CallType callType = CallType.AMCall;
        public CallType CallType
        {
            get { return callType; }
            set { callType = value; }
        }

        private CampaignStats campStats;
        public CampaignStats CampStats
        {
            get { return campStats; }
            set { campStats = value; }
        }

        private ThrottledPrediction Prediction;
        //public ThrottledPrediction Prediction
        //{
        //    get { return prediction; }
        //    set { prediction = value; }
        //}

        // Reference to RecordingsPath
        private string recordingsPath = string.Empty;
        public string RecordingsPath
        {
            get { return recordingsPath; }
            set { recordingsPath = value; }
        }


        // Reference to record calls switch
        private bool recordCalls = false;
        public bool RecordCalls
        {
            get { return recordCalls; }
            set { recordCalls = value; }
        }

        // Reference to record calls switch 
        private bool recordBeep = false;
        public bool RecordBeep
        {
            get { return recordBeep; }
            set { recordBeep = value; }
        }

        private int callDelay = 5;
        public int CallDelay
        {
            get { return callDelay; }
            set { callDelay = value; }
        }

        private bool terminateCampaignProcess = false;

        private bool NoRecordsExist = true;

        private int dialParamsInterval = 30;

        private DateTime lastDialParamUpdate = DateTime.Now;

        private int initialActiveQueryCount = 0;
        private int currentlyActiveQueryCount = 0;

        // Reference to agent availabilty check thread
        //private Thread tAgentCheckThread;

        // Reference to list of call threads
        private List<Thread> tCallThreads = new List<Thread>();

        // Query and call queue collections
        private Dictionary<long, Queue<CampaignDetails>> CallQueueList = new Dictionary<long, Queue<CampaignDetails>>();
        private List<Query> Querys = new List<Query>();

        private CampaignStatus terminateCampaignProcessState;

        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objCampaign"></param>
        /// <param name="objDialParameter"></param>
        public CampaignProcess(Campaign objCampaign, DialingParameter objDialParameter, OtherParameter objOtherParameter)
        {
            this.objCampaign = objCampaign;
            this.campaignMonitor = new CampaignMonitor(objCampaign);
            this.objDialParameter = objDialParameter;
            this.objOtherParameter = objOtherParameter;
            try
            { this.dialParamsInterval = Convert.ToInt16(ConfigurationManager.AppSettings["DialingParamsRefreshIntervalSecs"]); }
            catch { this.dialParamsInterval = 30; }
        } 
        #endregion

        #region Run a Manned Campaign
        /// <summary>
        /// Run Campaign : Gets call list for each query, 
        /// creates a thread for each number and start dialing, 
        /// before dialing checks for agent availability
        /// </summary>
        public void RunCampaign()
        {
            #region Setup
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            this.CampStats = new CampaignStats(objCampaign);
            this.CampStats.CallStatsWindow = objDialParameter.CallStatisticsWindow;
            this.Prediction = new ThrottledPrediction(objCampaign);
            decimal dInitialDialsPerAgent = 2;
            try
            {
                dInitialDialsPerAgent = objDialParameter.ChannelsPerAgent;
            }
            catch { }
            try
            {
                CallDelay = objDialParameter.MinimumDelayBetweenCalls;
            }
            catch { }
            #endregion

            try
            {
                if (objCampaign != null)
                {
                    #region Prepare Campaign

                    campaignMonitor.campaignStateChangeEvent += new CampaignMonitorEventHandler(campaignMonitor_campaignStateChangeEvent);
                    campaignMonitor.queryStateChangeEvent += new CampaignMonitorEventHandler(campaignMonitor_queryStateChangeEvent);

                    Thread campaignMonitorThread = new Thread(campaignMonitor.monitor);
                    campaignMonitorThread.Name = "CampaignMonitor";
                    campaignMonitorThread.Start(Thread.CurrentThread);

                    NoRecordsExist = true;
                    List<long> lstOrderIndexSet = new List<long>();

                    int iCallsPerQuery = Convert.ToInt32(Utilities.GetAppSetting("CallsPerQuery", "1"));

                    #endregion

                    while (CallQueueList.Count < 1) { Thread.Sleep(1000); }

                    if (CallQueueList.Count > 0)
                    {
                        long currentQueryId = Querys[0].QueryID;
                        int currentQueryIndex = 0;

                        int currentQueryDialCount = 0;
                        int counter = 0;

                        bool bPauseTimeAfterInitialDialAdded = false;

                        List<string> alDialedList = new List<string>();

                        #region Call Queue Loop
#if DEBUG
                        Stopwatch callLoopStopwatch = new Stopwatch();
                        Stopwatch callLoopPreDialStopwatch = new Stopwatch();
                        Stopwatch callLoopPostDialStopwatch = new Stopwatch();

                        Stopwatch segmentStopWatch = new Stopwatch();
#endif
                        while (CallQueueList.Count > 0)
                        {
#if DEBUG
                            segmentStopWatch.Reset();

                            if (counter > 0)
                            {
                                DialerEngine.Log.Write("|TIMERS|POST-DIAL count|ms: {0}|{1}", counter, callLoopPreDialStopwatch.ElapsedMilliseconds);
                                callLoopPostDialStopwatch.Reset();

                                DialerEngine.Log.Write("|TIMERS|DIAL count|ms: {0}|{1}", counter, callLoopStopwatch.ElapsedMilliseconds);
                            }

                            callLoopStopwatch.Reset();
                            callLoopStopwatch.Start();

                            callLoopPreDialStopwatch.Start();
#endif
                            #region Pick A Query
#if DEBUG
                            segmentStopWatch.Start();
#endif
                            if (currentQueryIndex >= Querys.Count) currentQueryIndex = 0;

                            Query currentQuery = Querys[currentQueryIndex];
                            Queue<CampaignDetails> qCallQueue = CallQueueList[currentQuery.QueryID];

                            if (currentQuery.QueryID != currentQueryId)
                            {
                                currentQueryId = currentQuery.QueryID;
                                currentQueryDialCount = 0;
                            }
#if DEBUG
                            DialerEngine.Log.Write("|TIMERS|PICK A QUERY count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                            segmentStopWatch.Reset();
#endif
                            #endregion

                            if (qCallQueue == null || qCallQueue.Count == 0)
                            {
                                #region Out of Calls
#if DEBUG
                                segmentStopWatch.Start();
#endif
                                DialerEngine.Log.Write("|CP|{0}|{1}|Query {2} '{3}' has completed.", objCampaign.CampaignID, objCampaign.ShortDescription, currentQuery.QueryID, currentQuery.QueryName);
                                CampaignAPI.UpdateQueryComplete(objCampaign, currentQuery.QueryID, false);

                                CallQueueList.Remove(currentQuery.QueryID);
                                Querys.RemoveAt(currentQueryIndex);

                                currentQueryIndex++;

                                // Check for standby queries to pull into the queue and remix
                                DataSet dsStandbyQueries = CampaignAPI.GetStandbyQueries(objCampaign);

                                if (dsStandbyQueries.Tables[0].Rows.Count > 0)
                                {
                                    DataRow dr = dsStandbyQueries.Tables[0].Rows[0];
                                    Query query = new Query();
                                    query.QueryID = Convert.ToInt64(dr["QueryID"]);
                                    query.QueryName = dr["QueryName"].ToString();
                                    query.QueryCondition = dr["QueryCondition"].ToString();
                                    DialerEngine.Log.Write("|CP|{0}|{1}|A standby query {2}, '{3}' has been found to activate.", objCampaign.CampaignID, objCampaign.ShortDescription, query.QueryID, query.QueryName);

                                    CampaignAPI.UpdateQueryStatus(objCampaign, query.QueryID, true, false);
                                    DialerEngine.Log.Write("|CP|{0}|{1}|Standby query {2}, '{3}' activation queued.", objCampaign.CampaignID, objCampaign.ShortDescription, query.QueryID, query.QueryName);
                                }
#if DEBUG
                                DialerEngine.Log.Write("|TIMERS|OUT OF CALLS count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                segmentStopWatch.Reset();
#endif
                                #endregion
                            }
                            else
                            {
                                #region Still Got Calls

                                #region Set Dialer Queue
#if DEBUG
                                segmentStopWatch.Start();
#endif
                                try
                                {
                                    if (terminateCampaignProcess) break;

                                    CampaignAPI.SetDialerQueue(currentQuery.QueryID, objCampaign.CampaignDBConnString);
                                }
                                catch { }

                                try
                                {
                                    CampaignDetails objCallDetails_C = qCallQueue.Peek();
                                    if (objCallDetails_C.OrderIndex > 1 && Querys.Count > lstOrderIndexSet.Count)
                                    {
                                        if (!lstOrderIndexSet.Contains(currentQuery.QueryID))
                                        {
                                            lstOrderIndexSet.Add(currentQuery.QueryID);
                                        }
                                        currentQueryIndex++;
#if DEBUG
                                        DialerEngine.Log.Write("|TIMERS|SET DIALER QUEUE count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                        segmentStopWatch.Reset();
#endif
                                        continue;
                                    }
                                }
                                catch { }
#if DEBUG
                                DialerEngine.Log.Write("|TIMERS|SET DIALER QUEUE count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                segmentStopWatch.Reset();
#endif
                                #endregion

                                #region Get Next Call from Queue
#if DEBUG
                                segmentStopWatch.Start();
#endif
                                CampaignDetails objCallDetails = qCallQueue.Dequeue();
#if DEBUG
                                DialerEngine.Log.Write("|TIMERS|GET CALL FROM QUEUE count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                segmentStopWatch.Reset();
#endif
                                #endregion

                                #region Skip Invalid Call
#if DEBUG
                                segmentStopWatch.Start();
#endif
                                if (!ValidateCallData(objCallDetails))
                                {
                                    //Check if the call needs to be terminated
                                    //If the AM PM call times are reset while the campaign is running
                                    if (terminateCampaignProcess)
                                    {
                                        DialerEngine.Log.Write("|CP|{0}|{1}|Dialing params verified and dialing is being terminated.", objCampaign.CampaignID, objCampaign.ShortDescription);
#if DEBUG
                                        DialerEngine.Log.Write("|TIMERS|VALIDATE CALL count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                        segmentStopWatch.Reset();
#endif
                                        break;
                                    }
#if DEBUG
                                    DialerEngine.Log.Write("|TIMERS|VALIDATE CALL count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                    segmentStopWatch.Reset();
#endif
                                    continue;
                                }
#if DEBUG
                                DialerEngine.Log.Write("|TIMERS|VALIDATE CALL count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                segmentStopWatch.Reset();
#endif
                                #endregion

                                // 2013-09-18 D. Pollastrini: 
                                // At this point we have a validated call to dial

                                #region Delays

                                #region Set Initial Dials / Agent
                                //  Begin predictive 
                                try
                                {
                                    // *** Power dial add 1.17.11
                                    if (objDialParameter.DialingMode == (int)DialingMode.PowerDial)
                                    {
                                        dInitialDialsPerAgent = 1;
                                    }
                                    else
                                    {
                                        dInitialDialsPerAgent = objDialParameter.ChannelsPerAgent;
                                    }
                                }
                                catch { }
                                #endregion

                                #region Set Call Delay
                                try
                                {
                                    CallDelay = objDialParameter.MinimumDelayBetweenCalls;
                                }
                                catch { }
                                #endregion

                                #region Wait For Min Agents
#if DEBUG
                                segmentStopWatch.Start();
#endif
                                // before dialing out check for agent availability
                                AgentProcess agentProcess = new AgentProcess(objCampaign);

                                // waits until required agents logged in
                                DateTime nextCheck = DateTime.Now;
                                int iAgentCount;
                                while ((iAgentCount = agentProcess.GetLoggedinAgentCount()) < AgentProcess.MinAgentsRequiredToDial)
                                {
                                    if (terminateCampaignProcess) break;
                                }
#if DEBUG
                                DialerEngine.Log.Write("|TIMERS|WAIT FOR AGENTS count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                segmentStopWatch.Reset();
#endif
                                #endregion

                                #endregion

                                // 2013-09-18 D. Pollastrini: 
                                // Why didn't we check for dupes earlier during call details validations (before predictive mode methods & delays?

                                #region Skip Dupes
#if DEBUG
                                segmentStopWatch.Start();
#endif
                                // This block skips dupe numbers 
                                try
                                {
                                    if (alDialedList.Contains(objCallDetails.PhoneNum))
                                    {
                                        DialerEngine.Log.Write("|CP|{0}|{1}|Query overlap phone number '{2}' found in query '{3}', skipping dial.", objCampaign.CampaignID, objCampaign.ShortDescription, objCallDetails.PhoneNum, currentQuery.QueryName);
#if DEBUG
                                        DialerEngine.Log.Write("|TIMERS|CHECK FOR DUPES count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                        segmentStopWatch.Reset();
#endif
                                        continue;
                                    }
                                    alDialedList.Add(objCallDetails.PhoneNum);
                                }
                                catch (Exception eex)
                                {
                                    Log.WriteException(eex, "Error in Campaign Process");
                                }
#if DEBUG
                                DialerEngine.Log.Write("|TIMERS|CHECK FOR DUPES count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                segmentStopWatch.Reset();
#endif
                                #endregion

                                #region Get Dialer for Call
#if DEBUG
                                segmentStopWatch.Start();
#endif
                                // start dialing
                                Dialer dialer = new Dialer(objCampaign, this);
                                dialer.CallDetails = objCallDetails;
                                dialer.CallType = CallType;
                                dialer.RecordingsPath = RecordingsPath;
                                dialer.DialParameter = objDialParameter;
                                dialer.OtherParameter = objOtherParameter;
                                dialer.CampStats = this.CampStats;
                                try
                                {
                                    dialer.QueryId = currentQuery.QueryID;
                                    dialer.Query = currentQuery;
                                }
                                catch { }
#if DEBUG
                                DialerEngine.Log.Write("|TIMERS|GET A DIALER count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                segmentStopWatch.Reset();
#endif
                                #endregion

                                if (terminateCampaignProcess) break;

                                #region Wait for Available Channel
#if DEBUG
                                segmentStopWatch.Start();
#endif
                                // Check for channels availabilty, wait until channels available
                                // Checking with dialing parameters max phone line count
                                DialerEngine.Log.Write("|CP|{0}|{1}|Checking for an available channel.", objCampaign.CampaignID, objCampaign.ShortDescription);
                                // MaxChannelsAllowed is implemented based on PhoneLineCount setting and 
                                // number of logged in agents, If there are 2 agents logged in and dialing out 20 channels
                                // does not make any sense, so we are restricting number of channes to be used for this 
                                // camapign based on loggedin agent count *** Bad conclusion .. changed to what the user set!
                                //int maxChannelsAllowed = Math.Min(objDialParameter.PhoneLineCount, (int)(iAgentCount * dInitialDialsPerAgent));
                                int maxChannelsAllowed = objDialParameter.PhoneLineCount;

                                bool bAvailable = ManagedChannel.IsChannelsAvailable(objCampaign.CampaignID, maxChannelsAllowed);
                                while (!bAvailable)
                                {
                                    DialerEngine.Log.Write("|CP|{0}|{1}|No channels available.  Max allowed is set to {2}.", objCampaign.CampaignID, objCampaign.ShortDescription, maxChannelsAllowed.ToString());
                                    Thread.Sleep(3000);
                                    bAvailable = ManagedChannel.IsChannelsAvailable(objCampaign.CampaignID, maxChannelsAllowed);

                                    if (terminateCampaignProcess) break;
                                }
#if DEBUG
                                DialerEngine.Log.Write("|TIMERS|WAIT FOR CHANNEL count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                segmentStopWatch.Reset();
#endif
                                #endregion

                                if (terminateCampaignProcess) break;

                                #region Dial that BIOTCH
#if DEBUG
                                segmentStopWatch.Start();
#endif
                                ThreadStart ts = new ThreadStart(dialer.RunScript);

                                Thread t = new Thread(ts);
                                t.Name = objCallDetails.PhoneNum;
                                tCallThreads.Add(t);

                                DialerEngine.Log.Write("|CP|{0}|{1}|Starting dialing process for outbound call to '{2}'.", objCampaign.CampaignID, objCampaign.ShortDescription, objCallDetails.PhoneNum);
#if DEBUG
                                DialerEngine.Log.Write("|TIMERS|SPIN UP DIALER THREAD count|ms: {0}|{1}", counter, segmentStopWatch.ElapsedMilliseconds);
                                segmentStopWatch.Reset();

                                DialerEngine.Log.Write("|TIMERS|PRE-DIAL count|ms: {0}|{1}", counter + 1, callLoopPreDialStopwatch.ElapsedMilliseconds);
                                callLoopPreDialStopwatch.Reset();
#endif
                                t.Start();
#if DEBUG
                                callLoopPostDialStopwatch.Start();
#endif

                                #endregion

                                #region Update Counts
                                //update the dial count +1
                                UpdateCampaignDialCount(objCampaign.CampaignID, true);

                                Thread.Sleep(1);

                                counter++;

                                if (currentQueryDialCount >= iCallsPerQuery) currentQueryIndex++;
                                #endregion

                                #region Initial Dial Mumbo Jumbo
                                if ((int)(iAgentCount * dInitialDialsPerAgent) > counter)
                                {
                                    DialerEngine.Log.Write("|CP|{0}|{1}|Initial dial in process statistics check: {2} channels, {3} dials, {4} agents.", objCampaign.CampaignID, objCampaign.ShortDescription, objDialParameter.PhoneLineCount, counter, iAgentCount);
                                    continue;
                                }
                                else
                                {
                                    if (!bPauseTimeAfterInitialDialAdded)
                                    {
                                        int pauseTimeAfterInitialDial = 20;
                                        try
                                        {
                                            pauseTimeAfterInitialDial = Convert.ToInt32(Utilities.GetAppSetting("PauseTimeAfterInitialDial", "20"));
                                        }
                                        catch { }
                                        DialerEngine.Log.Write("|CP|{0}|{1}|Pause time after initial dial: {2}.", objCampaign.CampaignID, objCampaign.ShortDescription, pauseTimeAfterInitialDial);
                                        bPauseTimeAfterInitialDialAdded = true;
                                        Thread.Sleep(pauseTimeAfterInitialDial * 1000);
                                    }
                                }
                                #endregion

                                #region Throttling
                                switch (objDialParameter.DialingMode)
                                {
                                    case (int)DialingMode.PowerDial:
                                        #region Throttle: Power Dial
                                        Thread.Sleep(1000);

                                        // wait for free agents - Changed to loop here instead of in agent process GW 10.01.10
                                        DialerEngine.Log.Write("|CP|{0}|{1}|Waiting for free agent.", objCampaign.CampaignID, objCampaign.ShortDescription);
                                        DateTime dtWaitUntil = DateTime.Now.AddMinutes(2.0);
                                        int iActiveAgentCount = 0;
                                        while (iActiveAgentCount < 1)
                                        {
                                            iActiveAgentCount = agentProcess.WaitForActiveAgents();
                                            DialerEngine.Log.Write("|CP|{0}|{1}|{2} free agents found.", objCampaign.CampaignID, objCampaign.ShortDescription, iActiveAgentCount);
                                            if (DateTime.Now > dtWaitUntil)
                                                break;

                                            if (terminateCampaignProcess) break;
                                        }

                                        if (terminateCampaignProcess) break;

                                        dtWaitUntil = DateTime.Now.AddMinutes(5.0);
                                        int iDialCount = GetCampaignDialCount(objCampaign.CampaignID);

                                        // If current dials are more than available agents then wait
                                        while (iDialCount >= iActiveAgentCount)
                                        {
                                            Thread.Sleep(2000);
                                            DialerEngine.Log.Write("|CP|{0}|{1}|Waiting for dial count to drop to agent count.", objCampaign.CampaignID, objCampaign.ShortDescription);

                                            // For safety, wait time not more than 5 minutes
                                            if (DateTime.Now > dtWaitUntil)
                                                break;

                                            iDialCount = GetCampaignDialCount(objCampaign.CampaignID);
                                            iActiveAgentCount = ManagedAgent.GetAvailableAgentCount(objCampaign.CampaignID);

                                            // Weighted drop rate sensitivity addition
                                            if (objDialParameter.DialingMode != (int)DialingMode.PowerDial)
                                            {
                                                if (objDialParameter.DropRateSensitivity > 1)
                                                {
                                                    double adjustedAgentCount = (double)iActiveAgentCount - ((double)iActiveAgentCount * ((double)(objDialParameter.DropRateSensitivity - 1) / 10));
                                                    iActiveAgentCount = Convert.ToInt16(adjustedAgentCount);
                                                }
                                            }

                                            if (terminateCampaignProcess) break;
                                        }

                                        if (terminateCampaignProcess) break;
                                        #endregion
                                        break;
                                    default:
                                        #region Throttle: Default Dial
                                        int iTSC;
                                        int iDroppedCalls;
                                        decimal iActualDropRatePercentage;

                                        switch (objDialParameter.ActiveDialingAlgorithm)
                                        {
                                            case 1:
                                                iTSC = campStats.GetNextCallTime(CallDelay, iAgentCount, objDialParameter.DefaultCallLapse);
                                                break;
                                            case 2:
                                                do
                                                {
                                                    iDroppedCalls = CampaignProcess.GetSilentCallCount(objCampaign.ShortDescription);
                                                    iActualDropRatePercentage = counter > 0 ? ((decimal)iDroppedCalls / (decimal)counter * 100m) : 0m;

                                                    int[] calculatedDelayorCallCount = Prediction.CalculateNextCallTime
                                                        (
                                                            objDialParameter, 
                                                            campStats, 
                                                            iAgentCount, 
                                                            ManagedAgent.GetAvailableAgentCount(objCampaign.CampaignID), 
                                                            iActualDropRatePercentage, 
                                                            GetCampaignDialCount(objCampaign.CampaignID)
                                                        );

                                                    iTSC = calculatedDelayorCallCount[0];
                                                    if (iTSC < 0) Thread.Sleep(Convert.ToInt32(Utilities.GetAppSetting("AlgorithmPauseSleepMS", "5000")));

                                                    if (terminateCampaignProcess) break;

                                                } while (iTSC < 0);

                                                DateTime nextCallTime = System.DateTime.Now.AddMilliseconds(iTSC);

                                                DialerEngine.Log.Write("|CP|{0}|{1}|Next call time determined: {2}.", objCampaign.CampaignID, objCampaign.ShortDescription, nextCallTime.ToString());

                                                int sleepTime = Convert.ToInt32(Utilities.GetAppSetting("NextCallCheckSleepMS", "100"));

                                                // Loop until time to call again while checking pause and idle state GW 10.17.10
                                                while (System.DateTime.Now < nextCallTime)
                                                {
                                                    if (terminateCampaignProcess) break;
                                                    Thread.Sleep(sleepTime);
                                                }
                                                if (terminateCampaignProcess) break;

                                                DialerEngine.Log.Write("|CP|{0}|{1}|Statistics: Total Calls {2}, Dropped Calls {3}, Drop Rate {4}%, Actual Drop Rate {5}%.", objCampaign.CampaignID, objCampaign.ShortDescription, counter, iDroppedCalls, objDialParameter.DropRatePercent, iActualDropRatePercentage);

                                                break;
                                            default:
                                                iTSC = campStats.GetNextCallTime(CallDelay, iAgentCount, objDialParameter.DefaultCallLapse);
                                                break;
                                        }

                                        #endregion
                                        break;
                                }
                                #endregion

                                #endregion
                            }

                            if (terminateCampaignProcess) break;

                        }
                        #endregion

                        #region Error Handling and Wrap Shit Up

                        campaignMonitor.campaignStateChangeEvent -= new CampaignMonitorEventHandler(campaignMonitor_campaignStateChangeEvent);
                        campaignMonitor.queryStateChangeEvent -= new CampaignMonitorEventHandler(campaignMonitor_queryStateChangeEvent);

                        // Wait until call threads finish.
                        for (int i = 0; i < tCallThreads.Count; i++)
                        {
                            if (tCallThreads[i] != null && tCallThreads[i].IsAlive)
                            {
                                tCallThreads[i].Join();
                            }
                        }

                        dCampaignDialCount[objCampaign.CampaignID] = 0;
                    }

                    if (NoRecordsExist)
                        DialerEngine.Log.Write("|CP|{0}|{1}|No available call records found.", objCampaign.CampaignID, objCampaign.ShortDescription);

                    objCampaign.StatusID = (long)terminateCampaignProcessState;
                    CampaignAPI.UpdateCampaignStatus(objCampaign);
                }
            }
            catch (ThreadAbortException TAE)
            {
                DialerEngine.Log.Write("|CP|{0}|{1}|Aborted run campaign exception: {2}.", objCampaign.CampaignID, objCampaign.ShortDescription, TAE.Message);

                if (!DialerEngine.Connected)
                {
                    if (tCallThreads.Count > 0)
                    {
                        for (int i = 0; i < tCallThreads.Count; i++)
                        {
                            if (tCallThreads[i] != null)
                            {
                                tCallThreads[i].Abort();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Unexpected Error in Campaign Process");
            }
            finally
            {
                DialerEngine.Log.Write("|CP|{0}|{1}|Campaign process complete.", objCampaign.CampaignID, objCampaign.ShortDescription);
            }
                        #endregion
        } 
        #endregion

        #region Run an Unmanned Campaign
        //-------------------------------------------------------------
        // 
        /// <summary>
        /// Run Campaign Unmanned mode : Gets call list for each query, 
        /// creates a thread for each number and start dialing, 
        /// </summary>
        //-------------------------------------------------------------
        public void RunCampaignUnmannedMode()
        {
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            this.CampStats = new CampaignStats(objCampaign);
            this.Prediction = new ThrottledPrediction(objCampaign);
            DataSet dsQueries = null;
            try
            {
                if (objCampaign != null)
                {

                    dsQueries = CampaignAPI.GetActiveQueries(objCampaign);
                    DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|Found {2} active queries.", objCampaign.CampaignID, objCampaign.ShortDescription, dsQueries.Tables[0].Rows.Count);
                    NoRecordsExist = true;
                    List<long> lstOrderIndexSet = new List<long>();

                    int iNumberOfQueries = dsQueries.Tables[0].Rows.Count;
                    int iCallsPerQuery = Convert.ToInt32(Utilities.GetAppSetting("CallsPerQuery", "1"));

                    foreach (DataRow dr in dsQueries.Tables[0].Rows)
                    {
                        Query query = new Query();
                        query.QueryID = Convert.ToInt64(dr["QueryID"]);
                        query.QueryName = dr["QueryName"].ToString();
                        query.QueryCondition = dr["QueryCondition"].ToString();

                        AddQueryToMix(query);
                    }

                    if (CallQueueList.Count > 0)
                    {
                        long currentQueryId = Querys[0].QueryID;

                        int currentQueryIndex = 0;
                        int currentQueryDialCount = 0;
                        int counter = 0;

                        System.Collections.ArrayList alDialedList = new System.Collections.ArrayList();

                        #region Call Queue Contains > 0 Calls
                        while (CallQueueList.Count > 0)
                        {
                            if (currentQueryIndex >= Querys.Count) currentQueryIndex = 0;
                            Query currentQuery = Querys[currentQueryIndex];
                            Queue<CampaignDetails> qCallQueue = CallQueueList[currentQuery.QueryID];

                            if (currentQuery.QueryID != currentQueryId)
                            {
                                currentQueryDialCount = 0;
                            }

                            if (qCallQueue == null || qCallQueue.Count == 0)
                            {
                                DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|Query {2} '{3}' has completed.", objCampaign.CampaignID, objCampaign.ShortDescription, currentQuery.QueryID, currentQuery.QueryName); 
                                CampaignAPI.UpdateQueryComplete(objCampaign, currentQuery.QueryID, false);

                                Querys.RemoveAt(currentQueryIndex);
                                CallQueueList.Remove(currentQuery.QueryID);

                                currentQueryIndex++;

                                DataSet dsStandbyQueries = CampaignAPI.GetStandbyQueries(objCampaign);

                                if (dsStandbyQueries.Tables[0].Rows.Count > 0)
                                {

                                    DataRow dr = dsStandbyQueries.Tables[0].Rows[0];
                                    Query query = new Query();
                                    query.QueryID = Convert.ToInt64(dr["QueryID"]);
                                    query.QueryName = dr["QueryName"].ToString();
                                    query.QueryCondition = dr["QueryCondition"].ToString();
                                    DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|A standby query {2}, '{3}' has been found to activate.", objCampaign.CampaignID, objCampaign.ShortDescription, query.QueryID, query.QueryName);

                                    AddQueryToMix(query);

                                    CampaignAPI.UpdateQueryStatus(objCampaign, query.QueryID, true, false);
                                    DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|Standby query {2}, '{3}' activation complete.", objCampaign.CampaignID, objCampaign.ShortDescription, query.QueryID, query.QueryName);
                                }
                            }
                            else
                            {
                                // Set current queue to dialer
                                try
                                {
                                    if (terminateCampaignProcess) break;
                                    CampaignAPI.SetDialerQueue(currentQuery.QueryID, objCampaign.CampaignDBConnString);
                                }
                                catch { }

                                try
                                {
                                    CampaignDetails objCallDetails_C = qCallQueue.Peek();
                                    if (objCallDetails_C.OrderIndex > 1 && Querys.Count > lstOrderIndexSet.Count)
                                    {
                                        if (!lstOrderIndexSet.Contains(currentQuery.QueryID))
                                        {
                                            lstOrderIndexSet.Add(currentQuery.QueryID);
                                        }
                                        currentQueryIndex++;
                                        continue;
                                    }
                                }
                                catch { }

                                CampaignDetails objCallDetails = qCallQueue.Dequeue();

                                // Check for valid data
                                if (!ValidateCallData(objCallDetails))
                                {
                                    //Check if the call needs to be terminated
                                    //If the AM PM call times are reset while the campaign is running
                                    if (terminateCampaignProcess)
                                    {
                                        DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|Dialing params verified and dialing is being terminated.", objCampaign.CampaignID, objCampaign.ShortDescription);
                                        break;
                                    }
                                    continue;
                                }


                                // Add prefix and suffix to the number (from dialparameter) Commented out moved to dialer thread 1.17.11
                                //if (objDialParameter != null)
                                //{
                                //    objCallDetails.PhoneNum = objDialParameter.LocalDialingPrefix.Trim()
                                //        + objCallDetails.PhoneNum + objDialParameter.LocalSuffix.Trim();
                                //}


                                // This block skips dupe numbers
                                try
                                {
                                    if (alDialedList.Contains(objCallDetails.PhoneNum))
                                    {
                                        DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|Query overlap phone number '{2}' found in query '{3}', skipping dial.", objCampaign.CampaignID, objCampaign.ShortDescription, objCallDetails.PhoneNum, currentQuery.QueryName);
                                        continue;
                                    }
                                    alDialedList.Add(objCallDetails.PhoneNum);
                                }
                                catch (Exception eex)
                                {
                                    Log.WriteException(eex, "Error in UM Campaign Process");
                                }


                                // start dialing
                                Dialer dialer = new Dialer(objCampaign, this);
                                dialer.CallDetails = objCallDetails;
                                dialer.CallType = CallType;
                                dialer.RecordingsPath = RecordingsPath;
                                dialer.DialParameter = objDialParameter;
                                dialer.CampStats = this.CampStats;
                                try
                                {
                                    dialer.QueryId = currentQuery.QueryID;
                                    dialer.Query = currentQuery;
                                }
                                catch { }

                                if (objOtherParameter != null && objOtherParameter.TransferMessage != "")
                                {
                                    dialer.HumanMessage = Utilities.GetAppSetting("AdminServerPath", "")
                                           + objOtherParameter.TransferMessage;
                                }

                                if (terminateCampaignProcess) break;

                                // Check for channels availabilty, wait until channels available
                                // Checking with dialing parameters max phone line count
                                DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|Checking for an available channel.", objCampaign.CampaignID, objCampaign.ShortDescription);

                                int maxChannelsAllowed = objDialParameter.PhoneLineCount;

                                bool bAvailable = ManagedChannel.IsChannelsAvailable(objCampaign.CampaignID,
                                    maxChannelsAllowed);
                                while (!bAvailable)
                                {
                                    DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|No channels available.  Max allowed is set to {2}.", objCampaign.CampaignID, objCampaign.ShortDescription, maxChannelsAllowed.ToString());
                                    Thread.Sleep(3000);
                                    bAvailable = ManagedChannel.IsChannelsAvailable(objCampaign.CampaignID,
                                        maxChannelsAllowed);

                                    if (terminateCampaignProcess) break;
                                }

                                if (terminateCampaignProcess) break;

                                ThreadStart ts = new ThreadStart(dialer.RunScript);

                                Thread t = new Thread(ts);
                                t.Name = objCallDetails.PhoneNum;
                                tCallThreads.Add(t);

                                DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|Starting dialing process for outbound call to '{2}'.", objCampaign.CampaignID, objCampaign.ShortDescription, objCallDetails.PhoneNum);

                                //t.IsBackground = true;
                                t.Start();

                                UpdateCampaignDialCount(objCampaign.CampaignID, true);

                                Thread.Sleep(1);
                                counter++;
                                currentQueryDialCount++;
                                if (currentQueryDialCount >= iCallsPerQuery) currentQueryIndex++;

                            } // else in while
                            if (terminateCampaignProcess) break;
                        } // end of while

                        #endregion

                        for (int i = 0; i < tCallThreads.Count; i++)
                        {
                            if (tCallThreads[i] != null && tCallThreads[i].IsAlive)
                            {
                                tCallThreads[i].Join();
                            }
                        }

                        dCampaignDialCount[objCampaign.CampaignID] = 0;
                    }

                    DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|No available call records found.", objCampaign.CampaignID, objCampaign.ShortDescription);

                    DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|Campaign process completed, checking threads for shutdown.", objCampaign.CampaignID, objCampaign.ShortDescription);

                    objCampaign.StatusID = (long)terminateCampaignProcessState;
                    CampaignAPI.UpdateCampaignStatus(objCampaign);
                }
            }
            catch (ThreadAbortException TAE)
            {
                DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|Aborted run campaign exception: {2}.", objCampaign.CampaignID, objCampaign.ShortDescription, TAE.Message);

                if (!DialerEngine.Connected)
                {
                    if (tCallThreads.Count > 0)
                    {
                        DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|Aborted call threads.", objCampaign.CampaignID, objCampaign.ShortDescription);
                        for (int i = 0; i < tCallThreads.Count; i++)
                        {
                            if (tCallThreads[i] != null)
                            {
                                tCallThreads[i].Abort();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Unexpected Error in Campaign Process");
            }
            finally
            {
                dsQueries = null;
                DialerEngine.Log.Write("|CP|{0}|{1}|Unmanned|Campaign process run complete.", objCampaign.CampaignID, objCampaign.ShortDescription);
            }
        } 
        #endregion

        #region Private Methods

        //-------------------------------------------------------------
        /// <summary>
        /// Remove call thread from list
        /// </summary>
        /// <param name="id"></param>
        /// <param name="stopCampaign"></param>
        //-------------------------------------------------------------
        public void RemoveCallThreadFromList(string name)
        {
            DialerEngine.Log.Write("|CP|{0}|{1}|Remove call thread {2} start.", objCampaign.CampaignID, objCampaign.ShortDescription, name);
            try
            {
                lock (tCallThreads)
                {
                    for (int i = 0; i < tCallThreads.Count; i++)
                    {
                        if (tCallThreads[i].Name == name)
                        {
                            try
                            {
                                tCallThreads[i] = null;
                                tCallThreads.RemoveAt(i);
                                DialerEngine.Log.Write("|CP|{0}|{1}|Remove call thread {2} complete.", objCampaign.CampaignID, objCampaign.ShortDescription, name);
                            }
                            catch (Exception ee)
                            {
                                Log.WriteException(ee, "Error in Remove Call Thread From List Remove At");
                            }
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Error in Remove Call Thread From List");
                throw ex;
            }
            finally
            {
            }
        }

        //-------------------------------------------------------------
        /// <summary>
        /// Validates call data with respect to dialparams
        /// </summary>
        /// <param name="objCallDetails"></param>
        /// <returns></returns>
        //-------------------------------------------------------------
        private bool ValidateCallData(CampaignDetails objCallDetails)
        {
            string msg = string.Empty;
            if (objCallDetails.PhoneNum == null || objCallDetails.PhoneNum == string.Empty)
            {
                msg = "Phone number is empty";
            }

            if (objCallDetails.ScheduleDate != DateTime.MinValue)
            {

                // check is it scheduled or not
                //if (objCallDetails.ScheduleDate > DateTime.Now)
                //{
                    msg = objCallDetails.PhoneNum + " is Scheduled - " +
                        objCallDetails.ScheduleDate.ToString("MM-dd-yyyy:hh:mm:ss");
                //}
            }

            //Fetch Dial Params invariably from DB
            while (true)
            {
                TimeSpan ts = new TimeSpan();
                ts = DateTime.Now - lastDialParamUpdate;
                if (ts.TotalSeconds > dialParamsInterval)
                {
                    UpdateDialParams();
                }
                if (terminateCampaignProcess)
                {
                    DialerEngine.Log.Write("|CP|{0}|{1}|Dial params verified. Dialing paused.", objCampaign.CampaignID, objCampaign.ShortDescription);

                    Thread.Sleep(10000);
                }
                else break;
            }

            int iNumberOfTimesDialed = 0;
            int iMaxAllowed = 0;
            if (CallType == CallType.AMCall)
            {
                iMaxAllowed = objDialParameter.AMCallTimes;
                if (objCallDetails.NumAttemptsAM != null)
                {
                    try
                    {
                        iNumberOfTimesDialed = Convert.ToInt32(objCallDetails.NumAttemptsAM);
                    }
                    catch { }
                }
                if (iMaxAllowed <= iNumberOfTimesDialed)
                {
                    msg = "Maximum AM Dials Over for this number : " + objCallDetails.PhoneNum;
                }
            }
            else if (CallType == CallType.PMCall)
            {
                iMaxAllowed = objDialParameter.PMCallTimes;
                if (objCallDetails.NumAttemptsPM != null)
                {
                    try
                    {
                        iNumberOfTimesDialed = Convert.ToInt32(objCallDetails.NumAttemptsPM);
                    }
                    catch { }
                }
                if (iMaxAllowed <= iNumberOfTimesDialed)
                {
                    msg = "Maximum PM Dials Over for this number : " + objCallDetails.PhoneNum;
                }
            }
            else if (CallType == CallType.WkendCall)
            {
                iMaxAllowed = objDialParameter.WeekendCallTimes;
                if (objCallDetails.NumAttemptsWkEnd != null)
                {
                    try
                    {
                        iNumberOfTimesDialed = Convert.ToInt32(objCallDetails.NumAttemptsWkEnd);
                    }
                    catch { }
                }
                if (iMaxAllowed <= iNumberOfTimesDialed)
                {
                    msg = "Maximum Weekend Dials Over for this number : " + objCallDetails.PhoneNum;
                }
            }

            if (msg != string.Empty)
            {
                DialerEngine.Log.Write("|CP|{0}|{1}|{2}.", objCampaign.CampaignID, objCampaign.ShortDescription, msg);
                return false;
            }

            // Check never call flag for this number as multiple queries can 
            // simultaneosly executed
            try
            {
                bool isNeverCallSet = CampaignAPI.IsNeverCallSet(objCampaign.CampaignDBConnString, objCallDetails.UniqueKey);
                if (isNeverCallSet)
                {
                    DialerEngine.Log.Write("|CP|{0}|{1}|Never call flag set for record ID {2}, probably by another query.", objCampaign.CampaignID, objCampaign.ShortDescription, objCallDetails.UniqueKey);
                    return false;
                }
            }
            catch { }

            return true;
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void AddQueryToMix(Query queryToAdd)
        {
            Queue<CampaignDetails> qCallQueue = null;
            if (objCampaign.DialAllNumbers)
            {
                // dial all numbers before implementing recycle in days
                qCallQueue = CampaignAPI.GetCallDetailsByQuery_Recyle_Last(objCampaign.CampaignDBConnString, queryToAdd.QueryCondition, objDialParameter, queryToAdd.QueryID);
            }
            else
            {
                string strQueryCond = CampaignAPI.PrepareDialerQuery(objCampaign, queryToAdd.QueryCondition, queryToAdd.QueryID);
                qCallQueue = CampaignAPI.GetCallDetailsByQuery(objCampaign, strQueryCond, objDialParameter);
            }
            if (qCallQueue != null)
            {
                DialerEngine.Log.Write("|CP|{0}|{1}|Query '{2}' has {3} available records.", objCampaign.CampaignID, objCampaign.ShortDescription, queryToAdd.QueryName, qCallQueue.Count);
            }

            // update query stats
            try
            {
                CampaignAPI.UpdateAvailableCountToQuery(objCampaign.CampaignDBConnString, qCallQueue.Count, queryToAdd);
            }
            catch { }
            if (qCallQueue != null && qCallQueue.Count > 0)
            {
                NoRecordsExist = false;
            }
            CallQueueList.Add(queryToAdd.QueryID, qCallQueue);
            Querys.Add(queryToAdd);
        }
        //-------------------------------------------------------------
        /// <summary>
        /// Update the dial params
        /// </summary>
        //-------------------------------------------------------------
        private void UpdateDialParams()
        {
            try
            {
                DialingParameter objDialParam = CampaignAPI.GetDialParam(objCampaign);
                if (objDialParam != null)
                {
                    objDialParameter = objDialParam;
                    this.CampStats.CallStatsWindow = objDialParameter.CallStatisticsWindow;

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
                    // Don:
                    //
                    // We ignore start and stop times for all
                    // modes other than unmanned.
                    //-------------------------------------------------
                    if (objDialParam.DialingMode != 6)
                    {
                        terminateCampaignProcess = false;
                    } 
                    else if (iCurrHour > iDPhour || ((iCurrHour == iDPhour) && (iCurrMinutes >= iDPMinutes)))
                    {
                        terminateCampaignProcess = false;

                        //---------------------------------------------
                        // Ignoring stop time for anything but unmanned.
                        //---------------------------------------------
                        if (objDialParam.DialingMode == 6)
                        {
                            try
                            {
                                if (DateTime.Now.Hour >= 12)
                                {
                                    DateTime dtStopTime = objDialParam.PMDialingStop;
                                    int iDPStophour = dtStopTime.Hour;
                                    int iDPStopMinutes = dtStopTime.Minute;
                                    if (iCurrHour > iDPStophour || ((iCurrHour == iDPStophour) && (iCurrMinutes >= iDPStopMinutes)))
                                    {
                                        Log.Write("Campaign PM Stop time reached");
                                        DialerEngine.Log.Write("|CP|{0}|{1}|PM stop time exceeded, terminating dialing.",
                                                               objCampaign.CampaignID, objCampaign.ShortDescription);
                                        terminateCampaignProcess = true;
                                    }
                                }
                                else
                                {
                                    // *** Added to impose AMStop time
                                    DateTime dtStopTime = objDialParam.AMDialingStop;
                                    int iDPStophour = dtStopTime.Hour;
                                    int iDPStopMinutes = dtStopTime.Minute;
                                    if (iCurrHour > iDPStophour || ((iCurrHour == iDPStophour) && (iCurrMinutes >= iDPStopMinutes)))
                                    {
                                        DialerEngine.Log.Write("|CP|{0}|{1}|AM stop time exceeded, terminating dialing.", objCampaign.CampaignID, objCampaign.ShortDescription);
                                        terminateCampaignProcess = true;
                                    }
                                }

                            }
                            catch { }
                        }


                        if (DateTime.Now.DayOfWeek == DayOfWeek.Saturday || DateTime.Now.DayOfWeek == DayOfWeek.Sunday)
                        {
                            callType = CallType.WkendCall;
                        }
                        this.CallType = callType;

                    }
                    else
                    {
                        terminateCampaignProcess = true;
                    }

                }

                DigitalizedRecording digRecording = CampaignAPI.GetDigitizedRecordings(objCampaign);
                if (digRecording != null)
                {
                    this.RecordingsPath = digRecording.RecordingFilePath;
                    this.RecordCalls = digRecording.EnableRecording;
                    this.RecordBeep = digRecording.StartWithABeep;
                }

            }
            catch { }

            try
            {
                if (objOtherParameter != null)
                {
                    OtherParameter objOtherParam = CampaignAPI.GetOtherParam(objCampaign);
                    if (objOtherParam != null)
                    {
                        objOtherParameter = objOtherParam;
                    }
                }
            }
            catch { }
            DialerEngine.Log.Write("Dialing params updated: ");
            DialerEngine.Log.Write("|CP|{0}|{1}|Dialing parameters updated to: Machine Detection {2}, Max Drop Rate {3}, Lines {4}, Ring Secs {5}, Min Delay {6}.", objCampaign.CampaignID, objCampaign.ShortDescription, objDialParameter.AnsMachDetect, objDialParameter.DropRatePercent, objDialParameter.PhoneLineCount, objDialParameter.RingSeconds, objDialParameter.MinimumDelayBetweenCalls);

        }

        // Reference to silent call counts
        private static Dictionary<string, int> dSilentCallCount = new Dictionary<string, int>();

        #endregion

        #region Public Methods
        // Update the current dial count for each campaign
        public static void UpdateCampaignDialCount(long campId, bool bIncrement)
        {
            DialerEngine.Log.Write("|CP|{0}|Updating dial counts.", campId);
            try
            {
                int count = 0;
                lock (dCampaignDialCount)
                {
                    if (dCampaignDialCount.ContainsKey(campId))
                    {
                        count = dCampaignDialCount[campId];
                        dCampaignDialCount.Remove(campId);
                    }
                    if (bIncrement)
                        count++;
                    else
                        count = (count == 0) ? 0 : (count - 1);
                    dCampaignDialCount.Add(campId, count);
                }
                //Log.Write("Updating dial counts,  {0} - {1} ", campId.ToString(), count);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "UpdateCampaignDialCount Exception");
            }
        }

        public static int GetSilentCallCount(string campName)
        {
            int count = 0;
            try
            {
                lock (dSilentCallCount)
                {
                    if (dSilentCallCount.ContainsKey(campName))
                    {
                        count = dSilentCallCount[campName];
                    }
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "In GetSilentCallCount");
            }
            return count;
        }

        public static void RemoveSilentCallCount(string campName)
        {
            try
            {
                lock (dSilentCallCount)
                {
                    if (campName == "-ALL-")
                    {
                        dSilentCallCount.Clear();
                    }
                    else if (dSilentCallCount.ContainsKey(campName))
                    {
                        dSilentCallCount.Remove(campName);
                    }
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "In RemoveSilentCallCount");
            }
        }

        public static void AddSilentCall(string campName)
        {
            int count = 1;
            try
            {
                lock (dSilentCallCount)
                {
                    if (dSilentCallCount.ContainsKey(campName))
                    {
                        count = dSilentCallCount[campName];
                        count++;
                        dSilentCallCount.Remove(campName);
                    }
                    dSilentCallCount.Add(campName, count);
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "In AddSilentCall");
            }
        }

        //added to fetch no of calls that are being dialled our curerntly
        public static int GetCampaignDialCount(long campId)
        {
            //Log.Write("GetCampaignDialCount Invoked");
            int count = 0;
            try
            {
                lock (dCampaignDialCount)
                {
                    if (dCampaignDialCount.ContainsKey(campId))
                    {
                        count = dCampaignDialCount[campId];
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "dCampaignDialCount Exception");
            }
            DialerEngine.Log.Write("|CP|{0}|Current dial count:{1}.", campId, count.ToString());
            return count;
        }

        public static void Dispose()
        {
            try
            {
                dCampaignDialCount.Clear();
            }
            catch { }
        } 
        #endregion

        #region Events

        protected virtual void campaignMonitor_campaignStateChangeEvent(object sender, EventArgs e)
        {
            CampaignStatus campaignState = (e as CampaignStateChangeEventArgs).status;

            DialerEngine.Log.Write("|CP|{0}|{1}|CAMPAIGN STATE CHANGE: {2}.", objCampaign.CampaignID, objCampaign.ShortDescription, campaignState.ToString());

            switch (campaignState)
            {
                case CampaignStatus.FlushIdle:
                    terminateCampaignProcess = true;
                    terminateCampaignProcessState = CampaignStatus.Idle;
                    break;
                case CampaignStatus.FlushPaused:
                    terminateCampaignProcess = true;
                    terminateCampaignProcessState = CampaignStatus.Pause;
                    break;
            }
        }

        protected virtual void campaignMonitor_queryStateChangeEvent(object sender, EventArgs e)
        {
            QueryState queryState = (e as QueryStateChangeEventArgs).state;
            Query query = (e as QueryStateChangeEventArgs).query;

            switch (queryState)
            {
                case QueryState.active:
                    AddQueryToMix(query);
                    break;
                case QueryState.inactive:
                    CallQueueList.Remove(query.QueryID);
                    Querys.RemoveAt
                    (
                        Querys.FindIndex(w => w.QueryID == query.QueryID)
                    );
                    break;
            }

            DialerEngine.Log.Write("|CP|{0}|{1}|QUERY STATE CHANGE: {2} -> {3}.", objCampaign.CampaignID, objCampaign.ShortDescription, query.QueryName, queryState.ToString());
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                Log.WriteException(ex, "Unhandled!");
            }
            else
            {
                Log.Write("Unhandled! No Object.");
            }
        }

        #endregion
    }

    public class CampaignStats
    {
        #region Variables and Properties
        // Reference to campaign
        private Campaign objCampaign = null;

        public CampaignStats(Campaign objCampaign)
        {
            this.objCampaign = objCampaign;
        }

        List<bool> callAnswerList = new List<bool>();
        public List<bool> CallAnswerList
        {
            get { return callAnswerList; }
            set { callAnswerList = value; }
        }
        List<int> callTimeList = new List<int>();
        public List<int> CallTimeList
        {
            get { return callTimeList; }
            set { callTimeList = value; }
        }
        private int callStatsWindow = 100;
        public int CallStatsWindow
        {
            get { return callStatsWindow; }
            set { callStatsWindow = value; }
        }
        #endregion

        #region Public Methods
        public void AddToAnswerList(bool bAnswered)
        {
            lock (this)
            {
                if (callAnswerList.Count == callStatsWindow)
                    callAnswerList.RemoveAt(0);
                callAnswerList.Add(bAnswered);
            }
            DialerEngine.Log.Write("|CS|{0}|{1}|Adding call to answer list with answered {2}", objCampaign.CampaignID, objCampaign.ShortDescription, bAnswered);
        }

        public void AddToACTList(int iInterval)
        {
            lock (this)
            {
                if (callTimeList.Count == callStatsWindow)
                    callTimeList.RemoveAt(0);
                callTimeList.Add(iInterval);
            }
        }

        public int GetNextCallTime(int iDelay, int iAgentCount, int defaultCallTime)
        {
            // TSC = AAIUT – ATTA + Delay
            // AAIUT = Average Agent in Use Time, ATTA = Average Time To Answer, Delay is DB setting (AnalyzeDelayFrequecy
            decimal dAAIUT = 0;
            decimal dATTA = 0;
            decimal dTSC = 0;

            lock (this)
            {
                dAAIUT = GetAAIUT();
                //if (dAAIUT > 0)
                dATTA = GetATTA();
                if (dAAIUT > dATTA)
                    dTSC = dAAIUT - dATTA + iDelay;
                else
                {
                    dTSC = iDelay;

                    try
                    {
                        //added condition to set Max of ACT, TSC
                        dTSC = Math.Max(dTSC, GetACT());
                    }
                    catch { }
                }

                DialerEngine.Log.Write("|CP|{0}|{1}|Basic algorithm vars: AAIUT - {2}, ATTA - {3}, Delay - {4}", objCampaign.CampaignID, objCampaign.ShortDescription, string.Format("{0:0.00}", dAAIUT), string.Format("{0:0.00}", dATTA), iDelay.ToString());

                // Default 40
                if (dAAIUT == 0)
                {


                    return defaultCallTime;
                }


                if (iAgentCount > 1)
                    dTSC /= iAgentCount;

                return Convert.ToInt32(Math.Floor(dTSC * 1000));
            }
        }

        public decimal GetAAIUT()
        {
            decimal dAAIUT = 0;
            List<AgentStat> lstAgentStats = AgentAPI.GetAgentStat(this.objCampaign.CampaignDBConnString, this.objCampaign.CampaignID);
            int iAgentStatCount = 0;
            if (lstAgentStats.Count > 0)
            {
                for (int i = 0; i < lstAgentStats.Count; i++)
                {
                    if (lstAgentStats[i].Calls > 0)
                    {
                        // stats exists for this agent
                        iAgentStatCount++;
                        dAAIUT += (lstAgentStats[i].TalkTime + lstAgentStats[i].WrapTime) / lstAgentStats[i].Calls;
                    }
                }
                if (iAgentStatCount > 0)
                {
                    // Calculate avarage of each agents AAIUT
                    dAAIUT /= iAgentStatCount;
                }
            }
            return dAAIUT;
        }

        public decimal GetATTA()
        {
            // ATTA = ACT / PA
            decimal dATTA = 0;
            try
            {
                if (callTimeList.Count == 0) return dATTA;

                decimal dACT = 0;
                try
                {
                    dACT = GetACT();
                }
                catch { }

                decimal dPA = 0;
                int iAnswers = 0;
                for (int i = 0; i < callAnswerList.Count; i++)
                {
                    if (callAnswerList[i])
                        iAnswers += 1;
                }

                // PA = Answers/NoAnswers
                if (callAnswerList.Count - iAnswers > 0)
                    dPA = iAnswers / (decimal)(callAnswerList.Count - iAnswers);

                dPA = (dPA == 0) ? 1 : dPA;
                if (dPA > 0)
                    dATTA = dACT / dPA;
            }
            catch (Exception ex)
            {
                if (!(ex is ThreadAbortException))
                    DialerEngine.Log.WriteException(ex, "GetATTA Error");
            }
            finally
            {
            }
            return dATTA;
        }

        public decimal GetACT()
        {
            decimal dACT = 0;
            for (int i = 0; i < callTimeList.Count; i++)
            {
                dACT += callTimeList[i];
            }
            if (callTimeList.Count > 0)
                dACT = dACT / callTimeList.Count;
            return dACT;
        } 
        #endregion

    }
}
