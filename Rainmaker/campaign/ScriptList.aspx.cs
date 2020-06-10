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
using Rainmaker.Web.CampaignWS;

namespace Rainmaker.Web.campaign
{
    public partial class ScriptList : PageBase
    {
        Script objScript = null;
        public bool Isrunning = false;

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
                GetScriptList();
            }
        }
        

        /// <summary>
        /// Navigates to script editor page fro editing script details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnScriptName_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            string parentScript = "";
            if (GetParentScriptId() <= 0)
            {
                parentScript = "&ParentScriptName=" + lbtnSender.Text;
            }
            else
            {
                parentScript = "&ParentScriptName=" + GetParentScriptName();
            }
            Response.Redirect(
                "ScriptEditor.aspx?ScriptID=" + lbtnSender.CommandArgument + "&ParentScriptID=" + GetParentScriptId() + parentScript);
        }

        protected void lbtnScriptPages_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            Response.Redirect(
                "ScriptList.aspx?ParentScriptID="+lbtnSender.CommandArgument+"&ParentScriptName="+lbtnSender.CommandName+"&ts="+DateTime.Now.Ticks.ToString());
        }

        protected void lbtnScriptSave_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            long ScriptID = Convert.ToInt64(lbtnSender.CommandArgument);
             
            GetScriptDetailByScriptID(ScriptID);
            BuildSavableScriptCopy(lbtnSender);
            Response.Redirect("CloneScript.aspx?ScriptID=" + lbtnSender.CommandArgument + "&ParentScriptName=" + lbtnSender.CommandName + "&ts=" + DateTime.Now.Ticks.ToString());
            //Response.Redirect("CloneScript.aspx?ScriptID=" + lbtnSender.CommandArgument + "&ParentScriptID=" + GetParentScriptId() + "&ParentScriptName=" + lbtnSender.CommandName + "&ts=" + DateTime.Now.Ticks.ToString());
        }

        protected void lbtnAddPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("ScriptEditor.aspx?ParentScriptID=" + GetParentScriptId() + "&ParentScriptName=" + GetParentScriptName());
        }

        /// <summary>
        /// On row data binding
        /// </summary>
        protected void grdScriptList_rowdatabound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                //if (Isrunning)
                //{
                    lbtnDelete.Attributes.Add("onClick", "alert('You cannot delete script when campaign is running');return false;");
                //}
                //else
                //{
                    lbtnDelete.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to delete the script?');");
                //}
            }
            try
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    if (GetParentScriptId() > 0)
                    {
                        e.Row.Cells[0].Text = "Page Name";
                    }
                }
            }
            catch { }
        }

        /// <summary>
        /// Detets a row in grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            long ScriptID = 0;
            ScriptID = Convert.ToInt64(lbtnSender.CommandArgument);


            if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];

                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                try
                {
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    string message = objCampaignService.DeleteScript(xDocCampaign, ScriptID);

                    if (message != "")
                    {
                        PageMessage = message;
                    }
                    else
                        GetScriptList();
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
            }
        }

        protected void lbtnClose_Click(object sender, EventArgs e)
        {
            if (GetParentScriptId() > 0)
            {
                Response.Redirect("ScriptList.aspx");
            }
            else
                Response.Redirect("home.aspx");
        }

        #endregion

        #region Private Methods
        private void GetScriptDetailByScriptID(long ScriptID)
        {
            Campaign objCampaign = (Campaign)Session["Campaign"];

            CampaignService objCampService = new CampaignService();
            XmlDocument xDocCampaign = new XmlDocument();
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            try
            {
                objScript = (Script)Serialize.DeserializeObject(objCampService.GetScriptByScriptID(xDocCampaign, ScriptID), "Script");
                hdnScriptHeader.Value = Server.UrlDecode(objScript.ScriptHeader);
                //hdnScriptSubHeader.Value = Server.UrlDecode(objScript.ScriptSubHeader);
                hdnScriptBody.Value = Server.UrlDecode(objScript.ScriptBody);
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }
        private long GetQueryString()
        {
            long ScriptID = 0;
            if (Request.QueryString["ScriptID"] != null)
            {
                ScriptID = Convert.ToInt64(Request.QueryString["ScriptID"]);
            }
            return ScriptID;
        }
        private void BuildSavableScriptCopy(LinkButton lbtnSender)
        {
            Script objScript = new Script();
            try
            {
                objScript.ScriptID = Convert.ToInt64(lbtnSender.CommandArgument);


                objScript.ScriptName = lbtnSender.CommandName;
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
        /// Binds the script list to grid
        /// </summary>
        private void GetScriptList()
        {
            DataSet dsScriptList;
            try
            {   
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];

                    lblCampaign.Text = objCampaign.Description; // Replaced Short description

                    CampaignService objCampService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    if (GetParentScriptId() > 0)
                    {
                        lbtnAddPage.Visible = true;
                        lbtnAddScript.Visible = false;
                        ltrlPage.Text = "&nbsp;&nbsp;<img src=\"../images/arrowright.gif\">&nbsp;&nbsp;<a href=\"ScriptList.aspx\" class=\"aHome\"><b>Script List<b></a>";
                        ltrlPage.Text += "&nbsp;&nbsp;<img src=\"../images/arrowright.gif\">&nbsp;&nbsp;<b>Script Pages<b>";
                        ltrlParentScriptName.Text = "<tr><td align=\"left\"><b>SCRIPT NAME : " + GetParentScriptName() + "</b></td></tr>";
                        dsScriptList = objCampService.GetPageListByScriptId(xDocCampaign, GetParentScriptId());
                        
                        grdScriptList.EmptyDataText = "No pages found.";
                    }
                    else
                    {
                        ltrlPage.Text = "&nbsp;&nbsp;<img src=\"../images/arrowright.gif\">&nbsp;&nbsp;<b>Script List<b>";
                        dsScriptList = objCampService.GetScriptList(xDocCampaign);
                    }

                    if (IsCampaignRunning())
                    {
                        Isrunning = true;
                    }

                    grdScriptList.DataSource = dsScriptList;
                    grdScriptList.DataBind();
                    if (GetParentScriptId() > 0)
                    {
                        grdScriptList.Columns[1].Visible = false;
                        grdScriptList.Columns[2].Visible = false; 
                    }
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

        #endregion
    }
}
