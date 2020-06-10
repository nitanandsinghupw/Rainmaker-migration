using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using Rainmaker.Common.DomainModel;
using Rainmaker.Common.Csv;
using System.Xml;
using Rainmaker.Web.CampaignWS;

namespace Rainmaker.Web.campaign
{
    public partial class Import : PageBase
    {
        #region Events


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ReadCampaignInfo();
                importRule1.Checked = true;
                exception1.Checked = true;
                setNeverCall.Checked = true;
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void ReadCampaignInfo()
        {
            if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                anchHome.InnerText = objCampaign.Description;
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        protected void lbtnNext_Click(object sender, EventArgs e)
        {
            if (!IsCampaignRunning())
            {
                UploadFile();
            }
            else
            {
                PageMessage = "Numbers cannot be imported while the campaign is running.";
            }
        }
        #endregion


        #region Private Methods

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void UploadFile()
        {
            /*
             * D. Pollastrini
             * 2012-04-18 13:04
             * See comments on deprecated isFileTooBig method.
            */

            // if(isFileTooBig()) return;

            // 2010-06-12 Dave Pollastrini
            // Why are we loading import params from non-visible elements that are not loaded from campaign.
            // e.g., allowSevenDigitNums and allTenDigitNums are loaded per chkSevenDigitNums and 
            // chkTenDigitNums respecitively.  However these UI elements are not visible and never bound on this
            // page.  They are then passed via session/redirect to the ImportMappings page that use these "UI" values
            // rather than the proper values set in the campaign.
            //
            // Will keep logic here, but will handle getting proper values into "allowXXDigitNums' to ImportMappings.

            List<string> values = prepareFile();
            string path = values[0];
            if (CheckFile(path, chkFirstLine.Checked, ddlDelimiter.SelectedValue))
            {
                Rainmaker.Web.common.ImportParams importparams = new Rainmaker.Web.common.ImportParams();
                importparams.delimter = ddlDelimiter.SelectedValue;
                importparams.filePath = path;
                importparams.uploadDirectory = values[1];
                importparams.isFirstRowHeader = chkFirstLine.Checked;
                importparams.allowSevenDigitNums = chkSevenDigitNums.Checked;
                importparams.allowTenDigitNums = chkTenDigitNums.Checked;
                importparams.importRule = whichImportRule();
                importparams.exceptionType = whichException();
                importparams.neverCallType = (setNeverCall.Checked) ? 1 : 0;
                Session["importparams"] = importparams;
                Response.Redirect("ImportMappings.aspx");
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private int whichImportRule()
        {
            if (importRule1.Checked) return 1;
            if (importRule2.Checked) return 2;
            if (importRule3.Checked) return 3;
            return 0;
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private int whichException()
        {
            if (exception1.Checked) return 1;
            if (exception2.Checked) return 2;
            if (exception3.Checked) return 3;
            return 0;
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private List<string> prepareFile()
        {
            string UploadDirectory = GetServerPath() + @"\UploadedFiles\" + DateTime.Today.Ticks.ToString() + "\\";
            string strFileName = FormatFileName(Path.GetFileName(fileUpload.PostedFile.FileName));
            string path = UploadDirectory + strFileName;
            Directory.CreateDirectory(UploadDirectory);
            fileUpload.PostedFile.SaveAs(path);
            List<string> values = new List<string>();
            values.Add(path);
            values.Add(UploadDirectory);
            return values;
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private bool isFileTooBig()
        {
            // The server seems to throw an error if the file is too big, so this doesn't seem to matter.

            /*
             * D. Pollastrini
             * 2012-04-18 13:08
             * 
             * This function is deprecated and was previously called by the UploadFile method.  
             * However, it will never be called if the upload file exceeds the 
             * <httpRuntime maxRequestLength="??????" /> attribute in web.config. IIS will throw an error 
             * before the application even sees the file is too big.
             * 
             * Adjust maximum file upload size using maxRequestLength. Related errors may be caught 
             * globally in the global.asax and redirected to a general error page, but cannot be 
             * handled by this page.
            */

            Int64 maxfileUploadSize = 20971520;
            if (fileUpload.PostedFile.ContentLength > maxfileUploadSize)
            {
                PageMessage = "The Upload File Size Cannot Exceed 20MB";
                return true;
            }
            else
            {
                return false;
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        // Checks uploaded file for formating errors
        private bool CheckFile(string Filepath, bool IsHeader, string delimiter)
        {
            bool IsFormated = true;
            string filename = Path.GetFileName(Filepath);
            string fileext = filename.Substring((filename.Length - 4), 4).ToLower();
            if (fileext != ".csv" && fileext != ".txt")
            {
                IsFormated = false;
                PageMessage = "The File Supported for Upload are .csv and .txt";
            }
            if (IsFormated)
            {
                try
                {
                    char carDelimiter = Convert.ToChar(delimiter == "t" ? "\t" : delimiter);
                    CreateSchemaFile(Filepath, carDelimiter, IsHeader);
                    DataSet ds = GetCsvDataSet(Filepath, IsHeader);
                }
                catch (Exception ex)
                {
                    // It throw an exception, so remove ill-formed file off of the filesytem
                    try
                    {
                        File.Delete(Filepath);
                    }
                    catch { }
                    IsFormated = false;
                    PageMessage = "The File is not well formatted: " + ex.Message.ToString();
                }
            }
            return IsFormated;
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        // This just makes the file pretty so it won't cause any issues with NTFS
        private string FormatFileName(string strFileName)
        {
            if (strFileName.IndexOf(" ") != -1)
            {
                strFileName = strFileName.Replace(" ", "_");
            }
            if (strFileName.IndexOf("-") != -1)
            {
                strFileName = strFileName.Replace("-", "_");
            }
            strFileName = System.Text.RegularExpressions.Regex.Replace(strFileName, @"[^\w\._]", "");
            if (strFileName.IndexOf(".") != strFileName.LastIndexOf("."))
            {
                string[] strArray = strFileName.Split('.');
                strFileName = "";
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (i != strArray.Length - 1)
                    {
                        strFileName = strFileName + strArray[i];
                    }
                    else
                    {
                        strFileName = strFileName + "." + strArray[i];
                    }
                }
            }
            return strFileName;
        }

        private string GetServerPath()
        {
            string serverPath = Server.MapPath("");
            serverPath = serverPath.Substring(0, serverPath.LastIndexOf(@"\"));
            return serverPath;
        }
        #endregion
    }
}
