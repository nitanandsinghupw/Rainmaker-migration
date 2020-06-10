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
using System.Security.Principal;
using System.IO;
using System.Text;
using Rainmaker.Common.DomainModel;
using System.Xml;
using Rainmaker.Web.AgentsWS;
using Rainmaker.Web.CampaignWS;
using Rainmaker.Web.DataAccess;

namespace Rainmaker
{
    public partial class _Default : Rainmaker.Web.PageBase
    {
        #region Events

        /// <summary>
        /// Page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Session.Abandon();
            }

        }

        protected void lbtnTest_Click(object sender, EventArgs e)
        {
                Campaign objCampaign = new Campaign();
                CampaignDetails callDetail = new CampaignDetails();
                objCampaign.CampaignDBConnString = @"Data Source=RAINMAKER\SQLEXPRESS;User ID=sa;Pwd=jetblue;Initial Catalog=Anime1";
                callDetail.AgentID = "35";
                callDetail.AgentName = "Bowzer";
                callDetail.UniqueKey = 1;
                CampaignService objCampService = new CampaignService();
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                XmlDocument xDocCallDetail = new XmlDocument();
                xDocCallDetail.LoadXml(Serialize.SerializeObject(callDetail, "CampaignDetails"));
                objCampService.AddAgentToCallDetail(xDocCampaign, xDocCallDetail, false);
                return;
        }
        /// <summary>
        /// Authenticates user and redirects to campaign list screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnLogin_Click(object sender, EventArgs e)
        {
            
            //lbtnLogin.Text = "<div border=\"0\" class=\"button blue small\" title=\"Logging In\" alt=\"\" >Login</div>";
            
            //lbtnLogin.Enabled = false;

            if (IsValidate())
            {
                try
                {
                    Agent agent = new Agent();

                    //Checks credentials from agent table and sets session object to agent if successful
                    GetAuthenticatedAgent();
                    agent = (Agent)Session["LoggedAgent"];

                    if (agent != null)
                    {

                        //Call AgentActivity_AgentID

                        //if stationid is missing and phone number is missing then give them message

                        //if logouttime is not null then give message
                        //"You are already logged into "Station1" at "datetimelogin" please logout first or choose another agent to log in as."
                        
                        if (agent.AgentID != 0)
                        {
                            if (agent.IsAdministrator)
                            {
                                try
                                {
                                    AgentService objAgentService = new AgentService();
                                    Agent objAgentToReset = (Agent)Serialize.DeserializeObject(
                          objAgentService.GetAgentByAgentID(agent.AgentID), "Agent");
                                    XmlDocument xDocAgent = new XmlDocument();
                                    xDocAgent.LoadXml(Serialize.SerializeObject(objAgentToReset, "Agent"));

                                    objAgentService.UpdateAgentLogOut(xDocAgent);


                                }
                                catch
                                {
                                    PageMessage = "Login_Click admin Problem updating agent logout.";
                                    lbtnLogin.Enabled = true;
                                    lbtnLogin.Text = "<div border=\"0\" class=\"button green small\" title=\"Login\" alt=\"\" >Login</div>";
                                }
                                Response.Redirect("~/campaign/CampaignList.aspx");
                            }
                            else
                            {
                                try
                                {
                                    //---------------------------------
                                    // We will log the person out in case the
                                    // are still left logged into the system.
                                    //---------------------------------
                                    //DBCampaign dbCampaign = new DBCampaign();
                                    //dbCampaign.bLogAgentOut(agent.AgentID, 
                                    //                        agent.AgentActivityID);

                                    /*if (agent.PhoneNumber == "") {

                                           PageMessage = "A phone number is required and missing.  Please contact your system administrator to add your phone number.";
                                           ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "alert('" + PageMessage + "');", true);
                                           Response.Redirect("../agent/LoginStatus.aspx");

                                   }*/
                                    Agent objAgent = new Agent();
                                    AgentService objAgentService = new AgentService();
                                    try
                                    {
                                        
                                        objAgent.LogoutTime = DateTime.MinValue;
                                        long agentid = agent.AgentID;
                                        objAgent = (Agent)Serialize.DeserializeObject(
                                                       objAgentService.AgentActivity_AgentID(agentid), "Agent");

                                        Session["LoggedAgent"] = objAgent;
                                    }
                                    catch
                                    {
                                        PageMessage = "Problem contacting database thru webservice. Please check with the system administrator.";
                                        lbtnLogin.Enabled = true;
                                        lbtnLogin.Text = "<div border=\"0\" class=\"button green small\" title=\"Login\" alt=\"\" >Login</div>";

                                    }
                                    Agent objMyAgent = (Agent)Serialize.DeserializeObject(
                              objAgentService.GetAgentByAgentID(agent.AgentID), "Agent");
                                    try
                                    {
                                        
                                        if (objAgent.LogoutTime == DateTime.MinValue)
                                        {


                                            PageMessage = "You are already logged into station " + objAgent.StationHost + " at " + objAgent.LoginTime + " Please have the system administrator reset the account.";
                                            txtPassword.Focus();
                                            lbtnLogin.Enabled = true;
                                            lbtnLogin.Text = "<div border=\"0\" class=\"button green small\" title=\"Login\" alt=\"\" >Login</div>";
                                            
                                            //Page.Response.Redirect("../Logout.aspx");
                                           
                                        }
                                        else
                                        {
                                            if (objMyAgent.IsReset)
                                            {
                                                long agentid = agent.AgentID;
                                                objAgentService.ToggleAgentReset(agentid, false);  
                                            }
                                        }
                                        
                                    }
                                    catch (Exception ex) {

                                        Web.ActivityLogger.WriteAgentEntry(agent, "Error checking if they are already logged in: " + ex.Message);
                                        lbtnLogin.Enabled = true;
                                        lbtnLogin.Text = "<div border=\"0\" class=\"button green small\" title=\"Login\" alt=\"\" >Login</div>";

                                    }
                                    
                                    try
                                    {
                                        if (objAgent.LogoutTime != DateTime.MinValue)
                                        {
                                            //XmlDocument xDocAgent = new XmlDocument();
                                            //xDocAgent.LoadXml(Serialize.SerializeObject(objMyAgent, "Agent"));
                                            //objAgentService.UpdateAgentLogOut(xDocAgent);
                                            string sIP = GetIpAddress();
                                            string sMachineName = GetClientsMachineName();

                                            

                                            XmlDocument xDocAgent = new XmlDocument();
                                            xDocAgent.LoadXml(Serialize.SerializeObject(agent, "Agent"));


                                            XmlNode xNodeAgent = objAgentService.InsGet_AgentActivity(xDocAgent, agent.CampaignDB, sIP, sMachineName);
                                            agent = (Agent)Serialize.DeserializeObject(xNodeAgent, "Agent");
                                            Session["LoggedAgent"] = agent;
                                        }

                                    }
                                    catch
                                    {
                                        PageMessage = "Login_Click agent Problem updating new agent activity id.";
                                        lbtnLogin.Enabled = true;
                                        lbtnLogin.Text = "<div border=\"0\" class=\"button green small\" title=\"Login\" alt=\"\" >Login</div>";
                                    }

                                 

                                    if (PageMessage != "") { return; }



                                    //HttpCookie cookie = new HttpCookie("InventiveGuid");
                                    //Set the cookies value
                                    //cookie.Values["AgentGuid"] = agent.AgentID.ToString();
                                    //cookie.Values["AgentActivityGuid"] = agent.AgentActivityID.ToString();

                                   // int timeout = 60;
                                   // this.Session.Timeout = 60;
                                   // try
                                   /* {
                                        if (this.Session.Timeout > 10)
                                        {
                                            timeout = this.Session.Timeout;

                                        }

                                    }
                                    catch { }*/

                                    // Coolkies set to expire twice as long as 
                                    // time out for now.  
                                    /* TimeSpan tsMinute = new TimeSpan(0, 0, timeout, 0);

                                    DateTime dtNow = DateTime.Now;
                                    cookie.Expires = dtNow + tsMinute;
                                    Response.Cookies.Add(cookie);
                                    */
                                    Web.ActivityLogger.WriteAgentEntry(agent, "Logging into system.");
                                }
                                catch (Exception ex) {

                                    Web.ActivityLogger.WriteAgentEntry(agent, "Error lbtnLogin_Click outer exception: " + ex.Message);
                                    lbtnLogin.Enabled = true;
                                    lbtnLogin.Text = "<div border=\"0\" class=\"button green small\" title=\"Login\" alt=\"\" >Login</div>";
                                }
                                Response.Redirect("~/agent/Campaigns.aspx");
                            }
                        }
                        else
                            PageMessage = "Invalid Credentials. Please try again";
                            lbtnLogin.Enabled = true;
                            lbtnLogin.Text = "<div border=\"0\" class=\"button green small\" title=\"Login\" alt=\"\" >Login</div>";
                    }
                    else
                        Web.ActivityLogger.WriteAdminEntry(null, "Failure on login authentication, agent has returned null. Check database connection string in WS config.");
                        PageMessage = "Error authenticating or connecting to remote server.  Please contact your administrator if this error continues.";
                        lbtnLogin.Enabled = true;
                        lbtnLogin.Text = "<div border=\"0\" class=\"button green small\" title=\"Login\" alt=\"\" >Login</div>";
                }
                catch (Exception ex)
                {
                    PageMessage = ex.Message;
                    lbtnLogin.Enabled = true;
                    lbtnLogin.Text = "<div border=\"0\" class=\"button green small\" title=\"Login\" alt=\"\" >Login</div>";
                }

            }
        }

        /// <summary>
        /// Resets username and pa
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        protected void lbtnReset_Click(object s, EventArgs e)
        {
            txtUserName.Text = "";
            txtPassword.Text = "";
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Validation of Text Feilds
        /// 
        /// </summary>
        private bool IsValidate()
        {

            bool rtnValue = true;
            string text = txtUserName.Text;
            if (text == string.Empty) rtnValue = false;
            text = txtPassword.Text;
            if (text == string.Empty) rtnValue = false;
            return rtnValue;
        }

        /// <summary>
        /// Get Authenticated Agent
        /// 
        /// </summary>
        private void GetAuthenticatedAgent()
        {
            try
            {
                Agent agent = new Agent();
                agent.LoginName = txtUserName.Text.Trim();
                agent.Password = txtPassword.Text.ToString();
                AgentService agentservice = new AgentService();
                string sLoginName = agent.LoginName;
                string sPassword = agent.Password;
                string sIP = GetIpAddress();
                string sMachineName = GetClientsMachineName();

                
                XmlNode xNodeAgent = agentservice.Authenticate(sLoginName, 
                                                               sPassword,
                                                               sIP, 
                                                               sMachineName);
                agent = (Agent)Serialize.DeserializeObject(xNodeAgent, "Agent");
                Session["LoggedAgent"] = agent;
            }
            catch (Exception ex)
            {
                PageMessage = ex.Message;
            }
        }

        /// <summary>
        /// Returns IP address of client machine
        /// </summary>
        /// <returns></returns>
        private string GetIpAddress()
        {

            string ip = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (ip == null || ip == string.Empty)
            {
                ip = Request.ServerVariables["REMOTE_ADDR"];
            }
            return ip;
        }

        /// <summary>
        /// Returns Client machine name
        /// </summary>
        /// <returns></returns>
        private string GetClientsMachineName()
        {
            string name = string.Empty;
            try
            {
                name = System.Net.Dns.GetHostEntry(Request.ServerVariables["REMOTE_HOST"]).HostName;
            }
            catch { }
            if (name == string.Empty)
            {
                return Request.ServerVariables["REMOTE_HOST"];
            }
            return name;
        }

        /// <summary>
        /// Validate Agent
        /// 
        /// </summary>
        //private void ValidateAgent()
        //{

        //    GetAuthenticatedAgent();
        //    Agent agent = new Agent();
        //    agent = (Agent)Session["LoggedAgent"];
        //    if (!agent.IsAdministrator)
        //    {
        //        HttpCookie cookie = Request.Cookies["InventiveGuid"];
        //        if (cookie != null)
        //        {
        //            if (cookie.Values["AgentGuid"] != null)
        //            {
        //                Response.Redirect("~/agent/Campaigns.aspx", false);
        //            }
        //        }
        //    }

        //}
        #endregion
    }
}
