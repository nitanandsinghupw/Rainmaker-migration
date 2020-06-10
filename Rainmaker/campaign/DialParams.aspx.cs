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
using System.IO;



namespace Rainmaker.Web.campaign
{
    public partial class DialParams : PageBase
    {
        public bool isMultiBox = false;
        #region Events

        //-------------------------------------------------------------
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        //-------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                
                hdnValidate.Value = "true";
                Session["DailingParameterID"] = "0";
                Campaign objCampaign;
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                    anchHome.InnerText = objCampaign.Description;// Replaced Short description

                    BindScripts(ddlCallScript, objCampaign, "ColdCallScript");
                    BindScripts(ddlVerScript, objCampaign, "VerificationScript");
                    BindScripts(ddlInboundScript, objCampaign, "InboundScript");

                    BindDropDown(1, 500, ddlPhoneCount);
                    BindDropDown(0, 100, ddlDropRate);
                    BindDropDown(1, 100, ddlRingSeconds);
                    BindDropDown(0, 100, ddlAnalyzeDelayFreq);

                    BindDropDown(1, 100, ddlAMCall);
                    BindDropDown(1, 100, ddlPMCall);
                    BindDropDown(1, 100, ddlWCall);
                    BindDropDown(0, 100, ddlDefaultcallLapse);

                    BindDialingTime(0, 59, ddlAMDailingMinutes);
                    BindDialingTime(0, 59, ddlAMDailingSTMinutes);
                    BindDialingTime(0, 59, ddlPMDailingMinutes);
                    BindDialingTime(0, 59, ddlPMDailingSTMinutes);

                    BindDropDown(1, 12, ddlAMDailingHrs);
                    BindDropDown(1, 12, ddlAMDailingSTHrs);
                    BindDropDown(1, 12, ddlPMDailingHrs);
                    BindDropDown(1, 12, ddlPMDailingSTHrs);

                    //BindDialingMode();
                    GetDialingParameter(objCampaign);
                    RefreshFilesForAudioPlayer();
                    EnableDialingModeControlSets();
                    CheckMultiConfig();


                }
                
                //ScriptManager.RegisterStartupScript(this, typeof(Page), this.UniqueID, "launchplayer();", true);
            }
            ScriptManager.RegisterStartupScript(this, typeof(Page), this.UniqueID, "disableRadioButtons();", true);
            ScriptManager.RegisterStartupScript(this, typeof(Page), this.UniqueID, "setDialogCode();", true);
            //ScriptManager.RegisterStartupScript(this, typeof(Page), this.UniqueID, "disableRadioButtons();", true);
        }


        //-------------------------------------------------------------
        /// <summary>
        /// Save Dialing Parameter
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        //-------------------------------------------------------------
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            //if (!IsCampaignRunning())
            SaveData();
            //else
            //    PageMessage = "You cannot modify dial parameters when campaign is running";
        }



        //-------------------------------------------------------------
        /// <summary>
        /// Dial mode has changed.
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        //-------------------------------------------------------------
        protected void ddlDialingMode_Change(object sender, EventArgs e)
        {
            EnableDialingModeControlSets();
        }

        //-------------------------------------------------------------
        /// <summary>
        /// Cancels Dialing Parameter
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        //-------------------------------------------------------------
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/campaign/DialParams.aspx");
        }

        #endregion

        #region Prompt Upload Control Events


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        protected void lbtnUploadMachine_Click(object sender, EventArgs e)
        {
            if (FileUploadMachineToPlay.HasFile)
            {
                string strMachineFile = UploadAndSaveFile("A");
                if (Path.GetFileNameWithoutExtension(strMachineFile).Length > 20)
                    lblMessage.Text = "(" + Path.GetFileNameWithoutExtension(strMachineFile).Remove(20)
                        + "..." + Path.GetExtension(strMachineFile) + ")";
                else
                    lblMessage.Text = "(" + Path.GetFileName(strMachineFile) + ")";
                lblMessage.ToolTip = Path.GetFileName(strMachineFile);
                //lbtnPlayMachine.Visible = true;
                //lbtnStopMachine.Visible = true;
                hdnMachineFile.Value = strMachineFile;

                RefreshFilesForAudioPlayer();
               
                   
            }
            else
            {
                PageMessage = "You must select or enter a file to upload.";
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        protected void lbtnUploadHuman_Click(object sender, EventArgs e)
        {
            if (FileUploadHumanToPlay.HasFile)
            {
                string strHumanFile = UploadAndSaveFile("H");
                if (Path.GetFileNameWithoutExtension(strHumanFile).Length > 20)
                    lblMessageH.Text = "(" + Path.GetFileNameWithoutExtension(strHumanFile).Remove(20)
                        + "..." + Path.GetExtension(strHumanFile) + ")";
                else
                    lblMessageH.Text = "(" + Path.GetFileName(strHumanFile) + ")";
                lblMessage.ToolTip = Path.GetFileName(strHumanFile);
                //lbtnPlayHuman.Visible = true;
                //lbtnStopHuman.Visible = true;
                hdnHumanFile.Value = strHumanFile;
                RefreshFilesForAudioPlayer();
                
            }
            else
            {
                PageMessage = "You must select or enter a file to upload.";
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        protected void lbtnUploadDrop_Click(object sender, EventArgs e)
        {
            if (FileUploadSilentCallToPlay.HasFile)
            {
                string strDropFile = UploadAndSaveFile("S");
                if (Path.GetFileNameWithoutExtension(strDropFile).Length > 20)
                    lblMessageS.Text = "(" + Path.GetFileNameWithoutExtension(strDropFile).Remove(20)
                        + "..." + Path.GetExtension(strDropFile) + ")";
                else
                    lblMessageS.Text = "(" + Path.GetFileName(strDropFile) + ")";
                lblMessage.ToolTip = Path.GetFileName(strDropFile);
                //lbtnPlayDrop.Visible = true;
                //lbtnStopDrop.Visible = true;
                hdnDropFile.Value = strDropFile;


                RefreshFilesForAudioPlayer();
                
            }
            else
            {
                PageMessage = "You must select or enter a file to upload.";
            }
        }
        #endregion

        #region Private Methods

        //-------------------------------------------------------------
        /// <summary>
        /// Check for split box and sets path to play recordings
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        //-------------------------------------------------------------
        private void CheckMultiConfig()
        {
            
                string ismultiboxconfig = ConfigurationManager.AppSettings["IsMultiBoxConfig"];
                if (ismultiboxconfig == "yes" || ismultiboxconfig == "Yes" || ismultiboxconfig == "YES")
                {
                    isMultiBox = true;
                    //if its a multibox config use this path
                    hdnServerPath.Value = Server.MapPath(ConfigurationManager.AppSettings["UploadPromptsPathMulti"]);
                    
                }
                else
                {
                    hdnServerPath.Value = "";
                }


        }
        //-------------------------------------------------------------
        /// <summary>
        /// Play Audio Files
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        //-------------------------------------------------------------
        private void RefreshFilesForAudioPlayer()
        {
            if (hdnMachineFile.Value != "")
            {
                hdnPlayAudioFile.Value = hdnMachineFile.Value;
                //hdnPlayAudioFile.Value = hdnPlayAudioFile.Value.Replace("\\", "\\\\");
            }
            if (hdnHumanFile.Value != "")
            {
                hdnPlayAudioFileH.Value = hdnHumanFile.Value;
                //hdnPlayAudioFileH.Value = hdnPlayAudioFileH.Value.Replace("\\", "\\\\");
            }
            if (hdnDropFile.Value != "")
            {
                hdnPlayAudioFileS.Value = hdnDropFile.Value;
                //hdnPlayAudioFileS.Value = hdnPlayAudioFileS.Value.Replace("\\", "\\\\");
            }
           // ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "disableRadioButtons();", true);
        
        }

        //-------------------------------------------------------------
        /// <summary>
        /// Bind Dialing Modes.
        /// </summary>
        //-------------------------------------------------------------
        private void BindDialingMode()
        {
            if (ddlDialingMode.SelectedValue == Convert.ToInt16(DialingMode.Unmanned).ToString())
            {
                //ListItem itemToRemove = ddlDialingMode.Items.FindByText("Outbound");
                //ddlDialingMode.Items.Remove(itemToRemove);
                //itemToRemove = ddlDialingMode.Items.FindByText("Manual Dial");
                //ddlDialingMode.Items.Remove(itemToRemove);
                //itemToRemove = ddlDialingMode.Items.FindByText("Power Dial");
                //ddlDialingMode.Items.Remove(itemToRemove);
                ddlDialingMode.Items.Add(new ListItem("Unmanned", Convert.ToInt16(DialingMode.Unmanned).ToString()));

            }
            else
            {
                ddlDialingMode.Items.Add(new ListItem("Outbound", Convert.ToInt16(DialingMode.OutboundOnly).ToString()));
                //ddlDialingMode.Items.Add(new ListItem("Inbound/Outbound", Convert.ToInt16(DialingMode.InboundOutbound).ToString()));
                //ddlDialingMode.Items.Add(new ListItem("Inbound Only", Convert.ToInt16(DialingMode.InboundOnly).ToString()));
                ddlDialingMode.Items.Add(new ListItem("Power Dial", Convert.ToInt16(DialingMode.PowerDial).ToString()));
                ddlDialingMode.Items.Add(new ListItem("Manual Dial", Convert.ToInt16(DialingMode.ManualDial).ToString()));
            }
        }


        //-------------------------------------------------------------
        /// <summary>
        /// Get Dialing Parameter.
        /// </summary>
        //-------------------------------------------------------------
        private void GetDialingParameter(Campaign objCampaign)
        {
            if (objCampaign != null)
            {
                //-----------------------------------------------------   
                // Obtaining the dialing parameters from the database.
                //-----------------------------------------------------
                CampaignService objCampaignService = new CampaignService();
                DialingParameter objDialingParameter = new DialingParameter();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                objDialingParameter = (DialingParameter)Serialize.DeserializeObject(objCampaignService.GetDialingParameter(xDocCampaign), "DialingParameter");
                if (objDialingParameter.DailingParameterID != 0)
                {
                    Session["DailingParameterID"] = objDialingParameter.DailingParameterID;
                    ddlPhoneCount.SelectedValue = objDialingParameter.PhoneLineCount.ToString();

                    ViewState["PhoneLineCount"] = objDialingParameter.PhoneLineCount.ToString();

                    ddlDropRate.SelectedValue = objDialingParameter.DropRatePercent.ToString();
                    ddlRingSeconds.SelectedValue = objDialingParameter.RingSeconds.ToString();
                    ddlAnalyzeDelayFreq.SelectedValue = objDialingParameter.MinimumDelayBetweenCalls.ToString();
                    ddlDialingMode.SelectedValue = objDialingParameter.DialingMode.ToString();
                    

                    try
                    {
                        ddlDefaultcallLapse.SelectedValue = objDialingParameter.DefaultCallLapse.ToString();
                    }
                    catch { }

                    chkAnswerMachine.Checked = objDialingParameter.AnsweringMachineDetection;
                    chkHumanMessageEnable.Checked = objDialingParameter.HumanMessageEnable;
                    chkSilentCallMessageEnable.Checked = objDialingParameter.SilentCallMessageEnable;

                    txt7DigPrefix.Text = objDialingParameter.SevenDigitPrefix;
                    txt10DigPrefix.Text = objDialingParameter.TenDigitPrefix;
                    txt7DigSuffix.Text = objDialingParameter.SevenDigitSuffix;
                    txt10DigSuffix.Text = objDialingParameter.TenDigitSuffix;

                    ddlCallScript.SelectedValue = objDialingParameter.ColdCallScriptID.ToString();
                    ddlVerScript.SelectedValue = objDialingParameter.VerificationScriptID.ToString();
                    ddlInboundScript.SelectedValue = objDialingParameter.InboundScriptID.ToString();

                    ddlAMCall.SelectedValue = objDialingParameter.AMCallTimes.ToString();
                    ddlPMCall.SelectedValue = objDialingParameter.PMCallTimes.ToString();
                    ddlWCall.SelectedValue = objDialingParameter.WeekendCallTimes.ToString();

                    // Setting AM Start Time
                    string[] strAMDailingST = objDialingParameter.AMDialingStart.ToString("t").Split(' ');
                    ddlAMDailing.SelectedValue = strAMDailingST[1];
                    strAMDailingST = strAMDailingST[0].Split(':');
                    ddlAMDailingHrs.SelectedValue = strAMDailingST[0];
                    ddlAMDailingMinutes.SelectedValue = strAMDailingST[1];

                    // Setting AM Stop Time.
                    string[] strAMDailingET = objDialingParameter.AMDialingStop.ToString("t").Split(' ');
                    ddlAMDailingST.SelectedValue = strAMDailingET[1];
                    strAMDailingET = strAMDailingET[0].Split(':');
                    ddlAMDailingSTHrs.SelectedValue = strAMDailingET[0];
                    ddlAMDailingSTMinutes.SelectedValue = strAMDailingET[1];

                    // Setting PM Start
                    string[] strPMDailingST = objDialingParameter.PMDialingStart.ToString("t").Split(' ');
                    ddlPMDailing.SelectedValue = strPMDailingST[1];
                    strPMDailingST = strPMDailingST[0].Split(':');
                    ddlPMDailingHrs.SelectedValue = strPMDailingST[0];
                    ddlPMDailingMinutes.SelectedValue = strPMDailingST[1];

                    // Setting PM Stop
                    string[] strPMDailingET = objDialingParameter.PMDialingStop.ToString("t").Split(' ');
                    ddlPMDailingST.SelectedValue = strPMDailingET[1];
                    strPMDailingET = strPMDailingET[0].Split(':');
                    ddlPMDailingSTHrs.SelectedValue = strPMDailingET[0];
                    ddlPMDailingSTMinutes.SelectedValue = strPMDailingET[1];

                    // *** To be fixed, new full path message
                    hdnMachineFile.Value = objDialingParameter.AnsweringMachineMessage;
                    hdnValidate.Value = "false";
                    if (objDialingParameter.AnsweringMachineMessage != "")
                    {
                        string strPath = objDialingParameter.AnsweringMachineMessage;
                        if (Path.GetFileNameWithoutExtension(strPath).Length > 20)
                            lblMessage.Text = "(" + Path.GetFileNameWithoutExtension(strPath).Remove(20)
                                + "..." + Path.GetExtension(strPath) + ")";
                        else
                            lblMessage.Text = "(" + Path.GetFileName(strPath) + ")";
                        lblMessage.ToolTip = Path.GetFileName(strPath);
                        //lbtnPlayMachine.Visible = true;
                        //lbtnStopMachine.Visible = true;
                    }

                    try
                    {
                        hdnHumanFile.Value = objDialingParameter.HumanMessage;
                        if (objDialingParameter.HumanMessage != "")
                        {
                            string strPath = objDialingParameter.HumanMessage;
                            if (Path.GetFileNameWithoutExtension(strPath).Length > 20)
                                lblMessageH.Text = "(" + Path.GetFileNameWithoutExtension(strPath).Remove(20)
                                    + "..." + Path.GetExtension(strPath) + ")";
                            else
                                lblMessageH.Text = "(" + Path.GetFileName(strPath) + ")";
                            lblMessageH.ToolTip = Path.GetFileName(strPath);
                            // lbtnPlayHuman.Visible = true;
                            //lbtnStopHuman.Visible = true;
                        }

                        hdnDropFile.Value = objDialingParameter.SilentCallMessage;
                        if (objDialingParameter.SilentCallMessage != "")
                        {
                            string strPath = objDialingParameter.SilentCallMessage;
                            if (Path.GetFileNameWithoutExtension(strPath).Length > 20)
                                lblMessageS.Text = "(" + Path.GetFileNameWithoutExtension(strPath).Remove(20)
                                    + "..." + Path.GetExtension(strPath) + ")";
                            else
                                lblMessageS.Text = "(" + Path.GetFileName(strPath) + ")";
                            lblMessageS.ToolTip = Path.GetFileName(strPath);
                            //lbtnPlayDrop.Visible = true;
                            //lbtnStopDrop.Visible = true;
                        }
                    }
                    catch { }

                    try
                    {
                        dddInitDials.SelectedValue = objDialingParameter.ChannelsPerAgent.ToString();
                    }
                    catch { }

                    if (ddlDialingMode.SelectedValue == "6")
                    {
                        ddlCallScript.Enabled = false;
                        ddlVerScript.Enabled = false;
                        ddlInboundScript.Enabled = false;
                        cmpCCS.Enabled = false;
                        cmpVS.Enabled = false;
                        cmpIS.Enabled = false;
                    }
                }
                else
                {
                    hdnValidate.Value = "true";
                }
            }
        }


        //-------------------------------------------------------------
        /// <summary>
        /// Save Dialing Parameter
        /// </summary>
        //-------------------------------------------------------------
        private void SaveData()
        {
            string strAMDialingStartTime = "";
            string strAMDialingStopTime = "";
            string strPMDialingStartTime = "";
            string strPMDialingStopTime = "";

            string strMachineFile = "";
            string strHumanFile = "";
            string strDroppedFile = "";


            Campaign objCampaign = (Campaign)Session["Campaign"];
            DialingParameter objDialingParameter = new DialingParameter();
            CampaignService objCampaignService = new CampaignService();
            XmlDocument xDocDialingParameter = new XmlDocument();
            XmlDocument xDocCampaign = new XmlDocument();
            try
            {
                if (Session["DailingParameterID"].ToString() != "0")
                {
                    objDialingParameter.DailingParameterID = Convert.ToInt64(Session["DailingParameterID"]);
                }

                try
                {
                    int PhoneLinesAvailable = 24;
                    try
                    {
                        PhoneLinesAvailable = Convert.ToInt32(ConfigurationManager.AppSettings["PhoneLinesAvailable"]);
                    }
                    catch { }
                    int currentLineCount = Convert.ToInt32(ddlPhoneCount.SelectedValue);
                    if (objDialingParameter.DailingParameterID > 0 && objCampaign.StatusID == (long)CampaignStatus.Run)
                    {
                        int prevLineCount = Convert.ToInt32(ViewState["PhoneLineCount"]);


                        int PhoneLinesUsed = 0;
                        try
                        {
                            PhoneLinesUsed = objCampaignService.GetPhoneLinesInUseCount(objCampaign.CampaignID);
                        }
                        catch { }

                        int linesRequired = PhoneLinesUsed - prevLineCount + currentLineCount;

                        if (linesRequired > PhoneLinesAvailable)
                        {
                            PageMessage = string.Format(@"The Line count({0}) for the campaign cannot exceed total lines({1}) available to system",
                                currentLineCount, PhoneLinesAvailable - (PhoneLinesUsed - prevLineCount));
                            return;
                        }
                    }
                    else
                    {
                        if (currentLineCount > PhoneLinesAvailable)
                        {
                            PageMessage = string.Format(@"The Line count({0}) for the campaign cannot exceed total lines({1}) available to system",
                                currentLineCount, PhoneLinesAvailable);
                            return;
                        }
                    }
                }
                catch { }

                if (chkAnswerMachine.Checked)
                {
                    if (FileUploadMachineToPlay.HasFile)
                        strMachineFile = UploadAndSaveFile("A");
                    else
                        strMachineFile = hdnMachineFile.Value;
                }
                else
                {
                    strMachineFile = "";
                }

                if (chkHumanMessageEnable.Checked)
                {
                    if (FileUploadHumanToPlay.HasFile)
                        strHumanFile = UploadAndSaveFile("H");
                    else
                        strHumanFile = hdnHumanFile.Value;
                }
                else
                {
                    strHumanFile = "";
                }

                if (chkSilentCallMessageEnable.Checked)
                {
                    if (FileUploadSilentCallToPlay.HasFile)
                        strDroppedFile = UploadAndSaveFile("S");
                    else
                        strDroppedFile = hdnDropFile.Value;
                }
                else
                {
                    strDroppedFile = "";
                }

                objDialingParameter.PhoneLineCount = Convert.ToInt32(ddlPhoneCount.SelectedValue);
                objDialingParameter.DropRatePercent = Convert.ToInt32(ddlDropRate.SelectedValue);
                objDialingParameter.RingSeconds = Convert.ToInt32(ddlRingSeconds.SelectedValue);
                objDialingParameter.MinimumDelayBetweenCalls = Convert.ToInt32(ddlAnalyzeDelayFreq.SelectedValue);
                objDialingParameter.DefaultCallLapse = Convert.ToInt32(ddlDefaultcallLapse.SelectedValue);
                objDialingParameter.DialingMode = Convert.ToInt32(ddlDialingMode.SelectedValue);
                objDialingParameter.AnsweringMachineDetection = chkAnswerMachine.Checked;
                //objDialingParameter.MessageRecordingTool = strMessageRecordingPath;

                objDialingParameter.SevenDigitPrefix = txt7DigPrefix.Text;
                objDialingParameter.TenDigitPrefix = txt10DigPrefix.Text;
                objDialingParameter.SevenDigitSuffix = txt7DigSuffix.Text;
                objDialingParameter.TenDigitSuffix = txt10DigSuffix.Text;

                objDialingParameter.ColdCallScriptID = Convert.ToInt64(ddlCallScript.SelectedValue);
                objDialingParameter.VerificationScriptID = Convert.ToInt64(ddlVerScript.SelectedValue);
                objDialingParameter.InboundScriptID = Convert.ToInt64(ddlInboundScript.SelectedValue);
                objDialingParameter.AMCallTimes = Convert.ToInt32(ddlAMCall.SelectedValue);
                objDialingParameter.PMCallTimes = Convert.ToInt32(ddlPMCall.SelectedValue);
                objDialingParameter.WeekendCallTimes = Convert.ToInt32(ddlWCall.SelectedValue);

                strAMDialingStartTime = ddlAMDailingHrs.SelectedValue + ":" + ddlAMDailingMinutes.SelectedValue + " " + ddlAMDailing.SelectedValue;
                strAMDialingStopTime = ddlAMDailingSTHrs.SelectedValue + ":" + ddlAMDailingSTMinutes.SelectedValue + " " + ddlAMDailingST.SelectedValue;
                strPMDialingStartTime = ddlPMDailingHrs.SelectedValue + ":" + ddlPMDailingMinutes.SelectedValue + " " + ddlPMDailing.SelectedValue;
                strPMDialingStopTime = ddlPMDailingSTHrs.SelectedValue + ":" + ddlPMDailingSTMinutes.SelectedValue + " " + ddlPMDailingST.SelectedValue;

                //-----------------------------------------------------
                //  Storing the start and stop times if appropriate.
                //-----------------------------------------------------

                if (objDialingParameter.DialingMode != 6)
                {
                    //-------------------------------------------------
                    // Default values for all other type of
                    // modes.
                    //-------------------------------------------------
                    strAMDialingStartTime = "09:00:00 AM";
                    strAMDialingStopTime = "11:59:00 AM";
                    strPMDialingStartTime = "12:00:00 PM";
                    strPMDialingStopTime = "09:00:00 PM";
                }

                objDialingParameter.AMDialingStart = Convert.ToDateTime(ConfigurationManager.AppSettings["DefaultTime"].ToString() + " " + strAMDialingStartTime);
                objDialingParameter.AMDialingStop = Convert.ToDateTime(ConfigurationManager.AppSettings["DefaultTime"].ToString() + " " + strAMDialingStopTime);
                objDialingParameter.PMDialingStart = Convert.ToDateTime(ConfigurationManager.AppSettings["DefaultTime"].ToString() + " " + strPMDialingStartTime);
                objDialingParameter.PMDialingStop = Convert.ToDateTime(ConfigurationManager.AppSettings["DefaultTime"].ToString() + " " + strPMDialingStopTime);

                objDialingParameter.AnsweringMachineDetection = chkAnswerMachine.Checked;
                objDialingParameter.AnsweringMachineMessage = strMachineFile;

                objDialingParameter.HumanMessageEnable = chkHumanMessageEnable.Checked;
                objDialingParameter.HumanMessage = strHumanFile;

                objDialingParameter.SilentCallMessageEnable = chkSilentCallMessageEnable.Checked;
                objDialingParameter.SilentCallMessage = strDroppedFile;

                try
                {
                    objDialingParameter.ChannelsPerAgent = Convert.ToDecimal(dddInitDials.SelectedValue);
                }
                catch { }

                xDocDialingParameter.LoadXml(Serialize.SerializeObject(objDialingParameter, "DialingParameter"));
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                objDialingParameter = (DialingParameter)Serialize.DeserializeObject(
                    objCampaignService.DialingParameterInsertUpdate(xDocCampaign, xDocDialingParameter), "DialingParameter");
                Response.Redirect("~/campaign/Home.aspx");
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion



        #region Show Hide Control Sets Method

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void EnableDialingModeControlSets()
        {
            try
            {
                switch (ddlDialingMode.SelectedValue)
                {
                    case "1": // Outbound, normal, enable all controls
                        // Left column controls top to bottom
                        ddlPhoneCount.Enabled = true;
                        ddlDropRate.Enabled = true;
                        ddlRingSeconds.Enabled = true;
                        ddlAnalyzeDelayFreq.Enabled = true;
                        chkAnswerMachine.Enabled = true;
                        FileUploadMachineToPlay.Enabled = true;
                        chkHumanMessageEnable.Enabled = true;
                        FileUploadHumanToPlay.Enabled = true;
                        chkSilentCallMessageEnable.Enabled = true;
                        FileUploadSilentCallToPlay.Enabled = true;

                        // Right column controls top to bottom
                        dddInitDials.Enabled = true;
                        ddlDefaultcallLapse.Enabled = true;
                        txt7DigPrefix.Enabled = true;
                        txt7DigSuffix.Enabled = true;
                        txt10DigPrefix.Enabled = true;
                        txt10DigSuffix.Enabled = true;
                        ddlCallScript.Enabled = true;
                        ddlVerScript.Enabled = true;
                        ddlInboundScript.Enabled = false; // No inbound now, always have disabled
                        ddlAMCall.Enabled = true;
                        ddlPMCall.Enabled = true;
                        ddlWCall.Enabled = true;
                        ddlAMDailingHrs.Enabled = true;
                        ddlAMDailingMinutes.Enabled = true;
                        ddlAMDailing.Enabled = true;
                        ddlAMDailingSTHrs.Enabled = true;
                        ddlAMDailingSTMinutes.Enabled = true;
                        ddlAMDailingST.Enabled = true;
                        ddlPMDailingHrs.Enabled = true;
                        ddlPMDailingMinutes.Enabled = true;
                        ddlPMDailing.Enabled = true;
                        ddlPMDailingSTHrs.Enabled = true;
                        ddlPMDailingSTMinutes.Enabled = true;
                        ddlPMDailingST.Enabled = true;

                        // Page validators
                        reqPLCount.Enabled = true; // Phone line count
                        reqDRPercentage.Enabled = true; // Drop rate
                        reqRingSeconds.Enabled = true; // Ring seconds 
                        reqDCL.Enabled = true; // Default call lapse
                        cmpCCS.Enabled = true; // Cold Call scripts
                        cmpVS.Enabled = true; // Verification script
                        cmpIS.Enabled = false; // Inbound script
                        reqWCT.Enabled = false; // weekend call times


                        ShowCallTimes(false);

                        break;

                    case "4": // Power dial control subset
                        // Left column controls top to bottom
                        ddlPhoneCount.Enabled = true;
                        ddlDropRate.Enabled = false;
                        ddlRingSeconds.Enabled = true;
                        ddlAnalyzeDelayFreq.Enabled = false;
                        chkAnswerMachine.Enabled = true;
                        FileUploadMachineToPlay.Enabled = true;
                        chkHumanMessageEnable.Enabled = true;
                        FileUploadHumanToPlay.Enabled = true;
                        chkSilentCallMessageEnable.Enabled = true;
                        FileUploadSilentCallToPlay.Enabled = true;

                        // Right column controls top to bottom
                        dddInitDials.Enabled = false;
                        ddlDefaultcallLapse.Enabled = false;
                        txt7DigPrefix.Enabled = true;
                        txt7DigSuffix.Enabled = true;
                        txt10DigPrefix.Enabled = true;
                        txt10DigSuffix.Enabled = true;
                        ddlCallScript.Enabled = true;
                        ddlVerScript.Enabled = true;
                        ddlInboundScript.Enabled = false; // No inbound now, always have disabled
                        ddlAMCall.Enabled = true;
                        ddlPMCall.Enabled = true;
                        ddlWCall.Enabled = true;
                        ddlAMDailingHrs.Enabled = true;
                        ddlAMDailingMinutes.Enabled = true;
                        ddlAMDailing.Enabled = true;
                        ddlAMDailingSTHrs.Enabled = true;
                        ddlAMDailingSTMinutes.Enabled = true;
                        ddlAMDailingST.Enabled = true;
                        ddlPMDailingHrs.Enabled = true;
                        ddlPMDailingMinutes.Enabled = true;
                        ddlPMDailing.Enabled = true;
                        ddlPMDailingSTHrs.Enabled = true;
                        ddlPMDailingSTMinutes.Enabled = true;
                        ddlPMDailingST.Enabled = true;
                        // Page validators
                        reqPLCount.Enabled = true; // Phone line count
                        reqDRPercentage.Enabled = false; // Drop rate
                        reqRingSeconds.Enabled = true; // Ring seconds 
                        reqDCL.Enabled = false; // Default call lapse
                        cmpCCS.Enabled = true; // Cold Call scripts
                        cmpVS.Enabled = false; // Verification script
                        cmpIS.Enabled = false; // Inbound script
                        reqWCT.Enabled = false; // weekend call times

                        ShowCallTimes(false);

                        break;

                    case "5": // Manual dial only subset of controls
                        // Left column controls top to bottom
                        ddlPhoneCount.Enabled = true;
                        ddlDropRate.Enabled = false;
                        ddlRingSeconds.Enabled = false;
                        ddlAnalyzeDelayFreq.Enabled = false;
                        chkAnswerMachine.Enabled = false;
                        FileUploadMachineToPlay.Enabled = false;
                        chkHumanMessageEnable.Enabled = false;
                        FileUploadHumanToPlay.Enabled = false;
                        chkSilentCallMessageEnable.Enabled = false;
                        FileUploadSilentCallToPlay.Enabled = false;

                        // Right column controls top to bottom
                        dddInitDials.Enabled = false;
                        ddlDefaultcallLapse.Enabled = false;
                        txt7DigPrefix.Enabled = true;
                        txt7DigSuffix.Enabled = true;
                        txt10DigPrefix.Enabled = true;
                        txt10DigSuffix.Enabled = true;
                        ddlCallScript.Enabled = true;
                        ddlVerScript.Enabled = true;
                        ddlInboundScript.Enabled = false; // No inbound now, always have disabled
                        ddlAMCall.Enabled = false;
                        ddlPMCall.Enabled = false;
                        ddlWCall.Enabled = false;
                        ddlAMDailingHrs.Enabled = true;
                        ddlAMDailingMinutes.Enabled = true;
                        ddlAMDailing.Enabled = true;
                        ddlAMDailingSTHrs.Enabled = true;
                        ddlAMDailingSTMinutes.Enabled = true;
                        ddlAMDailingST.Enabled = true;
                        ddlPMDailingHrs.Enabled = true;
                        ddlPMDailingMinutes.Enabled = true;
                        ddlPMDailing.Enabled = true;
                        ddlPMDailingSTHrs.Enabled = true;
                        ddlPMDailingSTMinutes.Enabled = true;
                        ddlPMDailingST.Enabled = true;

                        // Page validators
                        reqPLCount.Enabled = true; // Phone line count
                        reqDRPercentage.Enabled = false; // Drop rate
                        reqRingSeconds.Enabled = false; // Ring seconds 
                        reqDCL.Enabled = false; // Default call lapse
                        cmpCCS.Enabled = true; // Cold Call scripts
                        cmpVS.Enabled = false; // Verification script
                        cmpIS.Enabled = false; // Inbound script
                        reqWCT.Enabled = false; // weekend call times

                        ShowCallTimes(false);

                        break;

                    case "6": // Unmanned mode
                        // Left column controls top to bottom
                        ddlPhoneCount.Enabled = true;
                        ddlDropRate.Enabled = true;
                        ddlRingSeconds.Enabled = true;
                        ddlAnalyzeDelayFreq.Enabled = true;
                        chkAnswerMachine.Enabled = true;
                        FileUploadMachineToPlay.Enabled = true;
                        chkHumanMessageEnable.Enabled = true;
                        FileUploadHumanToPlay.Enabled = true;
                        chkSilentCallMessageEnable.Enabled = true;
                        FileUploadSilentCallToPlay.Enabled = true;

                        // Right column controls top to bottom
                        dddInitDials.Enabled = false;
                        ddlDefaultcallLapse.Enabled = true;
                        txt7DigPrefix.Enabled = true;
                        txt7DigSuffix.Enabled = true;
                        txt10DigPrefix.Enabled = true;
                        txt10DigSuffix.Enabled = true;
                        ddlCallScript.Enabled = false;
                        ddlVerScript.Enabled = false;
                        ddlInboundScript.Enabled = false; // No inbound now, always have disabled
                        ddlAMCall.Enabled = true;
                        ddlPMCall.Enabled = true;
                        ddlWCall.Enabled = true;
                        ddlAMDailingHrs.Enabled = true;
                        ddlAMDailingMinutes.Enabled = true;
                        ddlAMDailing.Enabled = true;
                        ddlAMDailingSTHrs.Enabled = true;
                        ddlAMDailingSTMinutes.Enabled = true;
                        ddlAMDailingST.Enabled = true;
                        ddlPMDailingHrs.Enabled = true;
                        ddlPMDailingMinutes.Enabled = true;
                        ddlPMDailing.Enabled = true;
                        ddlPMDailingSTHrs.Enabled = true;
                        ddlPMDailingSTMinutes.Enabled = true;
                        ddlPMDailingST.Enabled = true;

                        // Page validators
                        reqPLCount.Enabled = true; // Phone line count
                        reqDRPercentage.Enabled = true; // Drop rate
                        reqRingSeconds.Enabled = true; // Ring seconds 
                        reqDCL.Enabled = true; // Default call lapse
                        cmpCCS.Enabled = false; // Cold Call scripts
                        cmpVS.Enabled = false; // Verification script
                        cmpIS.Enabled = false; // Inbound script
                        reqWCT.Enabled = false; // weekend call times

                        ShowCallTimes(true);

                        

                        break;
                }
            }
            catch (Exception ex)
            {
                ActivityLogger.WriteException(ex, "Admin");
            }
            if (ddlDialingMode.SelectedValue == Convert.ToInt16(DialingMode.Unmanned).ToString())
            {
                ListItem itemToRemove = ddlDialingMode.Items.FindByText("Outbound");
                ddlDialingMode.Items.Remove(itemToRemove);
                itemToRemove = ddlDialingMode.Items.FindByText("Manual Dial");
                ddlDialingMode.Items.Remove(itemToRemove);
                itemToRemove = ddlDialingMode.Items.FindByText("Power Dial");
                ddlDialingMode.Items.Remove(itemToRemove);
                //ddlDialingMode.Items.Add(new ListItem("Unmanned", Convert.ToInt16(DialingMode.Unmanned).ToString()));

            }
            else
            {
                ListItem itemToRemove = ddlDialingMode.Items.FindByText("Unmanned");
                ddlDialingMode.Items.Remove(itemToRemove);
                //ddlDialingMode.Items.Add(new ListItem("Outbound", Convert.ToInt16(DialingMode.OutboundOnly).ToString()));
                //ddlDialingMode.Items.Add(new ListItem("Inbound/Outbound", Convert.ToInt16(DialingMode.InboundOutbound).ToString()));
                //ddlDialingMode.Items.Add(new ListItem("Inbound Only", Convert.ToInt16(DialingMode.InboundOnly).ToString()));
                //ddlDialingMode.Items.Add(new ListItem("Power Dial", Convert.ToInt16(DialingMode.PowerDial).ToString()));
                //ddlDialingMode.Items.Add(new ListItem("Manual Dial", Convert.ToInt16(DialingMode.ManualDial).ToString()));
            }

        }

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void ShowCallTimes(bool bVisible)
        {
            ddlAMDailingHrs.Visible = bVisible;
            ddlAMDailingMinutes.Visible = bVisible;
            ddlAMDailing.Visible = bVisible;
            ddlAMDailingSTHrs.Visible = bVisible;
            ddlAMDailingSTMinutes.Visible = bVisible;
            ddlAMDailingST.Visible = bVisible;
            ddlPMDailingHrs.Visible = bVisible;
            ddlPMDailingMinutes.Visible = bVisible;
            ddlPMDailing.Visible = bVisible;
            ddlPMDailingSTHrs.Visible = bVisible;
            ddlPMDailingSTMinutes.Visible = bVisible;
            ddlPMDailingST.Visible = bVisible;

            LBL_AMStartTime.Visible = bVisible;
            LBL_AMStopTime.Visible = bVisible;
            LBL_PMStartTime.Visible = bVisible;
            LBL_PMStopTime.Visible = bVisible;

        }


        #endregion

        #region UploadData

        /// <summary>
        /// Uploads file and saves to the server and returns the server path
        /// </summary>
        /// <returns>server relative path of uploaded file</returns>
        private string UploadAndSaveFile(string type)
        {
          
            string strFilePathAndName = "";
            string realPhysicalPath = "";
            try
            {
                string strUploadDirectory;
                //string strRelDirectory = GetRelativeDirPath(); ***
                string ismultiboxconfig = ConfigurationManager.AppSettings["IsMultiBoxConfig"];
                if (ismultiboxconfig == "yes" || ismultiboxconfig == "Yes" || ismultiboxconfig == "YES")
                {
                   
                    //if its a multibox config use this path
                    strUploadDirectory = ConfigurationManager.AppSettings["UploadPromptsPathMulti"];

                }
                else
                {
                    //if its a single box configuration use this
                    strUploadDirectory = ConfigurationManager.AppSettings["UploadPromptsPath"];
                }
                
                strFilePathAndName = strUploadDirectory + GetFileName(type);
                //strRelFilePath = strRelDirectory + GetFileName(type);
                //if (!System.IO.File.Exists(GetServerPath() + strRelFilePath))
                //    System.IO.File.Delete(GetServerPath() + strRelFilePath);
                if (System.IO.File.Exists(strFilePathAndName)) System.IO.File.Delete(strFilePathAndName);
                
                if (ismultiboxconfig == "yes" || ismultiboxconfig == "Yes" || ismultiboxconfig == "YES")
                {
                    realPhysicalPath = Server.MapPath(strFilePathAndName);
                }
                else
                {
                    realPhysicalPath = strFilePathAndName;
                }
                     //string success = CreateDirectory(Server.MapPath(strUploadDirectory + "yo"));
                if (type == "H")
                {
                    //Request.Files["FileUploadHumanToPlay"].SaveAs(GetServerPath() + strRelFilePath);
                    Request.Files["FileUploadHumanToPlay"].SaveAs(realPhysicalPath);
                }
                else if (type == "S")
                {
                    //Request.Files["FileUploadSilentCallToPlay"].SaveAs(GetServerPath() + strRelFilePath);
                    Request.Files["FileUploadSilentCallToPlay"].SaveAs(realPhysicalPath);
                }
                else
                    //Request.Files["FileUploadMachineToPlay"].SaveAs(GetServerPath() + strRelFilePath);
                    
                     
                    Request.Files["FileUploadMachineToPlay"].SaveAs(realPhysicalPath);
                //strRelFilePath = strRelFilePath.Substring(1);
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
                
                //strRelFilePath = "";
            }
            return realPhysicalPath;
        }

        /// <summary>
        /// Get Upload Files Relative Path on Server
        /// </summary>
        /// <returns>return relative directory</returns>
        private string GetRelativeDirPath()
        {
            string strRelDirectory = "";
            try
            {
                strRelDirectory = CreateDirectory(@"\UploadedFiles\MessageFiles\");
            }
            catch
            {
                strRelDirectory = "";
            }
            return strRelDirectory;
        }

        /// <summary>
        /// Create Directory
        /// </summary>
        /// <returns>return Directory</returns>
        private string CreateDirectory(string strFolderName)
        {
            //DirectorySecurity securityRules = new DirectorySecurity();
            //securityRules.AddAccessRule(new FileSystemAccessRule(@"Dev", FileSystemRights.FullControl, AccessControlType.Allow));

            //string strDirPath = GetServerPath() + strFolderName;
            string strDirPath = strFolderName;
            if (!System.IO.Directory.Exists(strDirPath))
                System.IO.Directory.CreateDirectory(strDirPath);
            return strFolderName;
        }

        /// <summary>
        /// Get Server Path
        /// </summary>
        /// <returns>return ServerPath</returns>
        private string GetServerPath()
        {
            string serverPath = Server.MapPath("");

            serverPath = serverPath.Substring(0, serverPath.LastIndexOf(@"\"));
            return serverPath;
        }

        /// <summary>
        /// Get File Name
        /// </summary>
        /// <returns>return FileName</returns>
        private string GetFileName(string type)
        {
            Campaign objCampaign;
            string strFileName = "";
            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];
                if (type == "H")
                {
                    //strFileName = System.IO.Path.GetFileName(Request.Files["FileUploadHumanToPlay"].FileName);
                    strFileName = System.IO.Path.GetFileName(FileUploadHumanToPlay.FileName);

                }
                else if (type == "S")
                {
                    //strFileName = System.IO.Path.GetFileName(Request.Files["FileUploadSilentCallToPlay"].FileName);
                    strFileName = System.IO.Path.GetFileName(FileUploadSilentCallToPlay.FileName);
                }
                else
                    //strFileName = System.IO.Path.GetFileName(Request.Files["FileUploadMachineToPlay"].FileName);
                    strFileName = System.IO.Path.GetFileName(FileUploadMachineToPlay.FileName);
                //string[] arrstrFileName = strFileName.Split('.');
                //if (arrstrFileName.Length > 1)
                //    return arrstrFileName[0] + "_" + objCampaign.CampaignID + "_" + type + @"_DP." + arrstrFileName[1];
                //else
                return strFileName;
            }
            return strFileName;
        }

        #endregion

 
    }
}
