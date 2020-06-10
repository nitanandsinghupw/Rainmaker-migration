using System;
using System.Collections.Generic;
using System.Text;
using VoiceElements.Common;
using Rainmaker.Common.DomainModel;

namespace Rainmaker.RainmakerDialer
{
    public class AgentProcess
    {
        private Campaign campaign;
        public AgentProcess(Campaign camp)
        {
            this.campaign = camp;
        }

        int iAgentQueueCheckTime = Convert.ToInt32(Utilities.GetAppSetting("AgentAvailabiltyCheckInterval", "30000"));

        // minimum available agents required for the campaign
        int iMinAgentsRequired = Convert.ToInt32(Utilities.GetAppSetting("MinAgentsRequiredToDial", "1"));


        public static double AgentCheckTime
        {
            get
            {
                double iCheckTime = 30.0;// default 30 sec
                try
                {
                    iCheckTime = Convert.ToDouble(Utilities.GetAppSetting("AgentAvailabiltyCheckInterval", "30000")) / 1000;
                }
                catch { }
                return iCheckTime;
            }
        }

        public static int MinAgentsRequiredToDial
        {
            get
            {
                int iMinAgentsRequired = 1;
                try
                {
                    iMinAgentsRequired = Convert.ToInt32(Utilities.GetAppSetting("MinAgentsRequiredToDial", "1"));
                }
                catch { }
                return iMinAgentsRequired;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public int WaitForActiveAgents()
        {
            int iCount = 0;

            //For safety we are waiting max of 2 minutes // GW changed to loop in campaign process 10.01.10
            //DateTime dtCheckUntil = DateTime.Now.AddMinutes(2.0);
            try
            {
                iCount = ManagedAgent.GetAvailableAgentCount(campaign.CampaignID);
                // int iCount = ManagedAgent.GetLoggedInAgentCount(campaign.CampaignID);
                if (iCount >= iMinAgentsRequired)
                {
                    return iCount;
                }
                else
                {
                    // wait until active agents found 
                    System.Threading.Thread.Sleep(iAgentQueueCheckTime);

                    // GW changed to loop in campaign process 10.01.10
                    //if (DateTime.Now < dtCheckUntil)
                        //WaitForActiveAgents();
                }
            }
            catch (System.Threading.ThreadAbortException TAE)
            {
                if (DialerEngine.Connected)
                {
                    DialerEngine.Log.Write("|AP|AgentThread Aborted " + TAE.Message);
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in CheckActiveAgents");
                throw ex;
            }
            finally
            {
                //
            }
            return iCount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int GetLoggedinAgentCount()
        {
            int iCount = 0;
            try
            {
                iCount = ManagedAgent.GetLoggedInAgentCount(campaign.CampaignID);
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in GetLoggedinAgentCount");
                throw ex;
            }
            finally
            {
                //
            }

            return iCount;
        }
    }
}
