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
using Rainmaker.Web.CampaignWS;
using System.Xml;
using Rainmaker.Common.DomainModel;

namespace Rainmaker.Web.campaign
{
    public partial class CloneScript : PageBase
    {
        Campaign objCampaign = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Campaign"] == null)
            {
                Response.Redirect("CampaignList.aspx");
            }
            if (!Page.IsPostBack)
            {
                objCampaign = (Campaign)Session["Campaign"];

                try
                {
                    lblScriptName.Text = GetParentScriptName();
                    txtScript.Text = lblScriptName.Text;
                    BindCampaignList();
                }
                catch { }
            }
        }

        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        protected void lbtnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("ScriptList.aspx");
            //Response.Redirect("ScriptEditor.aspx?ScriptID=" + GetScriptId() + "&ParentScriptName=" + GetParentScriptName());
        }

        private void SaveData()
        {
            try
            {
                if (Session["SaveAsScript"] != null)
                {
                    Script objScript = (Script)Session["SaveAsScript"];
                    // Clear to show script is new
                    objScript.ScriptID = 0;
                    objScript.ParentScriptID = 0;
                    objScript.ScriptName = txtScript.Text;

                    CampaignService objCampaignService = new CampaignService();
                    Campaign objTargetCampaign = new Campaign();
                    objTargetCampaign.CampaignDBConnString = ddlCampaign.SelectedValue;

                    XmlDocument xDocScript = new XmlDocument();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocScript.LoadXml(Serialize.SerializeObject(objScript, "Script"));
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objTargetCampaign, "Campaign"));

                    objScript = (Script)Serialize.DeserializeObject(objCampaignService.ScriptInsertUpdate
                        (xDocCampaign, xDocScript), "Script");
                    if (objScript.ScriptName.IndexOf("###ERROR###") >= 0)
                    {
                        PageMessage = objScript.ScriptName.Substring(11);
                    }
                    else
                        //Response.Redirect("ScriptList.aspx?ParentScriptID=" + GetParentScriptId() + "&ParentScriptName=" + GetParentScriptName());
                        Response.Redirect("ScriptList.aspx");
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }




            //objCampaign = (Campaign)Session["Campaign"];
            //CampaignService objCampService = new CampaignService();
            //string ErrorMsg = objCampService.CloneScript(objCampaign.CampaignDBConnString, 
            //    GetScriptId(), txtScript.Text.Trim(), ddlCampaign.SelectedValue);
            //if (ErrorMsg != "")
            //{
            //    PageMessage = string.Format("Campaign '{0}' : {1}", ddlCampaign.SelectedItem.Text, ErrorMsg);
            //}
            //else
            //{
            //    Response.Redirect("ScriptList.aspx");
            //    //Response.Redirect("ScriptEditor.aspx?ScriptID=" + GetScriptId() + "&ParentScriptName=" + GetParentScriptName());
            //    //PageMessage = "Successfully cloned";
            //}
        }

        private void BindCampaignList()
        {
            DataSet dsCampaignList;
            try
            {
                CampaignService objCampService = new CampaignService();
                dsCampaignList = objCampService.GetCampaignList();

                //foreach (DataRow dr in dsCampaignList.Tables[0].Rows)
                //{
                //    ddlCampaign.Items.Add(new ListItem(dr["ShortDescription"], dr["CampaignID"] + "#" + dr["CampaignDBConnString"]));
                //}

                ddlCampaign.DataTextField = "Description";
                ddlCampaign.DataValueField = "CampaignDBConnString";
                ddlCampaign.DataSource = dsCampaignList;
                ddlCampaign.DataBind();
                if (objCampaign != null)
                {
                    ddlCampaign.SelectedValue = objCampaign.CampaignDBConnString;
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Gets Parent ScriptID
        /// </summary>
        /// <returns></returns>
        private long GetScriptId()
        {
            long scriptID = 0;
            if (Request.QueryString["ScriptID"] != null)
            {
                scriptID = Convert.ToInt64(Request.QueryString["ScriptID"]);
            }
            return scriptID;
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
    }
}
