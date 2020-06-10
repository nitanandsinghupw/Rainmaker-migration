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
    public partial class AgentDetail : PageBase
    {

        #region Events

        /// <summary>
        /// On Page load
        /// show the agent details 
        /// if Agent id selected in List screen.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">event args</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["AgentID"] != null)
                {
                    long agentID = Convert.ToInt64(Request.QueryString["AgentID"]);
                    ShowAgentDetails(agentID);
                    hdnAgentId.Value = agentID.ToString();
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "key", "jqueryfunctions()", true);
        }

        /// <summary>
        /// Saves agent details and navigates to AgentList screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            if (SaveAgent())
            {
                Response.Redirect("~/agent/AgentList.aspx");
            }
        }

        /// <summary>
        /// get the data from Database based on Agent Id when editing,
        /// clear the data when adding agent
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnCancel_Click(object sender, EventArgs e)
        {
            if (hdnAgentId.Value != string.Empty)
            {
                ShowAgentDetails(Convert.ToInt64(hdnAgentId.Value));
            }
            else
            {
                ClearData();
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// gets agent details and shows
        /// </summary>
        /// <param name="agentID"></param>
        private void ShowAgentDetails(long agentID)
        {
            Agent objAgent;
            try
            {
                AgentService objAgentService = new AgentService();

                objAgent = (Agent)Serialize.DeserializeObject(
                               objAgentService.GetAgentByAgentID(agentID), "Agent");

                txtAgentname.Text = objAgent.AgentName;
                txtLoginName.Text = objAgent.LoginName;
                //txtPassword.Text = objAgent.Password;
                txtPhoneNumber.Text = objAgent.PhoneNumber;
                chkAllowManDial.Checked = objAgent.AllowManualDial;
                chkIsAdmin.Checked = objAgent.IsAdministrator;
                chkIsBoundAgent.Checked = objAgent.InBoundAgent;
                chkIsVerificationAgent.Checked = objAgent.VerificationAgent;
                hdnAgentP.Value = objAgent.Password;
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Saves agent details
        /// </summary>
        /// <returns></returns>
        private bool SaveAgent()
        {
            try
            {
                Agent objAgent = new Agent();
                long agentId = 0;
                if (hdnAgentId.Value != string.Empty)
                    agentId = Convert.ToInt64(hdnAgentId.Value);

                objAgent.AgentID = agentId;
                objAgent.AgentName = txtAgentname.Text;
                objAgent.LoginName = txtLoginName.Text;

                if (txtPassword.Text != "")
                {
                    Encryption enc = new Encryption();
                    objAgent.Password = enc.Encrypt(txtPassword.Text);
                }
                else
                    objAgent.Password = hdnAgentP.Value;

                objAgent.PhoneNumber = txtPhoneNumber.Text;
                objAgent.AllowManualDial = chkAllowManDial.Checked;
                objAgent.IsAdministrator = chkIsAdmin.Checked;
                objAgent.InBoundAgent = chkIsBoundAgent.Checked;
                objAgent.VerificationAgent = chkIsVerificationAgent.Checked;

                AgentService objAgentService = new AgentService();
                XmlDocument xDocAgent = new XmlDocument();

                xDocAgent.LoadXml(Serialize.SerializeObject(objAgent, "Agent"));
                objAgent = (Agent)Serialize.DeserializeObject(
                    objAgentService.AgentInsertUpdate(xDocAgent), "Agent");
                if (objAgent.AgentID > 0)
                    return true;
            }
            catch (Exception ex)
            {
                if (ex.Message.IndexOf("NumberDuplicateException") >= 0)
                    PageMessage = "This phone number already assigned to other Agent";
                else if (ex.Message.IndexOf("LoginDuplicateException") >= 0)
                    PageMessage = "Login Name already exists please try again";
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
            txtAgentname.Text = string.Empty;
            txtLoginName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtPhoneNumber.Text = string.Empty;
            chkAllowManDial.Checked = false;
            chkIsAdmin.Checked = false;
            chkIsBoundAgent.Checked = false;
            chkIsVerificationAgent.Checked = false;
        }

        #endregion

    }
}
