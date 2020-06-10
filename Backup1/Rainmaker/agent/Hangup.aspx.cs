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
    public partial class Hangup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                HangupCall();
            }
        }

        private void HangupCall()
        {
            try
            {
                if (Session["LoggedAgent"] != null)
                    ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Hangup handler page has been invoked.");
 
                if (Session["CampaignDtls"] != null && Session["Campaign"] != null)
                {
                    DataSet dsCampaignDtls = (DataSet)Session["CampaignDtls"];
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    
                    if (dsCampaignDtls.Tables[0].Rows.Count > 0)
                    {
                        long uniqueKey = Convert.ToInt64(dsCampaignDtls.Tables[0].Rows[0]["UniqueKey"].ToString());
                        string strDBConnstr = objCampaign.CampaignDBConnString;
                        CampaignService objCampService = new CampaignService();
                        objCampService.SetCallHangup(uniqueKey, strDBConnstr);
                    }
                }
            }
            catch (Exception ex)
            {
                ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "HangupCall error: " + ex.Message);
                ltrlScript.Text = "failed";
            }
            //ltrlScript.Text = "Called";
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
