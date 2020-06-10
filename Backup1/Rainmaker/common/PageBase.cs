using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Rainmaker.Common.DomainModel;
using System.Xml;
using System.Collections.Generic;
using Rainmaker.Web.CampaignWS;
using Rainmaker.Web.AgentsWS;
using System.Text;
using System.Data.OleDb;
using Rainmaker.Common;


namespace Rainmaker.Web
{
    /// <summary>
    /// Summary description for PageBase
    /// </summary>
    public class PageBase : Page
    {
        public PageBase()
        {

        }

        private string strPageMessage = string.Empty;
        public string PageMessage
        {
            get { return strPageMessage; }
            set
            {
                if (value.IndexOf("Server was unable to process request") > 0)
                {
                    strPageMessage = "There was a problem communicating with backend services.\r\nPlease try your request again now.\r\nIf the problem persists, inform your administrator to examine the site and web service logs to determine the problem.";
                }
                else
                {
                    strPageMessage = value;
                }
            }
        }

        private string strConfirmMessage = string.Empty;
        public string ConfirmMessage
        {
            get { return strConfirmMessage; }
            set { strConfirmMessage = value; }
        }
        private string strConfirmMessage2 = string.Empty;
        public string ConfirmMessage2
        {
            get { return strConfirmMessage2; }
            set { strConfirmMessage2 = value; }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            SetPageMessge();
            SetConfirmMessge();
            SetConfirmMessge2();
            base.OnPreRender(e);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (this.Session["LoggedAgent"] == null && Page.ToString().ToLower().IndexOf("default_aspx") == -1)
            {
                Response.Redirect("../Default.aspx");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //Check if the current session has login user info and 
        //redirect to Login page if it does not exist.

        //}

        /// <summary>
        /// This method declares the page message variable
        /// in the javascript ands sets the page message
        /// </summary>
        public void SetPageMessge()
        {
            ClientScriptManager cs = base.ClientScript;
            cs.RegisterHiddenField("PageMessage", PageMessage);
            PageMessage = string.Empty;
        }
        public static string EncodeJsString(string s)
        {
            StringBuilder sb = new StringBuilder();
            
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            

            return sb.ToString();
        }

        // Added GW 1.11.11
        public void SetConfirmMessge()
        {
            ClientScriptManager cs = base.ClientScript;
            cs.RegisterHiddenField("ConfirmMessage", ConfirmMessage);
            ConfirmMessage = string.Empty;
            
            
        }
        public void SetConfirmMessge2()
        {
            ClientScriptManager cs = base.ClientScript;
            cs.RegisterHiddenField("ConfirmMessage2", ConfirmMessage2);
            ConfirmMessage2 = string.Empty;
        }
        /// <summary>
        /// This method binds scripts data to the
        /// dropdown feilds
        /// 
        /// </summary>
        protected void BindScripts(DropDownList ddlScripts, Campaign objCampaign, string strScriptName)
        {
            DataSet dsScripts;
            try
            {
                objCampaign = (Campaign)Session["Campaign"];
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsScripts = objCampService.GetScriptList(xDocCampaign);
                ddlScripts.DataSource = dsScripts;
                ddlScripts.DataTextField = "ScriptName";
                ddlScripts.DataValueField = "ScriptID";
                ddlScripts.DataBind();
                ddlScripts.Items.Insert(0, (new ListItem("Select" + " " + strScriptName, "0")));
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }


        ////
        protected string AppendQuotes(string strvalue)
        {
            //  *** GW - Attempt to fix empty fields, subbing a space.  Preventing problems with repeating quotes - 
            if (strvalue.Length < 1)
            {
                return "\" \"";
            }
            return "\"" + strvalue + "\"";
        }

        /// <summary>
        /// Bind result codes
        /// </summary>
        protected DataView BindResultCodes(bool isVerificationAgent)
        {
            DataSet dsResultCodes;
            DataView dvResultCodes;

            try
            {
                CampaignService objCampService = new CampaignService();
                Campaign objCampaign = (Campaign)Session["Campaign"];
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                dsResultCodes = objCampService.GetResultCodes(xDocCampaign);
                dvResultCodes = dsResultCodes.Tables[0].DefaultView;
                if (isVerificationAgent)
                    dvResultCodes = FilterData(dsResultCodes.Tables[0], "DateDeleted is null");
                else
                    dvResultCodes = FilterData(dsResultCodes.Tables[0], "DateDeleted is null AND VerifyOnly = 0");

                return dvResultCodes;
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
            return null;
        }

        /// <summary>
        /// Get Current Campaign fields for current campaign(string, money, integer, etc) and field types from rainmaker master db.
        /// </summary>
        protected void GetCampaignFields()
        {
            if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];

                DataSet dsCampaignFields;
                try
                {
                    CampaignService objCampaignService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                    dsCampaignFields = objCampaignService.GetCampaignFields(xDocCampaign);
                    Session["CampaignFields"] = dsCampaignFields;
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
            }
        }

        /// <summary>
        /// Assign Campaign Data To Text Fields, return javascript data
        /// </summary>
        protected string AssignCampaignFieldsDataToTextFields(DataSet dsCampaignDtls, Literal ltrlScriptBody)
        {
            //if (ltrlScriptBody.Text.ToLower().IndexOf("<textarea ") >= 0)
            //{
            //    ParseTextAreaValues(ltrlScriptBody);
            //}

            string jscpt = "";
            try
            {
                DataSet dsCampaignFields = (DataSet)Session["CampaignFields"]; // campaign fields

                string strFieldName = string.Empty;
                string strFieldValue = string.Empty;
                string strFieldType = string.Empty;
                string strFieldReadOnly = string.Empty;
                bool blnReadOnly = false;

                for (int i = 0; i < dsCampaignFields.Tables[0].Rows.Count; i++)
                {
                    strFieldName = dsCampaignFields.Tables[0].Rows[i]["FieldName"].ToString();  // Field Name
                    strFieldType = dsCampaignFields.Tables[0].Rows[i]["FieldType"].ToString(); // Field Type
                    strFieldReadOnly = dsCampaignFields.Tables[0].Rows[i]["ReadOnly"].ToString(); // Field Type

                    try
                    {
                        blnReadOnly = Convert.ToBoolean(strFieldReadOnly);
                    }
                    catch
                    {
                        blnReadOnly = false;
                    }

                    strFieldValue = Server.HtmlEncode(dsCampaignDtls.Tables[0].Rows[0][strFieldName].ToString()); // Campaign Data

                    if (strFieldType.ToLower() == "date")
                    {
                        if (strFieldValue != string.Empty)
                        {
                            try
                            {
                                strFieldValue = Convert.ToDateTime(strFieldValue).ToString("MM/dd/yyyy HH:mm:ss");
                            }
                            catch
                            {
                                strFieldValue = "";
                            }
                        }
                    }
                    else if (strFieldType.ToLower() == "boolean")
                    {
                        bool isCheck = false;
                        try
                        {
                            isCheck = Convert.ToBoolean(strFieldValue);
                        }
                        catch { }
                        if (isCheck)
                        {
                            if (ltrlScriptBody.Text.ToLower().IndexOf(strFieldName.ToLower()) > 0)
                            {

                                jscpt += string.Format("SelectCheckbox('{0}');{1}", strFieldName.ToLower(), Environment.NewLine);
                            }
                        }
                    }

                    if (ltrlScriptBody.Text.ToLower().IndexOf("<input type=radio") > 0)
                    {
                        if (strFieldValue != "")
                        {
                            jscpt += string.Format("CheckRadioButton('{0}', '{1}');{2}", strFieldName.ToLower(), strFieldValue, Environment.NewLine);
                        }
                    }
                    if (ltrlScriptBody.Text.ToLower().IndexOf("<select") > 0)
                    {
                        
                            if (strFieldValue != "")
                            {
                                strFieldValue = strFieldValue.Replace("'", "&apos;");

                                jscpt += string.Format("SelectDropDown('{0}', '{1}');{2}", strFieldName.ToLower(), strFieldValue, Environment.NewLine);
                            }
                       
                    }
                    if ((strFieldType == "Encrypted") && (strFieldValue != ""))
                    {
                        strFieldValue = "This is encrypted data";
                    }
                    ltrlScriptBody.Text = ltrlScriptBody.Text.Replace(string.Format("{0}{1}{2}", "##", strFieldName, "##"), strFieldValue);
                    ltrlScriptBody.Text = ltrlScriptBody.Text.Replace(string.Format("{0}{1}{2}", "#", strFieldName, "#"), AppendQuotes(strFieldValue));
                    
                }

                foreach (string field in GetDefaultCampaignFields())
                {
                    //strFieldName = dsCampaignFields.Tables[0].Rows[i]["FieldName"].ToString();  // Field Name
                    //strFieldType = dsCampaignFields.Tables[0].Rows[i]["FieldType"].ToString(); // Field Type

                    strFieldValue = dsCampaignDtls.Tables[0].Rows[0][field].ToString(); // Campaign Data

                    if (field == "DateTimeofCall" || field == "CallSenttoDialTime" || field == "CalltoAgentTime" ||
                        field == "CallHangupTime" || field == "CallCompletionTime" || field == "CallWrapUpStartTime" ||
                        field == "CallWrapUpStopTime" || field == "ResultCodeSetTime" || field == "ScheduleDate")
                    {
                        if (strFieldValue != string.Empty)
                        {
                            try
                            {
                                strFieldValue = Convert.ToDateTime(strFieldValue).ToString("MM/dd/yyyy HH:mm:ss");
                            }
                            catch
                            {
                                strFieldValue = "";
                            }
                        }
                    }

                    ltrlScriptBody.Text = ltrlScriptBody.Text.Replace(string.Format("{0}{1}{2}", "##", field, "##"), strFieldValue);
                    ltrlScriptBody.Text = ltrlScriptBody.Text.Replace(string.Format("{0}{1}{2}", "#", field, "#"), AppendQuotes(strFieldValue));

                    if (ltrlScriptBody.Text.ToLower().IndexOf("<input type=radio") > 0)
                    {
                        if (strFieldValue != "")
                        {
                            jscpt += string.Format("CheckRadioButton('{0}', '{1}');{2} ", field.ToLower(), strFieldValue, Environment.NewLine);
                        }
                    }
                }

                try
                {
                    string campname = ((Campaign)Session["Campaign"]).ShortDescription;
                    string agentName = ((Agent)Session["LoggedAgent"]).AgentName;
                    ltrlScriptBody.Text = ltrlScriptBody.Text.Replace(string.Format("{0}{1}{2}", "#", "campaignname", "#"), AppendQuotes(campname));
                    ltrlScriptBody.Text = ltrlScriptBody.Text.Replace(string.Format("{0}{1}{2}", "#", "agentname", "#"), AppendQuotes(agentName));
                }
                catch { }

            }
            catch
            {
                //PageMessage = ex.Message;
            }
            ltrlScriptBody.Text = ltrlScriptBody.Text.Replace("h_r_ef=", "onclick=");
            ltrlScriptBody.Text = ltrlScriptBody.Text.Replace("_o_n_b_lur=", "onblur=");
            AssignFieldsDataToDropdowns(dsCampaignDtls, ltrlScriptBody);
            // MB GW fix on 09.15.10 - Updated 10.17.10
            if (ltrlScriptBody.Text.Contains("\"\""))
            {
                //ErrorLogger.Write("Anomylous doubled quotes field wrapper quotes detected in script for campaign " + ((Campaign)Session["Campaign"]).ShortDescription);
                //ErrorLogger.Write(ltrlScriptBody.Text);
                ltrlScriptBody.Text = ltrlScriptBody.Text.Replace("\"\"", "\"");
            }
            if (ltrlScriptBody.Text.Contains("\" \""))
            {
                //ErrorLogger.Write("Anomylous space and value quotes field wrapper quotes detected in script for campaign " + ((Campaign)Session["Campaign"]).ShortDescription);
                //ErrorLogger.Write(ltrlScriptBody.Text);
                ltrlScriptBody.Text = ltrlScriptBody.Text.Replace("\" \"", "\"\"");
            }

            //ErrorLogger.Write("Cleaned:" + ltrlScriptBody.Text);

            return jscpt;
        }
        /*
                private void ParseTextAreaValues(Literal ltrlScriptBody)
                {
                    string strText = ltrlScriptBody.Text.Replace("<textarea", "<TEXTAREA").Replace("</textarea", "</TEXTAREA");
                    try
                    {
                        while (strText.IndexOf("<TEXTAREA ") > 0)
                        {
                            int tagStartindex = strText.IndexOf("<TEXTAREA ");
                            int tagEndIndex = strText.IndexOf("</TEXTAREA>", tagStartindex);
                    
                            if (tagStartindex < tagEndIndex)
                            {


                                strText = strText.Insert(tagStartindex+1, "##");

                                string tagValue = strText.Substring(tagStartindex, tagEndIndex - tagStartindex+2);
                                if (tagValue.IndexOf("r_textareavalue=") > 0)
                                {
                                    string newTagValue = tagValue;
                                    int valueStartIndex = newTagValue.IndexOf("#s#") + 3;
                                    int valueEndIndex = newTagValue.IndexOf("#e#");

                                    if (valueStartIndex < valueEndIndex)
                                    {
                                        string value = newTagValue.Substring(valueStartIndex, valueEndIndex - valueStartIndex);
                                        newTagValue = newTagValue.Replace("r_textareavalue=\"#s#" + value + "#e#\"", "");
                                        newTagValue = newTagValue + "##" + value + "##";
                                        strText = strText.Replace(tagValue, newTagValue);
                                    }
                                    else return;
                                }
                            }
                            else
                                return;
                        }
                        strText = strText.Replace("<##TEXTAREA ", "<TEXTAREA ");
                    }
                    catch { return; }
                    ltrlScriptBody.Text = strText;
                }
        */
        protected void AssignFieldsDataToDropdowns(DataSet dsCampaignDtls, Literal ltrlScriptBody)
        {
            try
            {

                DataSet dsCampaignFields = (DataSet)Session["CampaignFields"]; // campaign fields

                string strFieldName = string.Empty;
                string strFieldValue = string.Empty;
                string strFieldType = string.Empty;
                System.Collections.ArrayList alFields = new System.Collections.ArrayList();

                for (int i = 0; i < dsCampaignFields.Tables[0].Rows.Count; i++)
                {
                    alFields.Add(dsCampaignFields.Tables[0].Rows[i]["FieldName"].ToString().ToLower());
                }

                foreach (string field in GetDefaultCampaignFields())
                {
                    alFields.Add(field.ToLower());
                }

                // Parse script and update dropdown selection
                string script = ltrlScriptBody.Text;
                int loop = 0;
                while (script.ToLower().IndexOf("<select") > 0 && loop < 100)
                {
                    string script_lower = script.ToLower();
                    int selectIndexStart = script_lower.IndexOf("<select");
                    int selectIndexEnd = script_lower.IndexOf(">", selectIndexStart);
                    int selectTagEnd = script_lower.IndexOf("</select>", selectIndexStart);

                    if (selectIndexEnd > selectIndexStart && selectTagEnd > selectIndexEnd)
                    {
                        string strFieldId = script.Substring(selectIndexStart + 13, selectIndexEnd - (selectIndexStart + 13)).Trim().ToLower();
                        int FieldNameStart = strFieldId.IndexOf('"') + 1;

                        string FieldName = strFieldId.Substring(FieldNameStart, strFieldId.Length - FieldNameStart);
                        int FieldNameEnd = FieldName.IndexOf('"');
                        FieldName = FieldName.Substring(0, FieldNameEnd);
                        if (alFields.Contains(FieldName))
                        {
                            string fldValue = "";
                            if (dsCampaignDtls.Tables[0].Rows[0][FieldName] != DBNull.Value)
                                fldValue = dsCampaignDtls.Tables[0].Rows[0][FieldName].ToString();

                            string strSelectTag = script.Substring(selectIndexStart, (selectTagEnd - selectIndexStart) + 9);
                            string strNewTag = strSelectTag;
                            strNewTag = strNewTag.Replace("selected=\"selected\" ", "").Replace(" SELECTED=\"SELECTED\" ", "");

                            if (fldValue == "" || fldValue.IndexOf(" ") > 0)
                            {
                                fldValue = "\"" + fldValue + "\"";
                            }

                            int optionIndex = strNewTag.ToLower().IndexOf("<option value=\"" + fldValue.ToLower());
                            if (optionIndex > 0)
                            {
                                strNewTag = strNewTag.Insert(optionIndex + 8, "selected=\"selected\" ");
                            }
                            script = script.Replace(strSelectTag, strNewTag);
                        }
                    }
                    script = script.Insert(selectIndexStart + 1, "#tempsel#");
                    loop++;
                }
                script = script.Replace("#tempsel#", "");
                ltrlScriptBody.Text = script;
            }
            catch
            {
                //PageMessage = ex.Message;
            }
        }

        public string[] GetDefaultCampaignFields()
        {
            string[] str = new string[] { "DBCompany", "VerificationAgentID","DateTimeofCall",
                "CallDuration","CallSenttoDialTime","CalltoAgentTime","CallHangupTime","CallCompletionTime",
                "CallWrapUpStartTime","CallWrapUpStopTime","ResultCodeSetTime","TotalNumAttempts","NumAttemptsAM",
                "NumAttemptsWkEnd","NumAttemptsPM","LeadProcessed","FullQueryPassCount","ScheduleDate","ScheduleNotes"};
            return str;
        }

        private Dictionary<string, string> dicCampaignFields = null;
        public Dictionary<string, string> GetDefaultCampaignFields_SE()
        {
            if (dicCampaignFields == null)
            {
                dicCampaignFields = new Dictionary<string, string>();
                dicCampaignFields.Add("DBCompany", "255");
                dicCampaignFields.Add("VerificationAgentID", "255");
                dicCampaignFields.Add("DateTimeofCall", "-1");
                dicCampaignFields.Add("CallDuration", "-1");
                dicCampaignFields.Add("CallSenttoDialTime", "-1");
                dicCampaignFields.Add("CalltoAgentTime", "-1");
                dicCampaignFields.Add("CallHangupTime", "-1");
                dicCampaignFields.Add("CallCompletionTime", "-1");
                dicCampaignFields.Add("CallWrapUpStartTime", "-1");
                dicCampaignFields.Add("CallWrapUpStopTime", "-1");
                dicCampaignFields.Add("ResultCodeSetTime", "-1");
                dicCampaignFields.Add("TotalNumAttempts", "-1");
                dicCampaignFields.Add("NumAttemptsAM", "-1");
                dicCampaignFields.Add("NumAttemptsWkEnd", "-1");
                dicCampaignFields.Add("NumAttemptsPM", "-1");
                dicCampaignFields.Add("LeadProcessed", "-1");
                dicCampaignFields.Add("FullQueryPassCount", "-1");
                dicCampaignFields.Add("ScheduleDate", "-1");
                dicCampaignFields.Add("ScheduleNotes", "5000");
            }
            return dicCampaignFields;
        }


        ///
        /// <summary>
        /// log off campaign which is logged in
        /// </summary>
        protected void LogOffAgentStat()
        {
            Campaign objCampaign;
            CampaignService objCampService = new CampaignService();
            AgentStat objAgentStat;
            XmlDocument xDocAgentStat = new XmlDocument();
            XmlDocument xDocCampaign = new XmlDocument();

            if (Session["AgentStat"] != null) // agent is already logged in campaign
            {
                objCampaign = (Campaign)Session["Campaign"];

                objAgentStat = (AgentStat)Session["AgentStat"];
                objAgentStat.LogOffDate = DateTime.Now;
                objAgentStat.LogOffTime = DateTime.Now;

                xDocAgentStat.LoadXml(Serialize.SerializeObject(objAgentStat, "AgentStat"));
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                objAgentStat = (AgentStat)Serialize.DeserializeObject(
                objCampService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat), "AgentStat");
            }
        }

        protected void LogOffAgentStat(Agent agent)
        {
            Campaign campaign = (Campaign)Session["Campaign"];

            // We are resetting agent from admin console, so agent status is not available in session.
            // (current session AgentStat is for the logged administrator, not the target agent to be reset)
            // Create an agent status object to complete logoff from campaign.

            AgentStat agentStat = new AgentStat();
            agentStat.AgentID = agent.AgentID;
            agentStat.StatusID = agent.AgentStatusID;
            agentStat.LoginDate = agent.LoginTime;
            agentStat.LoginTime = agent.LoginTime;
            agentStat.LogOffDate = DateTime.Now;
            agentStat.LogOffTime = DateTime.Now;

            XmlDocument xDocCampaign = new XmlDocument();
            xDocCampaign.LoadXml(Serialize.SerializeObject(campaign, "Campaign"));
            XmlDocument xDocAgentStat = new XmlDocument();
            xDocAgentStat.LoadXml(Serialize.SerializeObject(agentStat, "AgentStat"));

            CampaignService campaignService = new CampaignService();

            agentStat = (AgentStat)Serialize.DeserializeObject
            (
                campaignService.InsertUpdateAgentStat(xDocCampaign, xDocAgentStat),
                "AgentStat"
            );
        }

        /// <summary>
        /// Binding Values to the dropdown
        /// </summary>
        /// <param name="intMinValue"></param>
        /// <param name="MaxValue"></param>
        /// <param name="ddlDropDown"></param>
        protected void BindDropDown(int intMinValue, int MaxValue, DropDownList ddlDropDown)
        {
            ddlDropDown.Items.Clear();
            try
            {
                for (int count = intMinValue; count <= MaxValue; count++)
                {
                    ddlDropDown.Items.Add(count.ToString());
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Binding Values to the dropdown
        /// </summary>
        /// <param name="intMinValue"></param>
        /// <param name="intMaxValue"></param>
        /// <param name="ddlDropDown"></param>
        protected void BindDialingTime(int intMinValue, int MaxValue, DropDownList ddlDropDown)
        {
            ddlDropDown.Items.Clear();
            try
            {
                for (int count = intMinValue; count <= MaxValue; count++)
                {
                    ddlDropDown.Items.Add(count < 10 ? "0" + count.ToString() : count.ToString());
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Adds database(Campaign) colums to dropdown list
        /// </summary>
        /// <param name="ddl"></param>
        protected void BindColumnsDropdown(DropDownList ddl, ListItem DefaultItem)
        {
            ddl.Items.Clear();
            if (DefaultItem != null)
                ddl.Items.Add(DefaultItem);
            if (DefaultItem.Text != "Select Criteria")
                ddl.Items.Add(new ListItem("Campaign", "Campaign:Integer"));
            ddl.Items.Add(new ListItem("PhoneNum", "PhoneNum:String"));
            ddl.Items.Add(new ListItem("DBCompany", "DBCompany:String"));
            ddl.Items.Add(new ListItem("NeverCallFlag", "NeverCallFlag:Integer"));
            ddl.Items.Add(new ListItem("AgentID", "AgentID:String"));
            ddl.Items.Add(new ListItem("VerificationAgent", "VerificationAgentID:String"));
            ddl.Items.Add(new ListItem("CallResultCode", "CallResultCode:Integer"));
            ddl.Items.Add(new ListItem("DateTimeofCall", "DateTimeofCall:date"));
            ddl.Items.Add(new ListItem("CallDuration", "CallDuration:String"));
            ddl.Items.Add(new ListItem("CallSenttoDialTime", "CallSenttoDialTime:date"));
            ddl.Items.Add(new ListItem("CalltoAgentTime", "CalltoAgentTime:date"));
            ddl.Items.Add(new ListItem("CallHangupTime", "CallHangupTime:date"));
            ddl.Items.Add(new ListItem("CallCompletionTime", "CallCompletionTime:date"));
            ddl.Items.Add(new ListItem("CallWrapUpStartTime", "CallWrapUpStartTime:date"));
            ddl.Items.Add(new ListItem("CallWrapUpStopTime", "CallWrapUpStopTime:date"));
            ddl.Items.Add(new ListItem("ResultCodeSetTime", "ResultCodeSetTime:date"));
            ddl.Items.Add(new ListItem("TotalNumAttempts", "TotalNumAttempts:String"));
            ddl.Items.Add(new ListItem("NumAttemptsAM", "NumAttemptsAM:String"));
            ddl.Items.Add(new ListItem("NumAttemptsWkEnd", "NumAttemptsWkEnd:String"));
            ddl.Items.Add(new ListItem("NumAttemptsPM", "NumAttemptsPM:String"));
            ddl.Items.Add(new ListItem("LeadProcessed", "LeadProcessed:String"));
            ddl.Items.Add(new ListItem("FirstName", "FIRSTNAME:String"));
            ddl.Items.Add(new ListItem("LastName", "LASTNAME:String"));
            ddl.Items.Add(new ListItem("Address", "ADDRESS:String"));
            ddl.Items.Add(new ListItem("City", "CITY:String"));
            ddl.Items.Add(new ListItem("State", "STATE:String"));
            ddl.Items.Add(new ListItem("Zip", "ZIP:String"));
            ddl.Items.Add(new ListItem("Address2", "ADDRESS2:String"));
            //ddl.Items.Add(new ListItem("Country", "COUNTRY:String"));
            ddl.Items.Add(new ListItem("FullQueryPassCount", "FullQueryPassCount:Integer"));
            ddl.Items.Add(new ListItem("ScheduleDate", "scheduledate:date"));
            ddl.Items.Add(new ListItem("ScheduleNotes", "schedulenotes:String"));
            ddl.Items.Add(new ListItem("DateTimeofImport", "DateTimeofImport:date"));
        }

        /// <summary>
        /// Adds database(Campaign) colums to dropdown list
        /// </summary>
        /// <param name="ddl"></param>
        protected void BindColumnsDropdownDNC(DropDownList ddl, ListItem DefaultItem)
        {
            ddl.Items.Clear();
            if (DefaultItem != null)
                ddl.Items.Add(DefaultItem);
            if (DefaultItem.Text != "Select Criteria")
                //ddl.Items.Add(new ListItem("Campaign", "Campaign:Integer"));
                ddl.Items.Add(new ListItem("PhoneNum", "PhoneNum:String"));
            /*ddl.Items.Add(new ListItem("DBCompany", "DBCompany:String"));
            ddl.Items.Add(new ListItem("NeverCallFlag", "NeverCallFlag:Integer"));
            ddl.Items.Add(new ListItem("AgentID", "AgentID:String"));
            ddl.Items.Add(new ListItem("VerificationAgent", "VerificationAgentID:String"));
            ddl.Items.Add(new ListItem("CallResultCode", "CallResultCode:Integer"));
            ddl.Items.Add(new ListItem("DateTimeofCall", "DateTimeofCall:date"));
            ddl.Items.Add(new ListItem("CallDuration", "CallDuration:String"));
            ddl.Items.Add(new ListItem("CallSenttoDialTime", "CallSenttoDialTime:date"));
            ddl.Items.Add(new ListItem("CalltoAgentTime", "CalltoAgentTime:date"));
            ddl.Items.Add(new ListItem("CallHangupTime", "CallHangupTime:date"));
            ddl.Items.Add(new ListItem("CallCompletionTime", "CallCompletionTime:date"));
            ddl.Items.Add(new ListItem("CallWrapUpStartTime", "CallWrapUpStartTime:date"));
            ddl.Items.Add(new ListItem("CallWrapUpStopTime", "CallWrapUpStopTime:date"));
            ddl.Items.Add(new ListItem("ResultCodeSetTime", "ResultCodeSetTime:date"));
            ddl.Items.Add(new ListItem("TotalNumAttempts", "TotalNumAttempts:String"));
            ddl.Items.Add(new ListItem("NumAttemptsAM", "NumAttemptsAM:String"));
            ddl.Items.Add(new ListItem("NumAttemptsWkEnd", "NumAttemptsWkEnd:String"));
            ddl.Items.Add(new ListItem("NumAttemptsPM", "NumAttemptsPM:String"));
            ddl.Items.Add(new ListItem("LeadProcessed", "LeadProcessed:String"));
            ddl.Items.Add(new ListItem("FirstName", "FIRSTNAME:String"));
            ddl.Items.Add(new ListItem("LastName", "LASTNAME:String"));
            ddl.Items.Add(new ListItem("Address", "ADDRESS:String"));
            ddl.Items.Add(new ListItem("City", "CITY:String"));
            ddl.Items.Add(new ListItem("State", "STATE:String"));
            ddl.Items.Add(new ListItem("Zip", "ZIP:String"));
            ddl.Items.Add(new ListItem("Address2", "ADDRESS2:String"));
            //ddl.Items.Add(new ListItem("Country", "COUNTRY:String"));
            ddl.Items.Add(new ListItem("FullQueryPassCount", "FullQueryPassCount:Integer"));
            ddl.Items.Add(new ListItem("ScheduleDate", "scheduledate:date"));
            ddl.Items.Add(new ListItem("ScheduleNotes", "schedulenotes:String"));
            ddl.Items.Add(new ListItem("DateTimeofImport", "DateTimeofImport:date"));
             */
        }

        /// <summary>
        /// Bind Operator
        /// 
        /// </summary>
        ///
        /*
        2012-06-12 Dave Pollastrini
        Overloaded BindOperator to take dataType.
        */
        protected void BindOperator(DropDownList ddlOperator, string dataType)
        {
            ddlOperator.Items.Clear();

            switch (dataType)
            {
                case "boolean":
                    ddlOperator.Items.Add(new ListItem("Select Operator", "0"));
                    ddlOperator.Items.Add(new ListItem("Is True",  "{0} <> 0"));
                    ddlOperator.Items.Add(new ListItem("Is False", "{0} = 0"));

                    break;
                case "integer":
                case "date":
                    ddlOperator.Items.Add(new ListItem("Select Operator", "0"));
                    ddlOperator.Items.Add(new ListItem("=", "{0} = '{1}'"));
                    ddlOperator.Items.Add(new ListItem("<>", "{0} <> '{1}'"));
                    ddlOperator.Items.Add(new ListItem(">", "{0} > '{1}'"));
                    ddlOperator.Items.Add(new ListItem("<", "{0} < '{1}'"));
                    ddlOperator.Items.Add(new ListItem(">=", "{0} >= '{1}'"));
                    ddlOperator.Items.Add(new ListItem("<=", "{0} <= '{1}'"));

                    ddlOperator.Items.Add(new ListItem("Is Null", "{0} Is Null {1}"));
                    ddlOperator.Items.Add(new ListItem("Is Not Null", "{0} Is Not Null {1}"));

                    break;
                    
                default:
                    ddlOperator.Items.Add(new ListItem("Select Operator", "0"));
                    ddlOperator.Items.Add(new ListItem("=", "{0} = '{1}'"));
                    ddlOperator.Items.Add(new ListItem("<>", "{0} <> '{1}'"));
                    ddlOperator.Items.Add(new ListItem(">", "{0} > '{1}'"));
                    ddlOperator.Items.Add(new ListItem("<", "{0} < '{1}'"));
                    ddlOperator.Items.Add(new ListItem(">=", "{0} >= '{1}'"));
                    ddlOperator.Items.Add(new ListItem("<=", "{0} <= '{1}'"));

                    ddlOperator.Items.Add(new ListItem("Contains", "{0} LIKE '%{1}%'"));
                    ddlOperator.Items.Add(new ListItem("Does Not Contain", "{0} NOT LIKE '%{1}%'"));
                    ddlOperator.Items.Add(new ListItem("BeginsWith", "{0} LIKE '{1}%'"));
                    ddlOperator.Items.Add(new ListItem("EndsWith", "{0} LIKE '%{1}'"));

                    ddlOperator.Items.Add(new ListItem("Is Null", "{0} Is Null {1}"));
                    ddlOperator.Items.Add(new ListItem("Is Not Null", "{0} Is Not Null {1}"));
                    break;
            }
        }

        /* 2012-06-12 Dave Pollastrini
        Obsolete BindOperator overload.  New function takes a datatype instead of just isDateField
        */
        /*
        protected void BindOperator(DropDownList ddlOperator, bool isDateField)
        {
            ddlOperator.Items.Clear();
            ddlOperator.Items.Add(new ListItem("Select Operator", "0"));
            ddlOperator.Items.Add(new ListItem("=", "{0} = '{1}'"));
            ddlOperator.Items.Add(new ListItem("<>", "{0} <> '{1}'"));
            ddlOperator.Items.Add(new ListItem(">", "{0} > '{1}'"));
            ddlOperator.Items.Add(new ListItem("<", "{0} < '{1}'"));
            ddlOperator.Items.Add(new ListItem(">=", "{0} >= '{1}'"));
            ddlOperator.Items.Add(new ListItem("<=", "{0} <= '{1}'"));
            if (!isDateField)
            {
                ddlOperator.Items.Add(new ListItem("Contains", "{0} LIKE '%{1}%'"));
                ddlOperator.Items.Add(new ListItem("Does Not Contain", "{0} NOT LIKE '%{1}%'"));
                ddlOperator.Items.Add(new ListItem("BeginsWith", "{0} LIKE '{1}%'"));
                ddlOperator.Items.Add(new ListItem("EndsWith", "{0} LIKE '%{1}'"));
            }
            //ddlOperator.Items.Add(new ListItem("Greater Than", "{0} > '{1}'"));
            //ddlOperator.Items.Add(new ListItem("Less than", "{0} < '{1}'"));
            //ddlOperator.Items.Add(new ListItem("Greater Than Equal", "{0} >= '{1}'"));
            //ddlOperator.Items.Add(new ListItem("Less than Equal", "{0} <= '{1}'"));
            //ddlOperator.Items.Add(new ListItem("Is Null", "{0} Is Null {1}"));
            //ddlOperator.Items.Add(new ListItem("Is Not Null", "{0} Is Not Null {1}"));
            //ddlOperator.Items.Add(new ListItem("Does Not Equal", "{0} <> '{1}'"));

            ddlOperator.Items.Add(new ListItem("Is Null", "{0} Is Null {1}"));
            ddlOperator.Items.Add(new ListItem("Is Not Null", "{0} Is Not Null {1}"));

        }
        */

        /// <summary>
        /// Bind Logical Operator
        /// 
        /// </summary>
        protected void BindLogicalOperator(DropDownList ddlLogicalOperator)
        {
            ddlLogicalOperator.Items.Clear();
            ddlLogicalOperator.Items.Add(new ListItem("And", "And"));
            ddlLogicalOperator.Items.Add(new ListItem("OR", "OR"));
        }

        /// <summary>
        /// Bind Query Condition
        /// 
        /// </summary>
        protected string BuildQueryCondition(string strQueryCondition)
        {
            string includeAmPmAttempts = "no";
            StringBuilder sbQuery = new StringBuilder();
            //sbQuery.Append("SELECT UniqueKey, Campaign, PhoneNum, DBCompany, ");
            //sbQuery.Append("NeverCallFlag, AgentID, VerificationAgentID, CallResultCode,");
            //sbQuery.Append("DateTimeofCall, CallDuration, CallSenttoDialTime, ");
            //sbQuery.Append("CalltoAgentTime, CallHangupTime, CallCompletionTime, ");
            //sbQuery.Append("CallWrapUpStartTime, CallWrapUpStopTime, ResultCodeSetTime, ");
            //sbQuery.Append("TotalNumAttempts, NumAttemptsAM, NumAttemptsWkEnd, NumAttemptsPM, LeadProcessed, ");
            //sbQuery.Append("NAME, ADDRESS, CITY, STATE, ZIP, ADDRESS2, COUNTRY, FullQueryPassCount, ");
            //sbQuery.Append("APCR, APCRAgent,  APCRDT, APCRMemo,");
            //sbQuery.Append("APCR2, APCRAgent2, APCRDT2, APCRMemo2, ");
            //sbQuery.Append("APCR3, APCRAgent3,  APCRDT3, APCRMemo3,");
            //sbQuery.Append("APCR4, APCRAgent4, APCRDT4, APCRMemo4, ");
            //sbQuery.Append("APCR5,  APCRAgent5, APCRDT5, APCRMemo5, ");
            //sbQuery.Append("APCR6, APCRAgent6, APCRDT6, APCRMemo6, ScheduleDate ");
            sbQuery.Append("SELECT distinct UniqueKey, PhoneNum, NumAttemptsAM, NumAttemptsWkEnd, ");
            sbQuery.Append("NumAttemptsPM, ScheduleDate ");

            // *** Added 10.21.2010 GW add query conditions for AM and PMAttempts

            try
            {
                includeAmPmAttempts = "yes";
            }
            catch { }

            if (includeAmPmAttempts.ToLower() == "yes")
            {
                sbQuery.Append(" FROM CAMPAIGN, DIALINGPARAMETER WHERE ( ");
                sbQuery.Append(strQueryCondition);
                sbQuery.Append(" AND ((DATEPART(hour, GETDATE()) < 13 And (NumAttemptsAM is NULL OR NumAttemptsAM < DialingParameter.AMCallTimes)) Or (DATEPART(hour, GETDATE()) > 12 And (NumAttemptsPM is NULL OR NumAttemptsPM < DialingParameter.PMCallTimes)))");
            }
            else
            {
                // Original prior to addition
                sbQuery.Append(" FROM CAMPAIGN WHERE ( ");
                sbQuery.Append(strQueryCondition);
            }
            // ***  End addition GW
            sbQuery.Append(" )");
            //sbQuery.Append("FROM CAMPAIGN WHERE ((CallResultCode IS NULL AND AgentID IS NULL) OR CallResultCode=6)  AND IsManualDial = 0  ");
            //sbQuery.Append("AND ( NeverCallFlag=0 or NeverCallFlag is NULL )  AND ( ");
            //sbQuery.Append(strQueryCondition);
            //sbQuery.Append(" )");

            //ErrorLogger.Write(string.Format("New query built :'{0}'", sbQuery.ToString()));

            return sbQuery.ToString();
        }

        /// <summary>
        /// Checks run status of campaign 
        /// </summary>
        protected bool IsCampaignRunning()
        {
            Campaign objCampaign;
            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];
                if (objCampaign.StatusID == (long)CampaignStatus.Run)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Filter the data table based on filter condition and return filtered data
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strCondition"></param>
        /// <returns></returns>
        protected DataView FilterData(DataTable dt, string strCondition)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = strCondition;

                return dv;
            }
            return null;
        }

        protected string Format(string value)
        {
            try
            {
                return value.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "");
            }
            catch { return value; }
        }

        internal DataSet GetCsvDataSet(string filePath, bool isHeader)
        {

            try
            {
                DataSet dsData = new DataSet("Data");
                DataTable dtData = new DataTable("DataItem");

                string strCommand = string.Empty;
                string strConnect = string.Empty;
                string strFile = System.IO.Path.GetFileName(filePath);
                string strPath = System.IO.Path.GetDirectoryName(filePath) + "\\\\";
                string strHeader = string.Empty;
                //strHeader = isHeader ? "Yes" : "No";
                strHeader = "Yes";

                //strConnect = "Driver={Microsoft Text Driver (*.txt; *.csv)}Dbq=" + strPath;
                //Dim objConnection As OdbcConnection = New OdbcConnection(strConnect) 

                strCommand = "SELECT * FROM [" + strFile + "]";
                //Dim objAdapter As OdbcDataAdapter = New OdbcDataAdapter(strCommand, objConnection) 
                //objAdapter.Fill(dtData) 

                //objConnection.Close() 

                // strConnect = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPath + ";Extended Properties=\"text;HDR=" + strHeader + ";FMT=Delimited(,)\"";
                strConnect = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strPath + ";Extended Properties=\"text;HDR=" + strHeader + ";FMT=Delimited(,)\"";

                //strConnect = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPath + ";Extended Properties='text;FMT=Delimited(;);HDR=" + strHeader + ";'";

                //strConnect = "Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=" + strPath + ";";

                OleDbConnection objConnection = new OleDbConnection(strConnect);
                OleDbCommand objCommand = new OleDbCommand(strCommand, objConnection);
                OleDbDataAdapter objAdapter = new OleDbDataAdapter(objCommand);
                objAdapter.Fill(dtData);

                dsData.Tables.Add(dtData);

                return dsData;
            }
            catch (Exception exp)
            {

                throw exp;

            }
        }

        protected void CreateSchemaFile(string csvFilePath, char Delimiter, bool isHeader)
        {
            try
            {
                string strDirectory = System.IO.Path.GetDirectoryName(csvFilePath) + "\\";
                string strFileName = System.IO.Path.GetFileName(csvFilePath);
                string strFormat = string.Empty;

                // --- Grab the delimer value for the file
                if (Delimiter == Convert.ToChar(","))
                {
                    strFormat = "Format=CSVDelimited";
                }
                else if (Delimiter == Convert.ToChar("\t"))
                {
                    strFormat = "Format=TabDelimited";
                }
                else
                {
                    strFormat = "Format=Delimited(" + Delimiter + ")";
                }

                if (!isHeader)
                    strFormat += Environment.NewLine + "ColNameHeader=False";

                // --- Create the schema.ini file
                System.IO.StreamWriter stmSchema;
                stmSchema = System.IO.File.CreateText(strDirectory + "schema.ini");
                stmSchema.WriteLine("[" + strFileName + "]");
                stmSchema.WriteLine(strFormat);
                stmSchema.WriteLine("CharacterSet=ANSI");
                stmSchema.Close();
            }
            catch { }
        }

        protected string DisableReadOnlyScriptFields(string scriptBody)
        {
            if (Session["Campaign"] == null) return scriptBody;
            Campaign objCampaign = (Campaign)Session["Campaign"];
            DataSet dsCampaignFields;
            
            try
            {
                StringBuilder sb = new StringBuilder();
                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsCampaignFields = objCampaignService.GetCampaignFields(xDocCampaign);

                string strFieldName = string.Empty;
                string strFieldType = string.Empty;
                string strFieldValue = string.Empty;
                string strFieldReadOnly = string.Empty;
                string newScriptBody = scriptBody;
                bool blnReadOnly = false;
                bool isDataUpdate = false;
                isDataUpdate = scriptBody.Contains("This is encrypted data");
                for (int i = 0; i < dsCampaignFields.Tables[0].Rows.Count; i++)
                {
                    strFieldName = dsCampaignFields.Tables[0].Rows[i]["FieldName"].ToString();  // Field Name
                    strFieldReadOnly = dsCampaignFields.Tables[0].Rows[i]["ReadOnly"].ToString(); // Field Read Only
                    strFieldType = dsCampaignFields.Tables[0].Rows[i]["FieldType"].ToString(); // Field Type
                    strFieldValue = dsCampaignFields.Tables[0].Rows[i]["Value"].ToString(); // Field Value

                    try
                    {
                        blnReadOnly = Convert.ToBoolean(strFieldReadOnly);
                    }
                    catch
                    {
                        blnReadOnly = false;
                    }
                    if (blnReadOnly)
                    {
                        newScriptBody = newScriptBody.Replace(string.Format("name=\"{0}\"", strFieldName.ToLower()), string.Format("name=\"{0}\" disabled=\"disabled\"", strFieldName.ToLower()));
                    }
                    if ((strFieldType == "Encrypted") && isDataUpdate)
                    {
                        newScriptBody = newScriptBody.Replace(string.Format("name=\"{0}\"", strFieldName.ToLower()), string.Format("name=\"{0}\" disabled=\"disabled\"", strFieldName.ToLower()));
                    }
                }
                
                return newScriptBody;

            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
                return scriptBody;
            }

        }

        protected void BindCampaignFields()
        {
            if (Session["Campaign"] == null) return;
            Campaign objCampaign = (Campaign)Session["Campaign"];
            DataSet dsCampaignDtls = (DataSet)Session["CampaignDtls"];
            DataRow drCampaignDtls = dsCampaignDtls.Tables[0].Rows[0];
            DataSet dsCampaignFields;
            try
            {
                StringBuilder sb = new StringBuilder();
                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsCampaignFields = objCampaignService.GetCampaignFields(xDocCampaign);
                sb.Append("<script language=\"javascript\" type=\"text/javascript\">");
                sb.Append("var filledArray = new Array(");


                string[] dftDatefields = new string[]{"datetimeofcall","callsenttodialtime","calltoagenttime",
                    "callhanguptime","callcompletiontime","callwrapupstarttime","callwrapupstoptime",
                    "resultcodesettime","scheduledate"};
                bool set = false;
                foreach (string sfield in dftDatefields)
                {
                    string dateValue = drCampaignDtls[sfield].ToString();
                    if (dateValue != "")
                    {
                        try
                        {
                            dateValue = Convert.ToDateTime(drCampaignDtls[sfield]).ToString("MM/dd/yyyy HH:mm:ss");
                        }
                        catch
                        {
                            dateValue = "";
                        }
                    }
                    sb.AppendFormat("{3}new Array(\"{0}\", \"{1}\", \"{2}\")", sfield, "dt", dateValue, set ? "," : "");
                    set = true;
                }

                string[] dftIntfields = new string[]{"nevercallflag","totalnumattempts","numattemptsam",
                    "numattemptswkend","numattemptspm","leadprocessed","fullquerypasscount"};
                foreach (string sfield in dftIntfields)
                {
                    sb.AppendFormat(",new Array(\"{0}\", \"{1}\", \"{2}\")", sfield, "i", drCampaignDtls[sfield].ToString());
                }

                sb.AppendFormat(",new Array(\"{0}\", \"{1}\", \"{2}\")", "callduration", "dec", drCampaignDtls["CallDuration"].ToString());

                sb.AppendFormat(",new Array(\"{0}\", \"{1}\", \"{2}\")", "verificationagentid", "s", drCampaignDtls["verificationagentid"].ToString());

                sb.AppendFormat(",new Array(\"{0}\", \"{1}\", \"{2}\")", "dbcompany", "s", drCampaignDtls["dbcompany"].ToString());

                sb.AppendFormat(",new Array(\"{0}\", \"{1}\", \"{2}\")", "schedulenotes", "s", drCampaignDtls["schedulenotes"].ToString());


                foreach (DataRow dr in dsCampaignFields.Tables[0].Rows)
                {
                    string sfield = dr["FieldName"].ToString().ToLower();
                    if (dr["FieldType"].ToString().ToLower() == "date")
                    {
                        sb.AppendFormat(",new Array(\"{0}\", \"{1}\", \"{2}\")", sfield, "dt",
                        drCampaignDtls[sfield].ToString() != "" ? Convert.ToDateTime(drCampaignDtls[sfield]).ToString("MM/dd/yyyy HH:mm:ss") : "");
                    }
                    else if (dr["FieldType"].ToString().ToLower() == "integer")
                    {
                        sb.AppendFormat(",new Array(\"{0}\", \"{1}\", \"{2}\")", sfield, "i", drCampaignDtls[sfield].ToString());
                    }
                    else if (dr["FieldType"].ToString().ToLower() == "string")
                    {
                        string cleanedstringvalue = drCampaignDtls[sfield].ToString().Replace("\\", "\\\\").Replace("\"", "\\\"");
                        sb.AppendFormat(",new Array(\"{0}\", \"{1}\", \"{2}\")", sfield, "s", cleanedstringvalue);
                    }
                    else if (dr["FieldType"].ToString().ToLower() == "boolean")
                    {
                        string val = drCampaignDtls[sfield].ToString().Replace("\"", "\\\"").Trim();
                        bool isCheck = false;
                        try
                        {
                            isCheck = Convert.ToBoolean(val);
                        }
                        catch { }
                        sb.AppendFormat(",new Array(\"{0}\", \"{1}\", \"{2}\")", sfield, "bool", isCheck ? "1" : "0");
                    }
                    else if (dr["FieldType"].ToString().ToLower() == "encrypted") {
                        sb.AppendFormat(",new Array(\"{0}\", \"{1}\", \"{2}\")", sfield, "encrypted", drCampaignDtls[sfield].ToString().Replace("\"", "\\\""));
                    }
                    else
                    {
                        sb.AppendFormat(",new Array(\"{0}\", \"{1}\", \"{2}\")", sfield, "dec", drCampaignDtls[sfield].ToString());
                    }
                }
                sb.Append(");");
                sb.AppendFormat("var campaignId = {0};", drCampaignDtls["UniqueKey"]);
                sb.Append("</script>");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "CampaignFields", sb.ToString().Replace("\n", "\\n"), false);
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        protected void DisposeCall(int resultcodeID, bool isManualDial, string resultcodeName)
        {
            DataSet dsCampaignDtls = (DataSet)Session["CampaignDtls"]; // campaign data
            long uniqueKey = Convert.ToInt32(dsCampaignDtls.Tables[0].Rows[0]["UniqueKey"].ToString());

            Campaign objcampaign;
            Agent objAgent;

            AgentService objAgentService = new AgentService();

            if (Session["LoggedAgent"] != null && Session["Campaign"] != null && Session["CampaignDtls"] != null)
            {
                objAgent = (Agent)Session["LoggedAgent"];
                XmlDocument xDocAgent = new XmlDocument();

                if (!isManualDial)
                {
                    objAgent.AgentStatusID = (long)AgentLoginStatus.Ready;
                }
                else
                {
                    objAgent.AgentStatusID = (long)AgentLoginStatus.Pause;
                }

                xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));

                objcampaign = (Campaign)Session["Campaign"];
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objcampaign, "Campaign"));

                long queryId = 0;
                if (!isManualDial)
                {
                    try
                    {
                        queryId = Convert.ToInt64(dsCampaignDtls.Tables[0].Rows[0]["QueryId"].ToString());
                    }
                    catch (Exception ex) { throw ex; }
                }
                CampaignService objCampService = new CampaignService();

                // Added for agent busy trigger GW 
                objCampService.UpdateAgentStatus(xDocCampaign, xDocAgent);
                objAgentService.UpdateResultCode(xDocCampaign, xDocAgent, uniqueKey, resultcodeID, queryId);
                ActivityLogger.WriteAgentEntry(objAgent, "Call disposition complete as: {0}", resultcodeName);
                // Added by GW  - query stats update.
                try
                {
                    if (queryId > 0)
                    {
                        CampaignQueryStatus campQueryStatus = new CampaignQueryStatus();
                        campQueryStatus.QueryID = queryId;
                        bool update = false;
                        switch (resultcodeName.Trim())
                        {
                            case "Answering Machine": campQueryStatus.AnsweringMachine = 1; update = true; break;
                            case "No Answer": campQueryStatus.NoAnswer = 1; update = true; break;
                            case "Busy": campQueryStatus.Busy = 1; update = true; break;
                            case "Operator Intercept": campQueryStatus.OpInt = 1; update = true; break;
                            case "Dropped": campQueryStatus.Drops = 1; update = true; break;
                            default: campQueryStatus.ResultCodeId = resultcodeID; update = true; break;
                        }

                        if (update)
                        {
                            XmlDocument xDocCampQueryStatus = new XmlDocument();
                            xDocCampQueryStatus.LoadXml(Serialize.SerializeObject(campQueryStatus, "CampaignQueryStatus"));
                            objCampService.UpdateCampaignQueryStats(xDocCampaign, xDocCampQueryStatus);
                        }
                    }
                }
                catch { }
                try
                {
                    if (Session["CampaignDtls"] != null && Session["Campaign"] != null)
                    {
                        string strDBConnstr = objcampaign.CampaignDBConnString;
                        objCampService.SetCallHangup(uniqueKey, strDBConnstr);
                        if (Session["LoggedAgent"] != null)
                            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Disposition hangup flag has been set for the dialer.");
                    }
                }
                catch { }
                
            }

        }

        protected string FormatTime(string timeInSeconds)
        {
            int hours = 0;
            int minutes = 0;
            int seconds = 0;
            try
            {
                if (timeInSeconds != "")
                {
                    double time = Convert.ToDouble(timeInSeconds);
                    hours = (int)time / 3600;
                    time = (int)time % 3600;
                    minutes = (int)time / 60;
                    seconds = (int)time % 60;
                }
            }
            catch { }
            string strTime = string.Format("{0}:{1}:{2}",
                hours < 10 ? "0" + hours.ToString() : hours.ToString(),
                minutes < 10 ? "0" + minutes.ToString() : minutes.ToString(),
                seconds < 10 ? "0" + seconds.ToString() : seconds.ToString());
            return strTime;
        }

        protected bool IsSysResultCode(string resultCode, string[] sysResultCodes)
        {
            foreach (string s in sysResultCodes)
            {
                if (s.ToLower() == resultCode.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        protected bool ShowSysResultCode(string resultCode, string[] sysResultCodes)
        {
            foreach (string s in sysResultCodes)
            {
                if (s.ToLower() == resultCode.ToLower())
                {
                    return false;
                }
            }
            return true;
        }

        protected string PrepareDialerQuery(string query, long queryId)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" AND ( NeverCallFlag=0 or NeverCallFlag IS NULL ) ");
            sb.Append(" AND (ScheduleDate is null OR DATEDIFF(dd,getdate(),ScheduleDate) <= 0) ");
            sb.Append(" AND UniqueKey NOT IN ( ");
            sb.Append(" select distinct UniqueKey from CallList CL ");
            sb.Append(" INNER JOIN (SELECT MAX(calllistid) AS MaxCallListID FROM calllist GROUP BY uniquekey) MCL ON MCL.MaxCallListID = CL.CallListID  ");
            sb.Append(" inner join ResultCode on ResultCode.ResultCodeID = CL.ResultCodeID ");
            sb.AppendFormat(" where IsManualDial=1  OR (queryid = {0} AND (Redialable = 0 OR NeverCall = 1 OR NeverCall = 2 OR DATEDIFF(dd,CL.calldate,getdate()) < RecycleInDays)))  ", queryId);
            return (query + sb.ToString());
        }
    }

}