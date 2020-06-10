using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Threading;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Rainmaker.Common.DomainModel;
using System.Xml;
using Rainmaker.Web.AgentsWS;
using Rainmaker.Web.CampaignWS;
using Rainmaker.Web.common;
using System.Text;
using System.IO;


namespace Rainmaker.Web.agent
{
    public partial class WaitingforCall : PageBase
    {

        private string[] hideSysResultCodes = { 
                "Scheduled Callback","Transferred to Agent",
                "Transferred to Dialer","Transferred to Verification","Unmanned Live Contact",
                "Inbound Abandoned by Agent","Inbound abandoned by Caller","Error","Failed",
                "Cadence Break","Loop Current Drop","Pbx Detected","No Ringback","Analysis Stopped",
                "No DialTone","FaxTone Detected", "Unmanned Transferred to Answering Machine", "Transferred Offsite" };

        private string[] hideResultCodesForAgent = { 
                "Answering Machine","Busy","Operator Intercept","Dropped","No Answer","Scheduled Callback"};

        #region Events
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            lbtnDispose.Attributes.Add("onClick", "$(\"#dispositionDialog\").dialog(\"open\"); return false;");

            try
            {
                lbtnSchedule.Attributes.Add("onClick", "javascript:saveCampaign('Schedule',null);return false;");
            }
            catch {

               
            }

            if (!Page.IsPostBack)
            {
                // reset training page vars
                Session["TrainingPageTimeStamp"] = null;
                Session["TrainingPage"] = null;

                if (Session["LoggedAgent"] == null)
                {
                    Response.Redirect("../Logout.aspx", true);
                }

                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    if (objCampaign.EnableAgentTraining)
                    {
                        LoadTrainingSettings(objCampaign);
                    }

                    DataView dvResultCodes = BindResultCodes(false);
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
                }

                SetCallTransferOption();

                SetAgentstatus();
                GetCampaignFields();

                BindScript();
                try
                {
                    Timer1.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["Interval"]);
                }
                catch
                {
                    Timer1.Interval = 2000;
                }

                try
                {
                    Timer2.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["StatsUpdInterval"]);
                }
                catch
                {
                    Timer2.Interval = 300000; // 5 min - 5 * 60 * 1000
                }
                ScriptManager.RegisterStartupScript(this, typeof(Page), this.UniqueID, "partialpostbacks();", true);
                
            } 
            else
			{ 
                string ScriptID = hndScriptID.Value;
                if (ScriptID != "") 
                {
                    ScriptID = "";
                    hndScriptID.Value = "";
                    BindCampaignFields();
                    string scriptBody = hdnScriptBody.Value;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
                    ltrlScriptbody.Text =  DisableReadOnlyScriptFields(scriptBody);
                    
                }

            }
            /*if (hdnCallBackKey.Value == "")
            {
                CheckForCallBacks();
            }*/
            
            if (hdnDispose.Value == "IsDispose" && !(Timer1.Enabled))
            {
                SetAgentstatus();
                
                BindScript();
                
                hdnDispose.Value = "";
                Response.Redirect("~/Agent/WaitingforCall.aspx?ts=" + DateTime.Now.Ticks.ToString());
            }
            //  set agent availablity after scheduling call
            else if (hdnDispose.Value == "IsScheduled")
            {
                
                SetAgentAvailablility();
                
                SetAgentstatus();
                
                BindScript();
                
                hdnDispose.Value = "";
                Response.Redirect("~/Agent/WaitingforCall.aspx?ts=" + DateTime.Now.Ticks.ToString());
            }

            if (Session["OffsiteNumber"] != null)
            {
                
                string offsiteNumber = Session["OffsiteNumber"].ToString();
                
                Session["OffsiteNumber"] = "";
                if (offsiteNumber.Length > 6)
                {
                   
                    OffsiteTransfer(offsiteNumber);
                    Response.Redirect("Waitingforcall.aspx?ts=" + DateTime.Now.Ticks.ToString());
                }
            }

            // ***** Add some handlers for training?

            try
            {
                if (lbtnPause.Visible && lbtnReady.Visible)
                {
                    lbtnReady.Visible = false;
                }
            }
            catch { }
        }

        private void LoadTrainingSettings(Campaign objCampaign)
        {
            DataSet dsTrainingSchemeList;
            TrainingScheme mergedTrainingScheme = new TrainingScheme();
            CampaignService objCampService = new CampaignService();
            int currentlyActiveSchemes = 0;
            try
            {
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsTrainingSchemeList = objCampService.GetTrainingSchemeList(xDocCampaign);

                // Loop through records, mark active one and update drop down list.
                if (dsTrainingSchemeList.Tables[0].Rows.Count < 1)
                {
                    if (Session["LoggedAgent"] != null)
                        ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Agent interface cannot enable training, no active training schemes exist."); 
                    Session["TrainingScheme"] = null;
                    return;
                }
                DataRow dr;
                for (int i = 0; i < dsTrainingSchemeList.Tables[0].Rows.Count; i++)
                {
                    bool isActive = false;
                    dr = dsTrainingSchemeList.Tables[0].Rows[i];

                    isActive = (dr["IsActive"] == Convert.DBNull) ? false : (bool)dr["IsActive"];
                    if (isActive)
                    {
                        currentlyActiveSchemes++;
                        int sbFrequency = (dr["ScoreboardFrequency"] == Convert.DBNull) ? Convert.ToInt16(0) : Convert.ToInt16(dr["ScoreboardFrequency"]);
                        int sbDisplayTime = (dr["ScoreboardDisplayTime"] == Convert.DBNull) ? Convert.ToInt16(0) : Convert.ToInt16(dr["ScoreboardDisplayTime"]);
                        if (sbFrequency < mergedTrainingScheme.ScoreboardFrequency)
                            mergedTrainingScheme.ScoreboardFrequency = sbFrequency;
                        if (sbDisplayTime < mergedTrainingScheme.ScoreboardDisplayTime)
                            mergedTrainingScheme.ScoreboardDisplayTime = sbDisplayTime;
                    }
                }
                if (currentlyActiveSchemes > 0)
                    Session["TrainingScheme"] = mergedTrainingScheme;
                else
                    Session["TrainingScheme"] = null;
            }
            catch (Exception ex)
            {
                ActivityLogger.WriteException(ex, "Agent");
                throw;
            }
        }

        private void SetCallTransferOption()
        {

            try
            {
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    CampaignService objCampaignService = new CampaignService();
                    OtherParameter objOtherParameter = new OtherParameter();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    objOtherParameter = (OtherParameter)Serialize.DeserializeObject(
                        objCampaignService.GetOtherParameter(xDocCampaign), "OtherParameter");
                    if (objOtherParameter.OtherParameterID != 0)
                    {
                        if (objOtherParameter.CallTransfer == (int)CallBackOptions.AllowOffsiteCallTransfer &&
                            objOtherParameter.StaticOffsiteNumber.Length < 6)
                        {
                            //lbtnTransfer.Attributes.Remove("OnClick");
                            lbtnTransfer.Enabled = false;
                            lbtnTransfer.Visible = false;
                            lbtnOffTransfer.Enabled = true;
                            lbtnOffTransfer.Visible = true;
                            return;
                        }
                        else if (objOtherParameter.CallTransfer == (int)CallBackOptions.AllowOnSiteCallTransfer || objOtherParameter.CallTransfer == (int)CallBackOptions.AllowOffsiteCallTransfer)
                        {
                            lbtnTransfer.Enabled = true;
                            lbtnTransfer.Visible = true;
                            lbtnOffTransfer.Enabled = false;
                            lbtnOffTransfer.Visible = false;
                            return;
                        }
                    }
                }
            }
            catch { }
            trTransfer.Attributes["style"] = "Display:none";
        }

        /// <summary>
        /// It will change agent status to Pause
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnPause_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Pause button has been clicked."); 
            Session["PauseTime"] = DateTime.Now;
            SaveData(false, false, true); //Pause Clicked
        }

        /// <summary>
        /// It will change agent status to Pause
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnReady_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Ready button has been clicked."); 
            SaveData(false, false, false); //Ready Clicked
        }
        protected void OKButton_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect("../Logout.aspx");
        }

        /// <summary>
        /// Agent Quits the app and redirects to Login page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnLogoff_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Logoff button has been clicked."); 
            SaveData(true, false, false); //Log Off Clicked
            Response.Redirect("../Logout.aspx");
        }

        /// <summary>
        /// It will redirects to Login status Page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnclose_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Close button has been clicked."); 
            SaveData(false, true, false); //Close Clicked
            Response.Redirect("~/Agent/LoginStatus.aspx");
        }

        /// <summary>
        /// Refresh the page for every 5 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            BindScript();
        }

        //protected void TrainingTimer_Tick(object sender, EventArgs e)
        //{
        //    NextTrainingPage();
        //}

        /// <summary>
        /// Update stats for every 3 min
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Timer2_Tick(object sender, EventArgs e)
        {
            //ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|Timer2 start ");
            if (hdnactionphone.Value != "")
            {
                //ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|Timer2 hdnactionphone triggered: " + hdnactionphone.Value);
                return;
            }
            //ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|Timer2 hdnactionphone not triggered ");
            UpdateTimesFromTimer();
            try
            {
                // Added GW 09.30.2010 - Pop on idle or paused for agents
                Campaign objCampaign = (Campaign)Session["Campaign"];
                CampaignService objCampService = new CampaignService();
                long lngCampStatusID = objCampService.GetCampaignStatus(objCampaign.CampaignID);
                // Added to pop agents sooner than actual idle, while calls are flushing

                //check for reset

                Agent objAgent = (Agent)Session["LoggedAgent"];

                Agent objAgentWS;

                AgentService objAgentService = new AgentService();
                long agentID = objAgent.AgentID;
                objAgentWS = (Agent)Serialize.DeserializeObject(
                               objAgentService.GetAgentByAgentID(agentID), "Agent");
                //ready, pause only
                if ((lbtnPause.Visible || lbtnReady.Visible) && objAgentWS.IsReset)
                {
                    objAgentService.ToggleAgentReset(objAgent.AgentID, false);
                    ActivityLogger.WriteAgentEntry(objAgent, "Informing agent that he is being logged off.");
                    //PageMessage = "You have been reset by the administrator.";
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
                    SaveData(true, false, false);

                    this.MPE_Modal1.Show();

                }

                if ((lngCampStatusID == (long)CampaignStatus.Idle) && !objAgentWS.IsReset)
                {
                    //Agent objAgent = (Agent)Session["LoggedAgent"];
                    if (!objAgent.IdleInformed)
                    {
                        ActivityLogger.WriteAgentEntry(objAgent, "Informing agent that campaign has gone idle.");
                        PageMessage = "Your current campaign has now gone idle.  You will be automatically taken back to the campaign selection screen.";
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
                        objAgent.IdleInformed = true;
                    }
                    else
                    {
                        SaveData(false, true, false);
                        Response.Redirect("../agent/Campaigns.aspx");
                    }
                }
            
            if (lngCampStatusID == (long)CampaignStatus.Pause)
            {
                //Agent objAgent = (Agent)Session["LoggedAgent"];
                if (!objAgent.PauseInformed)
                {
                    
                    ActivityLogger.WriteAgentEntry(objAgent, "Informing agent that campaign has been paused."); 
                    PageMessage = "Notification: Your current campaign has now paused.  When finished, you should choose another or ask your manager for direction.";
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

        protected void lbtnHangup_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Hangup button has been clicked."); 
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
                        ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Hangup flag has been set for the dialer."); 
                    lbtnHangup.Enabled = false;
                    lbtnHangup.Text = "<img src=\"../images/hangup_grey_red.jpg\" alt=\"hangup\" border=\"0\" id=\"imgHangup\"/>";
                    
                    try
                    {
                        lbtnTransfer.Enabled = false;
                        lbtnTransfer.Text = "<img src=\"../images/transfer_grey.jpg\" alt=\"transfer\" border=\"0\" id=\"imgTransfer\"/>";
                    }
                    catch (Exception ex)
                    {
                        ActivityLogger.WriteException(ex, "lbtnHangup_Click Error: ");
                        throw;
                    }
                }
            }
        }

        protected void lbtnTransfer_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Transfer button has been clicked."); 
            TransferCall();
            Response.Redirect("Waitingforcall.aspx?ts=" + DateTime.Now.Ticks.ToString());
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Save Agent Status and Update Agent Stats
        /// </summary>
        private void SaveData(bool isLogoff, bool isClose, bool isPause)
        {
            if (Session["Campaign"] != null && Session["AgentStat"] != null && Session["LoggedAgent"] != null)
            {
                Agent objAgent;
                AgentStat objAgentStat;
                Campaign objCampaign;

                objCampaign = (Campaign)Session["Campaign"];

                objAgentStat = (AgentStat)Session["AgentStat"];

                objAgent = (Agent)Session["LoggedAgent"];

                if (isLogoff || isClose || isPause)
                {
                    objAgent.AgentStatusID = (long)AgentLoginStatus.Pause;
                    ActivityLogger.WriteAgentEntry(objAgent, "State has been set to 'pause'.");

                    if (Session["WaitTime"] != null)
                    {
                        try
                        {
                            TimeSpan ts = DateTime.Now.Subtract((DateTime)Session["WaitTime"]);
                            objAgentStat.WaitingTime += (decimal)ts.TotalSeconds;
                            Session.Remove("WaitTime");
                        }
                        catch { }
                    }
                    
                }
                else
                {
                    // Ready button clicked -- moving to waiting state
                    objAgent.AgentStatusID = (long)AgentLoginStatus.Ready;
                    Session["WaitTime"] = DateTime.Now;
                    ActivityLogger.WriteAgentEntry(objAgent, "State has been set to 'ready'.");
                }

                if (!isPause && Session["PauseTime"] != null)
                {
                    try
                    {
                        TimeSpan ts = DateTime.Now.Subtract((DateTime)Session["PauseTime"]);
                        objAgentStat.PauseTime += (decimal)ts.TotalSeconds;
                        Session.Remove("PauseTime");
                    }
                    catch { }
                }

                AgentService objAgentService = new AgentService();
                XmlDocument xDocAgent = new XmlDocument();

                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                
                XmlDocument xDocAgentStat = new XmlDocument();
                try
                {
                    xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                    if (!isLogoff)
                    {
                        objAgentStat.StatusID = objAgent.AgentStatusID;
                        xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));

                        objAgent = (Agent)Serialize.DeserializeObject(
                        objAgentService.AgentActivityInsertUpdate(xDocAgent), "Agent");
                        //SetAgentstatus();
                        objAgentStat = (AgentStat)Serialize.DeserializeObject(
                          objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");

                        if (objAgentStat != null)
                        {
                            Session["AgentStat"] = objAgentStat;
                        }
                        ActivityLogger.WriteAgentEntry(objAgent, "Statistics have been updated, refreshing page to wait for another call.");
                        Session["TrainingPageTimeStamp"] = DateTime.Now;
                        if (Page.IsPostBack && (!isClose))
                        {
                            Response.Redirect("~/Agent/WaitingforCall.aspx?ts=" + DateTime.Now.Ticks.ToString());
                        }
                    }
                    else
                    {

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

                        ActivityLogger.WriteAgentEntry(objAgent, "Agent has logged off after data saved.");
                    }
                }
                catch (ThreadAbortException)
                {
                    //do nothing let it clean itself up
                }
                catch (Exception ex)
                {
                    ActivityLogger.WriteException(ex, "Agent");
                    PageMessage = ex.Message;
                }
            }
        }

        /// <summary>
        /// Set Agent Status
        /// </summary>
        private void SetAgentstatus()
        {
            try
            {
                Agent agent;
                
                if (Session["LoggedAgent"] != null)
                {
                    agent = (Agent)Session["LoggedAgent"];
                    if (agent.AgentStatusID == (long)AgentLoginStatus.Pause)
                    {
                        lbtnPause.Visible = false;
                        lbtnReady.Visible = true;
                        pnlWaitingforcall.Visible = false;
                        pnlTrainingPage.Visible = false;

                        pnlPause.Visible = true;
                        if (Session["TrainingPageTimeStamp"] != null && Session["TrainingPage"] != null)
                        {
                            TimeSpan interval = new TimeSpan();
                            interval = DateTime.Now - Convert.ToDateTime(Session["TrainingPageTimeStamp"]);
                            TrainingPage trnPage = (TrainingPage)Session["TrainingPage"];
                            int displayCounter = Convert.ToInt16(Session["TrainingViewCounter"]);
                            Session["TrainingViewCounter"] = displayCounter + interval.TotalMilliseconds;
                        }

                        return; // Remove if we want training on pause
                    }
                    if (agent.AgentStatusID == (long)AgentLoginStatus.Ready)
                    {
                        lbtnReady.Visible = false;
                        lbtnPause.Visible = true;
                        if (!NextTrainingPage())
                            pnlWaitingforcall.Visible = true; // Decision point between training page vs waiting for call
                        pnlPause.Visible = false;
                    }

                    
                    
                }
            }
            catch (Exception ex)
            {
                ActivityLogger.WriteException(ex, "Agent");
                PageMessage = ex.Message;
            }
        }

        private bool NextTrainingPage()
        {
            bool isTraining = false;
            try
            {
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    if (objCampaign.EnableAgentTraining)
                    {
                        if (Session["TrainingScheme"] != null)
                        {
                            pnlWaitingforcall.Visible = false;
                            long previousPage = 0;
                            long pagesSinceScore = 0;
                            Agent objAgent = (Agent)Session["LoggedAgent"];
                            if (Session["PreviousTrainingPageID"] != null)
                            {
                                previousPage = (long)Session["PreviousTrainingPageID"];
                            }
                            if (Session["TrainingPagesBetweenScoreboard"] != null)
                            {
                                pagesSinceScore = (long)Session["TrainingPagesBetweenScoreboard"];
                            }
                            TrainingPage trainingPage = AgentTraining.NextTrainingPage(previousPage, pagesSinceScore, objAgent, objCampaign, (TrainingScheme)Session["TrainingScheme"]);
                            if (trainingPage == null)
                            {
                                return false;
                            }
                            Session["PreviousTrainingPageID"] = trainingPage.PageID;

                            if (trainingPage.PageID > 0)
                                pagesSinceScore++;
                            else
                                pagesSinceScore = 0;

                            Session["TrainingPagesBetweenScoreboard"] = pagesSinceScore;
                            
                            if (trainingPage.Content.Length > 0)
                            {
                                RenderTrainingPage(trainingPage);
                            }
                            pnlTrainingPage.Visible = true;
                            //TrainingTimer.Enabled = false;
                            
                            //TrainingTimer.Interval = (trainingPage.DisplayTime * 1000);
                            Session["TrainingViewCounter"] = 0;
                            Session["TrainingPageTimeStamp"] = DateTime.Now;
                            Session["TrainingPage"] = trainingPage;
                            //TrainingTimer.Enabled = true;
                            if (Session["LoggedAgent"] != null)
                                ActivityLogger.WriteAgentEntry(objAgent, "New training page will display for {0} seconds.", trainingPage.DisplayTime); 
                            Session["PreviousTrainingPageID"] = trainingPage.PageID;
                            // *** Maybe since SB counter
                            isTraining = true;
                        }
                        else
                        {
                            if (Session["LoggedAgent"] != null)
                                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Training is enabled but no valid training scheme exists."); 
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ActivityLogger.WriteException(ex, "Agent");
                PageMessage = ex.Message;
            }
            return isTraining;
        }

        private void RenderTrainingPage(TrainingPage trainingPage)
        {
            if (trainingPage.IsScoreBoard)
            {
                ltrTrainingPageContent.Text = trainingPage.Content;
            }
            else
            {
                ltrTrainingPageContent.Text = Server.UrlDecode(trainingPage.Content);
            }
        }

        /// <summary>
        /// Bind Script 
        /// </summary>
        private void BindScript()
        {
            if (ShowScript())
            {
                //ErrorLogger.Write("Script being displayed for agent step 1.");
 
                //lbtnlogoff.Attributes.Add("onClick", "javascript:alert('Please dispose the call');return false;");
                //lbtnclose.Attributes.Add("onClick", "javascript:alert('Please dispose the call');return false;");

                Timer1.Enabled = false;
                Timer2.Enabled = false;
                //TrainingTimer.Enabled = false;
                lbtnDispose.Enabled = true;
                lbtnDispose.Text = "<img src=\"../images/dispose.jpg\" alt=\"dispose\" border=\"0\" />";
                // Hangup button grey out here

                lbtnlogoff.Visible = false;
                lbtnclose.Visible = false;
                
                
                lbtnHangup.Enabled = true;
                lbtnHangup.Text = "<img src=\"../images/hangup.jpg\" alt=\"hangup\" border=\"0\" id=\"imgHangup\"/>";
                
                lbtnSchedule.Enabled = true;
                lbtnSchedule.Text = "<img src=\"../images/schedule.jpg\" alt=\"schedule\" border=\"0\" />";
                //if (lbtnPause.Visible)
                //    lbtnPause.Enabled = false;
                //else
                //    lbtnReady.Enabled = false;
                DataSet dsCampaignDtls;
                DataSet dsAgentScript;
                Campaign objcampaign;
                AgentService objAgentService = new AgentService();
                bool isVerification = false;
                // Added for agent busy trigger GW 09.29.10
                Agent objAgent;
                CampaignService objCampaignService = new CampaignService();

                if (Session["Campaign"] != null)
                {
                    objcampaign = (Campaign)Session["Campaign"];
                    
                    objAgent = (Agent)Session["LoggedAgent"];
                    try
                    {
                        XmlDocument xDocCampaign = new XmlDocument();
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objcampaign, "Campaign"));

                        XmlDocument xDocAgent = new XmlDocument();
                        objAgent.AgentStatusID = (long)AgentLoginStatus.Busy;
                        xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
                        objCampaignService.UpdateAgentStatus(xDocCampaign, xDocAgent);
                        ActivityLogger.WriteAgentEntry(objAgent, "Status has been updated.");

                        try
                        {
                            dsCampaignDtls = (DataSet)Session["CampaignDtls"];
                            int agentID = Convert.ToInt32(dsCampaignDtls.Tables[0].Rows[0]["AgentID"]);

                            int verificationAgentID = Convert.ToInt32(dsCampaignDtls.Tables[0].Rows[0]["VerificationAgentID"] == Convert.DBNull ? 0 : dsCampaignDtls.Tables[0].Rows[0]["VerificationAgentID"]);
                            
                            if (objAgent.AgentID == verificationAgentID) //Determines that this is verification
                                isVerification = true;

                        }
                        catch { }

                        if (!isVerification)
                        {
                            try
                            {
                                lbtnTransfer.Enabled = true;
                                lbtnTransfer.Text = "<img src=\"../images/transfer.jpg\" alt=\"transfer\" border=\"0\" id=\"imgTransfer\"/>";
                            }
                            catch { }
                        }
                        if (hdnCallBackKey.Value != "")
                        {
                            Campaign objCampaignCallback = (Campaign)Session["Campaign"];
                            dsCallBack.ConnectionString = objCampaignCallback.CampaignDBConnString;
                            string sqlUpdateQuery = string.Format("UPDATE Campaign SET ScheduleDate = NULL WHERE UniqueKey = {0} ", hdnCallBackKey.Value);
                            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|BindScript Callback query to execute: " + sqlUpdateQuery);
                            dsCallBack.UpdateCommand = sqlUpdateQuery;
                            dsCallBack.Update();
                            this.MPE_Modal2.Hide();
                            hdnCallBackKey.Value = "";
                        }
                        // BINGO, this is where the script is prepared for the client
                        dsAgentScript = objAgentService.GetScript2(xDocCampaign, isVerification);
                        if (dsAgentScript.Tables[0].Rows.Count > 0)
                        {
                            if (dsAgentScript.Tables[0].Rows[0]["ScriptHeader"] != null)
                                ltrlScripthdr.Text = Server.UrlDecode(dsAgentScript.Tables[0].Rows[0]["ScriptHeader"].ToString());
                            if (dsAgentScript.Tables[0].Rows[0]["ScriptBody"] != null)
                                ltrlScriptbody.Text = Server.UrlDecode(dsAgentScript.Tables[0].Rows[0]["ScriptBody"].ToString()); // **** Actual script renderer
                               
                        }
                        
                        
                        if (Session["CampaignDtls"] != null)
                        {
                            dsCampaignDtls = (DataSet)Session["CampaignDtls"];
                            if (dsCampaignDtls.Tables[0].Rows.Count > 0)
                            {
                                string jscript = AssignCampaignFieldsDataToTextFields(dsCampaignDtls,ltrlScriptbody);
                                if (jscript != "")
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript", jscript, true);
                                }
                                BindCampaignFields();

                                try
                                {
                                    if(!isVerification)
                                        lbtnTransfer.Attributes.Add("onClick", "return TrnsferCall();");
                                }
                                catch { }


                                StringBuilder strResultcodes = new StringBuilder();
                                strResultcodes.AppendFormat("<OPTION selected value=-1>Select Result Code</OPTION>");

                                // replace quotes in result code combo.  Result codes not populating cleanup method is revisited.
                                ltrlScriptbody.Text = ltrlScriptbody.Text.Replace("<select name=\"cboResultCode\"><option value=\"-1\" selected=\"selected\">Select Result Code</option></select>",
                                    "<select name=cboResultCode><OPTION value=-1 selected>Select Result Code</OPTION></select>");

                                // Another anomylous was FCk is presenting result code combo: 
                                ltrlScriptbody.Text = ltrlScriptbody.Text.Replace("<select name=\"cboResultCode\"><option value=\"-1\">Select Result Code</option></select>",
                                    "<select name=cboResultCode><OPTION value=-1 selected>Select Result Code</OPTION></select>");

                                ltrlScriptbody.Text = ltrlScriptbody.Text.Replace("<SELECT name=cboResultCode>", "<SELECT name=cboResultCode onchange=\"DisposeCall('cboResultCode')\">");
                                ltrlScriptbody.Text = ltrlScriptbody.Text.Replace("<select name=cboResultCode>", "<select name=cboResultCode onchange=\"DisposeCall('cboResultCode')\">");

                                if (ltrlScriptbody.Text.Contains(strResultcodes.ToString()) || ltrlScriptbody.Text.Contains("<OPTION value=-1 selected>Select Result Code</OPTION>"))
                                
                                {
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
                                                    // GW 09.22.10 Fix Result Code Population
                                                    hideResultCodesForAgent = ConfigurationManager.AppSettings["SysResultCodes"].Split(',');
                                                }
                                                catch { }
                                                if (!hideDefautResultCodes || ShowSysResultCode(resultCode, hideResultCodesForAgent))
                                                {
                                                    strResultcodes.AppendFormat("<option value='{0}'>{1}</option>", resultCodeID, resultCode);
                                                }
                                            }
                                        }
                                        // GW 09.22.10 Fix Result Code Population
                                        if (ltrlScriptbody.Text.Contains("<OPTION value=-1 selected>Select Result Code</OPTION>"))
                                        {
                                            ltrlScriptbody.Text = ltrlScriptbody.Text.Replace("<OPTION value=-1 selected>Select Result Code</OPTION>", strResultcodes.ToString());
                                        }
                                        else
                                        {
                                            ltrlScriptbody.Text = ltrlScriptbody.Text.Replace("<OPTION selected value=-1>Select Result Code</OPTION>", strResultcodes.ToString());
                                        }
                                        
                                        //ErrorLogger.Write(string.Format("Script Literal output : {0}", ltrlScriptbody.Text));
                                    }
                                }
                                // Added for read only fields addition
                                ltrlScriptbody.Text = DisableReadOnlyScriptFields(ltrlScriptbody.Text);
                                //ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "jqueryfunctions()", true);
                            }
                        }

                        
                    }
                    catch (Exception ex)
                    {
                        ActivityLogger.WriteException(ex, "Agent");
                        PageMessage = ex.Message;
                    }

                    UpdateWaitTimes(objAgent);
                    if (Session["LoggedAgent"] != null)
                    {
                        if (!isVerification)
                            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Receiving call script screen pop now for call id {0}.", Session["UniqueKey"].ToString());
                        else
                            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Receiving verification script screen pop now for call id {0}.", Session["UniqueKey"].ToString());
                    }
                }
            }
            else
            {
                if (Session["WaitTime"] == null && lbtnPause.Visible)
                {
                    Session["WaitTime"] = DateTime.Now;
                }

                lbtnDispose.Enabled = false;
                lbtnHangup.Enabled = false;
                try
                {
                    lbtnTransfer.Enabled = false;
                    lbtnTransfer.Text = "<img src=\"../images/transfer_grey.jpg\" alt=\"transfer\" border=\"0\" id=\"imgTransfer\"/>";
                }
                catch { }
                lbtnSchedule.Enabled = false;
                lbtnDispose.Text = "<img src=\"../images/dispose_grey.jpg\" alt=\"dispose\" border=\"0\" />";
                lbtnHangup.Text = "<img src=\"../images/hangup_grey.jpg\" alt=\"hangup\" border=\"0\" id=\"imgHangup\"/>";
                lbtnSchedule.Text = "<img src=\"../images/schedule_grey.jpg\" alt=\"schedule\" border=\"0\" />";

                //if (lbtnPause.Visible)
                //    lbtnPause.Enabled = true;
                //else
                //    lbtnReady.Enabled = true;
                lbtnDispose.Attributes.Remove("onClick");
                lbtnSchedule.Attributes.Remove("onClick");

                try
                {
                    lbtnTransfer.Attributes.Remove("onClick");
                }
                catch { }

                lbtnlogoff.Attributes.Remove("onClick");
                lbtnclose.Attributes.Remove("onClick");
                if (lbtnPause.Visible)  // waiting for call
                {
                    lbtnlogoff.Attributes.Add("onClick", "javascript:alert('You need to pause prior to logging off');return false;");
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateWaitTimes(Agent agentObj)
        {
            //ErrorLogger.Write(string.Format("Updating wait times for agent {0} with status {1}.", agentObj.AgentName ,agentObj.AgentStatusID));
            if (Session["WaitTime"] != null)
            {
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                XmlDocument xDocAgentStat = new XmlDocument();
                try
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    AgentStat objAgentStat = (AgentStat)Session["AgentStat"];
                    TimeSpan ts = DateTime.Now.Subtract((DateTime)Session["WaitTime"]);
                    objAgentStat.WaitingTime += (decimal)ts.TotalSeconds;
                    Session.Remove("WaitTime");
                    // Added for agent busy staying
                    objAgentStat.StatusID = agentObj.AgentStatusID;
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
                    ActivityLogger.WriteException(ex, "Agent");
                    PageMessage = ex.Message;
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private void UpdateTimesFromTimer()
        {
            
            if (Session["WaitTime"] != null || Session["PauseTime"] != null)
            {
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                XmlDocument xDocAgentStat = new XmlDocument();
                try
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    AgentStat objAgentStat = (AgentStat)Session["AgentStat"];

                    //Added to log status changes, set to ready because we are here in timer, not in script
                    objAgentStat.StatusID = (long)AgentLoginStatus.Ready; 
                    
                    if (Session["WaitTime"] != null)
                    {
                        TimeSpan ts = DateTime.Now.Subtract((DateTime)Session["WaitTime"]);
                        objAgentStat.WaitingTime += (decimal)ts.TotalSeconds;
                        Session["WaitTime"] = DateTime.Now;
                    }
                    if (Session["PauseTime"] != null)
                    {
                        TimeSpan ts = DateTime.Now.Subtract((DateTime)Session["PauseTime"]);
                        objAgentStat.PauseTime += (decimal)ts.TotalSeconds;
                        Session["PauseTime"] = DateTime.Now;
                    }
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));

                    objAgentStat = (AgentStat)Serialize.DeserializeObject(
                      objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");

                    if (objAgentStat != null)
                    {
                        Session["AgentStat"] = objAgentStat;
                        if (Session["LoggedAgent"] != null)
                            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Time stats have been updated to Pause:{0}, Wait:{1}", objAgentStat.PauseTime, objAgentStat.WaitingTime);
                    }
                }
                catch (Exception ex)
                {
                    ActivityLogger.WriteException(ex, "Agent");
                    PageMessage = ex.Message;
                }
            }
        }

        /// <summary>
        /// Show Script
        /// </summary>
        private bool ShowScript()
        {
            Campaign objcampaign;
            Agent objAgent;
            AgentService objAgentService = new AgentService();

            if (Session["Campaign"] != null && Session["LoggedAgent"] != null)
            {
                objcampaign = (Campaign)Session["Campaign"];

                DataSet dsCampaigndtls;
                objAgent = (Agent)Session["LoggedAgent"];

                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objcampaign, "Campaign"));

                dsCampaigndtls = objAgentService.GetCampaignDetailsByAgentID(objAgent.AgentID, xDocCampaign, false); // Query looks for script to pop

                if (dsCampaigndtls != null)
                {
                    if (dsCampaigndtls.Tables[0].Rows.Count > 0)
                    {
                        pnlScript.Visible = true;
                        pnlWaitingforcall.Visible = false;
                        pnlTrainingPage.Visible = false;
                        if (Session["TrainingPageTimeStamp"] != null && Session["TrainingPage"] != null)
                        {
                            TimeSpan interval = DateTime.Now - Convert.ToDateTime(Session["TrainingPageTimeStamp"]);
                            TrainingPage trnPage = (TrainingPage)Session["TrainingPage"];
                            int displayCounter = Convert.ToInt16(Session["TrainingViewCounter"]);
                            Session["TrainingViewCounter"] = displayCounter + interval.TotalMilliseconds;
                        }
                        pnlPause.Visible = false;
                        lbtnPause.Visible = false;
                        lbtnReady.Visible = false;
                        Session["CampaignDtls"] = dsCampaigndtls;
                        objAgent.CallUniqueKey = Convert.ToInt64(dsCampaigndtls.Tables[0].Rows[0]["UniqueKey"]);
                        Session["UniqueKey"] = objAgent.CallUniqueKey;
                        Session["LoggedAgent"] = objAgent;

                        try
                        {
                            if (objAgent.AgentStatusID != (long)AgentLoginStatus.Pause)
                                objAgent.AgentStatusID = (long)AgentLoginStatus.Busy;
                            XmlDocument xDocAgent = new XmlDocument();
                            xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
                            objAgent = (Agent)Serialize.DeserializeObject(
                                    objAgentService.AgentActivityInsertUpdate(xDocAgent), "Agent");
                        }
                        catch { }
                        //ErrorLogger.Write(string.Format("Script to pop for agent {0}, verification for call is {1}", objAgent.AgentID, dsCampaigndtls.Tables[0].Rows[0]["VerificationAgentID"])); 
                        return true;
                    }
                    else
                    {
                        pnlScript.Visible = false;
                        if (!pnlTrainingPage.Visible)
                            pnlWaitingforcall.Visible = lbtnPause.Visible;
                        pnlPause.Visible = !lbtnPause.Visible;
                       
                        objAgent.AgentStatusID = (long)AgentLoginStatus.Ready;
                        if(!Timer1.Enabled)
                            Timer1.Enabled = true;
                        if (!Timer2.Enabled)
                        {
                            try
                            {
                                UpdateTimesFromTimer();
                            }
                            catch { }
                            Timer2.Enabled = true;
                        }
                        try
                        {
                            if (Session["TrainingPageTimeStamp"] != null && Session["TrainingPage"] != null)
                            {
                                TimeSpan interval = new TimeSpan();
                                interval = DateTime.Now - Convert.ToDateTime(Session["TrainingPageTimeStamp"]);
                                TrainingPage trnPage = (TrainingPage)Session["TrainingPage"];
                                int displayCounter = Convert.ToInt32(Session["TrainingViewCounter"]);
                                Session["TrainingViewCounter"] = displayCounter + interval.TotalMilliseconds;
                                //TrainingTimer.Enabled = false;
                                if (displayCounter >= (trnPage.DisplayTime * 1000))
                                {
                                    NextTrainingPage();
                                }
                                else
                                {
                                    //TrainingTimer.Interval = (trnPage.DisplayTime * 1000) - displayCounter;
                                    //if (Session["LoggedAgent"] != null)
                                    //    ActivityLogger.WriteAgentEntry(objAgent, "Training page will display for an additional {0} milliseconds.", (trnPage.DisplayTime * 1000) - displayCounter);
                                    //TrainingTimer.Enabled = true;
                                    Session["TrainingPageTimeStamp"] = DateTime.Now;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ActivityLogger.WriteException(ex, "Agent");
                        }
                        hdnDispose.Value = "";
                        return false;
                    }
                }
            }
            return false;
        }

        private void SetAgentAvailablility()
        {
            try
            {
                Agent objAgent;
                AgentService objAgentService = new AgentService();
                objAgent = (Agent)Session["LoggedAgent"];
                objAgent.AgentStatusID = (long)AgentLoginStatus.Ready;
                XmlDocument xDocAgent = new XmlDocument();
                xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
                objAgent = (Agent)Serialize.DeserializeObject(
                        objAgentService.AgentActivityInsertUpdate(xDocAgent), "Agent");
            }
            catch { }
        }


        private void TransferCall()
        {
            try
            {
                if (Session["UniqueKey"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    CampaignService objCampaignService = new CampaignService();
                    OtherParameter objOtherParameter = new OtherParameter();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    objOtherParameter = (OtherParameter)Serialize.DeserializeObject(
                        objCampaignService.GetOtherParameter(xDocCampaign), "OtherParameter");
                    long uniqueKey = Convert.ToInt64(Session["UniqueKey"]);
                    AgentService objAgentService = new AgentService();

                    Agent objAgent = (Agent)Session["LoggedAgent"];
                    XmlDocument xDocAgent = new XmlDocument();
                    objAgent.AgentStatusID = (long)AgentLoginStatus.Ready;
                    xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
                    ActivityLogger.WriteAgentEntry(objAgent, "Executing transfer on call id {0}.", uniqueKey);
                    objAgentService.AddCampaignTransferCall(objCampaign.CampaignDBConnString, uniqueKey, xDocAgent, "");
                }
            }
            catch {}
        }

        private void OffsiteTransfer(string offsiteNumber)
        {
            try
            {
                if (Session["UniqueKey"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    CampaignService objCampaignService = new CampaignService();
                    OtherParameter objOtherParameter = new OtherParameter();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    objOtherParameter = (OtherParameter)Serialize.DeserializeObject(
                        objCampaignService.GetOtherParameter(xDocCampaign), "OtherParameter");
                    long uniqueKey = Convert.ToInt64(Session["UniqueKey"]);
                    AgentService objAgentService = new AgentService();

                    Agent objAgent = (Agent)Session["LoggedAgent"];
                    XmlDocument xDocAgent = new XmlDocument();
                    objAgent.AgentStatusID = (long)AgentLoginStatus.Ready;
                    xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));

                    ActivityLogger.WriteAgentEntry(objAgent, "Offsite transfer being submitted on call id {0} to {1}.", uniqueKey, offsiteNumber);

                    objAgentService.AddCampaignTransferCall(objCampaign.CampaignDBConnString, uniqueKey, xDocAgent, offsiteNumber);
                }
            }
            catch { }
        }
        
        private void CheckForCallBacks()
        {
            
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
                if (lbtnPause.Visible)
                {
                    
                    dsCallBack.ConnectionString = objCampaignCallback.CampaignDBConnString;
                    
                    dsCallBack.SelectCommand = string.Format("Select UniqueKey,PhoneNum,scheduledate,schedulenotes FROM Campaign WHERE AgentID = {0} AND scheduledate IS NOT NULL ", objAgent.AgentID.ToString());

                    dv = (DataView)dsCallBack.Select(DataSourceSelectArguments.Empty);
                    foreach (DataRowView rowView in dv)
                    {

                        scheduledate = rowView["scheduledate"].ToString();
                        //check scheduledate against current date
                        DateTime eventDt = DateTime.Parse(scheduledate);
                        
                        if (eventDt <= DateTime.Now)
                        {

                            phonenum = rowView["phonenum"].ToString();
                               hdnactionphone.Value = phonenum;
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
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|CheckCallbacks Error." + ex.Message);
            }
            finally
            {
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|CheckForCallBacks End");
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
                Timer1.Enabled = false;
                Timer2.Enabled = false;

            }

        }
        #endregion
        protected void lbtnLookUp_Click(object sender, EventArgs e)
        {
            if (Session["LoggedAgent"] != null)
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|Lookup only has been requested.");
            //Call manual dial with a lookup
            //BindScript(false);
            Response.Redirect("../Agent/ManualDial.aspx?ts=" + DateTime.Now.Ticks.ToString() + "&action=forcediallookup&pagefrom=waitingforcall&actionnumber=" + hdnactionphone.Value + "&callbackkey=" + hdnCallBackKey);
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
            this.MPE_Modal2.Hide();
            Campaign objCampaignCallback = (Campaign)Session["Campaign"];
            dsCallBack.ConnectionString = objCampaignCallback.CampaignDBConnString;
            string sqlUpdateQuery = string.Format("UPDATE Campaign SET ScheduleDate = NULL WHERE UniqueKey = {0} ", hdnCallBackKey.Value);
            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|btnCallbackDial_Click query to execute: " + sqlUpdateQuery);

            dsCallBack.UpdateCommand = sqlUpdateQuery;
            dsCallBack.Update();
            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|btnCallbackDial_Click query executed. ");

            //clear callback before dialing

            Response.Redirect("../Agent/ManualDial.aspx?ts=" + DateTime.Now.Ticks.ToString() + "&action=forcedial&pagefrom=waitingforcall&actionnumber=" + hdnactionphone.Value + "&callbackkey=" + hdnCallBackKey.Value);


        }

        protected void btnCallbackLookup_Click(object sender, EventArgs e)
        {

            this.MPE_Modal2.Hide();
            Campaign objCampaignCallback = (Campaign)Session["Campaign"];
            dsCallBack.ConnectionString = objCampaignCallback.CampaignDBConnString;
            string sqlUpdateQuery = string.Format("UPDATE Campaign SET ScheduleDate = NULL WHERE UniqueKey = {0} ", hdnCallBackKey.Value);
            
            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|btnCallbackLookup_Click query to execute: " + sqlUpdateQuery);

            dsCallBack.UpdateCommand = sqlUpdateQuery;
            dsCallBack.Update();

            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|btnCallbackLookup_Click query executed. ");

            //clear callback before doing lookup

            Response.Redirect("../Agent/ManualDial.aspx?ts=" + DateTime.Now.Ticks.ToString() + "&action=forcediallookup&pagefrom=waitingforcall&actionnumber=" + hdnactionphone.Value + "&callbackkey=" + hdnCallBackKey.Value);

        }

        protected void btnCallbackCancel_Click(object sender, EventArgs e)
        {

            try
            {

                Campaign objCampaignCallback = (Campaign)Session["Campaign"];
                dsCallBack.ConnectionString = objCampaignCallback.CampaignDBConnString;
                string sqlUpdateQuery = string.Format("UPDATE Campaign SET ScheduleDate = NULL WHERE UniqueKey = {0} ", hdnCallBackKey.Value);
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|btnCallbackCancel_Click query to execute: " + sqlUpdateQuery);

                dsCallBack.UpdateCommand = sqlUpdateQuery;
                dsCallBack.Update();
                this.MPE_Modal2.Hide();
                Response.Redirect("~/agent/waitingforCall.aspx?ts=" + DateTime.Now.Ticks.ToString());
                
            }
            catch (Exception ex)
            {
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "|WC|btnCallbackCancel_Click Error." + ex.Message);
            }

        }

    }
}
