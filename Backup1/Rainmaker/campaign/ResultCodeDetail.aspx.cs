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
using System.Xml;
using Rainmaker.Web.CampaignWS;
using Rainmaker.Common.DomainModel;

namespace Rainmaker.Web.campaign
{
    public partial class ResultCodeDetail : PageBase
    {
        #region Events
        private string[] resultCodes = { "Answering Machine", "Busy" , "Operator Intercept",
                "Dropped","No Answer","Scheduled Callback","Never Call","Transferred to Agent",
                "Transferred to Dialer","Transferred to Verification","Unmanned Live Contact",
                "Inbound Abandoned by Agent","Inbound abandoned by Caller","Error","Failed",
                "Cadence Break","Loop Current Drop","Pbx Detected","No Ringback","Analysis Stopped",
                "No DialTone","FaxTone Detected", "Unmanned Transferred to Answering Machine", "Transferred Offsite" };
        /// <summary>
        /// Load Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                resultCodes = ConfigurationManager.AppSettings["SysResultCodes"].Split(',');
            }
            catch { }
            if (!Page.IsPostBack)
            {
                if (GetQueryString() > 0)
                    GetResultCodeByResultCodeID(GetQueryString());
            }
            if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                lblCampaign.Text = objCampaign.Description;// Replaced Short description
                if (IsCampaignRunning())
                {
                    chkCampRunning.Checked = true;
                }
                else
                {
                    chkCampRunning.Checked = false;
                }
            }
        }

        /// <summary>
        /// Saves result code details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            if (!txtDescription.Enabled || !IsSysResultCode(txtDescription.Text.Trim(), resultCodes))
                SaveData();
            else
                PageMessage = "System result codes cannot be changed at this time.";
        }

        /// <summary>
        /// Cancels the changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            if (GetQueryString() > 0)
                GetResultCodeByResultCodeID(GetQueryString());
            else
                ClearData();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets ResultcodeId from Querystring
        /// </summary>
        /// <returns></returns>
        private long GetQueryString()
        {
            long resultCodeID = 0;
            if (Request.QueryString["ResultCodeID"] != null)
            {
                resultCodeID = Convert.ToInt64(Request.QueryString["ResultCodeID"]);
            }
            return resultCodeID;
        }

        /// <summary>
        /// Saves Data
        /// </summary>
        private void SaveData()
        {

            ResultCode objResultCode = new ResultCode();
            Campaign objCampaign = new Campaign();
            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];
            }
            objResultCode.ResultCodeID = GetQueryString();
            objResultCode.Description = txtDescription.Text.Trim();
            if (!chkNeverCall.Checked && !chkLead.Checked && !chkMasterDNC.Checked)
                objResultCode.RecycleInDays = Convert.ToInt32(txtRecInDays.Text.Trim());
            objResultCode.Presentation = chkPresentation.Checked;
            objResultCode.Redialable = chkRedialable.Checked;
            objResultCode.Lead = chkLead.Checked;
            objResultCode.MasterDNC = chkMasterDNC.Checked;
            objResultCode.NeverCall = _chkNeverCall.Value.Equals("true", StringComparison.InvariantCultureIgnoreCase);
            objResultCode.VerifyOnly = chkVerifyOnly.Checked;
            objResultCode.LiveContact = chkLiveContact.Checked;
            
            CampaignService objCampaignService = new CampaignService();
            XmlDocument xDocResultCode = new XmlDocument();
            XmlDocument xDocCampaign = new XmlDocument();
            try
            {
                xDocResultCode.LoadXml(Serialize.SerializeObject(objResultCode, "ResultCode"));
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                objResultCode = (ResultCode)Serialize.DeserializeObject(objCampaignService.ResultCodeInsertUpdate
                    (xDocCampaign, xDocResultCode), "ResultCode");
                Response.Redirect("ResultCodes.aspx");
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Gets Result code by ResultID
        /// </summary>
        /// <param name="ResultCodeID"></param>
        private void GetResultCodeByResultCodeID(long ResultCodeID)
        {
            ResultCode objResultCode;
            Campaign objCampaign = (Campaign)Session["Campaign"];
            CampaignService objCampService = new CampaignService();
            XmlDocument xDocCampaign = new XmlDocument();
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            try
            {
                objResultCode = (ResultCode)Serialize.DeserializeObject(objCampService.GetResultCodeByResultCodeID(xDocCampaign, ResultCodeID), "ResultCode");

                txtDescription.Text = objResultCode.Description;

                try
                {
                    bool isSysResultCode = false;
                    if (Request.QueryString["RCType"] != null)
                    {
                        isSysResultCode = (Request.QueryString["RCType"].ToString().ToLower() == "src");
                        txtDescription.Enabled = !isSysResultCode;
                    }
                }
                catch { }

                if (!objResultCode.NeverCall)
                    txtRecInDays.Text = objResultCode.RecycleInDays.ToString();
                chkPresentation.Checked = objResultCode.Presentation;
                chkRedialable.Checked = objResultCode.Redialable;
                chkLead.Checked = objResultCode.Lead;
                chkMasterDNC.Checked = objResultCode.MasterDNC;
                chkNeverCall.Checked = objResultCode.NeverCall;
                _chkNeverCall.Value = objResultCode.NeverCall.ToString();

                chkNeverCall.Enabled = !chkMasterDNC.Checked;

                chkVerifyOnly.Checked = objResultCode.VerifyOnly;
                chkLiveContact.Checked = objResultCode.LiveContact;
                if (objResultCode.Description.ToLower() == "scheduled callback" || objResultCode.Description.ToLower() == "scheduled call back")
                {
                    txtRecInDays.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Clears Fields
        /// </summary>
        private void ClearData()
        {
            txtDescription.Text = string.Empty;
            txtRecInDays.Text = string.Empty;
            chkLead.Checked = false;
            chkNeverCall.Checked = false;
            chkPresentation.Checked = false;
            chkMasterDNC.Checked = false;
            chkRedialable.Checked = false;
            chkVerifyOnly.Checked = false;
            chkLiveContact.Checked = false;
        }

        #endregion
    }
}
