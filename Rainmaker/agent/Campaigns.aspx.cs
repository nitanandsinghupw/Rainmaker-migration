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
using Rainmaker.Web.CampaignWS;
using Rainmaker.Web.AgentsWS;

namespace Rainmaker.Web.agent
{
    public partial class Campaigns : PageBase
    {
        #region Events

        private DateTime dtLastCampaignRunCheck = DateTime.Now;
        

        /// <summary>
        /// Page Load event,
        /// get campaign list when 1st time page is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                ResetReceiptModeCampaignIDToZero();

                if (Session["LoggedAgent"] == null)
                {
                    Response.Redirect("../Logout.aspx", true);
                }
                Agent objLoggedAgent = (Agent)Session["LoggedAgent"];

                LogOffAgentStat();
                //objLoggedAgent.CampaignID = 0;
                Session.Remove("Campaign"); //Remove Campaign Session Object

                ShowAgentLoginDetails(); //updates login details, not mapped / computer name etc

                GetActiveCampaignList(); // gets list of campaigns, populates select campaigns, enables ok button if campaigns available
                //ShowAgentCampaignMap();

                //AddToCookie("LastAccessedPage", "Campaigns");
                // Populate hidden fields for auto agent boot
                hdnLastRunCheck.Value = DateTime.Now.ToString();

                try
                { hdnMaxNoCampaignMins.Value = ConfigurationManager.AppSettings["IdleSystemAgentLogoutMinutes"]; }
                catch
                { hdnMaxNoCampaignMins.Value = "30"; }


            }
            else
            {
                try
                {
                    if (SelectedCampaignID.Value != "")
                    {
                        lbxCampaign.SelectedValue = SelectedCampaignID.Value;
                    }
                }
                catch { lbxCampaign.SelectedIndex = 0; }

            }
        }

        /// <summary>
        /// Update Agent Status and Navigates to login status page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnOk_Click(object sender, EventArgs e)
        {
            if (SaveAgentActivity(false))
                Response.Redirect("LoginStatus.aspx");

        }

        /// <summary>
        /// Updates logout status and Navigates to login page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnExit_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Agent has logged out from campaign selection page.");
            if (SaveAgentActivity(true))
                Response.Redirect("../Logout.aspx");

        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            if (hdnLogoutAgent.Value == "true")
            {
                // Log out agent
                SaveAgentActivity(true);
                Response.Redirect("../Logout.aspx");
            }

            GetActiveCampaignList();

            if (lbxCampaign.Items[0].Text == "No Active Campaigns")
            {
                TimeSpan ts = DateTime.Now - Convert.ToDateTime(hdnLastRunCheck.Value);
                if (ts.TotalMinutes > Convert.ToInt16(hdnMaxNoCampaignMins.Value))
                {
                    if (Session["LoggedAgent"] != null)
                        ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "No campaign running time limit of {0} minutes exceeded, forcing logout.", hdnMaxNoCampaignMins.Value);
                    PageMessage = String.Format("No campaigns have been active for over {0} minutes.  You will now be logged out automatically.", hdnMaxNoCampaignMins.Value);
                    hdnLogoutAgent.Value = "true";
                    Timer1.Interval = 1000;
                    return;
                }
            }
            else
            {
                hdnLastRunCheck.Value = DateTime.Now.ToString();
            }
        }

        #endregion

        #region Private Methods


        /// <summary>
        /// resets the Receipt Mode
        /// </summary>
        private void ResetReceiptModeCampaignIDToZero()
        {

            Agent objAgent;
            AgentStat objAgentStat;
            Campaign objCampaign;

            try
            {
                objCampaign = (Campaign)Session["Campaign"];
                objAgentStat = (AgentStat)Session["AgentStat"];
                objAgent = (Agent)Session["LoggedAgent"];
                if (objAgentStat != null)
                {
                    objAgentStat.ReceiptModeID = objAgent.ReceiptModeID = 0;
                }
                objAgent.CampaignID = 0;

                AgentService objAgentService = new AgentService();
                CampaignService objCampService = new CampaignService();

                XmlDocument xDocAgent = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();
                XmlDocument xDocAgentStat = new XmlDocument();
                if (objAgentStat != null)
                {
                    xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));
                }
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));


                xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));

                objAgent = (Agent)Serialize.DeserializeObject(
                    objAgentService.AgentActivityInsertUpdate(xDocAgent), "Agent");

                try
                {
                    if (objAgentStat != null)
                    {
                        objAgentStat = (AgentStat)Serialize.DeserializeObject(
                            objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");
                        if (objAgentStat != null)
                        {
                            Session["AgentStat"] = objAgentStat;
                        }
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
        /// Gets campaign List and Binds to ListBox
        /// </summary>
        private void GetActiveCampaignList()
        {
            DataSet dsCampaignList;
            try
            {
                CampaignService objCampService = new CampaignService();
                dsCampaignList = objCampService.GetActiveCampaignList();
                bool bCampaignsExists = (dsCampaignList.Tables[0].Rows.Count > 0);
                lbtnOk.Visible = bCampaignsExists;
                if (bCampaignsExists)
                {
                    lbxCampaign.SelectedIndex = -1;
                    lbxCampaign.DataSource = dsCampaignList;
                    lbxCampaign.DataTextField = "Description"; // Replaced Short description
                    lbxCampaign.DataValueField = "CampaignID";
                    lbxCampaign.DataBind();

                    if (Request.QueryString["CampaignID"] != null)
                    {
                        lbxCampaign.SelectedValue = Request.QueryString["CampaignID"].ToString();
                    }
                }
                else
                {
                    lbxCampaign.Items.Clear();
                    ListItem li = new ListItem("No Active Campaigns", "0");
                    lbxCampaign.Items.Add(li);
                    lbxCampaign.SelectedValue = "0";
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Shows agent login details
        /// </summary>
        private void ShowAgentLoginDetails()
        {

            if (Session["LoggedAgent"] != null)
            {
                Agent objAgent = (Agent)Session["LoggedAgent"];
                string strOffhookStatus = "Remote";
                if (objAgent.AllwaysOffHook)
                    strOffhookStatus = "Always OffHook";
                if (objAgent.StationNumber != "")
                {
                    lblLoginDetails.Text = string.Format("[ {0}, {1} ({2}) ] ", objAgent.StationHost,
                        objAgent.StationNumber, strOffhookStatus);
                }
                else
                {
                    lblLoginDetails.Text = string.Format("[ {0}, Not mapped ] ", objAgent.StationHost);
                }
            }
        }

        /// <summary>
        /// Shows the existing campaign detail by agentID
        /// </summary>
        private void ShowAgentCampaignMap()
        {
            Agent objAgent;
            AgentService objAgentService = new AgentService();
            AgentCampaignMap objAgentCampaignMap;
            if (Session["LoggedAgent"] != null)
            {
                objAgent = (Agent)Session["LoggedAgent"];
                objAgentCampaignMap = (AgentCampaignMap)Serialize.DeserializeObject(
                                   objAgentService.GetAgentCampaignMapByAgentID(objAgent.AgentID), "AgentCampaignMap");
                if (objAgentCampaignMap.CampaignID > 0)
                {
                    try
                    {
                        lbxCampaign.SelectedValue = lbxCampaign.Items.FindByValue(
                            objAgentCampaignMap.CampaignID.ToString()).Value;
                        ViewState["AgentCampaignMapID"] = objAgentCampaignMap.AgentCampaignMapID;
                    }
                    catch
                    {
                        // Check is Campaign Removed
                    }
                }
            }
        }

        /// <summary>
        /// Saves agent campaignmap details:
        /// Save Agent Stat and Agent Activity if isLogOff is false,
        /// else Update Log out status
        /// </summary>
        /// <returns></returns>
        private bool SaveAgentActivity(bool isLogoff)
        {

            Agent objAgent;

            if (Session["LoggedAgent"] != null)
            {
                objAgent = (Agent)Session["LoggedAgent"];

                if (lbxCampaign.SelectedValue != "")
                    objAgent.CampaignID = Convert.ToInt64(lbxCampaign.SelectedValue);

                AgentService objAgentService = new AgentService();
                XmlDocument xDocAgent = new XmlDocument();

                try
                {
                    xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));

                    if (!isLogoff)
                    {
                        string chosenCampaignName = "";
                        if (objAgent.CampaignID != 0)
                        {
                            
                            /*if (Request.QueryString["CampaignID"] != null && Request.QueryString["CampaignID"].ToString() == lbxCampaign.SelectedValue)
                            {
                                if (Session["AgentStat"] != null)
                                    objAgentStat = (AgentStat)Session["AgentStat"];
                            }*/
                            
                                

                                // Get selected campaign details and login in agent stat
                                CampaignService objCampService = new CampaignService();

                                Campaign objCampaign = (Campaign)Serialize.DeserializeObject(objCampService.GetCampaignByCampaignID(objAgent.CampaignID), "Campaign");
                                Session["Campaign"] = objCampaign;
                                AgentStat objAgentStat;
                                objAgentStat = new AgentStat();
                                objAgentStat.AgentID = objAgent.AgentID;
                                objAgentStat.StatusID = objAgent.AgentStatusID;
                                objAgentStat.LoginDate = objAgent.LoginTime;
                                objAgentStat.LoginTime = objAgent.LoginTime;

                                chosenCampaignName = objCampaign.Description;

                                XmlDocument xDocAgentStat = new XmlDocument();
                                XmlDocument xDocCampaign = new XmlDocument();
                                xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));
                                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                                objAgentStat = (AgentStat)Serialize.DeserializeObject(objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");
                                Session["AgentStat"] = objAgentStat;
                            
                        }
                        // change the campaign
                        objAgent = (Agent)Serialize.DeserializeObject(
                            objAgentService.AgentActivityInsertUpdate(xDocAgent), "Agent");
                        ActivityLogger.WriteAgentEntry(objAgent, "Campaign {0} with ID {1} has been chosen.", chosenCampaignName, objAgent.CampaignID);
                    }
                    else
                    {
                        objAgentService.UpdateAgentLogOut(xDocAgent); //Sets LogoutTime to now for specific agent
                        //LogOffAgentStat();
                        Session.Remove("LoggedAgent");
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
            }
            return false;
        }


        #endregion

    }
}
