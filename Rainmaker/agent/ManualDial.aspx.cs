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
using System.Text;
using Rainmaker.Web.CampaignWS;
using Rainmaker.Web.AgentsWS;
using System.Text.RegularExpressions;
using System.IO;


namespace Rainmaker.Web.agent
{
    public partial class ManualDial : PageBase
    {
        #region Events

        private string[] hideSysResultCodes = { 
                "Scheduled Callback","Transferred to Agent",
                "Transferred to Dialer","Transferred to Verification","Unmanned Live Contact",
                "Inbound Abandoned by Agent","Inbound abandoned by Caller","Error","Failed",
                "Cadence Break","Loop Current Drop","Pbx Detected","No Ringback","Analysis Stopped",
                "No DialTone","FaxTone Detected", "Unmanned Transferred to Answering Machine", "Transferred Offsite" };

        private string[] hideResultCodesForAgent = null;

        /// <summary>
        /// Page Load event,
        /// get campaign list,
        /// get campaign fields when 1st time page is loaded
        /// and refresh the page to allow manual dial after dispose
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lbtnDisposeLookup.Attributes.Add("onClick", "$(\"#dispositionDialog\").dialog(\"open\"); return false;");

                try
                {
                    lbtnSchedule.Attributes.Add("onClick", "javascript:saveCampaign('Schedule',null);return false;");
                }
                catch { }

                try
                {
                    lbtnScheduleLookup.Attributes.Add("onClick", "javascript:saveCampaign('ScheduleLookup',null);return false;");
                }
                catch { }

                if (Session["LoggedAgent"] == null)
                {
                    Response.Redirect("../Logout.aspx", true);
                }

                pnlLookupButtons.Visible = false;
                pnlToolbar.Visible = true;

                GetActiveCampaignList();
                SetCurrentCampaign();
                GetCampaignFields();
                BindScript(true);

                ScriptManager.RegisterStartupScript(this, typeof(Page), this.UniqueID, "cursorfix();", true);
                ScriptManager.RegisterStartupScript(this, typeof(Page), this.UniqueID, "setSelectionRange();", true);
                ScriptManager.RegisterStartupScript(this, typeof(Page), this.UniqueID, "doGetCaretPosition();", true);

                ScriptManager.GetCurrent(this.Page).SetFocus(this.txtPhoneNumber);
                try
                {
                    Timer2.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["StatsUpdInterval"]);
                }
                catch
                {
                    Timer2.Interval = 300000; // 5 min - 5 * 60 * 1000
                }
                string pagefrom = Request.QueryString["pagefrom"];
                hdnPageFrom.Value = pagefrom;
            }
            else
            {
                if (hdnDisposeDialog.Value == "MDFS")
                {
                    hdnDisposeDialog.Value = "";
                }

                if (hdnDispose.Value == "IsDispose" || hdnDispose.Value == "IsScheduled")
                {
                    pnlLookupButtons.Visible = false;
                    pnlToolbar.Visible = true;
                    txtPhoneNumber.Text = string.Empty;
                    BindScript(true);

                    UpdatePauseTime();
                    if (hdnPageFrom.Value == "waitingforcall")
                    {
                        Response.Redirect("~/Agent/WaitingforCall.aspx?ts=" + DateTime.Now.Ticks.ToString());
                    }
                    else
                    {
                        Response.Redirect("~/Agent/ManualDial.aspx?ts=" + DateTime.Now.Ticks.ToString());

                    }
                }

                string ScriptID = hndScriptID.Value;
                if (ScriptID != "")
                {
                    ScriptID = "";
                    hndScriptID.Value = "";

                    string isDial = hdnIsDial.Value;
                    if (isDial != "")
                    {
                        BindScript(true);
                    }
                    else
                    {
                        BindScript(false);
                    }
                }
            }

            ScriptManager.RegisterStartupScript(this, typeof(Page), this.UniqueID, "setPhoneNumberCaret();", true);
            ScriptManager.RegisterStartupScript(this, typeof(Page), this.UniqueID, "SelectDropDown(id,val);", true);
        }

        protected void OKButton_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("../Logout.aspx");
        }


        /// <summary>
        /// It will change agent status to Pause
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnPause_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|Pause button has been clicked on a manual dial.");
            Session["MDPauseTime"] = DateTime.Now;
            SetAgentstatus();
        }

        /// <summary>
        /// It will change agent status to Ready
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnReady_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|Ready button has been clicked on a manual dial.");
            UpdatePauseTime();
            SetAgentstatus();
        }

        /// <summary>
        /// Agent Quits the app and redirects to Login page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnLogoff_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|Logoff button has been clicked on a manual dial.");
            UpdatePauseTime();
            ChangeAgentCampaign(true);
        }

        /// <summary>
        /// It will redirects to Login status Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnclose_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|Close button has been clicked on a manual dial.");
            UpdatePauseTime();
            if (lbtnlogoff.Visible)
            {
                Response.Redirect("~/Agent/LoginStatus.aspx?ts=" + DateTime.Now.Ticks.ToString());
            }
            else
            {
                if (hdnPageFrom.Value == "waitingforcall")
                {

                    Response.Redirect("~/Agent/WaitingforCall.aspx?ts=" + DateTime.Now.Ticks.ToString());
                }
                else
                {
                    Response.Redirect("~/Agent/ManualDial.aspx?ts=" + DateTime.Now.Ticks.ToString());

                }
            }
        }

        /// <summary>
        /// Update stats for every 3 min
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Timer2_Tick(object sender, EventArgs e)
        {
            string pagefrom = Request.QueryString["pagefrom"];
            hdnPageFrom.Value = pagefrom;
            if (pagefrom != null)
            {

                string action = Request.QueryString["action"];
                string actionnumber = Request.QueryString["actionnumber"];
                string callbackkey = Request.QueryString["callbackkey"];

                if (action != "" && action == "forcedial")
                {
                    if (actionnumber != "")
                    {
                        hdnCallBackKey.Value = callbackkey;
                        txtPhoneNumber.Text = actionnumber;
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), this.UniqueID, "forcedial(false);", true);
                    }

                }
                else if (action != "" && action == "forcediallookup")
                {
                    if (actionnumber != "")
                    {
                        hdnCallBackKey.Value = callbackkey;
                        txtPhoneNumber.Text = actionnumber;
                        ScriptManager.RegisterClientScriptBlock(this, typeof(Page), this.UniqueID, "forcedial(true);", true);
                    }

                }

            }

            UpdateTimesFromTimer();
            try
            {
                //check for reset

                Agent objAgent = (Agent)Session["LoggedAgent"];

                Agent objAgentWS;

                if (objAgent.PhoneNumber == "")
                {
                    if (!objAgent.PhoneInformed)
                    {
                        ActivityLogger.WriteAgentEntry(objAgent, "|MD|Your phone number is required and missing.  Please contact your system administrator to add your contact phone number.");
                        PageMessage = "Your phone number is required and missing.  Please contact your system administrator to add your contact phone number.";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
                        objAgent.PhoneInformed = true;
                    }
                    else
                    {
                        Response.Redirect("../agent/LoginStatus.aspx");

                    }

                }
                AgentService objAgentService = new AgentService();
                long agentID = objAgent.AgentID;
                objAgentWS = (Agent)Serialize.DeserializeObject(
                               objAgentService.GetAgentByAgentID(agentID), "Agent");
                CampaignService objCampService = new CampaignService();
                Campaign objCampaign;
                objCampaign = (Campaign)Session["Campaign"];
                //If the agent is reset and has a status of pause or ready then ....
                if ((lbtnPause.Visible || lbtnReady.Visible) && objAgentWS.IsReset)
                {
                    objAgentService.ToggleAgentReset(objAgent.AgentID, false);

                    ActivityLogger.WriteAgentEntry(objAgent, "Informing agent that he is being logged off.");



                    AgentStat objAgentStat;
                    objAgentStat = (AgentStat)Session["AgentStat"];


                    XmlDocument xDocAgent = new XmlDocument();


                    XmlDocument xDocCampaign = new XmlDocument();

                    XmlDocument xDocAgentStat = new XmlDocument();


                    xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));


                    objAgentStat.LogOffDate = DateTime.Now;
                    objAgentStat.LogOffTime = DateTime.Now;

                    xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));
                    try
                    {
                        objAgentService.UpdateAgentLogOut(xDocAgent); //Sets LogoutTime to now for specific agent
                        objAgentStat = (AgentStat)Serialize.DeserializeObject(
                          objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");
                    }
                    catch (Exception ee)
                    {
                        ActivityLogger.WriteException(ee, "Agent");
                        PageMessage = ee.Message;
                    }

                    this.MPE_Modal1.Show();

                }


                // Added GW 09.30.2010 - Pop on idle or paused for agents


                long lngCampStatusID = objCampService.GetCampaignStatus(objCampaign.CampaignID);

                //if Campaign has gone idle or flushidle kick out agent....
                if ((lngCampStatusID == (long)CampaignStatus.Idle || lngCampStatusID == (long)CampaignStatus.FlushIdle) && !objAgentWS.IsReset)
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

                /*if (lngCampStatusID == (long)CampaignStatus.ShutDown)
                {
                    //Agent objAgent = (Agent)Session["LoggedAgent"];
                    if (!objAgent.IdleInformed)
                    {
                        ActivityLogger.WriteAgentEntry(objAgent, "|MD|Informing that shutdown has been initiated.");
                        //PageMessage = "All Campaigns have been shutdown.  You will be automatically taken back to the login screen.";
                        //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
                        objAgent.IdleInformed = true;
                    }
                    else
                    {
                        Response.Redirect("../agent/Default.aspx");
                    }
                }*/

                //if the Campaign is paused.... 
                if (lngCampStatusID == (long)CampaignStatus.Pause)
                {

                    if (!objAgent.PauseInformed)
                    {
                        if (Session["LoggedAgent"] != null)
                            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|Informing agent that the campaign has paused.");
                        PageMessage = "Notification: Your current campaign has now paused.  When finished with current call, you should choose another campaign or ask your manager for direction.";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
                        objAgent.PauseInformed = true;
                    }
                }
            }
            catch { }
            /*if (hdnCallBackKey.Value == "")
            {
                CheckForCallBacks();
            }*/
        }

        /// <summary>
        /// Hang up the phone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnHangup_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["LoggedAgent"] != null)
                    ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|Hangup button has been clicked on a manual dial.");

                if (Session["CampaignDtls"] != null && Session["Campaign"] != null)
                {
                    DataSet dsCampaignDtls = (DataSet)Session["CampaignDtls"];
                    Campaign objCampaign = (Campaign)Session["Campaign"];

                    if (dsCampaignDtls.Tables[0].Rows.Count > 0)
                    {
                        long uniqueKey = Convert.ToInt64(dsCampaignDtls.Tables[0].Rows[0]["UniqueKey"].ToString());
                        string strDBConnstr = objCampaign.CampaignDBConnString;
                        CampaignService objCampService = new CampaignService();
                        objCampService.SetCallHangup(uniqueKey, strDBConnstr);
                        if (Session["LoggedAgent"] != null)
                            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|Hangup flag has been set for the dialer.");
                        lbtnHangup.Enabled = false;
                        lbtnHangup.Text = "<img src=\"../images/hangup_grey_red.jpg\" alt=\"hangup\" border=\"0\" id=\"imgHangup\"/>";
                    }
                }
            }
            catch (Exception ex)
            {
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|lbtnHangup Hangup Error." + ex.Message);

            }
        }

        // THIS IS WHERE THE MANUAL DIAL IS TRIGGERED
        protected void lbtnDial_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|Manual dial initiated, checking campaign status running = {0}.", CheckCampaignStatus());

            if (!chkLookup.Checked)
            {
                hdnIsDial.Value = "isDial";

                //Check if capaign is running
                if (CheckCampaignStatus())
                {
                    InsertCampaignManualDial();
                }
                else
                {
                    UpdateAgent();
                    RenderCampaignInActiveScript();
                }
            }
            else
            {
                hdnIsDial.Value = "";
                if (Session["LoggedAgent"] != null)
                    ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|Manual dial lookup initiated, binding script.");
                BindScript(false);
            }
        }

        private void UpdateAgent()
        {
            try
            {
                LogOffAgentStat();
                AgentService objAgentService = new AgentService();
                Agent objAgent = (Agent)Session["LoggedAgent"];
                objAgent.CampaignID = 0;
                XmlDocument xDocAgent = new XmlDocument();
                xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
                objAgent = (Agent)Serialize.DeserializeObject(
                        objAgentService.AgentActivityInsertUpdate(xDocAgent), "Agent");
                Session["LoggedAgent"] = objAgent;
            }
            catch { }
        }

        private void RenderCampaignInActiveScript()
        {
            ClientScript.RegisterClientScriptBlock(this.GetType(), "CmapaignIdle",
                "<script language=\"javascript\" type=\"text/javascript\">ShowInvalidCampaign();</script>");
        }

        /// <summary>
        /// Change Campaign of Agent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlCompaign_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeAgentCampaign(false);
        }

        /// <summary>
        /// clear the text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            txtPhoneNumber.Text = string.Empty;
        }

        #endregion

        #region Private Methods

        #region Campaign
        /// <summary>
        /// Gets all campaigns that are in 'Run' status and that are not deleted.
        /// Populates drop down with these campaigns
        /// </summary>
        private void GetActiveCampaignList()
        {
            DataSet dsCampaignList;
            try
            {
                CampaignService objCampService = new CampaignService();
                dsCampaignList = objCampService.GetActiveCampaignList();
                ddlCompaign.DataSource = dsCampaignList;
                ddlCompaign.DataTextField = "Description";// Replaced Short description
                ddlCompaign.DataValueField = "CampaignID";
                ddlCompaign.DataBind();

                //ddlCompaign.Items.Insert(0, (new ListItem("Select Campaign", "0")));
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        private bool CheckCampaignStatus()
        {
            try
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                CampaignService objCampService = new CampaignService();
                Campaign tempCampaign = (Campaign)Serialize.DeserializeObject(
                    objCampService.GetCampaignByCampaignID(objCampaign.CampaignID), "Campaign");
                return tempCampaign.StatusID == (long)CampaignStatus.Run;
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// If we are in a campaign this code selects current campaign in the dropdown
        /// </summary>
        private void SetCurrentCampaign()
        {
            Campaign objCampaign;

            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];
                if (ddlCompaign.Items.FindByValue(objCampaign.CampaignID.ToString()) != null)
                    ddlCompaign.SelectedValue = objCampaign.CampaignID.ToString();
            }
        }


        /// <summary>
        /// logoff agent if isLofOff 
        /// else change campaign:-  logoff previous campaign and login agent stat in new campaign and update 
        /// campaign in agent activity, 
        /// </summary>
        /// <param name="isLogoff"></param>
        private void ChangeAgentCampaign(bool isLogoff)
        {
            Agent objAgent = (Agent)Session["LoggedAgent"];

            if (ddlCompaign.SelectedValue != "0")
            {
                try
                {
                    if (ddlCompaign.SelectedValue != "")
                        objAgent.CampaignID = Convert.ToInt64(ddlCompaign.SelectedValue);

                    AgentService objAgentService = new AgentService();
                    XmlDocument xDocAgent = new XmlDocument();


                    if (!isLogoff)
                    {
                        AgentStat objAgentStat;
                        Campaign objCampaign = (Campaign)Session["Campaign"];

                        if (objCampaign != null && objCampaign.CampaignID.ToString() == ddlCompaign.SelectedValue)
                        {
                            if (Session["AgentStat"] != null)
                                objAgentStat = (AgentStat)Session["AgentStat"];
                        }
                        else
                        {
                            LogOffAgentStat();

                            // Get selected campaign details and login in agent stat
                            CampaignService objCampService = new CampaignService();

                            objCampaign = (Campaign)Serialize.DeserializeObject(objCampService.GetCampaignByCampaignID(objAgent.CampaignID), "Campaign");
                            Session["Campaign"] = objCampaign;

                            objAgentStat = new AgentStat();
                            objAgentStat.AgentID = objAgent.AgentID;
                            objAgentStat.StatusID = objAgent.AgentStatusID;
                            objAgentStat.LoginDate = objAgent.LoginTime;
                            objAgentStat.LoginTime = objAgent.LoginTime;

                            XmlDocument xDocAgentStat = new XmlDocument();
                            XmlDocument xDocCampaign = new XmlDocument();
                            xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));
                            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                            objAgentStat = (AgentStat)Serialize.DeserializeObject(objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");
                            if (objAgentStat != null)
                            {
                                Session["AgentStat"] = objAgentStat;
                            }

                            objAgent.CampaignID = Convert.ToInt64(ddlCompaign.SelectedValue);
                            xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
                            // change the campaign
                            objAgent = (Agent)Serialize.DeserializeObject(
                                objAgentService.AgentActivityInsertUpdate(xDocAgent), "Agent");
                            ActivityLogger.WriteAgentEntry(objAgent, "|MD|Manual dial campaign change complete.");
                        }
                    }
                    else
                    {
                        xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));

                        objAgentService.UpdateAgentLogOut(xDocAgent); //Sets LogoutTime to now for specific agent
                        LogOffAgentStat();
                        Session.Remove("LoggedAgent");
                        Response.Redirect("../Logout.aspx");
                    }
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
            }
        }

        #endregion

        #region Agent Stat
        /// <summary>
        /// Set Agent Status
        /// </summary>
        private void SetAgentstatus()
        {
            try
            {
                if (lbtnPause.Visible)
                {
                    lbtnPause.Visible = false;
                    lbtnReady.Visible = true;
                    pnlManualDial.Visible = false;
                    pnlPause.Visible = true;
                }
                else if (lbtnReady.Visible)
                {
                    lbtnReady.Visible = false;
                    lbtnPause.Visible = true;
                    pnlManualDial.Visible = true;
                    pnlPause.Visible = false;
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        private void UpdatePauseTime()
        {
            if (Session["MDPauseTime"] != null)
            {

                Campaign objCampaign;
                CampaignService objCampService = new CampaignService();
                AgentStat objAgentStat;
                XmlDocument xDocAgentStat = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();

                if (Session["AgentStat"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];

                    objAgentStat = (AgentStat)Session["AgentStat"];

                    try
                    {
                        TimeSpan ts = DateTime.Now.Subtract((DateTime)Session["MDPauseTime"]);
                        objAgentStat.PauseTime += (decimal)ts.TotalSeconds;
                        Session.Remove("MDPauseTime");

                        xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                        objAgentStat = (AgentStat)Serialize.DeserializeObject(
                        objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");

                        Session["AgentStat"] = objAgentStat;
                    }
                    catch { }

                }
            }
        }

        #endregion

        #region Manual Dial
        /// <summary>
        /// Insert Campaign Manual Dial
        /// </summary>
        private void InsertCampaignManualDial()
        {
            AgentService objAgentService = new AgentService();
            try
            {
                if (objAgentService.IsDialerRunning())
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    Agent objAgent = (Agent)Session["LoggedAgent"];

                    XmlDocument xDocCampaign = new XmlDocument();

                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                    int res = objAgentService.InsertCampaignManualDial(xDocCampaign, objAgent.AgentID, objAgent.AgentName, Format(txtPhoneNumber.Text.Trim()));

                    ActivityLogger.WriteAgentEntry(objAgent, "|MD|Manual dial request inserted for number '{0}'.", Format(txtPhoneNumber.Text.Trim()));

                    if (res > 0)
                    {
                        BindScript(true);
                    }
                    else
                    {
                        ActivityLogger.WriteAgentEntry(objAgent, "|MD|Manual dial request for number '{0}' was rejected because DNC flag is set or it has been called over the specified number of times.", Format(txtPhoneNumber.Text.Trim()));
                        PageMessage = "This number cannot be dialed because it exists in the campaign data with the DNC flag set or it has been called over the specified number of times.";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
                    }
                }
                else
                {
                    if (Session["LoggedAgent"] != null)
                        ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|Manual dial request for number '{0}' was rejected because the dialer is not running.");
                    PageMessage = "Please start the dialer engine.";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion

        #region Scripts


        private string GetQueryString(string key)
        {
            string value = "0";
            if (Request.QueryString[key] != null)
            {
                value = Request.QueryString[key];
            }
            return value;
        }
        /// <summary>
        /// Bind Campaign Fields Data and Result code in script if a manual dial record found
        /// </summary>
        private void BindScript(bool isDial)
        {
            if (ShowScript(isDial))
            {
                //Dispose
                lbtnDispose.Enabled = true;
                lbtnDispose.Text = "<img src=\"../images/dispose.jpg\" alt=\"dispose\" border=\"0\" />";
                lbtnDisposeLookup.Enabled = true;
                lbtnDisposeLookup.Text = "<img src=\"../images/dispose.jpg\" alt=\"dispose\" border=\"0\" />";

                //Remove Logoff and Close if its a dial
                if (isDial)
                {
                    lbtnlogoff.Visible = false;
                    lbtnclose.Visible = false;
                }

                lbtnHangup.Enabled = true;
                lbtnHangup.Text = "<img src=\"../images/hangup.jpg\" alt=\"hangup\" border=\"0\" id=\"imgHangup\"/>";

                lbtnSchedule.Enabled = true;
                lbtnSchedule.Text = "<img src=\"../images/schedule.jpg\" alt=\"schedule\" id=\"imgSchedule\" border=\"0\" />";

                lbtnScheduleLookup.Enabled = true;
                lbtnScheduleLookup.Text = "<img src=\"../images/schedule.jpg\" alt=\"schedule\" id=\"imgScheduleLookup\" border=\"0\" />";

                Timer2.Enabled = false;

                DataSet dsCampaignDtls;
                DataSet dsAgentScript;
                Campaign objcampaign;
                AgentService objAgentService = new AgentService();

                Agent objAgent;
                CampaignService objCampaignService = new CampaignService();

                if (Session["Campaign"] != null)
                {
                    objcampaign = (Campaign)Session["Campaign"];

                    try
                    {
                        XmlDocument xDocCampaign = new XmlDocument();
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objcampaign, "Campaign"));

                        // Added for agent busy trigger GW 09.29.10
                        objAgent = (Agent)Session["LoggedAgent"];
                        ActivityLogger.WriteAgentEntry(objAgent, "|MD|Binding and encoding script.");
                        XmlDocument xDocAgent = new XmlDocument();
                        objAgent.AgentStatusID = (long)AgentLoginStatus.Busy;
                        xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
                        objCampaignService.UpdateAgentStatus(xDocCampaign, xDocAgent);
                        ActivityLogger.WriteAgentEntry(objAgent, "|MD|Status has been updated.");
                        bool isVerification = false;
                        try
                        {
                            dsCampaignDtls = (DataSet)Session["CampaignDtls"];
                            int agentID = Convert.ToInt32(dsCampaignDtls.Tables[0].Rows[0]["AgentID"]);
                            int verificationAgentID = Convert.ToInt32(dsCampaignDtls.Tables[0].Rows[0]["VerificationAgentID"] == Convert.DBNull ? 0 : dsCampaignDtls.Tables[0].Rows[0]["VerificationAgentID"]);
                            if (objAgent.AgentID == verificationAgentID) //Determines that this is verification
                                isVerification = true;

                        }
                        catch { }

                        // BINGO, this is where the script is prepared for the client
                        dsAgentScript = objAgentService.GetScript(xDocCampaign);
                        if (dsAgentScript.Tables[0].Rows.Count > 0)
                        {
                            if (dsAgentScript.Tables[0].Rows[0]["ScriptHeader"] != null)
                                ltrlScripthdr.Text = Server.UrlDecode(dsAgentScript.Tables[0].Rows[0]["ScriptHeader"].ToString());
                            //if (dsAgentScript.Tables[0].Rows[0]["ScriptSubHeader"] != null)
                            //    ltrlScriptsubhdr.Text = Server.UrlDecode(dsAgentScript.Tables[0].Rows[0]["ScriptSubHeader"].ToString());
                            if (dsAgentScript.Tables[0].Rows[0]["ScriptBody"] != null)
                                ltrlScriptbody.Text = Server.UrlDecode(dsAgentScript.Tables[0].Rows[0]["ScriptBody"].ToString());

                        }

                        if (Session["CampaignDtls"] != null)
                        {
                            // ** Debug script data
                            dsCampaignDtls = (DataSet)Session["CampaignDtls"];
                            ActivityLogger.WriteAgentEntry(objAgent, "|MD|Campaign data retreived, {0} rows returned.", dsCampaignDtls.Tables[0].Rows.Count);
                            if (dsCampaignDtls.Tables[0].Rows.Count > 0)
                            {
                                string jscript = AssignCampaignFieldsDataToTextFields(dsCampaignDtls, ltrlScriptbody);

                                if (jscript != "")
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript", jscript, true);
                                }

                                BindCampaignFields();

                                lbtnDispose.Attributes.Add("onClick", "$(\"#dispositionDialog\").dialog(\"open\"); return false;");

                                Regex re = new Regex(@"<select data-rmfieldnamesaved=""cboResultCode"" name=""cboResultCode"">\s*<option value=""-1"">#RESULT CODE#</option>\s*</select>");
                                ltrlScriptbody.Text = re.Replace(ltrlScriptbody.Text, "<select name=\"cboResultCode\" onchange=\"DisposeCall('cboResultCode');\"><option value=\"-1\" selected>Select Result Code</option></select>");

                                StringBuilder strResultcodes = new System.Text.StringBuilder();
                                strResultcodes.AppendFormat("<option value=\"-1\" selected>Select Result Code</option>");

                                DataView dvResultCodes;
                                dvResultCodes = BindResultCodes(isVerification);

                                try
                                {
                                    hideSysResultCodes = ConfigurationManager.AppSettings["SysResultCodesToHide"].Split(',');
                                }
                                catch { }

                                if (dvResultCodes != null)
                                {
                                    DataTable dtResultCodes = dvResultCodes.ToTable();

                                    foreach (DataRow dr in dtResultCodes.Rows)
                                    {
                                        string resultCode = dr["Description"].ToString();
                                        string resultCodeID = dr["ResultCodeID"].ToString();

                                        if (ShowSysResultCode(resultCode, hideSysResultCodes))
                                        {
                                            bool hideDefautResultCodes = true;
                                            try
                                            {
                                                hideDefautResultCodes = Convert.ToBoolean(ConfigurationManager.AppSettings["HideDefaultResultcodesForScript"]);
                                                hideResultCodesForAgent = ConfigurationManager.AppSettings["SysResultCodes"].Split(',');
                                            }
                                            catch { }

                                            if (!hideDefautResultCodes || ShowSysResultCode(resultCode, hideResultCodesForAgent))
                                            {
                                                strResultcodes.AppendFormat("<option value=\"{0}\">{1}</option>", resultCodeID, resultCode);
                                            }
                                        }
                                    }
                                }

                                ltrlScriptbody.Text = ltrlScriptbody.Text.Replace("<option value=\"-1\" selected>Select Result Code</option>",
                                    strResultcodes.ToString());

                                try
                                {
                                    string resultcode = dsCampaignDtls.Tables[0].Rows[0]["CallResultCode"].ToString();
                                    if (resultcode != "")
                                    {
                                        ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript", "SetResultCode('" + resultcode + "');", true);
                                    }
                                }
                                catch { }
                            }
                        }
                        ltrlScriptbody.Text = DisableReadOnlyScriptFields(ltrlScriptbody.Text);
                    }
                    catch (Exception ex)
                    {
                        PageMessage = ex.Message;
                    }
                }
            }
            else
            {
                if (!Timer2.Enabled)
                {
                    try
                    {
                        UpdateTimesFromTimer();
                    }
                    catch { }
                    Timer2.Enabled = true;
                }
                lbtnDispose.Enabled = false;
                lbtnHangup.Enabled = false;
                lbtnDispose.Text = "<img src=\"../images/dispose_grey.jpg\" alt=\"dispose\" border=\"0\" />";
                lbtnHangup.Text = "<img src=\"../images/hangup_grey.jpg\" alt=\"hangup\" border=\"0\" id=\"imgHangup\"/>";
                lbtnDispose.Attributes.Remove("onClick");
            }

        }

        private bool ShowScript(bool isDial)
        {
            Campaign objcampaign;
            Agent objagent;
            AgentService objAgentService = new AgentService();
            Session["CampaignDtls"] = null;
            if (Session["Campaign"] != null && Session["LoggedAgent"] != null)
            {
                objcampaign = (Campaign)Session["Campaign"];

                DataSet dsCampaigndtls;
                objagent = (Agent)Session["LoggedAgent"];

                try
                {
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objcampaign, "Campaign"));

                    if (isDial)
                    {
                        dsCampaigndtls = objAgentService.GetCampaignDetailsByAgentID(objagent.AgentID, xDocCampaign, true);
                    }
                    else
                    {
                        dsCampaigndtls = objAgentService.GetCampaignDetailsByPhoneNum(txtPhoneNumber.Text.Trim(), xDocCampaign);
                    }

                    if (dsCampaigndtls != null)
                    {
                        DataView dvResultCodes = BindResultCodes(objagent.VerificationAgent);

                        if (dvResultCodes != null)
                        {
                            DataTable dtResultCodes = dvResultCodes.ToTable();

                            StringWriter sw = new StringWriter();
                            using (HtmlTextWriter tw = new HtmlTextWriter(sw))
                            {
                                bool hideDefautResultCodes = Convert.ToBoolean(ConfigurationManager.AppSettings["HideDefaultResultcodesForAgentDisposition"]);

                                foreach (DataRow dr in dtResultCodes.Rows)
                                {
                                    string resultCode = dr["Description"].ToString();
                                    string resultCodeId = dr["ResultCodeID"].ToString();

                                    if (ShowSysResultCode(resultCode, hideSysResultCodes))
                                    {
                                        if (!hideDefautResultCodes || ShowSysResultCode(resultCode, hideResultCodesForAgent))
                                        {
                                            tw.AddAttribute("value", dr["Description"].ToString());
                                            tw.RenderBeginTag(HtmlTextWriterTag.Option);
                                            tw.Write(dr["Description"].ToString());
                                            tw.RenderEndTag();
                                        }
                                    }
                                }
                            }

                            dispositionOptions.Text = sw.ToString();
                            sw.Dispose();
                        }

                        if (dsCampaigndtls.Tables[0].Rows.Count > 0)
                        {
                            pnlScript.Visible = true;
                            pnlManualDial.Visible = false;
                            pnlPause.Visible = false;
                            lbtnPause.Visible = false;
                            lbtnReady.Visible = false;
                            Session["CampaignDtls"] = dsCampaigndtls;
                            objagent.CallUniqueKey = Convert.ToInt64(dsCampaigndtls.Tables[0].Rows[0]["UniqueKey"]);
                            Session["UniqueKey"] = objagent.CallUniqueKey;
                            Session["LoggedAgent"] = objagent;

                            pnlToolbar.Visible = isDial;
                            pnlLookupButtons.Visible = !isDial;

                            ActivityLogger.WriteAgentEntry(objagent, "|MD|Script detected to show.");
                            return true;
                        }
                        else
                        {
                            lbtnPause.Visible = true;
                            lbtnReady.Visible = false;
                            pnlScript.Visible = false;
                            pnlManualDial.Visible = lbtnPause.Visible;
                            pnlPause.Visible = !lbtnPause.Visible;
                            hdnDispose.Value = "";
                            if (!isDial)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('Number not found');", true);
                            }
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateTimesFromTimer()
        {
            if (Session["MDWaitTime"] != null || Session["MDPauseTime"] != null)
            {
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                XmlDocument xDocAgentStat = new XmlDocument();
                try
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    AgentStat objAgentStat = (AgentStat)Session["AgentStat"];
                    if (Session["MDWaitTime"] != null)
                    {
                        TimeSpan ts = DateTime.Now.Subtract((DateTime)Session["MDWaitTime"]);
                        objAgentStat.WaitingTime += (decimal)ts.TotalSeconds;
                        Session["MDWaitTime"] = DateTime.Now;
                    }
                    if (Session["MDPauseTime"] != null)
                    {
                        TimeSpan ts = DateTime.Now.Subtract((DateTime)Session["MDPauseTime"]);
                        objAgentStat.PauseTime += (decimal)ts.TotalSeconds;
                        Session["MDPauseTime"] = DateTime.Now;
                    }
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));

                    objAgentStat = (AgentStat)Serialize.DeserializeObject(
                      objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");

                    if (objAgentStat != null)
                    {
                        Session["AgentStat"] = objAgentStat;
                    }
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
            }

            try
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "key1", "SetFocusToPhoneNumber();", true);
            }
            catch { }
        }
        private void CheckForCallBacks()
        {
            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|CheckForCallBacks Start");
            Agent objAgent = (Agent)Session["LoggedAgent"];

            bool hasCallBack = false;
            Campaign objCampaignCallback = (Campaign)Session["Campaign"];
            XmlDocument xDocCampaign = new XmlDocument();
            DataView dv = new DataView();
            string phonenum = "";
            string scheduledate = "";
            string schedulenotes = "";
            string uniquekey = "";

            try
            {
                if (lbtnPause.Visible || lbtnReady.Visible)
                {
                    dsCallBack.ConnectionString = objCampaignCallback.CampaignDBConnString;
                    dsCallBack.SelectCommand = string.Format("Select UniqueKey,PhoneNum,scheduledate,schedulenotes FROM Campaign WHERE AgentID = {0} AND scheduledate IS NOT NULL ", objAgent.AgentID.ToString());

                    dv = (DataView)dsCallBack.Select(DataSourceSelectArguments.Empty);
                    foreach (DataRowView rowView in dv)
                    {

                        scheduledate = rowView["scheduledate"].ToString();
                        //check scheduledate against current date
                        DateTime eventDt = DateTime.Parse(scheduledate);
                        ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|CheckForCallBacks eventDt : " + eventDt.ToString());
                        if (eventDt <= DateTime.Now)
                        {

                            phonenum = rowView["phonenum"].ToString();
                            txtPhoneNumber.Text = phonenum;
                            scheduledate = rowView["scheduledate"].ToString();
                            schedulenotes = rowView["schedulenotes"].ToString();
                            uniquekey = rowView["uniquekey"].ToString();
                            hdnCallBackKey.Value = uniquekey;
                            hasCallBack = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|CheckCallbacks Error." + ex.Message);
            }
            finally
            {
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|CheckForCallBacks End");
            }
            //if there are any scheduled callbacks then show them
            //pass in the agentid to Sel_CampaignCallBack_By_AgentID and return callbacks
            //If the agent is reset and has a status of pause or ready then ....
            if (hasCallBack)
            {

                ActivityLogger.WriteAgentEntry(objAgent, "Informing agent that he has a scheduled callback.");
                this.MPE_Modal2.Show();

                this.lblCallback.Text = "You have a scheduled callback " + scheduledate + " for " + phonenum;

                if (schedulenotes != "")
                {

                    this.txtCallbackNotes.Text = schedulenotes.ToString();

                }
                else
                {

                    this.txtCallbackNotes.Text = "";
                }

            }

        }

        #endregion

        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            pnlLookupButtons.Visible = false;
            pnlToolbar.Visible = true;
            BindScript(true);
            txtPhoneNumber.Text = "";

            if (hdnPageFrom.Value == "waitingforcall")
            {
                Response.Redirect("../Agent/WaitingforCall.aspx?ts=" + DateTime.Now.Ticks.ToString() + hdnPageFrom.Value);
            }
            else
            {
                Response.Redirect("../Agent/ManualDial.aspx?ts=" + DateTime.Now.Ticks.ToString());

            }
        }
        #endregion

        protected void lbtnLookUp_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|Lookup only has been requested.");
            BindScript(false);
        }

        /*protected void lbtnCancelLookup_Click(object sender, EventArgs e)
        {
            pnlLookupButtons.Visible = false;
            pnlToolbar.Visible = true;
            BindScript(true);
            txtPhoneNumber.Text = "";
        }*/

        protected void btnCallbackDial_Click(object sender, EventArgs e)
        {
            //close dialog and dial number, enable schedule button
            Campaign objCampaignCallback = (Campaign)Session["Campaign"];
            dsCallBack.ConnectionString = objCampaignCallback.CampaignDBConnString;
            string sqlUpdateQuery = string.Format("UPDATE Campaign SET ScheduleDate = NULL WHERE UniqueKey = {0} ", hdnCallBackKey.Value);
            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|btnCallbackDial_Click query to execute: " + sqlUpdateQuery);

            dsCallBack.UpdateCommand = sqlUpdateQuery;
            dsCallBack.Update();
            this.MPE_Modal2.Hide();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "forcedial(false);", true);
        }

        protected void btnCallbackLookup_Click(object sender, EventArgs e)
        {
            Campaign objCampaignCallback = (Campaign)Session["Campaign"];
            dsCallBack.ConnectionString = objCampaignCallback.CampaignDBConnString;
            string sqlUpdateQuery = string.Format("UPDATE Campaign SET ScheduleDate = NULL WHERE UniqueKey = {0} ", hdnCallBackKey.Value);
            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|btnCallbackLookup_Click query to execute: " + sqlUpdateQuery);

            dsCallBack.UpdateCommand = sqlUpdateQuery;
            dsCallBack.Update();
            this.MPE_Modal2.Hide();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "forcedial(true);", true);

        }

        protected void btnCallbackCancel_Click(object sender, EventArgs e)
        {

            try
            {

                Campaign objCampaignCallback = (Campaign)Session["Campaign"];
                dsCallBack.ConnectionString = objCampaignCallback.CampaignDBConnString;
                string sqlUpdateQuery = string.Format("UPDATE Campaign SET ScheduleDate = NULL WHERE UniqueKey = {0} ", hdnCallBackKey.Value);
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|btnCallbackCancel_Click query to execute: " + sqlUpdateQuery);

                dsCallBack.UpdateCommand = sqlUpdateQuery;
                dsCallBack.Update();
                this.MPE_Modal2.Hide();
                Response.Redirect("../Agent/ManualDial.aspx?ts=" + DateTime.Now.Ticks.ToString());
            }
            catch (Exception ex)
            {
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|MD|btnCallbackCancel_Click Error." + ex.Message);
            }

        }
    }
}