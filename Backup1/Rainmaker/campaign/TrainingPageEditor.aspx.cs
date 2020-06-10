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

namespace Rainmaker.Web.campaign
{
    public partial class TrainingPageEditor : PageBase
    {
        TrainingPage objTrainingPage = null;
        #region Events

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (GetTrainingPageID() > 0)
                {
                    GetTrainingPageDetailByID(GetTrainingPageID());
                }
                else
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];

                    try
                    {
                        lblCampaignName.Text = objCampaign.Description;// Replaced Short description
                    }
                    catch { }
                    txtDisplayTime.Text = "20";
                }
            }
        }

        /// <summary>
        /// Saves the Data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSaveTrainingPage_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        /// <summary>
        /// Cancels the changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            if (GetTrainingPageID() > 0)
                GetTrainingPageDetailByID(GetTrainingPageID());
            else
                ClearData();
        }

        /// <summary>
        /// Navigates to pages list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void lbtnAddPage_Click(object sender, EventArgs e)
        //{
        //    Response.Redirect("ScriptList.aspx?ParentScriptID=" + GetQueryString()+"&ParentScriptName="+GetParentScriptName());
        //}

        protected void lbtnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrainingList.aspx");
        }

        //protected void lbtnSaveScripAs_Click(object sender, EventArgs e)
        //{
        //    // Make a copy of changed script like in save below
        //    BuildSavableScriptCopy();
        //    // Give that an ID, save it and pass to clone below
        //    Response.Redirect("CloneScript.aspx?ScriptID=" + GetQueryString()+"&ParentScriptName="+GetParentScriptName());
        //}
        

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets ScriptID
        /// </summary>
        /// <returns></returns>
        private long GetTrainingPageID()
        {
            long TrainingPageID = 0;
            if (Request.QueryString["TrainingPageID"] != null)
            {
                TrainingPageID = Convert.ToInt64(Request.QueryString["TrainingPageID"]);
            }
            return TrainingPageID;
        }

        private long GetTrainingSchemeID()
        {
            long TrainingSchemeID = 0;
            if (Request.QueryString["TrainingSchemeID"] != null)
            {
                TrainingSchemeID = Convert.ToInt64(Request.QueryString["TrainingSchemeID"]);
            }
            return TrainingSchemeID;
        }

        /// <summary>
        /// Saves Script Data
        /// </summary>
        private void SaveData()
        {   
            if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                TrainingPage objTrainingPage = new TrainingPage();

                objTrainingPage.PageID = GetTrainingPageID();
                objTrainingPage.Name = txtTrainingPageName.Text.Trim();
                objTrainingPage.Content = Server.UrlEncode(hdnTrainingPageContent.Value);
                objTrainingPage.TrainingSchemeID = GetTrainingSchemeID();
                objTrainingPage.DisplayTime = Convert.ToInt16(txtDisplayTime.Text);

                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocTrainingPage = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();
                try
                {
                    xDocTrainingPage.LoadXml(Serialize.SerializeObject(objTrainingPage, "TrainingPage"));
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                    objTrainingPage = (TrainingPage)Serialize.DeserializeObject(objCampaignService.TrainingPageInsertUpdate
                        (xDocCampaign, xDocTrainingPage), "TrainingPage");
                    if (objTrainingPage.Name.IndexOf("###ERROR###") >= 0)
                    {
                        PageMessage = objTrainingPage.Name.Substring(11);
                    }
                    else
                        Response.Redirect("TrainingList.aspx?TrainingSchemeID=" + GetTrainingSchemeID());
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
                //objScript.ScriptID = GetTrainingPageID();
                //objScript.ScriptName = txtScriptName.Text.Trim();
                //objScript.ScriptHeader = Server.UrlEncode(hdnScriptHeader.Value);
                ////objScript.ScriptSubHeader = Server.UrlEncode(hdnScriptSubHeader.Value);
                //objScript.ScriptBody = Server.UrlEncode(hdnTrainingPageContent.Value);

                //objScript.ParentScriptID = GetParentScriptId();
                //Session["SaveAsScript"] = objScript;
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
            txtTrainingPageName.Text = string.Empty;
            hdnScriptHeader.Value = string.Empty;
            //hdnScriptSubHeader.Value = string.Empty;
            hdnTrainingPageContent.Value = string.Empty;
        }

        /// <summary>
        /// Gets script details by scriptID
        /// </summary>
        /// <param name="ScriptID"></param>
        private void GetTrainingPageDetailByID(long trainingPageID)
        {
            Campaign objCampaign = (Campaign)Session["Campaign"];

            CampaignService objCampService = new CampaignService();
            XmlDocument xDocCampaign = new XmlDocument();
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            try
            {
                objTrainingPage = (TrainingPage)Serialize.DeserializeObject(objCampService.GetTrainingPage(xDocCampaign, trainingPageID), "TrainingPage");
                lblCampaignName.Text = objCampaign.Description;// Replaced Short description
                txtTrainingPageName.Text = objTrainingPage.Name;
                //hdnScriptSubHeader.Value = Server.UrlDecode(objScript.ScriptSubHeader);
                hdnTrainingPageContent.Value = Server.UrlDecode(objTrainingPage.Content);
                txtDisplayTime.Text = objTrainingPage.DisplayTime.ToString();
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        //private void BindCampaignFieldsLocal()
        //{
        //    if (Session["Campaign"] == null) return;
        //    Campaign objCampaign = (Campaign)Session["Campaign"];
        //    DataSet dsCampaignFields;
        //    DataSet dsScriptList;
        //    try
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        StringBuilder sb_size = new StringBuilder();
        //        StringBuilder sb_boolfields = new StringBuilder();
        //        CampaignService objCampaignService = new CampaignService();
        //        XmlDocument xDocCampaign = new XmlDocument();
        //        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
        //        dsCampaignFields = objCampaignService.GetCampaignFields(xDocCampaign);
        //        sb.Append("<script language=\"javascript\" type=\"text/javascript\">");
                
        //        sb.Append("arrRMFields = new Array(");
        //        sb_size.Append("arrRMFields_size = new Array(");
        //        sb_boolfields.Append("arrRMFields_b = new Array(");
        //        int recCount = dsCampaignFields.Tables[0].Rows.Count;
        //        int iCounter = 0;
        //        int iCounter_b = 0;

        //        foreach(DataRow dr in dsCampaignFields.Tables[0].Rows){
        //            if (dr["FieldType"].ToString().ToLower() != "boolean")
        //            {
        //                sb.AppendFormat("{1}\"{0}\"", dr["FieldName"], (iCounter == 0 ? "" : ","));
        //                if (dr["FieldType"].ToString().ToLower() == "string")
        //                    sb_size.AppendFormat("{1}\"{0}\"", dr["Value"], (iCounter == 0 ? "" : ","));
        //                else
        //                    sb_size.AppendFormat("{1}\"{0}\"", "-1", (iCounter == 0 ? "" : ","));
        //                iCounter++;
        //            }
        //            else
        //            {
        //                sb_boolfields.AppendFormat("{1}\"{0}\"", dr["FieldName"], (iCounter_b == 0 ? "" : ","));
        //                iCounter_b++;
        //            }
        //        }

        //        sb.AppendFormat("{0}\"campaignname\",\"agentname\"", dsCampaignFields.Tables[0].Rows.Count>0?",":"");
        //        sb_size.AppendFormat("{0}\"255\",\"255\"", dsCampaignFields.Tables[0].Rows.Count > 0 ? "," : "");


        //        foreach (System.Collections.Generic.KeyValuePair<string, string> campkeyValue in GetDefaultCampaignFields_SE())
        //        {
        //            sb.AppendFormat("{0}\"{1}\"", ",", campkeyValue.Key);
        //            sb_size.AppendFormat("{0}\"{1}\"", ",", campkeyValue.Value);
        //        }
                
        //        sb.Append(");");
        //        sb_boolfields.Append(");");
        //        sb_size.Append(");");


        //        sb.Append(sb_size.ToString());
        //        sb.Append(sb_boolfields.ToString());


        //        if (GetParentScriptId() > 0)
        //        {
        //            dsScriptList = objCampaignService.GetPageListByScriptId(xDocCampaign, GetParentScriptId());
        //        }
        //        else
        //        {
        //            dsScriptList = objCampaignService.GetPageListByScriptId(xDocCampaign, GetQueryString());
        //        }
        //        sb.AppendFormat("arrScripts = new Array(");
        //        bool added = false;
        //        foreach (DataRow dr in dsScriptList.Tables[0].Rows)
        //        {
        //            if (GetQueryString() != Convert.ToInt64(dr["ScriptID"]))
        //            {
        //                if (!added)
        //                {
        //                    sb.AppendFormat("new Array(\"{0}\",\"{1}\")", dr["ScriptGuid"], dr["ScriptName"]);
        //                    added = true;
        //                }
        //                else
        //                {
        //                    sb.AppendFormat(",new Array(\"{0}\",\"{1}\")", dr["ScriptGuid"], dr["ScriptName"]);
        //                }
        //            }

        //        }

        //        //if (dsScriptList.Tables[0].Rows.Count > 0)
        //        //{
        //            if (GetParentScriptId() > 0)
        //            {
        //                Script scrt = (Script)Serialize.DeserializeObject(objCampaignService.GetScriptByScriptID(xDocCampaign, GetParentScriptId()), "Script");
        //                if (scrt != null)
        //                {
        //                    sb.AppendFormat("{2}new Array(\"{0}\",\"{1}\")", scrt.ScriptGuid, scrt.ScriptName, added?",":"");
        //                }
        //            }
        //            //else
        //            //{
        //            //    if (objScript != null)
        //            //    {
        //            //        sb.AppendFormat(",new Array(\"{0}\",\"{1}\")", objScript.ScriptGuid, objScript.ScriptName);
        //            //    }
        //            //}
        //        //}

        //        sb.Append(");");

        //        // resultcodes
        //        StringBuilder sb_rc = new StringBuilder();
        //        try
        //        {
        //            sb_rc.AppendFormat("arrResultcodes = new Array(");
        //            DataSet dsResultCodes = objCampaignService.GetResultCodes(xDocCampaign);
        //            DataRow[] drResultCodes = dsResultCodes.Tables[0].Select("DateDeleted is null");
        //            string[] hideSysResultCodes = ConfigurationManager.AppSettings["SysResultCodesToHide"].Split(',');

        //            for (int k = 0; k < drResultCodes.Length; k++)
        //            {
        //                if (ShowSysResultCode(drResultCodes[k]["Description"].ToString(), hideSysResultCodes))
        //                {
        //                    sb_rc.AppendFormat("{1}\"{0}\"", drResultCodes[k]["Description"].ToString(), k == 0 ? "" : ",");
        //                }
        //            }

        //            sb_rc.Append(");");

        //            sb.Append(sb_rc.ToString());
        //        }
        //        catch { }

        //        sb.Append("</script>");
        //        ltrlCampaignFldScript.Text = sb.ToString();
        //    }
        //    catch (Exception ex)
        //    {
        //        PageMessage = ex.Message;
        //    }
        //}

        #endregion
    }
}
