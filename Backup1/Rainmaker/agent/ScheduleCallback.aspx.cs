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

namespace Rainmaker.Web.agent
{
    public partial class ScheduleCallback : PageBase
    {
        #region Events
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        protected void Page_Load(object sender, EventArgs e)
        {
            Agent objAgent = (Agent)Session["LoggedAgent"];
            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Scheduled Callback LoggedAgent is : " + objAgent.AgentID);
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["pagefrom"] != null)
                {
                    string pageFrom = Request.QueryString["pagefrom"];
                    hdnPageFrom.Value = pageFrom.ToString();
                }
                
                BindDialingTime(0, 59, ddlDailingMinutes);
                BindDropDown(1, 12, ddlDailingHrs);
                try
                {
                    ddlDailingHrs.SelectedValue = "12";
                }
                catch { }
                BindData();
            }
        }

        /// <summary>
        /// Saves ScheduleDate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        #endregion

        #region Private Methods
        /// <summary>
        /// Save Schedule Date
        /// </summary>
        private void SaveData()
        {
            if (Session["CampaignDtls"] != null && Session["Campaign"] != null)
            {
                try
                {
                    Campaign objCampaign;

                    objCampaign = (Campaign)Session["Campaign"];

                    CampaignService objCampService = new CampaignService();
                    CampaignDetails objCampaigndetails = new CampaignDetails();

                    DataSet dsCampaigndetails;
                    dsCampaigndetails = (DataSet)Session["CampaignDtls"];
                    if (dsCampaigndetails.Tables[0].Rows.Count > 0)
                    {
                        string scheduleDate = txtDate.Text.Trim();
                        //if (rbtnPM.Checked)
                        //{
                        scheduleDate = scheduleDate + " " + ddlDailingHrs.SelectedValue + ":" + ddlDailingMinutes.SelectedValue + " " + ddlDailing.SelectedValue;
                        //}
                        objCampaigndetails.ScheduleDate = Convert.ToDateTime(scheduleDate);
                        objCampaigndetails.UniqueKey = Convert.ToInt64(dsCampaigndetails.Tables[0].Rows[0]["UniqueKey"].ToString());
                        objCampaigndetails.ScheduleNotes = txtNotes.Text.Trim();
                        try
                        {

                            Agent objagent = (Agent)Session["LoggedAgent"];
                            objCampaigndetails.AgentID = Convert.ToString(objagent.AgentID);
                            objCampaigndetails.AgentName = objagent.AgentName;
                            if (dsCampaigndetails.Tables[0].Rows[0]["QueryId"].ToString() != "")
                            {
                                objCampaigndetails.QueryId = Convert.ToInt64(dsCampaigndetails.Tables[0].Rows[0]["QueryId"].ToString());
                            }
                            else
                            {
                                objCampaigndetails.QueryId = 0;
                            }
                            
                        }
                        catch { }
                    }
                    XmlDocument xDocCampaign = new XmlDocument();
                    xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
                    XmlDocument xDocCampaigndetails = new XmlDocument();
                    xDocCampaigndetails.LoadXml(Serialize.SerializeObject(objCampaigndetails, "CampaignDetails"));
                    //objCampService.InsertUpdateScheduledCampaign(xDocCampaign, xDocCampaigndetails);

                    objCampService.UpdateCampaignSchedule(xDocCampaign, xDocCampaigndetails, true);

                    //hangup the call
                    try
                    {
                        objCampService.SetCallHangup(objCampaigndetails.UniqueKey, objCampaign.CampaignDBConnString);
                    }
                    catch { };

                    hdnClose.Value = "false";
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "key", "Close();", true);
                }

                catch (Exception ex)
                {
                    if (ex.Message.ToString().IndexOf("valid DateTime") != -1)
                        PageMessage = "Enter Valid Schedule Date";
                    else
                        PageMessage = ex.Message;
                }
                /*finally
                {
                    if (hdnPageFrom.Value == "ManualDial")
                    {

                        Response.Redirect("../Agent/ManualDial.aspx?ts=" + DateTime.Now.Ticks.ToString());
                    }
                    if (hdnPageFrom.Value == "waitingforcall")
                    {

                        Response.Redirect("../Agent/WaitingForCall.aspx?ts=" + DateTime.Now.Ticks.ToString());
                    }


                }*/
            }
        }

        /// <summary>
        /// display customer name and Phone Number
        /// </summary>
        private void BindData()
        {
            if (Session["CampaignDtls"] != null)
            {
                DataSet dsCampaignDtls;
                dsCampaignDtls = (DataSet)Session["CampaignDtls"];
                if (dsCampaignDtls.Tables[0].Rows.Count > 0)
                {
                    txtFirstName.Text = dsCampaignDtls.Tables[0].Rows[0]["FirstName"].ToString();
                    txtLastName.Text = dsCampaignDtls.Tables[0].Rows[0]["LastName"].ToString();
                    txtPhoneNo.Text = dsCampaignDtls.Tables[0].Rows[0]["PhoneNum"].ToString();
                }
            }
        }
        #endregion

    }
}
