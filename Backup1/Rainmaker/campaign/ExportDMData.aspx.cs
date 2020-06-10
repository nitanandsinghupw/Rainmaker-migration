using System;
using System.Data;
using System.IO;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using System.Text;

namespace Rainmaker.Web.campaign
{
    public partial class ExportDMData : PageBase
    {
        #region Events
        /// <summary>
        /// Page load event,
        /// Bind the result codes to list on loading page for 1st time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                rdoNoDelete.Checked = true;   
            }
        }

        /// <summary>
        /// Saves result code for call,
        /// update agent Stat, 
        /// and update status to Ready for waiting for call agent,
        /// update call campaign fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string exportFilePath = "";
                string exportFileName = txtFileName.Text;
                if (txtFileName.Text.Length < 1)
                {
                    Response.Write("<script>alert('You must specify a valid file name.');</script>");
                    return;
                }

                try
                {
                    exportFilePath = Path.Combine(Server.MapPath("~/campaign/DataExports/"), exportFileName);
                }
                catch
                {
                    Response.Write("<script>alert('Error creating a file path on the server.  Your filename may have invalid characters or saving on the server may not be permitted.');</script>");
                    return;
                }

                Campaign currentCampaign = new Campaign();

                if (Session["Campaign"] != null)
                {
                    // we have an existing campaign
                    currentCampaign = (Campaign)Session["Campaign"];
                }
                else
                {
                    Response.Write("<script>alert('There is no campaign selected to export from.  Please go back to Data Manager and select one.');</script>");
                    return;
                }

                if (Session["SelectStmt"] == null || Session["SelectStmt"].ToString().Length < 2)
                {
                    Response.Write("<script>alert('There is no query selected to export from.  Please go back to Data Manager and select one.');</script>");
                    return;
                }
                //Response.Write("<script>alert('Please wait for the Save As dialog to appear. \n The system is working on your export.\n Depending on your export size, this may take a few minutes. \n Only close the window after you have been prompted to save.');</script>");
                string selectStatement = Session["SelectStmt"].ToString();

                // Load up the query
                dsExportData.ConnectionString = currentCampaign.CampaignDBConnString;
                dsExportData.SelectCommand = selectStatement;
                DataView dv = new DataView();
                dv = (DataView)dsExportData.Select(DataSourceSelectArguments.Empty);
                // add csv extension unless another extension exists
                
                if (exportFilePath.LastIndexOf('.') < (exportFilePath.Length - 4))
                {
                    exportFilePath = exportFilePath + ".csv";
                    exportFileName = exportFileName + ".csv";
                }

                // Make the file
                FileInfo file = new FileInfo(exportFilePath);

                file.Directory.Create();

                //Response.Write("<script>displayLoading();</script>");

                // Open the file stream object to start writing
                StreamWriter sw = new StreamWriter(exportFilePath, false);

                DataRowView drv = dv[0];

                // Write out column names
                int iColCount = dv.Table.Columns.Count;
                string fieldContents = "";
                for(int i = 0; i < iColCount; i++)
                {
                        fieldContents = dv.Table.Columns[i].ToString();

                    // Remove commas
                    fieldContents = fieldContents.Replace(",", " ");
	                sw.Write(fieldContents);
	                if (i < iColCount - 1)
	                {
		                sw.Write(",");
	                }
                }
                sw.Write(sw.NewLine);
                ActivityLogger.WriteAdminEntry(currentCampaign, "Exporting {0} record to file '{1}'", dv.Table.Rows.Count, exportFilePath);

	            // Now write all the rows.
	            foreach (DataRow dr in dv.Table.Rows)
	            {
                    for (int i = 0; i < iColCount; i++)
		            {
			            if (!Convert.IsDBNull(dr[i]))
			            {
                            string fieldname = dv.Table.Columns[i].ToString();
                            fieldContents = dr[i].ToString();
                            if (Session["EncryptedFields"] != null)
                            {

                                string encryptedfields = Session["EncryptedFields"].ToString();

                                string[] fieldListArray = encryptedfields.Split(',');

                                string passPhrase = "whatevesfasdfasdfr23";        // can be any string
                                string initVector = "Qt@&^SDF15F6g7H8"; // must be 16 bytes

                                // Before encrypting data, we will append plain text to a random
                                // salt value, which will be between 4 and 8 bytes long (implicitly
                                // used defaults).
                                RijndaelEnhanced rijndaelKey =
                                    new RijndaelEnhanced(passPhrase, initVector);
                                foreach (string strField in fieldListArray) {

                                    if (fieldname.Trim() == strField.Trim())
                                    {
                                        int index = dr.Table.Columns[strField.Trim()].Ordinal;
                                        string currentvalue = dr[index].ToString();
                                        ActivityLogger.WriteAdminEntry(currentCampaign, "ExportDMData - Column and current text: '{0}'", strField + " " + currentvalue);
                                        if (currentvalue != "&nbsp;" && currentvalue != "")
                                        {
                                            fieldContents = rijndaelKey.Decrypt(currentvalue);
                                            break;
                                        }
                                    }
                                }
                            }
                            // Remove commas
                            fieldContents = fieldContents.Replace(",", " ");
                            sw.Write(fieldContents);
			            }
			            if ( i < iColCount - 1)
			            {
				            sw.Write(",");
			            }
		            }
		            sw.Write(sw.NewLine);
	            }
	            sw.Close();

                if (rdoDelete.Checked)
                {
                    try
                    {
                        dsExportData.ConnectionString = currentCampaign.CampaignDBConnString;
                        string uniqueKeySelectStmt = string.Format("SELECT UniqueKey FROM Campaign {0}", selectStatement.Substring(selectStatement.IndexOf("WHERE"), (selectStatement.Length - selectStatement.IndexOf("WHERE"))));
                        dsExportData.SelectCommand = uniqueKeySelectStmt;
                        DataView dv1 = new DataView();
                        dv1 = (DataView)dsExportData.Select(DataSourceSelectArguments.Empty);
                        // Execute WS method to delete everything in the DB
                        List<long> keyList = new List<long>();
                        foreach (DataRow dr in dv1.Table.Rows)
                        {
                            keyList.Add(Convert.ToInt64(dr[0]));
                        }
                        ActivityLogger.WriteAdminEntry(currentCampaign, "Key List complete, contains {0} keys.", keyList.Count);

                        string deleteStatement = string.Format("DELETE {0}", selectStatement.Substring(selectStatement.IndexOf("FROM"), (selectStatement.Length - selectStatement.IndexOf("FROM"))));
                        dsExportData.SelectCommand = deleteStatement;
                        dv = (DataView)dsExportData.Select(DataSourceSelectArguments.Empty);
                        ActivityLogger.WriteAdminEntry(currentCampaign, "Bulk delete complete.");

                        long totalKeys = keyList.Count;
                        if (keyList.Count > 0)
                        {
                            int chunkSize = 1000;
                            try
                            {
                                chunkSize = Convert.ToInt32(ConfigurationManager.AppSettings["ChunkSize"]);
                            }
                            catch { }
                            CampaignService campService = new CampaignService();
                            XmlDocument xDocKeysToDelete = new XmlDocument();
                            campService.Timeout = System.Threading.Timeout.Infinite;
                            while (keyList.Count > 0)
                            {
                                if (keyList.Count < chunkSize)
                                {
                                    xDocKeysToDelete.LoadXml(Serialize.SerializeObject(keyList, typeof(List<long>)));
                                    campService.DeleteExportedLeads(xDocKeysToDelete, currentCampaign.CampaignDBConnString);
                                    keyList.Clear();
                                }
                                else
                                {
                                    List<long> templist = keyList.GetRange(0, chunkSize);
                                    keyList.RemoveRange(0, chunkSize);
                                    xDocKeysToDelete.LoadXml(Serialize.SerializeObject(templist, typeof(List<long>)));
                                    campService.DeleteExportedLeads(xDocKeysToDelete, currentCampaign.CampaignDBConnString);
                                    templist.Clear();
                                }
                            }
                        }
                        hdnDeleteConfirmed.Value = "False";
                        Session["ViewChanged"] = "yes";
                    }
                    catch (Exception ex)
                    {
                        ActivityLogger.WriteException(ex, "Admin");
                    }
                    
                }
                // Download file here
                
                Response.ContentType = "application/ms-excel";
                Response.AddHeader("content-disposition", "attachment; filename=" + exportFileName);
                Response.TransmitFile(exportFilePath);
                Response.End();
                //file.Delete();
                
            }
            catch (Exception ex)
            {
                PageMessage = "Exception saving view: " + ex.Message;
            }
        }
        #endregion
    }
}
