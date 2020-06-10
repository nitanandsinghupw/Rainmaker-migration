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
    public partial class MasterDNC : PageBase
    {
        

        private Agent currentAgent;
        
       
        
        
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


                try
                {

                    InitializeControls();
                    BuildFieldList();
                    BuildDataGridQuery();
                    BindDataGrid();
                    string action = GetQueryString("action");
                    string phonenumber = GetQueryString("phonenumber");
                    if (action == "delete")
                    {
                        lblMessage.Text = "Phone number " + phonenumber + " has been successfully deleted.";

                        hdnPhoneDelete.Value = "";
                    }
                    if (action == "add")
                    {
                        lblMessage.Text = "Phone number " + phonenumber + " has been successfully added.";

                        hdnPhone.Value = "";
                    }

                    txtPhoneNumber.Focus();
                }
                catch (Exception ex)
                {
                    PageMessage = "Exception loading page: " + ex.Message;
                }
            }
            else
            {
                //if PhoneNumber has been posted back in hidden field then add to database
                
                
                if (hdnPhoneDelete.Value != "")
                {
                    
                    DeletePhoneNumber();
                    
                }
                if (hdnPhone.Value != "")
                {

                    InsertPhoneNumber();
                    lblMessage.Text = "Phone number " + hdnPhone.Value + " has been successfully added.";
                    hdnAction.Value = "";
                    hdnPhone.Value = "";
                    

                }

            }
           
        }
        protected void ddlRecPerPage_Change(object sender, EventArgs e)
        {
            try
            {
                // Apply view parameters and refresh data grid
                BuildFieldList();
                BuildDataGridQuery();
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
            
            BindDataGrid();
        }

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        /*protected void grdDataPortal_EditCommand(object source, DataGridCommandEventArgs e)
        {
            ActivityLogger.WriteAdminEntry(null, "grdDataPortal_EditCommand '{0}'", "triggered");

            //-----------------------------------------------------
            // Don: forcing the name to away, back to numbers
            // for the result codes.
            //-----------------------------------------------------
           

            BuildFieldList();
            BuildDataGridQuery();
            
            grdDataPortal.EditItemIndex = e.Item.ItemIndex;
            grdDataPortal.Columns[1].Visible = false;
            BindDataGrid();


        }*/

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        /*protected void grdDataPortal_CancelEdit(object source, DataGridCommandEventArgs e)
        {
            BuildFieldList();
            BuildDataGridQuery();
            
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
            ActivityLogger.WriteAdminEntry(null, "grdDataPortal_UpdateRecord '{0}'", "triggered");
            try
            {
                bool bContinue = true;


                
                if (Session["GridFields"] != null)
                {
                    StringBuilder sbSqlStmt = new StringBuilder();
                    string[] strFieldNames = Session["GridFields"].ToString().Split(',');
                    sbSqlStmt.AppendFormat("UPDATE MasterDNC SET ");

                    // Formatting as sql string types for now ... handle data types?
                    sbSqlStmt.AppendFormat("{0} = '{1}'", strFieldNames[0].Trim(), ((TextBox)e.Item.Cells[3].Controls[0]).Text.prepSQL());
                    //DataRow dr = ((DataRowView)e.Item.DataItem).Row;
                    
                    for (int i = 3; i < (e.Item.Cells.Count - 1); i++)
                    {
                        if (((TextBox)e.Item.Cells[i].Controls[0]).Text.Length > 0)
                            sbSqlStmt.AppendFormat(", {0} = '{1}'", strFieldNames[i - 2].Trim(), ((TextBox)e.Item.Cells[i].Controls[0]).Text.prepSQL());
                        else
                            sbSqlStmt.AppendFormat(", {0} = NULL", strFieldNames[i - 2].Trim());
                    }

                    sbSqlStmt.AppendFormat(" WHERE UniqueKey = {0}", ((TextBox)e.Item.Cells[e.Item.Cells.Count - 2].Controls[0]).Text.prepSQL());

                    string sqlStmt = sbSqlStmt.ToString();
                    string campaignMasterDBConn = ConfigurationManager.AppSettings["MasterConnectionString"];
                    dsMainGrid.ConnectionString = "Server=leadsweeper-dev\\SQLEXPRESS;Database=RainmakerMaster;User Id=sa;Password=R@inM@ker;";
                    dsMainGrid.SelectCommand = sqlStmt;
                    dsMainGrid.Select(DataSourceSelectArguments.Empty);
                    /*
                     * D. Pollastrini
                     * 2012-04-18
                     * 
                     * Update validation code to determine whether CallResultCode is NULL or digits.  If so, OK to continue.
                     */

                    //bContinue = new Regex(@"CallResultCode\s*=\s*(NULL|'\d+'),").IsMatch(sqlStmt);

                    /*if (bContinue == true)
                    {
                        ActivityLogger.WriteAdminEntry(null, "Update Statement to Execute: '{0}'", sqlStmt);
                        dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                        dsMainGrid.SelectCommand = sqlStmt;
                        dsMainGrid.Select(DataSourceSelectArguments.Empty);

                    }
                    else
                    {
                        PageMessage = "Invalid Call Result Code - numeric value required.";
                        bContinue = false;
                    }*/

                /*}

                if (bContinue == true)
                {
                    BuildFieldList();
                    BuildDataGridQuery();
                    
                    grdDataPortal.EditItemIndex = -1;
                    grdDataPortal.Columns[1].Visible = true;
                    BindDataGrid();

                }
            }
            catch (Exception ex)
            {
                PageMessage = "Exception updating record: " + ex.Message;

            }
        }*/

        protected void grdDataPortal_DeleteRecord(object source, DataGridCommandEventArgs e)
        {
            // Update the database here
            try
            {
                
                string[] strFieldNames = Session["GridFields"].ToString().Split(',');
                int ColumnCount = 0;
                foreach (string str in strFieldNames) ColumnCount++;
                StringBuilder sbSqlStmt = new StringBuilder();
                
                
                sbSqlStmt.AppendFormat("DELETE FROM MasterDNC ");
                
                sbSqlStmt.AppendFormat(" WHERE UniqueKey = {0}", e.Item.Cells[ColumnCount].Text);
               
                
                string sqlStmt = sbSqlStmt.ToString();
                
                ActivityLogger.WriteAdminEntry(null, "Delete Statement to Execute: '{0}'", sqlStmt);
                
                //string campaignMasterDBConn = ConfigurationManager.AppSettings["MasterConnectionString"];
                //dsMainGrid.ConnectionString = campaignMasterDBConn;

                dsMainGrid.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
                dsMainGrid.SelectCommand = sqlStmt;
                dsMainGrid.Select(DataSourceSelectArguments.Empty);

                lblMessage.Text = "Phone number " + e.Item.Cells[ColumnCount+1].Text + " has been successfully deleted.";
                txtPhoneNumber.Focus();

                BuildFieldList();
                BuildDataGridQuery();
                
                grdDataPortal.EditItemIndex = -1;
                BindDataGrid();
                
            }
            catch (Exception ex)
            {
                PageMessage = "Exception deleting record: " + ex.Message;
            }
        }

        /*protected void grdDataPortal_DataBind(object source, DataGridItemEventArgs e)
        {

            // This code will disable the unique key field from editing

            if (e.Item.ItemType == ListItemType.EditItem)
            {

                ((TextBox)e.Item.Cells[e.Item.Cells.Count - 2].Controls[0]).Enabled = false;
                DataRow dr1 = ((DataRowView)e.Item.DataItem).Row;
                /*int index1 = dr1.Table.Columns.IndexOf("PHONENUM");
                try
                {
                    if (((TextBox)e.Item.Cells[index1 + 2].Controls[0]).Enabled == true)
                    {
                        ((TextBox)e.Item.Cells[index1 + 2].Controls[0]).Enabled = false;
                    }
                }
                catch { }
                */
      /*          if (Session["ReadOnlyFields"] != null)
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


           
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRow dr = ((DataRowView)e.Item.DataItem).Row;

                
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
        }*/

       

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

                

                // *** Bind recs per page here
                ddlRecPerPage.SelectedValue = "50";

                
                /*if (currentCampaign != null)
                {

                    
                  
                    ddlRecPerPage.Enabled = true;

                    

                   
                    
                    lblNoData.Visible = false;
                }
                else
                { */


                    // Disable controls since we don't even have a campaign
                    
                    
                    ddlRecPerPage.Enabled = false;
                    
                    lblNoData.Text = "There are no Master Do Not Call records in the system.";
                    lblNoData.Visible = true;
                //}

                

                // Set the grid control page index to 0 - page 1
                Session["GridPageIndex"] = "0";
                Session["RecsPerPage"] = "50";
            }
            catch (Exception ex)
            {
                PageMessage = "Exception initializing controls: " + ex.Message;
            }
        }

        

        protected void BuildFieldList()
        {
            //Session["GridFields"] = ConfigurationManager.AppSettings["ShowAllFieldsList"];
            Session["GridFields"] = "PhoneNum";
            /* string customFieldsList = "";
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

                             ActivityLogger.WriteAdminEntry(currentCampaign, "Data manager custom field list created: '{0}'", customFieldsList);
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

                         /*    default:
                
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
             } */
        }

        protected void BuildDataGridQuery()
        {
            //
            

           
                
        }

        protected void BindDataGrid()
        {
            try
            {
                
                // Bind data grid to the query we have built and display it.
                
                //dsMainGrid.ConnectionString = campaignMasterDBConn;
                dsMainGrid.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
                StringBuilder sbSqlStmt = new StringBuilder();
                sbSqlStmt.AppendFormat("SELECT UniqueKey,PhoneNum FROM MasterDNC ");
                string sqlStmt = sbSqlStmt.ToString();
                dsMainGrid.SelectCommand = sqlStmt;
                dsMainGrid.Select(DataSourceSelectArguments.Empty);
                
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
                if (currentRecordCount > 0) {
              

                   ddlRecPerPage.Enabled = true;
                    
                   lblNoData.Visible = false;
               }
               else
               {


                // Disable controls since we don't even have a campaign


                ddlRecPerPage.Enabled = false;

                lblNoData.Text = "There are no Master Do Not Call records in the system.";
                lblNoData.Visible = true;
                }

               
            }
            catch (Exception ex)
            {
                PageMessage += "Error binding data grid : " + ex.Message;
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                string value = txtPhoneNumber.Text;
                value = Regex.Replace(value, @"[^\d]", "");
                bool allowTenDigitNums = true;
                bool allowSevenDigitNums = false;
                switch (value.Length)
                {
                    case 7:
                        if (!allowSevenDigitNums)
                        {

                            PageMessage = "Phone Number is invalid";
                            txtPhoneNumber.Focus();
                            return;

                        }
                        break;
                    case 10:
                        if (!allowTenDigitNums)
                        {

                            PageMessage = "Phone Number is invalid";
                            txtPhoneNumber.Focus();
                            return;

                        }
                        break;

                    default:
                        PageMessage = "Phone Number is invalid";
                        txtPhoneNumber.Focus();
                        return;

                }
                if (txtPhoneNumber.Text == "")
                {
                    PageMessage = "Please enter a valid phone number to delete from the list.";
                    txtPhoneNumber.Focus();
                    return;

                }
                // Check if phone number exists already in database.
                
                //string campaignMasterDBConn = ConfigurationManager.AppSettings["MasterConnectionString"];
                //dsViews.ConnectionString = campaignMasterDBConn;
                dsViews.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
                

                dsViews.SelectCommand = string.Format("SELECT PhoneNum FROM MasterDNC WHERE PhoneNum = '{0}'", txtPhoneNumber.Text);
                DataView dv = new DataView();
                dv = (DataView)dsViews.Select(DataSourceSelectArguments.Empty);

                if (dv.Count > 0)
                {
                    StringBuilder Message = new StringBuilder();
                    Message.AppendFormat("The Phone Number {0} already exists in the Master Do Not Call database",txtPhoneNumber.Text);
                    PageMessage = Message.ToString(); 
                    txtPhoneNumber.Text = "";
                    txtPhoneNumber.Focus();
                    return;
                }
                else
                {
                    dsViews.SelectCommand = string.Format("INSERT INTO MasterDNC (PhoneNum) VALUES ('{0}')", txtPhoneNumber.Text);
                    DataView dv1 = new DataView();
                    dv1 = (DataView)dsViews.Select(DataSourceSelectArguments.Empty);

                }
                BuildFieldList();
                BuildDataGridQuery();

                
                BindDataGrid();

                lblMessage.Text = "Phone number " + txtPhoneNumber.Text + " has been successfully added.";
                txtPhoneNumber.Text = "";
                txtPhoneNumber.Focus();
                /*StringBuilder sbSqlStmt = new StringBuilder();
                sbSqlStmt.AppendFormat("SELECT UniqueKey,PhoneNum FROM MasterDNC ");
                string sqlStmt = sbSqlStmt.ToString();
                dsMainGrid.SelectCommand = sqlStmt;
                dsMainGrid.Select(DataSourceSelectArguments.Empty);
                //dsMainGrid.ConnectionString = currentCampaign.CampaignDBConnString;
                //dsMainGrid.SelectCommand = currentQuerySelectStmt;
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
                */
            }
            catch (Exception ex)
            {
                PageMessage += "Error checking for or adding record to MasterDNC database : " + ex.Message;
            }

        }

        protected void btnVerify_Click(object sender, EventArgs e)
        {
            string value = txtPhoneNumber.Text;
            value = Regex.Replace(value, @"[^\d]", "");
            bool allowTenDigitNums = true;
            bool allowSevenDigitNums = false;
            switch (value.Length)
            {
                case 7:
                    if (!allowSevenDigitNums)
                    {

                        PageMessage = "Phone Number is invalid";
                        txtPhoneNumber.Focus();
                        return;

                    }
                    break;
                case 10:
                    if (!allowTenDigitNums)
                    {

                        PageMessage = "Phone Number is invalid";
                        txtPhoneNumber.Focus();
                        return;

                    }
                    break;

                default:
                    PageMessage = "Phone Number is invalid";
                    txtPhoneNumber.Focus();
                    return;

            }
            if (txtPhoneNumber.Text == "")
            {
                PageMessage = "Please enter a valid phone number to delete from the list.";
                txtPhoneNumber.Focus();
                return;

            }
            // Check if phone number exists already in database.

            //string campaignMasterDBConn = ConfigurationManager.AppSettings["MasterConnectionString"];
            //dsViews.ConnectionString = campaignMasterDBConn;
            dsViews.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
            

            dsViews.SelectCommand = string.Format("SELECT PhoneNum FROM MasterDNC WHERE PhoneNum = '{0}'", txtPhoneNumber.Text);
            DataView dv = new DataView();
            dv = (DataView)dsViews.Select(DataSourceSelectArguments.Empty);

            if (dv.Count > 0)
            {
                StringBuilder Message = new StringBuilder();
                Message.AppendFormat("The Phone Number {0} already exists in the Master Do Not Call database", txtPhoneNumber.Text);
                PageMessage = Message.ToString(); 
                txtPhoneNumber.Text = "";
                txtPhoneNumber.Focus();
                return;
            }
            else
            {
                hdnPhone.Value = txtPhoneNumber.Text;
                StringBuilder Message = new StringBuilder();
                Message.AppendFormat("The Phone Number {0} does not exist in the Master Do Not Call database. Would you like to add it now?", txtPhoneNumber.Text);
                ConfirmMessage = Message.ToString(); 
                
                return;
            }

        }
        protected void InsertPhoneNumber()
        {
            try {

                StringBuilder sbSqlStmt = new StringBuilder();

                sbSqlStmt.AppendFormat("INSERT INTO MasterDNC (PhoneNum,DateTimeOfImport) VALUES ('{0}','{1}')", hdnPhone.Value, DateTime.Now);


                string sqlStmt = sbSqlStmt.ToString();

                ActivityLogger.WriteAdminEntry(null, "Insert Statement to Execute: '{0}'", sqlStmt);

                //string campaignMasterDBConn = ConfigurationManager.AppSettings["MasterConnectionString"];
                //dsMainGrid.ConnectionString = campaignMasterDBConn;
                dsMainGrid.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
                dsMainGrid.SelectCommand = sqlStmt;
                dsMainGrid.Select(DataSourceSelectArguments.Empty);
                hdnAction.Value = "add";
                //hdnPhone.Value = txtPhoneNumber.Text;
                

                Response.Redirect("~/Campaign/MasterDNC.aspx?action=add&phonenumber=" + txtPhoneNumber.Text);

            }
            catch (Exception ex)
            {
                PageMessage = "Exception adding record: " + ex.Message;
            }

        }
        protected void DeletePhoneNumber()
        {
            try
            {

                StringBuilder sbSqlStmt = new StringBuilder();

                sbSqlStmt.AppendFormat("DELETE FROM MasterDNC Where PhoneNum = '{0}'", hdnPhoneDelete.Value);


                string sqlStmt = sbSqlStmt.ToString();

                ActivityLogger.WriteAdminEntry(null, "Delete Statement to Execute: '{0}'", sqlStmt);

                //string campaignMasterDBConn = ConfigurationManager.AppSettings["MasterConnectionString"];
                //dsMainGrid.ConnectionString = campaignMasterDBConn;
                dsMainGrid.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
                dsMainGrid.SelectCommand = sqlStmt;
                dsMainGrid.Select(DataSourceSelectArguments.Empty);

                hdnPhoneDelete.Value = txtPhoneNumber.Text;

                Response.Redirect("~/Campaign/MasterDNC.aspx?action=delete&phonenumber=" + txtPhoneNumber.Text);

            }
            catch (Exception ex)
            {
                PageMessage = "Exception adding record: " + ex.Message;
            }

        }
        protected void btnImportNumbers_Click(object sender, EventArgs e)
        {
            Response.Redirect("~\\Campaign\\ImportDNC.aspx");

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string value = txtPhoneNumber.Text;
            value = Regex.Replace(value, @"[^\d]", "");
            bool allowTenDigitNums = true;
            bool allowSevenDigitNums = false;
            switch (value.Length)
            {
                case 7:
                    if (!allowSevenDigitNums)
                    {

                        PageMessage = "Phone Number is invalid";
                        txtPhoneNumber.Focus();
                        return;

                    }
                    break;
                case 10:
                    if (!allowTenDigitNums)
                    {

                        PageMessage = "Phone Number is invalid";
                        txtPhoneNumber.Focus();
                        return;

                    }
                    break;

                default:
                    PageMessage = "Phone Number is invalid";
                    txtPhoneNumber.Focus();
                    return;

            }
            if (txtPhoneNumber.Text == "")
            {
                PageMessage = "Please enter a valid phone number to delete from the list.";
                txtPhoneNumber.Focus();
                return;

            }
            // Check if phone number exists already in database.

            //string campaignMasterDBConn = ConfigurationManager.AppSettings["MasterConnectionString"];
            //dsViews.ConnectionString = campaignMasterDBConn;
            dsViews.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];


            dsViews.SelectCommand = string.Format("SELECT PhoneNum FROM MasterDNC WHERE PhoneNum = '{0}'", txtPhoneNumber.Text);
            DataView dv = new DataView();
            dv = (DataView)dsViews.Select(DataSourceSelectArguments.Empty);

            if (dv.Count > 0)
            {
                hdnPhoneDelete.Value = txtPhoneNumber.Text;
                StringBuilder Message = new StringBuilder();
                Message.AppendFormat("The Phone Number {0} already exists in the Master Do Not Call database.  Would you like to delete it now?", txtPhoneNumber.Text);
                ConfirmMessage2 = Message.ToString();
                
                return;
            }
            else
            {
                
                StringBuilder Message = new StringBuilder();
                Message.AppendFormat("The Phone Number {0} does not exist in the Master Do Not Call database.", txtPhoneNumber.Text);
                PageMessage = Message.ToString();

                return;
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
