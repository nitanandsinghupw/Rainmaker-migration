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
    public partial class GlobalDialingParams : PageBase
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
                ShowGlobalDialingData();
            }
        }

        /// <summary>
        /// Saves Global Dialing Params
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        /// <summary>
        /// Cancels the changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            ShowGlobalDialingData();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Binds the global dialing data
        /// </summary>
        private void ShowGlobalDialingData()
        {
            Rainmaker.Common.DomainModel.GlobalDialingParams objGlobalDialing;
            CampaignService objCampService = new CampaignService();
            try
            {
                objGlobalDialing = (Rainmaker.Common.DomainModel.GlobalDialingParams)Serialize.DeserializeObject(
                    objCampService.GetGlobalDialingParams(), "GlobalDialingParams");

                ViewState["GlobalDialingID"] = objGlobalDialing.GlobalDialingID;
                txtGlobalDialingPrefix.Text = objGlobalDialing.Prefix;
                txtGlobalDialingSuffix.Text = objGlobalDialing.Suffix;
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Saves global dialing Data
        /// </summary>
        private void SaveData()
        {
            Rainmaker.Common.DomainModel.GlobalDialingParams objGlobalDialing =
                new Rainmaker.Common.DomainModel.GlobalDialingParams();
            if (ViewState["GlobalDialingID"] != null)
                objGlobalDialing.GlobalDialingID = Convert.ToInt64(ViewState["GlobalDialingID"]);
            objGlobalDialing.Prefix = txtGlobalDialingPrefix.Text.Trim();
            objGlobalDialing.Suffix = txtGlobalDialingSuffix.Text.Trim();
            CampaignService objCampaignService = new CampaignService();
            XmlDocument xDocGlobalDialing = new XmlDocument();
            try
            {
                xDocGlobalDialing.LoadXml(Serialize.SerializeObject(objGlobalDialing, "GlobalDialingParams"));

                objGlobalDialing = (Rainmaker.Common.DomainModel.GlobalDialingParams)Serialize.DeserializeObject(
                    objCampaignService.GlobalDialingInsertUpdate(xDocGlobalDialing),
                    "GlobalDialingParams");
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion
    }
}
