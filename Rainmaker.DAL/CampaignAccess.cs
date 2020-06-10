using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Rainmaker.Common.DomainModel;
using Rainmaker.DAL.Properties;

namespace Rainmaker.DAL
{
    public static class CampaignAccess
    {
        private static int m_MaxTransactionRetries = 5;

        static CampaignAccess()
        {
            try
            {
                m_MaxTransactionRetries = Settings.Default.MaxTransactionRetries;
            }
            catch { }
        }

        #region Campaigns

        /// <summary>
        /// Campaign Insert/Update
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <param name="strMasterDBConn"></param>
        /// <param name="objCampaign"></param>
        /// <returns></returns>
        public static Campaign CampaignInsertUpdate(string strCampaignMasterDBConn, string strMasterDBConn,
            Campaign objCampaign, string campaignDBScriptPath, string campaignDataScriptPath)
        {
            bool isCampaignCreated = false;
            string campaignDBName = string.Empty;
            using (SqlConnection connect = new SqlConnection(strCampaignMasterDBConn))
            {
                connect.Open();

                using (SqlTransaction transaction = connect.BeginTransaction())
                {
                    try
                    {
                        long campaignID = objCampaign.CampaignID;
                        SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@CampaignID",objCampaign.CampaignID),
                            new SqlParameter("@Description",objCampaign.Description),  
                            new SqlParameter("@ShortDescription",objCampaign.ShortDescription),
                            new SqlParameter("@FundRaiserDataTracking",objCampaign.FundRaiserDataTracking),
                            new SqlParameter("@RecordLevelCallHistory",objCampaign.RecordLevelCallHistory),
                            new SqlParameter("@OnsiteTransfer",objCampaign.OnsiteTransfer),
                            new SqlParameter("@EnableAgentTraining",objCampaign.EnableAgentTraining),
                            new SqlParameter("@AllowDuplicatePhones",objCampaign.AllowDuplicatePhones),
                            new SqlParameter("@Allow7DigitNums",objCampaign.Allow7DigitNums),
                            new SqlParameter("@Allow10DigitNums",objCampaign.Allow10DigitNums),
                            new SqlParameter("@CampaignDBConnString",objCampaign.CampaignDBConnString),
                            new SqlParameter("@StatusID",objCampaign.StatusID),
                            new SqlParameter("IsDeleted",objCampaign.IsDeleted),
                            new SqlParameter("FlushCallQueueOnIdle",objCampaign.FlushCallQueueOnIdle),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date),
                            new SqlParameter("@OutboundCallerID",objCampaign.OutboundCallerID),
                            new SqlParameter("@DuplicateRule",objCampaign.DuplicateRule)};


                        objCampaign.CampaignID = (long)SqlHelper.ExecuteScalar(transaction,
                            CommandType.StoredProcedure, "InsUpd_Campaign", sparam_s);

                        if (campaignID != objCampaign.CampaignID)
                        {
                            CreateDatabase(strMasterDBConn, objCampaign.ShortDescription);
                            isCampaignCreated = true;

                            campaignDBName = objCampaign.ShortDescription;

                            string strOtherCommands = "";
                            if (!objCampaign.AllowDuplicatePhones)
                            {
                                strOtherCommands = "CREATE UNIQUE NONCLUSTERED INDEX IX_Campaign_PhoneNum ON dbo.Campaign(PhoneNum) ON [PRIMARY]";
                            }
                            bool is7digits = objCampaign.Allow7DigitNums;
                            CreateTables(strMasterDBConn, campaignDBName, strOtherCommands, true, is7digits);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        //Code to delete the created database, if exception arises after creation od database.
                        DebugLogger.Write("Clone campaign exception: " + ex.Message);
                        transaction.Rollback();
                        if (isCampaignCreated && campaignDBName != string.Empty)
                        {
                            DropCampaignDataBase(strMasterDBConn, campaignDBName);
                        }
                        if (ex.Message.Contains("dbo.Campaign") || ex.Message.Contains("IX_Campaign"))
                        {

                            throw new Exception("CampaignDuplicateEntityException", ex);
                        }
                        else
                            throw ex;
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
            return objCampaign;

        }


        /// <summary>
        /// ScrubDNC
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <param name="strMasterDBConn"></param>
        /// <param name="objCampaign"></param>
        /// <returns></returns>
        public static void ScrubDNC(string campaignDBConnString)
        {
            int count = 0;
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    count = (int)SqlHelper.ExecuteScalar(campaignDBConnString,
                         CommandType.StoredProcedure, "ScrubDNC");
                    break;
                }
                catch (SqlException ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in ScrubDNC failed, re-running SQL transaction.");
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


        public static Campaign CloneCampaign(
            string strCampaignMasterDBConn, 
            string strMasterDBConn,
            Campaign objCampaign, 
            CloneInfo cloneInfo
        )
        {
            bool isCampaignCreated = false;
            string campaignDBName = string.Empty;
            using (SqlConnection connect = new SqlConnection(strCampaignMasterDBConn))
            {
                connect.Open();

                using (SqlTransaction transaction = connect.BeginTransaction())
                {
                    try
                    {
                        long campaignID = objCampaign.CampaignID;
                        SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@CampaignID",objCampaign.CampaignID),
                            new SqlParameter("@Description",objCampaign.Description),  
                            new SqlParameter("@ShortDescription",objCampaign.ShortDescription),
                            new SqlParameter("@FundRaiserDataTracking",objCampaign.FundRaiserDataTracking),
                            new SqlParameter("@RecordLevelCallHistory",objCampaign.RecordLevelCallHistory),
                            new SqlParameter("@OnsiteTransfer",objCampaign.OnsiteTransfer),
                            new SqlParameter("@EnableAgentTraining",objCampaign.EnableAgentTraining),
                            new SqlParameter("@AllowDuplicatePhones",objCampaign.AllowDuplicatePhones),
                            new SqlParameter("@Allow7DigitNums",objCampaign.Allow7DigitNums),
                            new SqlParameter("@Allow10DigitNums",objCampaign.Allow10DigitNums),
                            new SqlParameter("@CampaignDBConnString",objCampaign.CampaignDBConnString),
                            new SqlParameter("@StatusID",objCampaign.StatusID),
                            new SqlParameter("IsDeleted",objCampaign.IsDeleted),
                            new SqlParameter("FlushCallQueueOnIdle",objCampaign.FlushCallQueueOnIdle),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date),
                            new SqlParameter("@OutboundCallerID",objCampaign.OutboundCallerID),
                            new SqlParameter("@DuplicateRule",objCampaign.DuplicateRule)};

                        DebugLogger.Write(string.Format("Executing insert_update campaign stored procedure."));
                        objCampaign.CampaignID = (long)SqlHelper.ExecuteScalar(transaction,
                            CommandType.StoredProcedure, "InsUpd_Campaign", sparam_s);

                        if (campaignID != objCampaign.CampaignID)
                        {
                            DebugLogger.Write(string.Format("Beginning creation of new campaign database '{0}'.", objCampaign.ShortDescription));
                            CreateDatabase(strMasterDBConn, objCampaign.ShortDescription);
                            isCampaignCreated = true;

                            campaignDBName = objCampaign.ShortDescription;

                            string strOtherCommands = "";
                            if (!objCampaign.AllowDuplicatePhones)
                            {
                                strOtherCommands = "CREATE UNIQUE NONCLUSTERED INDEX IX_Campaign_PhoneNum ON dbo.Campaign(PhoneNum) ON [PRIMARY]";
                            }
                            DebugLogger.Write(string.Format("Beginning creation of new tables in campaign database '{0}'.", objCampaign.ShortDescription));
                            CreateTables(strMasterDBConn, campaignDBName, strOtherCommands, true, objCampaign.Allow7DigitNums);
                            DebugLogger.Write(string.Format("Beginning check for fund raising data tracking for '{0}'.", objCampaign.ShortDescription));
                            // Code the pledge amount column is added.
                            if (objCampaign.FundRaiserDataTracking)
                            {
                                //-------------------------------------
                                // Don: New stored procedure add for
                                // adding the PledgeAmount column to 
                                // the new database.
                                //-------------------------------------
                                SqlParameter[] sparam_db = new SqlParameter[]{
                                   new SqlParameter("@DBName",objCampaign.ShortDescription)};
                                SqlHelper.ExecuteNonQuery(transaction,
                                                          CommandType.StoredProcedure,
                                                          "Add_PledgeAmount",
                                                          sparam_db);
                            }

                            sparam_s = new SqlParameter[]{
                            new SqlParameter("@ParentCampaignID",cloneInfo.ParentCampaignId),
                            new SqlParameter("@DBName",objCampaign.ShortDescription),  
                            new SqlParameter("@IncludeResultCodes",cloneInfo.IncludeResultCodes),
                            new SqlParameter("@IncludeQueries",cloneInfo.IncludeQueries),
                            new SqlParameter("@IncludeOptions",cloneInfo.IncludeOptions),
                            new SqlParameter("@IncludeFields",cloneInfo.IncludeFields),
                            new SqlParameter("@IncludeData",cloneInfo.IncludeData),
                            new SqlParameter("@IncludeScripts",cloneInfo.IncludeScripts),
                            new SqlParameter("@FullCopy",cloneInfo.FullCopy)};
                            //,new SqlParameter("@RecordingsPath",cloneInfo.RecordingsPath)};

                            string strSQL = "Clone_Campaign " + cloneInfo.ParentCampaignId + ",'"
                                                              + objCampaign.ShortDescription + "',"
                                                              + cloneInfo.IncludeResultCodes + ","
                                                              + cloneInfo.IncludeQueries + ","
                                                              + cloneInfo.IncludeOptions + ","
                                                              + cloneInfo.IncludeFields + ","
                                                              + cloneInfo.IncludeData + ","
                                                              + cloneInfo.IncludeScripts + ","
                                                              + cloneInfo.FullCopy; // + ",";
                                                              //+ cloneInfo.RecordingsPath;


                            // DebugLogger.Write(string.Format("Executing clone procedure of parent campaign ID {0} to '{1}'.", cloneInfo.ParentCampaignId, objCampaign.ShortDescription));
                            DebugLogger.Write(string.Format("Calling Clone_Campaign stored procedure for '{0}'.", objCampaign.ShortDescription));
                            SqlHelper.ExecuteNonQuery(transaction,
                                                      CommandType.StoredProcedure, "Clone_Campaign", sparam_s);

                            DebugLogger.Write(string.Format("Clone procedure complete."));
                            //Clone Data based on the options
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        //Code to delete the created database, if exception arises after creation od database.
                        DebugLogger.Write("Clone campaign exception during stored procedure: " + ex.Message);
                        transaction.Rollback();
                        if (isCampaignCreated && campaignDBName != string.Empty)
                        {
                            DropCampaignDataBase(strMasterDBConn, campaignDBName);
                        }
                        if (ex.Message.Contains("dbo.Campaign") || ex.Message.Contains("IX_Campaign"))
                        {
                            throw new Exception("CampaignDuplicateEntityException", ex);
                        }
                        else
                            throw ex;
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
            try
            {
                UpdateCampaignQueriesStats(objCampaign);
            }
            catch { }
            return objCampaign;

        }

        /// <summary>
        /// Campaign status update
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <param name="objCampaign"></param>
        /// <returns></returns>
        public static Campaign CampaignStatusUpdate(string strCampaignMasterDBConn,
            Campaign objCampaign)
        {
            //int result = 0;
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                    new SqlParameter("@CampaignID",objCampaign.CampaignID),
                    new SqlParameter("@StatusID",objCampaign.StatusID),
                    new SqlParameter("@FlushCallQueueOnIdle",objCampaign.FlushCallQueueOnIdle)};

                    objCampaign.CampaignID = (long)SqlHelper.ExecuteScalar(strCampaignMasterDBConn,
                         CommandType.StoredProcedure, "Upd_CampaignQuery_Status", sparam_s);
                    break;
                }
                catch (SqlException ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in CampaignStatusUpdate failed, re-running SQL transaction.");
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
            return objCampaign;

        }

        public static int GetCampaignActiveDialCount(string campaignDBConnString)
        {
            int count = 0;
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    count = (int)SqlHelper.ExecuteScalar(campaignDBConnString,
                         CommandType.StoredProcedure, "Get_CampaignActiveDialCount");
                    break;
                }
                catch (SqlException ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetCampaignActiveDialCount failed, re-running SQL transaction.");
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
            return count;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <param name="objCampaign"></param>
        /// <returns></returns>
        public static Campaign CampaignDialStatusUpdate(string strCampaignMasterDBConn,
            Campaign objCampaign)
        {
            //int result = 0;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                    new SqlParameter("@CampaignID",objCampaign.CampaignID),                    
                    new SqlParameter("@DialAllNumbers",objCampaign.DialAllNumbers)};

                    objCampaign.CampaignID = (long)SqlHelper.ExecuteScalar(strCampaignMasterDBConn,
                         CommandType.StoredProcedure, "Upd_Campaign_DialStatus", sparam_s);
                    break;
                }
                catch (SqlException ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in CampaignDialStatusUpdate failed, re-running SQL transaction.");
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
            return objCampaign;

        }

        /// <summary>
        /// Gets campaignlist by campaignID
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="CampaignID"></param>
        /// <returns>DataSet</returns>
        public static Campaign GetCampaignListByCampaignID(string campaignMasterDBConn,
            long CampaignID)
        {
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@CampaignID",CampaignID)
            };

            DataSet dsCampaignList;
            Campaign objCampaign = new Campaign();
            try
            {
                dsCampaignList = SqlHelper.ExecuteDataset(campaignMasterDBConn,
                    CommandType.StoredProcedure, "Sel_Campaign_Dtl", sparam_s);
                if (dsCampaignList.Tables[0].Rows.Count == 1)
                {
                    DataRow r;
                    r = dsCampaignList.Tables[0].Rows[0];
                    objCampaign.CampaignID = (long)r["campaignID"];
                    objCampaign.StatusID = (long)(r["statusID"]);
                    objCampaign.Description = r["description"].ToString();
                    objCampaign.ShortDescription = r["shortDescription"].ToString();
                    objCampaign.CampaignDBConnString = r["campaignDBConnString"].ToString();
                    objCampaign.FundRaiserDataTracking = (bool)r["fundRaiserDataTracking"];
                    objCampaign.RecordLevelCallHistory = (bool)r["recordLevelCallHistory"];
                    objCampaign.OnsiteTransfer = (bool)r["onsiteTransfer"];
                    objCampaign.EnableAgentTraining = (r["EnableAgentTraining"] == Convert.DBNull) ? false : (bool)r["EnableAgentTraining"];
                    objCampaign.FlushCallQueueOnIdle = (bool)r["FlushCallQueueOnIdle"];
                    objCampaign.IsDeleted = (bool)r["IsDeleted"];
                    objCampaign.AllowDuplicatePhones = (bool)r["allowDuplicatePhones"];
                    objCampaign.Allow7DigitNums = (bool)r["Allow7DigitNums"];
                    objCampaign.Allow10DigitNums = (bool)r["Allow10DigitNums"];
                    objCampaign.DuplicateRule = r["DuplicateRule"].ToString();
                    objCampaign.DateCreated = (DateTime)r["dateCreated"];
                    objCampaign.DateModified = (DateTime)r["dateModified"];
                    objCampaign.OutboundCallerID = r["OutboundCallerID"].ToString();

                    try
                    {
                        objCampaign.DialAllNumbers = (bool)r["DialAllNumbers"];
                    }
                    catch { }

                    objCampaign.StartTime = (r["StartTime"] == Convert.DBNull) ? DateTime.MinValue : Convert.ToDateTime(r["StartTime"]);
                    objCampaign.StopTime = (r["StopTime"] == Convert.DBNull) ? DateTime.MinValue : Convert.ToDateTime(r["StopTime"]);
                }

                dsCampaignList.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return objCampaign;
        }

        /// <summary>
        /// Get campaign list
        /// </summary>
        /// <param name="adminDBConn"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetCampaignList(string strCampaignMasterDBConn)
        {
            DataSet dsCampaignList;

            try
            {
                dsCampaignList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                    CommandType.StoredProcedure, "Sel_Campaign_List");
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return dsCampaignList;
        }

        /// <summary>
        /// Get active campaign list
        /// </summary>
        /// <param name="adminDBConn"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetActiveCampaignList(string strCampaignMasterDBConn)
        {
            DataSet dsCampaignList;

            try
            {
                dsCampaignList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                    CommandType.StoredProcedure, "Sel_ActiveCampaign_List");
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return dsCampaignList;
        }

        /// <summary>
        /// Get Campaign ScoreBoard Data
        /// </summary>
        /// <param name="adminDBConn"></param>
        /// <returns>DataSet</returns>
        public static DataSet GetCampaignScoreBoardData(string strCampaignMasterDBConn)
        {
            DataSet dsCampaignScoreboardData;

            try
            {
                dsCampaignScoreboardData = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                    CommandType.StoredProcedure, "SEL_CampaignScoreboardData");
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return dsCampaignScoreboardData;
        }


        // Dummy function for now to fix compiler error.
        public static void DeleteExportedLeads(List<long> list, string strCampaignDBConn)
        {

        }


        /// <summary>
        /// Delete Campaign.
        /// </summary>
        /// <param name="CampaignID">CampaignID.</param>
        public static int DeleteCampaign(string strCampaignMasterDBConn, string strMasterDBConn, long campaignID, string shortDescription)
        {
            int result = 0;
            SqlParameter param = new SqlParameter("@CampaignID", campaignID);
            try
            {
                DropCampaignDataBase(strMasterDBConn, string.Format("[{0}]", shortDescription));

                result = SqlHelper.ExecuteNonQuery(strCampaignMasterDBConn,
                     CommandType.StoredProcedure, "Dbo.DEL_Campaign", param);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// Update campaign Details
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="list"></param>
        public static void UpdateCallDetail(CampaignDetails campaignDetails, int callType, string strCampaignDBConn, long queryId)
        {
            DebugLogger.Write(string.Format("Updating Call Detail executing."));
            int result = 0;
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@UniqueKey", campaignDetails.UniqueKey),
                new SqlParameter("@CallType", callType),
                new SqlParameter("@QueryID", queryId)
            };
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCampaignDBConn,
                     CommandType.StoredProcedure, "Dbo.Upd_Campaign", sparam_s);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Adds Agent id to call detail
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="list"></param>
        public static void AddAgentToCallDetail(CampaignDetails campaignDetails, string strCampaignDBConn, bool isVerification)
        {
            int result = 0;
            DebugLogger.Write(string.Format("Updating agent to call list."));
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@AgentID", campaignDetails.AgentID != null ? campaignDetails.AgentID : Convert.DBNull),
                new SqlParameter("@AgentName", campaignDetails.AgentName),
                new SqlParameter("@UniqueKey",campaignDetails.UniqueKey)
            };
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCampaignDBConn,
                     CommandType.StoredProcedure, "Dbo.Upd_CampaignAgent", sparam_s);
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            // Update to callist **** Removed and all combined in above stored proc
            try
            {
                string ResultCodeDesc = "Transferred to Agent";
                AddResultCodeToCallList(strCampaignDBConn, campaignDetails.UniqueKey, ResultCodeDesc, Convert.ToInt64(campaignDetails.AgentID), "");
            }
            catch { }
        }

        /// <summary>
        /// Adds Agent id, verificatiobn agent id to call detail
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="list"></param>
        public static void AddVerificationAgentToCallDetail(long Uniquekey, long agentID, string agentName, string strCampaignDBConn)
        {
            int result = 0;
            DebugLogger.Write(string.Format("Updating Verification agent to call list: Key:{0}, AiD:{1}", Uniquekey, agentID));
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@AgentID", agentID),
                new SqlParameter("@AgentName", agentName),
                new SqlParameter("@UniqueKey", Uniquekey),
            };
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCampaignDBConn,
                     CommandType.StoredProcedure, "Dbo.Upd_CampaignVerificationAgent", sparam_s);
            }
            catch (SqlException ex)
            {
                throw ex;
            }

            // Update to callist
            try
            {
                string ResultCodeDesc = "Transferred to Agent";
                //if (campaignDetails.AgentID == campaignDetails.VerificationAgentID)
                //{
                //    ResultCodeDesc = "Transferred to Verification";
                //}
                AddVerificationToCallList(strCampaignDBConn, Uniquekey, ResultCodeDesc, agentID);
            }
            catch { }
        }

        public static string GetOffsiteTransferNumber(string strCampaignDBConn, long UniqueKey, string phoneNumber)
        {
            DataSet ds;
            string number = "";
            SqlParameter[] sparams = new SqlParameter[]{
                new SqlParameter("@UniqueKey", UniqueKey),
                new SqlParameter("@PhoneNumber", phoneNumber)
            };
            try
            {
                ds = SqlHelper.ExecuteDataset(strCampaignDBConn, CommandType.StoredProcedure,
                    "Sel_OffsiteTransferNumber", sparams);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow r;
                    r = ds.Tables[0].Rows[0];
                    number = r["OffsiteTransferNumber"].ToString();
                }
                return number;
            }
            catch (SqlException ex)
            {
                DebugLogger.Write(string.Format("Exception {0}", ex.Message));
                throw ex;
            }
        }

        /// <summary>
        /// Add result code to callist 
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="list"></param>
        public static void AddResultCodeToCallList(string strCampaignDBConn, long UniqueKey, string ResultDesc, long agentId, string offsiteTransferNumber)
        {
            int result = 0;
            long veriAgentId = 0;
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@UniqueKey", UniqueKey),
                new SqlParameter("@ResultDesc", ResultDesc),
                new SqlParameter("@AgentId", agentId),
                new SqlParameter("@VerificationAgentId", veriAgentId),
                new SqlParameter("@OffsiteTransferNumber", offsiteTransferNumber)
            };
            try
            {
                DebugLogger.Write(string.Format("Updating agent to call list: Key:{0}, Result:{1}, AiD:{2}, Offsite{3}", UniqueKey, ResultDesc, agentId, offsiteTransferNumber));
                result = SqlHelper.ExecuteNonQuery(strCampaignDBConn,
                     CommandType.StoredProcedure, "Dbo.Upd_Calllist", sparam_s);
            }
            catch (SqlException ex)
            {
                DebugLogger.Write(string.Format("Exception {0}", ex.Message));
                throw ex;
            }
        }

        public static void AddVerificationToCallList(string strCampaignDBConn, long UniqueKey, string ResultDesc, long agentId)
        {
            int result = 0;
            long origAgent = 0;
            string offsiteTransferNumber = "";
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@UniqueKey", UniqueKey),
                new SqlParameter("@ResultDesc", ResultDesc),
                new SqlParameter("@AgentId", origAgent),
                new SqlParameter("@VerificationAgentId", agentId),
                new SqlParameter("@OffsiteTransferNumber", offsiteTransferNumber)
            };
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCampaignDBConn,
                     CommandType.StoredProcedure, "Dbo.Upd_Calllist", sparam_s);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Sets call hangup
        /// </summary>
        /// <param name="uniqueKey"></param>
        /// <param name="strCampaignDBConn"></param>
        public static void SetCallHangup(long uniqueKey, string strCampaignDBConn)
        {
            int result = 0;
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@UniqueKey", uniqueKey)
            };
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCampaignDBConn,
                     CommandType.StoredProcedure, "Dbo.Upd_CampaignHangup", sparam_s);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Checks whether call is hangup
        /// </summary>
        /// <param name="uniqueKey"></param>
        /// <param name="strCampaignDBConn"></param>
        public static bool IsCallHangup(long uniqueKey, string strCampaignDBConn)
        {
            bool isHangUp = false;
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@UniqueKey", uniqueKey)
            };
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    object result = SqlHelper.ExecuteScalar(strCampaignDBConn,
                         CommandType.StoredProcedure, "Dbo.Get_Campaign_HangupTime", sparam_s);
                    if (result != null && result.ToString().Trim() != "")
                    {
                        try
                        {
                            // Check hangup is happened in current session only
                            //DateTime dt = Convert.ToDateTime(result);
                            //if (dt.AddMinutes(30) > DateTime.Now)
                            //{
                            isHangUp = true;
                            //}
                        }
                        catch { }
                    }
                    break;

                }
                catch (SqlException ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in IsCallHangup failed, re-running SQL transaction.");
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
            return isHangUp;
        }

        /// <summary>
        /// Log silent call
        /// </summary>
        /// <param name="silentCall"></param>
        /// <param name="strCampaignDBConn"></param>
        public static void LogSilentCall(SilentCall silentCall, string strCampaignDBConn)
        {
            int result = 0;
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@SilentCallID", silentCall.SilentCallID),
                new SqlParameter("@UniqueKey",silentCall.UniqueKey),
                new SqlParameter("@DateTimeofCall",silentCall.DateTimeofCall)
            };
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCampaignDBConn,
                     CommandType.StoredProcedure, "Dbo.InsUpd_SilentCallList", sparam_s);

                AddResultCodeToCallList(strCampaignDBConn, silentCall.UniqueKey, "Dropped", 0, "");
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update call details , completion time
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="list"></param>
        public static void UpdateCallCompletion(CampaignDetails campaignDetails, string strCampaignDBConn)
        {
            int result = 0;
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@UniqueKey",campaignDetails.UniqueKey),
                new SqlParameter("@CallDuration", campaignDetails.CallDuration != null ? 
                            campaignDetails.CallDuration : Convert.DBNull)
            };
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCampaignDBConn,
                     CommandType.StoredProcedure, "Dbo.Upd_CampaignCallCompletion", sparam_s);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update Campaign Details
        /// </summary>
        /// <param name="objCampaign"></param>
        /// <param name="objCampaigndetails"></param>
        /// <returns></returns>
        public static void UpdateCampaignDetails(Campaign objCampaign,
            string strCampaignDetailsQuery)
        {
            DebugLogger.Write(string.Format("Updating Call Detail executing query '{0}'.", strCampaignDetailsQuery));
            try
            {
                int result = SqlHelper.ExecuteNonQuery(objCampaign.CampaignDBConnString,
                 CommandType.Text, strCampaignDetailsQuery);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DataSet GetCampaignDetailsByKey(string CampaignDB, long key, long queryId)
        {
            DataSet ds;
            try
            {
                string strSql = string.Format("SELECT *,{0} as QueryId FROM CAMPAIGN WHERE UniqueKey = {1}", queryId.ToString(), key.ToString());
                ds = SqlHelper.ExecuteDataset(CampaignDB,
                    CommandType.Text, strSql);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return ds;
        }


        public static DataSet GetManualDailCallDetails(string strCampaignMasterDBConn, long agentID)
        {
            DataSet ds;
            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                        new SqlParameter("@AgentID", agentID)};
                ds = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                    CommandType.StoredProcedure, "Get_CampaignManualDial", sparam_s);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return ds;
        }

        #endregion

        #region ResultCodes

        /// <summary>
        /// returns result codes
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public static DataSet GetResultCodes(Campaign campaign)
        {
            DataSet ds;
            try
            {

                ds = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString, CommandType.StoredProcedure,
                    "Sel_ResultCode_List");
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return ds;
        }

        /// <summary>
        /// Result code insert/update
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="objResultCode"></param>
        /// <returns></returns>
        public static ResultCode ResultCodeInsertUpdate(Campaign campaign,
           ResultCode objResultCode)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@ResultCodeID",objResultCode.ResultCodeID),
                            new SqlParameter("@Description",objResultCode.Description),  
                            new SqlParameter("@Presentation",objResultCode.Presentation),
                            new SqlParameter("@Redialable",objResultCode.Redialable),
                            new SqlParameter("@RecycleInDays",objResultCode.RecycleInDays),
                            new SqlParameter("@Lead",objResultCode.Lead),
                            new SqlParameter("@MasterDNC",objResultCode.MasterDNC),
                            new SqlParameter("@NeverCall",objResultCode.NeverCall),
                            new SqlParameter("@VerifyOnly",objResultCode.VerifyOnly),
                            new SqlParameter("@CountAsLiveContact", objResultCode.LiveContact),
                            new SqlParameter("@DialThroughAll",objResultCode.DialThroughAll),
                            new SqlParameter("@ShowDeletedResultCodes",objResultCode.ShowDeletedResultCodes),
                            new SqlParameter("@DateDeleted",objResultCode.DateDeleted < DateTime.Now.Date ? Convert.DBNull : objResultCode.DateDeleted),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)
                };


                    objResultCode.ResultCodeID = (long)SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "InsUpd_ResultCode", sparam_s);
                    break;

                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in ResultCodeInsertUpdate failed, re-running SQL transaction.");
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

            return objResultCode;
        }

        /// <summary>
        /// Returns resultcode by id
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="resultCodeID"></param>
        /// <returns></returns>
        public static ResultCode GetResultCodeByResultCodeID(Campaign campaign, long resultCodeID)
        {
            ResultCode resultCode = new ResultCode();
            DataSet ds;

            try
            {
                SqlParameter[] sparams = new SqlParameter[]{
                    new SqlParameter("@ResultCodeID", resultCodeID)};

                ds = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString, CommandType.StoredProcedure,
                    "Sel_ResultCode_Dtl", sparams);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow r;
                    r = ds.Tables[0].Rows[0];
                    resultCode.ResultCodeID = (long)r["ResultCodeID"];
                    resultCode.Description = r["Description"].ToString();
                    resultCode.Presentation = (bool)r["Presentation"];
                    resultCode.Redialable = (bool)r["Redialable"];
                    resultCode.RecycleInDays = (Int16)r["RecycleInDays"];
                    resultCode.Lead = (bool)r["Lead"];
                    resultCode.MasterDNC = (bool)r["MasterDNC"];
                    resultCode.NeverCall = (bool)r["NeverCall"];
                    resultCode.VerifyOnly = (bool)r["VerifyOnly"];
                    resultCode.LiveContact = (bool)r["CountAsLiveContact"];
                    resultCode.DialThroughAll = (bool)r["DialThroughAll"];
                    resultCode.ShowDeletedResultCodes = (bool)r["ShowDeletedResultCodes"];

                    //if (!r.IsNull("DateDeleted"))
                    //    resultCode.DateDeleted = (DateTime)r["DateDeleted"];

                    resultCode.DateCreated = (DateTime)r["DateCreated"];
                    resultCode.DateModified = (DateTime)r["DateModified"];
                }
                ds.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return resultCode;
        }

        #endregion

        #region OtherParameter

        /// <summary>
        /// Returns other parameter details
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public static OtherParameter GetOtherParameter(Campaign campaign)
        {
            OtherParameter otherParams = new OtherParameter();

            DataSet ds;

            try
            {

                ds = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString, CommandType.StoredProcedure,
                    "Sel_OtherParameter_List");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow r;
                    r = ds.Tables[0].Rows[0];

                    otherParams.OtherParameterID = (long)r["OtherParameterID"];
                    otherParams.CallTransfer = Convert.ToInt16(r["CallTransfer"]);
                    otherParams.StaticOffsiteNumber = r["StaticOffsiteNumber"] == Convert.DBNull ? "" : r["StaticOffsiteNumber"].ToString();
                    otherParams.TransferMessageEnable = (bool)r["TransferMessage"];
                    otherParams.TransferMessage = r["AutoPlayMessage"] == Convert.DBNull ? "" : r["AutoPlayMessage"].ToString();
                    otherParams.HoldMessage = r["HoldMessage"] == Convert.DBNull ? "" : r["HoldMessage"].ToString();
                    otherParams.AllowManualDial = (bool)r["AllowManualDial"];
                    otherParams.StartingLine = Convert.ToInt16(r["StartingLine"] == Convert.DBNull ? Convert.ToInt16(-2) : Convert.ToInt16(r["StartingLine"]));
                    otherParams.EndingLine = Convert.ToInt16(r["EndingLine"] == Convert.DBNull ? Convert.ToInt16(-2) : Convert.ToInt16(r["EndingLine"]));
                    otherParams.AllowCallBacks = Convert.ToInt16(r["AllowCallBacks"]);
                    otherParams.AlertSupervisorOnCallbacks = Convert.ToInt16(r["AlertSupervisorOnCallbacks"] == Convert.DBNull ? Convert.ToInt16(-2) : Convert.ToInt16((r["AlertSupervisorOnCallbacks"])));
                    otherParams.QueryStatisticsInPercent = (bool)r["QueryStatisticsInPercent"];
                    otherParams.DateCreated = (DateTime)r["DateCreated"];
                    otherParams.DateModified = (DateTime)r["DateModified"];

                }

                ds.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return otherParams;
        }

        /// <summary>
        /// Other parameters insert/update
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="objOtherParameter"></param>
        /// <returns></returns>
        public static OtherParameter OtherParameterInsertUpdate(Campaign campaign,
            OtherParameter objOtherParameter)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@OtherParameterID",objOtherParameter.OtherParameterID),
                            new SqlParameter("@CallTransfer",objOtherParameter.CallTransfer),  
                            new SqlParameter("@StaticOffsiteNumber",objOtherParameter.StaticOffsiteNumber==string.Empty?
                            Convert.DBNull:objOtherParameter.StaticOffsiteNumber),
                            new SqlParameter("@TransferMessage",objOtherParameter.TransferMessageEnable),
                            new SqlParameter("@AutoPlayMessage",objOtherParameter.TransferMessage==string.Empty?
                            Convert.DBNull:objOtherParameter.TransferMessage),
                            new SqlParameter("@HoldMessage",objOtherParameter.HoldMessage==string.Empty?
                            Convert.DBNull:objOtherParameter.HoldMessage),
                            new SqlParameter("@AllowManualDial",objOtherParameter.AllowManualDial),
                            new SqlParameter("@StartingLine",objOtherParameter.StartingLine==-2?
                            Convert.DBNull:objOtherParameter.StartingLine),
                            new SqlParameter("@EndingLine",objOtherParameter.EndingLine==-2?
                            Convert.DBNull:objOtherParameter.EndingLine),
                            new SqlParameter("@AllowCallBacks",objOtherParameter.AllowCallBacks),
                            new SqlParameter("@AlertSupervisorOnCallbacks",objOtherParameter.AlertSupervisorOnCallbacks==-2?
                            Convert.DBNull:objOtherParameter.AlertSupervisorOnCallbacks),
                            new SqlParameter("@QueryStatisticsInPercent",objOtherParameter.QueryStatisticsInPercent),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)};



                    objOtherParameter.OtherParameterID = (long)SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "InsUpd_OtherParameter", sparam_s);
                    break;
                }
                catch (Exception ex)
                {

                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction inOtherParameterInsertUpdate failed, re-running SQL transaction.");
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
            return objOtherParameter;
        }

        #endregion

        #region DialingParameters

        /// <summary>
        /// Returns Dialing Parameters
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public static DialingParameter GetDialingParameter(Campaign campaign)
        {
            DialingParameter dialParams = new DialingParameter();

            DataSet ds;

            try
            {

                ds = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString, CommandType.StoredProcedure,
                    "Sel_DialingParameter_List");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow r;
                    r = ds.Tables[0].Rows[0];

                    dialParams.DailingParameterID = (long)r["DailingParameterID"];
                    dialParams.PhoneLineCount = Convert.ToInt16(r["PhoneLineCount"]);
                    dialParams.DropRatePercent = Convert.ToInt16(r["DropRatePercent"]);
                    dialParams.RingSeconds = Convert.ToInt16(r["RingSeconds"]);
                    dialParams.MinimumDelayBetweenCalls = Convert.ToInt16(r["MinimumDelayBetweenCalls"]);
                    dialParams.DialingMode = Convert.ToInt16(r["DialingMode"]);

                    dialParams.SevenDigitPrefix = Convert.ToString(r["SevenDigitPrefix"]);
                    dialParams.SevenDigitSuffix = Convert.ToString(r["SevenDigitSuffix"]);
                    dialParams.TenDigitPrefix = Convert.ToString(r["TenDigitPrefix"]);
                    dialParams.TenDigitPrefix = Convert.ToString(r["TenDigitSuffix"]);
                    dialParams.ColdCallScriptID = (long)r["ColdCallScriptID"];
                    dialParams.VerificationScriptID = (long)r["VerificationScriptID"];
                    dialParams.InboundScriptID = (long)r["InboundScriptID"];
                    dialParams.AMCallTimes = Convert.ToInt16(r["AMCallTimes"]);
                    dialParams.PMCallTimes = Convert.ToInt16(r["PMCallTimes"]);
                    dialParams.WeekendCallTimes = Convert.ToInt16(r["WeekendCallTimes"]);
                    dialParams.AMDialingStart = (DateTime)r["AMDialingStart"];
                    dialParams.AMDialingStop = (DateTime)r["AMDialingStop"];
                    dialParams.PMDialingStart = (DateTime)r["PMDialingStart"];
                    dialParams.PMDialingStop = (DateTime)r["PMDialingStop"];
                    dialParams.AnsMachDetect = Convert.ToInt16(r["AnsMachDetect"]);
                    dialParams.DateCreated = (DateTime)r["DateCreated"];
                    dialParams.DateModified = (DateTime)r["DateModified"];
                    dialParams.ErrorRedialLapse = (int)r["ErrorRedialLapse"];
                    dialParams.NoAnswerRedialLapse = (int)r["NoAnswerRedialLapse"];
                    dialParams.BusyRedialLapse = (int)r["BusyRedialLapse"];
                    try
                    {
                        dialParams.ChannelsPerAgent = (decimal)r["ChannelsPerAgent"];
                    }
                    catch { }
                    try
                    {
                        dialParams.DefaultCallLapse = (int)r["DefaultCallLapse"];
                    }
                    catch { }

                    try
                    {

                        dialParams.AnsweringMachineDetection = (bool)r["AnsweringMachineDetection"];
                        dialParams.AnsweringMachineMessage = r["AnsweringMachineMessage"] == Convert.DBNull ? "" : r["AnsweringMachineMessage"].ToString();
                        dialParams.HumanMessageEnable = r["HumanMessageEnable"] == Convert.DBNull ? false : (bool)r["HumanMessageEnable"];
                        dialParams.HumanMessage = r["HumanMessage"] == Convert.DBNull ? "" : r["HumanMessage"].ToString();
                        dialParams.SilentCallMessageEnable = r["SilentCallMessageEnable"] == Convert.DBNull ? false : (bool)r["SilentCallMessageEnable"];
                        dialParams.SilentCallMessage = r["SilentCallMessage"] == Convert.DBNull ? "" : r["SilentCallMessage"].ToString();
                    }
                    catch { }
                }

                ds.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return dialParams;
        }

        /// <summary>
        /// dialing parameters insert/update
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="objDialingParameter"></param>
        /// <returns></returns>
        public static DialingParameter DialingParameterInsertUpdate(Campaign campaign,
            DialingParameter objDialingParameter)
        {
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@DailingParameterID",objDialingParameter.DailingParameterID),
                            new SqlParameter("@PhoneLineCount",objDialingParameter.PhoneLineCount),  
                            new SqlParameter("@DropRatePercent",objDialingParameter.DropRatePercent),
                            new SqlParameter("@RingSeconds",objDialingParameter.RingSeconds),
                            new SqlParameter("@MinimumDelayBetweenCalls",objDialingParameter.MinimumDelayBetweenCalls),
                            new SqlParameter("@DialingMode",objDialingParameter.DialingMode),
                            new SqlParameter("@AnsweringMachineDetection",objDialingParameter.AnsweringMachineDetection),
                            //new SqlParameter("@MessageRecordingTool",objDialingParameter.MessageRecordingTool==string.Empty?
                            //Convert.DBNull:objDialingParameter.MessageRecordingTool),
                            new SqlParameter("@SevenDigitPrefix",objDialingParameter.SevenDigitPrefix),
                            new SqlParameter("@TenDigitPrefix",objDialingParameter.TenDigitPrefix),
                            new SqlParameter("@SevenDigitSuffix",objDialingParameter.SevenDigitSuffix),
                            new SqlParameter("@TenDigitSuffix",objDialingParameter.TenDigitSuffix),
                            new SqlParameter("@ColdCallScriptID",objDialingParameter.ColdCallScriptID),
                            new SqlParameter("@VerificationScriptID",objDialingParameter.VerificationScriptID),
                            new SqlParameter("@InboundScriptID",objDialingParameter.InboundScriptID),
                            new SqlParameter("@AMCallTimes",objDialingParameter.AMCallTimes),
                            new SqlParameter("@PMCallTimes",objDialingParameter.PMCallTimes),
                            new SqlParameter("@WeekendCallTimes",objDialingParameter.WeekendCallTimes),
                            new SqlParameter("@AMDialingStart",objDialingParameter.AMDialingStart),
                            new SqlParameter("@AMDialingStop",objDialingParameter.AMDialingStop),
                            new SqlParameter("@PMDialingStart",objDialingParameter.PMDialingStart),
                            new SqlParameter("@PMDialingStop",objDialingParameter.PMDialingStop),
                            new SqlParameter("@AnsMachDetect",objDialingParameter.AnsMachDetect),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date),
                            new SqlParameter("@ErrorRedialLapse",objDialingParameter.ErrorRedialLapse),
                            new SqlParameter("@BusyRedialLapse",objDialingParameter.BusyRedialLapse),
                            new SqlParameter("@NoAnswerRedialLapse",objDialingParameter.NoAnswerRedialLapse),
                            new SqlParameter("@ChannelsPerAgent",objDialingParameter.ChannelsPerAgent),
                            new SqlParameter("@DefaultCallLapse",objDialingParameter.DefaultCallLapse),
                            new SqlParameter("@AnsweringMachineMessage", objDialingParameter.AnsweringMachineMessage),
                            new SqlParameter("@HumanMessageEnable",objDialingParameter.HumanMessageEnable),
                            new SqlParameter("@HumanMessage",objDialingParameter.HumanMessage==string.Empty?
                            Convert.DBNull:objDialingParameter.HumanMessage),
                            new SqlParameter("@SilentCallMessageEnable",objDialingParameter.SilentCallMessageEnable),
                            new SqlParameter("@SilentCallMessage",objDialingParameter.SilentCallMessage==string.Empty?
                            Convert.DBNull:objDialingParameter.SilentCallMessage)
                            };


                    objDialingParameter.DailingParameterID = (long)SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "InsUpd_DialingParameter", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in DialingParameterInsertUpdate failed, re-running SQL transaction.");
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
            return objDialingParameter;

        }

        #endregion

        #region DialerActivity

        public static long InsertDialerActivity(string strRainmakerMasterDBConn, DateTime dtConnectTime)
        {
            long lDialerActivityID = 0;
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@ConnectTime",dtConnectTime)
                            };


                    lDialerActivityID = (long)SqlHelper.ExecuteScalar(strRainmakerMasterDBConn,
                         CommandType.StoredProcedure, "Ins_DialerActivity", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in InsertDialerActivity failed, re-running SQL transaction.");
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
            return lDialerActivityID;
        }

        public static void UpdateDialerStart(string strRainmakerMasterDBConn, long lDialerActivityID, DateTime dtDialerStartTime)
        {
            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@DialerActivityID",lDialerActivityID),
                            new SqlParameter("@DialerStartTime",dtDialerStartTime)
                            };


                SqlHelper.ExecuteNonQuery(strRainmakerMasterDBConn,
                     CommandType.StoredProcedure, "Upd_DialerStart", sparam_s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateDialerStop(string strRainmakerMasterDBConn, long lDialerActivityID, DateTime dtDialerStopTime)
        {
            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@DialerActivityID",lDialerActivityID),
                            new SqlParameter("@DialerStopTime",dtDialerStopTime)
                            };


                SqlHelper.ExecuteNonQuery(strRainmakerMasterDBConn,
                     CommandType.StoredProcedure, "Upd_DialerStop", sparam_s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region DigitalizedRecording

        /// <summary>
        /// Returns digitalized recording details 
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public static DigitalizedRecording GetDigitalizedRecording(Campaign campaign)
        {
            DigitalizedRecording digitalParams = new DigitalizedRecording();
            DataSet ds;

            try
            {

                ds = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString, CommandType.StoredProcedure,
                    "Sel_DigitalizedRecording_List");

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow r;
                    r = ds.Tables[0].Rows[0];

                    digitalParams.DigitalizedRecordingID = (long)r["DigitalizedRecordingID"];
                    digitalParams.EnableRecording = (bool)r["EnableRecording"];
                    digitalParams.EnableWithABeep = (bool)r["EnableWithABeep"];
                    digitalParams.StartWithABeep = (bool)r["StartWithABeep"];
                    digitalParams.RecordToWave = (bool)r["RecordToWave"];
                    digitalParams.HighQualityRecording = (bool)r["HighQualityRecording"];
                    digitalParams.RecordingFilePath = r["RecordingFilePath"].ToString();
                    digitalParams.FileNaming = r["FileNaming"].ToString();
                    digitalParams.DateCreated = (DateTime)r["DateCreated"];
                    digitalParams.DateModified = (DateTime)r["DateModified"];
                }

                ds.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return digitalParams;
        }

        /// <summary>
        /// Digitalized recordings insert/update
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="objDigitalizedRecording"></param>
        /// <returns></returns>
        public static DigitalizedRecording DigitalizedRecordingInsertUpdate(Campaign campaign,
            DigitalizedRecording objDigitalizedRecording)
        {
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    long DigitalizedRecordingID = objDigitalizedRecording.DigitalizedRecordingID;
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@DigitalizedRecordingID",objDigitalizedRecording.DigitalizedRecordingID),
                            new SqlParameter("@EnableRecording",objDigitalizedRecording.EnableRecording),  
                            new SqlParameter("@EnableWithABeep",objDigitalizedRecording.EnableWithABeep),
                            new SqlParameter("@StartWithABeep",objDigitalizedRecording.StartWithABeep),
                            new SqlParameter("@RecordToWave",objDigitalizedRecording.RecordToWave),
                            new SqlParameter("@HighQualityRecording",objDigitalizedRecording.HighQualityRecording),
                            new SqlParameter("@RecordingFilePath",objDigitalizedRecording.RecordingFilePath),
                            new SqlParameter("@FileNaming",objDigitalizedRecording.FileNaming),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)};


                    objDigitalizedRecording.DigitalizedRecordingID = (long)SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "InsUpd_DigitalizedRecording", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in DigitRecordingInsertUpdate failed, re-running SQL transaction.");
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

            return objDigitalizedRecording;
        }

        #endregion

        #region Campaign QueryStatus

        /// <summary>
        /// Returns Query status details
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public static DataSet GetCampaignQueryStatus(Campaign campaign)
        {
            CampaignQueryStatus cqStatus = new CampaignQueryStatus();
            DataSet ds = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {

                    ds = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString, CommandType.StoredProcedure,
                        "Sel_CampaignQueryStatus_List");
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetCampaignQueryStatus failed, re-running SQL transaction.");
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
        /// Query status List
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="objCampaignQueryStatus"></param>
        /// <returns></returns>
        public static void UpdateQueryStatus(Campaign campaign, string strQueryCondition, CampaignQueryStatus objCampaignQueryStatus)
        {
            DataSet dsCampaignResults = null;
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsCampaignResults = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString,
                                        CommandType.Text, strQueryCondition);
                    objCampaignQueryStatus.Total = dsCampaignResults.Tables[0].Rows.Count;

                    objCampaignQueryStatus.Available = dsCampaignResults.Tables[0].Rows.Count;
                    CampaignQueryStatusInsertUpdate(campaign, objCampaignQueryStatus);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in UpdateQueryStatus failed, re-running SQL transaction.");
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
        /// Resets all campaigns to idle.  This is done on startup and shutdown of the dialer
        /// </summary>
        /// <param name="strCampaignDBConn"></param>
        /// <param name="queryID"></param>
        /// <returns></returns>
        public static void ResetCampaignsToIdle(string strCampaignMasterDBConn)
        {
            try
            {
                SqlHelper.ExecuteNonQuery(strCampaignMasterDBConn,
                     CommandType.StoredProcedure, "Dbo.ResetCampaignsToIdle");
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return;
        }



        /// <summary>
        /// Query status insert/update
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="objCampaignQueryStatus"></param>
        /// <returns></returns>
        public static void CampaignQueryStatusInsertUpdate(Campaign campaign,
           CampaignQueryStatus objCampaignQueryStatus)
        {

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@CampaignQueryID",objCampaignQueryStatus.CampaignQueryID),
                            new SqlParameter("@QueryID",objCampaignQueryStatus.QueryID),  
                            new SqlParameter("@IsActive",objCampaignQueryStatus.IsActive),
                            new SqlParameter("@Total",objCampaignQueryStatus.Total),
                            new SqlParameter("@Available",objCampaignQueryStatus.Available),
                            new SqlParameter("@Dials",objCampaignQueryStatus.Dials),
                            new SqlParameter("@Talks",objCampaignQueryStatus.Talks),
                            new SqlParameter("@AnsweringMachine",objCampaignQueryStatus.AnsweringMachine),
                            new SqlParameter("@NoAnswer",objCampaignQueryStatus.NoAnswer),
                            new SqlParameter("@Busy",objCampaignQueryStatus.Busy),
                            new SqlParameter("@OpInt",objCampaignQueryStatus.OpInt),
                            new SqlParameter("@Drops",objCampaignQueryStatus.Drops),
                            new SqlParameter("@Failed",objCampaignQueryStatus.Failed),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)};


                    SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                         CommandType.StoredProcedure, "InsUpd_CampaignQueryStatus", sparam_s);
                    break;

                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in CampaignQueryStatusUpdate failed, re-running SQL transaction.");
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
        /// UpdateCampaignQueryStats from dialer
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="objCampaignQueryStatus"></param>
        /// <returns></returns>
        public static void UpdateCampaignQueryStats(Campaign campaign,
           CampaignQueryStatus objCampaignQueryStatus)
        {

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@QueryID",objCampaignQueryStatus.QueryID),
                            new SqlParameter("@Dials",objCampaignQueryStatus.Dials),
                            new SqlParameter("@Talks",objCampaignQueryStatus.Talks),
                            new SqlParameter("@AnsweringMachine",objCampaignQueryStatus.AnsweringMachine),
                            new SqlParameter("@NoAnswer",objCampaignQueryStatus.NoAnswer),
                            new SqlParameter("@Busy",objCampaignQueryStatus.Busy),
                            new SqlParameter("@OpInt",objCampaignQueryStatus.OpInt),
                            new SqlParameter("@Drops",objCampaignQueryStatus.Drops),
                            new SqlParameter("@Failed",objCampaignQueryStatus.Failed),
                            new SqlParameter("@ResultCodeId",objCampaignQueryStatus.ResultCodeId)};

                    SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                         CommandType.StoredProcedure, "Upd_CampaignQuery_Status_FromDialer", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in UpdateCampaignQueryStats failed, re-running SQL transaction.");
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
        /// UpdateCampaignQueryStats from dialer
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="objCampaignQueryStatus"></param>
        /// <returns></returns>
        public static void UpdateCampaignQueryInDialerQueue(long queryID, string campaignDBConnString)
        {

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@QueryID", queryID)};


                    SqlHelper.ExecuteScalar(campaignDBConnString,
                         CommandType.StoredProcedure, "Upd_CampaignQuery_INDialerQueue", sparam_s);
                    break;

                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in UpdateCampaignQueryInDialerQueue failed, re-running SQL transaction.");
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
        /// Query Status update (active/inactive)
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="campaignQueryID"></param>
        /// <param name="isActive"></param>
        public static void CampaignQueryStatusUpdate(Campaign campaign,
           long campaignQueryID, bool isActive, bool isStandby, bool showMessage, bool resetStats)
        {

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@CampaignQueryID", campaignQueryID),                            
                            new SqlParameter("@IsActive", isActive),
                            new SqlParameter("@IsStandby", isStandby),
                            new SqlParameter("@ShowMessage", showMessage),
                            new SqlParameter("@ResetStats", resetStats)};


                    campaignQueryID = (long)SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                        CommandType.StoredProcedure, "Upd_CampaignQuery_Status", sparam_s);
                    break;

                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in CampaignQueryStatusUpdate failed, re-running SQL transaction.");
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

        public static void CampaignQueryStatusUpdateDialer(Campaign campaign,
           long queryID, bool isActive, bool isStandby)
        {

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@QueryID",queryID),                            
                            new SqlParameter("@IsActive",isActive),
                            new SqlParameter("@IsStandby",isStandby)};

                    SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                        CommandType.StoredProcedure, "Upd_Query_Status_FromDialer", sparam_s);
                    break;

                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in CampaignQueryStatusUpdateDialer failed, re-running SQL transaction.");
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
        /// Query Status update (active/inactive)
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="campaignQueryID"></param>
        /// <param name="isActive"></param>
        public static void CampaignQueryComplete(Campaign campaign,
           long QueryID, bool isActive, bool showMessage)
        {

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@QueryID",QueryID),                            
                            new SqlParameter("@IsActive",isActive),
                            new SqlParameter("@IsStandby", false),
                            new SqlParameter("@ShowMessage",true)};


                    QueryID = (long)SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                        CommandType.StoredProcedure, "Upd_CampaignQueryComplete", sparam_s);
                    break;

                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in CampaignQueryComplete failed, re-running SQL transaction.");
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
        /// Campaign Query status insert/update
        /// </summary>
        /// <param name="query"></param>
        /// <param name="transaction"></param>
        /// <param name="recordCount"></param>
        public static void CampaignQueryStatusInsertUpdate(Query query, SqlTransaction transaction, int total, int available, bool isEdit)
        {

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    if (!isEdit)
                    {
                        available = total;
                    }
                    else
                    {
                        if (available < 0)
                        {
                            DataSet dsCampaignResults = SqlHelper.ExecuteDataset(transaction,
                                        CommandType.Text, PrepareDialerQuery(query.QueryCondition, query.QueryID));
                            available = dsCampaignResults.Tables[0].Rows.Count;
                        }
                    }

                    CampaignQueryStatus cqStatus = new CampaignQueryStatus();

                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@CampaignQueryID",cqStatus.CampaignQueryID),
                            new SqlParameter("@QueryID",query.QueryID),  
                            new SqlParameter("@IsActive",cqStatus.IsActive),
                            new SqlParameter("@Total",total < 0 ? Convert.DBNull : total),
                            new SqlParameter("@Available", available < 0 ? Convert.DBNull : available),
                            new SqlParameter("@Dials",cqStatus.Dials),
                            new SqlParameter("@Talks",cqStatus.Talks),
                            new SqlParameter("@AnsweringMachine",cqStatus.AnsweringMachine),
                            new SqlParameter("@NoAnswer",cqStatus.NoAnswer),
                            new SqlParameter("@Busy",cqStatus.Busy),
                            new SqlParameter("@OpInt",cqStatus.OpInt),
                            new SqlParameter("@Drops",cqStatus.Drops),
                            new SqlParameter("@Failed",cqStatus.Failed),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)};


                    SqlHelper.ExecuteScalar(transaction,
                        CommandType.StoredProcedure, "InsUpd_CampaignQueryStatus", sparam_s);
                    break;

                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in CampaignQueryStatusInsertUpdate failed, re-running SQL transaction.");
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
        /// 
        /// </summary>
        /// <param name="strCampaignDBConn"></param>
        /// <param name="availableCount"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static void UpdateAvailableCountToQuery(string strCampaignDBConn, int availableCount, Query query)
        {
            using (SqlConnection connect = new SqlConnection(strCampaignDBConn))
            {
                connect.Open();

                using (SqlTransaction transaction = connect.BeginTransaction())
                {
                    try
                    {

                        //DataSet dsCampaignResults = SqlHelper.ExecuteDataset(transaction,
                        //                CommandType.Text, query.QueryCondition);
                        //int total = dsCampaignResults.Tables[0].Rows.Count;

                        CampaignQueryStatusInsertUpdate(query, transaction, -1, availableCount, true);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        if (ex.Message.IndexOf("IX_Query") >= 0)
                            throw new Exception("DuplicateQueryException");
                        else
                            throw ex;
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
        }

        public static void UpdateCampaignQueriesStats(Campaign campaign)
        {
            DataSet dsQuerList = GetCampaignQueryStatus(campaign);
            if (dsQuerList != null && dsQuerList.Tables[0].Rows.Count > 0)
            {
                using (SqlConnection connect = new SqlConnection(campaign.CampaignDBConnString))
                {
                    connect.Open();

                    using (SqlTransaction transaction = connect.BeginTransaction())
                    {
                        try
                        {

                            DataTable dtQueries = dsQuerList.Tables[0];
                            foreach (DataRow dr in dtQueries.Rows)
                            {

                                try
                                {
                                    Query query = new Query();
                                    query.QueryID = Convert.ToInt64(dr["QueryID"]);
                                    query.QueryCondition = dr["QueryCondition"].ToString();
                                    DataSet dsCampaignData = GetCampaignData(campaign.CampaignDBConnString, query.QueryCondition);
                                    int total = dsCampaignData.Tables[0].Rows.Count;
                                    //DebugLogger.Write(string.Format("Query {0} condition {1} has total of {2} rows.", query.QueryID, query.QueryCondition, total)); 
                                    CampaignQueryStatusInsertUpdate(query, transaction, total, -1, true);
                                }
                                catch { }
                            }
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            connect.Close();
                        }
                    }
                }

            }
        }


        public static string PrepareDialerQuery(string query, long queryId)
        {
            // *** COmmented out, new query architechture - 03.16.11
            StringBuilder sb = new StringBuilder();
            //sb.Append(" AND ( NeverCallFlag=0 or NeverCallFlag IS NULL ) ");
            //sb.Append(" AND (ScheduleDate is null OR DATEDIFF(dd,getdate(),ScheduleDate) <= 0) ");
            //sb.Append(" AND UniqueKey NOT IN ( ");
            //sb.Append(" select distinct UniqueKey from CallList CL ");
            //sb.Append(" INNER JOIN (SELECT MAX(calllistid) AS MaxCallListID FROM calllist GROUP BY uniquekey) MCL ON MCL.MaxCallListID = CL.CallListID  ");
            //sb.Append(" inner join ResultCode on ResultCode.ResultCodeID = CL.ResultCodeID ");
            //sb.AppendFormat(" where IsManualDial=1  OR (queryid = {0} AND (Redialable = 0 OR NeverCall = 1 OR DATEDIFF(dd,CL.calldate,getdate()) < RecycleInDays)))  ", queryId);
            DebugLogger.Write(string.Format("Legacy query preparation attmpted on query {0}.", queryId));
            return (query + sb.ToString());
        }


        /// <summary>
        /// clear query stats
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="objCampaignQueryStatus"></param>
        /// <returns></returns>
        public static void ClearQueryStats(string strCampaignDB, string strQueryCondition, CampaignQueryStatus objCampaignQueryStatus)
        {
            DataSet dsCampaignResults;
            try
            {
                dsCampaignResults = SqlHelper.ExecuteDataset(strCampaignDB,
                                    CommandType.Text, strQueryCondition);
                objCampaignQueryStatus.Total = dsCampaignResults.Tables[0].Rows.Count;
                objCampaignQueryStatus.Available = dsCampaignResults.Tables[0].Rows.Count;
                ClearQueryStats(strCampaignDB, objCampaignQueryStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// clears query stats
        /// </summary>
        /// <param name="strCampaignDB"></param>
        /// <param name="objCampaignQueryStatus"></param>
        public static void ClearQueryStats(string strCampaignDB, CampaignQueryStatus objCampaignQueryStatus)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@CampaignQueryID",objCampaignQueryStatus.CampaignQueryID), 
                            new SqlParameter("@Total",objCampaignQueryStatus.Total),
                            new SqlParameter("@Available",objCampaignQueryStatus.Available)};


                    SqlHelper.ExecuteScalar(strCampaignDB,
                         CommandType.StoredProcedure, "ClearCampaignQueryStats", sparam_s);
                    break;

                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in ClearQueryStats failed, re-running SQL transaction.");
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

        #endregion

        #region Script

        /// <summary>
        /// Returns script list
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public static DataSet GetScriptList(Campaign campaign)
        {
            DataSet ds;

            try
            {
                ds = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString, CommandType.StoredProcedure,
                    "Sel_Script_List");
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return ds;
        }

        /// <summary>
        /// Returns page list
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public static DataSet GetPageListByScriptId(Campaign campaign, long parentScriptId)
        {
            DataSet ds;

            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                    new SqlParameter("@ParentScriptID", parentScriptId)};
                ds = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString, CommandType.StoredProcedure,
                    "Sel_Script_List_By_ParentScriptID", sparam_s);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return ds;
        }


        /// <summary>
        /// script insert/update
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public static Script ScriptInsertUpdate(Campaign campaign, Script script)
        {

            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                    new SqlParameter("@ScriptID",script.ScriptID),
                    new SqlParameter("@ScriptName",script.ScriptName),
                    new SqlParameter("@ScriptHeader",script.ScriptHeader),
                    new SqlParameter("@ScriptSubHeader",script.ScriptSubHeader),
                    new SqlParameter("@ScriptBody",script.ScriptBody),
                    new SqlParameter("@ParentScriptID", script.ParentScriptID>0? script.ParentScriptID : Convert.DBNull),
                    new SqlParameter("@ScriptGuid", script.ScriptID > 0 ? script.ScriptGuid : System.Guid.NewGuid().ToString()),
                    new SqlParameter("@DateCreated",DateTime.Now.Date),  
                    new SqlParameter("@DateModified",DateTime.Now.Date)};

                script.ScriptID = (long)SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "InsUpd_Script", sparam_s);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().IndexOf("cannot insert duplicate") >= 0)
                {
                    script.ScriptName = "###ERROR###Script name already exists";
                    return script;
                }
                throw ex;
            }
            return script;
        }

        /// <summary>
        /// script insert/update - in transaction
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="script"></param>
        /// <returns></returns>
        public static Script ScriptInsertUpdate(SqlTransaction transaction, Script script)
        {

            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                    new SqlParameter("@ScriptID",script.ScriptID),
                    new SqlParameter("@ScriptName",script.ScriptName),
                    new SqlParameter("@ScriptHeader",script.ScriptHeader),
                    new SqlParameter("@ScriptSubHeader",script.ScriptSubHeader),
                    new SqlParameter("@ScriptBody",script.ScriptBody),
                    new SqlParameter("@ParentScriptID", script.ParentScriptID>0? script.ParentScriptID : Convert.DBNull),
                    new SqlParameter("@ScriptGuid", script.ScriptGuid),
                    new SqlParameter("@DateCreated",DateTime.Now.Date),  
                    new SqlParameter("@DateModified",DateTime.Now.Date)};

                script.ScriptID = (long)SqlHelper.ExecuteScalar(transaction,
                    CommandType.StoredProcedure, "InsUpd_Script", sparam_s);
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().IndexOf("cannot insert duplicate") >= 0)
                {
                    script.ScriptName = "###ERROR###Script name already exists";
                    return script;
                }
                throw ex;
            }
            return script;
        }

        /// <summary>
        /// Returns script by scriptid
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="scriptID"></param>
        /// <returns></returns>
        public static Script GetScriptByScriptID(Campaign campaign, long scriptID)
        {
            Script script = new Script();
            DataSet ds;

            try
            {
                SqlParameter[] sparams = new SqlParameter[]{
                    new SqlParameter("@ScriptID", scriptID)};

                ds = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString, CommandType.StoredProcedure,
                    "Sel_Script_Dtl", sparams);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow r;
                    r = ds.Tables[0].Rows[0];
                    script.ScriptID = (long)r["ScriptID"];
                    script.ScriptName = r["ScriptName"].ToString();
                    script.ScriptHeader = r["ScriptHeader"].ToString();
                    script.ScriptSubHeader = r["ScriptSubHeader"].ToString();
                    script.ScriptBody = r["ScriptBody"].ToString();
                    script.ScriptGuid = r["ScriptGuid"].ToString();
                    script.DateCreated = (DateTime)r["DateCreated"];
                    script.DateModified = (DateTime)r["DateModified"];
                    script.ParentScriptID = r["ParentScriptID"] != DBNull.Value ? Convert.ToInt64(r["ParentScriptID"]) : 0;
                    script.ParentScriptName = r["ParentScriptName"] != DBNull.Value ? r["ParentScriptName"].ToString() : "";
                }


                ds.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return script;
        }

        /// <summary>
        /// Returns script by scriptguid
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="scriptID"></param>
        /// <returns></returns>
        public static Script GetScriptByScriptGUID(Campaign campaign, string scriptGUID)
        {
            Script script = new Script();
            DataSet ds;

            try
            {
                SqlParameter[] sparams = new SqlParameter[]{
                    new SqlParameter("@ScriptGuid", scriptGUID)};

                ds = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString, CommandType.StoredProcedure,
                    "Sel_script_Dtl_ByGUID", sparams);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow r;
                    r = ds.Tables[0].Rows[0];
                    script.ScriptID = (long)r["ScriptID"];
                    script.ScriptName = r["ScriptName"].ToString();
                    script.ScriptHeader = r["ScriptHeader"].ToString();
                    script.ScriptSubHeader = r["ScriptSubHeader"].ToString();
                    script.ScriptBody = r["ScriptBody"].ToString();
                    script.ScriptGuid = r["ScriptGuid"].ToString();
                    script.DateCreated = (DateTime)r["DateCreated"];
                    script.DateModified = (DateTime)r["DateModified"];
                    script.ParentScriptID = r["ParentScriptID"] != DBNull.Value ? Convert.ToInt64(r["ParentScriptID"]) : 0;
                }


                ds.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return script;
        }

        public static string CloneScript(string campDbString, long scriptId, string scriptName, string cloneCampaignDB)
        {
            string strError = "";
            Campaign objCampaign = new Campaign();
            objCampaign.CampaignDBConnString = campDbString;
            Script script = GetScriptByScriptID(objCampaign, scriptId);
            // Get pagelist
            DataSet ds = GetPageListByScriptId(objCampaign, scriptId);

            System.Collections.Generic.Dictionary<string, string> dGuid = new System.Collections.Generic.Dictionary<string, string>();
            dGuid.Add(script.ScriptGuid, System.Guid.NewGuid().ToString());
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow r in ds.Tables[0].Rows)
                {
                    dGuid.Add(r["ScriptGuid"].ToString(), System.Guid.NewGuid().ToString());
                }
            }

            using (SqlConnection connect = new SqlConnection(cloneCampaignDB))
            {
                connect.Open();

                using (SqlTransaction transaction = connect.BeginTransaction())
                {
                    try
                    {
                        script.ScriptID = 0;
                        script.ScriptName = scriptName;
                        objCampaign.CampaignDBConnString = cloneCampaignDB;
                        script.ScriptGuid = dGuid[script.ScriptGuid];
                        ReplaceGuids(script, dGuid);
                        script = ScriptInsertUpdate(transaction, script);
                        if (script.ScriptName.IndexOf("###ERROR###") >= 0)
                        {
                            strError = string.Format("Script Name({0}) already exists", scriptName);
                        }
                        else
                        {

                            long parentScriptId = script.ScriptID;
                            objCampaign.CampaignDBConnString = campDbString;


                            if (ds != null && ds.Tables[0].Rows.Count > 0)
                            {
                                foreach (DataRow r in ds.Tables[0].Rows)
                                {
                                    Script scpt1 = new Script();
                                    scpt1.ScriptID = 0;
                                    scpt1.ScriptName = r["ScriptName"].ToString();
                                    scpt1.ScriptHeader = r["ScriptHeader"].ToString();
                                    scpt1.ScriptSubHeader = r["ScriptSubHeader"].ToString();
                                    scpt1.ScriptBody = r["ScriptBody"].ToString();
                                    scpt1.ScriptGuid = dGuid[r["ScriptGuid"].ToString()];

                                    ReplaceGuids(scpt1, dGuid);

                                    scpt1.ParentScriptID = script.ScriptID;
                                    scpt1 = ScriptInsertUpdate(transaction, scpt1);
                                    if (scpt1.ScriptName.IndexOf("###ERROR###") >= 0)
                                    {
                                        strError = string.Format("Child Script Name({0}) already exists", r["ScriptName"].ToString());
                                        break;
                                    }
                                }
                            }
                        }


                        if (strError == "")
                            transaction.Commit();
                        else
                            transaction.Rollback();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
            return strError;
        }

        private static void ReplaceGuids(Script script, System.Collections.Generic.Dictionary<string, string> dGuid)
        {
            if (dGuid.Count > 0)
            {
                foreach (KeyValuePair<string, string> pair in dGuid)
                {
                    script.ScriptHeader = script.ScriptHeader.Replace(pair.Key, pair.Value);
                    script.ScriptSubHeader = script.ScriptSubHeader.Replace(pair.Key, pair.Value);
                    script.ScriptBody = script.ScriptBody.Replace(pair.Key, pair.Value);
                }
            }
        }

        /// <summary>
        /// deletes script
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="scriptID"></param>
        public static string DeleteScript(Campaign campaign, long scriptID)
        {
            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                    new SqlParameter("@ScriptID", scriptID)};

                object obj = SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "Del_Script", sparam_s);

                if (obj != null)
                {
                    return obj.ToString();
                }
                return "";
                //SqlHelper.ExecuteScalar(campaign.CampaignDBConnString, CommandType.Text,
                //    "Delete From Script Where ScriptID = " + scriptID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
        }

        #endregion

        #region DB Creation

        /// <summary>
        /// Creates Database
        /// </summary>
        /// <param name="dbConnString"></param>
        /// <param name="nodeName"></param>
        private static void CreateDatabase(string dbConnString, string dbName)
        {
            try
            {
                StringBuilder strDB = new StringBuilder();
                strDB.AppendFormat("IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'{0}'){1}", dbName.Replace("'", "''"), "\r\n");
                strDB.AppendFormat("DROP DATABASE [{0}]{1}", dbName, "\r\n");
                int result = SqlHelper.ExecuteNonQuery(dbConnString, CommandType.Text, strDB.ToString());
                strDB = new StringBuilder();
                strDB.AppendFormat("CREATE DATABASE [{0}]", dbName);
                result = SqlHelper.ExecuteNonQuery(dbConnString, CommandType.Text, strDB.ToString());
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        /// <summary>
        /// Creates tables
        /// </summary>
        /// <param name="dbConnString"></param>
        /// <param name="nodeTableStructFilePath"></param>
        /// <param name="campaignDBName"></param>
        private static void CreateTables(string dbConnString, string campaignDBName, string otherSqlCommands, bool includeDML,bool is7digits)
        {
            //Connect to Node DB not MASTER
            dbConnString = dbConnString.Replace("MASTER", campaignDBName);

            StringBuilder sbScript = new StringBuilder();
            try
            {
                sbScript.Append(
                    ReadDataFromFile(
                        ConfigurationManager.AppSettings["CampaignScriptFilePathDDL"]));

                /*if (is7digits)
                    sbScript.Append(
                        ReadDataFromFile(
                            ConfigurationManager.AppSettings["CampaignScriptFilePath7Digits"]));*/

                if (includeDML)
                    sbScript.Append(
                        ReadDataFromFile(
                            ConfigurationManager.AppSettings["CampaignScriptFilePathDML"]));
                    
            }
            catch (Exception exp)
            {
                throw exp;
            }

            //Create Node DB 
            using (SqlConnection conn = new SqlConnection(dbConnString))
            {
                conn.Open();
                try
                {
                    Microsoft.SqlServer.Management.Smo.Server server =
                        new Microsoft.SqlServer.Management.Smo.Server(new Microsoft.SqlServer.Management.Common.ServerConnection(conn));

                    //    int result = server.ConnectionContext.ExecuteNonQuery(sbScript.ToString());
                    //int result1 = server.ConnectionContext.ExecuteNonQuery("UPDATE dbo.CampaignFields SET [dbo].[CampaignFields].[Value] = 7 WHERE [dbo].[CampaignFields].[FieldID] = 8");
                }
                catch (Exception exp)
                {
                    DebugLogger.Write("Exception running clone script");
                    throw exp;
                }

                try
                {
                    if (otherSqlCommands != "")
                    {
                        int result = SqlHelper.ExecuteNonQuery(dbConnString, CommandType.Text, otherSqlCommands);
                    }
                }
                catch { }
            }
        }

        /// <summary>
        /// drop database
        /// </summary>
        /// <param name="masterDBConn"></param>
        /// <param name="campaignDBName"></param>
        private static void DropCampaignDataBase(string masterDBConn, string campaignDBName)
        {
            //Code to drop databse explicity closing all connections.
            StringBuilder strDB = new StringBuilder();
            strDB.AppendFormat("ALTER DATABASE {0} {1}", campaignDBName, System.Environment.NewLine);
            strDB.AppendFormat("SET SINGLE_USER {0}", System.Environment.NewLine);
            strDB.AppendFormat("WITH ROLLBACK IMMEDIATE {0}", System.Environment.NewLine);
            strDB.AppendFormat("DROP DATABASE {0}", campaignDBName);
            int result = SqlHelper.ExecuteNonQuery(masterDBConn, CommandType.Text, strDB.ToString());
        }

        /// <summary>
        /// Reads data from file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static string ReadDataFromFile(string filePath)
        {
            System.IO.FileInfo fiFile = new System.IO.FileInfo(filePath);
            if (!fiFile.Exists)
            {
                DebugLogger.Write(string.Format("Could not find sql creation script file in folder '{0}'.", filePath));
                return string.Empty;
            }
            System.IO.StreamReader srText = fiFile.OpenText();
            string strData = srText.ReadToEnd();
            srText.Close();
            fiFile = null;
            srText = null;

            return strData;
        }

        #endregion

        #region QueryDetail

        /// <summary>
        /// Query insert/update
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="xQueryDetails"></param>
        /// <param name="objQuery"></param>
        /// <returns></returns>
        public static Query QueryInsertUpdate(Campaign campaign,
           List<XmlNode> xQueryDetails, Query objQuery)
        {
            using (SqlConnection connect = new SqlConnection(campaign.CampaignDBConnString))
            {
                connect.Open();

                using (SqlTransaction transaction = connect.BeginTransaction())
                {
                    try
                    {
                        //StringBuilder sbQuery = new StringBuilder();
                        //sbQuery.Append("SELECT UniqueKey, Campaign, PhoneNum, DBCompany, ");
                        //sbQuery.Append("NeverCallFlag, AgentID, VerificationAgentID, CallResultCode,");
                        //sbQuery.Append("DateTimeofCall, CallDuration, CallSenttoDialTime, ");
                        //sbQuery.Append("CalltoAgentTime, CallHangupTime, CallCompletionTime, ");
                        //sbQuery.Append("CallWrapUpStartTime, CallWrapUpStopTime, ResultCodeSetTime, ");
                        //sbQuery.Append("TotalNumAttempts, NumAttemptsAM, NumAttemptsWkEnd, NumAttemptsPM, LeadProcessed, ");
                        //sbQuery.Append("NAME, ADDRESS, CITY, STATE, ZIP, ADDRESS2, COUNTRY, FullQueryPassCount, ");
                        //sbQuery.Append("APCR, APCRAgent,  APCRDT, APCRMemo,");
                        //sbQuery.Append("APCR2, APCRAgent2, APCRDT2, APCRMemo2, ");
                        //sbQuery.Append("APCR3, APCRAgent3,  APCRDT3, APCRMemo3,");
                        //sbQuery.Append("APCR4, APCRAgent4, APCRDT4, APCRMemo4, ");
                        //sbQuery.Append("APCR5,  APCRAgent5, APCRDT5, APCRMemo5, ");
                        //sbQuery.Append("APCR6, APCRAgent6, APCRDT6, APCRMemo6 ");
                        //sbQuery.Append("FROM CAMPAIGN WHERE CallResultCode IS NULL AND AgentID IS NULL AND ( ");
                        //sbQuery.Append(objQuery.QueryCondition);
                        //sbQuery.Append(" )");

                        //bool isEdit = objQuery.QueryID > 0;

                        SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@QueryID",objQuery.QueryID),
                            new SqlParameter("@QueryName",objQuery.QueryName),  
                            new SqlParameter("@QueryCondition",objQuery.QueryCondition),   
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)};

                        objQuery.QueryID = (long)SqlHelper.ExecuteScalar(transaction,
                            CommandType.StoredProcedure, "InsUpd_Query", sparam_s);

                        if (objQuery.QueryID != 0)
                        {
                            foreach (XmlNode xQD in xQueryDetails)
                            {
                                //objQueryDetail = QueryDetailInsertUpdate(objQuery.QueryID, objQueryDetail, campaign);
                                QueryDetail objQueryDetail = (QueryDetail)Serialize.DeserializeObject(xQD, "QueryDetail");

                                sparam_s = new SqlParameter[]{
                                new SqlParameter("@QueryDetailID",objQueryDetail.QueryDetailID),
                                new SqlParameter("@QueryID",objQuery.QueryID),  
                                new SqlParameter("@SearchCriteria",objQueryDetail.SearchCriteria),
                                new SqlParameter("@SearchOperator",objQueryDetail.SearchOperator),
                                new SqlParameter("@SearchString",objQueryDetail.SearchString),
                                new SqlParameter("@LogicalOperator",objQueryDetail.LogicalOperator),
                                new SqlParameter("@LogicalOrder",objQueryDetail.LogicalOrder),
                                new SqlParameter("@SubQueryID",objQueryDetail.SubQueryID),
                                new SqlParameter("@DateCreated",DateTime.Now.Date),  
                                new SqlParameter("@DateModified",DateTime.Now.Date),
                                new SqlParameter("@SubsetID",objQueryDetail.SubsetID),
                                new SqlParameter("@SubsetName",objQueryDetail.SubsetName),
                                new SqlParameter("@SubsetLevel",objQueryDetail.SubsetLevel),
                                new SqlParameter("@ParentSubsetID",objQueryDetail.ParentSubsetID),
                                new SqlParameter("@SubsetLogicalOrder",objQueryDetail.SubsetLogicalOrder),
                                new SqlParameter("@TreeNodeID",objQueryDetail.TreeNodeID),
                                new SqlParameter("@ParentTreeNodeID",objQueryDetail.ParentTreeNodeID),
                                new SqlParameter("@ElementText",objQueryDetail.ElementText)};

                                objQueryDetail.QueryDetailID = (long)SqlHelper.ExecuteScalar(transaction,
                                    CommandType.StoredProcedure, "InsUpd_QueryDetail", sparam_s);
                            }

                            DataSet dsCampaignResults = SqlHelper.ExecuteDataset(transaction,
                                    CommandType.Text, objQuery.QueryCondition);
                            CampaignQueryStatusInsertUpdate(objQuery, transaction, dsCampaignResults.Tables[0].Rows.Count, -1, true);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        if (ex.Message.IndexOf("IX_Query") >= 0)
                            throw new Exception("DuplicateQueryException");
                        else
                            throw ex;
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
            return objQuery;
        }

        /// <summary>
        /// Query detail insert/update
        /// </summary>
        /// <param name="QueryID"></param>
        /// <param name="objQueryDetail"></param>
        /// <param name="objcampaign"></param>
        /// <returns></returns>
        public static QueryDetail QueryDetailInsertUpdate(long QueryID,
           QueryDetail objQueryDetail, Campaign objcampaign)
        {

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {

                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@QueryDetailID",objQueryDetail.QueryDetailID),
                            new SqlParameter("@QueryID",QueryID),  
                            new SqlParameter("@SearchCriteria",objQueryDetail.SearchCriteria),
                            new SqlParameter("@SearchOperator",objQueryDetail.SearchOperator),
                            new SqlParameter("@SearchString",objQueryDetail.SearchString),
                            new SqlParameter("@LogicalOperator",objQueryDetail.LogicalOperator),
                            new SqlParameter("@LogicalOrder",objQueryDetail.LogicalOrder),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date),
                            new SqlParameter("@SubsetID",objQueryDetail.SubsetID),
                            new SqlParameter("@SubsetName",objQueryDetail.SubsetName),
                            new SqlParameter("@SubsetLevel",objQueryDetail.SubsetLevel),
                            new SqlParameter("@ParentSubsetID",objQueryDetail.ParentSubsetID),
                            new SqlParameter("@SubsetLogicalOrder",objQueryDetail.SubsetLogicalOrder),
                            new SqlParameter("@TreeNodeID",objQueryDetail.TreeNodeID),
                            new SqlParameter("@ParentTreeNodeID",objQueryDetail.ParentTreeNodeID),
                            new SqlParameter("@ElementText",objQueryDetail.ElementText)};

                    objQueryDetail.QueryDetailID = (long)SqlHelper.ExecuteScalar(objcampaign.CampaignDBConnString,
                        CommandType.StoredProcedure, "InsUpd_QueryDetail", sparam_s);

                    if (objQueryDetail.QueryDetailID != 0)
                    {
                        objQueryDetail.QueryID = QueryID;
                    }
                    break;

                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in QueryDetailInsertUpdate failed, re-running SQL transaction.");
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


            return objQueryDetail;
        }

        /// <summary>
        /// Returns query list
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetQueryList(string strCampaignMasterDBConn)
        {
            DataSet dsQueryList = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsQueryList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn, CommandType.StoredProcedure,
                        "Sel_Query_List");
                    break;
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetQueryList failed, re-running SQL transaction.");
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
            return dsQueryList;
        }

        /// <summary>
        /// Returns query list
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetActiveQueryList(string strCampaignMasterDBConn)
        {
            DataSet dsActiveQueryList = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsActiveQueryList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn, CommandType.StoredProcedure,
                        "Sel_ActiveQuery_List");
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetActiveQueryList failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            ExceptionManager.Publish(ex);
                            throw ex;
                        }
                    }
                    else
                    {
                        ExceptionManager.Publish(ex);
                        throw ex;
                    }
                }
            }
            return dsActiveQueryList;
        }

        public static DataSet GetStandbyQueryList(string strCampaignMasterDBConn)
        {
            DataSet dsStandbyQueryList = null;

            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsStandbyQueryList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn, CommandType.StoredProcedure,
                        "Sel_StandbyQuery_List");
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetStandbyQueryList failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            ExceptionManager.Publish(ex);
                            throw ex;
                        }
                    }
                    else
                    {
                        ExceptionManager.Publish(ex);
                        throw ex;
                    }
                }
            }
            return dsStandbyQueryList;
        }

        /// <summary>
        /// Get Campaign data on Query Condition
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetCampaignData(string strCampaignDBConn, string strQueryCondition)
        {
            DataSet dsCampaignData = null;
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsCampaignData = SqlHelper.ExecuteDataset(strCampaignDBConn, CommandType.Text,
                        strQueryCondition);
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetCampaignData failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            ExceptionManager.Publish(ex);
                            throw ex;
                        }
                    }
                    else
                    {
                        ExceptionManager.Publish(ex);
                        throw ex;
                    }
                }
            }
            return dsCampaignData;
        }


        /// <summary>
        /// Get Campaign data on Query Condition
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetCampaignData_Recycle_Last(string strCampaignDBConn, string strQueryCondition, long queryId)
        {
            DataSet dsCampaignData;
            SqlParameter[] sparams = new SqlParameter[]{
                    new SqlParameter("@queryid", queryId),
                    new SqlParameter("@QueryCondition", strQueryCondition)};
            try
            {
                dsCampaignData = SqlHelper.ExecuteDataset(strCampaignDBConn, CommandType.StoredProcedure,
                    "GET_QUERYCAMPAIGNLIST", sparams);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return dsCampaignData;
        }

        public static string PrepareDialerQuery(string strCampaignDBConn, string strQueryCondition, long queryId)
        {
            string newQueryCondition = "";
            DataSet dsQueryCond;
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                SqlParameter[] sparams = new SqlParameter[]{
                    new SqlParameter("@QueryCondition", strQueryCondition),
                    new SqlParameter("@QueryID", queryId)
                    };
                try
                {
                    dsQueryCond = SqlHelper.ExecuteDataset(strCampaignDBConn, CommandType.StoredProcedure,
                        "PrepareDialerQuery", sparams);
                    DataRow row = dsQueryCond.Tables[0].Rows[0];
                    newQueryCondition = row["NewQueryCondition"].ToString();
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in PrepareDialerQuery failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            ExceptionManager.Publish(ex);
                            throw ex;
                        }
                    }
                    else
                    {
                        ExceptionManager.Publish(ex);
                        throw ex;
                    }
                }
            }
            return newQueryCondition;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCampaignDBConn"></param>
        /// <param name="uniqueKey"></param>
        /// <returns></returns>
        public static bool IsNeverCallSet(string strCampaignDBConn, long uniqueKey)
        {
            string strQueryCondition = "Select NeverCallFlag from Campaign where uniqueKey = " + uniqueKey;
            try
            {
                object obj = SqlHelper.ExecuteScalar(strCampaignDBConn, CommandType.Text,
                    strQueryCondition);
                try
                {
                    return Convert.ToBoolean(obj);
                }
                catch { return false; }
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
            }
            return false;
        }

        /// <summary>
        /// Returns querydetail by id
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <param name="strQueryID"></param>
        /// <returns></returns>
        public static DataSet GetQueryDetailsByQueryID(string strCampaignMasterDBConn, string strQueryID)
        {
            DataSet dsQueryDetails;
            SqlParameter[] sparams = new SqlParameter[]{
                    new SqlParameter("@QueryID", strQueryID)};
            try
            {
                dsQueryDetails = SqlHelper.ExecuteDataset(strCampaignMasterDBConn, CommandType.StoredProcedure,
                    "Sel_QueryDetail_ByQueryID", sparams);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return dsQueryDetails;
        }

        /// <summary>
        /// deletes query
        /// </summary>
        /// <param name="strCampaignDBConn"></param>
        /// <param name="queryID"></param>
        /// <returns></returns>
        public static int DeleteQuery(string strCampaignDBConn, long queryID)
        {
            int result = 0;
            SqlParameter param = new SqlParameter("@QueryID", queryID);
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCampaignDBConn,
                     CommandType.StoredProcedure, "Dbo.Del_Query", param);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return result;
        }

        public static int DeleteQuery(string strCampaignDBConn, string queryName)
        {
            int result = 0;
            SqlParameter param = new SqlParameter("@QueryName", queryName);
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCampaignDBConn,
                     CommandType.StoredProcedure, "Dbo.Del_QueryByName", param);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return result;
        }

        /// <summary>
        /// deletes querydetail
        /// </summary>
        /// <param name="strCampaignDBConn"></param>
        /// <param name="queryDetailID"></param>
        /// <returns></returns>
        public static int DeleteQueryDetail(string strCampaignDBConn, long queryDetailID)
        {
            int result = 0;
            SqlParameter param = new SqlParameter("@QueryDetailID", queryDetailID);
            try
            {
                result = SqlHelper.ExecuteNonQuery(strCampaignDBConn,
                     CommandType.StoredProcedure, "Dbo.Del_QueryDetail", param);
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return result;
        }

        #endregion

        #region Area Code

        /// <summary>
        /// Areacoderule insert/update
        /// </summary>
        /// <param name="strRainmakerMasterDBConn"></param>
        /// <param name="objAreaCodeRule"></param>
        /// <returns></returns>
        public static AreaCodeRule AreaCodeRuleInsertUpdate(string strRainmakerMasterDBConn,
           AreaCodeRule objAreaCodeRule)
        {
            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AreaCodeRuleID",objAreaCodeRule.AreaCodeRuleID),
                            new SqlParameter("@AgentID",objAreaCodeRule.AgentID),  
                            new SqlParameter("@AreaCodeID",objAreaCodeRule.AreaCodeID <= 0 ? 
                                                Convert.DBNull : objAreaCodeRule.AreaCodeID),
                            new SqlParameter("@LikeDialing",objAreaCodeRule.LikeDialing),
                            new SqlParameter("@LikeDialingOption",objAreaCodeRule.LikeDialingOption),
                            new SqlParameter("@CustomeDialing",objAreaCodeRule.CustomeDialing),
                            new SqlParameter("@IsSevenDigit",objAreaCodeRule.IsSevenDigit),
                            new SqlParameter("@IsTenDigit",objAreaCodeRule.IsTenDigit),
                            new SqlParameter("@IntraLataDialing",DBNull.Value),
                            new SqlParameter("@IntraLataDialingAreaCode",objAreaCodeRule.IntraLataDialingAreaCode),
                            new SqlParameter("@ILDIsTenDigit",objAreaCodeRule.ILDIsTenDigit),
                            new SqlParameter("@ILDElevenDigit",objAreaCodeRule.ILDElevenDigit),
                            new SqlParameter("@ReplaceAreaCode",objAreaCodeRule.ReplaceAreaCode),
                            new SqlParameter("@LongDistanceDialing",objAreaCodeRule.LongDistanceDialing),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)
                };


                objAreaCodeRule.AreaCodeRuleID = (long)SqlHelper.ExecuteScalar(strRainmakerMasterDBConn,
                    CommandType.StoredProcedure, "InsUpd_AreaCodeRule", sparam_s);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objAreaCodeRule;
        }

        /// <summary>
        /// areacode insert/update
        /// </summary>
        /// <param name="strRainmakerMasterDBConn"></param>
        /// <param name="objAreaCode"></param>
        /// <returns></returns>
        public static AreaCode AreaCodeInsertUpdate(string strRainmakerMasterDBConn,
           AreaCode objAreaCode)
        {
            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AreaCodeID",objAreaCode.AreaCodeID),
                            new SqlParameter("@AreaCode",objAreaCode.AreaCodeName),  
                            new SqlParameter("@Prefix",objAreaCode.Prefix),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)
                };


                objAreaCode.AreaCodeID = (long)SqlHelper.ExecuteScalar(strRainmakerMasterDBConn,
                    CommandType.StoredProcedure, "InsUpd_AreaCode", sparam_s);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objAreaCode;
        }

        /// <summary>
        /// Returns area code
        /// </summary>
        /// <param name="strRainmakerMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetAreaCode(string strRainmakerMasterDBConn)
        {
            DataSet dsAreaCodes;

            try
            {
                dsAreaCodes = SqlHelper.ExecuteDataset(strRainmakerMasterDBConn, CommandType.StoredProcedure,
                    "Sel_AreaCode");
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return dsAreaCodes;
        }

        /// <summary>
        /// deletes area code
        /// </summary>
        /// <param name="strRainmakerMasterDBConn"></param>
        /// <param name="areaCodeID"></param>
        public static void DeleteAreaCode(string strRainmakerMasterDBConn, long areaCodeID)
        {
            SqlParameter param = new SqlParameter("@AreaCodeID", areaCodeID);
            try
            {
                SqlHelper.ExecuteScalar(strRainmakerMasterDBConn, CommandType.StoredProcedure,
                    "DEL_AreaCode", param);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
        }

        /// <summary>
        /// returns area code rule by id
        /// </summary>
        /// <param name="strRainmakerMasterDBConn"></param>
        /// <param name="agentID"></param>
        /// <returns></returns>
        public static AreaCodeRule GetAreaCodeRuleByAgentID(string strRainmakerMasterDBConn, long agentID)
        {
            AreaCodeRule areaCodeRule = new AreaCodeRule();
            DataSet dsAreaCodeRule;

            try
            {
                SqlParameter[] sparams = new SqlParameter[]{
                    new SqlParameter("@AgentID", agentID)};

                dsAreaCodeRule = SqlHelper.ExecuteDataset(strRainmakerMasterDBConn, CommandType.StoredProcedure,
                    "Sel_AreaCodeRule_ByAgentID", sparams);

                if (dsAreaCodeRule.Tables[0].Rows.Count > 0)
                {
                    DataRow r;
                    r = dsAreaCodeRule.Tables[0].Rows[0];
                    areaCodeRule.AreaCodeRuleID = (long)r["AreaCodeRuleID"];
                    areaCodeRule.AgentID = (long)r["AgentID"];
                    if (!r.IsNull("AreaCodeID"))
                        areaCodeRule.AreaCodeID = (long)r["AreaCodeID"];
                    areaCodeRule.LikeDialing = (bool)r["LikeDialing"];
                    areaCodeRule.LikeDialingOption = (bool)r["LikeDialingOption"];
                    areaCodeRule.CustomeDialing = (bool)r["CustomeDialing"];
                    areaCodeRule.IsSevenDigit = (bool)r["IsSevenDigit"];
                    areaCodeRule.IsTenDigit = (bool)r["IsTenDigit"];
                    areaCodeRule.IntraLataDialingAreaCode = r["IntraLataDialingAreaCode"].ToString();
                    areaCodeRule.ILDIsTenDigit = (bool)r["ILDIsTenDigit"];
                    areaCodeRule.ILDElevenDigit = (bool)r["ILDElevenDigit"];
                    areaCodeRule.ReplaceAreaCode = r["ReplaceAreaCode"].ToString();
                    areaCodeRule.LongDistanceDialing = (bool)r["LongDistanceDialing"];
                    areaCodeRule.DateCreated = (DateTime)r["DateCreated"];
                    areaCodeRule.DateModified = (DateTime)r["DateModified"];
                }
                dsAreaCodeRule.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return areaCodeRule;
        }

        #endregion

        #region Global Dialing

        /// <summary>
        /// Global Dialing insert/update
        /// </summary>
        /// <param name="strRainmakerMasterDBConn"></param>
        /// <param name="objGlobalDialing"></param>
        /// <returns></returns>
        public static GlobalDialingParams GlobalDialingInsertUpdate(string strRainmakerMasterDBConn,
           GlobalDialingParams objGlobalDialing)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@GlobalDialingID",objGlobalDialing.GlobalDialingID),
                            new SqlParameter("@Prefix",objGlobalDialing.Prefix),  
                            new SqlParameter("@Suffix",objGlobalDialing.Suffix),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)
                };


                    objGlobalDialing.GlobalDialingID = (long)SqlHelper.ExecuteScalar(strRainmakerMasterDBConn,
                        CommandType.StoredProcedure, "InsUpd_GlobalDialingParameters", sparam_s);

                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GlobalDialingInsertUpdate failed, re-running SQL transaction.");
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
            return objGlobalDialing;
        }

        /// <summary>
        /// returns globaldialing params
        /// </summary>
        /// <param name="strRainmakerMasterDBConn"></param>
        /// <returns></returns>
        public static GlobalDialingParams GetGlobalDialingParams(string strRainmakerMasterDBConn)
        {
            GlobalDialingParams globalDialing = new GlobalDialingParams();
            DataSet dsGlobalDialingParams;

            try
            {
                dsGlobalDialingParams = SqlHelper.ExecuteDataset(strRainmakerMasterDBConn, CommandType.StoredProcedure,
                    "Sel_GlobalDialingParameters_List");

                if (dsGlobalDialingParams.Tables[0].Rows.Count > 0)
                {
                    DataRow r;
                    r = dsGlobalDialingParams.Tables[0].Rows[0];
                    globalDialing.GlobalDialingID = (long)r["GlobalDialingID"];
                    globalDialing.Prefix = (string)r["Prefix"];
                    globalDialing.Suffix = (string)r["Suffix"];
                    globalDialing.DateCreated = (DateTime)r["DateCreated"];
                    globalDialing.DateModified = (DateTime)r["DateModified"];
                }
                dsGlobalDialingParams.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return globalDialing;
        }

        #endregion

        #region Agent Stats

        /// <summary>
        /// Returns agetstats
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public static DataSet GetAgentStat(string campaignDBConnString, long campaignId)
        {
            DataSet dsAgentStats;
            try
            {

                SqlParameter[] sparam_s = new SqlParameter[] { new SqlParameter("@CampaignID", campaignId) };

                dsAgentStats = SqlHelper.ExecuteDataset(campaignDBConnString, CommandType.StoredProcedure,
                    "Sel_AgentStat_List", sparam_s);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return dsAgentStats;
        }

        /// <summary>
        /// Get Agent Stat Detail
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="AgentID"></param>
        /// <returns></returns>
        public static AgentStat GetAgentStatByStatID(string campaignMasterDBConn,
            long agentStatID)
        {
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@StatID",agentStatID)
            };

            DataSet dsAgentStatDtl;
            AgentStat agentStat = new AgentStat();
            try
            {
                dsAgentStatDtl = SqlHelper.ExecuteDataset(campaignMasterDBConn,
                    CommandType.StoredProcedure, "Sel_AgentStat_Dtl", sparam_s);

                if (dsAgentStatDtl.Tables[0].Rows.Count == 1)
                {
                    DataRow r;
                    r = dsAgentStatDtl.Tables[0].Rows[0];
                    agentStat.StatID = (long)r["StatID"];
                    agentStat.AgentID = Convert.ToInt64(r["AgentID"] == Convert.DBNull ?
                        Convert.ToInt64(0) : Convert.ToInt64(r["AgentID"]));
                    agentStat.StatusID = Convert.ToInt64(r["StatusID"] == Convert.DBNull ?
                        Convert.ToInt64(0) : Convert.ToInt32(r["StatusID"]));
                    agentStat.LeadsSales = Convert.ToInt32(r["LeadsSales"] == Convert.DBNull ?
                        0 : Convert.ToInt32(r["LeadsSales"]));
                    agentStat.Presentations = Convert.ToInt32(r["Presentations"] == Convert.DBNull ?
                        0 : Convert.ToInt32(r["Presentations"]));
                    agentStat.Calls = Convert.ToInt32(r["Calls"] == Convert.DBNull ?
                        0 : Convert.ToInt32(r["Calls"]));
                    agentStat.LeadSalesRatio = Convert.ToDecimal(r["LeadSalesRatio"] == Convert.DBNull ?
                        Convert.ToDecimal(0) : Convert.ToDecimal(r["LeadSalesRatio"]));
                    agentStat.PledgeAmount = Convert.ToDecimal(r["PledgeAmount"] == Convert.DBNull ?
                        Convert.ToDecimal(0) : Convert.ToDecimal(r["PledgeAmount"]));
                    agentStat.TalkTime = Convert.ToDecimal(r["TalkTime"] == Convert.DBNull ?
                        Convert.ToDecimal(0) : Convert.ToDecimal(r["TalkTime"]));
                    agentStat.WaitingTime = Convert.ToDecimal(r["WaitingTime"] == Convert.DBNull ?
                        Convert.ToDecimal(0) : Convert.ToDecimal(r["WaitingTime"]));
                    agentStat.PauseTime = Convert.ToDecimal(r["PauseTime"] == Convert.DBNull ?
                        Convert.ToDecimal(0) : Convert.ToDecimal(r["PauseTime"]));
                    agentStat.WrapTime = Convert.ToDecimal(r["WrapTime"] == Convert.DBNull ?
                        Convert.ToDecimal(0) : Convert.ToDecimal(r["WrapTime"]));
                    agentStat.LoginDate = Convert.ToDateTime(r["LoginDate"] == Convert.DBNull ?
                        DateTime.MinValue : Convert.ToDateTime(r["LoginDate"]));
                    agentStat.LogOffDate = Convert.ToDateTime(r["LogOffDate"] == Convert.DBNull ?
                        DateTime.MinValue : Convert.ToDateTime(r["LogOffDate"]));
                    agentStat.LoginTime = Convert.ToDateTime(r["LoginTime"] == Convert.DBNull ?
                        DateTime.MinValue : Convert.ToDateTime(r["LoginTime"]));
                    agentStat.LogOffTime = Convert.ToDateTime(r["LogOffTime"] == Convert.DBNull ?
                        DateTime.MinValue : Convert.ToDateTime(r["LogOffTime"]));
                    agentStat.LastResultCodeID = Convert.ToInt64(r["LastResultCodeID"] == Convert.DBNull ?
                        Convert.ToInt64(0) : Convert.ToInt64(r["LastResultCodeID"]));
                    agentStat.DateCreated = Convert.ToDateTime(r["DateCreated"] == Convert.DBNull ?
                        DateTime.MinValue : Convert.ToDateTime(r["DateCreated"]));
                    agentStat.DateModified = Convert.ToDateTime(r["DateModified"] == Convert.DBNull ?
                        DateTime.MinValue : Convert.ToDateTime(r["DateModified"]));
                    agentStat.TimeModified = Convert.ToDateTime(r["TimeModified"] == Convert.DBNull ?
                        DateTime.MinValue : Convert.ToDateTime(r["TimeModified"]));
                }

                dsAgentStatDtl.Clear();
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return agentStat;
        }

        /// <summary>
        /// AgentStat Insert/Update
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="AgentStat"></param>
        /// <returns></returns>
        public static AgentStat InsertUpdateAgentStat(Campaign objCampaign,
            AgentStat objAgentStat)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@StatID", objAgentStat.StatID),
                            new SqlParameter("@AgentID",objAgentStat.AgentID),
                            new SqlParameter("@StatusID", objAgentStat.StatusID),
                            new SqlParameter("@LeadsSales", objAgentStat.LeadsSales==0?Convert.DBNull:objAgentStat.LeadsSales),
                            new SqlParameter("@Presentations", objAgentStat.Presentations==0?Convert.DBNull:objAgentStat.Presentations),
                            new SqlParameter("@Calls", objAgentStat.Calls==0?Convert.DBNull:objAgentStat.Calls),
                            new SqlParameter("@LeadSalesRatio", objAgentStat.LeadSalesRatio==0?Convert.DBNull:objAgentStat.LeadSalesRatio),
                            new SqlParameter("@PledgeAmount", objAgentStat.PledgeAmount==0?Convert.DBNull:objAgentStat.PledgeAmount),
                            new SqlParameter("@TalkTime", objAgentStat.TalkTime==0?Convert.DBNull:objAgentStat.TalkTime),
                            new SqlParameter("@WaitingTime", objAgentStat.WaitingTime==0?Convert.DBNull:objAgentStat.WaitingTime),
                            new SqlParameter("@PauseTime", objAgentStat.PauseTime==0?Convert.DBNull:objAgentStat.PauseTime),
                            new SqlParameter("@WrapTime", objAgentStat.WrapTime==0?Convert.DBNull:objAgentStat.WrapTime),
                            new SqlParameter("@LoginDate", objAgentStat.LoginDate==DateTime.MinValue?Convert.DBNull:objAgentStat.LoginDate),
                            new SqlParameter("@LogOffDate", objAgentStat.LogOffDate==DateTime.MinValue?Convert.DBNull:objAgentStat.LogOffDate),
                            new SqlParameter("@LoginTime", objAgentStat.LoginTime==DateTime.MinValue?Convert.DBNull:objAgentStat.LoginTime),
                            new SqlParameter("@LogOffTime", objAgentStat.LogOffTime==DateTime.MinValue?Convert.DBNull:objAgentStat.LogOffTime),
                            new SqlParameter("@LastResultCodeID", objAgentStat.LastResultCodeID==0?Convert.DBNull:objAgentStat.LastResultCodeID),
                            new SqlParameter("@DateCreated", DateTime.Now),
                            new SqlParameter("@DateModified", DateTime.Now)
                };

                    objAgentStat.StatID = (long)SqlHelper.ExecuteScalar(objCampaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "InsUpd_AgentStat", sparam_s);
                    break;
                    //if(objAgentStat.LeadProcessed!="")
                    //UpdateLeadSales(objCampaign.CampaignDBConnString,objAgentStat);
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in InsertUpdateAgentStat failed, re-running SQL transaction.");
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
            return objAgentStat;

        }

        public static void UpdateAgentStatus(string CampaignDBConnString, long agentID, long statusID)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AgentID", agentID),
                            new SqlParameter("@StatusID", statusID)      
                           };

                    int result = SqlHelper.ExecuteNonQuery(CampaignDBConnString,
                    CommandType.StoredProcedure, "Upd_AgentStatus", sparam_s);
                    DebugLogger.Write(string.Format("Agent {0} DB Update complete to status {1}.", agentID, statusID));
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in UpdateAgentStatus failed, re-running SQL transaction.");
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


        public static void UpdateAgentStats(string CampaignDBConnString, long agentID, string strLeadprocessed)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@AgentID", agentID),
                            new SqlParameter("@LeadProcessed",strLeadprocessed)      
                           };

                    int result = SqlHelper.ExecuteNonQuery(CampaignDBConnString,
                    CommandType.StoredProcedure, "Upd_AgentStat", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in UpdateAgentStats failed, re-running SQL transaction.");
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

        #endregion

        #region Campain FieldManager

        /// <summary>
        /// Gets data field types
        /// </summary>
        /// <param name="strCampaignMasterDBConn"></param>
        /// <returns></returns>
        public static DataSet GetFieldTypes(string strCampaignMasterDBConn)
        {
            DataSet dsFieldTypes;
            try
            {
                dsFieldTypes = SqlHelper.ExecuteDataset(strCampaignMasterDBConn, CommandType.StoredProcedure,
                    "Sel_FieldTypes_List");
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return dsFieldTypes;
        }

        public static long GetCampaignStatus(string strCampaignMasterDBConn, long lngCampaignID)
        {
            long lngStatusID = 0;
            using (SqlConnection connect = new SqlConnection(strCampaignMasterDBConn))
            {
                connect.Open();

                using (SqlTransaction transaction = connect.BeginTransaction())
                {
                    for (int i = 1; i <= m_MaxTransactionRetries; i++)
                    {
                        try
                        {
                            SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@CampaignID",lngCampaignID)};
                            lngStatusID = (long)SqlHelper.ExecuteScalar(transaction,
                                CommandType.StoredProcedure, "GetCampaignStatus", sparam_s);
                            break;
                        }
                        catch (Exception ex)
                        {
                            ExceptionManager.Publish(ex);
                            if (ex.Message.IndexOf("Rerun the transaction") > 0)
                            {
                                if (i < m_MaxTransactionRetries)
                                {
                                    DebugLogger.Write("Transaction in GetCampaignStatus failed, re-running SQL transaction.");
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
            return lngStatusID;
        }

        /// <summary>
        /// Inserts campaign fields
        /// </summary>
        /// <param name="objCampaign"></param>
        /// <param name="objCampaignFields"></param>
        /// <returns></returns>
        public static CampaignFields CampaignFieldsInsertUpdate(Campaign objCampaign,
            CampaignFields objCampaignFields)
        {
            using (SqlConnection connect = new SqlConnection(objCampaign.CampaignDBConnString))
            {
                connect.Open();

                using (SqlTransaction transaction = connect.BeginTransaction())
                {
                    try
                    {
                        long fieldID = objCampaignFields.FieldID;
                        SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@FieldID",objCampaignFields.FieldID),
                            new SqlParameter("@FieldName",objCampaignFields.FieldName),  
                            new SqlParameter("@FieldTypeID",objCampaignFields.FieldTypeID),
                            new SqlParameter("@IsDefault", objCampaignFields.IsDefault),
                            new SqlParameter("@Value",objCampaignFields.DBValue <= 0 ? 
                                    Convert.DBNull : objCampaignFields.DBValue)};

                        objCampaignFields.FieldID = (long)SqlHelper.ExecuteScalar(transaction,
                            CommandType.StoredProcedure, "InsUpd_CampaignFields", sparam_s);

                        if (fieldID != objCampaignFields.FieldID)
                        {
                            string strAlterQuery = string.Format("ALTER TABLE dbo.Campaign ADD {0} {1}{2} NULL",
                                objCampaignFields.FieldName, objCampaignFields.DbFieldType,
                                objCampaignFields.DBValue <= 0 ? string.Empty : string.Format("({0})",
                                    objCampaignFields.DBValue));
                            int result = SqlHelper.ExecuteNonQuery(transaction, CommandType.Text, strAlterQuery);
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        transaction.Rollback();
                        if (ex.Message.IndexOf("'IX_CampaignFields'") >= 0 || ex.Message.IndexOf("Column names in each table must be unique") >= 0)
                        {
                            throw new Exception("DuplicateColumnException");
                        }
                        else
                            throw ex;
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
            return objCampaignFields;
        }

        /// <summary>
        /// Gets list of newly added data fields to campaign
        /// </summary>
        /// <param name="objCampaign"></param>
        /// <returns></returns>
        public static DataSet GetCampaignFields(Campaign objCampaign)
        {
            DataSet dsCampaignFields;
            try
            {

                dsCampaignFields = SqlHelper.ExecuteDataset(objCampaign.CampaignDBConnString, CommandType.StoredProcedure,
                    "Sel_CampaignFields_List");
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return dsCampaignFields;
        }

        /// <summary>
        /// Deletes campaign field
        /// </summary>
        /// <param name="objCampaign"></param>
        /// <param name="fieldID"></param>
        /// <returns></returns>
        public static int DeleteCampaignField(Campaign objCampaign,
            long fieldID)
        {
            int result = 0;
            using (SqlConnection connect = new SqlConnection(objCampaign.CampaignDBConnString))
            {
                connect.Open();
                using (SqlTransaction transaction = connect.BeginTransaction())
                {
                    try
                    {
                        SqlParameter param = new SqlParameter("@FieldID", fieldID);

                        result = SqlHelper.ExecuteNonQuery(transaction,
                            CommandType.StoredProcedure, "Dbo.Del_CampaignColumn", param);

                        result = SqlHelper.ExecuteNonQuery(transaction,
                             CommandType.StoredProcedure, "Dbo.Del_CampaignField", param);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        //Code to delete the created database, if exception arises after creation of database.
                        transaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
            return result;
        }

        #endregion

        #region CallList
        /// <summary>
        /// ScheduledCampaign Insert/Update
        /// </summary>
        /// <param name="objCampaign"></param>
        /// <param name="objCampaigndetails"></param>
        /// <returns></returns>
        public static void InsertUpdateScheduledCampaign(Campaign objCampaign,
            CampaignDetails objCampaigndetails)
        {
            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@UniqueKey", objCampaigndetails.UniqueKey),
                            new SqlParameter("@ScheduleDate",objCampaigndetails.ScheduleDate)
                            };

                int result = SqlHelper.ExecuteNonQuery(objCampaign.CampaignDBConnString,
                 CommandType.StoredProcedure, "InsUpd_ScheduledCampaign", sparam_s);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// ScheduledCampaign Insert/Update
        /// </summary>
        /// <param name="objCampaign"></param>
        /// <param name="objCampaigndetails"></param>
        /// <returns></returns>
        public static void UpdateCampaignSchedule(Campaign objCampaign,
            CampaignDetails objCampaigndetails, bool isFromAgentInterface)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@UniqueKey", objCampaigndetails.UniqueKey),
                            new SqlParameter("@AgentID", objCampaigndetails.AgentID),
                            new SqlParameter("@ScheduleDate",objCampaigndetails.ScheduleDate),
                            new SqlParameter("@ScheduleNotes",objCampaigndetails.ScheduleNotes),
                            new SqlParameter("@IsFromAgent", isFromAgentInterface)
                            };

                    int result = SqlHelper.ExecuteNonQuery(objCampaign.CampaignDBConnString,
                     CommandType.StoredProcedure, "Upd_CampaignSchedule", sparam_s);

                    try
                    {
                        if (isFromAgentInterface)
                        {
                            AddResultCodeToCallList(objCampaign.CampaignDBConnString, objCampaigndetails.UniqueKey, "Scheduled Callback", 0, "");
                            if (objCampaigndetails.QueryId > 0)
                                try
                                {
                                    SqlParameter[] sparam_s2 = new SqlParameter[]{
                            new SqlParameter("@UniqueKey", objCampaigndetails.UniqueKey),
                            new SqlParameter("@CallResultCode", 6),
                            new SqlParameter("@AgentID", Convert.ToInt64(objCampaigndetails.AgentID)),
                            new SqlParameter("@AgentName", objCampaigndetails.AgentName),
                            new SqlParameter("@QueryID", objCampaigndetails.QueryId)
                           };

                                    SqlHelper.ExecuteNonQuery(objCampaign.CampaignDBConnString,
                                          CommandType.StoredProcedure, "Upd_CampaignResultCode", sparam_s2);
                                }
                                catch { }
                        }
                    }
                    catch { }
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in UpdateCampSchedule failed, re-running SQL transaction.");
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

        public static void ShutdownAllCampaigns(string campaignMasterDBConn)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    DateTime logoffDateTime = DateTime.Now;

                    
                    // Log off all logged in agents
                    foreach (DataRow agentRow in
                        AgentAccess.GetLoggedInAgents(campaignMasterDBConn).Tables[0].Rows)
                    {
                        /*AgentAccess.UpdateAgentLogOut
                        (
                            new Agent()
                            {
                                AgentID = long.Parse(agentRow["AgentID"].ToString())
                            },
                            campaignMasterDBConn
                        );*/
                        long AgentID = long.Parse(agentRow["AgentID"].ToString());
                        AgentAccess.ToggleAgentReset(AgentID, true, campaignMasterDBConn);
                        
                    }
                    // Shutdown all campaigns.
                    SqlHelper.ExecuteScalar(campaignMasterDBConn,
                        CommandType.StoredProcedure, "ShutdownAllCampaigns");


                    // Update status of agents in each campaign to logged off.
                    /*foreach (DataRow campaignRow in
                        CampaignAccess.GetCampaignList(campaignMasterDBConn).Tables[0].Rows)
                    {
                        string campaignDBConnectionString = campaignRow["CampaignDBConnString"].ToString();

                        foreach (DataRow agentStatRow in CampaignAccess.GetAgentStat(
                                campaignDBConnectionString,
                                long.Parse(campaignRow["CampaignID"].ToString())
                            ).Tables[0].Rows
                        )
                        {
                            CampaignAccess.InsertUpdateAgentStat
                            (
                                new Campaign()
                                {
                                    CampaignDBConnString = campaignDBConnectionString
                                },
                                new AgentStat()
                                {
                                    StatID = long.Parse(agentStatRow["StatID"].ToString()),
                                    LogOffDate = logoffDateTime,
                                    LogOffTime = logoffDateTime
                                }
                            );
                        }
                    }
                    
                    break;
                    */
                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in ShutdownAllCmapaigns failed, re-running SQL transaction.");
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
            return;
        }

        /// <summary>
        /// Gets campaignlist by campaignID
        /// </summary>
        /// <param name="campaignMasterDBConn"></param>
        /// <param name="CampaignID"></param>
        /// <returns>DataSet</returns>
        public static int GetPhoneLinesInUseCount(string campaignMasterDBConn,
            long CampaignID)
        {
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@CampaignID",CampaignID)
            };
            int count = 0;
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    count = Convert.ToInt32(SqlHelper.ExecuteScalar(campaignMasterDBConn,
                        CommandType.StoredProcedure, "Sel_PhoneLinesInUse", sparam_s));
                    break;

                }
                catch (Exception ex)
                {
                    ExceptionManager.Publish(ex);
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetLinesInUse failed, re-running SQL transaction.");
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
            return count;
        }

        /// <summary>
        /// Checks whether call is transfered
        /// </summary>
        /// <param name="campaignDBConn"></param>
        /// <param name="UniqueKey"></param>
        /// <returns></returns>
        public static bool CheckCampaignTransferCall(string campaignDBConn,
            long UniqueKey)
        {
            SqlParameter[] sparam_s = new SqlParameter[]{
                new SqlParameter("@Uniquekey", UniqueKey)
            };
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    object obj = SqlHelper.ExecuteScalar(campaignDBConn,
                        CommandType.StoredProcedure, "Get_CampaignTransferCall", sparam_s);
                    try
                    {
                        if (Convert.ToInt32(obj) == -1)
                            return true;
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
                            DebugLogger.Write("Transaction in CheckCampaignTransferCall failed, re-running SQL transaction.");
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
            return false;
        }
        #endregion

        #region AdminRequests

        public static void SubmitAdminRequest(string strRainmakerMasterDBConn, int requestType, string requestData)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@RequestType", requestType),
                            new SqlParameter("@RequestData", requestData)
                            };


                    SqlHelper.ExecuteScalar(strRainmakerMasterDBConn,
                         CommandType.StoredProcedure, "Ins_AdminRequest", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in InsertAdminReuqest failed, re-running SQL transaction.");
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
            return;
        }

        public static DataSet GetAdminRequests(string strCampaignMasterDBConn)
        {
            DataSet dsRequestList;

            try
            {
                dsRequestList = SqlHelper.ExecuteDataset(strCampaignMasterDBConn,
                    CommandType.StoredProcedure, "Sel_AdminRequests");
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw;
            }
            return dsRequestList;
        }

        public static void UpdateAdminRequestStatus(string strRainmakerMasterDBConn, long requestID, int requestStatus)
        {
            for (int i = 1; i <= m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@RequestID", requestID),
                            new SqlParameter("@RequestStatus", requestStatus)
                            };


                    SqlHelper.ExecuteScalar(strRainmakerMasterDBConn,
                         CommandType.StoredProcedure, "Upd_AdminRequestStatus", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in UpdateAdminRequestStatus failed, re-running SQL transaction.");
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
            return;
        }

        #endregion

        #region Training Methods
        public static TrainingScheme GetTrainingScheme(Campaign campaign,
           long TrainingSchemeID)
        {
            TrainingScheme objTrainingScheme = new TrainingScheme();
            DataSet dsTrainingScheme;
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@TrainingSchemeID", TrainingSchemeID)};


                    dsTrainingScheme = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "Sel_TrainingScheme_Dtl", sparam_s);
                    if (dsTrainingScheme.Tables[0].Rows.Count > 0)
                    {
                        DataRow r;
                        r = dsTrainingScheme.Tables[0].Rows[0];
                        objTrainingScheme.SchemeID = TrainingSchemeID;
                        objTrainingScheme.PageCount = (int)r["PageCount"];
                        objTrainingScheme.Name = (string)r["Name"];
                        objTrainingScheme.ScoreboardFrequency = (int)r["ScoreboardFrequency"];
                        objTrainingScheme.ScoreboardDisplayTime = (int)r["ScoreboardDisplayTime"];
                        objTrainingScheme.IsActive = (r["IsActive"] == Convert.DBNull) ? false : (bool)r["IsActive"];
                    }
                    dsTrainingScheme.Clear();
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetTrainingScheme failed, re-running SQL transaction.");
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
            return objTrainingScheme;
        }

        public static DataSet GetTrainingPages(Campaign campaign,
           long TrainingSchemeID)
        {
            DataSet dsTrainingPages = null;
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@TrainingSchemeID", TrainingSchemeID)};


                    dsTrainingPages = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "Sel_TrainingPage_List", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetTrainingPages failed, re-running SQL transaction.");
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
            return dsTrainingPages;
        }

        public static DataSet GetTrainingSchemeList(Campaign campaign)
        {
            DataSet dsTrainingPages = null;
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsTrainingPages = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "Sel_TrainingScheme_List");
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetTrainingSchemeList failed, re-running SQL transaction.");
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
            return dsTrainingPages;
        }

        public static void UpdateActiveTrainingScheme(Campaign campaign,
           long TrainingSchemeID)
        {
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@TrainingSchemeID", TrainingSchemeID)};


                    int result = SqlHelper.ExecuteNonQuery(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "Upd_ActiveTrainingScheme", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in UpdateActiveTrainingScheme failed, re-running SQL transaction.");
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

        public static void DeleteTrainingScheme(Campaign campaign,
           long TrainingSchemeID)
        {
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@TrainingSchemeID", TrainingSchemeID)};


                    int result = SqlHelper.ExecuteNonQuery(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "Del_TrainingScheme", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in DeleteTrainingScheme failed, re-running SQL transaction.");
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

        public static long AddTrainingScheme(Campaign campaign,
           string TrainingSchemeName)
        {
            long trainingSchemeID = 0;
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@TrainingSchemeName", TrainingSchemeName)};


                    trainingSchemeID = SqlHelper.ExecuteNonQuery(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "Ins_TrainingScheme", sparam_s);
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in AddTrainingScheme failed, re-running SQL transaction.");
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
            return trainingSchemeID;
        }

        public static TrainingPage GetTrainingPage(Campaign campaign,
           long TrainingPageID)
        {
            TrainingPage objTrainingPage = new TrainingPage();
            DataSet dsTrainingPage;
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@TrainingPageID", TrainingPageID)};


                    dsTrainingPage = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "Sel_TrainingPage_Dtl", sparam_s);
                    if (dsTrainingPage.Tables[0].Rows.Count > 0)
                    {
                        DataRow r;
                        r = dsTrainingPage.Tables[0].Rows[0];
                        objTrainingPage.PageID = TrainingPageID;
                        objTrainingPage.Name = (string)r["TrainingPageName"];
                        objTrainingPage.DateCreated = Convert.ToDateTime(r["DateCreated"] == Convert.DBNull ?
                        DateTime.MinValue : Convert.ToDateTime(r["DateCreated"]));
                        objTrainingPage.DateModified = Convert.ToDateTime(r["DateModified"] == Convert.DBNull ?
                        DateTime.MinValue : Convert.ToDateTime(r["DateModified"]));
                        objTrainingPage.Content = (r["TrainingPageContent"] == Convert.DBNull) ? "" : (string)r["TrainingPageContent"];
                        objTrainingPage.DisplayTime = (int)r["DisplayTime"];
                        objTrainingPage.IsActive = (r["IsActive"] == Convert.DBNull) ? false : (bool)r["IsActive"];
                    }
                    dsTrainingPage.Clear();
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetTrainingPage failed, re-running SQL transaction.");
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
            return objTrainingPage;
        }

        public static TrainingPage TrainingPageInsertUpdate(Campaign campaign, TrainingPage trainingPage)
        {
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    SqlParameter[] sparam_s = new SqlParameter[]{
                    new SqlParameter("@TrainingPageID", trainingPage.PageID),
                    new SqlParameter("@TrainingPageName", trainingPage.Name),
                    new SqlParameter("@TrainingPageContent", trainingPage.Content),
                    new SqlParameter("@TrainingSchemeID", trainingPage.TrainingSchemeID),
                    new SqlParameter("@DisplayTime", trainingPage.DisplayTime),
                    new SqlParameter("@DateCreated",DateTime.Now.Date),  
                    new SqlParameter("@DateModified",DateTime.Now.Date)};

                    trainingPage.PageID = (long)SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                        CommandType.StoredProcedure, "InsUpd_TrainingPage", sparam_s);
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in TrainingPageInsertUpdate failed, re-running SQL transaction.");
                            continue;
                        }
                        else
                        {
                            DebugLogger.Write("Error: Transaction retry limit setting reached, aborting SQL transaction.");
                            throw ex;
                        }
                    }

                    if (ex.Message.ToLower().IndexOf("cannot insert duplicate") >= 0)
                    {
                        trainingPage.Name = "###ERROR###Script name already exists";
                        return trainingPage;
                    }
                    throw ex;
                }
            }
            return trainingPage;
        }

        public static DataSet GetActiveTrainingPages(Campaign campaign)
        {
            DataSet dsActiveTrainingPages = null;
            for (int i = 1; i < m_MaxTransactionRetries; i++)
            {
                try
                {
                    dsActiveTrainingPages = SqlHelper.ExecuteDataset(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "Sel_TrainingPageActive_List");
                    break;
                }
                catch (Exception ex)
                {
                    if (ex.Message.IndexOf("Rerun the transaction") > 0)
                    {
                        if (i < m_MaxTransactionRetries)
                        {
                            DebugLogger.Write("Transaction in GetActiveTrainingPages failed, re-running SQL transaction.");
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
            return dsActiveTrainingPages;
        }
        #endregion
    }
}

