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
using System.Text;

namespace Rainmaker.Web.campaign
{
    public partial class CampaignFieldsList : PageBase
    {

        public bool Isrunning = false;
        private Campaign currentCampaign;

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
                Campaign objCampaign;
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                    anchHome.InnerText = objCampaign.Description; // Replaced Short description

                    if (IsCampaignRunning())
                    {
                        Isrunning = true;
                    }

                    BindDataGrid("");
                }
            }
        }


        /// <summary>
        /// On row data binding
        /// </summary>
        
        #endregion

        protected void grdCampaignFields_DataBind(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                CheckBox chkReadOnly = (CheckBox)e.Item.Cells[5].Controls[1];
                DropDownList ddlTypeNames = (DropDownList)e.Item.Cells[3].Controls[1];
                dsIsDefault.ConnectionString = currentCampaign.CampaignDBConnString;
                string strSqlStmt = string.Format("SELECT FieldTypeID, ReadOnly, IsDefault FROM CampaignFields WHERE FieldID = {0}", ((TextBox)e.Item.Cells[e.Item.Cells.Count - 1].Controls[0]).Text);
                dsIsDefault.SelectCommand = strSqlStmt;
                DataView dv1 = (DataView)dsIsDefault.Select(DataSourceSelectArguments.Empty);
                DataRow dr = dv1.Table.Rows[0];
                string isReadOnly = dr["ReadOnly"].ToString();
                if (isReadOnly.ToLower() == "true" || isReadOnly == "0")
                    chkReadOnly.Checked = true;
                else
                    chkReadOnly.Checked = false;

                dsTypeNames.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
                strSqlStmt = string.Format("SELECT FieldType, FieldTypeID FROM FieldTypes");
                // *** Add sort order handling
                dsTypeNames.SelectCommand = strSqlStmt;
                dsTypeNames.DataBind();
                ddlTypeNames.DataSource = dsTypeNames;
                ddlTypeNames.DataBind();

                ddlTypeNames.SelectedValue = dr["FieldTypeID"].ToString();

                ((TextBox)e.Item.Cells[e.Item.Cells.Count - 1].Controls[0]).Enabled = false;
                ((TextBox)e.Item.Cells[e.Item.Cells.Count - 2].Controls[0]).Enabled = false;
                ((DropDownList)e.Item.Cells[3].Controls[1]).Enabled = false;

                // Only enable editing of read only for system fields, disable all others
                if (Convert.ToInt16(((TextBox)e.Item.Cells[e.Item.Cells.Count - 1].Controls[0]).Text) < 9)
                {
                    ((TextBox)e.Item.Cells[2].Controls[0]).Enabled = false;
                    ((TextBox)e.Item.Cells[4].Controls[0]).Enabled = false;
                }

                // Disable editing of lengths for non-length field types
                if (((DropDownList)e.Item.Cells[3].Controls[1]).SelectedItem.Text != "String")
                {
                    ((TextBox)e.Item.Cells[4].Controls[0]).Enabled = false;
                }

                return;
            }
            if (e.Item.Cells[6].Text.ToLower() == "true")
            {
                e.Item.CssClass = "tableRowSys";
                //((LinkButton)e.Item.Cells[0].Controls[0]).Enabled = false;
                ((LinkButton)e.Item.Cells[1].Controls[0]).Enabled = false;
            }

        }

        protected void grdCampaignFields_EditCommand(object source, DataGridCommandEventArgs e)
        {
            Campaign objCampaign;
            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];
                anchHome.InnerText = objCampaign.Description;// Replaced Short description

                if (IsCampaignRunning())
                {
                    Isrunning = true;
                }
            }
            if (Isrunning)
            {
                PageMessage = "Fields may not be changed while the campaign is running.  Please stop the campaign and try again.";
                return;
            }

            string sortQueryAppend = "";
            if (Session["SortExp"] != null)
            {
                if (Session["SortExp"].ToString().Length > 0)
                {
                    
                    string sortDirection = (string)Session["SortDir"];

                    sortQueryAppend = string.Format(" ORDER BY {0} {1}", Session["SortExp"].ToString(), sortDirection);
                }
            }
            grdCampaignFields.EditItemIndex = e.Item.ItemIndex;
            grdCampaignFields.Columns[1].Visible = false;
            BindDataGrid(sortQueryAppend);
        }

        protected void grdCampaignFields_CancelEdit(object source, DataGridCommandEventArgs e)
        {
            string sortQueryAppend = "";
            if (Session["SortExp"] != null)
            {
                if (Session["SortExp"].ToString().Length > 0)
                {
                    
                    string sortDirection = (string)Session["SortDir"];

                    sortQueryAppend = string.Format(" ORDER BY {0} {1}", Session["SortExp"].ToString(), sortDirection);
                }
            }
            grdCampaignFields.EditItemIndex = -1;
            grdCampaignFields.Columns[1].Visible = true;
            BindDataGrid(sortQueryAppend);
        }

        protected void grdCampaignFields_UpdateRecord(object source, DataGridCommandEventArgs e)
        {
            try
            {
                // Update the database here
                if (Session["Campaign"] != null)
                {
                    // we have an existing campaign
                    currentCampaign = (Campaign)Session["Campaign"];
                }


                string sqlStmt = "";
                string strOriginalFieldName = "";
                string strOriginalLength = "";
                dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                dsMainGrid.SelectCommand = string.Format("SELECT FieldName, FieldTypeID, Value FROM CampaignFields WHERE FieldID = {0}", ((TextBox)e.Item.Cells[e.Item.Cells.Count - 1].Controls[0]).Text);
                
                DataView dv1 = (DataView)dsMainGrid.Select(DataSourceSelectArguments.Empty);
                DataRow dr = dv1.Table.Rows[0];
                strOriginalFieldName = dr["FieldName"].ToString();
                strOriginalLength = dr["Value"].ToString(); 

                StringBuilder sbSqlStmt = new StringBuilder();
                sbSqlStmt.AppendFormat("UPDATE CampaignFields SET ");

                // *** CHeck how nulls will work ... alert?  EMpty string?
                // Formatting as sql string types for now ... handle data types?
                if (((TextBox)e.Item.Cells[2].Controls[0]).Text.Length > 0)
                    sbSqlStmt.AppendFormat("FieldName = '{0}', ", ((TextBox)e.Item.Cells[2].Controls[0]).Text);
                else
                {
                    PageMessage="Field Name cannot be blank.  Please enter a name.";
                    return;
                }
                    sbSqlStmt.AppendFormat("FieldTypeID = {0}, ", ((DropDownList)e.Item.Cells[3].Controls[1]).SelectedValue);
                    
                
                if (((CheckBox)e.Item.Cells[5].Controls[1]).Checked)
                {
                    sbSqlStmt.AppendFormat("ReadOnly = 1");
                }
                else
                {
                    sbSqlStmt.AppendFormat("ReadOnly = 0");
                }

                // Update the field name if it has changed
                if (strOriginalFieldName != ((TextBox)e.Item.Cells[2].Controls[0]).Text)
                {
                    StringBuilder sbSqlStmt2 = new StringBuilder();
                    sbSqlStmt2.AppendFormat("EXEC sp_rename 'Campaign.{0}', '{1}', 'COLUMN'", strOriginalFieldName, ((TextBox)e.Item.Cells[2].Controls[0]).Text);

                    sqlStmt = sbSqlStmt2.ToString();

                    ActivityLogger.WriteAdminEntry(currentCampaign, "Change columns Statement to Execute: '{0}'", sqlStmt);

                    dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                    dsMainGrid.SelectCommand = sqlStmt;
                    dsMainGrid.Select(DataSourceSelectArguments.Empty);
                }

                // Update the field name if it has changed
                if (strOriginalLength != ((TextBox)e.Item.Cells[4].Controls[0]).Text)
                {
                    int numeric;
                    bool isNumeric = int.TryParse(((TextBox)e.Item.Cells[4].Controls[0]).Text, out numeric);
                    if (isNumeric)
                    {
                        StringBuilder sbSqlStmt2 = new StringBuilder();
                        sbSqlStmt2.AppendFormat("ALTER TABLE Campaign ALTER COLUMN [{0}] varchar({1})", ((TextBox)e.Item.Cells[2].Controls[0]).Text, ((TextBox)e.Item.Cells[4].Controls[0]).Text);

                        sqlStmt = sbSqlStmt2.ToString();
                        ActivityLogger.WriteAdminEntry(currentCampaign, "Change columns Statement to Execute: '{0}'", sqlStmt);
                        dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                        dsMainGrid.SelectCommand = sqlStmt;
                        dsMainGrid.Select(DataSourceSelectArguments.Empty);
                        if (((TextBox)e.Item.Cells[4].Controls[0]).Text.Length > 0)
                            sbSqlStmt.AppendFormat(", Value = '{0}' ", ((TextBox)e.Item.Cells[4].Controls[0]).Text);
                        else
                            sbSqlStmt.AppendFormat(", Value = NULL ");
                    }
                }
                sbSqlStmt.AppendFormat(" WHERE FieldID = {0}", ((TextBox)e.Item.Cells[e.Item.Cells.Count - 1].Controls[0]).Text);
                string sqlStmtFinal = sbSqlStmt.ToString();

                ActivityLogger.WriteAdminEntry(currentCampaign, "Update Statement to Execute: '{0}'", sqlStmtFinal);

                dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                dsMainGrid.SelectCommand = sqlStmtFinal;
                dsMainGrid.Select(DataSourceSelectArguments.Empty);

                string sortQueryAppend = "";
                if (Session["SortExp"] != null)
                {
                    if (Session["SortExp"].ToString().Length > 0)
                    {
                        string sortDirection = (string)Session["SortDir"];

                        sortQueryAppend = string.Format(" ORDER BY {0} {1}", Session["SortExp"].ToString(), sortDirection);
                    }
                }
                grdCampaignFields.EditItemIndex = -1;
                grdCampaignFields.Columns[1].Visible = true;
                BindDataGrid(sortQueryAppend);
            }
            catch (Exception ex)
            {
                PageMessage = "Exception updating record: " + ex.Message;
            }
        }

        protected void grdCampaignFields_DeleteRecord(object source, DataGridCommandEventArgs e)
        {
            Campaign objCampaign;
            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];
                anchHome.InnerText = objCampaign.Description;// Replaced Short description

                if (IsCampaignRunning())
                {
                    Isrunning = true;
                }
            }
            if (Isrunning)
            {
                PageMessage="Fields may not be changed while the campaign is running.  Please stop the campaign and try again.";
                return;
            }

            // Update the database here
            try
            {
                if (Session["Campaign"] != null)
                {
                    // we have an existing campaign
                    currentCampaign = (Campaign)Session["Campaign"];
                }

                string strOriginalFieldName = "";
                dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                dsMainGrid.SelectCommand = string.Format("SELECT FieldName FROM CampaignFields WHERE FieldID = {0}", e.Item.Cells[7].Text);

                DataView dv1 = (DataView)dsMainGrid.Select(DataSourceSelectArguments.Empty);
                DataRow dr = dv1.Table.Rows[0];
                strOriginalFieldName = dr["FieldName"].ToString();

                // Udelete the field in the campaign table
                StringBuilder sbSqlStmt2 = new StringBuilder();
                sbSqlStmt2.AppendFormat("ALTER TABLE Campaign DROP COLUMN {0}", strOriginalFieldName);

                string sqlStmt = sbSqlStmt2.ToString();
                ActivityLogger.WriteAdminEntry(currentCampaign, "Change columns Statement to Execute: '{0}'", sqlStmt);
                dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                dsMainGrid.SelectCommand = sqlStmt;
                dsMainGrid.Select(DataSourceSelectArguments.Empty);

                StringBuilder sbSqlStmt = new StringBuilder();
                sbSqlStmt.AppendFormat("DELETE FROM CampaignFields ");

                sbSqlStmt.AppendFormat(" WHERE FieldID = {0}", e.Item.Cells[7].Text);

                sqlStmt = sbSqlStmt.ToString();
                ActivityLogger.WriteAdminEntry(currentCampaign, "Delete statement to execute: '{0}'", sqlStmt);
                dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                dsMainGrid.SelectCommand = sqlStmt;
                dsMainGrid.Select(DataSourceSelectArguments.Empty);
                string sortDirection = (string)Session["SortDir"];
                string sortQueryAppend = "";
                if (Session["SortExp"] != null)
                {
                    if (Session["SortExp"].ToString().Length > 0)
                    {
                        sortQueryAppend = string.Format(" ORDER BY {0} {1}", Session["SortExp"].ToString(), sortDirection);
                    }
                }
                grdCampaignFields.EditItemIndex = -1;
                BindDataGrid(sortQueryAppend);
            }
            catch (Exception ex)
            {
                PageMessage = "Exception deleting record: " + ex.Message;
            }
        }

        protected void grdCampaignFields_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
        {
            // Set the grid control page index to 0 - page 1
            Session["GridPageIndex"] = e.NewPageIndex.ToString();
            string sortQueryAppend = "";
            if (Session["SortExp"] != null)
            {
                if (Session["SortExp"].ToString().Length > 0)
                {
                    string sortDirection = (string)Session["SortDir"];
                    sortQueryAppend = string.Format(" ORDER BY {0} {1}", Session["SortExp"].ToString(), sortDirection);
                }
            }
            BindDataGrid(sortQueryAppend);
        }

        protected void grdCampaignFields_SortCommand(object source, DataGridSortCommandEventArgs e)
        {
            try
            {
                // Add sort to query select statement
                string sortQueryAppend = "";
                string sortDirection = (string)Session["SortDir"];

                // Switch sort direct each time this event fires
                if (sortDirection == "asc")
                    sortDirection = "desc";
                else
                    sortDirection = "asc";

                Session["SortDir"] = sortDirection;
                Session["SortExp"] = e.SortExpression;

                sortQueryAppend = string.Format(" ORDER BY {0} {1}", e.SortExpression, sortDirection);
                BindDataGrid(sortQueryAppend);
            }
            catch (Exception ex)
            {
                PageMessage = "Exception changing sort: " + ex.Message;
            }
        }

        protected void BindDataGrid(string sortExpression)
        {
            try
            {
                if (Session["Campaign"] != null)
                {
                    // we have an existing campaign
                    currentCampaign = (Campaign)Session["Campaign"];
                }
                else
                {
                    return;
                }
                // Bind data grid to the query we have built and display it.

                dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                string strSqlStmt = string.Format
                (
					@"
					select
						cf.FieldName, 
						cf.FieldTypeID, 
						ft.FieldType		'FieldTypeName',
						cf.Value, 
						cf.ReadOnly, 
						cf.IsDefault, 
						cf.FieldID 
					from 
						dbo.CampaignFields cf (nolock)
						inner join
						RainMakerMaster.dbo.FieldTypes ft (nolock)
							on cf.FieldTypeID = ft.FieldTypeID
					{0}", 
					sortExpression
				);
				
                // *** Add sort order handling
                dsMainGrid.SelectCommand = strSqlStmt;
                dsMainGrid.DataBind();
                //DataView dv1 = (DataView)dsMainGrid.Select(DataSourceSelectArguments.Empty);
                grdCampaignFields.DataSource = dsMainGrid;
                //grdCampaignFields.PageSize = Convert.ToInt16(ddlRecPerPage.SelectedValue);
                try
                {
                    grdCampaignFields.CurrentPageIndex = Convert.ToInt16(Session["GridPageIndex"]);
                }
                catch
                {
                    grdCampaignFields.CurrentPageIndex = 1;
                }
                // First column (after 0 - edit, 1 delete) is Unique key, do not show.
                grdCampaignFields.DataBind();
            }
            catch (Exception ex)
            {
                PageMessage = "Error binding data grid : " + ex.Message;
            }

        }

        
    }
}
