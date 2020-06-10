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
    public partial class AreaCode : PageBase
    {
        private long areaCodeID = 0;
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
                pnlAreaCode.Style.Value = "display:none";
                GetAreaCodeRuleByAgentID();
                GetAreaCodes();
            }
            //if (Session["Campaign"] != null)
            //{
            //    Campaign objCampaign = (Campaign)Session["Campaign"];
            //    lblCampaign.Text = objCampaign.ShortDescription;
            //}
        }

        /// <summary>
        /// Saves Area Code Rule
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        /// <summary>
        /// Saves Local Aera Code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAreaCodeSave_Click(object sender, EventArgs e)
        {
            Rainmaker.Common.DomainModel.AreaCode objAreaCode = new Rainmaker.Common.DomainModel.AreaCode();
            long AreaCodeID = 0;
            if (ViewState["AreaCodeID"] != null)
                AreaCodeID = Convert.ToInt64(ViewState["AreaCodeID"]);
            objAreaCode.AreaCodeID = AreaCodeID;
            objAreaCode.AreaCodeName = txtLocalAreaCode.Text;
            objAreaCode.Prefix = txtLocalPrefix.Text;
            CampaignService objCampaignService = new CampaignService();
            XmlDocument xDocAreaCode = new XmlDocument();
            try
            {
                xDocAreaCode.LoadXml(Serialize.SerializeObject(objAreaCode, "AreaCode"));
                objAreaCode = (Rainmaker.Common.DomainModel.AreaCode)Serialize.DeserializeObject(
                    objCampaignService.AreaCodeInsertUpdate(xDocAreaCode), "AreaCode");
                pnlAreaCode.Style.Value = "display:none";
                GetAreaCodes();
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Shows the Local Area Code Panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAdd_Click(object sender, EventArgs e)
        {
            ViewState["AreaCodeID"] = null;
            txtLocalAreaCode.Text = string.Empty;
            txtLocalPrefix.Text = string.Empty;
            pnlAreaCode.Style.Value = "display:inline";
            txtLocalAreaCode.Focus();
        }

        /// <summary>
        /// Cleares the Local Area Code Fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAreaCodeCancel_Click(object sender, EventArgs e)
        {
            if (ViewState["AreaCodeID"] != null)
            {
                txtLocalAreaCode.Text = hdnGrdAreaCode.Value;
                txtLocalPrefix.Text = hdnGrdPrefix.Value;
            }
            else
            {
                txtLocalAreaCode.Text = string.Empty;
                txtLocalPrefix.Text = string.Empty;
            }
        }

        /// <summary>
        /// Closes the local area code panel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAreaCodeClose_Click(object sender, EventArgs e)
        {
            ViewState["AreaCodeID"] = null;
            txtLocalAreaCode.Text = string.Empty;
            txtLocalPrefix.Text = string.Empty;
            pnlAreaCode.Style.Value = "display:none";
        }

        /// <summary>
        /// This event used to select Row in Grid View Control
        /// </summary>
        int m_iRowIdx = 0;
        protected void OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdn = (HiddenField)e.Row.Cells[0].FindControl("hdnGrdAreaCodeID");
                e.Row.Attributes.Add("onclick", "onGridViewRowSelected('" + m_iRowIdx.ToString() + "' , '" + hdn.Value + "')");
                if (hdn.Value == areaCodeID.ToString())
                {
                    ClientScriptManager cs = base.ClientScript;
                    cs.RegisterStartupScript(this.GetType(),"", "onGridViewRowSelected('" + m_iRowIdx.ToString() + "' , '" + hdn.Value + "')", true);
                    //frmAreaCode.Attributes.Add("onload", "alert('test');onGridViewRowSelected('" + m_iRowIdx.ToString() + "' , '" + hdn.Value + "')");
                }
            }
            m_iRowIdx++;
        }

        /// <summary>
        /// Deletes the row in Grid View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            long AreaCodeID = 0;
            AreaCodeID = Convert.ToInt64(lbtnSender.CommandArgument);

            CampaignService objCampaignService = new CampaignService();
            try
            {
                objCampaignService.DeleteAreaCode(AreaCodeID);

                GetAreaCodes();
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// This event uses to edit a Row in Grid View
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAreaCode_Click(object sender, EventArgs e)
        {
            pnlAreaCode.Style.Value = "display:inline";
            LinkButton lbtnSender = (LinkButton)sender;
            string[] areaCodeFields = lbtnSender.CommandArgument.Split(',');
            ViewState["AreaCodeID"] = areaCodeFields[0];
            txtLocalAreaCode.Text = hdnGrdAreaCode.Value = lbtnSender.Text;
            txtLocalPrefix.Text = hdnGrdPrefix.Value = areaCodeFields[1];
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets area code rule by agentID
        /// </summary>
        private void GetAreaCodeRuleByAgentID()
        {
            long agentID = 0;
            Agent objAgent;
            if (Session["LoggedAgent"] != null)
            {
                objAgent = (Agent)Session["LoggedAgent"];
                agentID = objAgent.AgentID;
            }
            AreaCodeRule objAreaCodeRule;
            CampaignService objCampService = new CampaignService();
            XmlDocument xDocCampaign = new XmlDocument();
            try
            {
                objAreaCodeRule = (AreaCodeRule)Serialize.DeserializeObject(
                    objCampService.GetAreaCodeRuleByAgentID(agentID), "AreaCodeRule");

                areaCodeID = objAreaCodeRule.AreaCodeID;
                ViewState["AreaCodeRuleID"] = objAreaCodeRule.AreaCodeRuleID;
                rbtnAllNumbersDial.Checked = objAreaCodeRule.LikeDialing;
                if (objAreaCodeRule.LikeDialingOption)
                    rbtnTenDigitDialing.Checked = true;
                else
                    rbtnElevenDigitDialing.Checked = true;
                rbtnCustomeDialing.Checked = objAreaCodeRule.CustomeDialing;
                rbtnDialSevenDigits.Checked = objAreaCodeRule.IsSevenDigit;
                rbtnDialTenDigits.Checked = objAreaCodeRule.IsTenDigit;
                txtAreaCode.Text = objAreaCodeRule.IntraLataDialingAreaCode.ToString();
                rbtnILDialTenDigit.Checked = objAreaCodeRule.ILDIsTenDigit;
                rbtnILDialElevenDigit.Checked = objAreaCodeRule.ILDElevenDigit;
                txtReplaceAreaCode.Text = objAreaCodeRule.ReplaceAreaCode.ToString();
                if (objAreaCodeRule.ReplaceAreaCode != "")
                    chkReplaceAreaCode.Checked = true;
                if (objAreaCodeRule.LongDistanceDialing)
                    rbntLDDialTenDigits.Checked = true;
                else
                    rbntLDDialElevenTenDigits.Checked = true;
                pnlAreaCode.Style.Value = "display:none";
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Saves area code rule
        /// </summary>
        private void SaveData()
        {
            long AreaCodeID = 0;
            long AgentID = 0;
            AreaCodeRule objAreaCodeRule = new AreaCodeRule();
            Agent objAgent;
            if (Session["LoggedAgent"] != null)
            {
                objAgent = (Agent)Session["LoggedAgent"];
                AgentID = objAgent.AgentID;
            }
            if (ViewState["AreaCodeRuleID"] != null)
                objAreaCodeRule.AreaCodeRuleID = Convert.ToInt64(ViewState["AreaCodeRuleID"]);
            objAreaCodeRule.AgentID = AgentID;
            if (hdnAreaCodeID.Value != null && hdnAreaCodeID.Value != "")
                AreaCodeID = Convert.ToInt64(hdnAreaCodeID.Value);
            objAreaCodeRule.AreaCodeID = AreaCodeID;
            objAreaCodeRule.LikeDialing = rbtnAllNumbersDial.Checked;
            if (rbtnElevenDigitDialing.Checked)
                objAreaCodeRule.LikeDialingOption = false;
            else if (rbtnTenDigitDialing.Checked)
                objAreaCodeRule.LikeDialingOption = true;
            objAreaCodeRule.CustomeDialing = rbtnCustomeDialing.Checked;
            objAreaCodeRule.IsSevenDigit = rbtnDialSevenDigits.Checked;
            objAreaCodeRule.IsTenDigit = rbtnDialTenDigits.Checked;
            if (txtAreaCode.Text != string.Empty)
                objAreaCodeRule.IntraLataDialingAreaCode = txtAreaCode.Text;
            objAreaCodeRule.ILDIsTenDigit = rbtnILDialTenDigit.Checked;
            objAreaCodeRule.ILDElevenDigit = rbtnILDialElevenDigit.Checked;
            if (txtReplaceAreaCode.Text != string.Empty)
                objAreaCodeRule.ReplaceAreaCode = txtReplaceAreaCode.Text;
            if (rbntLDDialElevenTenDigits.Checked)
                objAreaCodeRule.LongDistanceDialing = false;
            else if (rbntLDDialTenDigits.Checked)
                objAreaCodeRule.LongDistanceDialing = true;
            CampaignService objCampaignService = new CampaignService();
            XmlDocument xDocAreaCodeRule = new XmlDocument();
            try
            {
                xDocAreaCodeRule.LoadXml(Serialize.SerializeObject(objAreaCodeRule, "AreaCodeRule"));
                objAreaCodeRule = (AreaCodeRule)Serialize.DeserializeObject(
                    objCampaignService.AreaCodeRuleInsertUpdate(xDocAreaCodeRule), "AreaCodeRule");

                GetAreaCodeRuleByAgentID();
                GetAreaCodes();
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Binds Local area codes to Grid
        /// </summary>
        private void GetAreaCodes()
        {
            DataSet dsAreaCodes;
            try
            {
                CampaignService objCampaignService = new CampaignService();
                dsAreaCodes = objCampaignService.GetAreaCode();
                grdAreaCode.DataSource = dsAreaCodes;
                grdAreaCode.DataBind();
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion
    }
}
