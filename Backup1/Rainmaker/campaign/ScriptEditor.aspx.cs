using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using Rainmaker.Web.CampaignWS;
using Rainmaker.Common.DomainModel;
using Rainmaker.libs.tools;

namespace Rainmaker.Web.campaign
{
    public partial class ScriptEditor : PageBase
    {
        Script objScript = null;
        #region Events
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (GetQueryString() > 0)
                {
                    GetScriptDetailByScriptID(GetQueryString());
                    if (GetParentScriptId() <= 0)
                    {
                        lbtnAddPage.Visible = true;
                        lbtnSaveScripAs.Visible = true;
                    }
                }
                else
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    try
                    {
                        lblCampaignName.Text = objCampaign.Description;// Replaced Short description
                    }
                    catch { }
                }
                if (GetParentScriptId() > 0)
                {
                    //ltrlPageHeader.Text = "&nbsp;&nbsp;<img src=\"../images/arrowright.gif\">&nbsp;&nbsp;<b>Script Editor</b>";
                    lblScriptname.Text = "PAGE NAME";
                    ltrlScriptName.Text = "<tr><td align=\"left\" colspan=\"2\"><b>SCRIPT NAME : " + GetParentScriptName() + "</b></td></tr>";
                }
                BindCampaignFieldsLocal();

                CKEditor1.Text = hdnScriptBody.Value;
            }
            
        }

        protected void lbtnsave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            if (GetQueryString() > 0)
                GetScriptDetailByScriptID(GetQueryString());
            else
                ClearData();
        }

        /// <summary>
        /// Navigates to pages list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAddPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("ScriptList.aspx?ParentScriptID=" + GetQueryString() + "&ParentScriptName=" + GetParentScriptName());
        }

        protected void lbtnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("ScriptList.aspx?ParentScriptID=" + GetParentScriptId() + "&ParentScriptName=" + GetParentScriptName());
        }

        protected void lbtnSaveScripAs_Click(object sender, EventArgs e)
        {
            // *** Uncomment to reactivate CK Editor upgrade
            //hdnScriptBody.Value = CKEditor1.Text;
            // Make a copy of changed script like in save below
            BuildSavableScriptCopy();
            // Give that an ID, save it and pass to clone below
            Response.Redirect("CloneScript.aspx?ScriptID=" + GetQueryString() + "&ParentScriptName=" + GetParentScriptName());
        }


        #endregion

        #region Private Methods

        /// <summary>
        /// Gets ScriptID
        /// </summary>
        /// <returns></returns>
        private long GetQueryString()
        {
            long ScriptID = 0;
            if (Request.QueryString["ScriptID"] != null)
            {
                ScriptID = Convert.ToInt64(Request.QueryString["ScriptID"]);
            }
            return ScriptID;
        }

        /// <summary>
        /// Gets Parent ScriptID
        /// </summary>
        /// <returns></returns>
        private long GetParentScriptId()
        {
            long parentScriptID = 0;
            if (Request.QueryString["ParentScriptID"] != null)
            {
                parentScriptID = Convert.ToInt64(Request.QueryString["ParentScriptID"]);
            }
            return parentScriptID;
        }

        /// <summary>
        /// Gets Parent Script Name
        /// </summary>
        /// <returns></returns>
        private string GetParentScriptName()
        {
            string parentScriptName = "";
            if (Request.QueryString["ParentScriptName"] != null)
            {
                parentScriptName = Request.QueryString["ParentScriptName"].ToString();
            }
            return parentScriptName;
        }

        // Checks the script to see if Make Simlar code is already present and
        // appends it if it's not
        private string addMakeSimlarJS(string script)
        {
            if (script.IndexOf("rmMakeSimilar") == -1)
            {
                script += "<script type='text/javascript'>";
                script += " alert('I am the prescript'); ";
                script += "$(document).ready";
                script += "	(";
                script += "	function() ";
                script += "		{";
                script += "		alert('Hello World');";
                //script += "		rmMakeSimilar();";
                script += "		}";
                script += "	);";
                //
                script += "function rmMakeSimilar()";
                script += "	{";
                script += "	console.log('called make similar');";
                script += "	}";
                script += "</script>";
            }
            return script;
        }

        private void SaveData()
        {
            if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                Script objScript = new Script();
                objScript.ScriptID = GetQueryString();
                objScript.ScriptName = txtScriptName.Text.Trim();
                objScript.ScriptHeader = Server.UrlEncode(hdnScriptHeader.Value);
                //objScript.ScriptSubHeader = Server.UrlEncode(hdnScriptSubHeader.Value);
                // DRP 2012-04-23 objScript.ScriptBody = Server.UrlEncode(hdnScriptBody.Value);
                objScript.ScriptBody = CKEditor1.Text;
                //objScript.ScriptBody = addMakeSimlarJS(objScript.ScriptBody);
                //SysLog.SendLog("Script Editor", objScript.ScriptBody);
                objScript.ParentScriptID = GetParentScriptId();
                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocScript = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();
                try
                {
                    xDocScript.LoadXml(Serialize.SerializeObject(objScript, "Script"));
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    objScript = (Script)Serialize.DeserializeObject(objCampaignService.ScriptInsertUpdate
                                                (xDocCampaign, xDocScript), "Script");
                    if (objScript.ScriptName.IndexOf("###ERROR###") >= 0)
                    {
                        PageMessage = objScript.ScriptName.Substring(11);
                    }
                    else
                        Response.Redirect("ScriptList.aspx?ParentScriptID=" + GetParentScriptId() + "&ParentScriptName=" + GetParentScriptName());
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
            }
        }

        private void BuildSavableScriptCopy()
        {
            Script objScript = new Script();
            try
            {
                objScript.ScriptID = GetQueryString();
                objScript.ScriptName = txtScriptName.Text.Trim();
                objScript.ScriptHeader = Server.UrlEncode(hdnScriptHeader.Value);
                //objScript.ScriptSubHeader = Server.UrlEncode(hdnScriptSubHeader.Value);
                objScript.ScriptBody = Server.UrlEncode(hdnScriptBody.Value);

                objScript.ParentScriptID = GetParentScriptId();
                Session["SaveAsScript"] = objScript;
            }
            catch (Exception ex)
            {
                PageMessage = "Error building script copy for Save As :" + ex.Message;
            }
        }

        /// <summary>
        /// Clears the fields
        /// </summary>
        private void ClearData()
        {
            txtScriptName.Text = string.Empty;
            hdnScriptHeader.Value = string.Empty;
            //hdnScriptSubHeader.Value = string.Empty;
            hdnScriptBody.Value = string.Empty;
        }

        /// <summary>
        /// Gets script details by scriptID
        /// </summary>
        /// <param name="ScriptID"></param>
        private void GetScriptDetailByScriptID(long ScriptID)
        {
            Campaign objCampaign = (Campaign)Session["Campaign"];

            CampaignService objCampService = new CampaignService();
            XmlDocument xDocCampaign = new XmlDocument();
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            try
            {
                objScript = (Script)Serialize.DeserializeObject(objCampService.GetScriptByScriptID(xDocCampaign, ScriptID), "Script");
                lblCampaignName.Text = objCampaign.Description;// Replaced Short description
                txtScriptName.Text = objScript.ScriptName;
                hdnScriptHeader.Value = Server.UrlDecode(objScript.ScriptHeader);
                //hdnScriptSubHeader.Value = Server.UrlDecode(objScript.ScriptSubHeader);
                hdnScriptBody.Value = Server.UrlDecode(objScript.ScriptBody);
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        private void BindCampaignFieldsLocal()
        {
            if (Session["Campaign"] == null) return;
            Campaign objCampaign = (Campaign)Session["Campaign"];
            DataSet dsCampaignFields;
            DataSet dsScriptList;
            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb_size = new StringBuilder();
                StringBuilder sb_boolfields = new StringBuilder();
                StringBuilder sb_datetimefields = new StringBuilder();
                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsCampaignFields = objCampaignService.GetCampaignFields(xDocCampaign);
                sb.Append("<script language=\"javascript\" type=\"text/javascript\">");

                sb.Append("arrRMFields = new Array(");
                sb_size.Append("arrRMFields_size = new Array(");
                sb_boolfields.Append("arrRMFields_b = new Array(");
                sb_datetimefields.Append("arrRMFields_datetime = new Array(");
                int recCount = dsCampaignFields.Tables[0].Rows.Count;
                int iCounter = 0;
                int iCounter_b = 0;
                int iCounter_datetime = 0;

                foreach (DataRow dr in dsCampaignFields.Tables[0].Rows)
                {
                    string fieldType = dr["FieldType"].ToString().ToLower();
                    string fieldName = dr["FieldName"].ToString();
                    string fieldValue = dr["Value"].ToString();

                    switch (fieldType)
                    {
                        case "encrypted":
                        case "string":
                            sb.AppendFormat("{1}\"{0}\"", fieldName, (iCounter == 0 ? "" : ","));
                            sb_size.AppendFormat("{1}\"{0}\"", fieldValue, (iCounter == 0 ? "" : ","));
                            iCounter++;
                            break;
                        case "date":
                            sb_datetimefields.AppendFormat("{1}\"{0}\"", fieldName, (iCounter_datetime == 0 ? "" : ","));
                            iCounter_datetime++;
                            break;
                        case "boolean":
                            sb_boolfields.AppendFormat("{1}\"{0}\"", fieldName, (iCounter_b == 0 ? "" : ","));
                            iCounter_b++;
                            break;
                        case "integer":
                            sb.AppendFormat("{1}\"{0}\"", fieldName, (iCounter == 0 ? "" : ","));
                            sb_size.AppendFormat("{1}\"{0}\"", "10", (iCounter == 0 ? "" : ","));
                            iCounter++;
                            break;
                        case "money":
                            sb.AppendFormat("{1}\"{0}\"", fieldName, (iCounter == 0 ? "" : ","));
                            sb_size.AppendFormat("{1}\"{0}\"", "10", (iCounter == 0 ? "" : ","));
                            iCounter++;
                            break;
                        default:
                            sb.AppendFormat("{1}\"{0}\"", fieldName, (iCounter == 0 ? "" : ","));
                            sb_size.AppendFormat("{1}\"{0}\"", "-1", (iCounter == 0 ? "" : ","));
                            iCounter++;
                            break;
                    }
                }

                sb.AppendFormat("{0}\"campaignname\",\"agentname\"", dsCampaignFields.Tables[0].Rows.Count > 0 ? "," : "");
                sb_size.AppendFormat("{0}\"255\",\"255\"", dsCampaignFields.Tables[0].Rows.Count > 0 ? "," : "");

                foreach (System.Collections.Generic.KeyValuePair<string, string> campkeyValue in GetDefaultCampaignFields_SE())
                {
                    sb.AppendFormat("{0}\"{1}\"", ",", campkeyValue.Key);
                    sb_size.AppendFormat("{0}\"{1}\"", ",", campkeyValue.Value);
                }

                sb.Append(");");
                sb_boolfields.Append(");");
                sb_datetimefields.Append(");");
                sb_size.Append(");");

                sb.Append(sb_size.ToString());
                sb.Append(sb_boolfields.ToString());
                sb.Append(sb_datetimefields.ToString());

                if (GetParentScriptId() > 0)
                {
                    dsScriptList = objCampaignService.GetPageListByScriptId(xDocCampaign, GetParentScriptId());
                }
                else
                {
                    dsScriptList = objCampaignService.GetPageListByScriptId(xDocCampaign, GetQueryString());
                }
                sb.AppendFormat("arrScripts = new Array(");
                bool added = false;
                foreach (DataRow dr in dsScriptList.Tables[0].Rows)
                {
                    if (GetQueryString() != Convert.ToInt64(dr["ScriptID"]))
                    {
                        if (!added)
                        {
                            sb.AppendFormat("new Array(\"{0}\",\"{1}\")", dr["ScriptGuid"], dr["ScriptName"]);
                            added = true;
                        }
                        else
                        {
                            sb.AppendFormat(",new Array(\"{0}\",\"{1}\")", dr["ScriptGuid"], dr["ScriptName"]);
                        }
                    }

                }

                if (GetParentScriptId() > 0)
                {
                    Script scrt = (Script)Serialize.DeserializeObject(objCampaignService.GetScriptByScriptID(xDocCampaign, GetParentScriptId()), "Script");
                    if (scrt != null)
                    {
                        sb.AppendFormat("{2}new Array(\"{0}\",\"{1}\")", scrt.ScriptGuid, scrt.ScriptName, added ? "," : "");
                    }
                }

                sb.Append(");");

                // resultcodes
                StringBuilder sb_rc = new StringBuilder();
                try
                {
                    sb_rc.AppendFormat("arrResultcodes = new Array(");
                    DataSet dsResultCodes = objCampaignService.GetResultCodes(xDocCampaign);
                    DataRow[] drResultCodes = dsResultCodes.Tables[0].Select("DateDeleted is null");
                    string[] hideSysResultCodes = ConfigurationManager.AppSettings["SysResultCodesToHide"].Split(',');

                    for (int k = 0; k < drResultCodes.Length; k++)
                    {
                        if (ShowSysResultCode(drResultCodes[k]["Description"].ToString(), hideSysResultCodes))
                        {
                            sb_rc.AppendFormat("{1}\"{0}\"", drResultCodes[k]["Description"].ToString(), k == 0 ? "" : ",");
                        }
                    }

                    sb_rc.Append(");");

                    sb.Append(sb_rc.ToString());
                }
                catch { }

                sb.Append("</script>");
                ltrlCampaignFldScript.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion
    }
}
