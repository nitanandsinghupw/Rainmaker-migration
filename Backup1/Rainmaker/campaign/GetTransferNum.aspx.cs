using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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
using System.Text;

namespace Rainmaker.Web.campaign
{
    public partial class GetTransferNum : PageBase
    {

        #region Events
        /// <summary>
        /// Page load event,
        /// Bind the result codes to list on loading page for 1st time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["OffsiteNumber"] = "";   
            }
        }

        /// <summary>
        /// Saves result code for call,
        /// update agent Stat, 
        /// and update status to Ready for waiting for call agent,
        /// update call campaign fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string offsiteNumber = "";
                bool isValidNumber = true; 

                offsiteNumber = txtTransNumber.Text.Replace("-", "");
                offsiteNumber = offsiteNumber.Replace(" ", "");
                offsiteNumber = offsiteNumber.Replace(".", "");
                offsiteNumber = offsiteNumber.Replace("(", "");
                offsiteNumber = offsiteNumber.Replace(")", "");

                if (offsiteNumber.Length == 7 || offsiteNumber.Length == 10)
                {

                    for (int i = 0; i < offsiteNumber.Length; i++)
                    {
                        int Num;
                        bool isNum = int.TryParse(offsiteNumber.Substring(i, 1), out Num);

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
                    Session["OffsiteNumber"] = "";
                    return;
                }

                Session["OffsiteNumber"] = offsiteNumber;
                Response.Write("<script> window.close();</script>");

            }
            catch (Exception ex)
            {
                PageMessage = "Exception entering offsite number : " + ex.Message;
            }
        }
        #endregion
    }
}
