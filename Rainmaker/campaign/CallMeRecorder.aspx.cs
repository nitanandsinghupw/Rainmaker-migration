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
using Rainmaker.Web.AgentsWS;
using Rainmaker.Web.CampaignWS;

namespace Rainmaker.Web.campaign
{
    public partial class CallMeRecorder : PageBase
    {
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
                
            }
            if (hdnSubmitted.Value == "yes")
            {
                hdnSubmitted.Value = "no";
                Response.Redirect("~/campaign/CampaignList.aspx");
            }

        }

        /// <summary>
        /// Saves Global Dialing Params
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnOK_Click(object sender, EventArgs e)
        {
            try
            {
                string targetNumber = "";
                bool isValidNumber = true;

                targetNumber = txtPhoneNumber.Text.Replace("-", "");
                targetNumber = targetNumber.Replace(" ", "");
                targetNumber = targetNumber.Replace(".", "");
                targetNumber = targetNumber.Replace("(", "");
                targetNumber = targetNumber.Replace(")", "");

                if (targetNumber.Length == 7 || targetNumber.Length == 10)
                {

                    for (int i = 0; i < targetNumber.Length; i++)
                    {
                        int Num;
                        bool isNum = int.TryParse(targetNumber.Substring(i, 1), out Num);

                        if (!isNum)
                        {
                            isValidNumber = false;
                            break;
                        }

                    }
                }
                else
                {
                    isValidNumber = false;
                }
                if (!isValidNumber)
                {
                    Response.Write("<script>alert('Please enter a valid phone number.');</script>");
                    return;
                }

                SubmitRequest(targetNumber);
                hdnSubmitted.Value = "yes";
                //ltrlClose.Text = "<script language='javascript'> alert('Your call request has been submitted.\r\nThe dialer will call your number shortly.\r\nMake sure the dialer is running, if not your call request will expire in 15 minutes.');</script>";
                //Response.Write("<script>alert('Your call request has been submitted.\r\nThe dialer will call your number shortly.\r\nMake sure the dialer is running, if not your call request will expire in 15 minutes.');</script>");
                //Response.Write("<script>alert('Your call request has been submitted.');</script>");
                return; 
            }
            catch (Exception ex)
            {
                PageMessage = "Exception entering offsite number : " + ex.Message;
            }
        }

        /// <summary>
        /// Cancels the changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/campaign/CampaignList.aspx");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Binds the global dialing data
        /// </summary>

        /// <summary>
        /// Saves global dialing Data
        /// </summary>
        private void SubmitRequest(string targetNumber)
        {

            CampaignService objCampaignService = new CampaignService();
            //XmlDocument xDocGlobalDialing = new XmlDocument();
            try
            {
                objCampaignService.SubmitAdminRequest(1, targetNumber);
            }
            catch (Exception ex)
            {
                PageMessage = "Error saving call request: " + ex.Message;
            }
        }

        #endregion
    }
}
