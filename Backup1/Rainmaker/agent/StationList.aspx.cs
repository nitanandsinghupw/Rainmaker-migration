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

namespace Rainmaker.Web.agent
{
    public partial class StationList : PageBase
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
                GetStationList();
            }
        }

        /// <summary>
        /// Navigate to the StationDetail page 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnStation_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            Response.Redirect("StationDetail.aspx?StationID=" + lbtnSender.CommandArgument);
        }

        /// <summary>
        /// Deletes the Station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            LinkButton lbtnDelete = (LinkButton)sender;
            AgentService objAgentService = new AgentService();
            long stationID = Convert.ToInt64(lbtnDelete.CommandArgument);
           
            try
            {   
                    int result = objAgentService.DeleteStation(stationID);
                    if (result != 0)
                    {
                        GetStationList();
                    }
                
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion

        #region Private Methods 

        /// <summary>
        /// Binds Station list to Grid View
        /// </summary>
        private void GetStationList()
        {
            DataSet dsStationList;
            try
            {
                AgentService objAgentService = new AgentService();
                dsStationList = objAgentService.GetStationList();
                if (dsStationList.Tables[0].Rows.Count == 0)
                {
                    // Redirect to Station creation
                    Response.Redirect("StationDetail.aspx", true);
                }
                grdStationList.DataSource = dsStationList;
                grdStationList.DataBind();

            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }        

        #endregion
    }
}
