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

namespace Rainmaker.Web.agent
{
    public partial class GetScript : PageBase
    {
        private string[] hideSysResultCodes = { 
                "Scheduled Callback","Transferred to Agent",
                "Transferred to Dialer","Transferred to Verification","Unmanned Live Contact",
                "Inbound Abandoned by Agent","Inbound abandoned by Caller","Error","Failed",
                "Cadence Break","Loop Current Drop","Pbx Detected","No Ringback","Analysis Stopped",
                "No DialTone","FaxTone Detected", "Unmanned Transferred to Answering Machine", "Transferred Offsite" };

        private string[] hideResultCodesForAgent = { 
                "Answering Machine","Busy","Operator Intercept","Dropped","No Answer","Scheduled Callback"};

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetScriptData();
            }
        }

        /// <summary>
        /// Gets script data
        /// </summary>
        private void GetScriptData()
        {

            //long ScriptID = Convert.ToInt64(GetQueryString("ScriptID"));
            string ScriptGUID = GetQueryString("ScriptID");
            bool isHeader = false;
            try
            {
                isHeader = Convert.ToBoolean(GetQueryString("IsHeader"));
            }
            catch { }
            //if (ScriptID > 0)
            if (ScriptGUID != "")
            {

                Script objScript;
                Campaign objCampaign = (Campaign)Session["Campaign"];

                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                Agent objAgent = (Agent)Session["LoggedAgent"];
                bool isVerification = false;
                try
                {
                    DataSet dsCampaignDtls = (DataSet)Session["CampaignDtls"];
                    int agentID = Convert.ToInt32(dsCampaignDtls.Tables[0].Rows[0]["AgentID"]);
                    int verificationAgentID = Convert.ToInt32(dsCampaignDtls.Tables[0].Rows[0]["VerificationAgentID"]);
                    if (objAgent.AgentID == verificationAgentID) //Determines that this is verification
                        isVerification = true;
                }
                catch { }
                try
                {
                    objScript = (Script)Serialize.DeserializeObject(objCampService.GetScriptByScriptGUID(xDocCampaign, ScriptGUID), "Script");
                    if(objScript.ScriptID <= 0)
                    {
                        ltrlScript.Text = "#invalidscript#";
                        return;
                    }
                    if (isHeader)
                        ltrlScript.Text = Server.UrlDecode(objScript.ScriptHeader);
                    else
                    {
                        ltrlScript.Text = Server.UrlDecode(objScript.ScriptBody);
                        if (Session["CampaignDtls"] != null)
                        {
                            DataSet dsCampaignDtls = (DataSet)Session["CampaignDtls"];
                  
                            if (dsCampaignDtls.Tables[0].Rows.Count > 0)
                            {
                                string jscript = AssignCampaignFieldsDataToTextFields(dsCampaignDtls, ltrlScript);
                                if (jscript != "")
                                {
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "jscript", jscript, true);
                                }
                            }
                            System.Text.StringBuilder strResultcodes = new System.Text.StringBuilder();
                            strResultcodes.AppendFormat("<OPTION selected value=-1>Select Result Code</OPTION>");

                            ltrlScript.Text = ltrlScript.Text.Replace("<SELECT name=cboResultCode>", "<SELECT name=cboResultCode onchange=\"DisposeCall('cboResultCode')\">");
                            ltrlScript.Text = ltrlScript.Text.Replace("<select name=cboResultCode>", "<select name=cboResultCode onchange=\"DisposeCall('cboResultCode')\">");

                            if (ltrlScript.Text.Contains(strResultcodes.ToString()))
                            {
                                DataView dvResultCodes;
                                dvResultCodes = BindResultCodes(isVerification);
                                try
                                {
                                    hideSysResultCodes = ConfigurationManager.AppSettings["SysResultCodesToHide"].Split(',');
                                }
                                catch { }
                                if (dvResultCodes != null)
                                {
                                    DataTable dtResultCodes = dvResultCodes.ToTable();
                                    bool selected = false;
                                    foreach (DataRow dr in dtResultCodes.Rows)
                                    {
                                        string resultCode = dr["Description"].ToString();
                                        string resultCodeID = dr["ResultCodeID"].ToString();
                                        string strSelectedResultcode = "_";
                                        try
                                        {
                                            strSelectedResultcode = dsCampaignDtls.Tables[0].Rows[0]["CallResultCode"].ToString();
                                        }
                                        catch { }
                                        if (ShowSysResultCode(resultCode, hideSysResultCodes))
                                        {

                                            bool hideDefautResultCodes = true;
                                            try
                                            {
                                                hideDefautResultCodes = Convert.ToBoolean(ConfigurationManager.AppSettings["HideDefaultResultcodesForScript"]);
                                            }
                                            catch { }
                                            if (!hideDefautResultCodes || ShowSysResultCode(resultCode, hideResultCodesForAgent))
                                            {
                                                if (!selected)
                                                    selected = (resultCodeID == strSelectedResultcode);
                                                strResultcodes.AppendFormat("<option value='{0}' {2}>{1}</option>", resultCodeID, resultCode, resultCodeID == strSelectedResultcode ? "selected" : "");
                                            }
                                        }
                                        if (selected)
                                        {
                                            strResultcodes = strResultcodes.Replace("value=-1 selected", "value=-1");
                                        }
                                    }
                                    ltrlScript.Text = ltrlScript.Text.Replace("<OPTION selected value=-1 selected>Select Result Code</OPTION>", strResultcodes.ToString());
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Gets ScriptID
        /// </summary>
        /// <returns></returns>
        private string GetQueryString(string key)
        {
            string value = "0";
            if (Request.QueryString[key] != null)
            {
                value = Request.QueryString[key];
            }
            return value;
        }
    }
}
