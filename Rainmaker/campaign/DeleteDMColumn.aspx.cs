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
using System.Text;

namespace Rainmaker.Web.campaign
{
    public partial class DeleteDMColumn : PageBase
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
                BindColumnList();
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
        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (lbxColumns.SelectedItem == null)
                {
                    Response.Write("<script>alert('Please select a column to delete.');</script>");
                    return;
                }

                if (lbxColumns.SelectedItem.Text.Length < 1)
                {
                    Response.Write("<script>alert('Please select a column to delete.');</script>");
                    return;
                }

                string FieldName = lbxColumns.SelectedItem.Text;
                string NewFieldList = "";

                string[] lstColumns = Session["GridFields"].ToString().Split(',');

                foreach (string clmn in lstColumns)
                {
                    if (clmn != FieldName)
                    {
                        NewFieldList = string.Format("{0},{1}", NewFieldList, clmn);
                    }
                }

                NewFieldList = NewFieldList.TrimStart(',');

                Session["GridFields"] = NewFieldList;

                if (lbxColumns.SelectedItem.Text.Length > 0)
                    Session["FieldsChanged"] = "true";

                lbxColumns.Items.Remove(lbxColumns.SelectedItem);

                //BindColumnList();
            }
            catch (Exception ex)
            {
                PageMessage = "Exception deleting column: " + ex.Message;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Bind result description and code to list of selected Campaign
        /// </summary>
        private void BindColumnList()
        {
            try
            {
                //if (lbxColumns.Items.Count > 0)
                //{
                //    lbxColumns.Items.Clear();
                //}

                string[] lstColumns = Session["GridFields"].ToString().Split(',');

                foreach (string clmn in lstColumns)
                {
                    lbxColumns.Items.Add(new ListItem(clmn));
                }
            }
            catch (Exception ex)
            {
                PageMessage = "Exception building column list: " + ex.Message;
            }
        }
        #endregion
    }
}
