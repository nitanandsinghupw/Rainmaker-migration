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
using System.Globalization;

namespace Rainmaker.Web.campaign
{
    public partial class Home : PageBase
    {
        protected NumberFormatInfo _numberFormatInfo;

        #region Events
        public bool Isrunning = false;
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            _numberFormatInfo = new NumberFormatInfo();
            _numberFormatInfo.PercentPositivePattern = 1;

            if (!IsPostBack)
            {
                ShowCampaignDetails(false);
                ClearSessions();
                try
                {
                    Timer1.Interval = Convert.ToInt32(ConfigurationManager.AppSettings["StatsUpdInterval"]);
                }
                catch
                {
                    Timer1.Interval = 5000; // 5 min - 5 * 60 * 1000
                }
                Timer1.Enabled = true;

                Campaign objCampaign;
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                    if (objCampaign.StatusID == (long)CampaignStatus.Idle)
                        Timer1.Enabled = false;
                    BindQueries(objCampaign, false);
                }
            }
        }

        /// <summary>
        /// Stops the campaign
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnIdle_Click(object sender, EventArgs e)
        {
            if (lbtnIdle.Text.IndexOf("[") >= 0)
                return;

            Timer1.Enabled = false;
            // Added GW 10.01.10 for going idle when dilaer is not running trap
            UpdateCampaignStatus(CampaignStatus.FlushIdle);
            Timer1.Enabled = true;
            ShowCampaignDetails(true);
        }

        /// <summary>
        /// Activate the campaign
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnRun_Click(object sender, EventArgs e)
        {

            try
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                if (objCampaign != null)
                {
                    CampaignService objCampService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                    objCampService.ScrubDNC(objCampaign.CampaignDBConnString);

                    DialingParameter objDialingParam = new DialingParameter();
                    objDialingParam = (DialingParameter)Serialize.DeserializeObject(objCampService.GetDialingParameter(xDocCampaign), "DialingParameter");

                    string AnsweringMachineMessage = objDialingParam.AnsweringMachineMessage;
                    string HumanMessage = objDialingParam.HumanMessage;

                    int dialingmode = objDialingParam.DialingMode;
                    HumanMessage = "a";
                    if ((AnsweringMachineMessage == "") || (HumanMessage == "")) { 
                        if (dialingmode == 6) {
                           return;
                        }
                    }

                    //Scrub Campaign table against Master DNC


                }

            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
            if (lbtnRun.Text.IndexOf("[") >= 0)
                return;

            Timer1.Enabled = false;
            //Filter out the DNC records
            
            UpdateCampaignStatus(CampaignStatus.Run);
            Timer1.Enabled = true;
            ShowCampaignDetails(true);
        }

        /// <summary>
        /// Halts the campaign
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnPause_Click(object sender, EventArgs e)
        {
            if (lbtnPause.Text.IndexOf("[") >= 0)
                return;

            Timer1.Enabled = false;
            Campaign objCampaign = (Campaign)Session["Campaign"];
            if (objCampaign.StatusID == (long)CampaignStatus.Idle || objCampaign.StatusID == (long)CampaignStatus.Idle)
            {
                PageMessage = "The campaign is currently idle.  You must run a campaign before pausing it.";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
            }
            else
            {
                UpdateCampaignStatus(CampaignStatus.FlushPaused);
                ShowCampaignDetails(true);
            }
            Timer1.Enabled = true;
        }
        #endregion

        #region Query Management Events

        protected void grdActiveQueries_DataBind(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbtnDelete = (LinkButton)e.Item.FindControl("lbtnDelete");
                LinkButton lbtnStandby = (LinkButton)e.Item.FindControl("lbtnStandby");
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
                    DataRowView item = (DataRowView)e.Item.DataItem;

                    double dials = Double.Parse(item["Dials"].ToString());
                    if (dials > 0)
                    {
                        int talks = Int32.Parse(item["Talks"].ToString());
                        int answeringMachine = Int32.Parse(item["AnsweringMachine"].ToString());
                        int noAnswer = Int32.Parse(item["NoAnswer"].ToString());
                        int busy = Int32.Parse(item["Busy"].ToString());
                        int optIn = Int32.Parse(item["OpInt"].ToString());
                        int drops = Int32.Parse(item["Drops"].ToString());
                        int failed = Int32.Parse(item["Failed"].ToString());

                        e.Item.Cells[4].Text = (talks / dials).ToString("P2", _numberFormatInfo);              // "Talks";
                        e.Item.Cells[5].Text = (answeringMachine / dials).ToString("P2", _numberFormatInfo);   // "AnsweringMachine";
                        e.Item.Cells[6].Text = (noAnswer / dials).ToString("P2", _numberFormatInfo);           // "NoAnswer";
                        e.Item.Cells[7].Text = (busy / dials).ToString("P2", _numberFormatInfo);               // "Busy";
                        e.Item.Cells[8].Text = (optIn / dials).ToString("P2", _numberFormatInfo);              // "OpInt";
                        e.Item.Cells[9].Text = (drops / dials).ToString("P2", _numberFormatInfo);              // "Drops";
                        e.Item.Cells[10].Text = (failed / dials).ToString("P2", _numberFormatInfo);            // "Failed";
                    }
                    else
                    {
                        e.Item.Cells[4].Text =
                        e.Item.Cells[5].Text =
                        e.Item.Cells[6].Text =
                        e.Item.Cells[7].Text =
                        e.Item.Cells[8].Text =
                        e.Item.Cells[9].Text =
                        e.Item.Cells[10].Text = (0.00).ToString("P2", _numberFormatInfo);
                    }

                }
            }
        }

        protected void grdStandbyQueries_DataBind(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbtnActivate = (LinkButton)e.Item.FindControl("lbtnActivate");
                LinkButton lbtnDelete = (LinkButton)e.Item.FindControl("lbtnDelete");

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
                    DataRowView item = (DataRowView)e.Item.DataItem;

                    double dials = Double.Parse(item["Dials"].ToString());
                    if (dials > 1)
                    {

                        int talks = Int32.Parse(item["Talks"].ToString());
                        int answeringMachine = Int32.Parse(item["AnsweringMachine"].ToString());
                        int noAnswer = Int32.Parse(item["NoAnswer"].ToString());
                        int busy = Int32.Parse(item["Busy"].ToString());
                        int optIn = Int32.Parse(item["OpInt"].ToString());
                        int drops = Int32.Parse(item["Drops"].ToString());
                        int failed = Int32.Parse(item["Failed"].ToString());

                        e.Item.Cells[3].Text = (talks / dials).ToString("P2", _numberFormatInfo);              // "Talks";
                        e.Item.Cells[4].Text = (answeringMachine / dials).ToString("P2", _numberFormatInfo);   // "AnsweringMachine";
                        e.Item.Cells[5].Text = (noAnswer / dials).ToString("P2", _numberFormatInfo);           // "NoAnswer";
                        e.Item.Cells[6].Text = (busy / dials).ToString("P2", _numberFormatInfo);               // "Busy";
                        e.Item.Cells[7].Text = (optIn / dials).ToString("P2", _numberFormatInfo);              // "OpInt";
                        e.Item.Cells[8].Text = (drops / dials).ToString("P2", _numberFormatInfo);              // "Drops";
                        e.Item.Cells[9].Text = (failed / dials).ToString("P2", _numberFormatInfo);            // "Failed";
                    }
                    else
                    {
                        e.Item.Cells[3].Text =
                        e.Item.Cells[4].Text =
                        e.Item.Cells[5].Text =
                        e.Item.Cells[6].Text =
                        e.Item.Cells[7].Text =
                        e.Item.Cells[8].Text =
                        e.Item.Cells[9].Text = (0.00).ToString("P2", _numberFormatInfo);
                    }
                }
            }
        }

        protected void grdAllQueries_DataBind(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                LinkButton lbtnActivate = (LinkButton)e.Item.FindControl("lbtnActivate");
                LinkButton lbtnDelete = (LinkButton)e.Item.FindControl("lbtnDelete");

                if (Isrunning)
                {
                    lbtnActivate.Attributes.Add("onClick", "alert('You cannot move query to active when campaign is running');return false;");
                    lbtnDelete.Attributes.Add("onClick", "alert('You cannot delete query when campaign is running');return false;");
                }
                else
                {
                    lbtnActivate.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to make this query Active?');");
                    lbtnDelete.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to delete this query?');");
                }

                if (bShowInPerc)
                {
                    DataRowView item = (DataRowView)e.Item.DataItem;

                    double dials = Double.Parse(item["Dials"].ToString());
                    if (dials > 0)
                    {
                        int talks = Int32.Parse(item["Talks"].ToString());
                        int answeringMachine = Int32.Parse(item["AnsweringMachine"].ToString());
                        int noAnswer = Int32.Parse(item["NoAnswer"].ToString());
                        int busy = Int32.Parse(item["Busy"].ToString());
                        int optIn = Int32.Parse(item["OpInt"].ToString());
                        int drops = Int32.Parse(item["Drops"].ToString());
                        int failed = Int32.Parse(item["Failed"].ToString());

                        e.Item.Cells[3].Text = (talks / dials).ToString("P2", _numberFormatInfo);              // "Talks";
                        e.Item.Cells[4].Text = (answeringMachine / dials).ToString("P2", _numberFormatInfo);   // "AnsweringMachine";
                        e.Item.Cells[5].Text = (noAnswer / dials).ToString("P2", _numberFormatInfo);           // "NoAnswer";
                        e.Item.Cells[6].Text = (busy / dials).ToString("P2", _numberFormatInfo);               // "Busy";
                        e.Item.Cells[7].Text = (optIn / dials).ToString("P2", _numberFormatInfo);              // "OpInt";
                        e.Item.Cells[8].Text = (drops / dials).ToString("P2", _numberFormatInfo);              // "Drops";
                        e.Item.Cells[9].Text = (failed / dials).ToString("P2", _numberFormatInfo);            // "Failed";
                    }
                    else
                    {
                        e.Item.Cells[3].Text =
                        e.Item.Cells[4].Text =
                        e.Item.Cells[5].Text =
                        e.Item.Cells[6].Text =
                        e.Item.Cells[7].Text =
                        e.Item.Cells[8].Text =
                        e.Item.Cells[9].Text = (0.00).ToString("P2", _numberFormatInfo);
                    }
                }
            }
        }


        //protected void grdQueries_EditCommand(object source, DataGridCommandEventArgs e)
        //{
        //    try
        //    {
        //        grdAllQueries.EditItemIndex = e.Item.ItemIndex;
        //        if (Session["Campaign"] != null)
        //        {
        //            Campaign objCampaign = (Campaign)Session["Campaign"];
        //            BindQueries(objCampaign, false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        PageMessage = "Error invoking edit " + ex.Message;
        //    }
        //}

        //protected void grdQueries_CancelEdit(object source, DataGridCommandEventArgs e)
        //{
        //    try
        //    {
        //        grdAllQueries.EditItemIndex = -1;
        //        if (Session["Campaign"] != null)
        //        {
        //            Campaign objCampaign = (Campaign)Session["Campaign"];
        //            BindQueries(objCampaign, false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        PageMessage = "Error cancelling edit " + ex.Message;
        //    }
        //}

        //protected void grdQueries_UpdateRecord(object source, DataGridCommandEventArgs e)
        //{
        //    try
        //    {
        //        if (Session["Campaign"] != null)
        //        {
        //            Campaign objCampaign = (Campaign)Session["Campaign"];
        //            string sqlStmt = "";
        //            // Update all equal or lower priority queries and reduce by one to prevent duplicate priorities
        //            string sqlStmt3 = string.Format("SELECT Priority, QueryID FROM CampaignQueryStatus WHERE Priority = {0}", ((TextBox)e.Item.Cells[2].Controls[0]).Text);
        //            dsPriority.ConnectionString = objCampaign.CampaignDBConnString;
        //            dsPriority.SelectCommand = sqlStmt3;
        //            DataView dv = (DataView)dsPriority.Select(DataSourceSelectArguments.Empty);

        //            if (dv.Table.Rows.Count > 0)
        //            {
        //                string sqlStmt2 = string.Format("SELECT Priority, QueryID FROM CampaignQueryStatus WHERE Priority >= {0}", ((TextBox)e.Item.Cells[2].Controls[0]).Text);
        //                dsPriority.ConnectionString = objCampaign.CampaignDBConnString;
        //                dsPriority.SelectCommand = sqlStmt2;
        //                dv = (DataView)dsPriority.Select(DataSourceSelectArguments.Empty);

        //                foreach (DataRow dr in dv.Table.Rows)
        //                {
        //                    sqlStmt = string.Format("UPDATE CampaignQueryStatus SET Priority = {0} WHERE QueryID = {1}", (Convert.ToInt16(dr["Priority"]) + 1), dr["QueryID"]);
        //                    dsStandbyQueries.ConnectionString = objCampaign.CampaignDBConnString;
        //                    dsStandbyQueries.SelectCommand = sqlStmt;
        //                    dsStandbyQueries.Select(DataSourceSelectArguments.Empty);
        //                }
        //            }


        //            sqlStmt = string.Format("UPDATE CampaignQueryStatus SET Priority = {0} WHERE QueryID = {1}", ((TextBox)e.Item.Cells[2].Controls[0]).Text, ((LinkButton)e.Item.Cells[12].Controls[5]).CommandArgument);
        //            dsStandbyQueries.ConnectionString = objCampaign.CampaignDBConnString;
        //            dsStandbyQueries.SelectCommand = sqlStmt;
        //            dsStandbyQueries.Select(DataSourceSelectArguments.Empty);


        //            grdAllQueries.EditItemIndex = -1;
        //            BindQueries(objCampaign, false);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        PageMessage = "Error updating record " + ex.Message;
        //    }
        //}

        //protected void grdQueries_DeleteRecord(object source, DataGridCommandEventArgs e)
        //{
        //    // *** To Be Implemented
        //}

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
        /*
         * D. Pollastrini
         * 2012-04-19 11:08
         * 
         * Remove grdStandbyQueries_rowDataBound.
         * 
         * GridView control was replaced with DataGrid control by previous developer.  However, RowDataBound was not 
         * rewired and is not applicable to DataGrid.  Moved functionality to appropriate event handler.
         * 
         */
        //protected void grdStandbyQueries_rowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        LinkButton lbtnActivate = (LinkButton)e.Row.FindControl("lbtnActivate");
        //        LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");

        //        if (Isrunning)
        //        {
        //            lbtnActivate.Attributes.Add("onClick", "alert('You cannot move query from standby to active when campaign is running');return false;");
        //            lbtnDelete.Attributes.Add("onClick", "alert('You cannot delete query when campaign is running');return false;");

        //        }
        //        else
        //        {
        //            lbtnActivate.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to make this query Active?');");
        //            lbtnDelete.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to delete this query?');");
        //        }

        //        if (bShowInPerc)
        //        {
        //            if (e.Row.Cells[3].Text != "0")
        //            {
        //                e.Row.Cells[4].Text = GetPerc(e.Row.Cells[4].Text, e.Row.Cells[3].Text);
        //                e.Row.Cells[5].Text = GetPerc(e.Row.Cells[5].Text, e.Row.Cells[3].Text);
        //                e.Row.Cells[6].Text = GetPerc(e.Row.Cells[6].Text, e.Row.Cells[3].Text);
        //                e.Row.Cells[7].Text = GetPerc(e.Row.Cells[7].Text, e.Row.Cells[3].Text);
        //                e.Row.Cells[8].Text = GetPerc(e.Row.Cells[8].Text, e.Row.Cells[3].Text);
        //                e.Row.Cells[9].Text = GetPerc(e.Row.Cells[9].Text, e.Row.Cells[3].Text);
        //            }
        //            else
        //            {
        //                e.Row.Cells[4].Text += "%";
        //                e.Row.Cells[5].Text += "%";
        //                e.Row.Cells[6].Text += "%";
        //                e.Row.Cells[7].Text += "%";
        //                e.Row.Cells[8].Text += "%";
        //                e.Row.Cells[9].Text += "%";
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// On row data binding
        /// </summary>
        /// 
        /*
         * D. Pollastrini
         * 2012-04-19 11:08
         * 
         * Remove grdActiveQueries_rowdatabound.
         * 
         * GridView control was replaced with DataGrid control by previous developer.  However, RowDataBound was not 
         * rewired and is not applicable to DataGrid.  Moved functionality to appropriate event handler.
         * 
         */
        //protected void grdActiveQueries_rowdatabound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {
        //        LinkButton lbtnDelete = (LinkButton)e.Row.FindControl("lbtnDelete");
        //        LinkButton lbtnStandby = (LinkButton)e.Row.FindControl("lbtnStandby");
        //        if (Isrunning)
        //        {
        //            lbtnStandby.Attributes.Add("onClick", "alert('You cannot move query from active to standby when campaign is running');return false;");
        //            lbtnDelete.Attributes.Add("onClick", "alert('You cannot delete query when campaign is running');return false;");
        //        }
        //        else
        //        {
        //            lbtnStandby.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to make this query stand-by?');");
        //            lbtnDelete.Attributes.Add("onClick", "javascript:return confirm('Are you sure you want to delete this query?');");
        //        }

        //        if (bShowInPerc)
        //        {
        //            if (e.Row.Cells[3].Text != "0")
        //            {
        //                e.Row.Cells[4].Text = GetPerc(e.Row.Cells[4].Text, e.Row.Cells[3].Text);
        //                e.Row.Cells[5].Text = GetPerc(e.Row.Cells[5].Text, e.Row.Cells[3].Text);
        //                e.Row.Cells[6].Text = GetPerc(e.Row.Cells[6].Text, e.Row.Cells[3].Text);
        //                e.Row.Cells[7].Text = GetPerc(e.Row.Cells[7].Text, e.Row.Cells[3].Text);
        //                e.Row.Cells[8].Text = GetPerc(e.Row.Cells[8].Text, e.Row.Cells[3].Text);
        //                e.Row.Cells[9].Text = GetPerc(e.Row.Cells[9].Text, e.Row.Cells[3].Text);
        //                e.Row.Cells[10].Text = GetPerc(e.Row.Cells[10].Text, e.Row.Cells[3].Text);
        //            }
        //            else
        //            {
        //                e.Row.Cells[4].Text += "%";
        //                e.Row.Cells[5].Text += "%";
        //                e.Row.Cells[6].Text += "%";
        //                e.Row.Cells[7].Text += "%";
        //                e.Row.Cells[8].Text += "%";
        //                e.Row.Cells[9].Text += "%";
        //                e.Row.Cells[10].Text += "%";
        //            }
        //        }
        //    }

        //}

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
                    Timer1.Enabled = false;
                    ShowCampaignDetails(true);

                    //if (objCampaign.StatusID != (long)CampaignStatus.Idle) &&&
                    //    Timer1.Enabled = true;
                }
                Timer1.Enabled = true;
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion

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
            Response.Redirect(Request.RawUrl);
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

        #region Private Methods

        /// <summary>
        /// Gets Current Campaign Details and presents the data
        /// </summary>
        private void ShowCampaignDetails(bool isStatusChanged)
        {
            Campaign objCampaign = null;
            long CampaignID = 0;
            try
            {
                if (Request.QueryString["CampaignID"] != null || isStatusChanged)
                {
                    try
                    {
                        if (!isStatusChanged)
                            CampaignID = Convert.ToInt64(Request.QueryString["CampaignID"]);
                        else
                            CampaignID = ((Campaign)Session["Campaign"]).CampaignID;
                    }
                    catch { }
                    if (CampaignID > 0)
                    {
                        CampaignService objCampService = new CampaignService();
                        objCampaign = (Campaign)Serialize.DeserializeObject(objCampService.GetCampaignByCampaignID(CampaignID), "Campaign");
                        Session["Campaign"] = objCampaign;
                    }
                }
                else
                {
                    objCampaign = (Campaign)Session["Campaign"];
                }

                try
                {
                    SetQueryStatsInPerc(objCampaign, false);
                    BindQueryStats(objCampaign);
                }
                catch { }

                ShowCampaignData(objCampaign);
                ShowCampaignScoreBoardData(objCampaign);
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Presents Campaign data
        /// </summary>
        /// <param name="objCampaign"></param>
        private void ShowCampaignData(Campaign objCampaign)
        {
            if (objCampaign != null)
            {
                lbtnIdle.Text = "Idle";
                lbtnRun.Text = "Run";
                lbtnPause.Text = "Pause";

                lbtnIdle.CssClass = "button red small";
                lbtnRun.CssClass = "button red small";
                lbtnPause.CssClass = "button red small";

                bool isRun = false;
                if (objCampaign.StatusID == (long)CampaignStatus.Idle)
                {
                    lbtnIdle.Text = "[Idle]";
                    lbtnIdle.CssClass = "button green small";
                }
                if (objCampaign.StatusID == (long)CampaignStatus.Run)
                {
                    lbtnRun.Text = "[Run]";
                    lbtnRun.CssClass = "button green small";
                    isRun = true;
                }
                if (objCampaign.StatusID == (long)CampaignStatus.Pause)
                {
                    lbtnPause.Text = "[Pause]";
                    lbtnPause.CssClass = "button green small";
                }
                lblCampaign.Text = objCampaign.Description; // Replaced Short description
                lblDateCreated.Text = objCampaign.DateCreated.ToString("MM/dd/yyyy");
                chkFCQI.Checked = objCampaign.FlushCallQueueOnIdle;

                try
                {
                    if (isRun)
                    {
                        lblTimeStarted.Text = objCampaign.StartTime != DateTime.MinValue ?
                            objCampaign.StartTime.ToString("MM/dd/yyyy HH:mm:ss") : "";
                    }
                    else
                    {
                        lblTimeStarted.Text = objCampaign.StartTime != DateTime.MinValue ?
                            objCampaign.StartTime.ToString("MM/dd/yyyy HH:mm:ss") + " - " +
                            objCampaign.StopTime.ToString("MM/dd/yyyy HH:mm:ss") : "";
                    }
                }
                catch { }

            }
        }

        /// <summary>
        /// Show Campaign ScoreBoard Data
        /// </summary>
        private void ShowCampaignScoreBoardData(Campaign objCampaign)
        {
            DataSet dsCampaignScoreBoard;
            try
            {
                if (objCampaign != null)
                {
                    CampaignService objCampService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    dsCampaignScoreBoard = objCampService.GetCampaignScoreBoardData(xDocCampaign);
                    DialingParameter objDialingParam = new DialingParameter();
                    objDialingParam = (DialingParameter)Serialize.DeserializeObject(objCampService.GetDialingParameter(xDocCampaign), "DialingParameter");
                    if (dsCampaignScoreBoard.Tables[0] != null)
                    {
                        if (dsCampaignScoreBoard.Tables[0].Rows.Count > 0)
                        {
                            DataRow drCampaignScoreBoard = dsCampaignScoreBoard.Tables[0].Rows[0];

                            string strResultCodesCount = drCampaignScoreBoard["ResultCodes"].ToString();
                            anchResultCodes.InnerText = strResultCodesCount != "0" ? strResultCodesCount : "N/A";

                            string strDialingParametersCount = drCampaignScoreBoard["DialingParameters"].ToString();
                            anchDialParams.InnerText = strDialingParametersCount != "0" ? strDialingParametersCount : "N/A";

                            string strOtherParametersCount = drCampaignScoreBoard["OtherParameters"].ToString();
                            anchOtherParams.InnerText = strOtherParametersCount != "0" ? strOtherParametersCount : "N/A";

                            string strDigitalizedRecordingCount = drCampaignScoreBoard["DigitalizedRecording"].ToString();
                            anchRecordings.InnerText = strDigitalizedRecordingCount != "0" ? strDigitalizedRecordingCount : "N/A";

                            string strAgentStatsCount = drCampaignScoreBoard["AgentStats"].ToString();
                            anchAgents.InnerText = strAgentStatsCount != "0" ? strAgentStatsCount : "N/A";

                            string strActiveQueries = drCampaignScoreBoard["ActiveQueries"].ToString();
                            string strStandByQueries = drCampaignScoreBoard["StandByQueries"].ToString();

                            //if (strActiveQueries != "0" || strStandByQueries != "0") &&& COmmented out for merge with query status page 1.5.2
                            //    anchqueryStatus.InnerText = Convert.ToString(Convert.ToInt32(strActiveQueries) + Convert.ToInt32(strStandByQueries));
                            //else
                            //    anchqueryStatus.InnerHtml = "N/A";

                            string strScriptsCount = drCampaignScoreBoard["Scripts"].ToString();
                            anchScriptList.InnerText = strScriptsCount != "0" ? strScriptsCount : "N/A";
                            if (objCampaign.EnableAgentTraining)
                            {
                                trTraining.Visible = true;
                                string strTrainingSchemeCount = drCampaignScoreBoard["TrainingSchemeCount"].ToString();
                                anchTrainingList.InnerText = strTrainingSchemeCount != "0" ? strTrainingSchemeCount : "N/A";
                            }
                            else
                            {
                                trTraining.Visible = false;
                            }

                            lblQueryListCount.Text = strActiveQueries != "0" ? strActiveQueries : "N/A";

                            bool IsDialerRunning = Convert.ToBoolean(drCampaignScoreBoard["IsDialerRunning"]);
                            bool IsScriptAssigned = Convert.ToBoolean(drCampaignScoreBoard["IsScriptAssigned"]);

                            System.Text.StringBuilder sbAlert = new System.Text.StringBuilder();

                            int iDialerQueueRecordCount = 0;
                            try
                            {
                                if (IsDialerRunning && objCampaign.StatusID == (long)CampaignStatus.Run)
                                {
                                    if (drCampaignScoreBoard["DialerRecInQueue"] != DBNull.Value)
                                    {
                                        iDialerQueueRecordCount = Convert.ToInt32(drCampaignScoreBoard["DialerRecInQueue"]);
                                    }
                                }
                            }
                            catch { }
                            lblDialerQueue.Text = iDialerQueueRecordCount.ToString();

                            if (!IsDialerRunning)
                            {
                                sbAlert.Append(@"Please start the dialer engine.\n");
                            }

                            if (strScriptsCount == "0" && objDialingParam.DialingMode != (int)DialingMode.Unmanned)
                            {
                                sbAlert.Append(@"Please add a script before running campaign and define dialing parameters.\n");
                            }

                            if (!IsScriptAssigned && objDialingParam.DialingMode != (int)DialingMode.Unmanned)
                            {
                                sbAlert.Append(@"Assign the script to the call scripts in dialing parameters.\n");
                            }

                            if (strActiveQueries == "0")
                            {
                                sbAlert.Append(@"Please add queries before running campaign.\n");
                            }

                            string strAlert = sbAlert.ToString();
                            if (strAlert != string.Empty)
                            {
                                lbtnRun.Attributes["onclick"] = "javascript:alert('" + strAlert + "');return false;";
                            }
                            else
                            {
                                lbtnRun.Attributes.Remove("onclick");
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Updates campaign status
        /// </summary>
        /// <param name="status"></param>
        private void UpdateCampaignStatus(CampaignStatus status)
        {
            if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                CampaignService objCampService = new CampaignService();

                if (status == CampaignStatus.Run)
                {
                    int recordCount = objCampService.GetCampaignActiveDialCount(objCampaign.CampaignDBConnString);
                    if (recordCount <= 0)
                    {
                        PageMessage = "Please select import the numbers you’d wish to dial.";
                        ShowCampaignData(objCampaign);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
                        return;
                    }

                    int PhoneLinesAvailable = 24;
                    try
                    {
                        PhoneLinesAvailable = Convert.ToInt32(ConfigurationManager.AppSettings["PhoneLinesAvailable"]);
                    }
                    catch { }

                    int PhoneLinesUsed = 0;
                    try
                    {
                        PhoneLinesUsed = objCampService.GetPhoneLinesInUseCount(objCampaign.CampaignID);
                    }
                    catch { }
                    if (PhoneLinesUsed > PhoneLinesAvailable)
                    {
                        int AlreadyUsed = objCampService.GetPhoneLinesInUseCount(0);
                        PageMessage = string.Format(@"The campaign cannot be activated.\nThe Line count({0}) for the campaign cannot exceed total lines({1}) available to system.",
                            PhoneLinesUsed - AlreadyUsed, PhoneLinesAvailable - AlreadyUsed);
                        ShowCampaignData(objCampaign);
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
                        return;
                    }
                }

                objCampaign.StatusID = (long)status;
                objCampaign.FlushCallQueueOnIdle = chkFCQI.Checked;

                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                objCampaign = (Campaign)Serialize.DeserializeObject(
                    objCampService.CampaignStatusUpdate(xDocCampaign), "Campaign");
                Session["Campaign"] = objCampaign;
                ShowCampaignData(objCampaign);
            }
        }

        /// <summary>
        /// Bind Queries.
        /// </summary>
        private void BindQueryStats(Campaign objCampaign)
        {
            if (objCampaign != null)
            {
                DataSet dsQuerList;
                try
                {
                    CampaignService objCampService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();

                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    dsQuerList = objCampService.GetCampaignQueryStatus(xDocCampaign);
                    DataView dvQueries = FilterData(dsQuerList.Tables[0], "IsActive = 1");
                    grdActiveQueries.DataSource = dvQueries;
                    grdActiveQueries.DataBind();

                    try
                    {
                        DataView dvFinishedQueries = FilterData(dsQuerList.Tables[0], "IsActive = 0 AND ShowMessage = 1");

                        if (dvFinishedQueries.Count > 0)
                        {
                            DataRowView dr = dvFinishedQueries[0];
                            long campaignQueryStatusId = Convert.ToInt64(dr["CampaignQueryID"]);
                            string queryName = dr["QueryName"].ToString();
                            UpdateQueryStatus(campaignQueryStatusId, false);

                            PageMessage = string.Format("Query \"{0}\", ID {1} has finished execution.", queryName, campaignQueryStatusId);
                            ActivityLogger.WriteAdminEntry(objCampaign, "Query '{0}', ID {1} complete, moving to standby.", queryName, campaignQueryStatusId);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "QueryAlert", "alert('" + PageMessage + "');", true);
                        }

                        DataView dvExhaustedQueries = FilterData(dsQuerList.Tables[0], "IsActive = 1 AND Available = 0");

                        if (dvExhaustedQueries.Count > 0)
                        {
                            DataRowView dr = dvExhaustedQueries[0];
                            long campaignQueryStatusId = Convert.ToInt64(dr["CampaignQueryID"]);
                            string queryName = dr["QueryName"].ToString();
                            UpdateQueryStatus(campaignQueryStatusId, false, false, false);

                            PageMessage = string.Format("\"{0}\" has no available numbers, moving to standby.", queryName, campaignQueryStatusId);
                            ActivityLogger.WriteAdminEntry(objCampaign, "Query '{0}', ID {1} has no available numbers, moving to standby.", queryName, campaignQueryStatusId);
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "QueryAlert", "alert('" + PageMessage + "');", true);
                        }

                    }
                    catch (Exception ex)
                    {
                        PageMessage = ex.Message;
                    }
                }

                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
            }
        }

        private void UpdateQueryStatus(long cqStatusID, bool isActive)
        {
            XmlDocument xDocCampaign = new XmlDocument();
            Campaign objCampaign = (Campaign)Session["Campaign"];
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            CampaignService objCampService = new CampaignService();
            try
            {
                objCampService.CampaignQueryStatusUpdate(xDocCampaign, cqStatusID, isActive, false, false, false);
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Query stats in percentage or not
        /// </summary>
        /// <param name="objCampaign"></param>
        private void SetQueryStatsInPerc(Campaign objCampaign, bool isUpdate)
        {
            if (objCampaign != null)
            {
                CampaignService objCampaignService = new CampaignService();
                OtherParameter objOtherParameter = new OtherParameter();
                XmlDocument xDocCampaign = new XmlDocument();

                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                objOtherParameter = (OtherParameter)Serialize.DeserializeObject(
                    objCampaignService.GetOtherParameter(xDocCampaign), "OtherParameter");

                if (isUpdate)
                {
                    // Don changed this, was commented out.
                    objOtherParameter.QueryStatisticsInPercent = chkQSP.Checked;

                    XmlDocument xDocOtherParameter = new XmlDocument();
                    xDocOtherParameter.LoadXml(Serialize.SerializeObject(objOtherParameter, "OtherParameter"));

                    objOtherParameter = (OtherParameter)Serialize.DeserializeObject(
                        objCampaignService.OtherParameterInsertUpdate(xDocCampaign, xDocOtherParameter), "OtherParameter");
                }

                if (objOtherParameter.OtherParameterID != 0)
                {
                    // Don:  changed this was commented.
                    chkQSP.Checked = objOtherParameter.QueryStatisticsInPercent;
                }
            }
        }

        /// <summary>
        /// Clear sessions
        /// </summary>
        private void ClearSessions()
        {
            try
            {
                Session.Remove("importparams");
                Session.Remove("ImportStats");
            }
            catch { }
        }

        /*
         * D. Pollastrini
         * 2012-04-19
         * 
         * Remove GetPerc.
         * 
         * This function is being handled locally by functions more efficiently.
         */
        //private string GetPerc(string a, string b)
        //{
        //    string result = "0%";
        //    try
        //    {
        //        if (a != "" && b != "")
        //        {
        //            if (b != "0")
        //            {
        //                result = Convert.ToString(Math.Round((Convert.ToDouble(a) / Convert.ToDouble(b)) * 100, 2)) + "%";
        //            }
        //        }
        //    }
        //    catch { }
        //    return result;
        //}

        #endregion

        #region Query Management Methods

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
        private void UpdateQueryStatus(Int64 cqStatusID, bool isActive, bool isStandby, bool resetStats)
        {
            XmlDocument xDocCampaign = new XmlDocument();
            Campaign objCampaign = (Campaign)Session["Campaign"];
            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            CampaignService objCampService = new CampaignService();
            try
            {
                //ErrorLogger.Write("Updating query status.");
                objCampService.CampaignQueryStatusUpdate(xDocCampaign, cqStatusID, isActive, isStandby, false, resetStats);
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


        #endregion

        protected void chkQSP_CheckedChanged(object sender, EventArgs e)
        {
            Campaign objCampaign = (Campaign)Session["Campaign"];
            SetQueryStatsInPerc(objCampaign, true);
            BindQueries(objCampaign, false);
        }



    }
}
