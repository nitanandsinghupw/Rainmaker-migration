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
using Rainmaker.Web.DataAccess;


namespace Rainmaker.Web.campaign
{
    public partial class CampaignList : PageBase
    {
        #region Events

        //-------------------------------------------------------------
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //-------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                GetAllCampaignStatus();
                GetCampaignList();
                ClearSessions();
            }
        }

        //-------------------------------------------------------------
        /// <summary>
        /// Filter Campaigns based on Status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //-------------------------------------------------------------
        protected void ddlCampaignStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetCampaignList();
        }

        //-------------------------------------------------------------
        /// <summary>
        /// Navigate to the Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //-------------------------------------------------------------
        protected void lbtnCampaign_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            Response.Redirect("Home.aspx?CampaignID=" + lbtnSender.CommandArgument);
        }

        //-------------------------------------------------------------
        /// <summary>
        /// Deletes the campaign if not running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //-------------------------------------------------------------
        protected void lbtnDelete_Click(object sender, EventArgs e)
        {

            //---------------------------------------------------------
            //  We only mark the campign as deleted but the data is
            //  kept in the database.
            //---------------------------------------------------------
            LinkButton lbtnDelete = (LinkButton)sender;
            string campaignDetails = lbtnDelete.CommandArgument;
            string shortDescription = campaignDetails.Substring(campaignDetails.IndexOf(",") + 1);
            Int64 iCampaignID = Convert.ToInt64(campaignDetails.Substring(0, campaignDetails.IndexOf(",")));
            long StatusID = Convert.ToInt64(lbtnDelete.CommandName);

            if (iCampaignID >= 0 && StatusID != (long)CampaignStatus.Run)
            {
                DBCampaign dbCampaign = new DBCampaign();
                bool bCheck = dbCampaign.bSet_IsDeleted(iCampaignID, true);
                if (bCheck == true)
                {
                    Response.Redirect("~\\Campaign\\CampaignList.aspx");
                }

            }
            else
            {
                PageMessage = "The current Campaign is running please stop the Campaign to delete!";
            }


            //---------------------------------------------------------
            // Old code, the delete in the WebService drops the tables
            // which should be preserved.
            //---------------------------------------------------------
            //
            // LinkButton lbtnDelete = (LinkButton)sender;
            // CampaignService objCampService = new CampaignService();
            //
            // string sampaignDetails = lbtnDelete.CommandArgument;
            // string shortDescription = sampaignDetails.Substring(sampaignDetails.IndexOf(",") + 1);
            // long CampaignID = Convert.ToInt64(sampaignDetails.Substring(0, sampaignDetails.IndexOf(",")));
            // long StatusID = Convert.ToInt64(lbtnDelete.CommandName);
            // try
            // {
            //     if (StatusID != (long)CampaignStatus.Run)
            //     {
            //         int result = objCampService.DeleteCampaign(CampaignID,shortDescription);
            //         if (result != 0)
            //         {
            //             GetCampaignList();
            //         }
            //     }
            //     else
            //     {
            //         PageMessage = "The current Campaign is running please stop the Campaign to delete!";
            //     }
            // }
            // catch (Exception ex)
            // {
            //     PageMessage = ex.Message;
            // }
            //
            //---------------------------------------------------------
        }

        //-------------------------------------------------------------
        /// <summary>
        /// Bind Campaign Status for each row in CampaignList gridview.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //-------------------------------------------------------------
        protected void grdCampaignList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                try
                {
                    lblStatus.Text = GetStatus(Convert.ToInt64(lblStatus.Text.Trim()));
                }
                catch
                {
                    lblStatus.Text = "Idle";
                }
            }
        }

        #endregion

        #region Private Methods


        //-------------------------------------------------------------
        /// <summary>
        /// Get Campaign and filter based on list and bind to gridview,
        /// Redirect to Create Campaign if new campaigns exists
        /// </summary>
        //-------------------------------------------------------------
        private void GetCampaignList()
        {
            DataSet dsCampaignList;
            try
            {
                CampaignService objCampService = new CampaignService();
                dsCampaignList = objCampService.GetCampaignList();
                if (dsCampaignList.Tables[0].Rows.Count == 0)
                {
                    // Redirect to campaign creation
                    Response.Redirect("CreateCampaign.aspx", true);
                }

                DataView dvCampaignStatus = dsCampaignList.Tables[0].DefaultView;

                if (ddlCampaignStatus.SelectedIndex != 0)
                {
                    // Filter Campaign based on Status
                    dvCampaignStatus = FilterData(dsCampaignList.Tables[0], "StatusID=" + ddlCampaignStatus.SelectedValue);
                }

                BindCampaigns(dvCampaignStatus);
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }


        //-------------------------------------------------------------
        /// <summary>
        /// Bind Campaigns
        /// </summary>
        /// <param name="dvCampaigns"></param>
        //-------------------------------------------------------------
        private void BindCampaigns(DataView dvCampaigns)
        {
            grdCampaignList.DataSource = dvCampaigns;
            grdCampaignList.DataBind();
        }


        //-------------------------------------------------------------
        /// <summary>
        /// Clear sessions
        /// </summary>
        //------------------------------------------------------------- 
        private void ClearSessions()
        {
            try
            {
                Session.Remove("Campaign");
            }
            catch { }
        }


        //-------------------------------------------------------------
        /// <summary>
        /// Get Campaign Status
        /// </summary>
        /// <param name="statusId"></param>
        /// <returns></returns>
        //-------------------------------------------------------------
        private string GetStatus(long statusId)
        {
            return ((CampaignStatus)statusId).ToString();
        }


        //-------------------------------------------------------------
        /// <summary>
        /// Get All Campaign Status
        /// </summary>
        //-------------------------------------------------------------
        private void GetAllCampaignStatus()
        {
            ddlCampaignStatus.Items.Clear();
            foreach (int statusId in Enum.GetValues(typeof(CampaignStatus)))
            {
                ddlCampaignStatus.Items.Add(new ListItem(GetStatus(statusId), statusId.ToString()));
            }
            ddlCampaignStatus.Items.Insert(0, new ListItem("All", "0"));
        }
        #endregion

    }
}
