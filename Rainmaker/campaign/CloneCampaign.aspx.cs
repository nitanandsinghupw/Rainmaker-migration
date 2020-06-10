using System;
using System.Data;
using System.IO;
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


namespace Rainmaker.Web.campaign
{
    public partial class CloneCampaign : PageBase
    {
        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowCampaignDetails();
            }
            if (RdoClone.Checked)
            {
                chkAll.Enabled = false;
                chkData.Enabled = false;
                chkFields.Enabled = false;
                chkOptions.Enabled = false;
                chkResultcodes.Enabled = false;
                chkQueries.Enabled = false;
                chkScripts.Enabled = false;
            }
            else
            {
                chkAll.Enabled = true;
                chkData.Enabled = true;
                chkFields.Enabled = true;
                chkOptions.Enabled = true;
                chkResultcodes.Enabled = true;
                chkQueries.Enabled = true;
                chkScripts.Enabled = true;
            }
        }

        /// <summary>
        /// Saves campaign and Creates new Campaign with data of selected options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnsave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

         protected void RdoDuplicateMode_Changed(object sender, EventArgs e)
        {
            if (RdoClone.Checked)
            {
                chkAll.Enabled = false;
                chkData.Enabled = false;
                chkFields.Enabled = false;
                chkOptions.Enabled = false;
                chkResultcodes.Enabled = false;
                chkQueries.Enabled = false;
                chkScripts.Enabled = false;
            }
            else
            {
                chkAll.Enabled = true;
                chkData.Enabled = true;
                chkFields.Enabled = true;
                chkOptions.Enabled = true;
                chkResultcodes.Enabled = true;
                chkQueries.Enabled = true;
                chkScripts.Enabled = true;
            }
        }

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
            CloneInfo objCloneInfo = new CloneInfo();

            long parentCampaignID = Convert.ToInt64(hdnCampaignId.Value);
            objCampaign.CampaignID = 0;
            objCampaign.Description = txtDescription.Text.Trim();
            objCampaign.ShortDescription = txtShortDesc.Text.Trim();
            //objCampaign.FundRaiserDataTracking = chkFundRaiser.Checked;
            //objCampaign.OnsiteTransfer = chkOnSiteCallTransfer.Checked;
            //objCampaign.EnableAgentTraining = chkTraining.Checked;
            //objCampaign.RecordLevelCallHistory = chkRecordLevel.Checked;
            objCampaign.EnableAgentTraining = true;
            objCampaign.RecordLevelCallHistory = false;
            objCampaign.AllowDuplicatePhones = chkDuplicatePh.Checked;
            objCampaign.Allow7DigitNums = chkSevenDigitNums.Checked;
            objCampaign.Allow10DigitNums = chkTenDigitNums.Checked;

            if (rdoIgnore.Checked)
                objCampaign.DuplicateRule = "I";
            else
            {
                objCampaign.DuplicateRule = "R";
            }

            objCampaign.OutboundCallerID = txtOutboundCallerID.Text.Trim();
            objCampaign.StatusID = Convert.ToInt64(ConfigurationManager.AppSettings["DefaultStatusID"]);
            objCampaign.IsDeleted = false;
            string RecordingsPath = @"C:\recordings\";
            string ismultiboxconfig = ConfigurationManager.AppSettings["IsMultiBoxConfig"];
            string strPath = objCampaign.ShortDescription;
                
                /*if (ismultiboxconfig == "yes" || ismultiboxconfig == "Yes" || ismultiboxconfig == "YES")
                {
                    RecordingsPath = ConfigurationManager.AppSettings["RecordingsPathMulti"];
                    strPath = strPath.Trim();
                    strPath = RecordingsPath + strPath;
                    Directory.CreateDirectory(Server.MapPath(strPath));
                }
                else
                {*/
                    RecordingsPath = ConfigurationManager.AppSettings["RecordingsPath"];
                    strPath = strPath.Trim();
                    strPath = RecordingsPath + strPath;
                    Directory.CreateDirectory(strPath);
                //}

            objCloneInfo.RecordingsPath = strPath.EndsWith(@"\") == true ? strPath.Trim() : strPath.Trim() + @"\";

            
            objCloneInfo.ParentCampaignId = parentCampaignID;
            objCloneInfo.ParentShortDesc = lblCampaign.Text;
            objCloneInfo.IncludeData = chkData.Checked;
            objCloneInfo.IncludeResultCodes = chkResultcodes.Checked;
            objCloneInfo.IncludeOptions = chkOptions.Checked;
            objCloneInfo.IncludeQueries = chkQueries.Checked;
            objCloneInfo.IncludeFields = chkFields.Checked;
            objCloneInfo.IncludeScripts = chkScripts.Checked;            

            if (RdoClone.Checked)
            {
                //-----------------------------------------------------
                // Clone all data is checked.
                //-----------------------------------------------------
                objCloneInfo.IncludeData         = true;
                objCloneInfo.IncludeResultCodes  = true;
                objCloneInfo.IncludeOptions      = true;
                objCloneInfo.IncludeQueries      = true;
                objCloneInfo.IncludeFields       = true;
                objCloneInfo.IncludeScripts      = true;
                objCloneInfo.FullCopy            = true;
                
            }
            else
            {
                objCloneInfo.IncludeData         = chkData.Checked;
                objCloneInfo.IncludeResultCodes  = chkResultcodes.Checked;
                objCloneInfo.IncludeOptions      = chkOptions.Checked;
                objCloneInfo.IncludeQueries      = chkQueries.Checked;
                objCloneInfo.IncludeFields       = chkFields.Checked;
                objCloneInfo.IncludeScripts      = chkScripts.Checked;
                objCloneInfo.FullCopy            = false;
            }

            CampaignService objCampaignService = new CampaignService();
            XmlDocument xDocCampaign = new XmlDocument();
            XmlDocument xDocCloneInfo = new XmlDocument();

            try
            {
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                xDocCloneInfo.LoadXml(Serialize.SerializeObject(objCloneInfo, "CloneInfo"));

                objCampaignService.Timeout = System.Threading.Timeout.Infinite;

                objCampaign = (Campaign)Serialize.DeserializeObject(objCampaignService.CampaignClone(xDocCampaign, xDocCloneInfo), "Campaign");

                Session["Campaign"] = objCampaign;
                Response.Redirect("~/campaign/Home.aspx?CampaignID=" + objCampaign.CampaignID);

            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("CampaignDuplicateEntityException") >= 0)
                {
                    PageMessage = "CloneCampaign.aspx SaveData Calling objCampaignService.CampaignClone threw error: " + ex.Message;
                }
                else
                {
                    PageMessage = ex.Message;
                }

                ActivityLogger.WriteException(ex, "Admin");
            }

        }


        /// <summary>
        /// Binds campaign details
        /// </summary>
        private void ShowCampaignDetails()
        {
            if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                hdnCampaignId.Value = objCampaign.CampaignID.ToString();
                lblParentCampaign.Text = string.Format("Save Campaign '{0}' as:", objCampaign.ShortDescription);
                lblCampaign.Text = objCampaign.Description;
                txtDescription.Text = string.Empty;
                txtShortDesc.Text = string.Empty;
                //chkFundRaiser.Checked = objCampaign.FundRaiserDataTracking;
                //chkOnSiteCallTransfer.Checked = objCampaign.OnsiteTransfer;

                //chkTraining.Checked = objCampaign.EnableAgentTraining;
                //chkRecordLevel.Checked = objCampaign.RecordLevelCallHistory;
                chkDuplicatePh.Checked = objCampaign.AllowDuplicatePhones;
                chkSevenDigitNums.Checked = objCampaign.Allow7DigitNums;
                chkTenDigitNums.Checked = objCampaign.Allow10DigitNums;
                rdoIgnore.Checked = (objCampaign.DuplicateRule == "I");
                rdoUpdate.Checked = (objCampaign.DuplicateRule != "I");
                txtOutboundCallerID.Text = objCampaign.OutboundCallerID;

                if (objCampaign.AllowDuplicatePhones)
                    hdnDup.Value = "true";
            }
            else
            {
                //TODO : Redirect to campaign list and this case does not occur
            }
        }

        #endregion
    }
}
