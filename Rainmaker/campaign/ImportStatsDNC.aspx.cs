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
using Rainmaker.Common.DomainModel;
using Rainmaker.Web.CampaignWS;
using Rainmaker.libs.tools;

namespace Rainmaker.Web.campaign
{
    public partial class ImportStatsDNC : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    UpdateQueryList();
                }
                catch { }
            }
            if (Session["ImportStats"] != null)
            {
                ImportStats objImportStats = (ImportStats)Session["ImportStats"];
                lblTotalLeads.Text = objImportStats.TotalLeads.ToString();
                lblImported.Text = objImportStats.LeadsImported.ToString();
                lblBlank.Text = objImportStats.LeadsBlankPhoneNumber.ToString();
                lblSpChar.Text = objImportStats.LeadsSPCharPhoneNumber.ToString();
                lblBadData.Text = objImportStats.LeadsBadData.ToString();
                lblDuplicate.Text = objImportStats.LeadsDuplicate.ToString();
                lblBadLength.Text = objImportStats.LeadsInvalidNumberLength.ToString();
                lblUpdated.Text = objImportStats.LeadsUpdated.ToString();
                try
                {
                    if (hdnExportToClient.Value != "")
                    {
                        ExportToClientMachine(hdnExportToClient.Value, "text/csv");
                        hdnExportToClient.Value = "";
                    }
                    else
                    {
                        if (objImportStats.LeadsDuplicate > 0 && Session["DupFilePath"] != null && Session["DupFilePath"].ToString() != "")
                        {
                            if (System.IO.File.Exists(Session["DupFilePath"].ToString()))
                            {
                                hdnExportToClient.Value = Session["DupFilePath"].ToString();
                            }
                            Session["DupFilePath"] = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void UpdateQueryList()
        {

            ImportStats objImportStats = (ImportStats)Session["ImportStats"];
            if (objImportStats.TotalLeads > 0 && Session["Campaign"] != null)
            {
                XmlDocument xDocCampaign = new XmlDocument();
                Campaign objCampaign = (Campaign)Session["Campaign"];
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                CampaignService objCampService = new CampaignService();
                objCampService.UpdateCampaignQueriesStats(xDocCampaign);
            }
        }


        /// <summary>
        /// Sends exported package(zip file) to client machine
        /// </summary>
        /// <param name="strFilePath"></param>
        /// <param name="strMimeType"></param>
        private void ExportToClientMachine(string strFilePath, string strMimeType)
        {
            string strfilename = System.IO.Path.GetFileName(strFilePath);
            long fileSize = 0;
            System.IO.FileInfo fInfo = null;
            try
            {
                if (System.IO.File.Exists(strFilePath))
                {
                    fInfo = new System.IO.FileInfo(strFilePath);
                    fileSize = fInfo.Length;
                    Response.ClearContent();
                    Response.ClearHeaders();
                    Response.ContentType = strMimeType;
                    Response.AddHeader("Content-Disposition", "attachment; filename=\"" + strfilename + "\"");
                    if (fileSize > 0)
                        Response.AppendHeader("Content-Length", fileSize.ToString());
                    Response.WriteFile(strFilePath);
                    Response.Flush();
                    Response.End();
                }
            }
            catch
            {
            }
            finally
            {
                fInfo = null;
            }
        }
    }
}
