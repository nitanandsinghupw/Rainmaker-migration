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
using System.Text;
using System.Collections.Generic;
using Rainmaker.Web.CampaignWS;
using Rainmaker.Web.AgentsWS;

namespace Rainmaker.Web.campaign
{
    public partial class QueryDetails : PageBase
    {
        string strQueryCondition = string.Empty;

        #region Events
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Campaign objCampaign;
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                    anchHome.InnerText = objCampaign.ShortDescription;
                    GetQueryConditions(objCampaign);
                }
            }
            try
            { grdQueryConditions.HeaderRow.TableSection = TableRowSection.TableHeader;}
            catch { }
            try
            {
                if (hdnQueryToOverwrite.Value == "true")
                {
                    // Delete the existing query
                    if (txtQueryName.Text.Length > 0)
                    {
                        Campaign objCampaign;
                        objCampaign = (Campaign)Session["Campaign"];
                        CampaignService objCampaignService = new CampaignService();
                        XmlDocument xDocCampaign = new XmlDocument();
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                        objCampaignService.DeleteQueryByName(xDocCampaign, txtQueryName.Text);

                        // Save query to avoid duplicate exception
                        SaveData();
                    }
                }
                hdnQueryToOverwrite.Value = "";
                hdnDuplicateQuery.Value = "";
            }
            catch (Exception ex)
            {
                PageMessage = "Exception saving duplicate query :" + ex.Message;
            }

            try
            {
                if (hdnShowSQL.Value == "true")
                {
                    BuildSQLLabel();
                    tblSQL.Visible = true;
                    lbtnHideSQL.Visible = true;
                    lbtnShowSQL.Visible = false;
                }
                else
                {
                    tblSQL.Visible = false;
                    lbtnHideSQL.Visible = false;
                    lbtnShowSQL.Visible = true;
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Exception showing / hiding raw sql :" + ex.Message;
            }

        }

        protected void lbtnShowHideSQL_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lbtnSender = (LinkButton)sender;

                if (lbtnSender.CommandName == "show")
                {
                    hdnShowSQL.Value = "true";
                    lbtnShowSQL.Visible = false;
                    lbtnHideSQL.Visible = true;
                    tblSQL.Visible = true;
                }
                else
                {
                    hdnShowSQL.Value = "false";
                    lbtnShowSQL.Visible = true;
                    lbtnHideSQL.Visible = false;
                    tblSQL.Visible = false;
                }
                if (hdnShowSQL.Value == "true")
                {
                    BuildSQLLabel();
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Exception in show/hide event :" + ex.Message;
            }
        }

        /// <summary>
        /// Query Detail RowDataBound
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void grdQueryConditions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try 
	            {	        
            		
                    if (e.Row.RowType == DataControlRowType.DataRow)
                    {
                        Label lblSubQuery = (Label)e.Row.FindControl("lblSubQuery");
                        DropDownList ddlCriteria = (DropDownList)e.Row.FindControl("ddlCriteria");
                        DropDownList ddlOperator = (DropDownList)e.Row.FindControl("ddlOperator");
                        DropDownList ddlLogical = (DropDownList)e.Row.FindControl("ddlLogical");
                        TextBox txtEnterValue = (TextBox)e.Row.FindControl("txtEnterValue");
                        DropDownList ddlPickByName = (DropDownList)e.Row.FindControl("ddlPickByName");

                        HiddenField hdnCriteria = (HiddenField)e.Row.FindControl("hdnCriteria");
                        HiddenField hdnOperator = (HiddenField)e.Row.FindControl("hdnOperator");
                        HiddenField hdnSearchString = (HiddenField)e.Row.FindControl("hdnSearchString");
                        HiddenField hdnLogical = (HiddenField)e.Row.FindControl("hdnLogical");

                        LinkButton lbtnsender = (LinkButton)e.Row.FindControl("lbtnDelete");
                        lbtnsender.Attributes.Add("onclick", "return onGridViewRowSelected('" + e.Row.RowIndex.ToString() + "')");

                        BindColumnsDropdown(ddlCriteria, new ListItem("Select Criteria", "0"));

                        // Added for query list expansion
                        BindAdditionalFields(ddlCriteria);

                        if (hdnCriteria.Value != "")
                            ddlCriteria.Items.FindByText(hdnCriteria.Value).Selected = true;

                        // 2012-06-12 Dave Pollastrini
                        // Changed BindOperator to take a datatype.
                        /*
                        bool isDateField = false;
                        try
                        {
                            isDateField = ddlCriteria.SelectedValue.IndexOf(":date") > 0;
                        }
                        catch { }

                        BindOperator(ddlOperator, isDateField);
                        */

                        string dataType = ddlCriteria.SelectedValue.Substring(ddlCriteria.SelectedValue.IndexOf(":") + 1, (ddlCriteria.SelectedValue.Length - (ddlCriteria.SelectedValue.IndexOf(":") + 1))).ToLower();
                        BindOperator(ddlOperator, dataType);

                        BindLogicalOperator(ddlLogical);

                        if (hdnOperator.Value != "")
                            ddlOperator.SelectedValue = hdnOperator.Value;
                        if (hdnCriteria.Value != "")
                            txtEnterValue.Text = hdnSearchString.Value;
                        if (hdnLogical.Value != "")
                            ddlLogical.SelectedValue = hdnLogical.Value.Trim();
                    }
	            }
	            catch (Exception ex)
	            {
                    PageMessage = "Exception in row data bind event " + ex.Message;

	            }


            
        }
        /// <summary>
        /// Add Filter Line 
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void lbtnAddFilter_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    return;
                }
            }
            catch{}

            Campaign objCampaign = new Campaign();
            try
            {
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                }
                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                LinkButton lbtnsender = (LinkButton)sender;
                LinkButton lbtnDelete = (LinkButton)sender;
                DataTable dtAddFilter = new DataTable();
                dtAddFilter.Columns.Add("QueryDetailID");
                dtAddFilter.Columns.Add("SearchCriteria");
                dtAddFilter.Columns.Add("SearchOperator");
                dtAddFilter.Columns.Add("SearchString");
                dtAddFilter.Columns.Add("LogicalOrder");
                dtAddFilter.Columns.Add("LogicalOperator");
                dtAddFilter.Columns.Add("SubQueryID");

                DropDownList ddlCriteria = null;
                DropDownList ddlOperator = null;
                DropDownList ddlPickByName = null;
                DropDownList ddlLogical = null;
                TextBox txtEnterValue = null;
                Label lblSubQuery = null;
                HiddenField hdnQueryDetailID = null;
                HiddenField hdnSubQueryID = null;
                CompareValidator cmpCriteria = null;
                CompareValidator cmpOperator = null;

                bool IsDelete = false;
                int deleteDataRow = 0;
                foreach (GridViewRow row in grdQueryConditions.Rows)
                {
                    ddlCriteria = (DropDownList)row.FindControl("ddlCriteria");
                    ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                    txtEnterValue = (TextBox)row.FindControl("txtEnterValue");
                    ddlLogical = (DropDownList)row.FindControl("ddlLogical");
                    ddlPickByName = (DropDownList)row.FindControl("ddlPickByName");
                    hdnQueryDetailID = (HiddenField)row.FindControl("hdnQueryDetailID");
                    lblSubQuery = (Label)row.FindControl("lblSubQuery");
                    hdnSubQueryID = (HiddenField)row.FindControl("hdnSubQueryID");
                    cmpCriteria = (CompareValidator)row.FindControl("cmpCriteria");
                    cmpOperator = (CompareValidator)row.FindControl("cmpOperator");

                    DataRow dr = dtAddFilter.NewRow();
                    if (!string.IsNullOrEmpty(hdnSubQueryID.Value))
                    {
                        // this is a sub query row
                        dr["SubQueryID"] = hdnSubQueryID.Value.ToString();
                    }
                    else
                    {
                        dr["SearchCriteria"] = ddlCriteria.SelectedItem.Text;
                        dr["SearchOperator"] = ddlOperator.SelectedValue;

                        CheckForNameDropDowns(ddlCriteria, ddlOperator, ddlPickByName, txtEnterValue);
                        if (txtEnterValue.Visible)
                            dr["SearchString"] = txtEnterValue.Text.Trim();
                        else
                            dr["SearchString"] = ddlPickByName.SelectedValue.Trim();
                    }
                    dr["LogicalOperator"] = ddlLogical.SelectedValue;
                    if (hdnQueryDetailID.Value != "0" || hdnQueryDetailID.Value != "")
                        dr["QueryDetailID"] = hdnQueryDetailID.Value;
                    else
                        dr["QueryDetailID"] = 0;
                    dtAddFilter.Rows.Add(dr);

                    if (lbtnsender.CommandArgument == "Delete")
                    {
                        if (hdnQueryDetailID.Value != "0")
                            deleteDataRow = deleteDataRow + 1;
                        IsDelete = true;
                    }

                }
                if (!IsDelete)
                {
                    DataRow drAddFilterRow = dtAddFilter.NewRow();
                    drAddFilterRow["QueryDetailID"] = 0;
                    dtAddFilter.Rows.Add(drAddFilterRow);
                    lbtnDelete.Visible = true;
                    hdnCount.Value = dtAddFilter.Rows.Count.ToString();
                    grdQueryConditions.DataSource = dtAddFilter;
                    grdQueryConditions.DataBind();
                    for (int i = 0; i < grdQueryConditions.Rows.Count; i++)
                    {
                        ddlCriteria = (DropDownList)grdQueryConditions.Rows[i].FindControl("ddlCriteria");
                        ddlOperator = (DropDownList)grdQueryConditions.Rows[i].FindControl("ddlOperator");
                        txtEnterValue = (TextBox)grdQueryConditions.Rows[i].FindControl("txtEnterValue");
                        ddlLogical = (DropDownList)grdQueryConditions.Rows[i].FindControl("ddlLogical");
                        ddlPickByName = (DropDownList)grdQueryConditions.Rows[i].FindControl("ddlPickByName");
                        hdnSubQueryID = (HiddenField)grdQueryConditions.Rows[i].FindControl("hdnSubQueryID");
                        lblSubQuery = (Label)grdQueryConditions.Rows[i].FindControl("lblSubQuery");
                        cmpCriteria = (CompareValidator)grdQueryConditions.Rows[i].FindControl("cmpCriteria");
                        cmpOperator = (CompareValidator)grdQueryConditions.Rows[i].FindControl("cmpOperator");

                        if (!string.IsNullOrEmpty(dtAddFilter.Rows[i]["SubQueryID"].ToString()))
                        {
                            // Enable label
                            lblSubQuery.Enabled = true;
                            lblSubQuery.Visible = true;
                            // Get query name, filter condition and set tooltip
                            string strSubQueryID = dtAddFilter.Rows[i]["SubQueryID"].ToString();
                            DataSet dsSubQueryDetails;
                            DataTable dtSubQueryConditions = new DataTable();
                            dsSubQueryDetails = objCampaignService.GetQueryDetailsByQueryID(xDocCampaign, strSubQueryID);
                            dtSubQueryConditions = dsSubQueryDetails.Tables[0];
                            lblSubQuery.Text = "Query: " + dtSubQueryConditions.Rows[0]["QueryName"].ToString();
                            string strSubQueryConditions = dtSubQueryConditions.Rows[0]["QueryCondition"].ToString(); ;
                            string strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                            strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();
                            lblSubQuery.ToolTip = strFilteredSubQueryConditions;
                            hdnSubQueryID.Value = dtAddFilter.Rows[i]["SubQueryID"].ToString();
                            cmpCriteria.Enabled = false;
                            cmpOperator.Enabled = false;
                            // Disable ddlCriteria - invisible
                            ddlCriteria.Visible = false;

                            // Disable ddl operator, pickbyname or txtxentervalue
                            ddlOperator.Visible = false;
                            ddlPickByName.Visible = false;
                            txtEnterValue.Visible = false;
                        }
                        else
                        {
                            CheckForNameDropDowns(ddlCriteria, ddlOperator, ddlPickByName, txtEnterValue);
                            if (ddlPickByName.Visible)
                            {
                                ddlPickByName.SelectedValue = dtAddFilter.Rows[i]["SearchString"].ToString();
                            }
                        }
                    }
                }
                if (IsDelete)
                {
                    long QueryDetailID = Convert.ToInt64(lbtnDelete.CommandName);
                    try
                    {
                        int result = 0;
                        if (QueryDetailID != 0 && deleteDataRow > 1)
                        {
                            result = objCampaignService.DeleteQueryDetail(xDocCampaign, QueryDetailID);
                            if (hdnDeleteCount.Value != "")
                                dtAddFilter.Rows.RemoveAt(Convert.ToInt32(hdnDeleteCount.Value));
                            IsDelete = false;

                            UpdateQueryCondition(objCampaign);
                        }
                        if (QueryDetailID == 0 && dtAddFilter.Rows.Count > 1)
                        {
                            if (hdnDeleteCount.Value != "")
                                dtAddFilter.Rows.RemoveAt(Convert.ToInt32(hdnDeleteCount.Value));
                            IsDelete = false;

                        }
                        if ((IsDelete && dtAddFilter.Rows.Count <= 1) || (QueryDetailID != 0 && deleteDataRow == 1))
                        {
                            PageMessage = "At least one condition is required for the query.";
                        }

                        hdnCount.Value = dtAddFilter.Rows.Count.ToString();

                     

                    }
                    catch (Exception ex)
                    {
                        PageMessage = ex.Message;
                    }
                }
                if (hdnShowSQL.Value == "true")
                {
                    BuildSQLLabel();
                }
            }
            catch (Exception ex)
            {
                PageMessage = "There has been an error adding or deleting a filter condition: " + ex.Message;
            }

        }

        protected void lbtnAddSubQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsValid)
                {
                    return;
                }
            }
            catch { }
            
            Campaign objCampaign = new Campaign();
            try
            {
                if (Session["SubQueryToAddID"] != null)
                {
                    if (Session["Campaign"] != null)
                    {
                        objCampaign = (Campaign)Session["Campaign"];
                    }
                    CampaignService objCampaignService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    DataTable dtAddSubQuery = new DataTable();

                    dtAddSubQuery.Columns.Add("QueryDetailID");
                    dtAddSubQuery.Columns.Add("SearchCriteria");
                    dtAddSubQuery.Columns.Add("SearchOperator");
                    dtAddSubQuery.Columns.Add("SearchString");
                    dtAddSubQuery.Columns.Add("LogicalOrder");
                    dtAddSubQuery.Columns.Add("LogicalOperator");
                    dtAddSubQuery.Columns.Add("SubQueryID");

                    DropDownList ddlCriteria = null;
                    DropDownList ddlOperator = null;
                    DropDownList ddlPickByName = null;
                    DropDownList ddlLogical = null;
                    TextBox txtEnterValue = null;
                    Label lblSubQuery = null;
                    HiddenField hdnQueryDetailID = null;
                    HiddenField hdnSubQueryID = null;
                    CompareValidator cmpCriteria = null;
                    CompareValidator cmpOperator = null;

                    foreach (GridViewRow row in grdQueryConditions.Rows)
                    {
                        ddlCriteria = (DropDownList)row.FindControl("ddlCriteria");
                        ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                        txtEnterValue = (TextBox)row.FindControl("txtEnterValue");
                        ddlLogical = (DropDownList)row.FindControl("ddlLogical");
                        ddlPickByName = (DropDownList)row.FindControl("ddlPickByName");
                        hdnQueryDetailID = (HiddenField)row.FindControl("hdnQueryDetailID");
                        lblSubQuery = (Label)row.FindControl("lblSubQuery");
                        hdnSubQueryID = (HiddenField)row.FindControl("hdnSubQueryID");
                        cmpCriteria = (CompareValidator)row.FindControl("cmpCriteria");
                        cmpOperator = (CompareValidator)row.FindControl("cmpOperator");

                        DataRow dr = dtAddSubQuery.NewRow();
                        if (!string.IsNullOrEmpty(hdnSubQueryID.Value))
                        {
                            // this is a sub query row
                            dr["SubQueryID"] = hdnSubQueryID.Value.ToString();
                        }
                        else
                        {
                            dr["SearchCriteria"] = ddlCriteria.SelectedItem.Text;
                            dr["SearchOperator"] = ddlOperator.SelectedValue;
                            
                            CheckForNameDropDowns(ddlCriteria, ddlOperator, ddlPickByName, txtEnterValue);
                            if (txtEnterValue.Visible)
                                dr["SearchString"] = txtEnterValue.Text.Trim();
                            else
                                dr["SearchString"] = ddlPickByName.SelectedValue.Trim();
                        }
                        dr["LogicalOperator"] = ddlLogical.SelectedValue;
                        if (hdnQueryDetailID.Value != "0" || hdnQueryDetailID.Value != "")
                            dr["QueryDetailID"] = hdnQueryDetailID.Value;
                        else
                            dr["QueryDetailID"] = 0;
                        dtAddSubQuery.Rows.Add(dr);
                    }
                    DataRow drAddFilterRow = dtAddSubQuery.NewRow();
                    drAddFilterRow["QueryDetailID"] = 0;
                    dtAddSubQuery.Rows.Add(drAddFilterRow);
                    hdnCount.Value = dtAddSubQuery.Rows.Count.ToString();
                    grdQueryConditions.DataSource = dtAddSubQuery;
                    grdQueryConditions.DataBind();
                    for (int i = 0; i < grdQueryConditions.Rows.Count; i++)
                    {
                        ddlCriteria = (DropDownList)grdQueryConditions.Rows[i].FindControl("ddlCriteria");
                        ddlOperator = (DropDownList)grdQueryConditions.Rows[i].FindControl("ddlOperator");
                        txtEnterValue = (TextBox)grdQueryConditions.Rows[i].FindControl("txtEnterValue");
                        ddlLogical = (DropDownList)grdQueryConditions.Rows[i].FindControl("ddlLogical");
                        ddlPickByName = (DropDownList)grdQueryConditions.Rows[i].FindControl("ddlPickByName");
                        hdnSubQueryID = (HiddenField)grdQueryConditions.Rows[i].FindControl("hdnSubQueryID");
                        lblSubQuery = (Label)grdQueryConditions.Rows[i].FindControl("lblSubQuery");
                        cmpCriteria = (CompareValidator)grdQueryConditions.Rows[i].FindControl("cmpCriteria");
                        cmpOperator = (CompareValidator)grdQueryConditions.Rows[i].FindControl("cmpOperator");

                        if (!string.IsNullOrEmpty(dtAddSubQuery.Rows[i]["SubQueryID"].ToString()))
                        {
                            // Enable label
                            lblSubQuery.Enabled = true;
                            lblSubQuery.Visible = true;
                            // Get query name, filter condition and set tooltip
                            string strSubQueryID = dtAddSubQuery.Rows[i]["SubQueryID"].ToString();
                            DataSet dsSubQueryDetails;
                            DataTable dtSubQueryConditions = new DataTable();
                            dsSubQueryDetails = objCampaignService.GetQueryDetailsByQueryID(xDocCampaign, strSubQueryID);
                            dtSubQueryConditions = dsSubQueryDetails.Tables[0];
                            lblSubQuery.Text = "Query: " + dtSubQueryConditions.Rows[0]["QueryName"].ToString();
                            string strSubQueryConditions = dtSubQueryConditions.Rows[0]["QueryCondition"].ToString(); ;
                            string strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                            strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();
                            lblSubQuery.ToolTip = strFilteredSubQueryConditions;
                            hdnSubQueryID.Value = dtAddSubQuery.Rows[i]["SubQueryID"].ToString();
                            cmpCriteria.Enabled = false;
                            cmpOperator.Enabled = false;
                            // Disable ddlCriteria - invisible
                            ddlCriteria.Visible = false;

                            // Disable ddl operator, pickbyname or txtxentervalue
                            ddlOperator.Visible = false;
                            ddlPickByName.Visible = false;
                            txtEnterValue.Visible = false;
                        }
                        else
                        {
                            CheckForNameDropDowns(ddlCriteria, ddlOperator, ddlPickByName, txtEnterValue);
                            if (ddlPickByName.Visible)
                            {
                                ddlPickByName.SelectedValue = dtAddSubQuery.Rows[i]["SearchString"].ToString();
                            }
                        }
                        if (i == (grdQueryConditions.Rows.Count - 1))
                        {
                            // This is the last row, add it as a sub query
                            string strSubQueryID = Session["SubQueryToAddID"].ToString();
                            DataSet dsSubQueryDetails;
                            DataTable dtSubQueryConditions = new DataTable();
                            dsSubQueryDetails = objCampaignService.GetQueryDetailsByQueryID(xDocCampaign, strSubQueryID);
                            dtSubQueryConditions = dsSubQueryDetails.Tables[0];
                            lblSubQuery.Text = "Query: " + dtSubQueryConditions.Rows[0]["QueryName"].ToString();
                            string strSubQueryConditions = dtSubQueryConditions.Rows[0]["QueryCondition"].ToString(); ;
                            string strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                            strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();
                            lblSubQuery.ToolTip = strFilteredSubQueryConditions;
                            hdnSubQueryID.Value = Session["SubQueryToAddID"].ToString();
                            cmpCriteria.Enabled = false;
                            cmpOperator.Enabled = false;
                            // Disable ddlCriteria - invisible
                            ddlCriteria.Visible = false;
                            // Enable label
                            lblSubQuery.Visible = true;
                            // Disable ddl operator, pickbyname or txtxentervalue
                            ddlOperator.Visible = false;
                            ddlPickByName.Visible = false;
                            txtEnterValue.Visible = false;
                        }
                    } 
                }
                Session["SubQueryToAddID"] = null;
                Session["SubQueryToAddName"] = null;
                if (hdnShowSQL.Value == "true")
                {
                    BuildSQLLabel();
                }
            }
            catch (Exception ex)
            {
                PageMessage = "There has been an error adding or deleting a sub query: " + ex.Message;
            }

        }
        /// <summary>
        /// Updata Query Condition On Deletion 
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void UpdateQueryCondition(Campaign objCampaign)
        {
            int intLogicalOrder = 1;
            string strQueryCondition = "";
            StringBuilder sbQueryCondition = new StringBuilder();
            foreach (GridViewRow row in grdQueryConditions.Rows)
            {

                DropDownList ddlCriteria = (DropDownList)row.FindControl("ddlCriteria");
                DropDownList ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                DropDownList ddlLogical = (DropDownList)row.FindControl("ddlLogical");
                TextBox txtEnterValue = (TextBox)row.FindControl("txtEnterValue");
                HiddenField hdnQueryDetailID = (HiddenField)row.FindControl("hdnQueryDetailID");

                if (hdnQueryDetailID.Value != "0")
                {
                    if (ddlCriteria.SelectedValue != "0" && ddlOperator.SelectedValue != "0")
                    {
                        sbQueryCondition.Append(" ");
                        sbQueryCondition.AppendFormat(ddlOperator.SelectedValue, ddlCriteria.SelectedItem.Text, txtEnterValue.Text.Trim().Replace("'", "''"));
                        sbQueryCondition.AppendFormat(" {0} ", intLogicalOrder == grdQueryConditions.Rows.Count ? "" : ddlLogical.SelectedValue);
                        intLogicalOrder++;

                    }
                }
            }

            if (sbQueryCondition.ToString().LastIndexOf("And") == sbQueryCondition.ToString().Length - 4)
                sbQueryCondition = sbQueryCondition.Remove(sbQueryCondition.ToString().LastIndexOf("And"), 3);
            if (sbQueryCondition.ToString().LastIndexOf("OR") == sbQueryCondition.ToString().Length - 3)
                sbQueryCondition = sbQueryCondition.Remove(sbQueryCondition.ToString().LastIndexOf("OR"), 3);
            if (sbQueryCondition.ToString() != "")
            {
                strQueryCondition = BuildQueryCondition(sbQueryCondition.ToString());

                if (strQueryCondition != "")
                {
                    CampaignService objCampService = new CampaignService();
                    Query objQuery = new Query();
                    if (Request.QueryString["QueryID"] != null)
                    {
                        objQuery.QueryID = Convert.ToInt32(Request.QueryString["QueryID"]);
                    }
                    try
                    {
                        objQuery.QueryName = txtQueryName.Text;
                        objQuery.QueryCondition = strQueryCondition;
                        List<XmlNode> queryDetailList = new List<XmlNode>();
                        XmlDocument xDocQuery = new XmlDocument();
                        XmlDocument xDocCampaign = new XmlDocument();
                        xDocQuery.LoadXml(Serialize.SerializeObject(objQuery, "Query"));
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                        objQuery = (Query)Serialize.DeserializeObject(objCampService.QueryDetailInsertUpdate(
                            xDocCampaign, queryDetailList.ToArray(), xDocQuery), "Query");
                    }
                    catch (Exception ex)
                    {
                        PageMessage = ex.Message;
                    }

                }
            }

        }
        /// <summary>
        /// Save Query Detail 
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }
            //if (!IsCampaignRunning())
                SaveData();
            //else
            //    PageMessage = "You cannot add/modify query details when campaign is running";
        }
        /// <summary>
        /// Cancel Query Detail 
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["QueryID"] != null)
                Response.Redirect("~/campaign/QueryDetail.aspx?QueryID=" + Request.QueryString["QueryID"].ToString());
            else
                Response.Redirect("~/campaign/QueryDetail.aspx");

        }

        protected void lbtnClose_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["DataManager"] != null)
            {
                Response.Redirect("~/campaign/DataPortal.aspx");
            }
            else
            {
                Response.Redirect("~/campaign/QueryStatus.aspx");
            }
        }
        /// <summary>
        /// Test Query  
        /// </summary>
        /// <param name="sender">sender.</param>
        /// <param name="e">e.</param>
        protected void lbtnTestQuery_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }
            try
            {
                if (Session["Campaign"] != null)
                {
                    DataSet dsCampaignQueryData;
                    Campaign objCampaign;
                    CampaignService objCampaignService = new CampaignService();

                    objCampaign = (Campaign)Session["Campaign"];

                    int intLogicalOrder = 1;
                    string strQueryCondition = "";
                    StringBuilder sbQueryCondition = new StringBuilder();
                    foreach (GridViewRow row in grdQueryConditions.Rows)
                    {
                        DropDownList ddlCriteria = (DropDownList)row.FindControl("ddlCriteria");
                        DropDownList ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                        DropDownList ddlLogical = (DropDownList)row.FindControl("ddlLogical");
                        TextBox txtEnterValue = (TextBox)row.FindControl("txtEnterValue");
                        DropDownList ddlPickByName = (DropDownList)row.FindControl("ddlPickByName");
                        HiddenField hdnQueryDetailID = (HiddenField)row.FindControl("hdnQueryDetailID");
                        Label lblSubQuery = (Label)row.FindControl("lblSubQuery");
                        HiddenField hdnSubQueryID = (HiddenField)row.FindControl("hdnSubQueryID");
                        if (string.IsNullOrEmpty(hdnSubQueryID.Value))
                        {
                            string operatorCondition = ddlOperator.SelectedValue;
                            if (txtEnterValue.Text.Trim().Length <= 10 && ddlCriteria.SelectedValue.ToLower().IndexOf(":date") >= 0
                                && ddlOperator.SelectedItem.Text == "Equals")
                            {
                                try
                                {
                                    txtEnterValue.Text = Convert.ToDateTime(txtEnterValue.Text.Trim()).ToString("MM/dd/yyyy");
                                }
                                catch { }
                                operatorCondition = "Convert(Varchar(10),{0},101) = '{1}'";
                            }

                            string enteredValue = "";

                            if (txtEnterValue.Visible)
                                enteredValue = txtEnterValue.Text.Trim();
                            else
                                enteredValue = ddlPickByName.SelectedValue.Trim();


                            if (ddlCriteria.SelectedValue.ToLower().IndexOf(":date") >= 0)
                            {
                                if (enteredValue.Length <= 10)
                                {
                                    if (ddlOperator.SelectedItem.Text == "Greater Than" ||
                                        ddlOperator.SelectedItem.Text == "Less than Equal")
                                    {
                                        enteredValue = enteredValue + " 23:59:59";
                                    }
                                }
                            }
                            // ************* Checked to here
                            string criteriaValue = ddlCriteria.SelectedValue.ToString().Substring(0, ddlCriteria.SelectedValue.ToString().IndexOf(":"));


                            sbQueryCondition.Append(" ");
                            sbQueryCondition.AppendFormat(operatorCondition, criteriaValue, enteredValue.Replace("'", "''"));
                            

                            // ************ Below differs from savedata
                            string strOp = ddlOperator.SelectedValue;

                            // 2012-06-12 Dave Pollastrini
                            // Changed BindOperator to take a datatype.
                            /*
                            bool isDateField = false;
                            try
                            {
                                isDateField = ddlCriteria.SelectedValue.IndexOf(":date") > 0;
                            }
                            catch { }

                            BindOperator(ddlOperator, isDateField);
                            */

                            string dataType = ddlCriteria.SelectedValue.Substring(ddlCriteria.SelectedValue.IndexOf(":") + 1, (ddlCriteria.SelectedValue.Length - (ddlCriteria.SelectedValue.IndexOf(":") + 1))).ToLower();
                            BindOperator(ddlOperator, dataType);

                            ddlOperator.SelectedValue = strOp;
                        }
                        else
                        {
                            string strSubQueryID = hdnSubQueryID.Value;
                            DataSet dsSubQueryDetails;
                            DataTable dtSubQueryConditions = new DataTable();
                            XmlDocument xDocCampaign1 = new XmlDocument();
                            xDocCampaign1.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                            CampaignService objCampaignService1 = new CampaignService();
                            dsSubQueryDetails = objCampaignService1.GetQueryDetailsByQueryID(xDocCampaign1, strSubQueryID);
                            dtSubQueryConditions = dsSubQueryDetails.Tables[0];
                            string strSubQueryConditions = dtSubQueryConditions.Rows[0]["QueryCondition"].ToString();
                            string strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                            strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();
                            sbQueryCondition.Append(" ");
                            sbQueryCondition.AppendFormat("({0})", strFilteredSubQueryConditions);
                        }
                        sbQueryCondition.AppendFormat(" {0} ", intLogicalOrder == grdQueryConditions.Rows.Count ? "" : ddlLogical.SelectedValue);
                        intLogicalOrder++; // ???????????????????
                    }


                    strQueryCondition = BuildQueryCondition(sbQueryCondition.ToString());

                    if (strQueryCondition != "")
                    {
                        try
                        {
                            //XmlDocument xDocCampaign = new XmlDocument();
                            //xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                            dsCampaignQueryData = objCampaignService.GetCampaignData(objCampaign.CampaignDBConnString, strQueryCondition);
                            if (dsCampaignQueryData.Tables[0] != null)
                                PageMessage = "Query Executed Successfully" + Environment.NewLine + "Available Count: " + dsCampaignQueryData.Tables[0].Rows.Count.ToString();
                        }
                        catch
                        {
                            PageMessage = "Error Executing Query";
                        }
                    }
                    else
                    {
                        PageMessage = "There should be at least one Query Condition to test the Query!";
                    }

                }
            }
            catch (Exception ex)
            {
                string strMsg = ex.Message;
                PageMessage = "There has been an error testing your query, please make sure your conditions are valid and try again.";
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Get Query Detail.
        /// </summary>
        private void GetQueryConditions(Campaign objCampaign)
        {
            DataTable dtQueryConditions = new DataTable();
            try
            {
                if (Request.QueryString["QueryID"] == null)
                {
                    lblRODate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    lblROModifiedDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
                    dtQueryConditions.Columns.Add("QueryDetailID");
                    dtQueryConditions.Columns.Add("SearchCriteria");
                    dtQueryConditions.Columns.Add("SearchOperator");
                    dtQueryConditions.Columns.Add("SearchString");
                    dtQueryConditions.Columns.Add("LogicalOrder");
                    dtQueryConditions.Columns.Add("LogicalOperator");

                    DataRow dr = dtQueryConditions.NewRow();
                    dr["QueryDetailID"] = 0;
                    dtQueryConditions.Rows.Add(dr);
                    grdQueryConditions.DataSource = dtQueryConditions;
                    grdQueryConditions.DataBind();

                }
                else
                {

                    DataSet dsQueryDetails;
                    string strQueryID = Request.QueryString["QueryID"].ToString();
                    CampaignService objCampaignService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    dsQueryDetails = objCampaignService.GetQueryDetailsByQueryID(xDocCampaign, strQueryID);
                    dtQueryConditions = dsQueryDetails.Tables[0];
                    grdQueryConditions.DataSource = dtQueryConditions;
                    grdQueryConditions.DataBind();

                    if (dtQueryConditions.Rows.Count > 0)
                    {
                        txtQueryName.Text = dtQueryConditions.Rows[0]["QueryName"].ToString();
                        lblRODate.Text = Convert.ToDateTime(dtQueryConditions.Rows[0]["DateCreated"]).ToString("MM/dd/yyyy");
                        lblROModifiedDate.Text = Convert.ToDateTime(dtQueryConditions.Rows[0]["DateModified"]).ToString("MM/dd/yyyy");
                        strQueryCondition = dtQueryConditions.Rows[0]["QueryCondition"].ToString();
                        if (strQueryCondition != "")
                        {
                            //XmlDocument xDocCampaignData = new XmlDocument();
                            DataSet dsCampaignQueryData;
                            //xDocCampaignData.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                            dsCampaignQueryData = objCampaignService.GetCampaignData(objCampaign.CampaignDBConnString, strQueryCondition);
                            if (dsCampaignQueryData.Tables[0] != null)
                                hdnQueryCount.Value = dsCampaignQueryData.Tables[0].Rows.Count.ToString();
                            // *** Added for issue 106 
                            DropDownList ddlCriteria = null;
                            DropDownList ddlOperator = null;
                            DropDownList ddlPickByName = null;
                            TextBox txtEnterValue = null;
                            Label lblSubQuery = null;
                            Label lblLevelIndicator = null;
                            HiddenField hdnSubQueryID = null;
                            CompareValidator cmpCriteria = null;
                            CompareValidator cmpOperator = null;

                            for (int i = 0; i < grdQueryConditions.Rows.Count; i++)
                            {
                                ddlCriteria = (DropDownList)grdQueryConditions.Rows[i].FindControl("ddlCriteria");
                                ddlOperator = (DropDownList)grdQueryConditions.Rows[i].FindControl("ddlOperator");
                                ddlPickByName = (DropDownList)grdQueryConditions.Rows[i].FindControl("ddlPickByName");
                                txtEnterValue = (TextBox)grdQueryConditions.Rows[i].FindControl("txtEnterValue");
                                lblSubQuery = (Label)grdQueryConditions.Rows[i].FindControl("lblSubQuery");
                                lblLevelIndicator = (Label)grdQueryConditions.Rows[i].FindControl("lblLevelIndicator");
                                hdnSubQueryID = (HiddenField)grdQueryConditions.Rows[i].FindControl("hdnSubQueryID");
                                cmpCriteria = (CompareValidator)grdQueryConditions.Rows[i].FindControl("cmpCriteria");
                                cmpOperator = (CompareValidator)grdQueryConditions.Rows[i].FindControl("cmpOperator");

                                CheckForNameDropDowns(ddlCriteria, ddlOperator, ddlPickByName, txtEnterValue);
                                if (ddlPickByName.Visible)
                                {
                                    ddlPickByName.SelectedValue = dtQueryConditions.Rows[i]["SearchString"].ToString();
                                }
                                // Added for new query manager - subquries feature
                                if (dtQueryConditions.Rows[i]["SubQueryID"] != DBNull.Value)
                                {
                                    if (Convert.ToInt64(dtQueryConditions.Rows[i]["SubQueryID"]) > 0)
                                    {
                                        // Get query name, filter condition and set tooltip
                                        string strSubQueryID = dtQueryConditions.Rows[i]["SubQueryID"].ToString();
                                        DataSet dsSubQueryDetails;
                                        DataTable dtSubQueryConditions = new DataTable();
                                        dsSubQueryDetails = objCampaignService.GetQueryDetailsByQueryID(xDocCampaign, strSubQueryID);
                                        dtSubQueryConditions = dsSubQueryDetails.Tables[0];
                                        lblSubQuery.Text = "Query: " + dtSubQueryConditions.Rows[0]["QueryName"].ToString();
                                        string strSubQueryConditions = dtSubQueryConditions.Rows[0]["QueryCondition"].ToString(); ;
                                        string strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                                        strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();
                                        lblSubQuery.ToolTip = strFilteredSubQueryConditions;
                                        hdnSubQueryID.Value = dtQueryConditions.Rows[i]["SubQueryID"].ToString();
                                        cmpCriteria.Enabled = false;
                                        cmpOperator.Enabled = false;
                                        // Disable ddlCriteria - invisible
                                        ddlCriteria.Visible = false;
                                        // Enable label
                                        lblSubQuery.Visible = true;
                                        // Disable ddl operator, pickbyname or txtxentervalue
                                        ddlOperator.Visible = false;
                                        ddlPickByName.Visible = false;
                                        txtEnterValue.Visible = false;
                                        // Change row color
                                        grdQueryConditions.Rows[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFF99");
                                    }
                                }
                                // Check to see if this is a criteria subsets - QM v2
                                if (dtQueryConditions.Rows[i]["SubsetID"] != DBNull.Value)
                                {
                                    if (Convert.ToInt64(dtQueryConditions.Rows[i]["SubsetID"]) > 0)
                                    {
                                        // Set indicator " -> or --> depending on level
                                        // Set backcolor depending on level
                                        // Loop through and set all controls as needed
                                        // Activate expand contract control for top row.
                                        // Handle curent subset, etc .....
                                        if (Convert.ToInt16(dtQueryConditions.Rows[i]["SubsetLogicalOrder"]) < 1)
                                        {
                                            // This is the title row of the subset, ignore for query but format row accordingly.
                                            lblSubQuery.Text = "Sub-Query: " + dtQueryConditions.Rows[i]["SubsetName"].ToString();
                                            if (Convert.ToInt16(dtQueryConditions.Rows[i]["SubsetLevel"]) % 2 != 0)
                                            {
                                                grdQueryConditions.Rows[i].BackColor = System.Drawing.ColorTranslator.FromHtml("#CCCCCC");
                                            }
                                            cmpCriteria.Enabled = false;
                                            cmpOperator.Enabled = false;
                                            // Disable ddlCriteria - invisible
                                            ddlCriteria.Visible = false;
                                            // Enable label
                                            lblSubQuery.Visible = true;
                                            // Disable ddl operator, pickbyname or txtxentervalue
                                            ddlOperator.Visible = false;
                                            ddlPickByName.Visible = false;
                                            txtEnterValue.Visible = false;
                                        }
                                        
                                    }

                                }
                                string levelIndicator = ">";
                                if (dtQueryConditions.Rows[i]["SubsetLevel"] != DBNull.Value)
                                {
                                    for (int j = 0; j < Convert.ToInt16(dtQueryConditions.Rows[i]["SubsetLevel"]); j++)
                                    {
                                        levelIndicator = "-" + levelIndicator;
                                    }
                                }
                                lblLevelIndicator.Text = levelIndicator;
                                // This is a condition within a subset, set row color, etc accordingly
                                //if 
                                // ***** to add - subset within subset and need to deliniate between
                                    // also - how to keep track of who is expanded, contracted

                            }
                        }
                        else
                        {
                            PageMessage = "false";
                        }
                    }
                }
                hdnCount.Value = dtQueryConditions.Rows.Count.ToString();
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }
        /// <summary>
        /// Save Query Detail
        /// </summary>
        private void SaveData()
        {
            int intLogicalOrder = 1;
            Campaign objCampaign = (Campaign)Session["Campaign"];
            QueryDetail objQueryDetail = new QueryDetail();
            Query objQuery = new Query();
            StringBuilder sbQuery = new StringBuilder();
            List<XmlNode> queryDetailList = new List<XmlNode>();

            try
            {

                objQuery.QueryName = txtQueryName.Text;
                objQuery.QueryCondition = string.Empty;

                foreach (GridViewRow row in grdQueryConditions.Rows)
                {

                    DropDownList ddlCriteria = (DropDownList)row.FindControl("ddlCriteria");
                    DropDownList ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                    DropDownList ddlLogical = (DropDownList)row.FindControl("ddlLogical");
                    TextBox txtEnterValue = (TextBox)row.FindControl("txtEnterValue");
                    DropDownList ddlPickByName = (DropDownList)row.FindControl("ddlPickByName");
                    HiddenField hdnQueryDetailID = (HiddenField)row.FindControl("hdnQueryDetailID");
                    Label lblSubQuery = (Label)row.FindControl("lblSubQuery");
                    HiddenField hdnSubQueryID = (HiddenField)row.FindControl("hdnSubQueryID");
                    if (string.IsNullOrEmpty(hdnSubQueryID.Value))
                    {
                        string operatorCondition = ddlOperator.SelectedValue;
                        if (txtEnterValue.Text.Trim().Length <= 10 && ddlCriteria.SelectedValue.ToLower().IndexOf(":date") >= 0
                            && ddlOperator.SelectedItem.Text == "Equals")
                        {
                            try
                            {
                                txtEnterValue.Text = Convert.ToDateTime(txtEnterValue.Text.Trim()).ToString("MM/dd/yyyy");
                            }
                            catch { }
                            operatorCondition = "Convert(Varchar(10),{0},101) = '{1}'";
                        }
                        string enteredValue;
                        if (txtEnterValue.Visible)
                            enteredValue = txtEnterValue.Text.Trim();
                        else
                            enteredValue = ddlPickByName.SelectedValue.Trim();

                        if (ddlCriteria.SelectedValue.ToLower().IndexOf(":date") >= 0)
                        {
                            if (enteredValue.Length <= 10)
                            {
                                if (ddlOperator.SelectedItem.Text == "Greater Than" ||
                                    ddlOperator.SelectedItem.Text == "Less than Equal")
                                {
                                    enteredValue = enteredValue + " 23:59:59";
                                }
                            }
                        }

                        objQueryDetail.SearchCriteria = ddlCriteria.SelectedValue.Substring(0, ddlCriteria.SelectedValue.IndexOf(":"));
                        objQueryDetail.SearchOperator = ddlOperator.SelectedValue;
                        objQueryDetail.SearchString = enteredValue;
                        objQueryDetail.SubQueryID = 0;
                        sbQuery.Append(" ");
                        sbQuery.AppendFormat(operatorCondition, ddlCriteria.SelectedValue.Substring(0, ddlCriteria.SelectedValue.IndexOf(":")), enteredValue.Replace("'", "''"));
                    }
                    else
                    {
                        string strSubQueryID = hdnSubQueryID.Value;
                        DataSet dsSubQueryDetails;
                        DataTable dtSubQueryConditions = new DataTable();
                        XmlDocument xDocCampaign1 = new XmlDocument();
                        xDocCampaign1.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                        CampaignService objCampaignService1 = new CampaignService();
                        dsSubQueryDetails = objCampaignService1.GetQueryDetailsByQueryID(xDocCampaign1, strSubQueryID);
                        dtSubQueryConditions = dsSubQueryDetails.Tables[0];
                        string strSubQueryConditions = dtSubQueryConditions.Rows[0]["QueryCondition"].ToString();
                        string strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                        strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();
                        objQueryDetail.SearchCriteria = "";
                        objQueryDetail.SearchOperator = "";
                        objQueryDetail.SearchString = "";
                        objQueryDetail.SubQueryID = Convert.ToInt64(strSubQueryID);
                        sbQuery.Append(" ");
                        sbQuery.AppendFormat("({0})", strFilteredSubQueryConditions);
                    }

                    objQueryDetail.LogicalOperator = ddlLogical.SelectedValue;
                    objQueryDetail.LogicalOrder = intLogicalOrder;

                    if (Request.QueryString["QueryID"] != null)
                    {
                        objQuery.QueryID = Convert.ToInt64(Request.QueryString["QueryID"]);
                        if (hdnQueryDetailID.Value != "")
                            objQueryDetail.QueryDetailID = Convert.ToInt64(hdnQueryDetailID.Value);
                    }

                    //sbQuery.Append(" ");
                    //sbQuery.AppendFormat(operatorCondition, ddlCriteria.SelectedValue.Substring(0, ddlCriteria.SelectedValue.IndexOf(":")), enteredValue.Replace("'", "''"));
                    sbQuery.AppendFormat(" {0} ", intLogicalOrder == grdQueryConditions.Rows.Count ? "" : ddlLogical.SelectedValue);
                    intLogicalOrder++;

                    XmlDocument xDocQueryDetail = new XmlDocument();
                    xDocQueryDetail.LoadXml(Serialize.SerializeObject(objQueryDetail, "QueryDetail"));
                    queryDetailList.Add(xDocQueryDetail);
                }
                objQuery.QueryCondition = BuildQueryCondition(sbQuery.ToString());

                XmlDocument xDocQuery = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocQuery.LoadXml(Serialize.SerializeObject(objQuery, "Query"));
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                CampaignService objCampaignService = new CampaignService();
                objQuery = (Query)Serialize.DeserializeObject(objCampaignService.QueryDetailInsertUpdate(
                    xDocCampaign, queryDetailList.ToArray(), xDocQuery), "Query");

                if (Request.QueryString["DataManager"] != null)
                {
                    Response.Redirect("~/campaign/DataPortal.aspx");
                }
                else
                {
                    Response.Redirect("~/campaign/QueryStatus.aspx");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("DuplicateQueryException") >= 0)
                    //PageMessage = "Query name already exists";
                    hdnDuplicateQuery.Value = "true";
                else
                    PageMessage = ex.Message;
            }
        }

        private void BindAdditionalFields(DropDownList ddl)
        {
            Campaign objCampaign = (Campaign)Session["Campaign"];
            DataSet dsFields;
            try
            {
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();

                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsFields = objCampService.GetCampaignFields(xDocCampaign);

                ddl.Items.Add(new ListItem("UniqueKey", "UniqueKey:Integer"));
                foreach (DataRow row in dsFields.Tables[0].Rows)
                {
                    if (!(bool)(row["IsDefault"]))
                    {
                        ddl.Items.Add(new ListItem(row["FieldName"].ToString(),
                            row["FieldName"].ToString() + ":" + row["FieldType"].ToString()));
                    }
                }
            }
            catch { }
        }

        protected void ddlOperator_Change(object sender, EventArgs e)
        {
            DropDownList ddlCriteria = null;
            DropDownList ddlOperator = null;
            DropDownList ddlPickByName = null;
            TextBox txtEnterValue = null;
            foreach (GridViewRow row in grdQueryConditions.Rows)
            {
                ddlCriteria = (DropDownList)row.FindControl("ddlCriteria");
                ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                ddlPickByName = (DropDownList)row.FindControl("ddlPickByName");
                txtEnterValue = (TextBox)row.FindControl("txtEnterValue");
                CheckForNameDropDowns(ddlCriteria, ddlOperator, ddlPickByName, txtEnterValue); 
            }
            if (hdnShowSQL.Value == "true")
            {
                BuildSQLLabel();
            }   
        }

        protected void ddlCriteria_Change(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddlCriteria = null;
                DropDownList ddlOperator = null;
                DropDownList ddlPickByName = null;
                TextBox txtEnterValue = null;
                DropDownList ddlSender = (DropDownList)sender;
                foreach (GridViewRow row in grdQueryConditions.Rows)
                {
                    ddlCriteria = (DropDownList)row.FindControl("ddlCriteria");
                    ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                    ddlPickByName = (DropDownList)row.FindControl("ddlPickByName");
                    txtEnterValue = (TextBox)row.FindControl("txtEnterValue");
                
                    CheckForNameDropDowns(ddlCriteria, ddlOperator, ddlPickByName, txtEnterValue);
                    // Bind operator needs to be redone ... needs to bind only that which changed ?????
                    if (ddlSender == ddlCriteria)
                    {
                        // This is the sender row, rebind the operator dropdown

                        // 2012-06-12 Dave Pollastrini
                        // Changed BindOperator to take a datatype.
                        /*
                        bool isDate = false;
                        string dataType = ddlCriteria.SelectedValue.Substring(ddlCriteria.SelectedValue.IndexOf(":") + 1, (ddlCriteria.SelectedValue.Length - (ddlCriteria.SelectedValue.IndexOf(":") + 1))).ToLower();

                        if (dataType == "integer" || dataType == "date")
                        {
                            isDate = true;
                        }
                        BindOperator(ddlOperator, isDate);
                        */

                        string dataType = ddlCriteria.SelectedValue.Substring(ddlCriteria.SelectedValue.IndexOf(":") + 1, (ddlCriteria.SelectedValue.Length - (ddlCriteria.SelectedValue.IndexOf(":") + 1))).ToLower();
                        BindOperator(ddlOperator, dataType);
                    }
                }
                if (hdnShowSQL.Value == "true")
                {
                    BuildSQLLabel();
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Exception in Criteria change " + ex.Message;
            }

        }


        private void CheckForNameDropDowns(DropDownList ddlCriteria, DropDownList ddlOperator, DropDownList ddlPickByName, TextBox txtEnterValue)
        {
            try
            {

                if (ddlOperator.SelectedValue == "{0} = '{1}'")
                {
                    switch (ddlCriteria.SelectedValue)
                    {
                        case "AgentID:String":
                            // we have agent selected and equals, build agent list.
                            if (ddlPickByName.SelectedIndex < 0)
                            {
                                ddlPickByName.Items.Clear();
                                BindAgentNames(ddlPickByName);
                            }
                            txtEnterValue.Visible = false;
                            ddlPickByName.Visible = true;
                            break;
                        case "CallResultCode:Integer":
                            // we have agent selected and equals, build agent list.
                            if (ddlPickByName.SelectedIndex < 0)
                            {
                                ddlPickByName.Items.Clear();
                                BindResultNames(ddlPickByName);
                            }
                            txtEnterValue.Visible = false;
                            ddlPickByName.Visible = true;
                            break;
                        default:
                            txtEnterValue.Visible = true;
                            ddlPickByName.Visible = false;
                            break;
                    }
                }
                else
                {
                    txtEnterValue.Visible = true;
                    ddlPickByName.Visible = false;
                }
            }
            catch { }

        }



        private void BindAgentNames(DropDownList ddl)
        {
            try
            {
                DataSet dsAgentList;
                AgentService objAgentService = new AgentService();
                dsAgentList = objAgentService.GetAgentList();
                foreach (DataRow row in dsAgentList.Tables[0].Rows)
                {
                        ddl.Items.Add(new ListItem(row["AgentName"].ToString(),
                            row["AgentID"].ToString()));
                }
            }
            catch { }
        }

        private void BindResultNames(DropDownList ddl)
        {
            DataSet dsResultCodes;
            try
            {
                CampaignService objCampService = new CampaignService();
                Campaign objCampaign = (Campaign)Session["Campaign"];
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsResultCodes = objCampService.GetResultCodes(xDocCampaign);
                foreach (DataRow row in dsResultCodes.Tables[0].Rows)
                {
                    ddl.Items.Add(new ListItem(row["Description"].ToString(),
                        row["ResultCodeID"].ToString()));
                }
            }
            catch { }
        }

        private void BuildSQLLabel()
        {
            try
            {
                if (Session["Campaign"] != null)
                {

                    Campaign objCampaign;
                    CampaignService objCampaignService = new CampaignService();

                    objCampaign = (Campaign)Session["Campaign"];

                    int intLogicalOrder = 1;
                    string strQueryCondition = "";
                    StringBuilder sbQueryCondition = new StringBuilder();
                    foreach (GridViewRow row in grdQueryConditions.Rows)
                    {
                        DropDownList ddlCriteria = (DropDownList)row.FindControl("ddlCriteria");
                        DropDownList ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                        DropDownList ddlLogical = (DropDownList)row.FindControl("ddlLogical");
                        TextBox txtEnterValue = (TextBox)row.FindControl("txtEnterValue");
                        DropDownList ddlPickByName = (DropDownList)row.FindControl("ddlPickByName");
                        HiddenField hdnQueryDetailID = (HiddenField)row.FindControl("hdnQueryDetailID");
                        Label lblSubQuery = (Label)row.FindControl("lblSubQuery");
                        HiddenField hdnSubQueryID = (HiddenField)row.FindControl("hdnSubQueryID");
                        if (string.IsNullOrEmpty(hdnSubQueryID.Value))
                        {
                            string operatorCondition = ddlOperator.SelectedValue;
                            if (txtEnterValue.Text.Trim().Length <= 10 && ddlCriteria.SelectedValue.ToLower().IndexOf(":date") >= 0
                                && ddlOperator.SelectedItem.Text == "Equals")
                            {
                                try
                                {
                                    txtEnterValue.Text = Convert.ToDateTime(txtEnterValue.Text.Trim()).ToString("MM/dd/yyyy");
                                }
                                catch { }
                                operatorCondition = "Convert(Varchar(10),{0},101) = '{1}'";
                            }

                            string enteredValue = "";

                            if (txtEnterValue.Visible)
                                enteredValue = txtEnterValue.Text.Trim();
                            else
                                enteredValue = ddlPickByName.SelectedValue.Trim();


                            if (ddlCriteria.SelectedValue.ToLower().IndexOf(":date") >= 0)
                            {
                                if (enteredValue.Length <= 10)
                                {
                                    if (ddlOperator.SelectedItem.Text == "Greater Than" ||
                                        ddlOperator.SelectedItem.Text == "Less than Equal")
                                    {
                                        enteredValue = enteredValue + " 23:59:59";
                                    }
                                }
                            }
                            string criteriaValue = ddlCriteria.SelectedValue.ToString().Substring(0, ddlCriteria.SelectedValue.ToString().IndexOf(":"));


                            sbQueryCondition.Append(" ");
                            sbQueryCondition.AppendFormat(operatorCondition, criteriaValue, enteredValue.Replace("'", "''"));
                            string strOp = ddlOperator.SelectedValue;

                            // 2012-06-12 Dave Pollastrini
                            // Changed BindOperator to take a datatype.
                            /*

                            bool isDateField = false;
                            try
                            {
                                isDateField = ddlCriteria.SelectedValue.IndexOf(":date") > 0;
                            }
                            catch { }

                            BindOperator(ddlOperator, isDateField);
                            */

                            string dataType = ddlCriteria.SelectedValue.Substring(ddlCriteria.SelectedValue.IndexOf(":") + 1, (ddlCriteria.SelectedValue.Length - (ddlCriteria.SelectedValue.IndexOf(":") + 1))).ToLower();
                            BindOperator(ddlOperator, dataType);

                            ddlOperator.SelectedValue = strOp;
                        }
                        else
                        {
                            string strSubQueryID = hdnSubQueryID.Value;
                            DataSet dsSubQueryDetails;
                            DataTable dtSubQueryConditions = new DataTable();
                            XmlDocument xDocCampaign1 = new XmlDocument();
                            xDocCampaign1.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                            CampaignService objCampaignService1 = new CampaignService();
                            dsSubQueryDetails = objCampaignService1.GetQueryDetailsByQueryID(xDocCampaign1, strSubQueryID);
                            dtSubQueryConditions = dsSubQueryDetails.Tables[0];
                            string strSubQueryConditions = dtSubQueryConditions.Rows[0]["QueryCondition"].ToString();
                            string strFilteredSubQueryConditions = strSubQueryConditions.Substring((strSubQueryConditions.IndexOf("WHERE (") + 7), (strSubQueryConditions.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strSubQueryConditions.IndexOf("WHERE (") + 7)));
                            strFilteredSubQueryConditions = strFilteredSubQueryConditions.Trim();
                            sbQueryCondition.Append(" ");
                            sbQueryCondition.AppendFormat("({0})", strFilteredSubQueryConditions);
                        }
                        sbQueryCondition.AppendFormat(" {0} ", intLogicalOrder == grdQueryConditions.Rows.Count ? "" : ddlLogical.SelectedValue);
                        intLogicalOrder++; // ???????????????????
                    }


                    strQueryCondition = BuildQueryCondition(sbQueryCondition.ToString());
                    string strFilteredQueryConditions = strQueryCondition.Substring((strQueryCondition.IndexOf("WHERE (") + 7), (strQueryCondition.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (strQueryCondition.IndexOf("WHERE (") + 7)));
                    strFilteredQueryConditions = strFilteredQueryConditions.Trim();
                    lblSQL.Text = strFilteredQueryConditions;
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error building raw sql to show : " + ex.Message;
            }
        }

        #endregion

        #region Validators

        protected void ValueEntryValidate(object source, ServerValidateEventArgs args)
        {
            DropDownList ddlCriteria = null;
            DropDownList ddlOperator = null;
            DropDownList ddlPickByName = null;
            TextBox txtEnterValue = null;
            Label lblSubQuery = null;
            try
            {
                foreach (GridViewRow row in grdQueryConditions.Rows)
                {
                    ddlCriteria = (DropDownList)row.FindControl("ddlCriteria");
                    ddlOperator = (DropDownList)row.FindControl("ddlOperator");
                    ddlPickByName = (DropDownList)row.FindControl("ddlPickByName");
                    txtEnterValue = (TextBox)row.FindControl("txtEnterValue");
                    lblSubQuery = (Label)row.FindControl("lblSubQuery");
                }

                if (ddlPickByName.Visible)
                {
                    if (Convert.ToInt16(ddlPickByName.SelectedValue) > 0)
                    {
                        args.IsValid = true;
                        return;
                    }
                }

                // If we have a sub query, consider this valid
                if (lblSubQuery.Visible)
                {
                    args.IsValid = true;
                    return;
                }

                // Check if can be blank ....
                if (ddlOperator.SelectedItem.Text == "Is Null" || ddlOperator.SelectedItem.Text == "Is Not Null")
                {
                    // can be blank, make it blank
                    txtEnterValue.Text = "";
                    txtEnterValue.Enabled = false;
                    args.IsValid = true;
                    return;
                }

                // Don't allow blank if both other drop downs selected
                if (txtEnterValue.Text.Length < 1 && (ddlOperator.SelectedValue != "0" || ddlCriteria.SelectedValue != "0"))
                {
                    PageMessage = "Please enter a valid value.";
                    args.IsValid = false;
                    return;
                }
                // check for numeric required
                if (ddlCriteria.SelectedValue.Substring(ddlCriteria.SelectedValue.IndexOf(":"), (ddlCriteria.SelectedValue.Length - ddlCriteria.SelectedValue.IndexOf(":"))).ToLower() == "integer")
                {
                    int numeric;
                    bool isNumeric = int.TryParse(txtEnterValue.Text, out numeric);
                    if (isNumeric)
                    {
                        args.IsValid = true;
                    }
                    else
                    {
                        PageMessage = "Please enter a numeric value.";
                        args.IsValid = false;
                    }
                    return;
                }
                args.IsValid = true;
                return;
            }
            catch (Exception ex)
            {
                PageMessage = "Error validating value entry " + ex.Message;
                args.IsValid = false;
            }
        }

        #endregion
    }
}
