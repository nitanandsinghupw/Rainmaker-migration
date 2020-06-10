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
    public partial class QueryList : PageBase
    {
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
                    anchHome.InnerText = objCampaign.Description;// Replaced Short description
                    GetQuerList(objCampaign);
                }
            }
        }
       
        protected void lbtnQuery_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            // *** Response.Redirect("QueryDetail.aspx?QueryID=" + lbtnSender.CommandArgument);
            Response.Redirect("QueryDetailTree.aspx?QueryID=" + lbtnSender.CommandArgument);
        }

        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
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
            GetQuerList(objCampaign);
        }
        #endregion

        #region BindData

        private void GetQuerList(Campaign objCampaign)
        {
           
            DataSet dsQuerList;
            try
            {
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();

                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                dsQuerList = objCampService.GetQueryList(xDocCampaign);
                grdQueryList.DataSource = dsQuerList;
                grdQueryList.DataBind();

            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion
    }
}
