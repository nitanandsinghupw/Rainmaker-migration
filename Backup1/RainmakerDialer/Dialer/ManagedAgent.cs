using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Threading;

using VoiceElements.Common;
using VoiceElements.Client;

using Rainmaker.Common.DomainModel;
namespace Rainmaker.RainmakerDialer
{
    public class ManagedAgent
    {

        #region LocalData

        //private Campaign objCampaign = null;

        // Reference to ManagedChannel
        private ManagedChannel managedChannel = null;
        public ManagedChannel ManagedChannel
        {
            get { return managedChannel; }
            set { managedChannel = value; }
        }

        // Holds CampaignId
        private long campaignId;
        public long CampaignId
        {
            get { return campaignId; }
            set { campaignId = value; }
        }
        
        // Holds Agent details 
        private Agent agent;
        public Agent AgentDetails
        {
            get { return agent; }
            set { agent = value; }
        }

        // Holds Agentstatus
        private AgentStatus status;
        public AgentStatus Status
        {
            get { return status; }
            set { status = value; }
        }

        // Holds Agent available from
        private DateTime availableFrom;
        public DateTime AvailableFrom
        {
            get { return availableFrom; }
            set { availableFrom = value; }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="agent"></param>
        public ManagedAgent(Agent agent)
        {
            this.AgentDetails = agent;
            
        }

        #endregion

        #region Static Data

        // Reference to Managed Agents
        private static Dictionary<string, ManagedAgent> dManagedAgents = new Dictionary<string, ManagedAgent>();

        // Reference to Offhook Agents
        private static Dictionary<string, ManagedAgent> dOffhookAgents = new Dictionary<string, ManagedAgent>();

        // Reference to Available agent count per campaign 
        private static Dictionary<long, int> dAvailableAgentCountPerCamp = new Dictionary<long, int>();

        // Reference to Loggedin agent count per campaign 
        private static Dictionary<long, int> dLoggedinAgentCountPerCamp = new Dictionary<long, int>();

        // Likes list to track how verification agents have been used to implement "round robin" hunting for verif. agents
        private static LinkedList<long> dVerificationAgentRotation = new LinkedList<long>();

        // lock variable
        private static object s_SyncVar = new object();

        // Reference to log
        private static Log Log = DialerEngine.Log;

        #endregion

        #region Static Methods

        /// <summary>
        /// Initializes/Updates/Remove agents
        /// </summary>
        /// <param name="lstAgents"></param>
        public static void InitializeAgents(List<Agent> lstAgents)
        {

            //Sets Loggedin Agent counts
            SetLoggedinAgents(lstAgents);

            lock (s_SyncVar)
            {
                try
                {

                    //Remove loggedoff agents
                    RemoveLoggedoffAgents(lstAgents);

                    // Create update agents
                    for (int i = 0; i < lstAgents.Count; i++)
                    {

                        try
                        {
                            // Check agent is in standby list
                            if (dManagedAgents.ContainsKey(lstAgents[i].StationNumber))
                            {
                                // check if agent changed to other campaign
                                if (dManagedAgents[lstAgents[i].StationNumber].CampaignId
                                    != lstAgents[i].CampaignID)
                                {

                                    // decrement previous campaign count
                                    if (dManagedAgents[lstAgents[i].StationNumber].Status == AgentStatus.Available)
                                    {
                                        UpdateAgentCounts(dManagedAgents[lstAgents[i].StationNumber].CampaignId, dManagedAgents[lstAgents[i].StationNumber].AgentDetails.ReceiptModeID, false);
                                    }

                                    // increment this campaign count
                                    if (lstAgents[i].Status == AgentStatus.Available)
                                    {
                                        UpdateAgentCounts(lstAgents[i].CampaignID, lstAgents[i].ReceiptModeID, true);
                                        dManagedAgents[lstAgents[i].StationNumber].AvailableFrom = DateTime.Now;
                                    }
                                    dManagedAgents[lstAgents[i].StationNumber].Status = lstAgents[i].Status;
                                }
                                else
                                {
                                    // Check status changed from paused to available
                                    if (lstAgents[i].Status == AgentStatus.Available &&
                                        dManagedAgents[lstAgents[i].StationNumber].Status == AgentStatus.Paused)
                                    {
                                        UpdateAgentCounts(lstAgents[i].CampaignID, lstAgents[i].ReceiptModeID, true);
                                        dManagedAgents[lstAgents[i].StationNumber].AvailableFrom = DateTime.Now;
                                        dManagedAgents[lstAgents[i].StationNumber].Status = lstAgents[i].Status;
                                    }
                                    // Check status changed from available to paused
                                    else if (lstAgents[i].Status == AgentStatus.Paused &&
                                        dManagedAgents[lstAgents[i].StationNumber].Status == AgentStatus.Available)
                                    {
                                        UpdateAgentCounts(lstAgents[i].CampaignID, lstAgents[i].ReceiptModeID, false);
                                        dManagedAgents[lstAgents[i].StationNumber].Status = lstAgents[i].Status;
                                    }
                                }

                                dManagedAgents[lstAgents[i].StationNumber].CampaignId = lstAgents[i].CampaignID;
                                dManagedAgents[lstAgents[i].StationNumber].AgentDetails = lstAgents[i];
                                //dManagedAgents[lstAgents[i].StationNumber].Status = lstAgents[i].Status;

                                // Manual dialing set
                                dManagedAgents[lstAgents[i].StationNumber].SetManualDialing();
                            }

                            // Check agent is in offhook list
                            else if (dOffhookAgents.ContainsKey(lstAgents[i].StationNumber))
                            {
                                // check if agent changed to other campaign
                                if (dOffhookAgents[lstAgents[i].StationNumber].CampaignId
                                    != lstAgents[i].CampaignID)
                                {
                                    // decrement previus campaign count
                                    if (dOffhookAgents[lstAgents[i].StationNumber].Status == AgentStatus.Available)
                                    {
                                        UpdateAgentCounts(dOffhookAgents[lstAgents[i].StationNumber].CampaignId, dOffhookAgents[lstAgents[i].StationNumber].AgentDetails.ReceiptModeID, false);
                                    }

                                    // increment this campaign count
                                    if (lstAgents[i].Status == AgentStatus.Available)
                                    {
                                        UpdateAgentCounts(lstAgents[i].CampaignID, lstAgents[i].ReceiptModeID, true);
                                        dOffhookAgents[lstAgents[i].StationNumber].AvailableFrom = DateTime.Now;
                                    }
                                    dOffhookAgents[lstAgents[i].StationNumber].Status = lstAgents[i].Status;
                                }
                                else
                                {
                                    // Check status changed from paused to available
                                    if (lstAgents[i].Status == AgentStatus.Available &&
                                        dOffhookAgents[lstAgents[i].StationNumber].Status == AgentStatus.Paused)
                                    {
                                        UpdateAgentCounts(lstAgents[i].CampaignID, lstAgents[i].ReceiptModeID, true);
                                        dOffhookAgents[lstAgents[i].StationNumber].AvailableFrom = DateTime.Now;
                                        dOffhookAgents[lstAgents[i].StationNumber].Status = lstAgents[i].Status;
                                    }
                                    // Check status changed from available to paused
                                    else if (lstAgents[i].Status == AgentStatus.Paused &&
                                        dOffhookAgents[lstAgents[i].StationNumber].Status == AgentStatus.Available)
                                    {
                                        UpdateAgentCounts(lstAgents[i].CampaignID, lstAgents[i].ReceiptModeID, false);
                                        dOffhookAgents[lstAgents[i].StationNumber].Status = lstAgents[i].Status;
                                    }
                                }
                                dOffhookAgents[lstAgents[i].StationNumber].CampaignId = lstAgents[i].CampaignID;
                                dOffhookAgents[lstAgents[i].StationNumber].AgentDetails = lstAgents[i];
                                //dOffhookAgents[lstAgents[i].StationNumber].Status = lstAgents[i].Status;

                                // Manual dialing set
                                dOffhookAgents[lstAgents[i].StationNumber].SetManualDialing();
                            }
                            else
                            {
                                // new agent, create and add it to standby list
                                ManagedAgent managedAgent = new ManagedAgent(lstAgents[i]);
                                long campId = lstAgents[i].CampaignID;
                                managedAgent.CampaignId = campId;
                                if (lstAgents[i].Status == AgentStatus.Paused)
                                    managedAgent.Status = AgentStatus.Paused;
                                else
                                {
                                    managedAgent.Status = AgentStatus.Available;
                                    UpdateAgentCounts(campId, managedAgent.AgentDetails.ReceiptModeID, true);
                                    managedAgent.AvailableFrom = DateTime.Now;
                                }

                                
                                if (managedAgent.AgentDetails.AllwaysOffHook)
                                {
                                    // added to offhook agent
                                    managedAgent.AllwaysOffHookAgent();
                                }
                                else
                                {
                                    // added to standby agents
                                    dManagedAgents.Add(lstAgents[i].StationNumber, managedAgent);
                                }

                                // Manual dialing set
                                managedAgent.SetManualDialing();
                            }
                        }
                        catch (Exception exx)
                        {
                            Log.WriteException(exx, "InitializeAgents Exception-1");
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "InitializeAgents Exception");
                }
            }

            // Ring standby agents and move them to off hook
            if(lstAgents.Count > 0)
                RingStandbyAgents(-1);
        }

        /// <summary>
        /// Updates available agent counts
        /// </summary>
        /// <param name="campId"></param>
        /// <param name="bIncrement"></param>
        private static void UpdateAgentCounts(long campId, long agentReceiptMode, bool bIncrement)
        {
            // *** Prevent Verify only agents from being counted
            Log.Write("|MA|{0}|Updating agent count for agent receipt type {1}.", campId, agentReceiptMode);
            if (agentReceiptMode == (long)AgentCallReceiptMode.VerifyOnly)
            {
                return;
            }
            try
            {
                int count = 0;
                lock (dAvailableAgentCountPerCamp)
                {
                    if (dAvailableAgentCountPerCamp.ContainsKey(campId))
                    {
                        count = dAvailableAgentCountPerCamp[campId];
                        dAvailableAgentCountPerCamp.Remove(campId);
                    }
                    if (bIncrement)
                        count++;
                    else
                        count = (count == 0) ? 0 : (count - 1);
                    dAvailableAgentCountPerCamp.Add(campId, count);
                }
                Log.Write("|MA|{0}|Updating agent counts for campaign {0} to {1} ", campId.ToString(), count);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "UpdateAgentCounts Exception");
            }
        }

        /// <summary>
        /// Returns available agent count for the campaign
        /// </summary>
        /// <param name="campId"></param>
        /// <returns></returns>
        public static int GetAvailableAgentCount(long campId)
        {
            int count = 0;
            try
            {
                lock (dAvailableAgentCountPerCamp)
                {
                    if (dAvailableAgentCountPerCamp.ContainsKey(campId))
                    {
                        count = dAvailableAgentCountPerCamp[campId];
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "GetAvailableAgentCount Exception");
            }
            Log.Write("|MA|{0}|Campaign available agent count {1}.", campId.ToString(), count.ToString());
            return count;
        }

        /// <summary>
        /// Returns logged agent count for the campaign
        /// </summary>
        /// <param name="campId"></param>
        /// <returns></returns>
        public static int GetLoggedInAgentCount(long campId)
        {
           
            int count = 0;
            try
            {
                lock (dLoggedinAgentCountPerCamp)
                {
                    if (dLoggedinAgentCountPerCamp.ContainsKey(campId))
                    {
                        count = dLoggedinAgentCountPerCamp[campId];
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "GetLoggedInAgentCount Exception");
            }
#if DEBUG
            Log.Write("|MA|{0}|Campaign logged in agent count {1}.", campId.ToString(), count.ToString());
#endif
            return count;
        }

        /// <summary>
        /// Update Loggedin agents
        /// </summary>
        /// <param name="listAgents"></param>
        private static void SetLoggedinAgents(List<Agent> lstAgents)
        {
            try
            {
                lock (dLoggedinAgentCountPerCamp)
                {
                    dLoggedinAgentCountPerCamp.Clear();
                    for (int i = 0; i < lstAgents.Count; i++)
                    {
                        if (lstAgents[i].Status != AgentStatus.Paused && lstAgents[i].ReceiptModeID != (long)AgentCallReceiptMode.VerifyOnly) // *** Trap to not count verification only agents
                        {
                            UpdateLoggedinAgentCounts(lstAgents[i].CampaignID, true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "UpdateLoggedinAgents Exception");
            }
        }

        /// <summary>
        /// Updates loggedin agent counts
        /// </summary>
        /// <param name="campId"></param>
        /// <param name="bIncrement"></param>
        private static void UpdateLoggedinAgentCounts(long campId, bool bIncrement)
        {
            try
            {
                //lock (dLoggedinAgentCountPerCamp)
                //{ 
                int count = 0;
                if (dLoggedinAgentCountPerCamp.ContainsKey(campId))
                {
                    count = dLoggedinAgentCountPerCamp[campId];
                    dLoggedinAgentCountPerCamp.Remove(campId);
                }
                if (bIncrement)
                    count++;
                else
                    count = (count == 0) ? 0 : (count - 1);
                dLoggedinAgentCountPerCamp.Add(campId, count);

                Log.Write("|MA|{0}|Updating logged in agent counts to {1}.", campId.ToString(), count);
                // }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "UpdateLoggedinAgentCounts Exception");
            }
        }

        /// <summary>
        /// Get offhook agent
        /// </summary>
        /// <param name="campId"></param>
        /// <returns></returns>
        public static ManagedAgent GetFastestAgent(long campId, bool isTransferedCall)
        {
            Log.Write("|MA|{0}|Get Fastest Agent routine invoked, transferred flag set to {1}.", campId, isTransferedCall);
            ManagedAgent ma = null;
            lock (s_SyncVar)
            {
                try
                {
                    if (dOffhookAgents.Count > 0)
                    {
                        foreach (KeyValuePair<string, ManagedAgent> pair in dOffhookAgents)
                        {
                            if (pair.Value.CampaignId == campId && pair.Value.ManagedChannel != null
                                && pair.Value.Status == AgentStatus.Available)
                            {

                                if (!isTransferedCall)
                                {
                                    if (pair.Value.AgentDetails.ReceiptModeID != (long)AgentCallReceiptMode.VerifyOnly)
                                    {
                                        ManagedAgent managedAgent = pair.Value;
                                        if (ma == null)
                                        {
                                            ma = managedAgent;
                                        }
                                        else
                                        {
                                            if (ma.AvailableFrom > managedAgent.AvailableFrom)
                                            {
                                                ma = managedAgent;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Log.Write("|MA|{0}|Transfer checking agent {1}, in receipt mode {2}.", campId, pair.Value.AgentDetails.AgentName, pair.Value.AgentDetails.ReceiptModeID);
                                    if (pair.Value.AgentDetails.ReceiptModeID == (long)AgentCallReceiptMode.VerifyOnly || pair.Value.AgentDetails.ReceiptModeID == (long)AgentCallReceiptMode.VerifyOnly)
                                    {
                                        ManagedAgent managedAgent = pair.Value;
                                        if (ma == null)
                                        {
                                            ma = managedAgent;
                                        }
                                        else
                                        {
                                            if (ma.AvailableFrom > managedAgent.AvailableFrom)
                                            {
                                                ma = managedAgent;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (ma != null)
                        {
                            ma.Status = AgentStatus.Busy;
                            UpdateAgentCounts(campId, ma.AgentDetails.ReceiptModeID, false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "GetFastestAgent Exception");
                }
            }
            return ma;
        }

        /// <summary>
        /// Ring Standby Agents
        /// </summary>
        /// <param name="campId">-1 indicates dial all logged in agents, else ring specific campaign agents</param>
        public static void RingStandbyAgents(long campId)
        {
            lock (s_SyncVar)
            {
                try
                {
                    foreach (KeyValuePair<string, ManagedAgent> pair in dManagedAgents)
                    {
                        if (pair.Value.Status == AgentStatus.Available)
                        {
                            Log.Write("|MA|{0}|Checking Managed Agent Pair: '{1}' at station {2}", campId, pair.Value.AgentDetails.AgentName, pair.Value.AgentDetails.StationNumber);  

                            // dial only current campaign agents
                            // Modified for remote agent functionality, 
                            // We dial remote agents(non msi), and move them to offhook
                            bool isMsiAgent = ManagedAgent.IsMsiAgent(pair.Value.AgentDetails.StationNumber);
                            if (pair.Value.CampaignId == campId || (campId == -1 && (!isMsiAgent)))
                            {
                                // pick available agents
                                if (pair.Value.Status == AgentStatus.Available)
                                {
                                    Log.Write("|MA|{0}|Agent Available: '{1}' at station {2}", campId, pair.Value.AgentDetails.AgentName, pair.Value.AgentDetails.StationNumber);  
                                    try
                                    {
                                        bool bChannelsAvailable = false;
                                        
                                        if (isMsiAgent)
                                        {
                                            bChannelsAvailable = ManagedChannel.IsMsiChannelAvailable(
                                                pair.Value.AgentDetails.StationNumber);
                                        }
                                        else
                                        {
                                            bChannelsAvailable = ManagedChannel.IsChannelsAvailable();
                                        }
                                        if (bChannelsAvailable)
                                        {
                                            ManagedAgent managedAgent = pair.Value;

                                            // Update status to busy, dial the agent
                                            dManagedAgents[pair.Key].Status = AgentStatus.Busy;
                                            UpdateAgentCounts(pair.Value.CampaignId, pair.Value.AgentDetails.ReceiptModeID, false);
                                            Thread t = new Thread(managedAgent.DialAgent);
                                            t.Name = managedAgent.AgentDetails.StationNumber + "_DialAgnt";
                                            t.IsBackground = true;
                                            t.Start();
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        Log.WriteException(ex, "RingStandbyAgents Exception1");
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "RingStandbyAgents Exception");
                }
            }
        }

        /// <summary>
        /// Adds agent to offhook list
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ma"></param>
        public static void AddAgentToOffhook(string key, ManagedAgent ma, bool isAvailable)
        {
            Log.Write("|MA|{0}|{1}|Add agent to off hook invoked.", ma.agent.CampaignID, ma.agent.AgentName);
            // play beep on hangup when agent is reset
            ma.PlayBeep();
            lock (s_SyncVar)
            {
                try
                {
                    // firsttime
                    if (!dOffhookAgents.ContainsKey(key))
                    {
                        //  remove from standby list
                        dManagedAgents.Remove(key);

                        // add it to offhook list
                        dOffhookAgents.Add(key, ma);
                    }
                    if (isAvailable)
                    {
                        if (ma.Status == AgentStatus.Busy)
                        {
                            dOffhookAgents[key].Status = AgentStatus.Available;
                            dOffhookAgents[key].AvailableFrom = DateTime.Now;
                            // Added to available list so increment the count
                            UpdateAgentCounts(ma.CampaignId, ma.AgentDetails.ReceiptModeID, true);
                            Log.Write("|MA|{0}|{1}|Agent has been set to available.", ma.agent.CampaignID, ma.agent.AgentName);
                        }
                    }
                    else
                    {
                        // wait until agent sets disposition
                        Log.Write("|MA|{0}|{1}|Agent has been set to paused, waiting for disposition.", ma.agent.CampaignID, ma.agent.AgentName);
                        dOffhookAgents[key].Status = AgentStatus.Paused;
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "AddAgentToOffhook Exception");
                }
            }

            //Poke the Agent Request Thread
            AgentRequest.AgentRequestEvent.Set();
        }

        /// <summary>
        /// Get agent by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static ManagedAgent GetOffhookAgentByKey(string key)
        {
            ManagedAgent managedAgent = null;
            lock (dOffhookAgents)
            {
                try
                {
                    managedAgent = dOffhookAgents[key];
                }
                catch { }
            }
            return managedAgent;
        }

        /// <summary>
        /// Remove agent from offhook and add it to statndby list
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ma"></param>
        public static void RemoveAgentFromOffhook(string key, ManagedAgent ma)
        {
            Log.Write("|MA|{0}|{1}|Remove agent from off hook invoked.", ma.agent.CampaignID, ma.agent.AgentName);
            try
            {
                lock (s_SyncVar)
                {
                    if (dOffhookAgents.ContainsKey(key))
                    {
                        if (dOffhookAgents[key].Status == AgentStatus.Busy)
                        {
                            //ma.Status = AgentStatus.Available;
                            //UpdateAgentCounts(ma.CampaignId, true);

                            // pause agent until disposes previous call disposed  
                            ma.Status = AgentStatus.Paused;
                            Log.Write("|MA|{0}|{1}|Agent has been set to paused from busy, waiting for disposition.", ma.agent.CampaignID, ma.agent.AgentName);
                        }
                        dOffhookAgents.Remove(key);
                        ma.ManagedChannel = null;
                        dManagedAgents.Add(key, ma);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "RemoveAgentFromOffhook Exception");
            }
        }

        /// <summary>
        /// Upadate status of managed agent
        /// </summary>
        /// <param name="key"></param>
        /// <param name="ma"></param>
        /// <param name="isStandBy"></param>
        public static void UpdateAgentStatusToAvailable(string key, ManagedAgent ma, bool isStandBy)
        {
            Log.Write("|MA|{0}|{1}|Update agent to available invoked for '{2}'.", ma.agent.CampaignID, ma.agent.AgentName);
            if (isStandBy)
            {
                // standby agents status
                lock (dManagedAgents)
                {
                    try
                    {
                        dManagedAgents[key].Status = AgentStatus.Available;
                        UpdateAgentCounts(ma.CampaignId, ma.AgentDetails.ReceiptModeID, true);
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "UpdateAgentStatusToAvailable Exception");
                    }
                }
            }
            else
            {
                // Offhook agents status
                lock (dOffhookAgents)
                {
                    try
                    {
                        if (dOffhookAgents[key].Status != AgentStatus.Available)
                        {
                            dOffhookAgents[key].Status = AgentStatus.Available;
                            UpdateAgentCounts(ma.CampaignId, ma.AgentDetails.ReceiptModeID, true);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "UpadteAgentStatusToAvailable Exception");
                    }
                }
            }
        }

        /// <summary>
        /// Check Agent Loggedin
        /// </summary>
        /// <param name="sAgentNumber"></param>
        /// <returns></returns>
        public static ManagedAgent CheckAgentLoggedIn(string sAgentNumber)
        {
            Log.Write("|MA|CheckAgentLoggedIn Invoked");
            try
            {
                lock (s_SyncVar)
                {
                    if (dManagedAgents.ContainsKey(sAgentNumber))
                    {
                        return dManagedAgents[sAgentNumber];
                    }
                    else if (dOffhookAgents.ContainsKey(sAgentNumber))
                    {
                        return dOffhookAgents[sAgentNumber];
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "CheckAgentLoggedIn Exception");
            }
            return null;
        }

        /// <summary>
        /// Checks if all threads finished their execution or not
        /// </summary>
        /// <param name="threads"></param>
        /// <returns></returns>
        private static bool CheckAllThreadsFinished(List<Thread> threads)
        {
            bool bAllThreadsFinishd = true;
            lock (s_SyncVar)
            {
                try
                {
                    for (int i = 0; i < threads.Count; i++)
                    {
                        if (threads[i].IsAlive)
                        {
                            bAllThreadsFinishd = false;
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "CheckAllThreadsFinished Exception");
                }
            }
            return bAllThreadsFinishd;
        }

        /// <summary>
        /// Removes Loggedoff agents
        /// </summary>
        /// <param name="listAgents"></param>
        private static void RemoveLoggedoffAgents(List<Agent> lstAgents)
        {
            try
            {
                List<string> lstAgentNumbers = new List<string>();
                for (int i = 0; i < lstAgents.Count; i++)
                {
                    lstAgentNumbers.Add(lstAgents[i].StationNumber);
                }

                // Remove logged off agents from standby list
                RemoveLoggedoffAgents(lstAgentNumbers, dManagedAgents);

                // Remove logged off agents from offhook list
                RemoveLoggedoffAgents(lstAgentNumbers, dOffhookAgents);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "RemoveLoggedoffAgents Exception");
            }

        }

        /// <summary>
        /// Removes Loggedoff agents from the specified list
        /// </summary>
        /// <param name="lstAgents"></param>
        private static void RemoveLoggedoffAgents(List<string> lstAgentNumbers,
                    Dictionary<string, ManagedAgent> dManagedAgents)
        {
            List<string> lstLogoffAgents = new List<string>();
            foreach (KeyValuePair<string, ManagedAgent> pair in dManagedAgents)
            {
                try
                {
                    if (!lstAgentNumbers.Contains(pair.Value.AgentDetails.StationNumber))
                    {
                        if (pair.Value.ManagedChannel != null)
                        {
                            ManagedChannel.ReturnChannel(pair.Value.ManagedChannel, pair.Value.CampaignId);
                            //if (pair.Value.AgentDetails.AllwaysOffHook)
                            //{
                            pair.Value.LoggedOff = true;
                            pair.Value.EndManualDial = true;
                            //}
                            //else
                            //    pair.Value.ManagedChannel = null;
                        }
                        if (dManagedAgents[pair.Key].Status == AgentStatus.Available)
                        {
                            UpdateAgentCounts(pair.Value.CampaignId, pair.Value.AgentDetails.ReceiptModeID, false);
                        }
                        lstLogoffAgents.Add(pair.Key);
                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "RemoveLoggedoffAgents-1 Exception");
                }
            }

            for (int i = 0; i < lstLogoffAgents.Count; i++)
            {
                try
                {
                    dManagedAgents.Remove(lstLogoffAgents[i]);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "RemoveLoggedoffAgents-2 Exception");
                }
            }
            lstLogoffAgents.Clear();

        }

        /// <summary>
        /// Dispose
        /// </summary>
        public static void Dispose()
        {
            Log.Write("|MA|Managed Agent dispose invoked");
            dManagedAgents.Clear();
            try
            {
                foreach (KeyValuePair<string, ManagedAgent> pair in dOffhookAgents)
                {
                    if (pair.Value.ManagedChannel != null)
                    {
                        try
                        {
                            pair.Value.ManagedChannel.ChannelResource.Disconnect();
                            pair.Value.ManagedChannel.ChannelResource.Dispose();
                        }
                        catch { }
                    }
                }
            }
            catch { }
            try
            {
                dOffhookAgents.Clear();
                dAvailableAgentCountPerCamp.Clear();
                AgentRequest.DisposeAgentRequest();
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snumber"></param>
        /// <returns></returns>
        public static bool IsMsiAgent(string snumber) 
        {
            if (snumber.IndexOf("msi") == 0)
            {
                return true;
            }
            return false;
        }

        #endregion

        public AutoResetEvent threadEvent = new AutoResetEvent(false);

        private bool m_Disconnected = false;

        public bool Disconnected
        {
            get { return m_Disconnected; }
            set
            {
                m_Disconnected = value;
                threadEvent.Set();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mc"></param>
        public void InboundAgent(ManagedChannel mc)
        {
            m_Disconnected = false;
            m_LoggedOff = false;
            ManagedChannel = mc;
            ManagedChannel.Connected = true;

            Log.Write("|MA|{0}|{1}|Inbound agent invoked.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName);

            try
            {
                ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
            }
            catch { }
            ManagedChannel.ChannelResource.Disconnected += new Disconnected(ChannelResource_Disconnected);

            try
            {
                ManagedAgent.AddAgentToOffhook(this.AgentDetails.StationNumber, this, true);

                threadEvent.Reset();

                double beepInterval = 15.0;
                try { beepInterval = Convert.ToDouble(Utilities.GetAppSetting("BeepMessageInterval", "15")); }
                catch { }
                DateTime nextBeep = DateTime.Now.AddSeconds(beepInterval);
                while (true)
                {
                    if (LoggedOff || Disconnected || !(DialerEngine.Connected)) return;

                    //Im Not connected to anyone if Dialer == null
                    if (Dialer == null && ManualDialer == null)
                    {
                        // if Im here, Im offhook and available.
                        if (DateTime.Now > nextBeep)
                        {
                            if (m_VoiceResourceThread == null)
                            {
                                ThreadStart ts = new ThreadStart(PlayWaitBeep);
                                m_VoiceResourceThread = new Thread(ts);
                                m_VoiceResourceThread.IsBackground = true;
                                m_VoiceResourceThread.Name = ManagedChannel.ChannelResource.DeviceName + ".PlayWait";
                                m_VoiceResourceThread.Start();
                                nextBeep = DateTime.Now.AddSeconds(beepInterval);
                            }
                        }
                    }

                    threadEvent.WaitOne(2000, false);

                }
            }
            catch (HangupException)
            {
                Log.Write("|MA|Agent Hungup.");
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Inbound Agent");
            }
            finally
            {
                //Route agent Channel Back To its Voice Resource
                if (DialerEngine.Connected)
                {
                    try
                    {
                        if (ManagedChannel.ChannelResource is MsiChannel)
                        {
                            ManagedChannel.ChannelResource.StopListening();                 
                        }
                        else
                        {
                            ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                        }

                        ManagedChannel.ChannelResource.Disconnect();
                        ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                        ManagedChannel = null;
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "Inbound Agent");
                    }
                    ManagedAgent.RemoveAgentFromOffhook(this.AgentDetails.StationNumber, this);
                }
            }
        }


        private bool m_LoggedOff;
        public bool LoggedOff
        {
            get { return m_LoggedOff; }
            set
            {
                m_LoggedOff = value;
                if (m_LoggedOff)
                {
                    // logs off so terminate the call
                    Dialer temp = this.Dialer;
                    if (temp != null)
                    {
                        temp.TerminateCall = true;
                    }
                }
                threadEvent.Set();
            }
        }


        private Thread m_AllwaysOffhookThread = null;

        /// <summary>
        /// 
        /// </summary>
        public void AllwaysOffHookAgent()
        {
            
            if (m_AllwaysOffhookThread == null)
            {
                ThreadStart ts = new ThreadStart(AddAllwaysOffHookAgent);
                m_AllwaysOffhookThread = new Thread(ts);
                m_AllwaysOffhookThread.IsBackground = true;
                m_AllwaysOffhookThread.Name = "AllwaysOffhook";
                m_AllwaysOffhookThread.Start();
            }
        }


        /// <summary>
        /// Adds allways offhook agent
        /// </summary>
        public void AddAllwaysOffHookAgent()
        {

            Log.Write("|MA|{0}|{1}|Always off hook agent logged in.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName);
            ManagedChannel mc = ManagedChannel.GetMsiChannel(this.AgentDetails.StationNumber);

            //For  testing
            //ManagedChannel mc = ManagedChannel.GetOutboundChannel(this.CampaignId);
            if (mc == null)
            {
                Log.Write("|MA|{0}|{1}| MsiChannel({2}) not available for always off hook routine.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName, this.AgentDetails.StationNumber);
                UpdateAgentCounts(this.CampaignId, this.AgentDetails.ReceiptModeID, false);
                return;
            }

            m_LoggedOff = false;
            ManagedChannel = mc;
            ManagedChannel.Connected = true;
            try
            {
                ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
            }
            catch { }
            ManagedChannel.ChannelResource.Disconnected += new Disconnected(ChannelResource_Disconnected);

            try
            {
                ManagedAgent.AddAgentToOffhook(this.AgentDetails.StationNumber, this, true);

                threadEvent.Reset();

                double beepInterval = 15.0;
                try { beepInterval = Convert.ToDouble(Utilities.GetAppSetting("BeepMessageInterval", "15")); }
                catch { }
                DateTime nextBeep = DateTime.Now.AddSeconds(beepInterval);
                while (true)
                {
                    if (LoggedOff || !(DialerEngine.Connected)) return;

                    //Im Not connected to anyone if Dialer == null
                    if (Dialer == null && ManualDialer == null)
                    {
                        // if Im here, Im offhook and available.
                        if (DateTime.Now > nextBeep)
                        {
                            if (m_VoiceResourceThread == null)
                            {
                                ThreadStart ts = new ThreadStart(PlayWaitBeep);
                                m_VoiceResourceThread = new Thread(ts);
                                m_VoiceResourceThread.IsBackground = true;
                                m_VoiceResourceThread.Name = ManagedChannel.ChannelResource.DeviceName + ".PlayWait";
                                m_VoiceResourceThread.Start();
                                nextBeep = DateTime.Now.AddSeconds(beepInterval);
                            }
                        }
                    }

                    threadEvent.WaitOne(2000, false);

                }
            }
            catch (HangupException)
            {
                Log.Write("|MA|{0}|{1}|Always off hook agent hung up.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName);
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "AllwaysOffHookAgent");
            }
            finally
            {
                //Route agent Channel Back To its Voice Resource
                if (DialerEngine.Connected)
                {
                    try
                    {
                        Log.Write("|MA|{0}|{1}|Always off hook agent logged off.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName);
                        if (ManagedChannel.ChannelResource is MsiChannel)
                        {
                            ManagedChannel.ChannelResource.StopListening();                 
                        }
                        else
                        {
                            ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                        }

                        ManagedChannel.ChannelResource.Disconnect();
                        ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                        ManagedChannel = null;
                    }
                    catch (Exception ex)
                    {
                        Log.WriteException(ex, "AllwaysOffHookAgent");
                    }
                    //ManagedAgent.RemoveAgentFromOffhook(this.AgentDetails.StationNumber, this);
                }
                m_AllwaysOffhookThread = null;
            }
        }

        private Thread m_VoiceResourceThread = null;

        /// <summary>
        /// Play Wait Beep
        /// </summary>
        public void PlayBeep()
        {
            try
            {
                if (managedChannel == null)
                {
                    return;
                }
                Log.Write("|MA|{0}|{1}|Playing beep on station.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName, this.managedChannel.ChannelResource.DeviceName);
                if (this.ManagedChannel.ChannelResource is MsiChannel)
                {
                    MsiChannel msi = this.ManagedChannel.ChannelResource as MsiChannel;
                    if (msi != null)
                    {
                        // Stop Listening to the voice resource, this unroutes the device.
                        msi.StopListening();
                        TerminationCode tc = msi.PlayZipTone();
                        
                        // Route Back to the Voice Resource
                        //msi.RouteFull(msi.VoiceResource);
                        Log.Write("|MA|{0}|{1}|Play beep termination code {2}.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName, tc);
                
                    }
                }
                else
                {
                    this.ManagedChannel.ChannelResource.VoiceResource.PlayTone(950, -10, 25);
                }
                
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "Play Beep Exception");
            }
        }

        /// <summary>
        /// Play Wait Beep
        /// </summary>
        private void PlayWaitBeep()
        {
            try
            {
                
                if (Utilities.GetAppSetting("PlayBeepMessage", "yes").ToLower() == "yes")
                {
                    // Play wait beep
                     
                    if (this.ManagedChannel.ChannelResource is MsiChannel)
                    {
                        MsiChannel msi = this.ManagedChannel.ChannelResource as MsiChannel;
                        if (msi != null)
                        {
                            // Stop Listening to the voice resource, this unroutes the device.
                            msi.StopListening();
                            TerminationCode tc = msi.PlayZipTone();
                            Log.Write("|MA|{0}|{1}|Play wait beep termination code {2}.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName, tc);
                
                            // Route Back to the Voice Resource
                            //msi.RouteFull(msi.VoiceResource);                            
                        }
                    }
                    else
                    {
                        this.ManagedChannel.ChannelResource.VoiceResource.PlayTone(1000, -30, 50);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "PlayWaitBeep Exception");
            }
            finally
            {
                m_VoiceResourceThread = null;
            }
        }

        /// <summary>
        /// Plays Connection message
        /// </summary>
        public void PlayConnectingCustomer()
        {
            try
            {
                while (m_VoiceResourceThread != null) Thread.Sleep(100);

                if (this.ManagedChannel.ChannelResource is MsiChannel)
                {
                    MsiChannel msi = this.ManagedChannel.ChannelResource as MsiChannel;
                    if (msi != null)
                    {
                        //Stop Listening to the voice resource, this unroutes the device.
                        msi.StopListening();
                        
                        int beepsCount = 1;
                        try
                        {
                            beepsCount = Convert.ToInt32(Utilities.GetAppSetting("CallNotificationBeepsCount", "1").Trim());
                        }
                        catch { }              
                        for (int i = 0; i < beepsCount; i++)
                        {
                            TerminationCode tc = msi.PlayZipTone();
                            Log.Write("|MA|{0}|{1}|Play connecting zip tone termination code {2}.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName, tc);
                        }
                        // Route Back to the Voice Resource
                        //msi.RouteFull(msi.VoiceResource);                                 
                    }
                }
                else
                {
                    int beepsCount = 1;
                    try
                    {
                        beepsCount = Convert.ToInt32(Utilities.GetAppSetting("CallNotificationBeepsCount", "1").Trim());
                    }
                    catch { }             
                    for (int i = 0; i < beepsCount; i++)
                    {
                        TerminationCode tc = this.ManagedChannel.ChannelResource.VoiceResource.PlayTone(950, -10, 25);
                        Log.Write("|MA|{0}|{1}|Play connecting play tone termination code {2}.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName, tc);
                        
                    }
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "PlayConnectingCustomer Exception");
            }
        }

        private void ChannelResource_Disconnected(object sender, DisconnectedEventArgs e)
        {

            Log.Write("|MA|{0}|{1}|Managed agent hung up.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName);

            try
            {
                if (ManualDialer != null)
                {
                    ManualDialer = null;
                }
            }
            catch { }

           
            if (!this.AgentDetails.AllwaysOffHook)
            {
                try
                {
                    if (this.ManagedChannel != null)
                        ManagedChannel.Connected = false;
                }
                catch { }


                Disconnected = true;
                Dialer temp = this.Dialer;
                if (temp != null)
                {
                    temp.TerminateCall = true;
                }
            }
            else
            {
                Log.Write("|MA|Always Off Hook station disconnected : {0}", this.AgentDetails.StationNumber);
            }

        }


        public void DialAgent()
        {

            m_Disconnected = false;

            m_LoggedOff = false;

            bool bAgentConnected = false;

            if (ManagedAgent.IsMsiAgent(this.AgentDetails.StationNumber)) 
            {
                ManagedChannel = ManagedChannel.GetMsiChannel(this.AgentDetails.StationNumber);
            }
            else
            {
                ManagedChannel = ManagedChannel.GetOutboundChannel(0);
            }

            if (ManagedChannel != null && ManagedChannel.ChannelResource != null)
            {

                try
                {
                    ManagedChannel.ChannelResource.Disconnected -= new Disconnected(ChannelResource_Disconnected);
                }
                catch { }
                ManagedChannel.ChannelResource.Disconnected += new Disconnected(ChannelResource_Disconnected);

                try
                {
                    // dial the agent
                    Log.Write("|MA|{0}|{1}|Dialing agent at {2}.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName, this.AgentDetails.StationNumber); 

                    ManagedChannel.ChannelResource.OriginatingPhoneNumber = this.AgentDetails.OutboundCallerID;
                    ManagedChannel.ChannelResource.MaximumTime = 30;

                    //Make sure we are routed to the voice resource!
                    if (ManagedChannel.ChannelResource is T1Channel)
                    {
                        ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                    }

                    DialResult dr1 = ManagedChannel.ChannelResource.Dial(this.AgentDetails.StationNumber);

                    Log.Write("|MA|{0}|{1}|Dial agent at {2} returns {3}.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName, this.AgentDetails.StationNumber, dr1.ToString());
                    switch (dr1)
                    {
                        case DialResult.Connected:
                        case DialResult.HumanDetected:
                        case DialResult.Successful:
                            bAgentConnected = true;
                            ManagedChannel.Connected = true;
                            Log.Write("|MA|{0}|{1}|Dial agent connected.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName);

                            ManagedAgent.AddAgentToOffhook(this.AgentDetails.StationNumber, this, true);

                            threadEvent.Reset();

                            double beepInterval = 15.0;
                            try { beepInterval = Convert.ToDouble(Utilities.GetAppSetting("BeepMessageInterval", "15")); }
                            catch { }
                            DateTime nextBeep = DateTime.Now.AddSeconds(beepInterval);
                            while (true)
                            {
                                if (LoggedOff || Disconnected || !(DialerEngine.Connected)) break;

                                //Im Not connected to anyone if Dialer == null
                                if (Dialer == null && ManualDialer == null)
                                {
                                    // if Im here, Im offhook and available.
                                    if (DateTime.Now > nextBeep)
                                    {
                                        if (m_VoiceResourceThread == null)
                                        {
                                            ThreadStart ts = new ThreadStart(PlayWaitBeep);
                                            m_VoiceResourceThread = new Thread(ts);
                                            m_VoiceResourceThread.Name = ManagedChannel.ChannelResource.DeviceName + ".PlayWait";
                                            m_VoiceResourceThread.IsBackground = true;
                                            m_VoiceResourceThread.Start();

                                            nextBeep = DateTime.Now.AddSeconds(beepInterval);
                                        }
                                    }
                                }

                                threadEvent.WaitOne(2000, false);

                            }
                            break;
                        default:
                            Log.Write("|MA|{0}|{1}|Dial agent unexpected dial result '{2}', cancelling call.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName, dr1.ToString());
                            break;
                    }
                }
                catch (HangupException)
                {
                    Log.Write("|MA|{0}|{1}|Dial agent  hung up.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName);
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "DialAgent Exception");
                }
                finally
                {
                    //Route agent Channel Back To its Voice Resource
                    if (DialerEngine.Connected)
                    {
                        try
                        {

                            if (ManagedChannel.ChannelResource is MsiChannel)
                            {
                                ManagedChannel.ChannelResource.StopListening();                
                            }
                            else
                            {
                                ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                            }

                            ManagedChannel.ChannelResource.Disconnect();
                            ManagedChannel.ReturnChannel(ManagedChannel, 0);
                            ManagedChannel = null;
                        }
                        catch (Exception ex)
                        {
                            Log.WriteException(ex, "DialAgent Exception");
                        }
                    }
                }
            }

            if (!LoggedOff)
            {

                if (bAgentConnected)
                {
                    // Agent disconnected so remove from offhook
                    ManagedAgent.RemoveAgentFromOffhook(this.AgentDetails.StationNumber, this);
                }
                else
                {
                    // Agent not connected, Update status to available for next call
                    if (this.Status != AgentStatus.Paused)
                        ManagedAgent.UpdateAgentStatusToAvailable(this.AgentDetails.StationNumber, this, true);
                }
            }
        }

        private Dialer m_Dialer = null;

        private Thread m_AssignDialerThread = null;        
        private ManualResetEvent m_AssignDialerThreadStop = new ManualResetEvent(false);        
        public Dialer Dialer
        {
            get { return m_Dialer; }
            set
            {
                lock (this)
                {
                    if (m_Dialer != null && value != null)
                    {
                        Log.Write("|MA|Error - Dialer was not null when assigning new dialer!");
                    }

                    m_Dialer = value;

                    if (m_Dialer != null)
                    {
                        m_AssignDialerThreadStop.Reset();       
                        Log.Write("|MA|Assign Dialer Thread spinning up. for {0}", m_Dialer.CallDetails.PhoneNum);
                        ThreadStart ts = new ThreadStart(AssignDialerThread);
                        m_AssignDialerThread = new Thread(ts);      
                        m_AssignDialerThread.IsBackground = true;   
                        m_AssignDialerThread.Priority = ThreadPriority.Highest;     
                        m_AssignDialerThread.Name = m_Dialer.CallDetails.PhoneNum + "_ADT";     
                        m_AssignDialerThread.Start();       
                    }
                    else
                    {
                        try
                        {
                            m_AssignDialerThreadStop.Set();         

                            while (m_AssignDialerThread != null) Thread.Sleep(50);      
                            // Since we are done with the dialer...
                            // Route agent Channel Back To its Voice Resource

                            if (ManagedChannel.ChannelResource is MsiChannel)
                            {
                                ManagedChannel.ChannelResource.StopListening();                 
                            }
                            else
                            {
                                ManagedChannel.ChannelResource.RouteFull(ManagedChannel.ChannelResource.VoiceResource);
                            }

                            if (!m_Disconnected)
                            {
                                ManagedAgent.AddAgentToOffhook(this.AgentDetails.StationNumber, this, false);
                            }
                        }
                        catch { }
                    }
                }
            }
        }

        private Thread m_ManualDialThread = null;
        private bool m_endManualDial = false;
        public bool EndManualDial
        {
            get { return m_endManualDial; }
            set
            {
                if (value == true)
                {
                    try
                    {
                        if (m_ManualDialThread != null)
                        {
                            m_ManualDialThread.Abort();
                        }
                    }
                    catch { }
                    m_ManualDialThread = null;
                }
                m_endManualDial = value;
            }
        }

        public void SetManualDialing()
        {
            if (this.AgentDetails.AllowManualDial)
            {
                if (m_ManualDialThread == null)
                {
                    ThreadStart ts = new ThreadStart(ManualDial);
                    m_ManualDialThread = new Thread(ts);
                    m_ManualDialThread.Name = this.AgentDetails.AgentName + "_ManualDial";
                    m_ManualDialThread.IsBackground = true;
                    m_ManualDialThread.Start();
                }
            }
        }

        private ManualDialer m_ManualDialer = null;

        public ManualDialer ManualDialer
        {
            get { return m_ManualDialer; }
            set
            {
                if (value == null && m_ManualDialer != null)
                {
                    m_ManualDialer.TerminateCall = true;
                }
                this.m_ManualDialer = value;
            }
        }

        public void ManualDial()
        {
            DateTime nextCheck = DateTime.Now.AddSeconds(2);
            long UniqueKey = 0;
            bool agentConnectionFailed = false;
            while (true)
            {
                if (DateTime.Now > nextCheck)
                {
                    if (EndManualDial || !DialerEngine.Connected || LoggedOff)
                    {
                        ManualDialer = null;
                        break;
                    }

                    try
                    {
                        // not avialable for dialer, he can manual dial
                        if (this.Status != AgentStatus.Available)
                        {

                            CampaignDetails callDetails = CampaignAPI.GetManualDailCallDetails(this.AgentDetails.CampaignDB, this.AgentDetails.AgentID);

                            if (callDetails != null && callDetails.PhoneNum.Trim() != string.Empty)
                            {
                                if (UniqueKey == callDetails.UniqueKey && agentConnectionFailed)
                                {
                                    Log.Write("|MA|{0}|{1}|{2}|Manual Dial failed due to agent connection failure.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName, callDetails.PhoneNum);
                                    //break;
                                }
                                else
                                {
                                    UniqueKey = callDetails.UniqueKey;

                                    Log.Write("|MA|{0}|{1}|{2}|Manual Dial initiated.", this.AgentDetails.CampaignID, this.AgentDetails.AgentName, callDetails.PhoneNum);
                                    
                                    // There is a manual dial call 
                                    if (this.ManagedChannel == null || (!this.ManagedChannel.Connected))
                                    {
                                        Log.Write("|MA|{0}|{1}|{2}|Agent is remote, need to dial them.", this.AgentDetails, this.AgentDetails.AgentName, callDetails.PhoneNum);
                                    
                                        bool isMsiAgent = IsMsiAgent(this.AgentDetails.StationNumber);
                                        bool bChannelsAvailable = false;
                                        if (isMsiAgent)
                                        {
                                            bChannelsAvailable = ManagedChannel.IsMsiChannelAvailable(
                                                this.AgentDetails.StationNumber);
                                        }
                                        else
                                        {
                                            bChannelsAvailable = ManagedChannel.IsChannelsAvailable();
                                        }
                                        if (bChannelsAvailable)
                                        {
                                            Thread t = new Thread(DialAgent);
                                            t.IsBackground = true;
                                            t.Name = "MD_DialAgent_" + this.AgentDetails.StationNumber;
                                            t.Start();

                                            DateTime checkTime = DateTime.Now.AddSeconds(40.0);
                                            while (true)
                                            {
                                                if (this.ManagedChannel != null && this.ManagedChannel.Connected)
                                                {
                                                    break;
                                                }
                                                if (DateTime.Now > checkTime)
                                                    break;

                                                Thread.Sleep(500);
                                            }

                                            if (this.ManagedChannel == null || (!this.ManagedChannel.Connected))
                                            {
                                                Log.Write("|MA|{0}|{1}|{2}|Agent connection failed.", this.AgentDetails, this.AgentDetails.AgentName, callDetails.PhoneNum);
                                    
                                            }
                                            else
                                            {
                                                Log.Write("|MA|{0}|{1}|{2}|Agent connected.", this.AgentDetails, this.AgentDetails.AgentName, callDetails.PhoneNum);
                                    
                                            }
                                        }
                                        else
                                        {
                                            Log.Write("|MA|{0}|{1}|{2}|Manual Dial failed due to no channels available.", this.AgentDetails, this.AgentDetails.AgentName, callDetails.PhoneNum);
                                    
                                        }
                                    }

                                    if (this.ManagedChannel != null && this.ManagedChannel.Connected)
                                    {
                                        agentConnectionFailed = false;
                                        // dial the number and route him
                                        try
                                        {
                                            m_ManualDialer = new ManualDialer(this, callDetails);
                                            m_ManualDialer.RunScript();

                                            // Call over
                                            m_ManualDialer = null;
                                        }
                                        catch (Exception ex)
                                        {
                                            if (!(ex is System.Threading.ThreadAbortException))
                                                Log.WriteException(ex, "Manual dialing exception");
                                        }
                                    }
                                    else
                                    {
                                        agentConnectionFailed = true;
                                        Log.Write("|MA|{0}|{1}|{2}|Manual dial agent connection failure.", this.AgentDetails, this.AgentDetails.AgentName, callDetails.PhoneNum);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        if (!(ex is System.Threading.ThreadAbortException))
                            Log.WriteException(ex, "Manual dialing exception main");
                    }
                    finally
                    {
                        try
                        {
                            m_ManualDialer = null;
                        }
                        catch { }
                    }

                    nextCheck = DateTime.Now.AddSeconds(2);
                }
                else
                {
                    Thread.Sleep(500);
                }
            }
        }



        public void AssignDialerThread()
        {
            try
            {
                PlayConnectingCustomer();
                try
                {
                    Dialer.ManagedAgent = this;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Agent No Longer Needed") >= 0)
                    {
                        Log.Write("|MA|Agent No Longer Needed");
                        ManagedAgent.AddAgentToOffhook(this.AgentDetails.StationNumber, this, true);
                    }
                    else
                    {
                        Log.WriteException(ex, "AssignDialerThread");
                    }
                    return;
                }

                if (m_AssignDialerThreadStop.WaitOne(0, false))                 
                {
                    Log.Write("|MA|{0}|{1}|Dialer hung before route on call.", Dialer.ManagedAgent.AgentDetails.CampaignID, Dialer.ManagedAgent.AgentDetails.AgentName);                  
                    return;                                                         
                }
                Log.Write("|MA|{0}|{1}|Routing agent to phone number '{2}'.", Dialer.ManagedAgent.AgentDetails.CampaignID, Dialer.ManagedAgent.AgentDetails.AgentName, Dialer.CallDetails.PhoneNum);
                Log.Write("|MA|{0}|{1}|Devices being routed: {2} to {3}.", Dialer.ManagedAgent.AgentDetails.CampaignID, Dialer.ManagedAgent.AgentDetails.AgentName, ManagedChannel.ChannelResource.DeviceName, Dialer.ManagedChannel.ChannelResource.DeviceName); 
                // Route customer to agent
                ManagedChannel.ChannelResource.RouteFull(Dialer.ManagedChannel.ChannelResource);

                try
                {
                    Log.Write("|MA|{0}|{1}|Route complete to phone number '{2}'.", Dialer.ManagedAgent.AgentDetails.CampaignID, Dialer.ManagedAgent.AgentDetails.AgentName, Dialer.CallDetails.PhoneNum);
                }
                catch { }

                // Check if agent hangup from agent interface
               
                // Sleep after route to stop transferred calls from hanging up
                Thread.Sleep(1500);

                try
                {
                    while (true)        //JMC
                    {
                        bool isHangup = false;
                        try
                        {
                            isHangup = CampaignAPI.IsCallHangup(this.Dialer.CallDetails.UniqueKey, this.Dialer.Campaign.CampaignDBConnString);
                        }
                        catch (Exception ex)
                        {
                            if (this.Dialer == null)
                            {
                                Log.Write("|MA|Hangup Check threads problem");
                            }
                            else
                            {
                                if (ex.InnerException != null)
                                    ex = ex.InnerException;
                                Log.WriteException(ex, "Call Hangup Exception");
                            }
                        }
                        if (isHangup)
                        {
                            try
                            {
                                this.Dialer.TerminateCall = true;
                                Log.Write("|MA|Call hangup from agent interface.");
                            }
                            catch { }
                            break;
                        }

                        bool isTransfered = false;
                        try
                        {
                            isTransfered = CampaignAPI.IsCallTransfered(this.Dialer.CallDetails.UniqueKey, this.Dialer.Campaign.CampaignDBConnString);
                        }
                        catch (Exception ex)
                        {
                            Log.WriteException(ex, "Agent manager transfer exception.");
                        }
                        if (isTransfered)
                        {
                            try
                            {
                                // Find verification agent
                                this.Dialer.TerminateCall = true;
                                this.Dialer.IsTransferedCall = true;
                                Log.Write("|MA|{0}|{1}|Managed agent thread has determined transfer, informing dialer thread.", Dialer.ManagedAgent.AgentDetails.CampaignID,  Dialer.ManagedAgent.AgentDetails.AgentName);
                                if (m_AssignDialerThreadStop.WaitOne(3000, false))          // GW Bug trap 11.23.11
                                {                                                           
                                    break;                                                  
                                }                          
                            }
                            catch { }
                            break;
                        }

                        if (m_AssignDialerThreadStop.WaitOne(1000, false))          //JMC
                        {                                                           
                            break;                                                  
                        }                                                           

                    }
                }
                catch (Exception ex)
                {
                    Log.WriteException(ex, "Call Hangup check");
                }

            }
            catch (Exception ex2)
            {
                Log.WriteException(ex2, "AssignDialerThread");
            }
            finally                                                     //JMC 
            {                                                              
                m_AssignDialerThread = null;                                
            }                                                                       

        }
    }



    
    public class AgentRequest
    {
        private static Log Log;
        private static Queue<AgentRequest> _RequestQueue = new Queue<AgentRequest>();
        private static AutoResetEvent _AgentRequestEvent = new AutoResetEvent(false);

        public static AutoResetEvent AgentRequestEvent
        {
            get { return AgentRequest._AgentRequestEvent; }
        }
        private static Thread _AgentRequestThread = null;

        //static AgentRequest()
        //{
        //    ThreadStart ts = new ThreadStart(AgentRequestProcessor);
        //    _AgentRequestThread = new Thread(ts);
        //    _AgentRequestThread.Name = "AgentRequestProcessor";
        //    _AgentRequestThread.Start();
        //}

        public static void GetMeAnAgent(Dialer dialer)
        {
            AgentRequest agentRequest = new AgentRequest(dialer);
            lock (_RequestQueue)
            {
                _RequestQueue.Enqueue(agentRequest);


                DialerEngine.Log.Write("|MA|{0}|{1}|Agent request for {2}.", dialer.Campaign.CampaignID, dialer.Campaign.ShortDescription, dialer.CallDetails.PhoneNum);

                if (_AgentRequestThread == null)
                {
                    Log = DialerEngine.Log;
                    ThreadStart ts = new ThreadStart(AgentRequestProcessor);
                    _AgentRequestThread = new Thread(ts);
                    _AgentRequestThread.Priority = ThreadPriority.Highest;
                    //_AgentRequestThread.IsBackground = true;
                    _AgentRequestThread.Name = dialer.CallDetails.PhoneNum + "_ARP";
                    _AgentRequestThread.Start();
                }

                _AgentRequestEvent.Set();

            }
        }


        public static void CancelMyAgentRequest(Dialer dialer)
        {
            try
            {
                DialerEngine.Log.Write("|MA|{0}|{1}|Cancel my agent request for {2}.", dialer.Campaign.CampaignID, dialer.Campaign.ShortDescription, dialer.CallDetails.PhoneNum);
            }
            catch { }
            lock (_RequestQueue)
            {
                AgentRequest[] agentRequestArray = _RequestQueue.ToArray();
                foreach (AgentRequest agentRequest in agentRequestArray)
                {
                    if (agentRequest.Dialer.Equals(dialer))
                    {
                        agentRequest.StillWaiting = false;
                        break;
                    }
                }
            }
        }

        private static void AgentRequestProcessor()
        {
            try
            {

                while (true)
                {
                    AgentRequest agentRequest = null;
                    lock (_RequestQueue)
                    {
                        if (_RequestQueue.Count == 0 || (!DialerEngine.Connected)) break;

                        while (true)
                        {
                            if (_RequestQueue.Count == 0) break;

                            agentRequest = _RequestQueue.Peek();

                            // Dump this entry if they went away.
                            if (!agentRequest.StillWaiting)
                            {
                                agentRequest = null;
                                _RequestQueue.Dequeue();
                                continue;
                            }

                            break;
                        }
                    }

                    if (agentRequest != null)
                    {
                        // If a Fast Agent is available, Use them!
                        // If not, start dialing agents!

                        // Get offhook agent
                        ManagedAgent managedAgent = ManagedAgent.GetFastestAgent(agentRequest.Dialer.Campaign.CampaignID, agentRequest.Dialer.IsTransferedCall);

                        if (managedAgent == null)
                        {
                            // offhook agent not found, ring all standby agents
                            Log.Write("|MA|{0}|{1}|Offhook agent not found trying to ring standby agents.", agentRequest.Dialer.Campaign.CampaignID, agentRequest.Dialer.Campaign.ShortDescription);
                            ManagedAgent.RingStandbyAgents(agentRequest.Dialer.Campaign.CampaignID);
                        }
                        else
                        {
                            // We have an agent
                            Log.Write("|MA|{0}|{1}|Dialer has an agent '{2}' at station {3} for phone number {4}", agentRequest.Dialer.Campaign.CampaignID, agentRequest.Dialer.Campaign.ShortDescription, managedAgent.AgentDetails.AgentName, managedAgent.AgentDetails.StationNumber, agentRequest.Dialer.CallDetails.PhoneNum);
                            lock (_RequestQueue)
                            {
                                _RequestQueue.Dequeue();
                            }
                            Log.Write("|MA|{0}|{1}|Assigning agent '{2}' at station {3} for phone number {4}", agentRequest.Dialer.Campaign.CampaignID, agentRequest.Dialer.Campaign.ShortDescription, managedAgent.AgentDetails.AgentName, managedAgent.AgentDetails.StationNumber, agentRequest.Dialer.CallDetails.PhoneNum);
                            managedAgent.Dialer = agentRequest.Dialer;
                            continue;
                        }

                        if (!agentRequest.Dialer.IsTransferedCall)
                        {
                            if (DateTime.Now > agentRequest.WaitUntil)
                            {
                                agentRequest.Dialer.NoAvailableAgents = true;
                                lock (_RequestQueue)
                                {
                                    _RequestQueue.Dequeue();
                                }
                                continue;
                            }
                        }
                        else
                        {
                            // 5 minutes waiting for verification agent
                            if (DateTime.Now > agentRequest.WaitUntil.AddMinutes(5))
                            {
                                agentRequest.Dialer.NoAvailableAgents = true;
                                lock (_RequestQueue)
                                {
                                    _RequestQueue.Dequeue();
                                }
                                continue;
                            }
                        }
                    }


                    _AgentRequestEvent.WaitOne(5000, false);

                    if (!DialerEngine.Connected) break;

                }
            }
            catch (ThreadAbortException te)
            {
                if (DialerEngine.Connected)
                {
                    Log.WriteException(te, "AgentRequestProcessor Exception!");
                }
            }
            catch (Exception ex)
            {
                Log.WriteException(ex, "AgentRequestProcessor Exception!");
            }
            finally
            {
                _AgentRequestThread = null;
            }
        }

        public static void DisposeAgentRequest()
        {
            _RequestQueue.Clear();
            if (_AgentRequestThread != null)
            {
                try
                {
                    _AgentRequestThread.Abort();
                }
                finally { _AgentRequestThread = null; }
            }
        }

        private Dialer m_Dialer;

        public Dialer Dialer
        {
            get { return m_Dialer; }
        }

        private bool m_StillWaiting = true;

        public bool StillWaiting
        {
            get { return m_StillWaiting; }
            set { m_StillWaiting = value; }
        }

        private DateTime m_WaitUntil = DateTime.Now.AddSeconds(
            Convert.ToDouble(Utilities.GetAppSetting("AgentConnectionTimeout", "25")));

        public DateTime WaitUntil
        {
            get { return m_WaitUntil; }
        }

        private AgentRequest(Dialer dialer)
        {
            m_Dialer = dialer;
        }
    }
}
