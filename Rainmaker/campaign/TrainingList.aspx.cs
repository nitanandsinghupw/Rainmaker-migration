using System;
using System.Data;
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
using Rainmaker.Web.CampaignWS;

namespace Rainmaker.Web.campaign
{
    public partial class TrainingList : PageBase
    {
        public bool Isrunning = false;

        #region Events

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RefreshActiveSelection(); 
                RefreshSchemeList(GetTrainingSchemeID());
                GetTrainingPageList();
            }
        }

        

        /// <summary>
        /// Navigates to script editor page fro editing script details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnTrainingPageName_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            Response.Redirect(string.Format("TrainingPageEditor.aspx?TrainingSchemeID={0}&TrainingPageID={1}", ddlActiveScheme.SelectedValue, lbtnSender.CommandArgument));
        }

        protected void rdoActiveScheme_Change(object sender, EventArgs e)
        {
            Campaign objCampaign = null;
            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];
            }
            CampaignService objCampService = new CampaignService();
            XmlDocument xDocCampaign = new XmlDocument();
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

            if (rdoSelected.Checked)
            {
                objCampService.UpdateActiveTrainingScheme(xDocCampaign, Convert.ToInt64(ddlActiveScheme.SelectedValue));
                return;
            }
            if (rdoAll.Checked)
            {
                objCampService.UpdateActiveTrainingScheme(xDocCampaign, -1);
                return;
            }

            objCampService.UpdateActiveTrainingScheme(xDocCampaign, 0);
        }

        protected void lbtnTrainingPageSave_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            Response.Redirect(
                "CloneScript.aspx?ScriptID=" + lbtnSender.CommandArgument + "&ParentScriptName=" + lbtnSender.CommandName + "&ts=" + DateTime.Now.Ticks.ToString());
        }

        protected void lbtnAddPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("TrainingPageEditor.aspx?TrainingSchemeID=" + ddlActiveScheme.SelectedValue);
        }

        /// <summary>
        /// On row data binding
        /// </summary>
        protected void grdTrainingPageList_rowdatabound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                lbtnDelete.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to delete this training page?');");
            }
        }

        protected void lbtnDeleteScheme_Click(object sender, EventArgs e)
        {
            if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];

                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                try
                {
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    objCampaignService.DeleteTrainingScheme(xDocCampaign, Convert.ToInt64(ddlActiveScheme.SelectedValue));
                    RefreshSchemeList(0);
                    if (Convert.ToInt64(ddlActiveScheme.SelectedValue) > 0)
                        objCampaignService.UpdateActiveTrainingScheme(xDocCampaign, Convert.ToInt64(ddlActiveScheme.SelectedValue));
                    GetTrainingPageList();
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
            }

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
                        GetTrainingPageList();
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
            }
        }

        protected void lbtnClose_Click(object sender, EventArgs e)
        {
            Response.Redirect("home.aspx");
        }

        protected void lbtnNewScheme_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtNewScheme.Text.Length < 1)
                {
                    Response.Write("<script>alert('Please enter a name for the new training scheme');</script>");
                    return;
                }
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];

                    CampaignService objCampaignService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();
                    
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                        long newSchemeID = objCampaignService.AddTrainingScheme(xDocCampaign, txtNewScheme.Text);

                        txtNewScheme.Text = string.Empty;
                        RefreshSchemeList(0);
                        GetTrainingPageList();
                        
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        protected void ddlActiveScheme_Change(object sender, EventArgs e)
        {
            try
            {
                if (rdoSelected.Checked)
                {
                    if (Session["Campaign"] != null)
                    {
                        Campaign objCampaign = (Campaign)Session["Campaign"];

                        CampaignService objCampService = new CampaignService();
                        XmlDocument xDocCampaign = new XmlDocument();
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                        ltrlPage.Text = "&nbsp;&nbsp;<img src=\"../images/arrowright.gif\">&nbsp;&nbsp;<b>Training Schemes<b>";
                        objCampService.UpdateActiveTrainingScheme(xDocCampaign, Convert.ToInt64(ddlActiveScheme.SelectedValue));
                    }
                }
                GetTrainingPageList();
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Binds the script list to grid
        /// </summary>
        private void GetTrainingPageList()
        {
            DataSet dsTrainingPageList;
            try
            {   
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];

                    lblCampaign.Text = objCampaign.Description; // Replaced Short description

                    CampaignService objCampService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    ltrlPage.Text = "&nbsp;&nbsp;<img src=\"../images/arrowright.gif\">&nbsp;&nbsp;<b>Training Schemes<b>";
                    dsTrainingPageList = objCampService.GetTrainingPageList(xDocCampaign, Convert.ToInt64(ddlActiveScheme.SelectedValue));

                    if (IsCampaignRunning())
                    {
                        Isrunning = true;
                    }

                    grdTrainingPageList.DataSource = dsTrainingPageList;
                    grdTrainingPageList.DataBind();
                }

            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        private void RefreshSchemeList(long schemeID)
        {
            DataSet dsTrainingSchemeList;
            string strActiveSchemeName = "";
            string strActiveSchemeID = schemeID.ToString();
            try
            {
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];

                    ddlActiveScheme.Items.Clear();

                    lblCampaign.Text = objCampaign.Description; // Replaced Short description

                    CampaignService objCampService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    ltrlPage.Text = "&nbsp;&nbsp;<img src=\"../images/arrowright.gif\">&nbsp;&nbsp;<b>Training Schemes<b>";
                    dsTrainingSchemeList = objCampService.GetTrainingSchemeList(xDocCampaign);

                    // Loop through records, mark active one and update drop down list.
                    if (dsTrainingSchemeList.Tables[0].Rows.Count < 1)
                    {
                        ddlActiveScheme.Items.Add(new ListItem("No Training Schemes Exist", "0"));
                        ddlActiveScheme.Enabled = false;
                        return;
                    }
                    DataRow dr;
                    for (int i = 0; i < dsTrainingSchemeList.Tables[0].Rows.Count; i++)
                    {
                        bool isActive = false;
                        dr = dsTrainingSchemeList.Tables[0].Rows[i];
                        ddlActiveScheme.Items.Add(new ListItem(dr["Name"].ToString(), dr["TrainingSchemeID"].ToString()));
                        isActive = (dr["IsActive"] == Convert.DBNull) ? false : (bool)dr["IsActive"];
                        if (isActive && (Convert.ToInt64(strActiveSchemeID) < 1))
                        {
                            strActiveSchemeName = dr["Name"].ToString();
                            strActiveSchemeID = dr["TrainingSchemeID"].ToString();
                        }
                    }
                    if (!string.IsNullOrEmpty(strActiveSchemeID))
                        ddlActiveScheme.SelectedValue = strActiveSchemeID;
                }

            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        private void RefreshActiveSelection()
        {
            DataSet dsTrainingSchemeList;
            int activeSchemeCount = 0;
            try
            {
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    CampaignService objCampService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    dsTrainingSchemeList = objCampService.GetTrainingSchemeList(xDocCampaign);

                    if (dsTrainingSchemeList.Tables[0].Rows.Count < 1)
                    {
                        rdoNone.Checked = true;
                        return;
                    }

                    DataRow dr;
                    for (int i = 0; i < dsTrainingSchemeList.Tables[0].Rows.Count; i++)
                    {
                        bool isActive = false;
                        dr = dsTrainingSchemeList.Tables[0].Rows[i];
                        isActive = (dr["IsActive"] == Convert.DBNull) ? false : (bool)dr["IsActive"];
                        if (isActive)
                        {
                            activeSchemeCount++;
                        }
                    }
                    switch (activeSchemeCount)
                    {
                        case 0:
                            rdoNone.Checked = true;
                            break;
                        case 1:
                            rdoSelected.Checked = true;
                            break;
                        default:
                            rdoAll.Checked = true;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
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
        #endregion
    }
}
