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
using Rainmaker.Web.AgentsWS;
using Rainmaker.Web.CampaignWS;
using System.Text;

namespace Rainmaker.Web.agent
{
    public partial class CallDisposition : PageBase
    {
        #region Variables

        TextBox txtControl;
        CompareValidator cmpControl;
        private string[] hideSysResultCodes = { 
                "Scheduled Callback","Transferred to Agent",
                "Transferred to Dialer","Transferred to Verification","Unmanned Live Contact",
                "Inbound Abandoned by Agent","Inbound abandoned by Caller","Error","Failed",
                "Cadence Break","Loop Current Drop","Pbx Detected","No Ringback","Analysis Stopped",
                "No DialTone","FaxTone Detected", "Unmanned Transferred to Answering Machine", "Transferred Offsite" };

        private string[] hideResultCodesForAgent = { 
                "Answering Machine","Busy","Operator Intercept","Dropped","No Answer","Scheduled Callback"};
        #endregion

        #region Events

        protected void Page_Init(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Page load event,
        /// Bind the result codes to list on loading page for 1st time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            
            Agent objAgent = (Agent)Session["LoggedAgent"];
           
            if (!Page.IsPostBack)
            {
                
                if (Request["FromScript"] != null)
                {
                    
                    BindCallDisposition(true);
                   
                    DisposeCall(Request["resultcodename"].ToString());
                    
                    if (Session["LoggedAgent"] != null)
                        ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Call disposition complete from script in disposition dialog as '{0}'.", Request["resultcodename"].ToString());
                    
                }
                else
                {
                    
                    BindCallDisposition(false);
                    
                    SetResultCode();
                    
                    if (Session["LoggedAgent"] != null)
                        ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Call disposition complete from disposition dialog.");
                }
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
        protected void lbtnOk_Click(object sender, EventArgs e)
        {
            
            try
            {

                int resultcodeID;
                resultcodeID = Convert.ToInt32(lbxCallDisposition.SelectedValue);

                string resultCodeName = lbxCallDisposition.SelectedItem.Text;

                if (Request.QueryString["pagefrom"] != null &&
                    Request.QueryString["pagefrom"].ToString() == "ManualDialLookup")
                {
                    
                    try
                    {
                        DataSet dsCampaignDtls = (DataSet)Session["CampaignDtls"];

                        string updquery = string.Format("UPDATE Campaign SET CallResultCode={0} where UniqueKey = {1}",
                        lbxCallDisposition.SelectedValue, dsCampaignDtls.Tables[0].Rows[0]["UniqueKey"].ToString());

                        Campaign objCampaign = (Campaign)Session["Campaign"];
                        XmlDocument xDocCampaign = new XmlDocument();
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                        CampaignService campService = new CampaignService();
                        campService.UpdateCampaignDetails(xDocCampaign, updquery);
                    }
                    catch (Exception ex)
                    {
                        ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "CallDisposition lbtnOK_Click error: ", ex.Message);
                    }
                    try
                    {

                        ResultCode objResultCode;
                        Campaign objCampaign = (Campaign)Session["Campaign"];
                        DataSet dsCampaignDtls = (DataSet)Session["CampaignDtls"];
                        CampaignService objCampService = new CampaignService();
                        XmlDocument xDocCampaign = new XmlDocument();
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                        long ResultCode = Int64.Parse(lbxCallDisposition.SelectedValue);
                        objResultCode = (ResultCode)Serialize.DeserializeObject(objCampService.GetResultCodeByResultCodeID(xDocCampaign,  ResultCode), "ResultCode");
                        bool masterdncflag = objResultCode.MasterDNC;
                        if (masterdncflag == true)
                        {
                            dsViews.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
                            dsViews.SelectCommand = string.Format("SELECT PhoneNum FROM MasterDNC WHERE PhoneNum = '{0}'", dsCampaignDtls.Tables[0].Rows[0]["PhoneNum"].ToString());
                            DataView dv = new DataView();
                            dv = (DataView)dsViews.Select(DataSourceSelectArguments.Empty);

                            if (dv.Count < 1)
                            {

                                string mysql = string.Format("INSERT INTO MasterDNC (PhoneNum) VALUES ('{0}')", dsCampaignDtls.Tables[0].Rows[0]["PhoneNum"].ToString());

                                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "CallDisposition lbnOk_Click Insert Attempting to Execute: {0}", mysql);

                                
                                dsViews.SelectCommand = mysql;

                                DataView dv1 = new DataView();
                                dv1 = (DataView)dsViews.Select(DataSourceSelectArguments.Empty);

                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "CallDisposition lbnOk_Click Insert failed: {0}", ex.Message);
                    }

                    hdnClose.Value = "false";
                    if (Session["LoggedAgent"] != null)
                        ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Call disposition complete from manual lookup in disposition dialog as '{0}'.", resultCodeName);
                }
                else
                {

                    bool isManualDial = false;
                    try
                    {
                        if (Request.QueryString["pagefrom"].ToString() == "ManualDial")
                        {
                            isManualDial = true;
                        }
                    }
                    catch { }
                    DisposeCall(resultcodeID, isManualDial, resultCodeName);
                    DataSet dsCampaignDtls = (DataSet)Session["CampaignDtls"]; // campaign data
                    DateTime isScheduledCallback = Convert.ToDateTime(dsCampaignDtls.Tables[0].Rows[0]["scheduledate"] == Convert.DBNull ?
                        DateTime.MinValue : Convert.ToDateTime(dsCampaignDtls.Tables[0].Rows[0]["scheduledate"]));
                    if (isScheduledCallback > DateTime.MinValue)
                    {
                        //clear the date and time
                    }
                    
                    hdnUniqueKey.Value = dsCampaignDtls.Tables[0].Rows[0]["UniqueKey"].ToString();

                    hdnClose.Value = "false";
                    if (Session["LoggedAgent"] != null)
                        ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Call disposition complete from disposition dialog as '{0}'.", resultCodeName);
                }

            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
            hdnClose.Value = "false";
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "Close();", true);
            
        }

        #endregion

        #region Methods
       
        private void UpdatePauseTime()
        {
            if (Session["MDPauseTime"] != null)
            {

                Campaign objCampaign;
                CampaignService objCampService = new CampaignService();
                AgentStat objAgentStat;
                XmlDocument xDocAgentStat = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();

                if (Session["AgentStat"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];

                    objAgentStat = (AgentStat)Session["AgentStat"];

                    try
                    {
                        TimeSpan ts = DateTime.Now.Subtract((DateTime)Session["MDPauseTime"]);
                        objAgentStat.PauseTime += (decimal)ts.TotalSeconds;
                        Session.Remove("MDPauseTime");

                        xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                        objAgentStat = (AgentStat)Serialize.DeserializeObject(
                        objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");

                        Session["AgentStat"] = objAgentStat;
                    }
                    catch { }

                }
            }
        }

        /// <summary>
        /// Bind result description and code to list of selected Campaign
        /// </summary>
        private void BindCallDisposition(bool showAll)
        {
           
            DataSet dsResultCodes;
            DataView dvResultCodes;
            try
            {
                Agent objAgent = (Agent)Session["LoggedAgent"];
                CampaignService objCampService = new CampaignService();
                Campaign objCampaign = (Campaign)Session["Campaign"];
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));


                bool isVerification = false;
                try
                {
                    DataSet dsCampaignDtls = (DataSet)Session["CampaignDtls"];
                    int agentID = Convert.ToInt32(dsCampaignDtls.Tables[0].Rows[0]["AgentID"]);
                    int verificationAgentID = Convert.ToInt32(dsCampaignDtls.Tables[0].Rows[0]["VerificationAgentID"]);
                    if (objAgent.AgentID == verificationAgentID) //Determines that this is verification
                        isVerification = true;
                }
                catch {
                    
                }

                dsResultCodes = objCampService.GetResultCodes(xDocCampaign);
                dvResultCodes = dsResultCodes.Tables[0].DefaultView;
                if (isVerification)
                    dvResultCodes = FilterData(dsResultCodes.Tables[0], "DateDeleted is null");
                else
                    dvResultCodes = FilterData(dsResultCodes.Tables[0], "DateDeleted is null AND VerifyOnly = 0");

                try
                {
                    hideSysResultCodes = ConfigurationManager.AppSettings["SysResultCodesToHide"].Split(',');
                }
                catch {
                   
                }

                lbxCallDisposition.Items.Clear();
                if (dvResultCodes != null)
                {
                    
                    DataTable dtResultCodes = dvResultCodes.ToTable();

                    foreach (DataRow dr in dtResultCodes.Rows)
                    {
                        string resultCode = dr["Description"].ToString();
                        string resultCodeId = dr["ResultCodeID"].ToString();
                        
                        if (ShowSysResultCode(resultCode, hideSysResultCodes))
                        {
                            bool hideDefautResultCodes = true;
                            try
                            {
                                hideDefautResultCodes = Convert.ToBoolean(ConfigurationManager.AppSettings["HideDefaultResultcodesForAgentDisposition"]);
                            }
                            catch { }
                            if (showAll || !hideDefautResultCodes || ShowSysResultCode(resultCode, hideResultCodesForAgent))
                            {
                                lbxCallDisposition.Items.Add(new ListItem(resultCode, resultCodeId));
                            }
                        }
                    }
                }
                

                //lbxCallDisposition.DataSource = dvResultCodes;
                //lbxCallDisposition.DataTextField = "Description";
                //lbxCallDisposition.DataValueField = "ResultCodeID";
                //lbxCallDisposition.DataBind();

            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Get Campaign Details
        /// </summary>
        private void GetCampaignDetails()
        {
            
            if (Session["Campaign"] != null && Session["LoggedAgent"] != null)
            {
                
                Campaign objcampaign = (Campaign)Session["Campaign"];

                //DataSet dsCampaigndtls;
                Agent objagent = (Agent)Session["LoggedAgent"];

                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objcampaign, "Campaign"));

                XmlDocument xDocAgent = new XmlDocument();
                xDocAgent.LoadXml(Serialize.SerializeObject(objagent, "Agent"));
                

            }
        }

        /// <summary>
        /// Create campaign Fields controls dynamically
        /// </summary>
        private void CreateCampaignFields()
        {
            HtmlTableRow trData;
            HtmlTableCell tdLabel;
            HtmlTableCell tdTextBox;

            DataSet dsCampaignFields;
            dsCampaignFields = (DataSet)Session["CampaignFields"]; // campaign fields
            
            try
            {
                if (dsCampaignFields != null)
                {
                    if (dsCampaignFields.Tables.Count > 0)
                    {
                        string strFieldName = string.Empty;
                        string strFieldType = string.Empty;
                        int intFieldLength = 0;

                        for (int i = 0; i < dsCampaignFields.Tables[0].Rows.Count; i++)
                        {
                            strFieldName = dsCampaignFields.Tables[0].Rows[i]["FieldName"].ToString();
                            strFieldType = dsCampaignFields.Tables[0].Rows[i]["FieldType"].ToString();
                            intFieldLength = strFieldType == "String" ? Convert.ToInt32(dsCampaignFields.Tables[0].Rows[i]["Value"]) : 10;


                            // Begin  Code for Creating controls dynamically

                            trData = new HtmlTableRow();
                            trData.ID = "tr" + i.ToString();

                            tdLabel = new HtmlTableCell();
                            tdLabel.ID = "tdl" + i.ToString();
                            tdLabel.Align = "right";
                            tdLabel.InnerHtml = "<b>" + strFieldName + "&nbsp;:&nbsp;</b>";

                            txtControl = new TextBox();

                            // td  text box
                            tdTextBox = new HtmlTableCell();
                            tdTextBox.ID = "tdt" + i.ToString();

                            // text box
                            txtControl.ID = "txt" + strFieldName;
                            txtControl.Visible = true;
                            txtControl.CssClass = "txtnormal";
                            txtControl.MaxLength = intFieldLength;
                            txtControl.Text = "";

                            if (strFieldName == "PHONENUM")
                            {
                                txtControl.ReadOnly = true;
                            }

                            // td text boxes
                            tdTextBox.InnerHtml = "&nbsp;";
                            tdTextBox.Align = "left";
                            tdTextBox.Controls.Add(txtControl);

                            if (strFieldType != "Date")
                            {
                                cmpControl = new CompareValidator();
                                cmpControl.ID = "cmp" + strFieldName;
                                cmpControl.Operator = ValidationCompareOperator.DataTypeCheck;
                                cmpControl.ControlToValidate = "txt" + strFieldName;
                                cmpControl.ErrorMessage = "Please enter valid " + strFieldName;
                                cmpControl.Text = "*";
                                cmpControl.Display = ValidatorDisplay.Static;
                                cmpControl.SetFocusOnError = true;
                                switch (strFieldType)
                                {
                                    case "Decimal":
                                        cmpControl.Type = ValidationDataType.Double;
                                        break;
                                    case "Money":
                                        cmpControl.Type = ValidationDataType.Double;
                                        break;
                                    case "Integer":
                                        cmpControl.Type = ValidationDataType.Integer;
                                        break;
                                    case "String":
                                        cmpControl.Type = ValidationDataType.String;
                                        break;
                                }
                                tdTextBox.Controls.Add(cmpControl);
                            }

                            //add new row with label and text box
                            trData.Controls.Add(tdLabel);
                            trData.Controls.Add(tdTextBox);

                            // add new row to table
                            tbData.Controls.Add(trData);

                            //  End code
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Assign Campaign Data To Controls
        /// </summary>
        private void AssignCampaignFieldsDataToControls()
        {
            
            try
            {
                GetCampaignDetails();
                
                DataSet dsCampaignDtls = (DataSet)Session["CampaignDtls"]; // campaign data
                DataSet dsCampaignFields = (DataSet)Session["CampaignFields"]; // campaign fields

                hdnUniqueKey.Value = dsCampaignDtls.Tables[0].Rows[0]["UniqueKey"].ToString();

                string strFieldName = string.Empty;
                string strFieldValue = string.Empty;
                string strFieldType = string.Empty;
                StringBuilder sbCampaignFieldsQuery = new StringBuilder();
                
                for (int i = 0; i < dsCampaignFields.Tables[0].Rows.Count; i++)
                {
                
                    strFieldName = dsCampaignFields.Tables[0].Rows[i]["FieldName"].ToString();  // Field Name
                    strFieldType = dsCampaignFields.Tables[0].Rows[i]["FieldType"].ToString(); // Field Type
                
                    strFieldValue = dsCampaignDtls.Tables[0].Rows[0][strFieldName].ToString(); // Campaign Data
                    txtControl = (TextBox)tbData.FindControl("txt" + strFieldName);


                    if (strFieldType == "Date")
                    {
                        txtControl.Attributes["onblur"] = "javascript:if(this.value != '') return checkDate(this);";
                        txtControl.Text = strFieldValue == string.Empty ? "" : Convert.ToDateTime(strFieldValue).ToString("d");
                    }
                    else
                    {
                        txtControl.Text = strFieldValue;
                    }
                
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Build Update Script For Updating Campaign details
        /// </summary>
        /// <returns></returns>
        private string BuildCampaignFieldsScript()
        {
            
            DataSet dsCampaignFields = (DataSet)Session["CampaignFields"]; // campaign fields
            
            string strFieldName = string.Empty;
            string strFieldValue = string.Empty;
            string strFieldType = string.Empty;
            StringBuilder sbCampaignFieldsQuery = new StringBuilder();

            try
            {

                sbCampaignFieldsQuery.Append("UPDATE Campaign SET ");

                for (int i = 0; i < dsCampaignFields.Tables[0].Rows.Count; i++)
                {
                    strFieldName = dsCampaignFields.Tables[0].Rows[i]["FieldName"].ToString();
                    strFieldType = dsCampaignFields.Tables[0].Rows[i]["FieldType"].ToString(); // Field Type

                    txtControl = (TextBox)tbData.FindControl("txt" + strFieldName);

                    strFieldValue = txtControl.Text.Trim().Replace("'", "''");

                    sbCampaignFieldsQuery.AppendFormat("{0}={1}{2}", strFieldName, strFieldType == "Date" && string.IsNullOrEmpty(strFieldValue) ? "null" : string.Format("'{0}'", strFieldValue), i == dsCampaignFields.Tables[0].Rows.Count - 1 ? "" : ",");
                }

                sbCampaignFieldsQuery.AppendFormat(" WHERE {0}={1}", "UniqueKey", hdnUniqueKey.Value);
            }
            catch (Exception ex)
            {
               
                PageMessage = ex.Message;
            }

            return sbCampaignFieldsQuery.ToString();
        }


        private void DisposeCall(string strResultCodeName)
        {
            
            int resultcodeID = 0;
            foreach (ListItem li in lbxCallDisposition.Items)
            {
                if (strResultCodeName == li.Text)
                {
                    resultcodeID = Convert.ToInt32(li.Value);
                    break;
                }
            }

            bool isManualDial = false;
            try
            {
                if (Request.QueryString["pagefrom"].ToString() == "ManualDial")
                {
                    isManualDial = true;
                }
            }
            catch {}
            
            if (resultcodeID > 0)
            {
               
                DisposeCall(resultcodeID, isManualDial, strResultCodeName);
                
            }
            else
            {
                    Response.Write("#ERROR:NORESULTCODEEXIST#");
            }
        }

        private void SetResultCode()
        {
           
            try
            {
                DataSet dsCampaigndtls = (DataSet)Session["CampaignDtls"];
                string resultcode = dsCampaigndtls.Tables[0].Rows[0]["CallResultCode"].ToString();
                lbxCallDisposition.SelectedValue = resultcode;
            }
            catch(Exception) {
                
                          
            }
        }

        #endregion
    }
}
