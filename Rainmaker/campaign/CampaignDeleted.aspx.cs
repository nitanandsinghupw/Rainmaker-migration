using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Web.UI.WebControls;
using Rainmaker.Web.CampaignWS;
using Rainmaker.Web.DataAccess;


namespace Rainmaker.Web.campaign
{
    public partial class CampaignDeleted : System.Web.UI.Page
    {

        //-------------------------------------------------------------
        /// <summary>
        /// Page setup and load routine.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //------------------------------------------------------------- 
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        //-------------------------------------------------------------
        /// <summary>
        /// Restores a compaign which has been marked for deletion.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //-------------------------------------------------------------
        protected void GridView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iIndex = 0;
            Int64 iCampaignID = -1;
            bool bCheck = false;

            iIndex = GridView1.SelectedIndex;

            iCampaignID = Convert.ToInt16(GridView1.Rows[iIndex].Cells[1].Text);
            if (iCampaignID >= 0)
            {

                DBCampaign dbCampaign = new DBCampaign();
                bCheck = dbCampaign.bSet_IsDeleted(iCampaignID, false);
                if (bCheck == true)
                {
                    Response.Redirect("~\\Campaign\\CampaignDeleted.aspx");
                }
            }

        }

        //-------------------------------------------------------------
        /// <summary>
        /// Deletes the campaign if not running
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //-------------------------------------------------------------
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //---------------------------------------------------------
            //  We only mark the campign as deleted but the data is
            //  kept in the database.
            //---------------------------------------------------------
            LinkButton lbtnDelete = (LinkButton)sender;            
            string campaignDetails = lbtnDelete.CommandArgument;            
            CampaignService objCampService = new CampaignService();            
            string shortDescription = campaignDetails.Substring(campaignDetails.IndexOf(",") + 1);           
            Int64 iCampaignID = Convert.ToInt64(campaignDetails.Substring(0, campaignDetails.IndexOf(",")));

            if (iCampaignID >= 0)
            {
                int result = objCampService.DeleteCampaign(iCampaignID, shortDescription);
                if (result != 0)
                {
                    DBCampaign dbCampaign = new DBCampaign();
                    bool bCheck = dbCampaign.bDelete(iCampaignID);
                    if (bCheck == true)
                    {

                        string strPath = shortDescription;
                        strPath = strPath.Trim();
                        strPath = Global.strRecordings + strPath;
                        string RecordingsPath = "";

                        string ismultiboxconfig = ConfigurationManager.AppSettings["IsMultiBoxConfig"];
                        strPath = shortDescription;

                        if (ismultiboxconfig == "yes" || ismultiboxconfig == "Yes" || ismultiboxconfig == "YES")
                        {
                            RecordingsPath = ConfigurationManager.AppSettings["RecordingsPathMulti"];
                            strPath = strPath.Trim();
                            strPath = RecordingsPath + strPath;
                            
                        }
                        else
                        {
                            RecordingsPath = ConfigurationManager.AppSettings["RecordingsPath"];
                            strPath = strPath.Trim();
                            strPath = RecordingsPath + strPath;
                            
                        }

                        if (Directory.Exists(strPath) == true)
                        {
                            Directory.Delete(strPath);
                        }

                        Response.Redirect("~\\Campaign\\CampaignDeleted.aspx");
                    }
                }

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
            //
            // long CampaignID = Convert.ToInt64(sampaignDetails.Substring(0, sampaignDetails.IndexOf(",")));
            //
            // long StatusID = Convert.ToInt64(lbtnDelete.CommandName);
            // try
            // {
            //     if (StatusID != (long)CampaignStatus.Run)
            //     {
            //         int result = objCampService.DeleteCampaign(CampaignID, shortDescription);
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


    }
}
