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
    public partial class CampaignFieldDetails : PageBase
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
                Campaign objCampaign;
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                    anchHome.InnerText = objCampaign.Description; // Replaced Short description
                    BindFieldType();
                }
            }

        }

        /// <summary>
        /// It adds field to campaign table
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            if (!IsCampaignRunning())
                SaveData();
            else
                PageMessage = "You cannot add campaign field when campaign is running.";
        }

        /// <summary>
        ///Cancels the operation and Naviagtes to the same page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/campaign/CampaignFieldDetails.aspx");
        }

        protected void ddlfieldtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (ddlfieldtype.SelectedValue == "1")
            //{
            //    pnllength.Visible = true;
            //}
            //else
            //{
            //    pnllength.Visible = false;
            //}
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Binds FieldType to dropdown
        /// </summary>
        protected void BindFieldType()
        {
            DataSet dsType;
            try
            {
                CampaignService objCampService = new CampaignService();
                dsType = objCampService.GetFieldTypes();
                ddlfieldtype.DataSource = dsType;
                ddlfieldtype.DataTextField = "FieldType";
                ddlfieldtype.DataValueField = "FieldTypeID";
                ddlfieldtype.DataBind();
                ddlfieldtype.Items.Insert(0, (new ListItem("Select Field Type")));
                ViewState["FieldType"] = dsType;

            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Saves CampaignFields
        /// </summary>
        private void SaveData()
        {
            Campaign objCampaign;
            DataSet dsFieldType;
            if (Session["Campaign"] != null)
            {
                objCampaign = (Campaign)Session["Campaign"];

                CampaignFields objCampaignFields = new CampaignFields();

                objCampaignFields.FieldName = txtFieldname.Text.Trim();
                objCampaignFields.FieldTypeID = Convert.ToInt64(ddlfieldtype.SelectedValue);
                objCampaignFields.IsDefault = false;
                if (txtLength.Text.Trim() != string.Empty)
                    objCampaignFields.DBValue = Convert.ToInt32(txtLength.Text.Trim());
                if (ddlfieldtype.SelectedItem.Text.Equals("encrypted", StringComparison.InvariantCultureIgnoreCase))
                    objCampaignFields.DBValue = 1024;
                    
                if (ViewState["FieldType"] != null)
                {
                    dsFieldType = (DataSet)ViewState["FieldType"];

                    foreach (DataRow dr in dsFieldType.Tables[0].Rows)
                    {
                        if (ddlfieldtype.SelectedValue == dr["FieldTypeID"].ToString())
                        {
                            objCampaignFields.DbFieldType = dr["DBFieldType"].ToString();
                            break;
                        }
                    }


                    CampaignService objCampaignService = new CampaignService();
                    XmlDocument xDocCampaign = new XmlDocument();
                    XmlDocument xDocCampaignFields = new XmlDocument();
                    try
                    {
                        xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                        xDocCampaignFields.LoadXml(Serialize.SerializeObject(objCampaignFields, "CampaignFields"));
                        objCampaignFields = (CampaignFields)Serialize.DeserializeObject(objCampaignService.CampaignFieldsInsertUpdate(xDocCampaign, xDocCampaignFields), "CampaignFields");
                        Response.Redirect("~/campaign/CampaignFieldsList.aspx");

                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.IndexOf("DuplicateColumnException") >= 0)
                            PageMessage = "field name already exists try again";
                        else
                            PageMessage = ex.Message;
                    }

                }
            }
        }

        #endregion
    }
}
