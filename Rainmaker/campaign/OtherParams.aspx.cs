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

namespace Rainmaker.Web.campaign
{
    public partial class OtherParams : PageBase
    {
        public bool IsHoldplay = false;
        //public string strHoldPalyFileName = string.Empty;
        #region Events

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ViewState["OtherParameterID"] = "0";
                Campaign objCampaign;
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                    anchHome.InnerText = objCampaign.Description;// Replaced Short description
                    GetOtherParameter(objCampaign);
                    PlayAudioFiles();
                }
            }
        }

        /// <summary>
        /// Save Other Parameter
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            //if (!IsCampaignRunning())
            SaveData();
            //else
            //    PageMessage = "You cannot modify other parameters when campaign is running";
        }

        /// <summary>
        /// Cancels Other Parameter
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/campaign/OtherParams.aspx");

        }

        // /// <summary>
        ///// Play Audio Files
        ///// </summary>
        ///// <param name="sender">sender.</param>
        ///// <param name="e">e.</param>
        //protected void lbtnHoldplay_Click(object sender, EventArgs e)
        //{
        //    IsHoldplay = true;
        //    if (FileUploadholdmsg.HasFile)
        //        strHoldPalyFileName = UploadAndSaveFile("FileUploadholdmsg");
        //    else if (Session["OtherParameterID"].ToString() != "0" && hdnAutoPlayMesssage.Value!="")
        //        strHoldPalyFileName = hdnAutoPlayMesssage.Value;

        //    strHoldPalyFileName =  strHoldPalyFileName;
        //    strHoldPalyFileName = strHoldPalyFileName.Replace("\\", "\\\\");
        //}

        #endregion

        #region Private Methods
        /// <summary>
        /// Play Audio Files
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        private void PlayAudioFiles()
        {
            if (ViewState["OtherParameterID"].ToString() != "0")
            {
                if (hdnHoldMessage.Value != "")
                {
                    hdnPlayHoldMessagePath.Value = hdnHoldMessage.Value;
                    hdnPlayHoldMessagePath.Value = hdnPlayHoldMessagePath.Value.Replace("\\", "\\\\");
                }
                if (hdnAutoPlayMesssage.Value != "")
                {
                    hdnPlayAutoMessagePath.Value = hdnAutoPlayMesssage.Value;
                    hdnPlayAutoMessagePath.Value = hdnPlayAutoMessagePath.Value.Replace("\\", "\\\\");
                }
            }
        }

        /// <summary>
        /// Uploads file and saves to the server and returns the server path
        /// </summary>
        /// <returns>server relative path of uploaded file</returns>
        private string UploadAndSaveFile(string strFileUploadID)
        {
            string strRelFilePath = "";

            try
            {
                string strRelDirectory = GetRelativeDirPath();
                strRelFilePath = strRelDirectory + GetFileName(strFileUploadID);
                if (!System.IO.File.Exists(GetServerPath() + strRelFilePath))
                    System.IO.File.Delete(GetServerPath() + strRelFilePath);
                Request.Files[strFileUploadID].SaveAs(GetServerPath() + strRelFilePath);
                // strRelFilePath = strRelFilePath.Substring(1);
            }
            catch (Exception ex)
            {
                throw ex;
                //strRelFilePath = "";
            }
            return strRelFilePath;
        }

        /// <summary>
        /// Get Upload Files Relative Path on Server
        /// </summary>
        /// <returns></returns>
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
        /// Create Directory For uploading file
        /// </summary>
        /// <param name="strFolderName"></param>
        /// <returns></returns>
        private string CreateDirectory(string strFolderName)
        {
            string strDirPath = GetServerPath() + strFolderName;
            if (!System.IO.Directory.Exists(strDirPath))
                System.IO.Directory.CreateDirectory(strDirPath);
            return strFolderName;
        }

        /// <summary>
        /// Gets server path
        /// </summary>
        /// <returns></returns>
        private string GetServerPath()
        {
            string serverPath = Server.MapPath("");

            serverPath = serverPath.Substring(0, serverPath.LastIndexOf(@"\"));
            return serverPath;
        }

        /// <summary>
        /// Get file name to upload
        /// </summary>
        /// <param name="strFileUploadID"></param>
        /// <returns></returns>
        private string GetFileName(string strFileUploadID)
        {
            Campaign objCampaign;
            string strFileName = "";
            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];

                strFileName = System.IO.Path.GetFileName(Request.Files[strFileUploadID].FileName);
                if (strFileName != "")
                {
                    string[] arrstrFileName = strFileName.Split('.');
                    if (arrstrFileName.Length > 1)
                        return arrstrFileName[0] + "_"+ objCampaign.CampaignID + @"_OP." + arrstrFileName[1];
                    else
                        return strFileName;
                }
                return strFileName;
            }
            return strFileName;
        }

        /// <summary>
        /// Save Other Parameter
        /// </summary>
        private void SaveData()
        {
            int intCallBacks = 0;
            int intCallTransfer = 0;
            string strAutoPlayMessagePath = "";
            string strHoldMessagePath = "";

            Campaign objCampaign = (Campaign)Session["Campaign"];
            OtherParameter objOtherParameter = new OtherParameter();
            try
            {
                if (rbtnDACT.Checked)
                    intCallTransfer = (int)CallBackOptions.DonotAllowCallTransfer;
                if (rbtnAFCT.Checked)
                    intCallTransfer = (int)CallBackOptions.AllowOffsiteCallTransfer;
                if (rbtnAOCT.Checked)
                {
                    intCallTransfer = (int)CallBackOptions.AllowOnSiteCallTransfer;
                    if (FileUploadautoplaymsg.HasFile)
                        strAutoPlayMessagePath = UploadAndSaveFile("FileUploadautoplaymsg");
                    else if (ViewState["OtherParameterID"].ToString() != "0")
                        strAutoPlayMessagePath = hdnAutoPlayMesssage.Value;
                    if (FileUploadholdmsg.HasFile)
                        strHoldMessagePath = UploadAndSaveFile("FileUploadholdmsg");
                    else if (ViewState["OtherParameterID"].ToString() != "0")
                        strHoldMessagePath = hdnHoldMessage.Value;
                }
                if (rbtnDAC.Checked)
                    intCallBacks = (int)CallBackOptions.DonotAllowCallBacks;
                if (rbtnAAC.Checked)
                    intCallBacks = (int)CallBackOptions.AllowAgentCallBacks;
                if (rbtnASC.Checked)
                    intCallBacks = (int)CallBackOptions.AllowStationCallBacks;
                if (rbtnAsys.Checked)
                    intCallBacks = (int)CallBackOptions.AllowSystemCallBacks;

                if (ViewState["OtherParameterID"].ToString() != "0")
                {
                    objOtherParameter.OtherParameterID = (long)ViewState["OtherParameterID"];
                }
                objOtherParameter.CallTransfer = intCallTransfer;
                objOtherParameter.StaticOffsiteNumber = txtPhoneNo.Text;
                objOtherParameter.TransferMessageEnable = chkTAPM.Checked;
                objOtherParameter.TransferMessage = strAutoPlayMessagePath;
                objOtherParameter.HoldMessage = strHoldMessagePath;
                objOtherParameter.AllowManualDial = chkAllow.Checked;
                objOtherParameter.StartingLine = Convert.ToInt16(txtStartingLine.Text == string.Empty ? "-2" : txtStartingLine.Text);
                objOtherParameter.EndingLine = Convert.ToInt16(txtEndingLine.Text == string.Empty ? "-2" : txtEndingLine.Text);
                objOtherParameter.AllowCallBacks = Convert.ToInt16(intCallBacks);
                objOtherParameter.AlertSupervisorOnCallbacks = Convert.ToInt16(
                    txtAlertSupervisor.Text == string.Empty ? "-2" : txtAlertSupervisor.Text);
                objOtherParameter.QueryStatisticsInPercent = chkQSP.Checked;
                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocOtherParameter = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocOtherParameter.LoadXml(Serialize.SerializeObject(objOtherParameter, "OtherParameter"));
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                objOtherParameter = (OtherParameter)Serialize.DeserializeObject(
                    objCampaignService.OtherParameterInsertUpdate(xDocCampaign, xDocOtherParameter), "OtherParameter");
                Response.Redirect("~/campaign/Home.aspx");
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Get Other Parameter.
        /// </summary>
        /// <param name="objCampaign"></param>
        private void GetOtherParameter(Campaign objCampaign)
        {
            if (objCampaign != null)
            {
                string strHoldMsgPath = null;
                string strAutoPlayPath = null;
                CampaignService objCampaignService = new CampaignService();
                OtherParameter objOtherParameter = new OtherParameter();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                objOtherParameter = (OtherParameter)Serialize.DeserializeObject(
                    objCampaignService.GetOtherParameter(xDocCampaign), "OtherParameter");
                if (objOtherParameter.OtherParameterID != 0)
                {
                    ViewState["OtherParameterID"] = objOtherParameter.OtherParameterID;
                    if (objOtherParameter.StaticOffsiteNumber != "")
                    {
                        rbtnAFCT.Checked = true;
                        chkStaticOffSite.Checked = true;
                        txtPhoneNo.Text = objOtherParameter.StaticOffsiteNumber;
                    }
                    //if (objOtherParameter.AutoPlayMessage != "" || objOtherParameter.HoldMessage != "")
                    //    chkTAPM.Checked = true;
                    chkTAPM.Checked = objOtherParameter.TransferMessageEnable;
                    hdnAutoPlayMesssage.Value = objOtherParameter.TransferMessage;
                    if (objOtherParameter.TransferMessage != "")
                    {
                        strAutoPlayPath = objOtherParameter.TransferMessage;
                        if (System.IO.Path.GetFileNameWithoutExtension(strAutoPlayPath).Length > 20)
                            lblAutoPlayMessage.Text = "(" + System.IO.Path.GetFileNameWithoutExtension(
                                strAutoPlayPath).Remove(20) + "..." + System.IO.Path.GetExtension(strAutoPlayPath) + ")";
                        else
                            lblAutoPlayMessage.Text = "(" + System.IO.Path.GetFileName(strAutoPlayPath) + ")";
                        lblAutoPlayMessage.ToolTip = System.IO.Path.GetFileName(strAutoPlayPath);
                    }
                    hdnHoldMessage.Value = objOtherParameter.HoldMessage;
                    if (objOtherParameter.HoldMessage != "")
                    {
                        strHoldMsgPath = objOtherParameter.HoldMessage;
                        if (System.IO.Path.GetFileNameWithoutExtension(strHoldMsgPath).Length > 20)
                            lblHoldMessage.Text  = "(" + System.IO.Path.GetFileNameWithoutExtension(
                                strHoldMsgPath).Remove(20) + "..." + System.IO.Path.GetExtension(strHoldMsgPath) + ")";
                        else
                            lblHoldMessage.Text = "(" + System.IO.Path.GetFileName(strHoldMsgPath) + ")";
                        lblHoldMessage.ToolTip = System.IO.Path.GetFileName(strHoldMsgPath);
                    }
                    chkAllow.Checked = objOtherParameter.AllowManualDial;
                    if (objOtherParameter.StartingLine == -2)
                        txtStartingLine.Text = "";
                    else
                        txtStartingLine.Text = objOtherParameter.StartingLine.ToString();

                    if (objOtherParameter.EndingLine == -2)
                        txtEndingLine.Text = "";
                    else
                        txtEndingLine.Text = objOtherParameter.EndingLine.ToString();

                    if (objOtherParameter.AlertSupervisorOnCallbacks == -2)
                        txtAlertSupervisor.Text = "";
                    else
                        txtAlertSupervisor.Text = objOtherParameter.AlertSupervisorOnCallbacks.ToString();

                    chkQSP.Checked = objOtherParameter.QueryStatisticsInPercent;

                    if (objOtherParameter.CallTransfer == (int)CallBackOptions.DonotAllowCallTransfer)
                        rbtnDACT.Checked = true;
                    if (objOtherParameter.CallTransfer == (int)CallBackOptions.AllowOffsiteCallTransfer)
                        rbtnAFCT.Checked = true;
                    if (objOtherParameter.CallTransfer == (int)CallBackOptions.AllowOnSiteCallTransfer)
                        rbtnAOCT.Checked = true;

                    if (objOtherParameter.AllowCallBacks == (int)CallBackOptions.DonotAllowCallBacks)
                        rbtnDAC.Checked = true;
                    if (objOtherParameter.AllowCallBacks == (int)CallBackOptions.AllowAgentCallBacks)
                        rbtnAAC.Checked = true;
                    if (objOtherParameter.AllowCallBacks == (int)CallBackOptions.AllowStationCallBacks)
                        rbtnASC.Checked = true;
                    if (objOtherParameter.AllowCallBacks == (int)CallBackOptions.AllowSystemCallBacks)
                        rbtnAsys.Checked = true;
                }
            }
        }

        #endregion
    }
}
