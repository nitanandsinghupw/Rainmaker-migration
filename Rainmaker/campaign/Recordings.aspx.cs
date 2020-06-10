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
    public partial class Recordings : PageBase
    {
        public string RecordingsPath = @"C:\recordings\";

        #region Events

        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["DigitalizedRecordingID"] = "0";
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    //-------------------------------------------------
                   // Displaying the default path for the recordings.
                   //-------------------------------------------------
                    string strPath = objCampaign.ShortDescription;
                    strPath.Trim();
                    strPath = Global.strRecordings + strPath + "\\";
                    txtFileStoragePath.Text = strPath;
                    lblCampaign.Text = objCampaign.Description;// Replaced Short description
                    GetRecordings(objCampaign);
                }
            }
            try
            {

                RecordingsPath = ConfigurationManager.AppSettings["RecordingsPath"];

            }
            catch { }
        }

        /// <summary>
        /// Saves recording details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            if (!IsCampaignRunning())
                SaveData();
            else
                PageMessage = "You cannot add/modify Digitized Recording when campaign is running";
        }

        /// <summary>
        /// Cancel the changes made
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /*protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                GetRecordings(objCampaign);
            }
            else
            {
                SetDefaultData();
            }
        }*/

        #endregion

        #region Private Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objCampaign"></param>
        private void GetRecordings(Campaign objCampaign)
        {
            if (objCampaign != null)
            {
                CampaignService objCampaignService = new CampaignService();
                DigitalizedRecording objDigitalizedRecording = new DigitalizedRecording();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                objDigitalizedRecording = (DigitalizedRecording)Serialize.DeserializeObject(objCampaignService.GetDigitalizedRecording(xDocCampaign), "DigitalizedRecording");
                if (objDigitalizedRecording.DigitalizedRecordingID != 0)
                {
                    ViewState["DigitalizedRecordingID"] = objDigitalizedRecording.DigitalizedRecordingID;
                    chkEnableDigitizedRecording.Checked = objDigitalizedRecording.EnableRecording;
                    //chkStartRecordingWithBeep.Checked = objDigitalizedRecording.StartWithABeep;
                    //chkWaveFormat.Checked = objDigitalizedRecording.RecordToWave;
                    //chkHigherQuality.Checked = objDigitalizedRecording.HighQualityRecording;
                    txtFileStoragePath.Text = objDigitalizedRecording.RecordingFilePath;
                }
            }
        }

        /// <summary>
        /// Saves recording Data
        /// </summary>
        private void SaveData()
        {
            if (chkEnableDigitizedRecording.Checked && !IsPathValid())
            {
                PageMessage = "Please provide valid Recording File's Storage Path";
                txtFileStoragePath.Focus();
            }
            else
            {
                DigitalizedRecording objDigitalizedRecording = new DigitalizedRecording();
                Campaign objCampaign = new Campaign();
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                }
                if (ViewState["DigitalizedRecordingID"].ToString() != "0")
                {
                    objDigitalizedRecording.DigitalizedRecordingID = (long)ViewState["DigitalizedRecordingID"];
                }

                objDigitalizedRecording.EnableRecording = chkEnableDigitizedRecording.Checked;
                objDigitalizedRecording.EnableWithABeep = false;
                //objDigitalizedRecording.StartWithABeep = chkStartRecordingWithBeep.Checked;
                //objDigitalizedRecording.RecordToWave = chkWaveFormat.Checked;
                //objDigitalizedRecording.HighQualityRecording = chkHigherQuality.Checked;
                objDigitalizedRecording.RecordingFilePath = txtFileStoragePath.Text.EndsWith(@"\") == true ?
                    txtFileStoragePath.Text.Trim() : txtFileStoragePath.Text.Trim() + @"\";

                /*string RecordingsPath;
                string ismultiboxconfig = ConfigurationManager.AppSettings["IsMultiBoxConfig"];
                if (ismultiboxconfig == "yes" || ismultiboxconfig == "Yes" || ismultiboxconfig == "YES")
                {
                    RecordingsPath = ConfigurationManager.AppSettings["RecordingsPathMulti"];
                
                }
                else
                {
                    RecordingsPath = ConfigurationManager.AppSettings["RecordingsPath"];
                
                }
                
                objDigitalizedRecording.RecordingFilePath = RecordingsPath;
                */
                objDigitalizedRecording.FileNaming = string.Empty;
                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocDigitalizedRecording = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();
                try
                {
                    xDocDigitalizedRecording.LoadXml(Serialize.SerializeObject(
                        objDigitalizedRecording, "DigitalizedRecording"));

                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                    objDigitalizedRecording = (DigitalizedRecording)Serialize.DeserializeObject(
                        objCampaignService.DigitalizedRecordingInsertUpdate(xDocCampaign,
                        xDocDigitalizedRecording), "DigitalizedRecording");
                    Response.Redirect("Home.aspx");
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
            }
        }

        /// <summary>
        /// Returns true if Recordings Storage Path is valid
        /// </summary>
        /// <returns></returns>
        private bool IsPathValid()
        {
            // Need to check for absolute path
            string strPath = txtFileStoragePath.Text.Trim();
            if (strPath.Length < 2)
                return false;
            char ch1 = strPath[0];
            char ch2 = strPath[1];
            if (strPath.IndexOf(":") == -1)
            {
                // drive not exists, so check for network path
                bool isNetworkPath = (strPath[0] == '\\' && strPath[1] == '\\');
                if (!isNetworkPath)
                    return false;
            }
            try
            {
                //System.IO.Path.
                if (!System.IO.Directory.Exists(strPath))
                {
                    System.IO.Directory.CreateDirectory(strPath);
                }
                txtFileStoragePath.Text = strPath;
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Sets default values
        /// </summary>
        private void SetDefaultData()
        {
            chkEnableDigitizedRecording.Checked = true;
            //chkStartRecordingWithBeep.Checked = true;
            //chkHigherQuality.Checked = false;
            //chkWaveFormat.Checked = true;
            txtFileStoragePath.Text = string.Empty;
        }

        #endregion
    }
}
