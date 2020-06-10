using System;
using System.Data;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml;
using Rainmaker.DAL;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Configuration;
using System.Collections.Generic;
using Rainmaker.Common.DomainModel;


namespace Rainmaker.WebServices
{
    /// <summary>
    /// Summary description for CampaignService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class CampaignService : System.Web.Services.WebService
    {
        #region Campaigns

        /// <summary>
        /// InsertUpdate Campaign
        /// </summary>
        [WebMethod]
        public XmlNode CampaignInsertUpdate(XmlNode xNodeCampaign)
        {
            string strCampaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            string strMasterDBConn = ConfigurationManager.AppSettings["MasterDBConn"];
            string strCampaignDBScript = ConfigurationManager.AppSettings["CampaignScriptFilePath_DDL"];

            Campaign objCampaign;

            objCampaign = (Campaign)Serialize.DeserializeObject(xNodeCampaign, "Campaign");
            objCampaign.CampaignDBConnString = strCampaignMasterDBConn.Replace("RainmakerMaster", objCampaign.ShortDescription);

            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.CampaignInsertUpdate(strCampaignMasterDBConn,
                    strMasterDBConn, objCampaign, strCampaignDBScript, string.Empty), "Campaign"));
            }
            catch (Exception ex)
            {
                if (ex.Message == "CampaignDuplicateEntityException")
                    throw ex;
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }


        /// <summary>
        /// Scrubs A Campaign Against the Master DNC List
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <returns></returns>
        [WebMethod]
        public void ScrubDNC(string campaignConnString)
        {
            try
            {
                CampaignAccess.ScrubDNC(campaignConnString);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                
            }
        }





        [WebMethod]
        public XmlNode CampaignClone(XmlNode xNodeCampaign, XmlNode xNodeCloneInfo)
        {
            string strCampaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            string strMasterDBConn = ConfigurationManager.AppSettings["MasterDBConn"];
            // string strCampaignDBScript = ConfigurationManager.AppSettings["CampaignScriptFilePath_DDL"];

            Campaign objCampaign;
            CloneInfo objCloneInfo;

            objCampaign = (Campaign)Serialize.DeserializeObject(xNodeCampaign, "Campaign");
            objCloneInfo = (CloneInfo)Serialize.DeserializeObject(xNodeCloneInfo, "CloneInfo");

            objCampaign.CampaignDBConnString = strCampaignMasterDBConn.Replace("RainmakerMaster", objCampaign.ShortDescription);

            XmlDocument xDoc = new XmlDocument();
            DebugLogger.Write(string.Format("Initializing clone on campaign '{0}'.", objCampaign.ShortDescription));
            try
            {
                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.CloneCampaign(strCampaignMasterDBConn,
                    strMasterDBConn, objCampaign, objCloneInfo), "Campaign"));
            }
            catch (Exception ex)
            {
                if (ex.Message == "CampaignDuplicateEntityException")
                    throw ex;
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Updates the campaign status
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode CampaignStatusUpdate(XmlNode xCampaign)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.CampaignStatusUpdate(strRainmakerMasterDBConn, campaign), "Campaign"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Resets all campaigns to idle for startup and shutdown of dialer
        /// </summary>
        /// <param name="lDialerActivityID"></param>
        /// <param name="dtDialerStartTime"></param>
        [WebMethod]
        public void ResetCampaignsToIdle()
        {
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                CampaignAccess.ResetCampaignsToIdle(strRainmakerMasterDBConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public void ShutdownAllCampaigns()
        {
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                CampaignAccess.ShutdownAllCampaigns(strRainmakerMasterDBConn);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [WebMethod]
        public int GetCampaignActiveDialCount(string campaignConnString)
        {
            try
            {
                return CampaignAccess.GetCampaignActiveDialCount(campaignConnString);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                return 0;
            }
        }

        /// <summary>
        /// Updates Dial all numbers status
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode CampaignDialStatusUpdate(XmlNode xCampaign)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.CampaignDialStatusUpdate(strRainmakerMasterDBConn, campaign), "Campaign"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Gets all campaigns
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetCampaignList()
        {
            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            DataSet dsCamaignList;
            try
            {
                dsCamaignList = CampaignAccess.GetCampaignList(campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsCamaignList;
        }

        /// <summary>
        /// Gets active campaigns
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetActiveCampaignList()
        {
            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            DataSet dsCamaignList;
            try
            {
                dsCamaignList = CampaignAccess.GetActiveCampaignList(campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsCamaignList;
        }

        /// <summary>
        /// Get Campaign ScoreBoard Data
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetCampaignScoreBoardData(XmlNode xCampaign)
        {
            Campaign campaign;
            DataSet ds;
            try
            {
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                ds = CampaignAccess.GetCampaignScoreBoardData(campaign.CampaignDBConnString);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return ds;
        }

        /// <summary>
        /// Delete campaign
        /// </summary>
        /// <returns>result</returns>
        [WebMethod]
        public int DeleteCampaign(long CampaignID, string ShortDescription)
        {
            int result = 0;
            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            string masterDBConn = ConfigurationManager.AppSettings["MasterDBConn"];
            try
            {
                result = CampaignAccess.DeleteCampaign(campaignMasterDBConn, masterDBConn, CampaignID, ShortDescription);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return result;
        }

        /// <summary>
        /// Gets campaign details by campaignId
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetCampaignByCampaignID(long CampaignID)
        {
            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.GetCampaignListByCampaignID(campaignMasterDBConn, CampaignID), "Campaign"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        [WebMethod]
        public void DeleteExportedLeads(XmlNode xKeysToDelete, string strCampaignDBConn)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                List<long> list = (List<long>)Serialize.DeserializeObject(xKeysToDelete, typeof(List<long>));
                CampaignAccess.DeleteExportedLeads(list, strCampaignDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Decode campaign data
        /// </summary>
        /// <param name="list"></param>
        private List<ImportFieldRow> DecodeData(List<ImportFieldRow> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ImportFieldRow row = (ImportFieldRow)list[i];
                List<ImportField> impList = row.ImportFieldsList;
                for (int j = 0; j < impList.Count; j++)
                {
                    ImportField impField = (ImportField)impList[j];
                    impField.FieldValue = Server.UrlDecode(impField.FieldValue);
                }
                row.ImportFieldsList = impList;
                list[i] = row;
            }
            return list;
        }

        /// <summary>
        /// Update call detail
        /// </summary>
        /// <param name="XCampaignDetails"></param>
        /// <param name="isAMCall"></param>
        [WebMethod]
        public void UpdateCallDetail(XmlNode xCampaign, XmlNode XCampaignDetails, int callType, long queryId)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                CampaignDetails campaignDetail = (CampaignDetails)Serialize.DeserializeObject(
                        XCampaignDetails, "CampaignDetails");

                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignAccess.UpdateCallDetail(campaignDetail, callType, campaign.CampaignDBConnString, queryId);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Sets call hangup
        /// </summary>
        /// <param name="uniqueKey"></param>
        /// <param name="strCampaignDbConn"></param>
        [WebMethod]
        public void SetCallHangup(long uniqueKey, string strCampaignDbConn)
        {
            try
            {
                CampaignAccess.SetCallHangup(uniqueKey, strCampaignDbConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Checks is call hangup
        /// </summary>
        /// <param name="uniqueKey"></param>
        /// <param name="strCampaignDbConn"></param>
        [WebMethod]
        public bool IsCallHangup(long uniqueKey, string strCampaignDbConn)
        {
            try
            {
                return CampaignAccess.IsCallHangup(uniqueKey, strCampaignDbConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Add Agent to call
        /// </summary>
        /// <param name="XCampaignDetails"></param>
        /// <param name="isAMCall"></param>
        [WebMethod]
        public void AddAgentToCallDetail(XmlNode xCampaign, XmlNode XCampaignDetails, bool isVerification)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                CampaignDetails campaignDetail = (CampaignDetails)Serialize.DeserializeObject(
                        XCampaignDetails, "CampaignDetails");

                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignAccess.AddAgentToCallDetail(campaignDetail, campaign.CampaignDBConnString, isVerification);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        [WebMethod]
        public void AddVerificationAgentToCallDetail(long Uniquekey, long agentID, string agentName, string strCampaignDBConn)
        {
            try
            {
                CampaignAccess.AddVerificationAgentToCallDetail(Uniquekey, agentID, agentName, strCampaignDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Add RESULT CODE to calllist
        /// </summary>
        /// <param name="XCampaignDetails"></param>
        /// <param name="isAMCall"></param>
        [WebMethod]
        public void AddResultCodeToCallList(string strCampDB, long UniqueKey, string ResultDesc, string offsiteTransferNumber)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                CampaignAccess.AddResultCodeToCallList(strCampDB, UniqueKey, ResultDesc, 0, offsiteTransferNumber);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        [WebMethod]
        public string GetOffsiteTransferNumber(string strCampDB, long UniqueKey, string phoneNumber)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                string offsiteNumber = CampaignAccess.GetOffsiteTransferNumber(strCampDB, UniqueKey, phoneNumber);
                return offsiteNumber;
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Log silent call
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="XSilentCall"></param>
        [WebMethod]
        public void LogSilentCall(XmlNode xCampaign, XmlNode XSilentCall)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                SilentCall silentCall = (SilentCall)Serialize.DeserializeObject(
                        XSilentCall, "SilentCall");

                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignAccess.LogSilentCall(silentCall, campaign.CampaignDBConnString);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Update call details, completion time
        /// </summary>
        /// <param name="XCampaignDetails"></param>
        /// <param name="isAMCall"></param>
        [WebMethod]
        public void UpdateCallCompletion(XmlNode xCampaign, XmlNode XCampaignDetails)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                CampaignDetails campaignDetail = (CampaignDetails)Serialize.DeserializeObject(
                        XCampaignDetails, "CampaignDetails");

                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignAccess.UpdateCallCompletion(campaignDetail, campaign.CampaignDBConnString);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Insert or Update Campaign Details
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="xCampaigndetails"></param>
        /// <returns></returns>
        [WebMethod]
        public void UpdateCampaignDetails(XmlNode xCampaign, string strCampaignDetailsQuery)
        {
            try
            {
                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignAccess.UpdateCampaignDetails(objCampaign, strCampaignDetailsQuery);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Get Campaign Details by key
        /// </summary>
        /// <param name="CampaignDB"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetCampaignDetailsByKey(string CampaignDB, long key, long queryId)
        {
            try
            {
                return CampaignAccess.GetCampaignDetailsByKey(CampaignDB, key, queryId);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }


        [WebMethod]
        public DataSet GetManualDailCallDetails(string CampaignDB, long agentID)
        {
            try
            {
                return CampaignAccess.GetManualDailCallDetails(CampaignDB, agentID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        #endregion

        #region Result Codes

        /// <summary>
        /// Get all Result codes
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetResultCodes(XmlNode xCampaign)
        {
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                return CampaignAccess.GetResultCodes(campaign);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            //return null;
        }

        /// <summary>
        /// InsertUpdate ResultCode
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode ResultCodeInsertUpdate(XmlNode xCampaign, XmlNode xResultCode)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                ResultCode resultCode;
                resultCode = (ResultCode)Serialize.DeserializeObject(xResultCode, "ResultCode");

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.ResultCodeInsertUpdate(campaign, resultCode), "ResultCode"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Get Result Code by ResultCodeID
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="scriptID"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetResultCodeByResultCodeID(XmlNode xCampaign, long resultCodeID)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                xd.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.GetResultCodeByResultCodeID(campaign, resultCodeID), "ResultCode"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        #endregion

        #region OtherParameter

        /// <summary>
        /// Get OtherParameters
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetOtherParameter(XmlNode xCampaign)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                xd.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.GetOtherParameter(campaign), "OtherParameter"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        /// <summary>
        /// InsertUpdate OtherParameters
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode OtherParameterInsertUpdate(XmlNode xCampaign, XmlNode xOtherParams)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                OtherParameter otherParams;
                otherParams = (OtherParameter)Serialize.DeserializeObject(xOtherParams, "OtherParameter");

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.OtherParameterInsertUpdate(campaign, otherParams), "OtherParameter"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        #endregion

        #region DialingParameters

        /// <summary>
        /// Get all Dialing Parameters
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetDialingParameter(XmlNode xCampaign)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                xd.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.GetDialingParameter(campaign), "DialingParameter"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        /// <summary>
        /// InsertUpdate DialingParameters
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode DialingParameterInsertUpdate(XmlNode xCampaign, XmlNode xDialParams)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                DialingParameter dialingParams;
                dialingParams = (DialingParameter)Serialize.DeserializeObject(xDialParams, "DialingParameter");

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.DialingParameterInsertUpdate(campaign, dialingParams), "DialingParameter"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        #endregion

        #region DialerActivity
        /// <summary>
        /// Insert Dailer connect time
        /// </summary>
        /// <param name="dtConnectTime"></param>
        /// <returns></returns>
        [WebMethod]
        public long InsertDialerActivity(DateTime dtConnectTime)
        {
            long lDialerActivityID = 0;

            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                lDialerActivityID = CampaignAccess.InsertDialerActivity(strRainmakerMasterDBConn, dtConnectTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return lDialerActivityID;
        }
        /// <summary>
        /// update dialer start time
        /// </summary>
        /// <param name="lDialerActivityID"></param>
        /// <param name="dtDialerStartTime"></param>
        [WebMethod]
        public void UpdateDialerStart(long lDialerActivityID, DateTime dtDialerStartTime)
        {
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                CampaignAccess.UpdateDialerStart(strRainmakerMasterDBConn, lDialerActivityID, dtDialerStartTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Update dialer stop time
        /// </summary>
        /// <param name="lDialerActivityID"></param>
        /// <param name="dtDialerStopTime"></param>
        [WebMethod]
        public void UpdateDialerStop(long lDialerActivityID, DateTime dtDialerStopTime)
        {
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                CampaignAccess.UpdateDialerStop(strRainmakerMasterDBConn, lDialerActivityID, dtDialerStopTime);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DigitalizedRecording

        /// <summary>
        /// Get all Digitalized Recording
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetDigitalizedRecording(XmlNode xCampaign)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                xd.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.GetDigitalizedRecording(campaign), "DigitalizedRecording"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        /// <summary>
        /// InsertUpdate DigitalizedRecording
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode DigitalizedRecordingInsertUpdate(XmlNode xCampaign, XmlNode xDigitalRec)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                DigitalizedRecording digitalRec;
                digitalRec = (DigitalizedRecording)Serialize.DeserializeObject(xDigitalRec, "DigitalizedRecording");

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.DigitalizedRecordingInsertUpdate(campaign, digitalRec), "DigitalizedRecording"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        #endregion

        #region Campaign QueryStatus

        /// <summary>
        /// Get Campaign Query Status
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetCampaignQueryStatus(XmlNode xCampaign)
        {
            DataSet dsCampaignStatus;
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                dsCampaignStatus = CampaignAccess.GetCampaignQueryStatus(campaign);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsCampaignStatus;
        }

        /// <summary>
        /// Update QueryStatus
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void UpdateQueryStatus(XmlNode xCampaign, string strQueryCondition, XmlNode xCampaignQueryStatus)
        {
            CampaignQueryStatus campaignQueryStatus;
            Campaign campaign;
            try
            {
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                campaignQueryStatus = (CampaignQueryStatus)Serialize.DeserializeObject(
                    xCampaignQueryStatus, "CampaignQueryStatus");
                CampaignAccess.UpdateQueryStatus(campaign, strQueryCondition, campaignQueryStatus);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        [WebMethod]
        public string PrepareDialerQuery(XmlNode xCampaign, string strQueryCondition, long queryId)
        {
            Campaign campaign;
            string newQueryCondition;
            try
            {
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                string dbConnString = campaign.CampaignDBConnString;
                newQueryCondition = CampaignAccess.PrepareDialerQuery(dbConnString, strQueryCondition, queryId);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return newQueryCondition;
        }

        /// <summary>
        /// Update Campaign Query Status
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void CampaignQueryStatusUpdate(XmlNode xCampaign, long campaignQueryID, bool isActive, bool isStandby, bool showMessage, bool resetStats)
        {
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignAccess.CampaignQueryStatusUpdate(campaign, campaignQueryID, isActive, isStandby, showMessage, resetStats);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        [WebMethod]
        public void CampaignQueryStatusUpdateDialer(XmlNode xCampaign, long queryID, bool isActive, bool isStandby)
        {
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignAccess.CampaignQueryStatusUpdateDialer(campaign, queryID, isActive, isStandby);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }
        /// <summary>
        /// Update Campaign Query Status
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void CampaignQueryComplete(XmlNode xCampaign, long QueryID, bool isActive, bool showMessage)
        {
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignAccess.CampaignQueryComplete(campaign, QueryID, isActive, showMessage);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void UpdateCampaignQueryStats(XmlNode xCampaign, XmlNode xCampaignQueryStatus)
        {
            CampaignQueryStatus campaignQueryStatus;
            Campaign campaign;
            try
            {
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                campaignQueryStatus = (CampaignQueryStatus)Serialize.DeserializeObject(
                    xCampaignQueryStatus, "CampaignQueryStatus");
                CampaignAccess.UpdateCampaignQueryStats(campaign, campaignQueryStatus);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void UpdateCampaignQueryInDialerQueue(long queryId, string campaignDBConnString)
        {
            try
            {
                CampaignAccess.UpdateCampaignQueryInDialerQueue(queryId, campaignDBConnString);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Clears QueryStats
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void ClearQueryStats(string strDBConn, string strQueryCondition, XmlNode xCampaignQueryStatus)
        {
            CampaignQueryStatus campaignQueryStatus;
            try
            {
                campaignQueryStatus = (CampaignQueryStatus)Serialize.DeserializeObject(
                    xCampaignQueryStatus, "CampaignQueryStatus");
                CampaignAccess.ClearQueryStats(strDBConn, strQueryCondition, campaignQueryStatus);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        #endregion

        #region Script

        /// <summary>
        /// Gets Script List
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetScriptList(XmlNode xCampaign)
        {
            DataSet dsScriptList;
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                dsScriptList = CampaignAccess.GetScriptList(campaign);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsScriptList;
        }

        /// <summary>
        /// Gets PageList By ScriptId
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetPageListByScriptId(XmlNode xCampaign, long parentScriptId)
        {
            DataSet dsScriptList;
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                dsScriptList = CampaignAccess.GetPageListByScriptId(campaign, parentScriptId);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsScriptList;
        }


        /// <summary>
        /// Get Script Details By ScriptID
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetScriptByScriptID(XmlNode xCampaign, long scriptID)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                xd.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.GetScriptByScriptID(campaign, scriptID), "Script"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        /// <summary>
        /// Get Script Details By ScriptGUID
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetScriptByScriptGUID(XmlNode xCampaign, string scriptGUID)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                xd.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.GetScriptByScriptGUID(campaign, scriptGUID), "Script"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        /// <summary>
        /// Clone script
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string CloneScript(string campDbString, long scriptId, string scriptName, string cloneCampaignDB)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                return CampaignAccess.CloneScript(campDbString, scriptId, scriptName, cloneCampaignDB);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// InsertUpdate Script
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode ScriptInsertUpdate(XmlNode xCampaign, XmlNode xScript)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                Script script;
                script = (Script)Serialize.DeserializeObject(xScript, "Script");

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.ScriptInsertUpdate(campaign, script), "Script"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Delete Script
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public string DeleteScript(XmlNode xCampaign, long scriptID)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                return CampaignAccess.DeleteScript(campaign, scriptID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            //return "";
        }

        #endregion

        #region QueryDetail

        /// <summary>
        /// InsertUpdate Query
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode QueryDetailInsertUpdate(XmlNode xCampaign, List<XmlNode> xQueryDetail, XmlNode xQuery)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                Query query;
                query = (Query)Serialize.DeserializeObject(xQuery, "Query");

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.QueryInsertUpdate(campaign, xQueryDetail, query), "Query"));
            }
            catch (Exception ex)
            {
                if (ex.Message == "DuplicateQueryException")
                    throw ex;
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Get QuerList
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetQueryList(XmlNode xCampaign)
        {
            DataSet dsQueryList;
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                dsQueryList = CampaignAccess.GetQueryList(campaign.CampaignDBConnString);

            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsQueryList;
        }

        /// <summary>
        /// Get Active query list.
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetActiveQueryList(XmlNode xCampaign)
        {
            DataSet dsQueryList;
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                dsQueryList = CampaignAccess.GetActiveQueryList(campaign.CampaignDBConnString);

            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsQueryList;
        }

        /// <summary>
        /// Get Standby query list.
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetStandbyQueryList(XmlNode xCampaign)
        {
            DataSet dsQueryList;
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                dsQueryList = CampaignAccess.GetStandbyQueryList(campaign.CampaignDBConnString);

            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsQueryList;
        }

        /// <summary>
        ///  Get CampaignData
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetCampaignData(string strCampaignDBConn, string strQueryCondition)
        {
            DataSet dsCampaignData;
            try
            {
                dsCampaignData = CampaignAccess.GetCampaignData(strCampaignDBConn, strQueryCondition);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsCampaignData;
        }

        /// <summary>
        ///  Get CampaignData
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetCampaignData_Recycle_Last(string strCampaignDBConn, string strQueryCondition, long queryId)
        {
            DataSet dsCampaignData;
            try
            {
                dsCampaignData = CampaignAccess.GetCampaignData_Recycle_Last(strCampaignDBConn, strQueryCondition, queryId);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsCampaignData;
        }

        /// <summary>
        ///  Get CampaignData
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public bool IsNeverCallSet(string strCampaignDBConn, long uniqueKey)
        {
            try
            {
                return CampaignAccess.IsNeverCallSet(strCampaignDBConn, uniqueKey);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCampaignDBConn"></param>
        /// <param name="availableCount"></param>
        /// <param name="queryID"></param>
        /// <returns></returns>
        [WebMethod]
        public void UpdateAvailableCountToQuery(string strCampaignDBConn, int availableCount, XmlNode xQuery)
        {
            try
            {
                Query query = (Query)Serialize.DeserializeObject(xQuery, "Query");
                CampaignAccess.UpdateAvailableCountToQuery(strCampaignDBConn, availableCount, query);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        ///  Get CampaignData
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public void UpdateCampaignQueriesStats(XmlNode xCampaign)
        {
            try
            {
                Campaign campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                CampaignAccess.UpdateCampaignQueriesStats(campaign);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Get Query Details By QueryID
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetQueryDetailsByQueryID(XmlNode xCampaign, string strQueryID)
        {
            DataSet dsQuerDetails;
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                dsQuerDetails = CampaignAccess.GetQueryDetailsByQueryID(campaign.CampaignDBConnString, strQueryID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsQuerDetails;
        }

        /// <summary>
        /// Delete Query
        /// </summary>
        /// <returns>result</returns>
        [WebMethod]
        public int DeleteQuery(XmlNode xCampaign, long queryID)
        {
            int result = 0;
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                result = CampaignAccess.DeleteQuery(campaign.CampaignDBConnString, queryID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return result;
        }

        [WebMethod]
        public int DeleteQueryByName(XmlNode xCampaign, string queryName)
        {
            int result = 0;
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                result = CampaignAccess.DeleteQuery(campaign.CampaignDBConnString, queryName);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return result;
        }

        /// <summary>
        /// Delete QueryDetail
        /// </summary>
        /// <returns>result</returns>
        [WebMethod]
        public int DeleteQueryDetail(XmlNode xCampaign, long queryDetailID)
        {
            int result = 0;
            XmlDocument xd = new XmlDocument();
            try
            {
                Campaign campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                result = CampaignAccess.DeleteQueryDetail(campaign.CampaignDBConnString, queryDetailID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return result;
        }

        #endregion

        #region Area Code

        /// <summary>
        /// Insert or Update Area Code Rule
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="xAreaCodeRule"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode AreaCodeRuleInsertUpdate(XmlNode xAreaCodeRule)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

                AreaCodeRule areaCodeRule;
                areaCodeRule = (AreaCodeRule)Serialize.DeserializeObject(xAreaCodeRule, "AreaCodeRule");

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.AreaCodeRuleInsertUpdate(strRainmakerMasterDBConn, areaCodeRule), "AreaCodeRule"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Insert or Update Area Code
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="xAreaCode"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode AreaCodeInsertUpdate(XmlNode xAreaCode)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

                AreaCode areaCode;
                areaCode = (AreaCode)Serialize.DeserializeObject(xAreaCode, "AreaCode");

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.AreaCodeInsertUpdate(strRainmakerMasterDBConn, areaCode), "AreaCode"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Gets Area Codes
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetAreaCode()
        {
            DataSet dsAreaCode;
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

                dsAreaCode = CampaignAccess.GetAreaCode(strRainmakerMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsAreaCode;
        }

        /// <summary>
        /// Delete AreaCode
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void DeleteAreaCode(long areaCodeID)
        {
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

                CampaignAccess.DeleteAreaCode(strRainmakerMasterDBConn, areaCodeID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Get Area Code Rule By AgentID
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetAreaCodeRuleByAgentID(long agentID)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

                xd.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.GetAreaCodeRuleByAgentID(strRainmakerMasterDBConn, agentID), "AreaCodeRule"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        #endregion

        #region Global Dialing

        /// <summary>
        /// Insert or Update Global Dialing Params
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="xGlobalDialing"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GlobalDialingInsertUpdate(XmlNode xGlobalDialing)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

                GlobalDialingParams globalDialing;
                globalDialing = (GlobalDialingParams)Serialize.DeserializeObject(xGlobalDialing,
                    "GlobalDialingParams");

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.GlobalDialingInsertUpdate(strRainmakerMasterDBConn, globalDialing),
                    "GlobalDialingParams"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Get Globala Dialing Params
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetGlobalDialingParams()
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

                xd.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.GetGlobalDialingParams(strRainmakerMasterDBConn), "GlobalDialingParams"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        #endregion

        #region Agent Stats

        /// <summary>
        /// Gets Agent Stat List
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetAgentStat(string campaignDBConnString, long campaignId)
        {
            DataSet dsAgentStat;
            try
            {
                dsAgentStat = CampaignAccess.GetAgentStat(campaignDBConnString, campaignId);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsAgentStat;
        }


        /// <summary>
        /// Gets Agent Stat List
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetAgentStatByStatID(XmlNode xCampaign, long agentStatID)
        {
            XmlDocument xAgentStat = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                AgentStat agentStat;
                agentStat = CampaignAccess.GetAgentStatByStatID(campaign.CampaignDBConnString, agentStatID);
                xAgentStat.LoadXml((string)Serialize.SerializeObject(agentStat, "AgentStat"));

            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xAgentStat;
        }

        /// <summary>
        /// AgentStat Insert/Update
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="xAgentStat"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode InsertUpdateAgentStat(XmlNode xCampaign, XmlNode xAgentStat)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {

                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                AgentStat objAgentStat;
                objAgentStat = (AgentStat)Serialize.DeserializeObject(xAgentStat, "AgentStat");

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.InsertUpdateAgentStat(objCampaign, objAgentStat), "AgentStat"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;

        }

        /// <summary>
        /// AgentStat Update
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="agentID"></param>
        /// <param name="strLeadprocessed"></param>
        /// <returns></returns>
        [WebMethod]
        public void UpdateAgentStats(XmlNode xCampaign, long agentID, string strLeadprocessed)
        {
            try
            {

                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignAccess.UpdateAgentStats(objCampaign.CampaignDBConnString, agentID, strLeadprocessed);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }

        }

        [WebMethod]
        public void UpdateAgentStatus(XmlNode xCampaign, XmlNode xAgent)
        {
            try
            {
                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                Agent objAgent;
                objAgent = (Agent)Serialize.DeserializeObject(xAgent, "Agent");

                CampaignAccess.UpdateAgentStatus(objCampaign.CampaignDBConnString, objAgent.AgentID, objAgent.AgentStatusID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }

        }

        #endregion

        #region CampaignFields
        /// <summary>
        /// Gets  Campaign Fields
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetCampaignFields(XmlNode xCampaign)
        {
            DataSet dsCampaignFields;
            try
            {
                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                dsCampaignFields = CampaignAccess.GetCampaignFields(objCampaign);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsCampaignFields;
        }

        /// <summary>
        /// Gets current campaign status
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public long GetCampaignStatus(long campaignID)
        {
            long statusID;
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                statusID = CampaignAccess.GetCampaignStatus(strRainmakerMasterDBConn, campaignID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return statusID;
        }

        /// <summary>
        /// Gets  Field Types
        /// </summary>
        /// <returns>DataSet</returns>
        [WebMethod]
        public DataSet GetFieldTypes()
        {
            DataSet dsFieldTypes;
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                dsFieldTypes = CampaignAccess.GetFieldTypes(strRainmakerMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsFieldTypes;
        }

        /// <summary>
        /// Insert or Update Campaign Fields
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="xCampaignFields"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode CampaignFieldsInsertUpdate(XmlNode xCampaign, XmlNode xCampaignFields)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                try
                {
                    ExceptionManager.Publish(new Exception("CampaignFieldsInsertUpdate From Application"));
                }
                catch { }
                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignFields objCampaignFields;
                objCampaignFields = (CampaignFields)Serialize.DeserializeObject(xCampaignFields, "CampaignFields");

                xDoc.LoadXml((string)Serialize.SerializeObject(
                    CampaignAccess.CampaignFieldsInsertUpdate(objCampaign, objCampaignFields), "CampaignFields"));
            }
            catch (Exception ex)
            {
                if (ex.Message == "DuplicateColumnException")
                    throw ex;
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Delete CampaignField
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void DeleteCampaignField(XmlNode xCampaign, long fieldID)
        {
            try
            {
                try
                {
                    ExceptionManager.Publish(new Exception("DeleteCampaignField From Application"));
                }
                catch { }
                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignAccess.DeleteCampaignField(objCampaign, fieldID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        #endregion

        #region CallList

        /// <summary>
        /// Insert or Update ScheduledCampaign
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="xCampaigndetails"></param>
        /// <returns></returns>
        [WebMethod]
        public void InsertUpdateScheduledCampaign(XmlNode xCampaign, XmlNode xCampaigndetails)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {

                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignDetails objCampaigndetails;
                objCampaigndetails = (CampaignDetails)Serialize.DeserializeObject(xCampaigndetails, "CampaignDetails");

                CampaignAccess.InsertUpdateScheduledCampaign(objCampaign, objCampaigndetails);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }


        }

        /// <summary>
        /// Insert or Update ScheduledCampaign
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="xCampaigndetails"></param>
        /// <returns></returns>
        [WebMethod]
        public void UpdateCampaignSchedule(XmlNode xCampaign, XmlNode xCampaigndetails, bool isFromAgentInterface)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {

                Campaign objCampaign = new Campaign();
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                CampaignDetails objCampaigndetails = new CampaignDetails();
                objCampaigndetails = (CampaignDetails)Serialize.DeserializeObject(xCampaigndetails, "CampaignDetails");

                CampaignAccess.UpdateCampaignSchedule(objCampaign, objCampaigndetails, isFromAgentInterface);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }


        }

        /// <summary>
        /// Gets campaign details by campaignId
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public int GetPhoneLinesInUseCount(long CampaignID)
        {
            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            try
            {
                return CampaignAccess.GetPhoneLinesInUseCount(campaignMasterDBConn, CampaignID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// To check transfer call
        /// </summary>
        /// <param name="strCampDB"></param>
        /// <param name="UniqueKey"></param>
        /// <returns></returns>
        [WebMethod]
        public bool CheckCampaignTransferCall(string strCampDB, long UniqueKey)
        {
            try
            {
                return CampaignAccess.CheckCampaignTransferCall(strCampDB, UniqueKey);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        #endregion

        #region AdminRequests

        [WebMethod]
        public void SubmitAdminRequest(int requestType, string requestData)
        {
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                CampaignAccess.SubmitAdminRequest(strRainmakerMasterDBConn, requestType, requestData);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        [WebMethod]
        public DataSet GetAdminRequests()
        {
            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            DataSet dsRequestList;
            try
            {
                dsRequestList = CampaignAccess.GetAdminRequests(campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsRequestList;
        }

        [WebMethod]
        public void UpdateAdminRequestStatus(long requestID, int requestStatus)
        {
            try
            {
                string strRainmakerMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                CampaignAccess.UpdateAdminRequestStatus(strRainmakerMasterDBConn, requestID, requestStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return;
        }

        #endregion
        #region Training Methods

        [WebMethod]
        public XmlNode GetTrainingScheme(XmlNode xCampaign, long trainingSchemeID)
        {

            XmlDocument xDoc = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                xDoc.LoadXml((string)Serialize.SerializeObject(CampaignAccess.GetTrainingScheme(campaign, trainingSchemeID), "TrainingScheme"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        [WebMethod]
        public DataSet GetTrainingPageList(XmlNode xCampaign, long trainingSchemeID)
        {
            DataSet dsPageList;
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                dsPageList = CampaignAccess.GetTrainingPages(campaign, trainingSchemeID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsPageList;
        }

        [WebMethod]
        public DataSet GetTrainingSchemeList(XmlNode xCampaign)
        {
            DataSet dsSchemeList;
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                dsSchemeList = CampaignAccess.GetTrainingSchemeList(campaign);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsSchemeList;
        }

        [WebMethod]
        public void UpdateActiveTrainingScheme(XmlNode xCampaign, long trainingSchemeID)
        {
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                CampaignAccess.UpdateActiveTrainingScheme(campaign, trainingSchemeID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        [WebMethod]
        public void DeleteTrainingScheme(XmlNode xCampaign, long trainingSchemeID)
        {
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                CampaignAccess.DeleteTrainingScheme(campaign, trainingSchemeID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        [WebMethod]
        public long AddTrainingScheme(XmlNode xCampaign, string trainingSchemeName)
        {
            long trainingSchemeID = 0;
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                trainingSchemeID = CampaignAccess.AddTrainingScheme(campaign, trainingSchemeName);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return trainingSchemeID;
        }

        [WebMethod]
        public XmlNode GetTrainingPage(XmlNode xCampaign, long trainingPageID)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                xDoc.LoadXml((string)Serialize.SerializeObject(CampaignAccess.GetTrainingPage(campaign, trainingPageID), "TrainingPage"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        [WebMethod]
        public XmlNode TrainingPageInsertUpdate(XmlNode xCampaign, XmlNode xTrainingPage)
        {
            XmlDocument xDoc = new XmlDocument();
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                TrainingPage trainingPage;
                trainingPage = (TrainingPage)Serialize.DeserializeObject(xTrainingPage, "TrainingPage");
                xDoc.LoadXml((string)Serialize.SerializeObject(CampaignAccess.TrainingPageInsertUpdate(campaign, trainingPage), "TrainingPage"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        [WebMethod]
        public DataSet GetActiveTrainingPageList(XmlNode xCampaign)
        {
            DataSet dsPageList;
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                dsPageList = CampaignAccess.GetActiveTrainingPages(campaign);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsPageList;
        }
        #endregion
    }
}
