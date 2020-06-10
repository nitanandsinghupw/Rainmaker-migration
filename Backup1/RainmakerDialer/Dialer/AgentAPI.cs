using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Xml;
using System.Collections;

using RainMakerDialer.AgentsWS;
using Rainmaker.Common.DomainModel;
using RainMakerDialer.CampaignWS;

namespace Rainmaker.RainmakerDialer
{
    public class AgentAPI
    {
        #region Variables
        
        // lock variable
        private static object s_SyncVar = new object();
        
        #endregion

        #region Constroctor

        /// <summary>
        /// Constructor
        /// </summary>
        public AgentAPI()
        {
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Get All LoggedIn agents
        /// </summary>
        /// <returns></returns>
        public static List<Agent> GetLoggedInAgents()
        {
            List<Agent> agentList = new List<Agent>();
            AgentService objAgentService = null;
            DataSet ds = null;
            Agent agent = null;
            try
            {

                objAgentService = new AgentService();
                ds = objAgentService.GetLoggedInAgents();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (row["StationNumber"].ToString().Trim() != "")
                    {
                        agent = new Agent();
                        agent.AgentID = (long)row["AgentID"];
                        agent.AgentName = (row["AgentName"]).ToString();
                        agent.PhoneNumber = row["PhoneNumber"].ToString();
                        agent.StationNumber = row["StationNumber"].ToString();
                        agent.AllwaysOffHook = (bool)row["AllwaysOffHook"];
                        if (Convert.ToInt32(row["AgentStatusID"]) == (int)AgentLoginStatus.Ready)
                        {
                            agent.Status = AgentStatus.Available;
                        }
                        else if (Convert.ToInt32(row["AgentStatusID"]) == (int)AgentLoginStatus.Busy)
                        {
                            agent.Status = AgentStatus.Busy;
                        }
                        else
                        {
                            agent.Status = AgentStatus.Paused;
                        }

                        agent.CampaignID = (long)row["CampaignID"];
                        
                        agent.OutboundCallerID = (row["OutboundCallerID"]).ToString();
                        try
                        {
                            agent.AllowManualDial = (bool)row["AllowManualDial"];
                            agent.CampaignDB = (row["CampaignDBConnString"]).ToString();
                            agent.ShortDescription = (row["ShortDescription"]).ToString();
                            agent.VerificationAgent = (bool)row["VerificationAgent"];
                            agent.ReceiptModeID = (long)row["AgentReceiptModeID"];
                            //agent.ReceiptModeID = (long)row[" "];
                        }
                        catch {}
                        agentList.Add(agent);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().IndexOf("unable to connect to the remote server") >= 0)
                {
                    DialerEngine.Log.Write("Exception : Database Accessing Error, Please check services");
                }
                else
                {
                    DialerEngine.Log.WriteException(ex, "Error in GetLoggedInAgents");
                }
                //throw ex;
            }
            finally
            {
                //list = null;
                ds = null;
                objAgentService = null;
                //DialerEngine.Log.Write("GetLoggedInAgents End");
            }
            return agentList;
        }
        /// <summary>
        /// Get all currently Logged in Agent Stat
        /// </summary>
        /// <param name="campaignDBConnString"></param>
        /// <returns></returns>
        public static List<AgentStat> GetAgentStat(string campaignDBConnString, long campaignId)
        {
            List<AgentStat> agentStatList = new List<AgentStat>();
            AgentStat agentStat;
            CampaignService objCampService = null;
            DataSet dsAgentStat = null;
            
            try
            {   
                objCampService = new CampaignService();
                dsAgentStat = objCampService.GetAgentStat(campaignDBConnString, campaignId);

                foreach (DataRow drAgent in dsAgentStat.Tables[0].Rows)
                {
                    try
                    {
                        if (Convert.ToInt32(drAgent["StatusID"]) != (int)AgentLoginStatus.Pause)
                        {
                            if (drAgent["WrapTime"] != DBNull.Value)
                            {
                                agentStat = new AgentStat();
                                agentStat.AgentID = Convert.ToInt64(drAgent["AgentID"]);
                                agentStat.WrapTime = Convert.ToDecimal(drAgent["WrapTime"] != DBNull.Value ? drAgent["WrapTime"] : "0");
                                agentStat.TalkTime = Convert.ToDecimal(drAgent["TalkTime"] != DBNull.Value ? drAgent["TalkTime"] : "0");
                                agentStat.Calls = Convert.ToInt32(drAgent["Calls"] != DBNull.Value ? drAgent["Calls"] : "0");
                                agentStatList.Add(agentStat);
                            }
                        }
                    }
                    catch (Exception ee)
                    {
                        if (!(ee is System.Threading.ThreadAbortException))
                            DialerEngine.Log.WriteException(ee, "Error in GetAgentStat1");
                    }
                }
            }
            catch (Exception ex)
            {
                if (!(ex is System.Threading.ThreadAbortException))
                {
                    DialerEngine.Log.WriteException(ex, "Error in GetAgentStat");
                }
                throw ex;
            }
            finally
            {   
                dsAgentStat.Dispose();
                objCampService.Dispose();
                agentStat = null;
            }
            return agentStatList;
        }

        #endregion

    }
}
