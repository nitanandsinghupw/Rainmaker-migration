using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.IO;
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

namespace Rainmaker.Web.campaign
{
    public partial class CreateCampaign : PageBase
    {
        public string RecordingsPath = @"C:\recordings\";
        #region Events

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            //this.chkSevenDigitNums.Attributes.Add("onclick", "CheckDialingDigits();");
            //this.chkTenDigitNums.Attributes.Add("onclick", "CheckDialingDigits();");
            if (!Page.IsPostBack)
            {
                ShowCampaignDetails();
            }            
        }

        /// <summary>
        /// Saves campaign
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnsave_Click(object sender, EventArgs e)
        {
            //if (!IsCampaignRunning())
                SaveData();
            //else
            //    PageMessage = "You cannot modify campaign details when campaign is running";
        }

        //protected void DigitCheck_Click(object sender, EventArgs e)
        //{

        //    //ErrorLogger.Write("Firing check change. Seven:" + this.chkTenDigitNums.Checked + ", Ten:" + this.chkTenDigitNums.Checked);
        //    if(this.chkTenDigitNums.Checked)
        //    {
        //        if (!this.chkSevenDigitNums.Checked)
        //        { 
        //            // Display label that only 10 digit numbers will be dialed
        //            this.trTenDialingMessage.Visible = true;
        //            this.trSevenDialingMessage.Visible = false;
        //            return;
        //        }
        //    }

        //    if(this.chkSevenDigitNums.Checked)
        //    {
        //        if(!this.chkTenDigitNums.Checked)
        //        {
        //            this.trSevenDialingMessage.Visible = true;
        //            this.trTenDialingMessage.Visible = false;
        //            return;
        //        }
        //    }
        //    this.trSevenDialingMessage.Visible = false;
        //    this.trTenDialingMessage.Visible = false;
        //    return;

        //}


        /// <summary>
        /// Cancels the changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            ShowCampaignDetails();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Saves Campaign and navigate to Home Page
        /// </summary>
        private void SaveData()
        {
            Campaign objCampaign = new Campaign();
            bool isNewCampaign = true;

            if (hdnCampaignId.Value != "")
            {
                objCampaign.CampaignID = Convert.ToInt64(hdnCampaignId.Value);
                isNewCampaign = false;
            }

            objCampaign.Description = txtDescription.Text.Trim();
            objCampaign.ShortDescription = txtShortDesc.Text.Trim();
            objCampaign.FundRaiserDataTracking = chkFundRaiser.Checked;
            objCampaign.OnsiteTransfer = chkOnSiteCallTransfer.Checked;
            //objCampaign.EnableAgentTraining = chkTraining.Checked;
            //objCampaign.RecordLevelCallHistory = chkRecordLevel.Checked;
            objCampaign.EnableAgentTraining = true;
            objCampaign.RecordLevelCallHistory = true;
            objCampaign.AllowDuplicatePhones = chkDuplicatePh.Checked;

            //objCampaign.Allow7DigitNums = chkSevenDigitNums.Checked;
            //objCampaign.Allow10DigitNums = chkTenDigitNums.Checked;

            objCampaign.Allow7DigitNums = rlPhoneDigitsAllowed.SelectedValue == "7";
            objCampaign.Allow10DigitNums = rlPhoneDigitsAllowed.SelectedValue == "10";

            if (rdoIgnore.Checked)
                objCampaign.DuplicateRule = "I";
            else
            {
                objCampaign.DuplicateRule = "R";
            }

            objCampaign.OutboundCallerID = txtOutboundCallerID.Text.Trim();
            objCampaign.StatusID = Convert.ToInt64(ConfigurationManager.AppSettings["DefaultStatusID"]);
            objCampaign.IsDeleted = false;

            //objCampaign.CampaignDBConnString = ConfigurationManager.AppSettings["CampaignDBConnString"].ToString();
            CampaignService objCampaignService = new CampaignService();
            XmlDocument xDocCampaign = new XmlDocument();
            try
            {                
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                objCampaignService.Timeout = System.Threading.Timeout.Infinite;
                objCampaign = (Campaign)Serialize.DeserializeObject(objCampaignService.CampaignInsertUpdate(xDocCampaign), "Campaign");

                XmlDocument xDocNewCampaign = new XmlDocument();
                xDocNewCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                if (objCampaign.FundRaiserDataTracking && isNewCampaign)
                {
                    ActivityLogger.WriteAdminEntry(objCampaign, "Creating campaign with fundraiser tracking, adding pledge amount support.");
                    CampaignFields objCampaignFields = new CampaignFields();
                    objCampaignFields.FieldName = "PledgeAmount";
                    objCampaignFields.FieldTypeID = 4;
                    objCampaignFields.DbFieldType = "money";
                    objCampaignFields.IsDefault = true;

                    XmlDocument xDocCampaignFields = new XmlDocument();
                    xDocCampaignFields.LoadXml(Serialize.SerializeObject(objCampaignFields, "CampaignFields"));
                    objCampaignFields = (CampaignFields)Serialize.DeserializeObject(objCampaignService.CampaignFieldsInsertUpdate(xDocNewCampaign, xDocCampaignFields), "CampaignFields");
                }


                //-----------------------------------------------------
                // Making sure the parent directory for recording is there.
                // We then make a directory for the recordings for the
                // campaign.
                //-----------------------------------------------------
                
                string ismultiboxconfig = ConfigurationManager.AppSettings["IsMultiBoxConfig"];
                string strPath = objCampaign.ShortDescription;
                
                if (ismultiboxconfig == "yes" || ismultiboxconfig == "Yes" || ismultiboxconfig == "YES")
                {
                    RecordingsPath = ConfigurationManager.AppSettings["RecordingsPathMulti"];
                    strPath = strPath.Trim();
                    strPath = RecordingsPath + strPath;
                    Directory.CreateDirectory(Server.MapPath(strPath));
                }
                else
                {
                    RecordingsPath = ConfigurationManager.AppSettings["RecordingsPath"];
                    strPath = strPath.Trim();
                    strPath = RecordingsPath + strPath;
                    Directory.CreateDirectory(strPath);
                }


                //new adds default digital record table entry
                DigitalizedRecording objDigitalizedRecording = new DigitalizedRecording();
                
                objDigitalizedRecording.DigitalizedRecordingID = 0; //insert new record
                objDigitalizedRecording.EnableRecording = true;
                objDigitalizedRecording.EnableWithABeep = false;
                objDigitalizedRecording.StartWithABeep = true;
                objDigitalizedRecording.RecordToWave = true;
                objDigitalizedRecording.HighQualityRecording = false;
                objDigitalizedRecording.RecordingFilePath = "C:\\Recordings\\" + objCampaign.ShortDescription.Trim() + "\\";
                objDigitalizedRecording.FileNaming = string.Empty;
                
                XmlDocument xDocDigitalizedRecording = new XmlDocument();
                XmlDocument xDocCamp = new XmlDocument();
                try
                {
                    xDocDigitalizedRecording.LoadXml(Serialize.SerializeObject(
                        objDigitalizedRecording, "DigitalizedRecording"));

                    xDocCamp.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                    objDigitalizedRecording = (DigitalizedRecording)Serialize.DeserializeObject(
                        objCampaignService.DigitalizedRecordingInsertUpdate(xDocCamp,
                        xDocDigitalizedRecording), "DigitalizedRecording");
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
                if (chkUnmannedMode.Checked)
                {
                    
                    DialingParameter objDialingParameter = new DialingParameter();

                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    objDialingParameter = (DialingParameter)Serialize.DeserializeObject(objCampaignService.GetDialingParameter(xDocCampaign), "DialingParameter");
                    objDialingParameter.DialingMode = 6;
                    XmlDocument xDocDialingParameter = new XmlDocument();
                    
                    xDocDialingParameter.LoadXml(Serialize.SerializeObject(objDialingParameter, "DialingParameter"));
                    
                    
                    objDialingParameter = (DialingParameter)Serialize.DeserializeObject(
                        objCampaignService.DialingParameterInsertUpdate(xDocCampaign, xDocDialingParameter), "DialingParameter");
                    
                }

                
                Response.Redirect("~/campaign/Home.aspx?CampaignID=" + objCampaign.CampaignID);
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("CampaignDuplicateEntityException") >= 0)
                    PageMessage = "campaign already exists with short description '" + txtShortDesc.Text + "'";
                else
                    PageMessage = ex.Message;
            }


        }

        /// <summary>
        /// Binds campaign details or Clears Fields
        /// </summary>
        private void ShowCampaignDetails()
        {
            rdoIgnore.Checked = true;
            if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                hdnCampaignId.Value = objCampaign.CampaignID.ToString();
                txtDescription.Text = objCampaign.Description;
                txtShortDesc.Text = objCampaign.ShortDescription;
                lblShortDesc.Text = objCampaign.ShortDescription;

                chkFundRaiser.Checked = objCampaign.FundRaiserDataTracking;

                // 2012-06-07 Dave Pollastrini
                // Cannot change existing campaign fund raising status.
                // Disable checkbox.
                chkFundRaiser.Enabled = false;

                chkOnSiteCallTransfer.Checked = objCampaign.OnsiteTransfer;
                
                CampaignService objCampaignService = new CampaignService();
                DialingParameter objDialingParameter = new DialingParameter();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                objDialingParameter = (DialingParameter)Serialize.DeserializeObject(objCampaignService.GetDialingParameter(xDocCampaign), "DialingParameter");
                if (objDialingParameter.DailingParameterID != 0)
                {
                    if (objDialingParameter.DialingMode == 6)
                    {
                        chkUnmannedMode.Checked = true;
                    }
                }
                chkUnmannedMode.Enabled = false;
                //chkTraining.Checked = objCampaign.EnableAgentTraining;
                //chkRecordLevel.Checked = objCampaign.RecordLevelCallHistory;
                objCampaign.EnableAgentTraining = true;
                objCampaign.RecordLevelCallHistory = true;
                chkDuplicatePh.Checked = objCampaign.AllowDuplicatePhones;

                rdoIgnore.Checked = (objCampaign.DuplicateRule == "I");
                rdoUpdate.Checked = (objCampaign.DuplicateRule != "I");
                txtOutboundCallerID.Text = objCampaign.OutboundCallerID;

                //chkSevenDigitNums.Checked = objCampaign.Allow7DigitNums;
                //chkTenDigitNums.Checked = objCampaign.Allow10DigitNums;

                rlPhoneDigitsAllowed.SelectedValue = objCampaign.Allow7DigitNums ? "7" : "10";

                //txtShortDesc.Enabled = false;
                txtShortDesc.Visible = false;
                lblShortDesc.Visible = true;

                chkDuplicatePh.Enabled = false;
                //chkSevenDigitNums.Enabled = false;
                //chkTenDigitNums.Enabled = false;
                rlPhoneDigitsAllowed.Enabled = false;
            }
            else
            {
                txtDescription.Text = string.Empty;
                txtShortDesc.Text = string.Empty;

                chkFundRaiser.Checked = false;
                // 2012-06-07 Dave Pollastrini
                // Creating a new campaign, so may sure fund raising
                // checkbox is enable.
                chkFundRaiser.Enabled = true;

                chkOnSiteCallTransfer.Checked = false;
                //chkRecordLevel.Checked = false;
                chkDuplicatePh.Checked = false;
                //chkTenDigitNums.Checked = true;
                //chkSevenDigitNums.Checked = false;
                rlPhoneDigitsAllowed.SelectedValue = "10";
                txtOutboundCallerID.Text = string.Empty;               
            }
        }

        

        #endregion

    }
}
