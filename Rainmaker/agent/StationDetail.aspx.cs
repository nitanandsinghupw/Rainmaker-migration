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
    public partial class StationDetail : PageBase
    {

        #region Events

        /// <summary>
        /// On Page load
        /// show the station details 
        /// if Station id selected in List screen.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["StationID"] != null)
                {
                    long stationID = Convert.ToInt64(Request.QueryString["StationID"]);
                    ShowStationDetails(stationID);
                    hdnStationId.Value = stationID.ToString();
                }
            }
        }

        /// <summary>
        /// Saves Station details and navigates to Station List screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            if (SaveStation())
            {
                Response.Redirect("~/agent/StationList.aspx");
            }
        }

        /// <summary>
        /// get the data from Database based on Station Id when editing,
        /// clear the data when adding Station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            if (hdnStationId.Value != string.Empty)
            {
                ShowStationDetails(Convert.ToInt64(hdnStationId.Value));
            }
            else
            {
                ClearData();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// gets station details and shows
        /// </summary>
        /// <param name="agentID"></param>
        private void ShowStationDetails(long stationID)
        {
            Station objStation;
            try
            {
                AgentService objAgentService = new AgentService();

                objStation = (Station)Serialize.DeserializeObject(
                               objAgentService.GetStationByStationID(stationID), "Station");

                txtStationIP.Text = objStation.StationIP;
                txtStationNumber.Text = objStation.StationNumber;
                chkAllwaysOffhook.Checked = objStation.AllwaysOffHook;
                
                hdnStationId.Value = stationID.ToString();
                
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Saves Station details
        /// </summary>
        /// <returns></returns>
        private bool SaveStation()
        {
            try
            {                
                long stationId = 0;
                if (hdnStationId.Value != string.Empty)
                    stationId = Convert.ToInt64(hdnStationId.Value);

                Station objStation = new Station();
                
                objStation.StationID = stationId;
                objStation.StationIP = txtStationIP.Text.Trim().Replace("'", "''");
                objStation.StationNumber = txtStationNumber.Text.Trim().Replace("'","''");
                objStation.AllwaysOffHook = chkAllwaysOffhook.Checked;
                
                AgentService objAgentService = new AgentService();
                XmlDocument xDocStation = new XmlDocument();

                xDocStation.LoadXml(Serialize.SerializeObject(objStation, "Station"));

                stationId = objAgentService.InsertUpdateStation(xDocStation);
                if (stationId > 0)
                    return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("DuplicateColumnException") >= 0)
                    PageMessage = "Station IP / Station Number already exists";
                else
                    PageMessage = ex.Message;
            }
            return false;
        }

        /// <summary>
        /// Clears data
        /// </summary>
        private void ClearData()
        {
            txtStationIP.Text = string.Empty;
            txtStationNumber.Text = string.Empty;
        }

        #endregion

    }
}
