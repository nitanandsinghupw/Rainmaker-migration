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
using System.Xml;
using Rainmaker.Common.DomainModel;
using Rainmaker.Web.CampaignWS;

namespace Rainmaker.Web.campaign
{

    public partial class Agents : PageBase
    {
        private int m_totalRows = 0;
        private int m_totalLiveAgents = 0;
        private int m_totalLS = 0;
        private double m_totalPledges = 0;
        private int m_totalPress = 0;
        private int m_totalCalls = 0;
        private double m_totalRatio = 0;
        private double m_averageRatio = 0;
        private double m_totalTalk = 0;
        private double m_totalReady = 0;
        private double m_totalPause = 0;
        private double m_totalWrap = 0;

        Campaign objCampaign = null;
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
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                    anchHome.InnerText = objCampaign.Description;// Replaced Short description
                    BindAgentStat(objCampaign);
                }
            }
        }

        #endregion

        #region Private Methods

        private void BindAgentStat(Campaign objCampaign)
        {
            DataSet dsAgentStat;
            try
            {
                CampaignService objCampService = new CampaignService();
                dsAgentStat = objCampService.GetAgentStat(objCampaign.CampaignDBConnString, objCampaign.CampaignID);

                grdAgent.DataSource = dsAgentStat;
                grdAgent.DataBind();
                grdFundraiserAgent.DataSource = dsAgentStat;
                grdFundraiserAgent.DataBind();
                if (objCampaign.FundRaiserDataTracking)
                {
                    pnlFundraiserView.Visible = true;
                    pnlNormalView.Visible = false;
                }
                else
                {
                    pnlFundraiserView.Visible = false;
                    pnlNormalView.Visible = true;
                }
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        protected void grdAgent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Aggregate data for footer row
                m_totalRows++;
                if (e.Row.Cells[2].Text == "Talking")
                {
                    m_totalLiveAgents++;
                }
                m_totalLS += Convert.ToInt16(e.Row.Cells[3].Text);
                m_totalPress += Convert.ToInt16(e.Row.Cells[4].Text);
                m_totalCalls += Convert.ToInt16(e.Row.Cells[5].Text);
                m_totalRatio += Convert.ToDouble(e.Row.Cells[6].Text);
                m_totalTalk += Convert.ToDouble(e.Row.Cells[7].Text);
                m_totalReady += Convert.ToDouble(e.Row.Cells[8].Text);
                m_totalPause += Convert.ToDouble(e.Row.Cells[9].Text);
                m_totalWrap += Convert.ToDouble(e.Row.Cells[10].Text);
                for (int i = 7; i <= 10; i++)
                {
                    e.Row.Cells[i].Text = FormatTime(e.Row.Cells[i].Text);
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                m_averageRatio = m_totalRatio / m_totalRows;
                e.Row.Cells[3].Text = "Total: " + m_totalLS.ToString();
                e.Row.Cells[4].Text = "Total: " + m_totalPress.ToString();
                e.Row.Cells[5].Text = "Total: " + m_totalCalls.ToString();
                e.Row.Cells[6].Text = "Avg: " + string.Format("{0:0.00}", m_averageRatio);
                e.Row.Cells[7].Text = "Total: " + FormatTime(m_totalTalk.ToString());
                e.Row.Cells[8].Text = "Total: " + FormatTime(m_totalReady.ToString());
                e.Row.Cells[9].Text = "Total: " + FormatTime(m_totalPause.ToString());
                e.Row.Cells[10].Text = "Total: " + FormatTime(m_totalWrap.ToString());
            }
        }

        protected void grdFundAgent_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Aggregate data for footer row
                m_totalRows++;
                if (e.Row.Cells[2].Text == "Talking")
                {
                    m_totalLiveAgents++;
                }
                m_totalPledges += Convert.ToDouble(e.Row.Cells[3].Text);
                m_totalPress += Convert.ToInt16(e.Row.Cells[4].Text);
                m_totalCalls += Convert.ToInt16(e.Row.Cells[5].Text);
                m_totalRatio += Convert.ToDouble(e.Row.Cells[6].Text);
                m_totalTalk += Convert.ToDouble(e.Row.Cells[7].Text);
                m_totalReady += Convert.ToDouble(e.Row.Cells[8].Text);
                m_totalPause += Convert.ToDouble(e.Row.Cells[9].Text);
                m_totalWrap += Convert.ToDouble(e.Row.Cells[10].Text);
                for (int i = 7; i <= 10; i++)
                {
                    e.Row.Cells[i].Text = FormatTime(e.Row.Cells[i].Text);
                }
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                m_averageRatio = m_totalRatio / m_totalRows;
                e.Row.Cells[3].Text = "Total: " + string.Format("{0:0.00}", m_totalPledges);
                e.Row.Cells[4].Text = "Total: " + m_totalPress.ToString();
                e.Row.Cells[5].Text = "Total: " + m_totalCalls.ToString();
                e.Row.Cells[6].Text = "Avg: " + string.Format("{0:0.00}", m_averageRatio);
                e.Row.Cells[7].Text = "Total: " + FormatTime(m_totalTalk.ToString());
                e.Row.Cells[8].Text = "Total: " + FormatTime(m_totalReady.ToString());
                e.Row.Cells[9].Text = "Total: " + FormatTime(m_totalPause.ToString());
                e.Row.Cells[10].Text = "Total: " + FormatTime(m_totalWrap.ToString());
            }
        }

        /// <summary>
        /// Runs the timer for each 20 seconds
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Timer1_Tick(object sender, EventArgs e)
        {
            Timer1.Enabled = false;
            try
            {
                if (Session["Campaign"] != null)
                {
                    objCampaign = (Campaign)Session["Campaign"];
                    BindAgentStat(objCampaign);
                }
            }
            catch { }
            Timer1.Enabled = true;
        }

        #endregion
    }
}
