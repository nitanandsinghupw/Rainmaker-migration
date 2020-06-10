using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Rainmaker.Common.DomainModel;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Rainmaker.DAL.Properties;

namespace Rainmaker.DAL
{
    public static class AgentAccess
    {

        private static int m_MaxTransactionRetries = 5;
        static AgentAccess()
        {
            try
            {
                m_MaxTransactionRetries = Settings.Default.MaxTransactionRetries;   
            }
            catch{}
        }
        /// <summary>
        /// Authenticates Agent
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="campaignMasterDBConn"></param>
        /// <returns></returns>
        public static Agent AuthenticateAgent(string userName, string password,
            string campaignMasterDBConn, string ipAddress, string hostName)
        {
            Agent agent = new Agent();
            Encryption enc = new Encryption();
            DataSet ds;

            try
            {
                SqlParameter[] sparams = new SqlParameter[]{
                    new SqlParameter("@LoginName", userName.ToString())};

                ds = SqlHelper.ExecuteDataset(campaignMasterDBConn, CommandType.StoredProcedure,
                    "p_AgentInfoByLoginNameGet", sparams);
                if (ds.Tables[0].Rows.Count == 1)
                {
                    DataRow r;
                    r = ds.Tables[0].Rows[0];

                    if (r["Password"].ToString() != enc.Encrypt(password))
                    {
                        agent.Status = AgentStatus.IncorrectPassword;
                    }
                    else
                    {
                        agent.AgentID = (long)r["AgentID"];
                        agent.AgentName = r["AgentName"].ToString();
                        agent.LoginName = r["AgentID"].ToString();
                        agent.IsAdministrator = (bool)r["IsAdministrator"];
                        agent.AllowManualDial = (bool)r["AllowManualDial"];
                        agent.VerificationAgent = (bool)r["VerificationAgent"];
                        agent.InBoundAgent = (bool)r["InBoundAgent"];
                       // agent.IsDefault = (bool)r["IsDefault"];
                        agent.PhoneNumber = r["PhoneNumber"].ToString();
                        agent.DateCreated = (DateTime)r["DateCreated"];
                        agent.DateModified = (DateTime)r["DateModified"];
                        agent.Status = AgentStatus.Authenticated;

                        if (agent.IsAdministrator == false)
                        {
                            agent = InsGetAgentActivity(agent, ipAddress, hostName, campaignMasterDBConn);
                        }
                    }
                }
                else
                {
                    agent.Status = AgentStatus.UserDoesNotExist;
                }

                ds.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return agent;
        }

        /// <summary>
        /// Insert and Select Agent activity details
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="campaignMasterDBConn"></param>
        /// <returns></returns>
        public static Agent InsGetAgentActivity(Agent agent, string ipAddress, string hostName, 
            string campaignMasterDBConn)
        {
            //InsGet_AgentActivity
            DataSet ds;
            SqlParameter[] sparams = new SqlParameter[]{
                    new SqlParameter("@AgentID", agent.AgentID),
                    new SqlParameter("@StationIP", ipAddress),
                    new SqlParameter("@StationHostName", hostName)};
            ds = SqlHelper.ExecuteDataset(campaignMasterDBConn, CommandType.StoredProcedure,
                     "InsGet_AgentActivity", sparams);
            if (ds.Tables[0].Rows.Count == 1)
            {
                DataRow r;
                r = ds.Tables[0].Rows[0];
                agent.AgentActivityID = (long)r["AgentActivityID"];
                agent.AgentID = Convert.ToInt64(r["AgentID"]);
                agent.AgentStatusID = Convert.ToInt64(r["AgentStatusID"]);
                agent.ReceiptModeID = Convert.ToInt64(r["AgentReceiptModeID"]);
                agent.CampaignID = Convert.ToInt64(r["CampaignID"] == Convert.DBNull ? Convert.ToInt64(0) : Convert.ToInt64(r["CampaignID"]));
                agent.LoginTime = Convert.ToDateTime(r["LoginTime"]);
                agent.AllwaysOffHook = Convert.ToBoolean(r["AllwaysOffHook"]);
                agent.StationNumber = Convert.ToString(r["StationNumber"]);
                agent.StationHost  = Convert.ToString(r["StationHostName"]);
            }
            return agent;
            //Upd_AgentActivity
        }

        /// <summary>
        /// Insert and Select Agent activity details
        /// </summary>
        /// <param name="agent"></param>
        /// <param name="campaignMasterDBConn"></param>
        /// <returns></returns>
        public static Agent GetAgentActivity(long agentActivityId, Agent agent, string ipAddress, string hostName,
            string campaignMasterDBConn)
        {
            //InsGet_AgentActivity
            DataSet ds;
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparams = new SqlParameter[]{
                    new SqlParameter("@AgentActivityID", agentActivityId),
                    new SqlParameter("@StationIP", ipAddress),
                    new SqlParameter("@StationHostName", hostName)};
                    ds = SqlHelper.ExecuteDataset(campaignMasterDBConn, CommandType.StoredProcedure,
                             "Get_AgentActivity_ById", sparams);
                    if (ds.Tables[0].Rows.Count == 1)
                    {
                        DataRow r;
                        r = ds.Tables[0].Rows[0];
                        agent.AgentActivityID = (long)r["AgentActivityID"];
                        agent.AgentID = Convert.ToInt64(r["AgentID"]);
                        agent.AgentStatusID = Convert.ToInt64(r["AgentStatusID"]);
                        agent.CampaignID = Convert.ToInt64(r["CampaignID"] == Convert.DBNull ? Convert.ToInt64(0) : Convert.ToInt64(r["CampaignID"]));
                        agent.LoginTime = Convert.ToDateTime(r["LoginTime"]);
                        agent.AllwaysOffHook = Convert.ToBoolean(r["AllwaysOffHook"]);
                        agent.StationNumber = Convert.ToString(r["StationNumber"]);
                        agent.StationHost = Convert.ToString(r["StationHostName"]);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetAgentActivity failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return agent;
            //Upd_AgentActivity
        }

        /// <summary>
        /// Agent Insert/Update
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="agent"></param>
        /// <returns></returns>
        public static Agent AgentInsertUpdate(string campaignMasterDBConn,
            Agent agent)
        {
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AgentID", agent.AgentID),
                            new SqlParameter("@AgentName", agent.AgentName),
                            new SqlParameter("@LoginName", agent.LoginName),
                            new SqlParameter("@Password", agent.Password),
                            new SqlParameter("@IsAdministrator", agent.IsAdministrator),
                            new SqlParameter("@AllowManualDial", agent.AllowManualDial),
                            new SqlParameter("@VerificationAgent", agent.VerificationAgent),
                            new SqlParameter("@InBoundAgent", agent.InBoundAgent),
                            new SqlParameter("@PhoneNumber", agent.PhoneNumber),
                            new SqlParameter("@DateCreated", DateTime.Now.Date),
                            new SqlParameter("@DateModified", DateTime.Now.Date)};

                    agent.AgentID = (long)SqlHelper.ExecuteScalar(campaignMasterDBConn,
                        CommandType.StoredProcedure, "InsUpd_Agent", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("UNIQUE KEY constraint 'IX_Agent'") >= 0)
                    {
                        throw new Exception("NumberDuplicateException");
                    }
                    if (ex.Message.IndexOf("UNIQUE KEY constraint 'IX_Agent_1'") >= 0)
                    {
                        throw new Exception("LoginDuplicateException");
                    }
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in AgentInsertUpdate failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return agent;
        }

        /// <summary>
        /// Get Agent List
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetAgentList(string strCampaignMasterDBConn)
        {
            DataSet dsAgentList = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsAgentList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                        CommandType.StoredProcedure, "Sel_Agent_List");
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetAgentList failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return dsAgentList;
        }

        /// <summary>
        /// Get Agent List
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetAgentsByCampaign(Campaign campaign, string strCampaignMasterDBConn)
        {
            DataSet dsAgentList = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@CampaignID", campaign.CampaignID)};
                    //dsAgentList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                    //    CommandType.StoredProcedure, "Sel_Agent_List", sparam_s);
                    dsAgentList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                        CommandType.StoredProcedure, "Sel_Agent_List");
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetAgentsByCmapaign failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return dsAgentList;
        }

        /// <summary>
        /// Get Agent List
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetLoggedInAgents(string strCampaignMasterDBConn)
        {
            DataSet dsAgentList = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsAgentList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                        CommandType.StoredProcedure, "Sel_LoggedInAgents");
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetLoggedInAgents failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return dsAgentList;
        }

        /// <summary>
        /// Upadate Agent status
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static void UpdateAgentStatus(Agent agent, string campaignMasterDBConn)
        {
            // Added by GW 09.29.10 - Update status

            try
                {
                    AgentActivityInsertUpdate(campaignMasterDBConn, agent);
                }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get Agent Detail
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static Agent GetAgentByAgentID(string campaignMasterDBConn,
            long AgentID)
        {
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@AgentID",AgentID)
            };

            DataSet dsAgentList;
            Agent agent = new Agent();
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsAgentList = SqlHelper.ExecuteDataset(campaignMasterDBConn,
                        CommandType.StoredProcedure, "Sel_Agent_Dtl", sparam_s);

                    if (dsAgentList.Tables[0].Rows.Count == 1)
                    {
                        DataRow r;
                        r = dsAgentList.Tables[0].Rows[0];
                        agent.AgentID = (long)r["AgentID"];
                        agent.AgentName = r["AgentName"].ToString();
                        agent.LoginName = r["LoginName"].ToString();
                        agent.Password = r["Password"].ToString();
                        agent.IsAdministrator = (bool)r["IsAdministrator"];
                        agent.AllowManualDial = (bool)r["AllowManualDial"];
                        agent.VerificationAgent = (bool)r["VerificationAgent"];
                        agent.InBoundAgent = (bool)r["InBoundAgent"];
                        agent.PhoneNumber = r["PhoneNumber"].ToString();
                        agent.DateCreated = (DateTime)r["DateCreated"];
                        agent.DateModified = (DateTime)r["DateModified"];
                    }

                    dsAgentList.Clear();
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetAgentById failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return agent;
        }


        /// <summary>
        /// Get Agent Detail
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static Agent GetAgentByAgentID(string campaignMasterDBConn,
            long AgentID, string ipAddress, string hostName, long agentactivityId)
        {
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@AgentID",AgentID)
            };

            DataSet dsAgentList;
            Agent agent = new Agent();
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsAgentList = SqlHelper.ExecuteDataset(campaignMasterDBConn,
                        CommandType.StoredProcedure, "Sel_Agent_Dtl", sparam_s);

                    if (dsAgentList.Tables[0].Rows.Count == 1)
                    {
                        DataRow r;
                        r = dsAgentList.Tables[0].Rows[0];
                        agent.AgentID = (long)r["AgentID"];
                        agent.AgentName = r["AgentName"].ToString();
                        agent.LoginName = r["LoginName"].ToString();
                        agent.Password = r["Password"].ToString();
                        agent.IsAdministrator = (bool)r["IsAdministrator"];
                        agent.AllowManualDial = (bool)r["AllowManualDial"];
                        agent.VerificationAgent = (bool)r["VerificationAgent"];
                        agent.InBoundAgent = (bool)r["InBoundAgent"];
                        agent.PhoneNumber = r["PhoneNumber"].ToString();
                        agent.DateCreated = (DateTime)r["DateCreated"];
                        agent.DateModified = (DateTime)r["DateModified"];
                        agent.IsReset = (bool)r["IsReset"];

                        agent = GetAgentActivity(agentactivityId, agent, ipAddress, hostName, campaignMasterDBConn);
                    }

                    dsAgentList.Clear();
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetAGentByAgentID failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return agent;
        }


        /// <summary>
        /// Delete Agent.
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="agentID"></param>
        /// <returns></returns>
        public static int DeleteAgent(string campaignMasterDBConn, long agentID)
        {
            int result = 0;
            SqlParameter param = new SqlParameter("@AgentID", agentID);
            try
            {
                result = SqlHelper.ExecuteNonQuery(campaignMasterDBConn,
                     CommandType.StoredProcedure, "Dbo.DEL_Agent", param);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Insert or Update Agent and Campaign Id's
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="agentCampaignMap"></param>
        /// <returns></returns>
        public static AgentCampaignMap AgentCampaignMapInsertUpdate(string campaignMasterDBConn,
            AgentCampaignMap agentCampaignMap)
        {
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AgentCampaignMapID", agentCampaignMap.AgentCampaignMapID),
                            new SqlParameter("@AgentID", agentCampaignMap.AgentID),
                            new SqlParameter("@CampaignID", agentCampaignMap.CampaignID),
                            new SqlParameter("@DateCreated", DateTime.Now.Date),
                            new SqlParameter("@DateModified", DateTime.Now.Date)};

                    agentCampaignMap.AgentCampaignMapID = (long)SqlHelper.ExecuteScalar(campaignMasterDBConn,
                        CommandType.StoredProcedure, "InsUpd_AgentCampaignMap", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in AgentCampMapUpdate failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return agentCampaignMap;
        }

        /// <summary>
        /// Get agentcampaignmap detail
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static AgentCampaignMap GetAgentCampaignMapByAgentID(string campaignMasterDBConn,
            long AgentID)
        {
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@AgentID",AgentID)
            };

            DataSet dsAgentCampaignMap;
            AgentCampaignMap agentCampaignMap = new AgentCampaignMap();
            try
            {
                dsAgentCampaignMap = SqlHelper.ExecuteDataset(campaignMasterDBConn,
                    CommandType.StoredProcedure, "Sel_AgentCampaignMap_Dtl", sparam_s);

                if (dsAgentCampaignMap.Tables[0].Rows.Count == 1)
                {
                    DataRow r;
                    r = dsAgentCampaignMap.Tables[0].Rows[0];
                    agentCampaignMap.AgentCampaignMapID = (long)r["AgentCampaignMapID"];
                    agentCampaignMap.AgentID = (long)r["AgentID"];
                    agentCampaignMap.CampaignID = (long)r["CampaignID"];
                    agentCampaignMap.DateCreated = (DateTime)r["DateCreated"];
                    agentCampaignMap.DateModified = (DateTime)r["DateModified"];
                }

                dsAgentCampaignMap.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return agentCampaignMap;
        }

        /// <summary>
        /// Gets Login status list
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetLoginStatusList(string strCampaignMasterDBConn)
        {
            DataSet dsLoginStatusList = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsLoginStatusList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                        CommandType.StoredProcedure, "Sel_LoginStatus_List");
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetLoginStatusList failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return dsLoginStatusList;
        }

        /// <summary>
        /// Insert or update Login Status
        /// </summary>
        /// <param name="currentCampaignDBConn"></param>
        /// <param name="agentLogin"></param>
        /// <returns></returns>
        public static AgentLogin LoginStatusInsertUpdate(string currentCampaignDBConn,
            AgentLogin agentLogin)
        {
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AgentLoginID", agentLogin.AgentLoginID),
                            new SqlParameter("@AgentID", agentLogin.AgentID),
                            new SqlParameter("@LoginStatusID", agentLogin.LoginStatusID),
                            new SqlParameter("@DateCreated", DateTime.Now.Date),
                            new SqlParameter("@DateModified", DateTime.Now.Date)};

                    agentLogin.AgentLoginID = (long)SqlHelper.ExecuteScalar(currentCampaignDBConn,
                        CommandType.StoredProcedure, "InsUpd_AgentLogin", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in LoginStatusUpdate failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return agentLogin;
        }

        /// <summary>
        /// Gets login status detail by agentID
        /// </summary>
        /// <param name="currentCampaignDBConn"></param>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static AgentLogin GetLoginStatusByAgentID(string currentCampaignDBConn,
            long AgentID)
        {

            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@AgentID",AgentID)
            };

            DataSet dsLoginStatus;
            AgentLogin agentLogin = new AgentLogin();
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsLoginStatus = SqlHelper.ExecuteDataset(currentCampaignDBConn,
                        CommandType.StoredProcedure, "Sel_AgentLogin_Dtl", sparam_s);

                    if (dsLoginStatus.Tables[0].Rows.Count == 1)
                    {
                        DataRow r;
                        r = dsLoginStatus.Tables[0].Rows[0];
                        agentLogin.AgentLoginID = (long)r["AgentLoginID"];
                        agentLogin.AgentID = (long)r["AgentID"];
                        agentLogin.LoginStatusID = (long)r["LoginStatusID"];
                        agentLogin.DateCreated = (DateTime)r["DateCreated"];
                        agentLogin.DateModified = (DateTime)r["DateModified"];
                    }
                    dsLoginStatus.Clear();
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetLoginStstusByAgentId failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return agentLogin;
        }

        /// <summary>
        /// Agent activity status insert or update
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="agentActivity"></param>
        /// <returns></returns>
        public static Agent AgentActivityInsertUpdate(string campaignMasterDBConn,
            Agent agent)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AgentID", agent.AgentID),
                            new SqlParameter("@CampaignID", agent.CampaignID > 0 ? agent.CampaignID : Convert.DBNull),
                            new SqlParameter("@AgentStatusID", agent.AgentStatusID),
                            new SqlParameter("@AgentReceiptModeID", agent.ReceiptModeID),
                            new SqlParameter("@AgentActivityID", agent.AgentActivityID),
                            new SqlParameter("@IsDeleted", agent.IsDeleted)
                            };

                    int result = (int)SqlHelper.ExecuteNonQuery(campaignMasterDBConn,
                        CommandType.StoredProcedure, "Upd_AgentActivity", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in AgentActivityInsertUpdate failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return agent;
        }

        /// <summary>
        /// Update agent Logout status
        /// </summary>
        /// <param name="agentActivity"></param>
        /// <param name="strCampaignMasterDBConn"></param>
        public static void UpdateAgentLogOut(Agent agent, string strCampaignMasterDBConn)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AgentID", agent.AgentID),
                            new SqlParameter("@AgentActivityID", agent.AgentActivityID)};
                    SqlHelper.ExecuteNonQuery(strCampaignMasterDBConn,
                        CommandType.StoredProcedure, "Upd_AgentLogOut", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in UpdateAgentLogout failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
        }

        public static void ToggleAgentReset(long agentID, bool resetStatus, string strCampaignMasterDBConn)
        {
            DebugLogger.Write(string.Format("Resetting agent {0}.", agentID));
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AgentID", agentID),
                            new SqlParameter("@ResetSwitch", resetStatus)};
                    SqlHelper.ExecuteNonQuery(strCampaignMasterDBConn,
                        CommandType.StoredProcedure, "ToggleAgentReset", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in ToggleAgentReset failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
            // Loop through all non deleted campaigns and reset agent in each
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                DataSet dsCampaignList;
                try
                {
                    dsCampaignList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                        CommandType.StoredProcedure, "Sel_Campaign_List");
                    for (int j = 1; j < dsCampaignList.Tables[0].Rows.Count; j++)
                    {
                        try
                        {
                            string strCampDBConnection = dsCampaignList.Tables[0].Rows[j]["CampaignDBConnString"].ToString();
                            SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AgentID", agentID)};
                            SqlHelper.ExecuteNonQuery(strCampDBConnection,
                                CommandType.StoredProcedure, "ResetAgent", sparam_s);
                        }
                        catch{}
                    }
                    DebugLogger.Write(string.Format("Reset of agent {0} complete in {1} campaigns.", agentID, dsCampaignList.Tables[0].Rows.Count));
                    dsCampaignList.Clear();
                    
                    break;
                } 
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in ToggleAgentReset (Campaign Reset) failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Get Script
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetScript(string strCampaignDBConnString, bool isVerification)
        {
            DataSet dsScript = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@IsVericationScript", isVerification)};
                    dsScript = SqlHelper.ExecuteDataset(strCampaignDBConnString,
                        CommandType.StoredProcedure, "Sel_AgentScript", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetScript failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return dsScript;
        }

        /// <summary>
        /// Get Campaign Details
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetCampaignByAgentID(Agent objAgent, Campaign objCampaign, string campaignMasterDBConn)
        {
            DataSet ds = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {

                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AgentID", objAgent.AgentID)};

                    ds = SqlHelper.ExecuteDataset(objCampaign.CampaignDBConnString,
                        CommandType.StoredProcedure, "Sel_Campaign_By_AgentID", sparam_s);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        objAgent.AgentStatusID = (long)AgentLoginStatus.Busy;
                        AgentActivityInsertUpdate(campaignMasterDBConn, objAgent);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetCampByAgentID failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return ds;
        }


        /// <summary>
        /// Get Campaign Details
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetCampaignDetailsByAgentID(long agentID, Campaign objCampaign, bool isManualDial)
        {
            DataSet dsCampaignDetails = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AgentID", agentID),
                            new SqlParameter("@IsManualDial", isManualDial)};

                    dsCampaignDetails = SqlHelper.ExecuteDataset(objCampaign.CampaignDBConnString,
                        CommandType.StoredProcedure, "Sel_CampaignDetails_By_AgentID ", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetCampDetailsbyAgentId failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return dsCampaignDetails;
        }

        /// <summary>
        /// Get Campaign Details
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetCampaignDetailsByPhoneNum(string phoneNumber, Campaign objCampaign)
        {
            DataSet dsCampaignDetails = null;
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@PhoneNumber", phoneNumber)};

                    dsCampaignDetails = SqlHelper.ExecuteDataset(objCampaign.CampaignDBConnString,
                        CommandType.StoredProcedure, "Sel_CampaignDetails_By_PhoneNumber", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetCampDetailsByPhoneNum failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return dsCampaignDetails;
        }
        

        /// <summary>
        /// Update ResultCode
        /// </summary>
        /// <param name="agentID"></param>
        /// <param name="callresultcode"></param>
        /// <returns></returns>
        public static void UpdateResultCode(Campaign objCampaign,Agent objagent,long uniqueKey,
            int callresultcode, long queryId, string campaignMasterDBConn)
        {
            int result = 0;
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@UniqueKey", uniqueKey),
                            new SqlParameter("@CallResultCode", callresultcode==0?Convert.DBNull:callresultcode),
                            new SqlParameter("@AgentID", objagent.AgentID),
                            new SqlParameter("@AgentName", objagent.AgentName),
                            new SqlParameter("@QueryID", queryId)
                           };

                    result = SqlHelper.ExecuteNonQuery(objCampaign.CampaignDBConnString,
                          CommandType.StoredProcedure, "Upd_CampaignResultCode", sparam_s);
                    if (result > 0)
                    {
                        AgentActivityInsertUpdate(campaignMasterDBConn, objagent);
                    }
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in UpdateResultCode failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    };
                } 
            }
        }


        /// <summary>
        /// Agent activity status insert or update
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="agentActivity"></param>
        /// <returns></returns>
        public static long InsertUpdateStation(string campaignMasterDBConn,
            Station objStation)
        {
            long stationID = 0;
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@StationID", objStation.StationID),
                            new SqlParameter("@StationIP", objStation.StationIP),
                            new SqlParameter("@StationNumber", objStation.StationNumber),
                            new SqlParameter("@AllwaysOffHook", objStation.AllwaysOffHook)
                            };

                    stationID = (long)SqlHelper.ExecuteScalar(campaignMasterDBConn,
                        CommandType.StoredProcedure, "InsUpd_AgentStation", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Cannot insert duplicate key") >= 0)
                    {
                        throw new Exception("DuplicateColumnException");
                    }
                    else
                    {
                        if (ex.Message.IndexOf("Rerun the transaction") > 0)
                        {
                            if (i < m_MaxTransactionRetries)
                            {
                                DebugLogger.Write("Transaction in InsertUpdateStation failed, re-running SQL transaction.");
                                continue;
                            }
                            else
                            {
                                DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                                throw ex;
                            }
                        }
                        else
                        {
                            throw ex;
                        }
                    }
                } 
            }
            return stationID;
        }

        /// <summary>
        /// Get Station List
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static Station GetStationByStationID(string strCampaignMasterDBConn, long stationID)
        {
            Station objStation = new Station();

            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlDataReader drStation;
                    SqlParameter[] sparm_s = new SqlParameter[]{
                                        new SqlParameter("@StationID",stationID)
                                    };
                    drStation = SqlHelper.ExecuteReader(strCampaignMasterDBConn,
                                CommandType.StoredProcedure, "Dbo.Sel_Station_Dtl", sparm_s);

                    if (drStation.Read())
                    {
                        objStation.StationID = stationID;
                        objStation.StationIP = drStation["StationIP"].ToString();
                        objStation.StationNumber = drStation["StationNumber"].ToString();
                        objStation.AllwaysOffHook = (bool)drStation["AllwaysOffHook"];
                    }
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetStationbyStationId failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return objStation;
        }


        /// <summary>
        /// Get Station List
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetStationList(string strCampaignMasterDBConn)
        {
            DataSet dsStationList = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsStationList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                        CommandType.StoredProcedure, "Dbo.Sel_Station_List");
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in SelStationList failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return dsStationList;
        }

        /// <summary>
        /// Delete Station 
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="stationID"></param>
        /// <returns></returns>
        public static int DeleteStation(string campaignMasterDBConn, long stationID)
        {
            int result = 0;
            SqlParameter param = new SqlParameter("@StationID", stationID);
            try
            {
                result = SqlHelper.ExecuteNonQuery(campaignMasterDBConn,
                     CommandType.StoredProcedure, "Dbo.DEL_AgentStation", param);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return result;
        }
        /// <summary>
        /// Insert Phone num for campaign in Manual Dial 
        /// </summary>
        /// <param name="objCampaign"></param>
        /// <param name="agentID"></param>
        /// <param name="phoneNum"></param>
        public static int InsertCampaignManualDial(Campaign objCampaign, long agentID, string agentName, string phoneNum)
        {   
            int result = 0;
            SqlParameter[] param = new SqlParameter[]{   
                                                    new SqlParameter("@AgentID", agentID),
                                                    new SqlParameter("@AgentName", agentName),
                                                    new SqlParameter("@PhoneNum",phoneNum)
                                                };
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    object obj = SqlHelper.ExecuteScalar(objCampaign.CampaignDBConnString,
                         CommandType.StoredProcedure, "Dbo.Ins_CampaignManualDial", param);
                    try
                    {
                        result = Convert.ToInt32(obj);
                    }
                    catch { }
                    break;

                }
                catch (SqlException ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in InsertManualDial failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return result;
        }

        /// <summary>
        /// Checks whether dialer is running or not
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static bool IsDialerRunning(string strCampaignMasterDBConn)
        {
            bool isRunning = false;
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    object result = SqlHelper.ExecuteScalar(strCampaignMasterDBConn,
                        CommandType.StoredProcedure, "IsDialerRunning");
                    isRunning = Convert.ToBoolean(result);
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in IsDialerRunning failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
            return isRunning;
        }


        /// <summary>
        /// To transfer call
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="UniqueKey"></param>
        public static void AddCampaignTransferCall(string campaignDBConn,
            long UniqueKey, Agent agent, string campaignMasterDBConn, string offsiteNumber)
        {
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@Uniquekey", UniqueKey),
                new SqlParameter("@OffsiteNumber", offsiteNumber)
            };
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlHelper.ExecuteNonQuery(campaignDBConn,
                        CommandType.StoredProcedure, "Upd_CampaignTransferCall", sparam_s);

                    try
                    {
                        AgentActivityInsertUpdate(campaignMasterDBConn, agent);
                    }
                    catch { }
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in AddCampTransferCall failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }
                    else
                    {
                        throw ex;
                    }
                } 
            }
        }
    }
}
