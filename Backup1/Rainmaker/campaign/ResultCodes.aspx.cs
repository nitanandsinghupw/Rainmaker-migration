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
    public partial class ResultCodes : PageBase
    {
        public bool Isrunning = false;
        private const int SYSTEM_RESULTCODE_COUNT = 7;
        private string[] resultCodes = { "Answering Machine", "Busy" , "Operator Intercept",
                "Dropped","No Answer","Scheduled Callback","Never Call","Transferred to Agent",
                "Transferred to Dialer","Transferred to Verification","Unmanned Live Contact",
                "Inbound Abandoned by Agent","Inbound abandoned by Caller","Error","Failed",
                "Cadence Break","Loop Current Drop","Pbx Detected","No Ringback","Analysis Stopped",
                "No DialTone","FaxTone Detected", "Unmanned Transferred to Answering Machine", "Transferred Offsite" };
        private string[] hideSysResultCodes = { 
                "Scheduled Callback","Transferred to Agent",
                "Transferred to Dialer","Transferred to Verification","Unmanned Live Contact",
                "Inbound Abandoned by Agent","Inbound abandoned by Caller","Error","Failed",
                "Cadence Break","Loop Current Drop","Pbx Detected","No Ringback","Analysis Stopped",
                "No DialTone","FaxTone Detected", "Unmanned Transferred to Answering Machine", "Transferred Offsite" };

        #region Events

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                resultCodes = ConfigurationManager.AppSettings["SysResultCodes"].Split(',');
                hideSysResultCodes = ConfigurationManager.AppSettings["SysResultCodesToHide"].Split(',');
            }
            catch { }
            if (!Page.IsPostBack)
            {
                BindResultCodesLocal();
                if (Session["Campaign"] != null)
                {
                    Campaign objCampaign = (Campaign)Session["Campaign"];
                    lblCampaign.Text = objCampaign.Description; // Replaced Short description
                    // Removed 09.08.10 by GW ***
                    //chkDialThroughAllNum.Checked = objCampaign.DialAllNumbers;
                }
            }
        }

        /// <summary>
        /// Navigates to result code detail page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnStatus_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            Response.Redirect("ResultCodeDetail.aspx?ResultCodeID=" + lbtnSender.CommandArgument + "&RCType=" + lbtnSender.CommandName);
        }

        /// <summary>
        /// Shows the deleted result codes in Grid View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void chkShowDeletedCallRC_CheckedChanged(object sender, EventArgs e)
        {
            BindResultCodesLocal();
        }

        /// <summary>
        /// This event to disable deleted rows
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdResultCodes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsCampaignRunning())
            {
                Isrunning = true;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                HiddenField hdn = (HiddenField)e.Row.FindControl("hdnDeleted");
                if (hdn.Value != null && hdn.Value != "")
                {
                    //e.Row.CssClass = "tableHdr";
                    e.Row.Enabled = false;
                }
                else
                {
                    int lastCellIndex = e.Row.Cells.Count - 1;
                    IButtonControl btn = (IButtonControl)e.Row.Cells[lastCellIndex].Controls[0];
                    LinkButton lbtn = (LinkButton)e.Row.FindControl("lbtnStatus");
                    bool isSysResultcode = IsSysResultCode(lbtn.Text, resultCodes);
                    bool hideResultCode = true;

                    //if (Convert.ToInt64(grdResultCodes.DataKeys[e.Row.RowIndex].Value) <= SYSTEM_RESULTCODE_COUNT)
                    if (isSysResultcode)
                    {
                        // 01/16/2010 : showing 'Scheduled Callback' result code 
                        if (lbtn.Text.ToLower() != "scheduled callback")
                        {
                            hideResultCode = ShowSysResultCode(lbtn.Text, hideSysResultCodes);
                        }

                        e.Row.Visible = hideResultCode;
                        // change background color of system result codes
                        e.Row.CssClass = "tableRowSys";
                        ((WebControl)btn).Enabled = false;
                        lbtn.CommandName = "SRC";
                        //lbtn.Enabled = false;
                    }
                    else
                    {
                        lbtn.CommandName = "RC";
                        if (!Isrunning)
                            ((WebControl)btn).Attributes.Add("onClick", "return confirm('Are you sure you want to delete this resultcode?');");
                        else
                            ((WebControl)btn).Attributes.Add("onClick", "alert('You cannot delete result code when campaign is running');return false;");
                    }
                }
            }
        }

        /// <summary>
        /// Deletes a row in grid view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdResultCodes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            System.Web.UI.WebControls.DataKey dk = ((System.Web.UI.WebControls.DataKey)grdResultCodes.DataKeys[e.RowIndex]);
            long ResultCodeID = Convert.ToInt64(dk.Value);

            ResultCode objResultCode = new ResultCode();
            Campaign objCampaign = new Campaign();
            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];
            }
            try
            {
                objResultCode.ResultCodeID = ResultCodeID;
                LinkButton lbtn = (LinkButton)grdResultCodes.Rows[e.RowIndex].Cells[1].FindControl("lbtnStatus");
                objResultCode.Description = lbtn.Text;
                objResultCode.RecycleInDays = Convert.ToInt32(grdResultCodes.Rows[e.RowIndex].Cells[2].Text);
                objResultCode.Presentation = Convert.ToBoolean(grdResultCodes.Rows[e.RowIndex].Cells[3].Text);
                objResultCode.Redialable = Convert.ToBoolean(grdResultCodes.Rows[e.RowIndex].Cells[4].Text);
                objResultCode.Lead = Convert.ToBoolean(grdResultCodes.Rows[e.RowIndex].Cells[5].Text);
                objResultCode.MasterDNC = Convert.ToBoolean(grdResultCodes.Rows[e.RowIndex].Cells[6].Text);
                objResultCode.NeverCall = Convert.ToBoolean(grdResultCodes.Rows[e.RowIndex].Cells[7].Text);
                objResultCode.VerifyOnly = Convert.ToBoolean(grdResultCodes.Rows[e.RowIndex].Cells[8].Text);
                objResultCode.DialThroughAll = false;
                objResultCode.ShowDeletedResultCodes = false;
                objResultCode.DateDeleted = DateTime.Now.Date;
                CampaignService objCampaignService = new CampaignService();
                XmlDocument xDocResultCode = new XmlDocument();
                XmlDocument xDocCampaign = new XmlDocument();
            
                xDocResultCode.LoadXml(Serialize.SerializeObject(objResultCode, "ResultCode"));
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                objCampaignService.ResultCodeInsertUpdate(xDocCampaign, xDocResultCode);
                BindResultCodesLocal();
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// changing DialThroughAll status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void chkDialThroughAllNum_CheckedChanged(object sender, EventArgs e)
        //{
        //    if (Session["Campaign"] != null)
        //    {
        //        Campaign objCampaign = (Campaign)Session["Campaign"];
        //        objCampaign.DialAllNumbers = chkDialThroughAllNum.Checked;

        //        XmlDocument xDocCampaign = new XmlDocument();
        //        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

        //        CampaignService objCampService = new CampaignService();
        //        objCampaign = (Campaign)Serialize.DeserializeObject(
        //            objCampService.CampaignDialStatusUpdate(xDocCampaign), "Campaign");

        //        lblCampaign.Text = objCampaign.ShortDescription;

        //        Session["Campaign"] = objCampaign;
        //        BindResultCodesLocal();

        //    }
        //}

        #endregion

        #region Private Methods

        /// <summary>
        /// Binds result codes to grid view
        /// </summary>
        private void BindResultCodesLocal()
        {
            DataSet dsResultCodes;
            try
            {
                CampaignService objCampService = new CampaignService();
                Campaign objCampaign = (Campaign)Session["Campaign"];
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));

                dsResultCodes = objCampService.GetResultCodes(xDocCampaign);
                if (!chkShowDeletedCallRC.Checked)
                {
                    grdResultCodes.DataSource = FilterDataLocal(dsResultCodes.Tables[0], "DateDeleted is null");
                    grdResultCodes.DataBind();
                }
                else
                {
                    grdResultCodes.DataSource = dsResultCodes;
                    grdResultCodes.DataBind();
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion

        #region Function to Filter DataTable
        /// <summary>
        /// Flter Data 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="strCondition"></param>
        /// <returns></returns>
        private DataView FilterDataLocal(DataTable dt, string strCondition)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = strCondition;
                return dv;
            }
            return null;
        }
        #endregion


    }
}
