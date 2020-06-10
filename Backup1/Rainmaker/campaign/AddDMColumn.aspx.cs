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
    public partial class AddDMColumn : PageBase
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
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlColumnName.SelectedItem == null || ddlColumnName.SelectedValue == "Select a Column")
                {
                    Response.Write("<script>alert('Please select a column to add.');</script>");
                    return;
                }

                if (ddlPosition.SelectedItem == null || ddlPosition.SelectedValue == "Select a Position")
                {
                    Response.Write("<script>alert('Please select a column position.');</script>");
                    return;
                }

                string FieldName = ddlColumnName.SelectedItem.Text;
                int Position = Convert.ToInt16(ddlPosition.SelectedItem.Text);
                string NewFieldList = "";

                string[] lstColumns = Session["GridFields"].ToString().Split(',');

                int FieldIndex = 1;
                bool FieldInserted = false;

                foreach (string clmn in lstColumns)
                {
                    if (FieldIndex == Position)
                    {
                        NewFieldList = string.Format("{0},{1}", NewFieldList, FieldName);
                        NewFieldList = string.Format("{0},{1}", NewFieldList, clmn);
                        FieldInserted = true;
                    }
                    else
                    {
                        NewFieldList = string.Format("{0},{1}", NewFieldList, clmn);
                    }
                    FieldIndex++;
                }

                if (!FieldInserted)
                {
                    // last position ... insert
                    NewFieldList = string.Format("{0},{1}", NewFieldList, FieldName);
                }

                NewFieldList = NewFieldList.TrimStart(',');

                Session["GridFields"] = NewFieldList;
                Session["FieldsChanged"] = "true";

                lbxColumns.Items.Insert((Position - 1), FieldName);

                ddlColumnName.Items.Remove(FieldName);

                ddlPosition.SelectedValue = "Select a Position";
                ddlColumnName.SelectedValue = "Select a Column";
            }
            catch (Exception ex)
            {
                PageMessage = "Exception deleteing column: " + ex.Message;
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
                Campaign currentCampaign = new Campaign();
                if (Session["Campaign"] != null)
                {
                    // we have an existing campaign
                    currentCampaign = (Campaign)Session["Campaign"];
                }
                else
                {
                    return;
                }

                string[] lstColumns = Session["GridFields"].ToString().Split(',');
                List<string> ColumnList = new List<string>();
                foreach (string clmn in lstColumns)
                {
                    lbxColumns.Items.Add(new ListItem(clmn));
                    ColumnList.Add(clmn.ToLower().Trim());
                }

                ColumnList.Add("uniquekey");

                ddlPosition.Items.Add("Select a Position");
                ddlColumnName.Items.Add("Select a Column");

                for (int i = 1; i <= (lbxColumns.Items.Count + 1); i++)
			    {
                    ddlPosition.Items.Add(i.ToString());
			    }

                dsColumns.ConnectionString = currentCampaign.CampaignDBConnString;
                dsColumns.SelectCommand = "Select * FROM Campaign";
                //dsColumns.DataBind();

                DataView dv = (DataView)dsColumns.Select(DataSourceSelectArguments.Empty);
                for (int i = 0; i < dv.Table.Columns.Count; i++)
                {
                    if (!ColumnList.Contains(dv.Table.Columns[i].ColumnName.ToLower()))
                    {
                        ddlColumnName.Items.Add(dv.Table.Columns[i].ColumnName);
                    }
                }

                ddlPosition.SelectedValue = "Select a Position";
                ddlColumnName.SelectedValue = "Select a Column";

            }
            catch (Exception ex)
            {
                PageMessage = "Exception building column list: " + ex.Message;
            }
        }
        #endregion
    }
}
