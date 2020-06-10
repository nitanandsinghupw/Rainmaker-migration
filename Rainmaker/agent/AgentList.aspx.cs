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
    public partial class AgentList : PageBase
    {

        # region Events

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindAgentList();
            }
        }

        /// <summary>
        /// Raised when user clicks on agent name, navigates to Agentdetail screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnAgent_Click(object sender, EventArgs e)
        {
            LinkButton lbtnSender = (LinkButton)sender;
            Response.Redirect("AgentDetail.aspx?AgentID=" + lbtnSender.CommandArgument);
        }

        /// <summary>
        /// Agent deletion event,
        /// Deletes the selected agent if agent selected is 
        /// not default user or logged in user and
        /// refresh the list of agents 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnDelete_Click(object sender, EventArgs e)
        {
            LinkButton lbtnDelete = (LinkButton)sender;
            Agent agent;

            if (Session["LoggedAgent"] != null)
            {
                agent = (Agent)Session["LoggedAgent"];

                AgentService objAgentService = new AgentService();
                long AgentID = Convert.ToInt64(lbtnDelete.CommandArgument);
                string strDelete = lbtnDelete.CommandName;
                try
                {
                    if (strDelete != "True")
                    {
                        if (agent.AgentID != AgentID)
                        {
                            int result = 0;
                            result = objAgentService.DeleteAgent(AgentID);
                        }
                        else
                        {
                            PageMessage = "Current Logged in user cannot be deleted";
                        }
                    }
                    else
                        PageMessage = "Default Admin user cannot be deleted";
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
                BindAgentList();
            }
        }

        protected void lbtnReset_Click(object sender, EventArgs e)
        {
            LinkButton lbtnReset = (LinkButton)sender;
            Agent agent;

            if (Session["LoggedAgent"] != null)
            {
                agent = (Agent)Session["LoggedAgent"];

                AgentService objAgentService = new AgentService();
                long AgentID = Convert.ToInt64(lbtnReset.CommandArgument);
                string strReset = lbtnReset.CommandName;
                
                try
                {
                    if (strReset != "True")
                    {
                        if (agent.AgentID != AgentID)
                        {
                            Agent objAgentToReset = (Agent)Serialize.DeserializeObject(
                               objAgentService.GetAgentByAgentID(AgentID), "Agent");
                            objAgentService.ToggleAgentReset(AgentID, true);
                            XmlDocument xDocAgent = new XmlDocument();
                            xDocAgent.LoadXml(Serialize.SerializeObject(objAgentToReset, "Agent"));

                            objAgentService.UpdateAgentLogOut(xDocAgent);

                            // 2012-07-15 Dave Pollastrini
                            // I'm not sure where the following call came from, but the function call attempts to set logoff on
                            // agentstat record for the specified agent in the current session campaign to current date/time.
                            // The problem is since we're calling this function from the admin console, there is no
                            // session campaign object.  The call results in a null reference exception.  Since the
                            // UpdateAgentLogOut call above already logs out the agentstat records, there is no need for the  
                            // following call (commented out).

                            // LogOffAgentStat(objAgentToReset);

                            //ActivityLogger.WriteAgentEntry(objAgentToReset, "Adminstrator {0} has caused me to reset.", agent.AgentName);
                            PageMessage = "Selected agent has now been reset.";
                        }
                        else
                        {
                            PageMessage = "Cannot reset agent.  You are logged in as this agent!";
                        }
                    }
                    else
                        PageMessage = "Default Admin user cannot be reset.";
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                }
                BindAgentList();
            }
        }

        /*protected void btnResetAllAgents_Click(object sender, EventArgs e)
        {
            Agent loggedAgent = (Agent)Session["LoggedAgent"];
            if (loggedAgent.IsAdministrator)
            {
                AgentService agentService = new AgentService();

                int agentID;
                foreach (DataRow agentRow in agentService.GetAgentList().Tables[0].Rows)
                {
                    //if !agentRow["isAdministrator"]
                    if (int.TryParse(agentRow["AgentID"].ToString(), out agentID))
                    {
                        agentService.ToggleAgentReset(agentID, true);

                        Agent agent =
                            (Agent)Serialize.DeserializeObject(agentService.GetAgentByAgentID(agentID), "Agent");

                        XmlDocument xDocAgent = new XmlDocument();
                        xDocAgent.LoadXml(Serialize.SerializeObject(agent, "Agent"));

                        agentService.UpdateAgentLogOut(xDocAgent);

     

                        // 2012-08-04 Dave Pollastrini
                        // Update.  The above is true with respect to there being no campaign context, however, the UpdateAgentLogOut
                        // function above logs the agent out of the SYSTEM, but not the individual campaigns.

                        LogOffAgentStat(agent);

                        ActivityLogger.WriteAgentEntry(agent, "Adminstrator {0} has caused agent {1} to be reset.", loggedAgent.AgentName, agent.AgentName);
                    }
                }

                BindAgentList();
                PageMessage = "ALL AGENTS have now been reset.";
            }
            else
            {
                PageMessage = "Only administrators can reset all agents.  Please log in as administrator and try again.";
            }
        }*/

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets agent list and binds to grid
        /// </summary>
        private void BindAgentList()
        {
            DataSet dsAgentList;
            try
            {
                AgentService objAgentService = new AgentService();
                dsAgentList = objAgentService.GetAgentList();
                grdAgentList.DataSource = dsAgentList;
                grdAgentList.DataBind();
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        #endregion

    }
}
