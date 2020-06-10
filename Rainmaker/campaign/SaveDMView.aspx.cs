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
    public partial class SaveDMView : PageBase
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
                Agent currentAgent = new Agent();
                // Set the agent(user) and campaign if we have them
                if (Session["LoggedAgent"] != null)
                {
                    // we have an existing agent
                    currentAgent = (Agent)Session["LoggedAgent"];
                }
                else
                {
                    currentAgent = null;
                    Response.Write("<script>alert('No logged in user detected, please log out and try again.');</script>");
                    return;
                }

                if (txtViewName.Text.Length < 1)
                {
                    Response.Write("<script>alert('Please enter a view name.');</script>");
                    return;
                }

                dsViews.ConnectionString = ConfigurationManager.AppSettings["MasterConnectionString"];
                dsViews.SelectCommand = string.Format("DELETE FROM DataManagerViews WHERE ViewName = '{0}'", txtViewName.Text);
                DataView dv = new DataView();
                dv = (DataView)dsViews.Select(DataSourceSelectArguments.Empty);

                dsViews.SelectCommand = string.Format("INSERT INTO DataManagerViews (ViewName, AgentID, FieldList, RecordsPerPage) VALUES ('{0}', {1}, '{2}', {3})", txtViewName.Text, currentAgent.AgentID, Session["GridFields"].ToString(), Session["RecsPerPage"].ToString());
                dv = (DataView)dsViews.Select(DataSourceSelectArguments.Empty);

                Session["DataView"] = txtViewName.Text;
                Session["ViewChanged"] = "yes";

                Response.Write("<script>alert('The view has been saved.');</script>");
                Response.Write("<script>close();</script>");
            }
            catch (Exception ex)
            {
                PageMessage = "Exception saving view: " + ex.Message;
            }
        }
        #endregion
    }
}
