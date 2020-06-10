using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Xml;
using RainMakerDialer.CampaignWS;
using Rainmaker.Common.DomainModel;

namespace Rainmaker.RainmakerDialer
{
    public class CampaignAPI
    {

        #region constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public CampaignAPI()
        {
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// Get active campaigns list
        /// </summary>
        /// <returns></returns>
        public static Queue<Campaign> GetActiveCampaigns()
        {
            DialerEngine.Log.Write("|CA|Get active campaigns invoked");
            Queue<Campaign> campaignList = null;
            DataSet ds = null;
            CampaignService objCampService = null;
            Campaign campaign = null;
            try
            {
                campaignList = new Queue<Campaign>();
                objCampService = new CampaignService();
                ds = objCampService.GetCampaignList();
                DataRow[] rows = ds.Tables[0].Select("StatusID = " + ((int)CampaignStatus.Run).ToString());
                for (int i = 0; i < rows.Length; i++)
                {
                    campaign = new Campaign();
                    campaign.CampaignID = (long)rows[i]["campaignID"];
                    campaign.StatusID = (long)(rows[i]["statusID"]);
                    campaign.Description = rows[i]["description"].ToString();
                    campaign.ShortDescription = rows[i]["shortDescription"].ToString();
                    campaign.CampaignDBConnString = rows[i]["campaignDBConnString"].ToString();
                    campaign.FundRaiserDataTracking = (bool)rows[i]["fundRaiserDataTracking"];
                    campaign.RecordLevelCallHistory = (bool)rows[i]["recordLevelCallHistory"];
                    campaign.OnsiteTransfer = (bool)rows[i]["onsiteTransfer"];
                    campaign.FlushCallQueueOnIdle = (bool)rows[i]["FlushCallQueueOnIdle"];
                    //campaign.IsDeleted = (bool)rows[i]["IsDeleted"];
                    campaign.AllowDuplicatePhones = (bool)rows[i]["allowDuplicatePhones"];
                    campaign.DuplicateRule = rows[i]["DuplicateRule"].ToString();
                    campaign.DateCreated = (DateTime)rows[i]["dateCreated"];
                    campaign.DateModified = (DateTime)rows[i]["dateModified"];
                    campaign.OutboundCallerID = rows[i]["OutboundCallerID"].ToString();
                    campaignList.Enqueue(campaign);
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in GetActiveCampaigns");
                throw ex;
            }
            finally
            {
                campaignList = null;
                ds = null;
                objCampService = null;
                campaign = null;
            }
            return campaignList;
        }


        /// <summary>
        /// Get all campaigns list
        /// </summary>
        /// <returns></returns>
        public static Queue<Campaign> GetAllCampaigns()
        {
            //DialerEngine.Log.Write("|CA|GetCampaigns Invoked");
            Queue<Campaign> campaignList = null;
            DataSet ds = null;
            Campaign campaign = null;
            CampaignService objCampService = null;
            try
            {
                campaignList = new Queue<Campaign>();
                objCampService = new CampaignService();
                ds = objCampService.GetCampaignList();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    campaign = new Campaign();
                    campaign.CampaignID = (long)row["campaignID"];
                    campaign.StatusID = (long)(row["statusID"]);
                    campaign.Description = row["description"].ToString();
                    campaign.ShortDescription = row["shortDescription"].ToString();
                    campaign.CampaignDBConnString = row["campaignDBConnString"].ToString();
                    campaign.FundRaiserDataTracking = (bool)row["fundRaiserDataTracking"];
                    campaign.RecordLevelCallHistory = (bool)row["recordLevelCallHistory"];
                    campaign.OnsiteTransfer = (bool)row["onsiteTransfer"];
                    campaign.FlushCallQueueOnIdle = (bool)row["FlushCallQueueOnIdle"];
                    //campaign.IsDeleted = (bool)row["IsDeleted"];
                    campaign.AllowDuplicatePhones = (bool)row["allowDuplicatePhones"];
                    campaign.DuplicateRule = row["DuplicateRule"].ToString();
                    campaign.DateCreated = (DateTime)row["dateCreated"];
                    campaign.DateModified = (DateTime)row["dateModified"];
                    campaign.OutboundCallerID = row["OutboundCallerID"].ToString();
                    try
                    {
                        campaign.DialAllNumbers = (bool)row["DialAllNumbers"];
                    }
                    catch { }
                    campaignList.Enqueue(campaign);
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.ToLower().IndexOf("unable to connect to the remote server") >= 0)
                {
                    DialerEngine.Log.Write("|CA|Exception : Database Accessing Error, Please check services");
                }
                else
                {
                    DialerEngine.Log.WriteException(ex, "Error in GetAllCampaigns");
                }
            }
            finally
            {
                //campaignList = null;
                ds = null;
                campaign = null;
                objCampService = null;
            }
            return campaignList;
        }


        /// <summary>
        /// Get calllist for the query
        /// </summary>
        /// <param name="camp"></param>
        /// <param name="queryCondition"></param>
        /// <returns></returns>
        public static Queue<CampaignDetails> GetCallDetailsByQuery(Campaign objCampaign, string queryCondition, DialingParameter objDialParameter)
        {
            // *** Note : This is running on every call ending.  May not be necessary to hit that often
            DataSet ds = null;
            Queue<CampaignDetails> queryCallQueue = null;
            CampaignService objCampService = null;
            CampaignDetails campaignDetails = null;
            string strCampaignDBConn = objCampaign.CampaignDBConnString;
            try
            {
                string availableQueryCondition = string.Format("{0} {1}", queryCondition, @"AND ( NeverCallFlag=0 or NeverCallFlag IS NULL ) 
			                AND (ScheduleDate is null OR DATEDIFF(dd,getdate(),ScheduleDate) <= 0)
			                AND ((DateTimeofCall is null AND (CallResultCode is null OR CallResultCode = 0))
                                OR CallResultCode NOT IN (
				            SELECT DISTINCT ResultCodeID FROM ResultCode  
				            WHERE 
					            (Redialable = 0 OR NeverCall = 1 OR NeverCall = 2 OR DATEDIFF(dd, Campaign.DateTimeofCall ,GETDATE()) < RecycleInDays)))");

                queryCallQueue = new Queue<CampaignDetails>();
                // Get calllist for this campaign
                objCampService = new CampaignService();
                ds = objCampService.GetCampaignData(strCampaignDBConn, availableQueryCondition);
                DialerEngine.Log.Write("|CA|{0}|{1}|Query run of conditions '{2}' returns {3} records.", objCampaign.CampaignID, objCampaign.ShortDescription, availableQueryCondition, ds.Tables[0].Rows.Count);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    campaignDetails = new CampaignDetails();
                    campaignDetails.UniqueKey = Convert.ToInt64(row["UniqueKey"]);
                    campaignDetails.PhoneNum = row["PhoneNum"] != DBNull.Value ? (row["PhoneNum"]).ToString() : string.Empty;
                    campaignDetails.NumAttemptsAM = row["NumAttemptsAM"] != DBNull.Value ? (row["NumAttemptsAM"]).ToString() : "0";
                    campaignDetails.NumAttemptsPM = row["NumAttemptsPM"] != DBNull.Value ? (row["NumAttemptsPM"]).ToString() : "0";
                    campaignDetails.NumAttemptsWkEnd = row["NumAttemptsWkEnd"] != DBNull.Value ? (row["NumAttemptsWkEnd"]).ToString() : "0";
                    try
                    {
                        campaignDetails.ScheduleDate = row["ScheduleDate"] != DBNull.Value ? Convert.ToDateTime(row["ScheduleDate"]) : DateTime.MinValue;
                    }
                    catch { }

                    try
                    {
                        campaignDetails.OrderIndex = 1;
                    }
                    catch { }

                    bool maxAttemptsOver = false;
                    try{
                        if(objDialParameter.AMCallTimes <= Convert.ToInt32(campaignDetails.NumAttemptsAM)
                            && objDialParameter.PMCallTimes <= Convert.ToInt32(campaignDetails.NumAttemptsPM)
                            && objDialParameter.WeekendCallTimes <= Convert.ToInt32(campaignDetails.NumAttemptsWkEnd))
                        {
                            maxAttemptsOver = true;
                        }
                    }
                    catch{}
                    
                    if(!maxAttemptsOver)
                        queryCallQueue.Enqueue(campaignDetails);
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in GetCallDetailsByQuery");
                throw ex;
            }
            finally
            {
                ds = null;
                //queryCallQueue = null;
                objCampService = null;
                campaignDetails = null;
            }
            return queryCallQueue;
        }

        /// <summary>
        /// Get calllist for the query
        /// </summary>
        /// <param name="camp"></param>
        /// <param name="queryCondition"></param>
        /// <returns></returns>
        public static Queue<CampaignDetails> GetCallDetailsByQuery_Recyle_Last(string strCampaignDBConn, string queryCondition, DialingParameter objDialParameter, long queryId)
        {
            DataSet ds = null;
            Queue<CampaignDetails> queryCallQueue = null;
            CampaignService objCampService = null;
            CampaignDetails campaignDetails = null;
            try
            {
                queryCallQueue = new Queue<CampaignDetails>();
                // Get calllist for this campaign
                objCampService = new CampaignService();
                ds = objCampService.GetCampaignData_Recycle_Last(strCampaignDBConn, queryCondition, queryId);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    campaignDetails = new CampaignDetails();
                    campaignDetails.UniqueKey = Convert.ToInt64(row["UniqueKey"]);
                    campaignDetails.PhoneNum = row["PhoneNum"] != DBNull.Value ? (row["PhoneNum"]).ToString() : string.Empty;
                    campaignDetails.NumAttemptsAM = row["NumAttemptsAM"] != DBNull.Value ? (row["NumAttemptsAM"]).ToString() : "0";
                    campaignDetails.NumAttemptsPM = row["NumAttemptsPM"] != DBNull.Value ? (row["NumAttemptsPM"]).ToString() : "0";
                    campaignDetails.NumAttemptsWkEnd = row["NumAttemptsWkEnd"] != DBNull.Value ? (row["NumAttemptsWkEnd"]).ToString() : "0";
                    try
                    {
                        campaignDetails.ScheduleDate = row["ScheduleDate"] != DBNull.Value ? Convert.ToDateTime(row["ScheduleDate"]) : DateTime.MinValue;
                    }
                    catch { }

                    try
                    {
                        campaignDetails.OrderIndex = Convert.ToInt32(row["OrderIndex"]);
                    }
                    catch { }


                    bool maxAttemptsOver = false;
                    try
                    {
                        if (objDialParameter.AMCallTimes <= Convert.ToInt32(campaignDetails.NumAttemptsAM)
                            && objDialParameter.PMCallTimes <= Convert.ToInt32(campaignDetails.NumAttemptsPM)
                            && objDialParameter.WeekendCallTimes <= Convert.ToInt32(campaignDetails.NumAttemptsWkEnd))
                        {
                            maxAttemptsOver = true;
                        }
                    }
                    catch { }

                    if (!maxAttemptsOver)
                        queryCallQueue.Enqueue(campaignDetails);
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in GetCallDetailsByQuery");
                throw ex;
            }
            finally
            {
                ds = null;
                //queryCallQueue = null;
                objCampService = null;
                campaignDetails = null;
            }
            return queryCallQueue;
        }

        public static bool IsNeverCallSet(string strCampaignDBConn, long uniqueKey)
        {
            try
            {
                bool bNeverCall = false;
                CampaignService objCampService = new CampaignService();
                bNeverCall = objCampService.IsNeverCallSet(strCampaignDBConn, uniqueKey);
            }
            catch { }
            return false;
        }



        public static void UpdateAvailableCountToQuery(string strCampaignDBConn, int availableCount, Query query)
        {
            CampaignService objCampService = null;
            XmlDocument xDocQuery = null;
            try
            {
                xDocQuery = new XmlDocument();
                xDocQuery.LoadXml(Serialize.SerializeObject(query, "Query"));
                objCampService = new CampaignService();
                objCampService.UpdateAvailableCountToQuery(strCampaignDBConn, availableCount, xDocQuery);
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in UpdateAvailableCountToQuery");
                throw ex;
            }
            finally
            {
                xDocQuery = null;
                objCampService = null;
            }
        }

        /// <summary>
        /// Get Active queries 
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public static DataSet GetActiveQueries(Campaign camp)
        {
            //DialerEngine.Log.Write("|CA|GetActiveQueries Invoked for campaign - {0}",
            //    camp.CampaignID.ToString());
            DataSet ds = null;
            CampaignService objCampService = null;
            XmlDocument xDocCampaign = null;
            // Get calllist for this campaign
            try
            {
                objCampService = new CampaignService();
                xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(camp, "Campaign"));

                ds = objCampService.GetActiveQueryList(xDocCampaign);
                return ds;
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in GetActiveQueries");
                throw ex;
            }
            finally
            {
                //ds = null;
                objCampService = null;
                xDocCampaign = null;
            }
        }

        /// <summary>
        /// Get Standby queries 
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public static DataSet GetStandbyQueries(Campaign camp)
        {
            //DialerEngine.Log.Write("|CA|GetStandbyQueries Invoked for campaign - {0}",
            //    camp.CampaignID.ToString());
            DataSet ds = null;
            CampaignService objCampService = null;
            XmlDocument xDocCampaign = null;
            // Get calllist for this campaign
            try
            {
                objCampService = new CampaignService();
                xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(camp, "Campaign"));

                ds = objCampService.GetStandbyQueryList(xDocCampaign);
                return ds;
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in GetStandbyQueries");
                throw ex;
            }
            finally
            {
                //ds = null;
                objCampService = null;
                xDocCampaign = null;
            }
        }

        /// <summary>
        /// Get Dialing parameters
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public static DialingParameter GetDialParam(Campaign objCampaign)
        {
            
                
            CampaignService objCampaignService = null;
            DialingParameter objDialingParameter = null;
            XmlDocument xDocCampaign = null;
            try
            {
                objCampaignService = new CampaignService();
                xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                objDialingParameter = (DialingParameter)Serialize.DeserializeObject(
                    objCampaignService.GetDialingParameter(xDocCampaign), "DialingParameter");
                if (objDialingParameter.DailingParameterID == 0)
                {
                    return null;
                }
                // *** Temp to pull algorithm settings from app config.  to be added to db per campaign
                objDialingParameter.ActiveDialingAlgorithm = Convert.ToInt16(Utilities.GetAppSetting("ActiveDialingAlgorithm", "1"));
                objDialingParameter.CallStatisticsWindow = Convert.ToInt16(Utilities.GetAppSetting("PredictionCallStatsWindow", "100"));
                objDialingParameter.DropRateThrottle = Convert.ToDecimal(Utilities.GetAppSetting("dropRateThrottle", "1"));
                DialerEngine.Log.Write("|CA|{0}|{1}|Dialing parameters refreshed. Algortihm {2}, stats window {3}, throttle {4}", objCampaign.CampaignID, objCampaign.ShortDescription, objDialingParameter.ActiveDialingAlgorithm, objDialingParameter.CallStatisticsWindow, objDialingParameter.DropRateThrottle);
            }
            
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in GetDialParam");
                throw ex;
            }
            finally
            {
                objCampaignService = null;
                //objDialingParameter = null;
                xDocCampaign = null;
            }
            return objDialingParameter;
        }

        /// <summary>
        /// Sets call as hung up in database
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public static void SetCallHangup(long uniqueKey, string dbConn)
        {
            CampaignService objCampaignService = null;
            try
            {
                objCampaignService = new CampaignService();
                objCampaignService.SetCallHangup(uniqueKey, dbConn);

            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in SetHangup of Campaign API");
                throw ex;
            }
            finally
            {
                objCampaignService = null;
            }
            return;
        } 

        /// <summary>
        /// Get current campaign status
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public static long GetCampaignStatus(long campaignID)
        {
            CampaignService objCampaignService = null;
            long statusID;
            try
            {
                objCampaignService = new CampaignService();

                statusID = objCampaignService.GetCampaignStatus(campaignID);

            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in GetCampaignStatus of Campaign API");
                throw ex;
            }
            finally
            {
                objCampaignService = null;
            }
            return statusID;
        } 

        /// <summary>
        /// Get Dialing parameters
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public static OtherParameter GetOtherParam(Campaign objCampaign)
        {
            DialerEngine.Log.Write("|CA|{0}|{1}|Retrieving additional dialing parameters.", objCampaign.CampaignID, objCampaign.ShortDescription);
            CampaignService objCampaignService = null;
            OtherParameter objOtherParameter = null;
            XmlDocument xDocCampaign = null;
            try
            {
                objCampaignService = new CampaignService();
                xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                objOtherParameter = (OtherParameter)Serialize.DeserializeObject(
                    objCampaignService.GetOtherParameter(xDocCampaign), "OtherParameter");
                if (objOtherParameter.OtherParameterID == 0)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in GetOtherParam");
                throw ex;
            }
            finally
            {
                objCampaignService = null;
                //objDialingParameter = null;
                xDocCampaign = null;
            }
            return objOtherParameter;
        }

        /// <summary>
        /// Get Recording parameters
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public static DigitalizedRecording GetDigitizedRecordings(Campaign camp)
        {
            //DialerEngine.Log.Write("|CA|GetDigitizedRecordings Invoked for Campaign - {0}",
            //    camp.CampaignID.ToString());
            CampaignService objCampaignService = null;
            DigitalizedRecording objDigitalizedRecording = null;
            XmlDocument xDocCampaign = null;
            try
            {
                objCampaignService = new CampaignService();
                xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(camp, "Campaign"));
                objDigitalizedRecording = (DigitalizedRecording)Serialize.DeserializeObject(
                    objCampaignService.GetDigitalizedRecording(xDocCampaign), "DigitalizedRecording");
                if (objDigitalizedRecording.DigitalizedRecordingID == 0)
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in GetDigitizedRecordings");
                return null;
            }
            finally
            {
                objCampaignService = null;
                //objDialingParameter = null;
                xDocCampaign = null;
            }
            return objDigitalizedRecording;
        }

        /// <summary>
        /// Update call details
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="callDetail"></param>
        /// <param name="CallType"></param>
        /// <returns></returns>
        public static bool UpdateCallDetails(Campaign objCampaign, CampaignDetails callDetail, CallType CallType, long queryId)
        {
            DialerEngine.Log.Write("|CA|{0}|{1}|Update call details invoked.", objCampaign.CampaignID, objCampaign.ShortDescription);
            CampaignService objCampService = null;
            XmlDocument xDocCallDetail = null;
            XmlDocument xDocCampaign = null;
            try
            {
                if (callDetail != null)
                {
                    objCampService = new CampaignService();
                    xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    xDocCallDetail = new XmlDocument();
                    xDocCallDetail.LoadXml(Serialize.SerializeObject(callDetail, "CampaignDetails"));
                    objCampService.UpdateCallDetail(xDocCampaign, xDocCallDetail, (int)CallType, queryId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in UpdateCallDetails");
            }
            finally
            {
                objCampService = null;
                xDocCampaign = null;
            }
            return false;
        }

        /// <summary>
        /// Add agent id to calldetail
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="callDetail"></param>
        /// <returns></returns>
        public static bool AddAgentToCallDetail(Campaign objCampaign, CampaignDetails callDetail, bool isVerification)
        {

            // 03/19/2010 - removed verification agent as 'Transfered To verification' resultcode is removed (Client request)
            callDetail.VerificationAgentID = "";
            DialerEngine.Log.Write("|CA|{0}|{1}|Adding agent '{2}' to call details invoked, verification flag set to {3}.", objCampaign.CampaignID, objCampaign.ShortDescription, callDetail.AgentName, isVerification);
            CampaignService objCampService = null;
            XmlDocument xDocCallDetail = null;
            XmlDocument xDocCampaign = null;
            try
            {
                if (callDetail != null)
                {
                    objCampService = new CampaignService();
                    xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    xDocCallDetail = new XmlDocument();
                    xDocCallDetail.LoadXml(Serialize.SerializeObject(callDetail, "CampaignDetails"));
                    objCampService.AddAgentToCallDetail(xDocCampaign, xDocCallDetail, isVerification);
                    return true;
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in AddAgentToCallDetail");
            }
            finally
            {
                objCampService = null;
                xDocCampaign = null;
            }
            return false;
        }

        /// <summary>
        /// Add agent id to calldetail
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="callDetail"></param>
        /// <returns></returns>
        public static bool AddVerificationAgentToCallDetail(Campaign objCampaign, CampaignDetails callDetail)
        {
            DialerEngine.Log.Write("|CA|{0}|{1}|Add verification agent '{2}' to call details invoked.", objCampaign.CampaignID, objCampaign.ShortDescription, callDetail.AgentName);
            
            CampaignService objCampService = null;
            try
            {
                if (callDetail != null)
                {
                    objCampService = new CampaignService();
                    objCampService.AddVerificationAgentToCallDetail(callDetail.UniqueKey, Convert.ToInt64(callDetail.AgentID), callDetail.AgentName, objCampaign.CampaignDBConnString);
                    return true;
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in AddAgentToCallDetail");
            }
            finally
            {
                objCampService = null;
            }
            return false;
        }

        
        /// <summary>
        /// Add agent id to calldetail
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="callDetail"></param>
        /// <returns></returns>
        public static void AddResultCodeToCallList(string campDBConn, CampaignDetails callDetail, string strResultDesc)
        {
            CampaignService objCampService = null;
            try
            {
                if (callDetail != null)
                {
                    objCampService = new CampaignService();
                    objCampService.AddResultCodeToCallList(campDBConn, callDetail.UniqueKey, strResultDesc, callDetail.OffsiteTransferNumber);
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in Add Result Code To Call List");
            }
            finally
            {
                objCampService = null;
            }
        }

        public static string GetOffsiteTransferNumber(string campDbConn, CampaignDetails callDetail)
        {
            CampaignService objCampService = null;
            string offsiteNumber = "";
            try
            { 
                if (callDetail != null)
                {
                    objCampService = new CampaignService();
                    offsiteNumber = objCampService.GetOffsiteTransferNumber(campDbConn, callDetail.UniqueKey, callDetail.PhoneNum);
                }
                return offsiteNumber;
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in AddResultCodeToCallList");
                return offsiteNumber;
            }
            finally
            {
                objCampService = null;
            }
        }

        /// <summary>
        /// Logs silent call
        /// </summary>
        /// <param name="camp"></param>
        /// <returns></returns>
        public static void LogSilentCall(Campaign campaign, SilentCall silentCall)
        {
            CampaignService objCampService = null;
            XmlDocument xDocSilentCall = null;
            XmlDocument xDocCampaign = null;
            try
            {
                if (silentCall != null)
                {
                    objCampService = new CampaignService();
                    xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(campaign, "Campaign"));
                    xDocSilentCall = new XmlDocument();
                    xDocSilentCall.LoadXml(Serialize.SerializeObject(silentCall, "SilentCall"));
                    objCampService.LogSilentCall(xDocCampaign, xDocSilentCall);
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in LogSilentCall");
            }
            finally
            {
                objCampService = null;
                xDocCampaign = null;
                //DialerEngine.Log.Write("|CA|LogSilentCall End");
            }
        }

        /// <summary>
        /// Update call comletion times callduration times
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="callDetail"></param>
        /// <returns></returns>
        public static bool UpdateCallCompletion(Campaign campaign, CampaignDetails callDetail)
        {
            CampaignService objCampService = null;
            XmlDocument xDocCallDetail = null;
            XmlDocument xDocCampaign = null;
            try
            {
                if (callDetail != null)
                {
                    objCampService = new CampaignService();
                    xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(campaign, "Campaign"));
                    xDocCallDetail = new XmlDocument();
                    xDocCallDetail.LoadXml(Serialize.SerializeObject(callDetail, "CampaignDetails"));
                    objCampService.UpdateCallCompletion(xDocCampaign, xDocCallDetail);
                    return true;
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in UpdateCallCompletion");
            }
            finally
            {
                objCampService = null;
                xDocCampaign = null;
            }
            return false;
        }

        /// <summary>
        /// Update call details, schedule it for later
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="callDetail"></param>
        /// <returns></returns>
        public static bool UpdateCampaignSchedule(Campaign campaign, CampaignDetails callDetail)
        {
            CampaignService objCampService = null;
            XmlDocument xDocCallDetail = null;
            XmlDocument xDocCampaign = null;
            try
            {
                if (callDetail != null)
                {
                    objCampService = new CampaignService();
                    xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(campaign, "Campaign"));
                    xDocCallDetail = new XmlDocument();
                    xDocCallDetail.LoadXml(Serialize.SerializeObject(callDetail, "CampaignDetails"));
                    objCampService.UpdateCampaignSchedule(xDocCampaign, xDocCallDetail,false);
                    return true;
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in UpdateCampaignSchedule");
            }
            finally
            {
                objCampService = null;
                xDocCampaign = null;
                xDocCallDetail = null;
            }
            return false;
        }

        /// <summary>
        /// Checks is call hangup
        /// </summary>
        /// <param name="uniqueKey"></param>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public static bool IsCallHangup(long uniqueKey, string campDBString)
        {
            CampaignService objCampService = null;
            try
            {
                objCampService = new CampaignService();
                return objCampService.IsCallHangup(uniqueKey, campDBString);
            }
            catch
            {
                //DialerEngine.Log.WriteException(ex, "Error in IsCallHangup");
            }
            finally
            {
                objCampService = null;
            }
            return false;
        }


        /// <summary>
        /// Checks is call transfered
        /// </summary>
        /// <param name="uniqueKey"></param>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public static bool IsCallTransfered(long uniqueKey, string campDBString)
        {
            CampaignService objCampService = null;
            try
            {
                objCampService = new CampaignService();
                return objCampService.CheckCampaignTransferCall(campDBString, uniqueKey);
            }
            catch(Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in IsCallTransfered");
            }
            finally
            {
                objCampService = null;
            }
            return false;
        }
        

        /// <summary>
        /// Update campaign query stats
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="callDetail"></param>
        /// <returns></returns>
        public static bool UpdateCampaignQueryStats(Campaign objCampaign, CampaignQueryStatus campQueryStatus)
        {
            DialerEngine.Log.Write("|CA|{0}|{1}|Updating query {2} '{3}' statistics adding: Dials={4}, Talks={5}, AnsM={6}, NoAn={7}, Busy={8}, OPI={9}, Failed={10}, Drop={11}.", objCampaign.CampaignID, objCampaign.ShortDescription, campQueryStatus.QueryID, campQueryStatus.QueryName,
                campQueryStatus.Dials, campQueryStatus.Talks, campQueryStatus.AnsweringMachine, campQueryStatus.NoAnswer, campQueryStatus.Busy, campQueryStatus.OpInt, campQueryStatus.Failed, campQueryStatus.Drops);
            CampaignService objCampService = null;
            XmlDocument xDocCampQueryStatus = null;
            XmlDocument xDocCampaign = null;
            try
            {
                if (campQueryStatus != null)
                {
                    objCampService = new CampaignService();
                    xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    xDocCampQueryStatus = new XmlDocument();
                    xDocCampQueryStatus.LoadXml(Serialize.SerializeObject(campQueryStatus, "CampaignQueryStatus"));
                    objCampService.UpdateCampaignQueryStats(xDocCampaign, xDocCampQueryStatus);
                    return true;
                }
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in UpdateCampaignQueryStats");
            }
            finally
            {
                objCampService = null;
                xDocCampaign = null;
                xDocCampQueryStatus = null;
            }
            return false;
        }

        public static void UpdateQueryStatus(Campaign objCampaign, long QueryID, bool isActive, bool isStandby)
        {
            XmlDocument xDocCampaign = new XmlDocument();
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            CampaignService objCampService = new CampaignService();
            try
            {
                objCampService.CampaignQueryStatusUpdateDialer(xDocCampaign, QueryID, isActive, isStandby);
            }
            catch (Exception ex)
            {
                DialerEngine.Log.WriteException(ex, "Error in UpdateQueryStatus");
            }
            finally
            {
                objCampService = null;
                xDocCampaign = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="campaiagnDB"></param>
        /// <param name="agentId"></param>
        /// <returns></returns>
        public static CampaignDetails GetManualDailCallDetails(string campaiagnDB, long agentId)
        {
            //DialerEngine.Log.Write("|CA|GetCallDetailsByQuery Invoked for agentId - {0}", agentId.ToString());
            DataSet ds = null;
            CampaignService objCampService = null;
            CampaignDetails campaignDetails = null;
            try
            {
                objCampService = new CampaignService();
                ds = objCampService.GetManualDailCallDetails(campaiagnDB, agentId);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    campaignDetails = new CampaignDetails();
                    campaignDetails.UniqueKey = Convert.ToInt64(row["UniqueKey"]);
                    campaignDetails.PhoneNum = row["PhoneNum"] != DBNull.Value ? (row["PhoneNum"]).ToString() : string.Empty;
                    campaignDetails.AgentID = row["AgentID"] != DBNull.Value ? row["AgentID"].ToString() : "0";
                }
            }
            catch (Exception ex)
            {
                //DialerEngine.Log.WriteException(ex, "Error in GetManualDailCallDetails");
                throw ex;
            }
            finally
            {
                ds = null;
                objCampService = null;
            }
            return campaignDetails;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static long DialerConnected()
        {
            CampaignService objCampService = null;
            long id = 0;
            try
            {
                objCampService = new CampaignService();
                id = objCampService.InsertDialerActivity(DateTime.Now);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                objCampService = null;
            }
            return id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ActivityId"></param>
        public static void DialerStarted(long ActivityId)
        {
            if (ActivityId > 0)
            {
                CampaignService objCampService = null;
                try
                {
                    objCampService = new CampaignService();
                    objCampService.UpdateDialerStart(ActivityId, DateTime.Now);
                }
                catch(Exception ex)
                {
                    DialerEngine.Log.WriteException(ex, "Error in DialerStarted");
                }
                finally
                {
                    objCampService = null;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ActivityId"></param>
        public static void DialerStoped(long ActivityId)
        {
            if (ActivityId > 0)
            {
                CampaignService objCampService = null;
                try
                {
                    objCampService = new CampaignService();
                    objCampService.UpdateDialerStop(ActivityId, DateTime.Now);
                }
                catch(Exception ex)
                {
                    DialerEngine.Log.WriteException(ex, "Error in DialerStoped");
                }
                finally
                {
                    objCampService = null;
                }
            }
        }

        /// <param name="campaiagnDBConn"></param>
        public static void SetDialerQueue(long queryId, string campaiagnDBConn)
        {
            CampaignService objCampService = null;
            try
            {
                objCampService = new CampaignService();
                objCampService.UpdateCampaignQueryInDialerQueue(queryId, campaiagnDBConn);
            }
            catch (Exception ex)
            {
                if(!(ex is System.Threading.ThreadAbortException))
                    DialerEngine.Log.WriteException(ex, "Error in SetDialerQueue");
            }
            finally
            {
                objCampService = null;
            }
        }


        public static void UpdateCampaignStatus(Campaign objCampaign)
        {
            CampaignService objCampService = new CampaignService();
            XmlDocument xDocCampaign = new XmlDocument();
            try
            {
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                objCampService.CampaignStatusUpdate(xDocCampaign);
            }
            catch { }
        }

        /// <summary>
        /// Resets all campaigns to idle state.
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryId"></param>
        /// <returns></returns>
        public static void ResetCampaignsToIdle()
        {
            CampaignService objCampService = new CampaignService();
            try
            {
                objCampService.ResetCampaignsToIdle();
            }
            catch { }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <param name="queryId"></param>
        /// <returns></returns>
        public static string PrepareDialerQuery(Campaign objCampaign, string query, long queryId)
        {
            XmlDocument xDocCampaign = new XmlDocument();
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            CampaignService objCampService = new CampaignService();
            string newQueryCondition = ""; 
            try
            {
                newQueryCondition = objCampService.PrepareDialerQuery(xDocCampaign, query, queryId);
            }
            catch{}
            return newQueryCondition;
        }

        /// <summary>
        /// Update Query Status.
        /// </summary>
        public static void UpdateQueryComplete(Campaign objCampaign, long queryID, bool isActive)
        {
            XmlDocument xDocCampaign = new XmlDocument();
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            CampaignService objCampService = new CampaignService();
            try
            {
                objCampService.CampaignQueryComplete(xDocCampaign, queryID, isActive, true);
            }
            catch{}
        }

        #endregion

        #region AdminRequests

        public static DataSet GetAdminRequests()
        {
            //DialerEngine.Log.Write("|CA|GetActiveQueries Invoked for campaign - {0}",
            //    camp.CampaignID.ToString());
            DataSet ds = null;
            CampaignService objCampService = null;
            try
            {
                objCampService = new CampaignService();

                ds = objCampService.GetAdminRequests();
                return ds;
            }
            catch { return null; }
            finally
            {
                objCampService = null;
            }
        }

        public static void UpdateAdminRequestStatus(long requestID, int requestStatus)
        {
            CampaignService objCampService = null;
            try
            {
                objCampService = new CampaignService();
                objCampService.UpdateAdminRequestStatus(requestID, requestStatus);
            }
            catch
            {}
            finally
            {
                objCampService = null;
            }
            return;
        }

        #endregion

    }

}
