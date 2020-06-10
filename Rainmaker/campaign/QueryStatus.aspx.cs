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
using Rainmaker.Common.DomainModel;
using Rainmaker.Web.CampaignWS;
using System.Text;
namespace Rainmaker.Web.campaign
{
    public partial class QueryStatus : PageBase
    {
        #region Events

        public bool Isrunning = false;
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Campaign objCampaign;
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                    if (objCampaign.StatusID == (long)CampaignStatus.Idle)
                        Timer1.Enabled = false;
                    anchHome.InnerText = objCampaign.Description; // Replaced Short description
                    BindQueries(objCampaign, false);
                }
            }
        }

        /// <summary>
        /// Move to Stand By 
        /// 
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        

        protected void grdActiveQueries_DataBind(object sender, DataGridItemEventArgs e)
        {
            
        }

        protected void grdActiveQueries_EditCommand(object source, DataGridCommandEventArgs e)
        {
            
        }

        protected void grdActiveQueries_CancelEdit(object source, DataGridCommandEventArgs e)
        {
            
        }

        protected void grdActiveQueries_UpdateRecord(object source, DataGridCommandEventArgs e)
        {
            
        }

        protected void grdActiveQueries_DeleteRecord(object source, DataGridCommandEventArgs e)
        {
            
        }

        protected void grdStandbyQueries_DataBind(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    grdStandbyQueries.EditItemIndex = e.Item.ItemIndex;
                    ((LinkButton)e.Item.Cells[0].Controls[1]).Enabled = false;
                    //((LinkButton)e.Item.Cells[1].Controls[1]).Enabled = false;
                    for (int i = 3; i < 12; i++)
                    {
                        ((TextBox)e.Item.Cells[i].Controls[0]).Enabled = false;
                    }

                    ((LinkButton)e.Item.Cells[12].Controls[1]).Enabled = false;
                    ((LinkButton)e.Item.Cells[13].Controls[1]).Enabled = false;
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error invoking edit bind " + ex.Message;
            }
        }

        protected void grdAllQueries_DataBind(object sender, DataGridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.EditItem)
                {
                    grdAllQueries.EditItemIndex = e.Item.ItemIndex;
                    ((LinkButton)e.Item.Cells[0].Controls[1]).Enabled = false;
                    //((LinkButton)e.Item.Cells[1].Controls[1]).Enabled = false;
                    //((LinkButton)e.Item.Cells[1].Controls[1]).Enabled = false;
                    for (int i = 3; i < 12; i++)
                    {
                        ((TextBox)e.Item.Cells[i].Controls[0]).Enabled = false;
                    }

                    ((LinkButton)e.Item.Cells[12].Controls[1]).Enabled = false;
                    ((LinkButton)e.Item.Cells[12].Controls[3]).Enabled = false;
                    ((LinkButton)e.Item.Cells[12].Controls[5]).Enabled = false;
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error invoking edit bind " + ex.Message;
            }
        }


        protected void grdQueries_EditCommand(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdAllQueries.EditItemIndex = e.Item.ItemIndex;
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    BindQueries(objCampaign, false);
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error invoking edit " + ex.Message;
            }
        }

        protected void grdQueries_CancelEdit(object source, DataGridCommandEventArgs e)
        {
            try
            {
                grdAllQueries.EditItemIndex = -1;
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    BindQueries(objCampaign, false);
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error cancelling edit " + ex.Message;
            }
        }

        protected void grdQueries_UpdateRecord(object source, DataGridCommandEventArgs e)
        {
            try
            {
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    string sqlStmt = "";
                    // Update all equal or lower priority queries and reduce by one to prevent duplicate priorities
                    string sqlStmt3 = string.Format("SELECT Priority, QueryID FROM CampaignQueryStatus WHERE Priority = {0}", ((TextBox)e.Item.Cells[2].Controls[0]).Text);
                    dsPriority.ConnectionString = objCampaign.CampaignDBConnString;
                    dsPriority.SelectCommand = sqlStmt3;
                    DataView dv = (DataView)dsPriority.Select(DataSourceSelectArguments.Empty);

                    if (dv.Table.Rows.Count > 0)
                    {
                        string sqlStmt2 = string.Format("SELECT Priority, QueryID FROM CampaignQueryStatus WHERE Priority >= {0}", ((TextBox)e.Item.Cells[2].Controls[0]).Text);
                        dsPriority.ConnectionString = objCampaign.CampaignDBConnString;
                        dsPriority.SelectCommand = sqlStmt2;
                        dv = (DataView)dsPriority.Select(DataSourceSelectArguments.Empty);

                        foreach (DataRow dr in dv.Table.Rows)
                        {
                            sqlStmt = string.Format("UPDATE CampaignQueryStatus SET Priority = {0} WHERE QueryID = {1}", (Convert.ToInt16(dr["Priority"]) + 1), dr["QueryID"]);
                            dsStandbyQueries.ConnectionString = objCampaign.CampaignDBConnString;
                            dsStandbyQueries.SelectCommand = sqlStmt;
                            dsStandbyQueries.Select(DataSourceSelectArguments.Empty);
                        }
                    }


                    sqlStmt = string.Format("UPDATE CampaignQueryStatus SET Priority = {0} WHERE QueryID = {1}", ((TextBox)e.Item.Cells[2].Controls[0]).Text, ((LinkButton)e.Item.Cells[12].Controls[5]).CommandArgument);
                    dsStandbyQueries.ConnectionString = objCampaign.CampaignDBConnString;
                    dsStandbyQueries.SelectCommand = sqlStmt;
                    dsStandbyQueries.Select(DataSourceSelectArguments.Empty);

                    
                    grdAllQueries.EditItemIndex = -1;
                    BindQueries(objCampaign, false);
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error updating record " + ex.Message;
            }
        }

        protected void grdQueries_DeleteRecord(object source, DataGridCommandEventArgs e)
        {
            // *** To Be Implemented
        }

        #region Status Change Button Events
        /// <summary>
        /// Move to Activate 
        /// 
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void lbtnActivate_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            LinkButton lbtnActivate = (LinkButton)sender;
            UpdateQueryStatus(Convert.ToInt64(lbtnActivate.CommandArgument), true, false, true);
            if (campaignStatusID() != (long)CampaignStatus.Idle)
                Timer1.Enabled = true;
        }

        protected void lbtnHold_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            LinkButton lbtnActivate = (LinkButton)sender;
            UpdateQueryStatus(Convert.ToInt64(lbtnActivate.CommandArgument), false, false, false);
            if (campaignStatusID() != (long)CampaignStatus.Idle)
                Timer1.Enabled = true;
        }

        protected void lbtnStandby_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            LinkButton lbtnStandby = (LinkButton)sender;
            UpdateQueryStatus(Convert.ToInt64(lbtnStandby.CommandArgument), false, true, false);
            updStandByQuery.Update();
            if (campaignStatusID() != (long)CampaignStatus.Idle)
                Timer1.Enabled = true;
        } 
        #endregion

        /// <summary>
        ///  Navigates to Query Detail
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void lbtnQuery_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            LinkButton lbtnSender = (LinkButton)sender;
            //Response.Redirect("QueryDetail.aspx?QueryID=" + lbtnSender.CommandArgument);
            Response.Redirect("QueryDetailTree.aspx?QueryID=" + lbtnSender.CommandArgument);
        }

        /// <summary>
        /// On row data binding
        /// </summary>
        protected void grdStandbyQueries_rowDataBound(object sender, GridViewRowEventArgs e)
        {  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtnActivate = (LinkButton)e.Row.FindControl("lbtnActivate");
                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

                if (Isrunning)
                {
                    lbtnActivate.Attributes.Add("onClick", "alert('You cannot move query from standby to active when campaign is running');return false;");
                    lbtnDelete.Attributes.Add("onClick", "alert('You cannot delete query when campaign is running');return false;");

                }
                else
                {
                    lbtnActivate.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to make this query Active?');");
                    lbtnDelete.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to delete this query?');");
                }

                if (bShowInPerc)
                {
                    if (e.Row.Cells[3].Text != "0")
                    {
                        e.Row.Cells[4].Text = GetPerc(e.Row.Cells[4].Text, e.Row.Cells[3].Text);
                        e.Row.Cells[5].Text = GetPerc(e.Row.Cells[5].Text, e.Row.Cells[3].Text);
                        e.Row.Cells[6].Text = GetPerc(e.Row.Cells[6].Text, e.Row.Cells[3].Text);
                        e.Row.Cells[7].Text = GetPerc(e.Row.Cells[7].Text, e.Row.Cells[3].Text);
                        e.Row.Cells[8].Text = GetPerc(e.Row.Cells[8].Text, e.Row.Cells[3].Text);
                        e.Row.Cells[9].Text = GetPerc(e.Row.Cells[9].Text, e.Row.Cells[3].Text);
                    }
                    else
                    {
                        e.Row.Cells[4].Text += "%";
                        e.Row.Cells[5].Text += "%";
                        e.Row.Cells[6].Text += "%";
                        e.Row.Cells[7].Text += "%";
                        e.Row.Cells[8].Text += "%";
                        e.Row.Cells[9].Text += "%";
                    }
                }
            }

        }

        /// <summary>
        /// On row data binding
        /// </summary>
        protected void grdActiveQueries_rowdatabound(object sender, GridViewRowEventArgs e)
        {  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
                LinkButton lbtnStandby = (LinkButton)e.Row.FindControl("lbtnStandby");
                if (Isrunning)
                {
                    lbtnStandby.Attributes.Add("onClick", "alert('You cannot move query from active to standby when campaign is running');return false;");
                    lbtnDelete.Attributes.Add("onClick", "alert('You cannot delete query when campaign is running');return false;");
                }
                else
                {
                    lbtnStandby.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to make this query stand-by?');");
                    lbtnDelete.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to delete this query?');");
                }

                if (bShowInPerc)
                {
                    if (e.Row.Cells[3].Text != "0")
                    {
                        e.Row.Cells[4].Text = GetPerc(e.Row.Cells[4].Text, e.Row.Cells[3].Text);
                        e.Row.Cells[5].Text = GetPerc(e.Row.Cells[5].Text, e.Row.Cells[3].Text);
                        e.Row.Cells[6].Text = GetPerc(e.Row.Cells[6].Text, e.Row.Cells[3].Text);
                        e.Row.Cells[7].Text = GetPerc(e.Row.Cells[7].Text, e.Row.Cells[3].Text);
                        e.Row.Cells[8].Text = GetPerc(e.Row.Cells[8].Text, e.Row.Cells[3].Text);
                        e.Row.Cells[9].Text = GetPerc(e.Row.Cells[9].Text, e.Row.Cells[3].Text);
                        e.Row.Cells[10].Text = GetPerc(e.Row.Cells[10].Text, e.Row.Cells[3].Text);
                    }
                    else
                    {
                        e.Row.Cells[4].Text += "%";
                        e.Row.Cells[5].Text += "%";
                        e.Row.Cells[6].Text += "%";
                        e.Row.Cells[7].Text += "%";
                        e.Row.Cells[8].Text += "%";
                        e.Row.Cells[9].Text += "%";
                        e.Row.Cells[10].Text += "%";
                    }
                }
            }

        }

        /// <summary>
        ///  Delete Query Detail
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            LinkButton lbtnDelete = (LinkButton)sender;
            XmlDocument xDocCampaign = new XmlDocument();
            Campaign objCampaign = (Campaign)Session["Campaign"];
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

            CampaignService objCampService = new CampaignService();
            long QueryID = Convert.ToInt64(lbtnDelete.CommandArgument);
            try
            {
                int result = objCampService.DeleteQuery(xDocCampaign, QueryID);
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
            finally
            {
                BindQueries(objCampaign, false);
                if (objCampaign.StatusID != (long)CampaignStatus.Idle)
                    Timer1.Enabled = true;
            }
        }

        /// <summary>
        ///  Refresh Active Queries 
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            try
            {
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    BindQueries(objCampaign, true);

                    if (objCampaign.StatusID != (long)CampaignStatus.Idle)
                        Timer1.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion

        #region Private Methods

        private bool bShowInPerc = false;

        /// <summary>
        /// Bind Queries.
        /// </summary>
        private void BindQueries(Campaign objCampaign, bool IsActive)
        {
            //if (IsCampaignRunning())
            //{
            //    Isrunning = true;
            //}

            DataSet dsQuerList;
            try
            {
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();

                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                ActivityLogger.WriteAdminEntry(objCampaign, "Getting campaign query status, generic.");
                dsQuerList = objCampService.GetCampaignQueryStatus(xDocCampaign);
                DataView dvQueries = new DataView();

                try
                {
                    SetQueryStatsInPerc(objCampaign);
                }
                catch { }
                

                // Added 11/12/10 for preventing 0 available queries to be activated - enhancement
                DataView dvExhaustedQueries = FilterData(dsQuerList.Tables[0], "IsActive = 1 AND Available = 0");

                if (dvExhaustedQueries == null)
                {
                    PageMessage = "This campaign has no queries, please create a dialing query to begin dialing.";
                    return;
                }

                if (dvExhaustedQueries.Count > 0)
                {
                    DataRowView dr = dvExhaustedQueries[0];
                    long campaignQueryStatusId = Convert.ToInt64(dr["CampaignQueryID"]);
                    string queryName = dr["QueryName"].ToString();
                    UpdateQueryStatus(campaignQueryStatusId, false, false, false);
                    ActivityLogger.WriteAdminEntry(objCampaign, "Getting campaign query status, exhausted queries exist.");
                    dsQuerList = objCampService.GetCampaignQueryStatus(xDocCampaign);

                    PageMessage = string.Format("\"{0}\" has no available numbers, it will remain on hold.", queryName, campaignQueryStatusId);
                    ActivityLogger.WriteAdminEntry(objCampaign, "Query '{0}' has no available numbers when user attempted to activate, moving to on hold.", queryName, campaignQueryStatusId);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "QueryAlert", "alert('" + PageMessage + "');", true);
                }

                if (IsActive)
                {
                    dvQueries = FilterData(dsQuerList.Tables[0], "IsActive = 1");
                    dvQueries.Sort = "DateModified ASC";
                    grdActiveQueries.DataSource = dvQueries;
                    grdActiveQueries.DataBind();
                }
                else
                {
                    dvQueries = FilterData(dsQuerList.Tables[0], "IsActive = 1");
                    dvQueries.Sort = "DateModified ASC";
                    grdActiveQueries.DataSource = dvQueries;
                    grdActiveQueries.DataBind();
                
                    dvQueries = FilterData(dsQuerList.Tables[0], "IsActive = 0 AND IsStandby = 1");
                    dvQueries.Sort = "DateModified ASC";
                    grdStandbyQueries.DataSource = dvQueries;
                    grdStandbyQueries.DataBind();

                    dvQueries = FilterData(dsQuerList.Tables[0], "IsActive = 0 OR IsActive = 1");
                    dvQueries.Sort = "DateModified ASC";
                    grdAllQueries.DataSource = dvQueries;
                    grdAllQueries.DataBind();
                }
                if (grdActiveQueries.Items.Count < 1)
                {
                    lblNoActive.Visible = true;
                    grdActiveQueries.Visible = false;
                }
                else
                {
                    lblNoActive.Visible = false;
                    grdActiveQueries.Visible = true;
                }
                if (grdStandbyQueries.Items.Count < 1)
                {
                    lblNoStanByQueries.Visible = true;
                    grdStandbyQueries.Visible = false;
                }
                else
                {
                    lblNoStanByQueries.Visible = false;
                    grdStandbyQueries.Visible = true;
                }
                // *** Bonus add query conditions as tooltips
                dvQueries = FilterData(dsQuerList.Tables[0], "IsActive = 1");
                dvQueries.Sort = "Priority ASC";
                for (int i = 0; i < grdActiveQueries.Items.Count; i++)
                {
                    LinkButton lbtnQuery = (LinkButton)grdActiveQueries.Items[i].FindControl("lbtnQuery");

                    string strSubQueryConditions = dvQueries.Table.Rows[i]["QueryCondition"].ToString(); ;
                    string strFilteredSubQueryConditions = "";

                    if (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") > 0)
                        strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                    if (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") > 0)
                        strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                    strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();

                    lbtnQuery.ToolTip = strFilteredSubQueryConditions;
                }
                dvQueries = FilterData(dsQuerList.Tables[0], "IsActive = 0 AND IsStandby = 1");
                dvQueries.Sort = "Priority ASC";
                for (int i = 0; i < grdStandbyQueries.Items.Count; i++)
                {
                    LinkButton lbtnQuery = (LinkButton)grdStandbyQueries.Items[i].FindControl("lbtnQuery");

                    string strSubQueryConditions = dvQueries.Table.Rows[i]["QueryCondition"].ToString(); ;
                    string strFilteredSubQueryConditions = "";

                    if (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") > 0)
                        strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                    if (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") > 0)
                        strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                    
                    strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();
                    lbtnQuery.ToolTip = strFilteredSubQueryConditions;
                }
                dvQueries = FilterData(dsQuerList.Tables[0], "IsActive = 0 OR IsActive = 1");
                dvQueries.Sort = "Priority ASC";
                for (int i = 0; i < grdAllQueries.Items.Count; i++)
                {
                    LinkButton lbtnQuery = (LinkButton)grdAllQueries.Items[i].FindControl("lbtnQuery");

                    string strSubQueryConditions = dvQueries.Table.Rows[i]["QueryCondition"].ToString(); ;
                    string strFilteredSubQueryConditions = "";

                    if (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") > 0)
                        strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                    if (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") > 0)
                        strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));

                    strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();
                    lbtnQuery.ToolTip = strFilteredSubQueryConditions;
                }
            }

            catch (Exception ex)
            {
                ActivityLogger.WriteException(ex, "Admin");
            }
        }


        /// <summary>
        /// Query stats in percentage or not
        /// </summary>
        /// <param name="objCampaign"></param>
        private void SetQueryStatsInPerc(Campaign objCampaign)
        {
            if (objCampaign != null)
            {
                CampaignService objCampaignService = new CampaignService();
                OtherParameter objOtherParameter = new OtherParameter();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                objOtherParameter = (OtherParameter)Serialize.DeserializeObject(
                    objCampaignService.GetOtherParameter(xDocCampaign), "OtherParameter");

                if (objOtherParameter.OtherParameterID != 0)
                {
                    bShowInPerc = objOtherParameter.QueryStatisticsInPercent;
                }
            }
        }


        /// <summary>
        /// Get Query Condition.
        /// </summary>
        private string GetQueryCondition(Campaign objCampaign, string strQueryID)
        {
            string strQueryCondition = "";
            DataSet dsQueryDetails;
            CampaignService objCampaignService = new CampaignService();
            XmlDocument xDocCampaign = new XmlDocument();
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            dsQueryDetails = objCampaignService.GetQueryDetailsByQueryID(xDocCampaign, strQueryID);
            if (dsQueryDetails.Tables[0].Rows.Count > 0)
            {
                strQueryCondition = dsQueryDetails.Tables[0].Rows[0]["QueryCondition"].ToString();
            }
            return strQueryCondition;

        }

        /// <summary>
        /// Update Query Status.
        /// </summary>
        private void UpdateQueryStatus(Int64 cqStatusID, bool isActive, bool isStandby, bool resetStatus)
        {
            XmlDocument xDocCampaign = new XmlDocument();
            Campaign objCampaign = (Campaign)Session["Campaign"];
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            CampaignService objCampService = new CampaignService();
            try
            {
                //ErrorLogger.Write("Updating query status.");
                objCampService.CampaignQueryStatusUpdate(xDocCampaign, cqStatusID, isActive, isStandby, false, resetStatus);
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
            BindQueries(objCampaign, false);
        }

        /// <summary>
        /// Returns a campaign status Id
        /// </summary>
        /// <returns></returns>
        private long campaignStatusID()
        {
            Campaign objCampaign;
            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];
                return objCampaign.StatusID;
            }
            return 0;
        }

        private string GetPerc(string a, string b)
        {
            string result = "0%";
            try
            {
                if (a != "" && b != "")
                {
                    if (b != "0")
                    {
                        result = Convert.ToString(Math.Round((Convert.ToDouble(a) / Convert.ToDouble(b)) * 100, 2)) + "%";
                    }
                }
            }
            catch { }
            return result;
        }

        #endregion
    }
}
