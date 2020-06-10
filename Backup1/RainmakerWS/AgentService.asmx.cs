using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml;
using System.Configuration;
using Rainmaker.DAL;
using Rainmaker.Common.DomainModel;
using Microsoft.ApplicationBlocks.ExceptionManagement;

namespace Rainmaker.WebServices
{
    /// <summary>
    /// Summary description for AgentService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class AgentService : System.Web.Services.WebService
    {
        /// <summary>
        /// Authentaicates the user.
        /// </summary>
        /// <param name="userEmail">The user name.</param>
        /// <param name="userPassword">The user password.</param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode Authenticate(string userName, string userPassword, string ipAddress, string hostName)
        {

            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            DebugLogger.Write(string.Format("Authenticating user {0} login, beginning session.", userName));
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml((string)Serialize.SerializeObject(
                    AgentAccess.AuthenticateAgent(userName, userPassword, campaignMasterDBConn, ipAddress, hostName), "Agent"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        /// <summary>
        /// Returns Agent by ID
        /// </summary>
        /// <param name="agentID">agent id</param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetAgentByAgentID(long agentID)
        {

            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml
                ((string)Serialize.SerializeObject
                    (
                    AgentAccess.GetAgentByAgentID(campaignMasterDBConn, agentID),
                    "Agent"
                    )
                );
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }
        /// <summary>
        /// Returns Agent by ID
        /// </summary>
        /// <param name="agentID">agent id</param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode AgentActivity_AgentID(long agentID)
        {
            
            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml((string)Serialize.SerializeObject(
                    AgentAccess.AgentActivity_AgentID(campaignMasterDBConn,agentID), "Agent"));
                
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }
        
        /// <summary>
        /// Returns Agent by ID
        /// </summary>
        /// <param name="agentID">agent id</param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode InsGet_AgentActivity(XmlNode xAgent, string campaignDB, string ipAddress, string hostName)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                Agent agent;
                agent = (Agent)Serialize.DeserializeObject(xAgent, "Agent");

                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                xd.LoadXml((string)Serialize.SerializeObject(AgentAccess.InsGet_AgentActivity(agent,campaignMasterDBConn,ipAddress,hostName), "Agent"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }
        
        /// <summary>
        /// Returns Agent by ID
        /// </summary>
        /// <param name="agentID">agent id</param>
        /// <returns></returns>
        /// 
        [WebMethod]
        public XmlNode GetAgentWithActivityDetails(long agentID, long activityId, string ipAddress, string hostName)
        {

            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml((string)Serialize.SerializeObject(
                    AgentAccess.GetAgentByAgentID(campaignMasterDBConn, agentID, ipAddress, hostName, activityId), "Agent"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }
        /// <summary>
        /// ToggleAgentReset
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void ToggleAgentReset(long agentID, bool resetSwitch)
        {

            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            try
            {
                AgentAccess.ToggleAgentReset(agentID, resetSwitch, campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return;
        }
       

        /// <summary>
        /// Get AgentList
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetAgentList()
        {
            DataSet dsAgentList;
            try
            {
                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                dsAgentList = AgentAccess.GetAgentList(campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsAgentList;
        }

        /// <summary>
        /// Get AgentList
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetAgentsByCampaign(XmlNode xCampaign)
        {
            DataSet dsAgentList;
            try
            {
                Campaign campaign;
                campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                dsAgentList = AgentAccess.GetAgentsByCampaign(campaign, campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsAgentList;
        }

        /// <summary>
        /// Get AgentList
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetLoggedInAgents()
        {
            DataSet dsAgentList;
            try
            {
                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                dsAgentList = AgentAccess.GetLoggedInAgents(campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsAgentList;
        }

        /// <summary>
        /// Update Agent status
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public void UpdateAgentStatus(XmlNode xAgent)
        {
            try
            {
                Agent agent;
                agent = (Agent)Serialize.DeserializeObject(xAgent, "Agent");

                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                AgentAccess.UpdateAgentStatus(agent, campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// InsertUpdate Agent details
        /// </summary>
        /// <param name="xAgent"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode AgentInsertUpdate(XmlNode xAgent)
        {

            string strCampaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

            Agent agent;
            agent = (Agent)Serialize.DeserializeObject(xAgent, "Agent");

            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.LoadXml((string)Serialize.SerializeObject(
                    AgentAccess.AgentInsertUpdate(strCampaignMasterDBConn, agent), "Agent"));
            }
            catch (Exception ex)
            {
                if (ex.Message == "NumberDuplicateException")
                    throw ex;
                if (ex.Message == "LoginDuplicateException")
                    throw ex;
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Delete Agent
        /// </summary>
        /// <param name="CampaignID"></param>
        /// <returns></returns>
        [WebMethod]
        public int DeleteAgent(long agentID)
        {
            int result = 0;
            string masterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            try
            {
                result = AgentAccess.DeleteAgent(masterDBConn, agentID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return result;
        }

        /// <summary>
        /// Insert or Update Agent and Campaign ID's
        /// </summary>
        /// <param name="xAgentCampaignMap"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode AgentCampaignMapInsertUpdate(XmlNode xAgentCampaignMap)
        {
            string strCampaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

            AgentCampaignMap agentCampaignMap;
            agentCampaignMap = (AgentCampaignMap)Serialize.DeserializeObject(xAgentCampaignMap, "AgentCampaignMap");

            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.LoadXml((string)Serialize.SerializeObject(AgentAccess.AgentCampaignMapInsertUpdate(
                    strCampaignMasterDBConn, agentCampaignMap), "AgentCampaignMap"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Get agentcampaignmap by agentID
        /// </summary>
        /// <param name="agentID"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetAgentCampaignMapByAgentID(long agentID)
        { 

            string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml((string)Serialize.SerializeObject(
                    AgentAccess.GetAgentCampaignMapByAgentID(campaignMasterDBConn, agentID), "AgentCampaignMap"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        /// <summary>
        /// Gets login ststus list
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetLoginStatusList()
        {
            DataSet dsLoginStatusList;
            try
            {
                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                dsLoginStatusList = AgentAccess.GetLoginStatusList(campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsLoginStatusList;
        }

        /// <summary>
        /// Insert or update selected status option
        /// </summary>
        /// <param name="xAgentLogin"></param>
        /// <param name="currentCampaignDBConn"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode LoginStatusInsertUpdate(XmlNode xAgentLogin, string currentCampaignDBConn)
        {
            AgentLogin agentLogin;
            agentLogin = (AgentLogin)Serialize.DeserializeObject(xAgentLogin, "AgentLogin");

            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.LoadXml((string)Serialize.SerializeObject(
                    AgentAccess.LoginStatusInsertUpdate(currentCampaignDBConn, agentLogin), "AgentLogin"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Gets login status detail by agentID
        /// </summary>
        /// <param name="agentID"></param>
        /// <param name="currentCampaignDBConn"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetLoginStatusAgentID(long agentID, string currentCampaignDBConn)
        {
            XmlDocument xd = new XmlDocument();
            try
            {
                xd.LoadXml((string)Serialize.SerializeObject(
                    AgentAccess.GetLoginStatusByAgentID(currentCampaignDBConn, agentID), "AgentLogin"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xd;
        }

        /// <summary>
        /// Agent activity status insert or update
        /// </summary>
        /// <param name="xAgentActivity"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode AgentActivityInsertUpdate(XmlNode xAgent)
        {
            string strCampaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

            Agent agent;
            agent = (Agent)Serialize.DeserializeObject(xAgent, "Agent");

            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.LoadXml((string)Serialize.SerializeObject(AgentAccess.AgentActivityInsertUpdate(
                    strCampaignMasterDBConn, agent), "Agent"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Update agent Logout status
        /// </summary>
        /// <param name="xAgentActivity"></param>
        /// <returns></returns>
        [WebMethod]
        public void UpdateAgentLogOut(XmlNode xAgent)
        {
            try
            {
                Agent agent;
                agent = (Agent)Serialize.DeserializeObject(xAgent, "Agent");

                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                AgentAccess.UpdateAgentLogOut(agent, campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Get Script
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetScript(XmlNode xCampaign)
        {
            DataSet dsScript;
            try
            {
                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                dsScript = AgentAccess.GetScript(objCampaign.CampaignDBConnString, false);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsScript;
        }

        /// <summary>
        /// Get Script
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetScript2(XmlNode xCampaign, bool isVerification)
        {
            DataSet dsScript;
            try
            {
                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                dsScript = AgentAccess.GetScript(objCampaign.CampaignDBConnString, isVerification);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsScript;
        }

        /// <summary>
        /// Get Campaign Details
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetCampaignByAgentID(XmlNode xAgent, XmlNode xCampaign)
        {
            DataSet dsCampaignDetails;
            try
            {
                Agent objagent;
                objagent = (Agent)Serialize.DeserializeObject(xAgent, "Agent");
                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                dsCampaignDetails = AgentAccess.GetCampaignByAgentID(objagent, objCampaign, campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsCampaignDetails;
        }

        /// <summary>
        /// Get Campaign Details
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetCampaignDetailsByAgentID(long agentID, XmlNode xCampaign, bool isManualDial)
        {
            DataSet dsCampaignDetails;
            try
            {
                Campaign objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                dsCampaignDetails = AgentAccess.GetCampaignDetailsByAgentID(agentID, objCampaign, isManualDial);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsCampaignDetails;
        }

         /// <summary>
        /// Get Campaign Details by phone number
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetCampaignDetailsByPhoneNum(string phoneNumber, XmlNode xCampaign)
        {
            DataSet dsCampaignDetails;
            try
            {
                Campaign objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");

                dsCampaignDetails = AgentAccess.GetCampaignDetailsByPhoneNum(phoneNumber, objCampaign);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsCampaignDetails;
        }

        

        /// <summary>
        /// Update ResultCode
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="xAgent"></param>
        /// <param name="uniqueKey"></param>
        /// <param name="callresultcode"></param>
        /// <returns></returns>
        [WebMethod]
        public void UpdateResultCode(XmlNode xCampaign, XmlNode xAgent, long uniqueKey, int callresultcode,long queryId)
        {
            try
            {
                Campaign objCampaign;
                objCampaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                Agent objagent;
                objagent = (Agent)Serialize.DeserializeObject(xAgent, "Agent");
                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                AgentAccess.UpdateResultCode(objCampaign, objagent, uniqueKey, callresultcode, queryId, campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        [WebMethod]
        public long InsertUpdateStation(XmlNode xAgentStation)
        {
            long stationID;
            try
            {
                Station objStation;
                objStation = (Station)Serialize.DeserializeObject(xAgentStation, "Station");

                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];

                stationID = AgentAccess.InsertUpdateStation(campaignMasterDBConn, objStation);
            }
            catch (Exception ex)
            {
                if (ex.Message == "DuplicateColumnException")
                    throw ex;
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return stationID;
        }

        /// <summary>
        /// Delete Agent
        /// </summary>
        /// <param name="CampaignID"></param>
        /// <returns></returns>
        [WebMethod]
        public XmlNode GetStationByStationID(long stationID)
        {
            XmlDocument xDoc = new XmlDocument();
            string masterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            try
            {
                xDoc.LoadXml((string)Serialize.SerializeObject(AgentAccess.GetStationByStationID(masterDBConn, stationID), "Station"));
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;
        }

        /// <summary>
        /// Get Station List
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public DataSet GetStationList()
        {
            DataSet dsStationList;
            try
            {
                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                dsStationList = AgentAccess.GetStationList(campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return dsStationList;
        }

        /// <summary>
        /// Delete Agent
        /// </summary>
        /// <param name="CampaignID"></param>
        /// <returns></returns>
        [WebMethod]
        public int DeleteStation(long stationID)
        {
            int result = 0;
            string masterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            try
            {
                result = AgentAccess.DeleteStation(masterDBConn, stationID);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return result;
        }

        /// <summary>
        /// Insert Campaign Manual Dial
        /// </summary>
        /// <param name="xCampaign"></param>
        /// <param name="agentID"></param>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        [WebMethod]
        public int InsertCampaignManualDial(XmlNode xCampaign, long agentID, string agentName, string phoneNum)
        {
            try
            {
                Campaign campaign = (Campaign)Serialize.DeserializeObject(xCampaign, "Campaign");
                return AgentAccess.InsertCampaignManualDial(campaign, agentID, agentName, phoneNum);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        /// <summary>
        /// Checks whether dialer is running or not
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        public bool IsDialerRunning()
        {
            try
            {
                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                return AgentAccess.IsDialerRunning(campaignMasterDBConn);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                //throw new SoapException();
            }
            return false;
        }

        /// <summary>
        /// To transfer call
        /// </summary>
        /// <param name="strCampDB"></param>
        /// <param name="UniqueKey"></param>
        [WebMethod]
        public void AddCampaignTransferCall(string strCampDB, long UniqueKey, XmlNode xAgent, string offsiteNumber)
        {
            try
            {
                Agent objagent;
                objagent = (Agent)Serialize.DeserializeObject(xAgent, "Agent");
                string campaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
                AgentAccess.AddCampaignTransferCall(strCampDB, UniqueKey, objagent, campaignMasterDBConn, offsiteNumber);
            }
            catch (Exception ex)
            {
                ExceptionManager.Publish(ex);
                throw new SoapException();
            }
        }

        private void InitializeComponent()
        {

        }
    }
}
