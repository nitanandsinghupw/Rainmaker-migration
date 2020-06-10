using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Rainmaker.Common.DomainModel;
using System.Xml;
using Rainmaker.Web.AgentsWS;
using Rainmaker.Web.CampaignWS;

namespace Rainmaker.Web.agent
{
    public partial class LoginStatus : PageBase
    {
        #region Events

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                ResetReceiptModeToZero();
                GetLoginStatusList();
                //ShowAgentDetails();
                //ShowLoginStatusDetail();

                /* AddToCookie("LastAccessedPage", "LoginStatus");
                try
                {
                    AddToCookie("CampaignId", ((Agent)Session["LoggedAgent"]).CampaignID.ToString());

                    AddToCookie("StatId", ((AgentStat)Session["AgentStat"]).StatID.ToString());
                }
                catch { } */
                try
                {
                    Timer2.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["StatsUpdInterval"]);
                }
                catch
                {
                    Timer2.Interval = 300000; // 5 min - 5 * 60 * 1000
                }
            }
        }

        /// <summary>
        /// Navigates to desired screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnOk_Click(object sender, EventArgs e)
        {

            SaveLoginStatus();
        }

        #endregion

        #region Private Methods


        
        /// <summary>
        /// resets the Receipt Mode
        /// </summary>
        private void ResetReceiptModeToZero()
        {

            Agent objAgent;
            AgentStat objAgentStat;
            Campaign objCampaign;

            try
            {
            objCampaign = (Campaign)Session["Campaign"];
            objAgentStat = (AgentStat)Session["AgentStat"];
            objAgent = (Agent)Session["LoggedAgent"];
        
            objAgentStat.ReceiptModeID = objAgent.ReceiptModeID = 0;
            
            AgentService objAgentService = new AgentService();
            CampaignService objCampService = new CampaignService();

            XmlDocument xDocAgent = new XmlDocument();
            XmlDocument xDocCampaign = new XmlDocument();
            XmlDocument xDocAgentStat = new XmlDocument();

            xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

            
                xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
               
                    objAgent = (Agent)Serialize.DeserializeObject(
                        objAgentService.AgentActivityInsertUpdate(xDocAgent), "Agent");

                    try
                    {
                        objAgentStat = (AgentStat)Serialize.DeserializeObject(
                            objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");
                        if (objAgentStat != null)
                        {
                            Session["AgentStat"] = objAgentStat;
                        }
                    }
                    catch { }
               

            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }


        }

        /// <summary>
        /// Gets the login status list
        /// </summary>
        private void GetLoginStatusList()
        {
            //AgentService objAgentService = new AgentService();
            //DataSet ds = new DataSet();
            //try
            //{
            //    ds = objAgentService.GetLoginStatusList();
            //}
            //catch (Exception ex)
            //{
            //    PageMessage = ex.Message;
            //}
            //lbxLoginStatus.DataSource = ds;
            //lbxLoginStatus.DataTextField = "Status";
            //lbxLoginStatus.DataValueField = "LoginStatusID";
            //lbxLoginStatus.DataBind();

            bool showManualDial = true;
            bool isVerificationAgent = false;
            try
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                OtherParameter objOtherParameter = (OtherParameter)Serialize.DeserializeObject(
                    objCampaignService.GetOtherParameter(xDocCampaign), "OtherParameter");
                // Check which agent modes are allowed by the campaign *** Add inbound in future
                if (objOtherParameter.OtherParameterID > 0)
                    showManualDial = objOtherParameter.AllowManualDial;

            }
            catch { }

            try
            {
                if (Session["LoggedAgent"] != null)
                {
                    Agent agent;
                    agent = (Agent)Session["LoggedAgent"];
                    if (showManualDial)
                    {
                        showManualDial = agent.AllowManualDial;
                    }
                    isVerificationAgent = agent.VerificationAgent;
                }
            }
            catch { }

            lbxLoginStatus.Items.Add(new ListItem("Receive Outbound Calls Only", "Receive Outbound Calls Only"));
            if (showManualDial)
                lbxLoginStatus.Items.Add(new ListItem("Manual Dial", "Manual Dial"));
            if (isVerificationAgent)
            {
                lbxLoginStatus.Items.Add(new ListItem("Verification Only", "Verification Only"));
                lbxLoginStatus.Items.Add(new ListItem("Blended: Verification and Calls", "Blended: Verification and Outbound Calls"));
            }
            lbxLoginStatus.Items.Add(new ListItem("Select Another Campaign", "Select Another Campaign"));
            lbxLoginStatus.Items.Add(new ListItem("Log Off", "Log Off"));


        }

        /// <summary>
        /// Shows agent details
        /// </summary>
        private void ShowAgentDetails()
        {

        }

        /// <summary>
        /// Saves Agent Stats, Agent Activity, Log Off status
        /// </summary>
        /// <returns></returns>
        private void SaveLoginStatus()
        {
            Agent objAgent;
            AgentStat objAgentStat;
            Campaign objCampaign;
            /*if (lbxLoginStatus.SelectedValue == "Manual Dial")
            {
                return;
            }*/
            if (Session["Campaign"] != null && Session["AgentStat"] != null && Session["LoggedAgent"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];
                objAgentStat = (AgentStat)Session["AgentStat"];
                objAgent = (Agent)Session["LoggedAgent"];

                if (lbxLoginStatus.SelectedValue == "Receive Outbound Calls Only")
                {
                    objAgentStat.StatusID = objAgent.AgentStatusID = (long)AgentLoginStatus.Ready;
                    objAgentStat.ReceiptModeID = objAgent.ReceiptModeID = (long)AgentCallReceiptMode.OutboundOnly;
                }
                if (lbxLoginStatus.SelectedValue == "Verification Only")
                {
                    objAgentStat.StatusID = objAgent.AgentStatusID = (long)AgentLoginStatus.Ready;
                    objAgentStat.ReceiptModeID = objAgent.ReceiptModeID = (long)AgentCallReceiptMode.VerifyOnly;
                }
                if (lbxLoginStatus.SelectedValue == "Blended: Verification and Outbound Calls")
                {
                    objAgentStat.StatusID = objAgent.AgentStatusID = (long)AgentLoginStatus.Ready;
                    objAgentStat.ReceiptModeID = objAgent.ReceiptModeID = (long)AgentCallReceiptMode.VerifyBlended;
                }
                if (lbxLoginStatus.SelectedValue == "Manual Dial")
                {
                    objAgentStat.StatusID = objAgent.AgentStatusID = (long)AgentLoginStatus.Pause;
                    objAgentStat.ReceiptModeID = objAgent.ReceiptModeID = (long)AgentCallReceiptMode.ManualDial;
                }
                if (lbxLoginStatus.SelectedValue == "Log Off")
                {
                    objAgentStat.LogOffDate = DateTime.Now;
                    objAgentStat.LogOffTime = DateTime.Now;
                }

                AgentService objAgentService = new AgentService();
                CampaignService objCampService = new CampaignService();

                
                XmlDocument xDocAgent = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();
                XmlDocument xDocAgentStat = new XmlDocument();

                xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                try
                {
                    xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
                    if (lbxLoginStatus.SelectedValue == "Receive Outbound Calls Only" || lbxLoginStatus.SelectedValue == "Manual Dial" || lbxLoginStatus.SelectedValue == "Verification Only" || lbxLoginStatus.SelectedValue == "Blended: Verification and Outbound Calls")
                    {
                        objAgent = (Agent)Serialize.DeserializeObject(
                            objAgentService.AgentActivityInsertUpdate(xDocAgent), "Agent");

                        try
                        {
                            objAgentStat = (AgentStat)Serialize.DeserializeObject(
                                objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");
                            if (objAgentStat != null)
                            {
                                Session["AgentStat"] = objAgentStat;
                            }
                        }
                        catch { }
                    }
                    else if (lbxLoginStatus.SelectedValue == "Log Off")
                    {
                        objAgentService.UpdateAgentLogOut(xDocAgent); //Sets LogoutTime to now for specific agent

                        try
                        {
                            objAgentStat = (AgentStat)Serialize.DeserializeObject
                            (
                                objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), 
                                "AgentStat"
                            );
                        }
                        catch { }
                    }

                    string timestamp = "ts=" + DateTime.Now.Ticks.ToString();
                    if (lbxLoginStatus.SelectedValue == "Receive Outbound Calls Only" || lbxLoginStatus.SelectedValue == "Verification Only" || lbxLoginStatus.SelectedValue == "Blended: Verification and Outbound Calls")
                    {
                        ActivityLogger.WriteAgentEntry(objAgent, "Agent has chosen to recieve outbound calls only.");
                        Response.Redirect("~/agent/waitingforCall.aspx" + "?" + timestamp);
                    }
                    else if (lbxLoginStatus.SelectedValue == "Manual Dial")
                    {
                        ActivityLogger.WriteAgentEntry(objAgent, "Agent has chosen to manual dial.");
                        Response.Redirect("~/agent/ManualDial.aspx" + "?" + timestamp);
                    }
                    else if (lbxLoginStatus.SelectedValue == "Select Another Campaign")
                    {
                        ActivityLogger.WriteAgentEntry(objAgent, "Agent has chosen to select another campaign.");
                        Response.Redirect("~/agent/Campaigns.aspx");
                        
                    }
                    else
                    {
                        ActivityLogger.WriteAgentEntry(objAgent, "Agent has chosen to log out.");
                        Session.Remove("LoggedAgent");
                        Response.Redirect("../Logout.aspx" + "?" + timestamp);
                    }

                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }

            }
        }

        #endregion

        protected void Timer2_Tick(object sender, EventArgs e)
        {
            Agent objAgent = (Agent)Session["LoggedAgent"];
            Campaign objCampaign = (Campaign)Session["Campaign"];
            CampaignService objCampService = new CampaignService();

            long lngCampStatusID = objCampService.GetCampaignStatus(objCampaign.CampaignID);

            //if Campaign has gone idle or flushidle kick out agent....
            if (lngCampStatusID == (long)CampaignStatus.Idle || lngCampStatusID == (long)CampaignStatus.FlushIdle)
            {
                //Agent objAgent = (Agent)Session["LoggedAgent"];
                if (!objAgent.IdleInformed)
                {
                    ActivityLogger.WriteAgentEntry(objAgent, "|MD|Informing that current campaign has gone idle.");
                    PageMessage = "Your current campaign has now gone idle.  You will be automatically taken back to the campaign selection screen.";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
                    objAgent.IdleInformed = true;
                }
                else
                {
                    Response.Redirect("../agent/Campaigns.aspx");
                }
            }
        }
    }
}
