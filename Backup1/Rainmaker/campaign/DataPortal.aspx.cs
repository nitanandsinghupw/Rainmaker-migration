using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using Rainmaker.Common.DomainModel;
using Rainmaker.Common.Extensions;
using Rainmaker.Web.CampaignWS;



namespace Rainmaker.Web.campaign
{
    public partial class DataPortal : PageBase
    {
        private bool includeCustomFields;

        private Agent currentAgent;
        private Campaign currentCampaign;

        private long currentQueryID = 0;
        private string currentQuerySelectStmt = "";
        private string currentQueryConditions = "";
        private string currentFieldList = "";
        private int currentRecordCount = 0;
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
                    if (Session["Campaign"] != null)
                    {
                        Campaign objCampaign = (Campaign)Session["Campaign"];
                        anchHome.InnerText = objCampaign.Description;
                    }
                    try
                    {
                        Session["DataManager"] = "yes";
                        InitializeControls();
                        BuildFieldList();
                        BuildDataGridQuery();
                        BindDataGrid();
                    }
                    catch (Exception ex)
                    {
                        PageMessage = "Exception loading page: " + ex.Message;
                    }
                }
                else
                {
                    if (Session["FieldsChanged"] != null)
                    {
                        if (Session["FieldsChanged"].ToString() == "true")
                        {
                            Session["FieldsChanged"] = "false";
                            if (!ddlViews.Items.Contains(ListItem.FromString("Custom")))
                                ddlViews.Items.Add("Custom");
                            ddlViews.SelectedValue = "Custom";
                            BuildFieldList();
                            BuildDataGridQuery();
                            BindDataGrid();
                            return;
                        }
                    }
                    if (Session["ViewChanged"] != null)
                    {
                        if (Session["ViewChanged"].ToString() == "yes")
                        {
                            Session["ViewChanged"] = "no";

                            if (Session["DataView"] != null)
                            {
                                ddlViews.Items.Add(Session["DataView"].ToString());
                                if (!ddlViews.Items.Contains(ListItem.FromString("Custom")))
                                    ddlViews.Items.Remove("Custom");
                                ddlViews.SelectedValue = Session["DataView"].ToString();
                            }
                            BuildFieldList();
                            BuildDataGridQuery();
                            BindDataGrid();
                            return;
                        }
                    }
                }
            }

            protected void chkResultNames_Change(object sender, EventArgs e)
            {
                BuildFieldList();
                BuildDataGridQuery();
                // Set the grid control page index to 0 - page 1
                BindDataGrid();
            }

            protected void ddlCampaigns_Change(object sender, EventArgs e)
            {
                try
                {
                    long CampaignID = 0;
                    CampaignID = Convert.ToInt64(ddlCampaign.SelectedValue);
                    // Load up this campaign object
                    if (CampaignID > 0)
                    {
                        CampaignService objCampService = new CampaignService();
                        currentCampaign = (Campaign)Serialize.DeserializeObject(objCampService.GetCampaignByCampaignID(CampaignID), "Campaign");
                        // Reset all web session variables

                        Session["Campaign"] = currentCampaign;
                        Session["GridPageIndex"] = "0";
                        Session["SortExp"] = null;
                        Session["SortDir"] = null;
                        Session["FieldsChanged"] = "false";
                        // Reset the views dropdown
                        ddlViews.SelectedValue = "Show All";

                        // Reset the query dropdown
                        ResetQueryControl();
                        // Apply view parameters?
                        BuildFieldList();
                        BuildDataGridQuery();
                        // Set the grid control page index to 0 - page 1
                        
                        BindDataGrid();

                        // Enable all controls
                        ddlViews.Enabled = true;
                        ddlQuery.Enabled = true;
                        ddlRecPerPage.Enabled = true;
                        btnDeleteColumn.Enabled = true;
                        btnExport.Enabled = true;
                        btnAddColumn.Enabled = true;
                        btnSaveView.Enabled = true;
                        btnNewQuery.Enabled = true;
                        lblNoData.Text = "";
                        lblNoData.Visible = false;
                    }
                    else
                    {
                        // Problem with selected campaign, force the control back to original
                        ddlCampaign.SelectedValue = currentCampaign.CampaignID.ToString();
                    }
                }
                catch (Exception ex)
                {
                    PageMessage = "Exception changing campaign when control changed: " + ex.Message;
                }
            }

        

            protected void ddlQuery_Change(object sender, EventArgs e)
            {
                try
                {
                    // Apply view parameters and refresh data grid
                    BuildFieldList();
                    BuildDataGridQuery();
                    // Set the grid control page index to 0 - page 1
                    Session["GridPageIndex"] = "0";
                    Session["SortExp"] = null;
                    Session["SortDir"] = null;
                    BindDataGrid();
                }
                catch (Exception ex)
                {
                    PageMessage = "Exception changing query when control changed: " + ex.Message;
                }     
            }

            protected void ddlRecPerPage_Change(object sender, EventArgs e)
            {
                try
                {
                    // Apply view parameters and refresh data grid
                    BuildFieldList();
                    BuildDataGridQuery();
                    if (Session["SortExp"] != null)
                    {
                        if (Session["SortExp"].ToString().Length > 0)
                        {
                            string sortQueryAppend = "";
                            string sortDirection = (string)Session["SortDir"];

                            sortQueryAppend = string.Format(" ORDER BY {0} {1}", Session["SortExp"].ToString(), sortDirection);
                            currentQuerySelectStmt = currentQuerySelectStmt + sortQueryAppend;
                        }
                    }
                    BindDataGrid();
                    Session["RecsPerPage"] = ddlRecPerPage.SelectedValue;
                }
                catch (Exception ex)
                {
                    PageMessage = "Exception changing recs per page when control changed: " + ex.Message;
                }
            }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        protected void grdDataPortal_SortCommand(object source, DataGridSortCommandEventArgs e)
            {
                try
                {
                    // Apply view parameters and refresh data grid
                    BuildFieldList();
                    BuildDataGridQuery();
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
                    currentQuerySelectStmt = currentQuerySelectStmt + sortQueryAppend;

                    BindDataGrid();
                }
                catch (Exception ex)
                {
                    PageMessage = "Exception changing sort: " + ex.Message;
                }
            }


            //-------------------------------------------------------------
            //
            //
            //-------------------------------------------------------------

            protected void grdDataPortal_PageIndexChanged(object source, DataGridPageChangedEventArgs e)
            {
                // Set the grid control page index to 0 - page 1
                Session["GridPageIndex"] = e.NewPageIndex.ToString();
                BuildFieldList();
                BuildDataGridQuery();
                if (Session["SortExp"] != null)
                {
                    if (Session["SortExp"].ToString().Length > 0)
                    {
                        string sortQueryAppend = "";
                        string sortDirection = (string)Session["SortDir"];

                        sortQueryAppend = string.Format(" ORDER BY {0} {1}", Session["SortExp"].ToString(), sortDirection);
                        currentQuerySelectStmt = currentQuerySelectStmt + sortQueryAppend;
                    }
                }
                BindDataGrid();
            }

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        protected void grdDataPortal_EditCommand(object source, DataGridCommandEventArgs e)
            {
                ActivityLogger.WriteAdminEntry(currentCampaign, "grdDataPortal_EditCommand '{0}'", "triggered");

                //-----------------------------------------------------
                // Don: forcing the name to away, back to numbers
                // for the result codes.
                //-----------------------------------------------------
                chkResultNames.Checked = false;

                BuildFieldList();
                BuildDataGridQuery();
                if (Session["SortExp"] != null)
                {
                    if (Session["SortExp"].ToString().Length > 0)
                    {
                        string sortQueryAppend = "";
                        string sortDirection = (string)Session["SortDir"];

                        sortQueryAppend = string.Format(" ORDER BY {0} {1}", Session["SortExp"].ToString(), sortDirection);
                        currentQuerySelectStmt = currentQuerySelectStmt + sortQueryAppend;
                    }
                }
                grdDataPortal.EditItemIndex = e.Item.ItemIndex;
                grdDataPortal.Columns[1].Visible = false;
                BindDataGrid();

                
            }

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        protected void grdDataPortal_CancelEdit(object source, DataGridCommandEventArgs e)
            {
                BuildFieldList();
                BuildDataGridQuery();
                if (Session["SortExp"] != null)
                {
                    if (Session["SortExp"].ToString().Length > 0)
                    {
                        string sortQueryAppend = "";
                        string sortDirection = (string)Session["SortDir"];

                        sortQueryAppend = string.Format(" ORDER BY {0} {1}", Session["SortExp"].ToString(), sortDirection);
                        currentQuerySelectStmt = currentQuerySelectStmt + sortQueryAppend;
                    }
                }
                grdDataPortal.EditItemIndex = -1;
                grdDataPortal.Columns[1].Visible = true;
                BindDataGrid();
            }

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        protected void grdDataPortal_UpdateRecord(object source, DataGridCommandEventArgs e)
            {
                ActivityLogger.WriteAdminEntry(currentCampaign, "grdDataPortal_UpdateRecord '{0}'", "triggered");
                try
                {
                    bool bContinue = true;


                    // *** Update the database here
                    if (Session["Campaign"] != null)
                    {
                        // we have an existing campaign
                        currentCampaign = (Campaign)Session["Campaign"];
                    }
                    if (Session["GridFields"] != null)
                    {
                        StringBuilder sbSqlStmt = new StringBuilder();
                        string[] strFieldNames = Session["GridFields"].ToString().Split(',');
                        sbSqlStmt.AppendFormat("UPDATE Campaign SET ");

                        // Formatting as sql string types for now ... handle data types?
                        sbSqlStmt.AppendFormat("{0} = '{1}'", strFieldNames[0].Trim(), ((TextBox)e.Item.Cells[2].Controls[0]).Text.prepSQL());
                        //DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                        if (Session["EncryptedFields"] != null)
                        {
                            string encryptedfields = Session["EncryptedFields"].ToString();

                            string[] fieldListArray = encryptedfields.Split(',');

                            string passPhrase = "whatevesfasdfasdfr23";        // can be any string
                            string initVector = "Qt@&^SDF15F6g7H8"; // must be 16 bytes

                            // Before encrypting data, we will append plain text to a random
                            // salt value, which will be between 4 and 8 bytes long (implicitly
                            // used defaults).
                            RijndaelEnhanced rijndaelKey =
                                new RijndaelEnhanced(passPhrase, initVector);
                            foreach (string strField in fieldListArray)
                            {
                                int i = 0;
                                foreach (string columnname in strFieldNames)
                                {
                                    if (strField.Trim() == strFieldNames[i].Trim())
                                    {
                                        break; //its a match
                                    }

                                    i++;
                                }
                                int index = i;
                                string currentvalue = ((TextBox)e.Item.Cells[index+2].Controls[0]).Text;
                                ActivityLogger.WriteAdminEntry(currentCampaign, "UpdateRecord - Column and current text: '{0}'", strField + " " + currentvalue);
                                if (currentvalue != "&nbsp;" && currentvalue != "")
                                {
                                    ((TextBox)e.Item.Cells[index+2].Controls[0]).Text = rijndaelKey.Encrypt(currentvalue);

                                }
                            }
                        }
                        for (int i = 3; i < (e.Item.Cells.Count - 1); i++)
                        {
                            if (((TextBox)e.Item.Cells[i].Controls[0]).Text.Length > 0)
                                sbSqlStmt.AppendFormat(", {0} = '{1}'", strFieldNames[i - 2].Trim(), ((TextBox)e.Item.Cells[i].Controls[0]).Text.prepSQL());
                            else
                                sbSqlStmt.AppendFormat(", {0} = NULL", strFieldNames[i - 2].Trim());
                        }

                        sbSqlStmt.AppendFormat(" WHERE UniqueKey = {0}", ((TextBox)e.Item.Cells[e.Item.Cells.Count - 1].Controls[0]).Text.prepSQL());

                        string sqlStmt = sbSqlStmt.ToString();

                        /*
                         * D. Pollastrini
                         * 2012-04-18
                         * 
                         * Update validation code to determine whether CallResultCode is NULL or digits.  If so, OK to continue.
                         */

                        bContinue = new Regex(@"CallResultCode\s*=\s*(NULL|'\d+'),").IsMatch(sqlStmt);

                        ////---------------------------------------------
                        //// Checking to make sure a proper Call Result 
                        //// Code is entered.
                        ////---------------------------------------------
                        //int iIndex = -1;
                        //string strCallResultCode = "";
                        //int iCallResultCode = 0;
                        //iIndex = sqlStmt.IndexOf("CallResultCode = '");
                        //strCallResultCode = sqlStmt.Substring(iIndex + 18, sqlStmt.Length - iIndex - 18);
                        //iIndex = strCallResultCode.IndexOf("'");
                        //if (iIndex != -1)
                        //{
                        //    strCallResultCode = strCallResultCode.Substring(0, iIndex);
                        //    try
                        //    {
                        //        // Checking to see proper number was entered.
                        //        iCallResultCode = Convert.ToInt16(strCallResultCode);
                        //    }
                        //    catch
                        //    {
                        //        // Invalid data.
                        //        bContinue = false;
                        //    }
                        //}


                        if (bContinue == true)
                        {
                           ActivityLogger.WriteAdminEntry(currentCampaign, "Update Statement to Execute: '{0}'", sqlStmt);
                           dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                           dsMainGrid.SelectCommand = sqlStmt;
                           dsMainGrid.Select(DataSourceSelectArguments.Empty);
                           
                        }
                        else
                        {
                            PageMessage = "Invalid Call Result Code - numeric value required.";
                            bContinue = false;
                        }

                    }

                    if (bContinue == true)
                    {
                        BuildFieldList();
                        BuildDataGridQuery();
                        if (Session["SortExp"] != null)
                        {
                            if (Session["SortExp"].ToString().Length > 0)
                            {
                                string sortQueryAppend = "";
                                string sortDirection = (string)Session["SortDir"];

                                sortQueryAppend = string.Format(" ORDER BY {0} {1}", Session["SortExp"].ToString(), sortDirection);
                                currentQuerySelectStmt = currentQuerySelectStmt + sortQueryAppend;
                            }
                        }
                        grdDataPortal.EditItemIndex = -1;
                        grdDataPortal.Columns[1].Visible = true;
                        BindDataGrid();

                    }
                }
                catch (Exception ex)
                {
                    PageMessage = "Exception updating record: " + ex.Message;

                }
            }

        protected void grdDataPortal_DeleteRecord(object source, DataGridCommandEventArgs e)
        {
            // Update the database here
            try
            {
                if (Session["Campaign"] != null)
                {
                    // we have an existing campaign
                    currentCampaign = (Campaign)Session["Campaign"];
                }
                string[] strFieldNames = Session["GridFields"].ToString().Split(',');
                int ColumnCount = 0;
                foreach (string str in strFieldNames) ColumnCount++;
                StringBuilder sbSqlStmt = new StringBuilder();
                StringBuilder sbSqlStmt2 = new StringBuilder();
                StringBuilder sbSqlStmt3 = new StringBuilder();
                sbSqlStmt.AppendFormat("DELETE FROM CallList ");
                sbSqlStmt2.AppendFormat("DELETE FROM SilentCallList ");
                sbSqlStmt3.AppendFormat("DELETE FROM Campaign ");
                sbSqlStmt.AppendFormat(" WHERE UniqueKey = {0}", e.Item.Cells[ColumnCount + 2].Text);
                sbSqlStmt2.AppendFormat(" WHERE UniqueKey = {0}", e.Item.Cells[ColumnCount + 2].Text);
                sbSqlStmt3.AppendFormat(" WHERE UniqueKey = {0}", e.Item.Cells[ColumnCount + 2].Text);

                string sqlStmt = sbSqlStmt.ToString();
                ActivityLogger.WriteAdminEntry(currentCampaign, "Delete Statement to Execute: '{0}'", sqlStmt);
                dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                dsMainGrid.SelectCommand = sqlStmt;
                dsMainGrid.Select(DataSourceSelectArguments.Empty);

                string sqlStmt2 = sbSqlStmt.ToString();
                ActivityLogger.WriteAdminEntry(currentCampaign, "Delete Statement to Execute: '{0}'", sqlStmt2);
                dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                dsMainGrid.SelectCommand = sqlStmt2;
                dsMainGrid.Select(DataSourceSelectArguments.Empty);

                string sqlStmt3 = sbSqlStmt3.ToString();
                ActivityLogger.WriteAdminEntry(currentCampaign, "Delete Statement to Execute: '{0}'", sqlStmt3);
                dsMainGrid.SelectCommand = sqlStmt3;
                dsMainGrid.Select(DataSourceSelectArguments.Empty);

                BuildFieldList();
                BuildDataGridQuery();
                if (Session["SortExp"] != null)
                {
                    if (Session["SortExp"].ToString().Length > 0)
                    {
                        string sortQueryAppend = "";
                        string sortDirection = (string)Session["SortDir"];

                        sortQueryAppend = string.Format(" ORDER BY {0} {1}", Session["SortExp"].ToString(), sortDirection);
                        currentQuerySelectStmt = currentQuerySelectStmt + sortQueryAppend;
                    }
                }
                grdDataPortal.EditItemIndex = -1;
                BindDataGrid();
            }
            catch (Exception ex)
            {
                PageMessage = "Exception deleting record: " + ex.Message;
            }
        }

        protected void grdDataPortal_DataBind(object source, DataGridItemEventArgs e)
        {

            // This code will disable the unique key field from editing
            
            if (e.Item.ItemType == ListItemType.EditItem)
            {
                
                ((TextBox)e.Item.Cells[e.Item.Cells.Count - 1].Controls[0]).Enabled = false;
                DataRow dr1 = ((DataRowView)e.Item.DataItem).Row;
                int index1 = dr1.Table.Columns.IndexOf("PHONENUM");
                try
                {
                    if (((TextBox)e.Item.Cells[index1 + 2].Controls[0]).Enabled == true)
                    {
                        ((TextBox)e.Item.Cells[index1 + 2].Controls[0]).Enabled = false;
                    }
                }
                catch { }
                try
                {
                    if (Session["GridFields"] != null)
                    {
  
                        if (Session["EncryptedFields"] != null)
                            {

                            string encryptedfields = Session["EncryptedFields"].ToString();

                            string[] fieldListArray = encryptedfields.Split(',');
                            
                            string passPhrase = "whatevesfasdfasdfr23";        // can be any string
                            string initVector = "Qt@&^SDF15F6g7H8"; // must be 16 bytes

                            // Before encrypting data, we will append plain text to a random
                            // salt value, which will be between 4 and 8 bytes long (implicitly
                            // used defaults).
                            RijndaelEnhanced rijndaelKey =
                                new RijndaelEnhanced(passPhrase, initVector);
                            foreach (string strField in fieldListArray)
                            {

                                int index = dr1.Table.Columns[strField.Trim()].Ordinal;
                                string currentvalue = ((TextBox)e.Item.Cells[index + 2].Controls[0]).Text;
                                if (currentvalue != "&nbsp;" && currentvalue != "")
                                {
                                    ((TextBox)e.Item.Cells[index + 2].Controls[0]).Text = rijndaelKey.Decrypt(currentvalue);

                                }
                            }
                        }
                    }
                }
                catch
                {

                }
                if (Session["ReadOnlyFields"] != null)
                {

                    string readonlyfields = Session["ReadOnlyFields"].ToString();
                    string[] readonlyfieldListArray = readonlyfields.Split(',');
                    foreach (string strField in readonlyfieldListArray)
                    {

                        int index = dr1.Table.Columns.IndexOf(strField.Trim());
                        try
                        {
                            if (((TextBox)e.Item.Cells[index + 2].Controls[0]).Enabled == true)
                            {
                                ((TextBox)e.Item.Cells[index + 2].Controls[0]).Enabled = false;
                            }
                        }
                        catch { }

                    }
                }
            }


            // 2012-06-08 Dave Pollastrini
            // Old School datagrid requires some finesse to format individual cells,
            // especially with dynamic columns.  Added code to format decimal seconds
            // to hh:mm:ss.
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                
                if (Session["EncryptedFields"] != null) {
                
                    string encryptedfields = Session["EncryptedFields"].ToString();
                    
                    string[] fieldListArray = encryptedfields.Split(',');
                    
                    string passPhrase = "whatevesfasdfasdfr23";        // can be any string
                    string initVector = "Qt@&^SDF15F6g7H8"; // must be 16 bytes

                    RijndaelEnhanced rijndaelKey =
                        new RijndaelEnhanced(passPhrase, initVector);
                  
                    foreach (string strField in fieldListArray)
                    {
                        
                        int index = dr.Table.Columns[strField.Trim()].Ordinal;
                        
                        string currentvalue = e.Item.Cells[index+2].Text;
                        if (currentvalue != "&nbsp;") {
                            try
                            {
                                string ciphertext = rijndaelKey.Decrypt(currentvalue);
                                e.Item.Cells[index + 2].Text = ciphertext;
                            }
                            catch
                            {

                            }

                            }
                       
                    }

                }
                if (dr.Table.Columns["CallDuration"] != null)
                {
                    int callDurationOrdinal = dr.Table.Columns["CallDuration"].Ordinal;
                    
                    TimeSpan ts = TimeSpan.FromSeconds(
                        dr.IsNull("CallDuration") ? 0.00 : double.Parse(dr["CallDuration"].ToString())
                    );

                    e.Item.Cells[callDurationOrdinal + 2].Text =
                        string.Format("{0:00}:{1:00}:{2:00}", Math.Truncate(ts.TotalHours), ts.Minutes, ts.Seconds);
                }
                
            }
        }

        protected void ddlView_Change(object sender, EventArgs e)
        {
            try
            {
                // Apply view parameters and refresh data grid
                BuildFieldList();
                BuildDataGridQuery();
                // Set the grid control page index to 0 - page 1
                Session["GridPageIndex"] = "0";
                Session["SortExp"] = null;
                Session["SortDir"] = null;
                BindDataGrid();
            }
            catch (Exception ex)
            {
                PageMessage = "Exception changing view when control changed: " + ex.Message;
            }
        }

        #endregion

        private int GetColumnIndexByName(GridView grid, string name)
        {
            foreach (DataControlField col in grid.Columns)
            {
                if (col.HeaderText.ToLower().Trim() == name.ToLower().Trim())
                {
                    return grid.Columns.IndexOf(col);
                }
            }

            return -1;
        }

        protected void InitializeControls()
        {
            // Set connections to fire up master data sources
            try 
	        {	        
		        dsCampaigns.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
                dsViews.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
	        }
	        catch (Exception)
	        {
        		PageMessage = "There has been an error loading database connections, please contact your administrator.";
		        return;
	        }

            try
            {
                // Set the agent(user) and campaign if we have them
                if (Session["LoggedAgent"] != null)
                {
                    // we have an existing agent
                    currentAgent = (Agent)Session["LoggedAgent"];
                }
                else
                {
                    currentAgent = null;
                }
                if (Session["Campaign"] != null)
                {
                    // we have an existing campaign
                    currentCampaign = (Campaign)Session["Campaign"];
                }
                else
                {
                    currentCampaign = null;
                }

                // Populate Records per page dropdown
                ddlRecPerPage.Items.Add("10");
                ddlRecPerPage.Items.Add("15");
                ddlRecPerPage.Items.Add("20");
                ddlRecPerPage.Items.Add("25");
                ddlRecPerPage.Items.Add("30");
                ddlRecPerPage.Items.Add("50");
                ddlRecPerPage.Items.Add("75");
                ddlRecPerPage.Items.Add("100");
                ddlRecPerPage.Items.Add("150");
                ddlRecPerPage.Items.Add("200");
                ddlRecPerPage.Items.Add("500");

                // If we have a logged in user, load up their views and populate
                if (currentAgent != null)
                {
                    dsViews.SelectCommand = string.Format("SELECT ViewName FROM DataManagerViews WHERE AgentID = {0} ORDER BY ViewName ASC", currentAgent.AgentID);
                    dsViews.DataBind();
                    ddlViews.DataSource = dsViews;
                    ddlViews.DataTextField = "ViewName";
                    ddlViews.DataValueField = "ViewName";
                    ddlViews.DataBind();
                }

                ddlViews.Items.Add("Show All");

                ddlViews.SelectedValue = "Show All";

                // *** Bind recs per page here
                ddlRecPerPage.SelectedValue = "50";

                // load up list of all campaigns, sort alphabetically            
 
                dsCampaigns.SelectCommand = string.Format("SELECT CampaignID, Description FROM Campaign WHERE IsDeleted = 0 ORDER BY ShortDescription ASC");

                dsCampaigns.DataBind();
                ddlCampaign.DataSource = dsCampaigns;
                ddlCampaign.DataTextField = "Description";
                ddlCampaign.DataValueField = "CampaignID";
                ddlCampaign.DataBind();

                if (currentCampaign != null)
                {
                    ddlCampaign.SelectedValue = currentCampaign.CampaignID.ToString();
                    ddlViews.Enabled = true;
                    ddlQuery.Enabled = true;
                    ddlRecPerPage.Enabled = true;
                    btnDeleteColumn.Enabled = true;
                    btnExport.Enabled = true;
                    btnAddColumn.Enabled = true;
                    btnSaveView.Enabled = true;
                    btnNewQuery.Enabled = true;
                    lblNoData.Visible = false;
                }
                else
                {
                    ddlCampaign.Items.Add("Select a Campaign");
                    ddlCampaign.SelectedValue = "Select a Campaign";
                    // Disable controls since we don't even have a campaign
                    ddlViews.Enabled = false;
                    ddlQuery.Enabled = false;
                    ddlRecPerPage.Enabled = false;
                    btnDeleteColumn.Enabled = false;
                    btnExport.Enabled = false;
                    btnAddColumn.Enabled = false;
                    btnSaveView.Enabled = false;
                    btnNewQuery.Enabled = false;
                    lblNoData.Text = "There is no campaign currently selected.  Select one above and data will appear here.";
                    lblNoData.Visible = true;
                }

                // If we have a campaign, build query list and select id = 1 (all numbers) or lowest ID
                ResetQueryControl();

                // Set the grid control page index to 0 - page 1
                Session["GridPageIndex"] = "0";
                Session["RecsPerPage"] = "50";
            }
            catch (Exception ex)
            {
                PageMessage = "Exception initializing controls: " + ex.Message;
            }  
        }

        protected void ResetQueryControl()
        {
            // If we have a campaign, build query list and select id = 1 (all numbers) or lowest ID
            try
            {
                if (currentCampaign != null)
                {
                    dsQueries.ConnectionString = currentCampaign.CampaignDBConnString;
                    dsQueries.SelectCommand = "SELECT QueryID, QueryName, QueryCondition FROM Query ORDER BY QueryID ASC";
                    dsQueries.DataBind();
                    ddlQuery.DataSource = dsQueries;
                    ddlQuery.DataValueField = "QueryID";
                    ddlQuery.DataTextField = "QueryName";
                    ddlQuery.DataBind();
                    currentQueryID = Convert.ToInt64(ddlQuery.SelectedValue);
                }
                else
                {
                    ddlQuery.Items.Add("Select a Query");
                    currentQueryID = 0;
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Exception initializing query selector control: " + ex.Message;
                throw;
            }

        }

        protected void BuildFieldList()
        {
            string customFieldsList = "";
            string encryptedFieldsList = "";
            string normalFieldsList = "";
            string readonlyFieldsList = "";
            try
            {
                if (Session["Campaign"] != null)
                {
                    // we have an existing campaign
                    currentCampaign = (Campaign)Session["Campaign"];
                }
                else
                {
                    currentCampaign = null;
                    try
                    {
                        Session["GridFields"] = ConfigurationManager.AppSettings["ShowAllFieldsList"];
                    }
                    catch
                    {
                        Session["GridFields"] = "PhoneNum, FirstName, LastName, Address, City, State, Zip, NeverCallFlag, CallResultCode, DateTimeofCall";
                    }
                    return;
                }

                
                // Build a version for the data grid.  If we are selected "Show All", then build with all show fields

                switch (ddlViews.SelectedValue) 
                {
                    case "Show All":
                        try
                        {
                            Session["GridFields"] = ConfigurationManager.AppSettings["ShowAllFieldsList"];
                        }
                        catch
                        {
                            Session["GridFields"] = "PhoneNum, FirstName, LastName, Address, City, State, Zip, NeverCallFlag, CallResultCode, DateTimeofCall";
                        }

                        try
                        {
                            if (ConfigurationManager.AppSettings["ShowCustomCampaignFields"].ToLower() == "yes") includeCustomFields = true;
                            else includeCustomFields = false;
                        }
                        catch
                        {
                            includeCustomFields = true;
                        }
                        if (includeCustomFields)
                        {
                            dsCustomFields.ConnectionString = currentCampaign.CampaignDBConnString;
                            dsCustomFields.SelectCommand = "SELECT FieldName,FieldTypeID,ReadOnly FROM CampaignFields WHERE FieldID > 8";
                            dsCustomFields.DataBind();
                            DataView dv = new DataView();
                            dv = (DataView)dsCustomFields.Select(DataSourceSelectArguments.Empty);
                            string fieldtype;
                            string readonlyfield;
                           
                            foreach (DataRowView drv1 in dv)
                            {
                                fieldtype = drv1["FieldTypeID"].ToString();
                                if (fieldtype == "7")
                                {
                                    ActivityLogger.WriteAdminEntry(currentCampaign, "Data Portal adding encrypted field to list: '{0}'", drv1["FieldName"]);
                                    encryptedFieldsList = encryptedFieldsList + ", " + drv1["FieldName"];
                               
                                }
                                readonlyfield = drv1["ReadOnly"].ToString();
                                //ActivityLogger.WriteAdminEntry(currentCampaign, "readonly field: '{0}'", readonlyfield);
                                if (readonlyfield == "True")
                                {
                                    ActivityLogger.WriteAdminEntry(currentCampaign, "Data Portal adding read only attribute on '{0}'", drv1["FieldName"]);
                                    readonlyFieldsList = readonlyFieldsList + ", " + drv1["FieldName"];
                                }

                                customFieldsList = customFieldsList + ", " + drv1["FieldName"];
                            }

                            customFieldsList = customFieldsList.TrimStart(',');
                            customFieldsList = customFieldsList.Trim();
                            customFieldsList = customFieldsList.TrimEnd(',');
                            encryptedFieldsList = encryptedFieldsList.TrimStart(',');
                            encryptedFieldsList = encryptedFieldsList.Trim();
                            encryptedFieldsList = encryptedFieldsList.TrimEnd(',');
                            readonlyFieldsList = readonlyFieldsList.TrimStart(',');
                            readonlyFieldsList = readonlyFieldsList.Trim();
                            readonlyFieldsList = readonlyFieldsList.TrimEnd(',');

                            ActivityLogger.WriteAdminEntry(currentCampaign, "Data manager custom field list created: '{0}'",  customFieldsList);
                        }

                        normalFieldsList = ConfigurationManager.AppSettings["ShowAllFieldsList"];
                        if (customFieldsList.Length > 0)
                            Session["GridFields"] = string.Format("{0},{1}", normalFieldsList, customFieldsList);
                        else
                            Session["GridFields"] = normalFieldsList;
                        if (encryptedFieldsList.Length > 0)
                            Session["EncryptedFields"] = string.Format("{0}", encryptedFieldsList);
                        else
                            Session.Remove("EncryptedFields");
                        if (readonlyFieldsList.Length > 0)
                            Session["ReadOnlyFields"] = string.Format("{0}", readonlyFieldsList);
                        else
                            Session.Remove("ReadOnlyFields");
                        break;
                    case "Custom":
                        break;
                    default:
               
                        if (Session["LoggedAgent"] != null)
                        {
                            // we have an existing agent
                            currentAgent = (Agent)Session["LoggedAgent"];
                        }
                        else
                        {
                            currentAgent = null;
                        }
                        // Load a custom view field list
                        if (currentAgent != null)
                        {
                            dsViews.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
                            dsViews.SelectCommand = string.Format("SELECT FieldList, RecordsPerPage FROM DataManagerViews WHERE AgentID = {0} AND ViewName = '{1}'", currentAgent.AgentID, ddlViews.SelectedValue);
                            DataView dv = new DataView();
                            dv = (DataView)dsViews.Select(DataSourceSelectArguments.Empty);

                            if (dv.Count < 1)
                            {
                                PageMessage = "View could not be found, please check your database or contact your administrator.";
                                return;
                            }

                            DataRowView drv = dv[0];

                            //DataView dv = new DataView();
                            //dv = (DataView)dsQueryDetail.Select(DataSourceSelectArguments.Empty);
                            //DataRowView drv = dv[0];
                            string tempFieldList = drv["FieldList"] as string;
                            string recordsPerPage = drv["RecordsPerPage"] as string;

                            ddlRecPerPage.SelectedValue = recordsPerPage;

                            string[] fieldListArray = tempFieldList.Split(',');
                            List<string> validFields = new List<string>();
                            dsViewDetail.ConnectionString = currentCampaign.CampaignDBConnString;

                            foreach (string strField in fieldListArray)
                            {
                                dsViewDetail.SelectCommand = string.Format("Select COUNT(*) FROM Information_Schema.columns WHERE Table_name = 'Campaign' and Column_name ='{0}'", strField.Trim());
                                DataView dv1 = new DataView();
                                dv1 = (DataView)dsViewDetail.Select(DataSourceSelectArguments.Empty);
                                DataRowView drv1 = dv1[0];

                                string fieldExists = drv1[0].ToString();

                                if (fieldExists != null)
                                {
                                    if (Convert.ToInt16(fieldExists) > 0)
                                    {
                                        validFields.Add(strField);
                                    }
                                    else
                                    {
                                        ActivityLogger.WriteAdminEntry(currentCampaign, "Data manager view {0} contains a non existant field '{1}'.", ddlViews.SelectedValue, strField);
                                    }
                                }
                            }

                            foreach (string strField in validFields)
                            {
                                customFieldsList = customFieldsList + ", " + strField;
                            }
                            customFieldsList = customFieldsList.TrimStart(',');
                            customFieldsList = customFieldsList.TrimEnd(',');
                            customFieldsList = customFieldsList.Trim();
                            Session["GridFields"] = customFieldsList;
                            break;
                        }
                        else
                        {
                            PageMessage = "There has been an error with your login.  Please log back in and try again.";
                            return;
                        }

                }
                currentFieldList = Session["GridFields"].ToString();
            }
            catch (Exception ex)
            {
                PageMessage = "Error building field list : " + ex.Message;
            }
        } 
   
        protected void BuildDataGridQuery()
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
            try
            {
                long QueryID = 0;
                QueryID = Convert.ToInt64(ddlQuery.SelectedValue);
                // Load up this campaign object
                if (QueryID > 0)
                {
                    currentQueryID = QueryID;
                }
                else
                {
                    // Problem with selected campaign, force the control back to original
                    ddlQuery.SelectedValue = currentQueryID.ToString();
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Error binding query ID from control : " + ex.Message;
            }
            
            // Build a select statement out of the quey in the db
            if (currentCampaign != null && currentQueryID > 0)
            {
                try
                {
                    // We have a campaign and a query, build the right conditions for the grid
                    dsQueryDetail.ConnectionString = currentCampaign.CampaignDBConnString;
                    dsQueryDetail.SelectCommand = string.Format("SELECT QueryCondition FROM Query WHERE QueryID = {0}", currentQueryID);
                    dsQueryDetail.DataBind();
                    DataView dv = new DataView();
                    dv = (DataView)dsQueryDetail.Select(DataSourceSelectArguments.Empty);
                    DataRowView drv = dv[0];
                    string tempQuerySelectStmt = drv["QueryCondition"] as string;

                    // Chop up query and compile new one ......
                    ActivityLogger.WriteAdminEntry(currentCampaign, "Data Manager - original query '{0}'", tempQuerySelectStmt);
                    if (tempQuerySelectStmt.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") > 0)
                        currentQueryConditions = tempQuerySelectStmt.Substring((tempQuerySelectStmt.IndexOf("WHERE (") + 7), (tempQuerySelectStmt.IndexOf("And ((DATEPART(hour, GETDATE()) < 13") - (tempQuerySelectStmt.IndexOf("WHERE (") + 7)));
                    if (tempQuerySelectStmt.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") > 0)
                        currentQueryConditions = tempQuerySelectStmt.Substring((tempQuerySelectStmt.IndexOf("WHERE (") + 7), (tempQuerySelectStmt.IndexOf("AND ((DATEPART(hour, GETDATE()) < 13") - (tempQuerySelectStmt.IndexOf("WHERE (") + 7)));

                    currentQueryConditions = currentQueryConditions.Trim();
                    ActivityLogger.WriteAdminEntry(currentCampaign, "Extracted conditions: '{0}'", currentQueryConditions);
                    
                    if (chkResultNames.Visible == true && chkResultNames.Checked == true && (currentFieldList.IndexOf("CallResultCode") > 0))
                    {
                        currentFieldList = currentFieldList.Replace("CallResultCode", "ResultCode.Description AS CallResult"); 
                        currentQuerySelectStmt = string.Format("SELECT {0}, UniqueKey FROM Campaign LEFT JOIN ResultCode ON Campaign.CallResultCode = ResultCode.ResultCodeID WHERE {1}", currentFieldList, currentQueryConditions);
                    }
                    else
                    {
                        currentQuerySelectStmt = string.Format("SELECT {0}, UniqueKey FROM Campaign WHERE {1}", currentFieldList, currentQueryConditions);
                    }
                    Session["SelectStmt"] = currentQuerySelectStmt;
                }
                catch (Exception ex)
                {
                    PageMessage = "Error loading query conditions : " + ex.Message;
                }
            }
        }

        protected void BindDataGrid() 
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
                dsMainGrid.SelectCommand = currentQuerySelectStmt;
                dsMainGrid.DataBind();
                grdDataPortal.DataSource = dsMainGrid;
                grdDataPortal.PageSize = Convert.ToInt16(ddlRecPerPage.SelectedValue);
                try
                {
                    grdDataPortal.CurrentPageIndex = Convert.ToInt16(Session["GridPageIndex"]);
                }
                catch
                {
                    grdDataPortal.CurrentPageIndex = 0;
                }
                
                    // First column (after 0 - edit, 1 delete) is Unique key, do not show.
                grdDataPortal.DataBind();
                // Bind record count and update label ahowing query conditions and count 
                DataView dv = (DataView)dsMainGrid.Select(DataSourceSelectArguments.Empty);
                currentRecordCount = dv.Count;
                if (Session["SortExp"] != null)
                {
                    if ((string)Session["SortDir"] == "desc")
                        lblConditions.Text = string.Format("Query Conditions: '{0}' returns {1} total records. Currently sorted by {2} {3}", currentQueryConditions, currentRecordCount.ToString(), (string)Session["SortExp"], "descending");
                    else
                        lblConditions.Text = string.Format("Query Conditions: '{0}' returns {1} total records. Currently sorted by {2} {3}", currentQueryConditions, currentRecordCount.ToString(), (string)Session["SortExp"], "ascending");
                }
                else
                {
                    lblConditions.Text = string.Format("Query Conditions '{0}' return {1} total records.", currentQueryConditions, currentRecordCount.ToString());
                }
                EnableDataControls();
            }
            catch (Exception ex)
            {
                PageMessage = "Error binding data grid : " + ex.Message;
            }

        }

        private void EnableDataControls()
        {
            btnDeleteColumn.Enabled = true;
            btnDeleteColumn.Attributes.Add("OnClick", "javascript:DeleteColumn()");
            btnAddColumn.Enabled = true;
            btnAddColumn.Attributes.Add("OnClick", "javascript:AddColumn()");
            btnSaveView.Enabled = true;
            btnSaveView.Attributes.Add("OnClick", "javascript:SaveView()");
            btnExport.Enabled = true;
            btnExport.Attributes.Add("OnClick", "javascript:ExportData()");
            if ((currentFieldList.IndexOf("CallResultCode") > 0) || (currentFieldList.IndexOf("ResultCode.Description AS CallResult") > 0)) 
            {
                chkResultNames.Visible = true;
            }
            else
            {
                chkResultNames.Visible = false;
            }
        }

        protected void grdDataPortal_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
