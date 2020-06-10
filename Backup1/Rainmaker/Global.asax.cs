using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Rainmaker.Web.AgentsWS;
using Rainmaker.Web.CampaignWS;
using Rainmaker.Common.DomainModel;
using System.Xml;

namespace Rainmaker.Web
{
    public class Global : System.Web.HttpApplication
    {
        static public DBAccess dbAccess;
        static public string strImportMapPath;
        static public string strRecordings;
        static public string strLogFilePath;


        #region Events

        //-------------------------------------------------------------
        /// <summary>
        /// Invokes on application startup.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //-------------------------------------------------------------
        protected void Application_Start(object sender, EventArgs e)
        {

            dbAccess = new DBAccess();
            strImportMapPath = ConfigurationManager.AppSettings["DataImportMapPath"].ToString();
            strRecordings = ConfigurationManager.AppSettings["RecordingsMapPath"].ToString(); 
            strLogFilePath = ConfigurationManager.AppSettings["LogFilesPath"].ToString();            

        }


        //-------------------------------------------------------------
        /// <summary>
        /// Invoked on error conditions.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //-------------------------------------------------------------
        protected void Application_Error(Object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError().GetBaseException();

            string err = "Error Caught in Application_Error event\n" +
             "Error in: " + Request.Url.ToString() +
             "\nError Message:" + objErr.Message.ToString() +
            "\nStack Trace:" + objErr.StackTrace.ToString();
            // System.Diagnostics.EventLog.WriteEntry("Rainmaker Debug", err, System.Diagnostics.EventLogEntryType.Error);
 
            //LogError
            ActivityLogger.WriteException(objErr, "Global");

            Server.ClearError();
        }

        /// <summary>
        /// Invokes on application shutdown.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Application_End(object sender, EventArgs e)
        {
            UpdateAgentStatus();
            LogOffAgentStat();

        }

        /// <summary>
        /// Invokes when a new session is started.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_Start(object sender, EventArgs e)
        {
            //int i = 0;
        }

        /// <summary>
        /// Invokes when Login session Ends.
        /// Note: The Session_End event is raised only when the sessionstate mode
        /// is set to InProc in the Web.config file. If session mode is set to StateServer 
        /// or SQLServer, the event is not raised.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Session_End(object sender, EventArgs e)
        {
            UpdateAgentStatus();
            LogOffAgentStat();
        }


        #endregion

        #region Private Methods



        /// <summary>
        /// Updates the Log out status of an Agent
        /// </summary>
        private void UpdateAgentStatus()
        {            
            if (Session["LoggedAgent"] != null)
            {
                Agent objAgent = new Agent();
                objAgent = (Agent)Session["LoggedAgent"];
                UpdateAgentStatus(objAgent);
            }
        }

        private void UpdateAgentStatus(Agent objAgent)
        {
           objAgent = (Agent)Session["LoggedAgent"];
           if (!objAgent.IsAdministrator)
           {
              AgentService objAgentService = new AgentService();
              XmlDocument xDocAgent = new XmlDocument();
              xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
              objAgentService.UpdateAgentLogOut(xDocAgent); //Sets LogoutTime to now for specific agent
           }
        }

        private void LogOffAgentStat()
        {          
            if (Session["AgentStat"] != null && Session["Campaign"] != null) // agent is already logged in campaign
            {
                Campaign objCampaign =  (Campaign) Session["Campign"];
                AgentStat objAgentStat = (AgentStat) Session["AgentStat"];
                LogOffAgentStat(objCampaign, objAgentStat);
            }
        }

        private void LogOffAgentStat(Campaign objCampaign, AgentStat objAgentStat)
        {
            if (objAgentStat != null && objCampaign != null) // agent is already logged in campaign
            {
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocAgentStat = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();

                objAgentStat.LogOffDate = DateTime.Now;
                objAgentStat.LogOffTime = DateTime.Now;

                xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                objAgentStat = (AgentStat)Serialize.DeserializeObject(
                objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");
            }
        }

        #endregion
    }
}