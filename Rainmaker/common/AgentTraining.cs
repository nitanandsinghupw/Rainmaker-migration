using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Rainmaker.Common.DomainModel;
using System.Xml;
using System.Xml.Serialization;
using System.Text;
using Rainmaker.Web.CampaignWS;
using System.Collections.Generic;

namespace Rainmaker.Web.common
{
    public class AgentTraining
    {
        public static TrainingPage NextTrainingPage (long previousPageID, long pagesSinceScoreBoard, Agent objAgent, Campaign campaign, TrainingScheme trainingScheme) 
        {
            // Choose next page randomly for now, algorithm will go here for later phase ***
            // Add WS load up list, return data set, choose from those available, not previous
            // Need to grab pages from all active schemes automatically

            DataSet dsTrainingPages;
            CampaignService srvCampaign = new CampaignService();
            TrainingPage tpPage = new TrainingPage();
            bool scoreboardTime = false;
            bool isPageValid = false;
            int triesForValidPage = 0;

            if (pagesSinceScoreBoard >= trainingScheme.ScoreboardFrequency)
            {
                scoreboardTime = true;
                pagesSinceScoreBoard = 0;
            }
            else
            {
                pagesSinceScoreBoard++;
            }

            ActivityLogger.WriteAgentEntry(objAgent, "Training module determining next page to build. {0} pages since last scoreboard, which is set to display every {1} pages.", pagesSinceScoreBoard, trainingScheme.ScoreboardFrequency);

            try
            {
                if (scoreboardTime)
                {
                    tpPage.PageID = 0;
                    tpPage.DisplayTime = 20;
                    tpPage.Content = BuildScoreboardContent(campaign, objAgent.AgentID);
                    tpPage.Name = "Scoreboard";
                    tpPage.IsScoreBoard = true;
                }
                else
                {
                    while (!isPageValid && triesForValidPage < 5)
                    {
                        XmlDocument xDocCampaign = new XmlDocument();
                        xDocCampaign.LoadXml(Serialize.SerializeObject(campaign, "Campaign"));
                        dsTrainingPages = srvCampaign.GetActiveTrainingPageList(xDocCampaign);
                        DataRow dr;
                        switch (dsTrainingPages.Tables[0].Rows.Count)
                        {
                            case 0:
                                ActivityLogger.WriteAgentEntry(objAgent, "Active training schemes contain no pages. Cannot return a valid training page.");
                                tpPage = null;
                                return tpPage;
                            case 1:
                                ActivityLogger.WriteAgentEntry(objAgent, "Active training schemes contain only 1 page. The previous page will persist.");
                                dr = dsTrainingPages.Tables[0].Rows[0];
                                break;
                            case 2:
                                ActivityLogger.WriteAgentEntry(objAgent, "Active training schemes contain only 2 pages. The pages will now simply switch.");
                                dr = dsTrainingPages.Tables[0].Rows[0];
                                if ((long)dr["TrainingPageID"] != previousPageID)
                                {
                                    break;
                                }
                                dr = dsTrainingPages.Tables[0].Rows[1];
                                break;
                            default:
                                // *** Randomize which record to grab.
                                Random r = new Random();
                                int rand = r.Next(dsTrainingPages.Tables[0].Rows.Count - 1);
                                dr = dsTrainingPages.Tables[0].Rows[rand];
                                break;
                        }
                        try
                        {
                            tpPage.PageID = (long)dr["TrainingPageID"];
                            //tpPage.TrainingSchemeID = trainingScheme.SchemeID;
                            tpPage.DisplayTime = (int)dr["DisplayTime"];
                            tpPage.Content = dr["TrainingPageContent"] == Convert.DBNull ? string.Empty : dr["TrainingPageContent"].ToString();
                            tpPage.Name = (string)dr["TrainingPageName"];
                            tpPage.IsScoreBoard = false;
                        }
                        catch
                        {
                            ActivityLogger.WriteAgentEntry(objAgent, "Corrupt or misconfigured training page found, looking for another.");
                            triesForValidPage++;
                            continue;
                        }
                        if (tpPage.Content.Length < 1)
                        {
                            ActivityLogger.WriteAgentEntry(objAgent, "Training page with empty content detected. Page ID {0}, name {1}", tpPage.PageID, tpPage.Name);
                            triesForValidPage++;
                            continue;
                        }
                        else
                        {
                            if (tpPage.PageID != previousPageID)
                            {
                                isPageValid = true;
                                break;
                            }
                            triesForValidPage++;
                            continue;
                        }
                    }

                }
                
                ActivityLogger.WriteAgentEntry(objAgent, "Next training page routine returning page '{0}'.", tpPage.Name);
            }
            catch (Exception ex)
            {
                ActivityLogger.WriteException(ex, "Agent");
                tpPage = null; 
            }
            return tpPage;
        }

        public static TrainingScheme GetScheme()
        {
            // *** Future stub for alogrithm to choose between schemes ... add parameters needed by alg., also what is needed to switch schemes in scheme data structure
            return null;
        }

        public static DataSet AgentScoreboard()
        {
            // Build and return dataset for agent scoreboard / ranking page
            // *** Q: Return data set or page?
            return null;
        }

        private static string BuildScoreboardContent(Campaign campaign, long agentID)
        {
            DataSet dsAgentStat;
            string boardContent = string.Empty;
            try
            {
                CampaignService objCampService = new CampaignService();
                dsAgentStat = objCampService.GetAgentStat(campaign.CampaignDBConnString, campaign.CampaignID);

                SortedList<decimal, AgentStat> lstRankedAgents = new SortedList<decimal, AgentStat>(); 
                DataRow dr;

                for (int i = 0; i < dsAgentStat.Tables[0].Rows.Count; i++)
                {
                    dr = dsAgentStat.Tables[0].Rows[i];
                    AgentStat tmpAgentStat = new AgentStat();
                    tmpAgentStat.AgentName = dr["AgentName"].ToString();
                    tmpAgentStat.Status = dr["Status"].ToString();
                    tmpAgentStat.AgentID = (dr["AgentID"] == Convert.DBNull) ? 0 : Convert.ToInt64(dr["AgentID"]);
                    tmpAgentStat.LeadsSales = (dr["LeadsSales"] == Convert.DBNull) ? Convert.ToInt16(0) : Convert.ToInt16(dr["LeadsSales"]);
                    tmpAgentStat.Presentations = (dr["Presentations"] == Convert.DBNull) ? Convert.ToInt16(0) : Convert.ToInt16(dr["Presentations"]);
                    tmpAgentStat.Calls = (dr["Calls"] == Convert.DBNull) ? Convert.ToInt16(0) : Convert.ToInt16(dr["Calls"]);
                    tmpAgentStat.LeadSalesRatio = (dr["LeadSalesRatio"] == Convert.DBNull) ? 0 : Convert.ToDecimal(dr["LeadSalesRatio"]);
                    tmpAgentStat.TalkTime = (dr["TalkTime"] == Convert.DBNull) ? 0 : Convert.ToDecimal(dr["TalkTime"]);
                    tmpAgentStat.WaitingTime = (dr["WaitingTime"] == Convert.DBNull) ? 0 : Convert.ToDecimal(dr["WaitingTime"]);
                    tmpAgentStat.PauseTime = (dr["PauseTime"] == Convert.DBNull) ? 0 : Convert.ToDecimal(dr["PauseTime"]);
                    tmpAgentStat.WrapTime = (dr["WrapTime"] == Convert.DBNull) ? 0 : Convert.ToDecimal(dr["WrapTime"]);
                    tmpAgentStat.LoginDate = (dr["LoginDate"] == Convert.DBNull) ? DateTime.Now : Convert.ToDateTime(dr["LoginDate"]);

                    lstRankedAgents.Add(tmpAgentStat.LeadsSales, tmpAgentStat);
                }

                // Build HTML for page 
                StringBuilder sb = new StringBuilder();
                sb.Append(@"<div>");
                sb.Append(@"<table cellspacing=""1"" cellpadding=""5"" rules=""all"" border=""1"" id=""grdAgent"" style=""border-color:Black;border-width:1px;border-style:solid;width:100%;"">");
                sb.Append(@"<tr align=""center"" style=""color:White;background-color:#333366;font-size:small;font-weight:bold;"">");
                sb.Append(@"<th scope=""col"">Name</th><th scope=""col"">Status</th><th scope=""col"">L/S</th><th scope=""col"">Presentations</th><th scope=""col"">Calls</th><th scope=""col"">Lead Sales Ratio</th><th scope=""col"">Talk Time</th><th scope=""col"">Ready Time</th><th scope=""col"">Pause Time</th><th scope=""col"">Wrap Time</th><th scope=""col"">Logged In</th>");
                sb.Append(@"</tr>");
                for (int i = 0; i < lstRankedAgents.Count; i++)
                {
                    string specialFormatting = "";
                    double rankingPercentile = 0;

                    AgentStat tmpAgentStat = lstRankedAgents.Values[i];

                    if (tmpAgentStat.AgentID == agentID)
                    {
                        // Colors and the like - formatting if the table entry is the active agent
                        switch (lstRankedAgents.Count)
                        {
                            case 1:
                                rankingPercentile = 100;
                                break;
                            default:
                                rankingPercentile = (1 - (i / lstRankedAgents.Count)) * 100;
                                break;
                        }

                        specialFormatting = @" align=""center"" style=""font-weight:bold;";
                        if (rankingPercentile > 66)
                        {
                            specialFormatting = specialFormatting + @"color:#339900;""";
                        }
                        else
                        {
                            if (rankingPercentile > 33)
                            {
                                specialFormatting = specialFormatting + @"color:#CC6600;""";
                            }
                            else
                            {
                                specialFormatting = specialFormatting + @"color:#CC0000;""";
                            }
                        }
                    }

                    sb.AppendFormat(@"<tr{11}><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td><td>{5}</td><td>{6}</td><td>{7}</td><td>{8}</td><td>{9}</td><td>{10}</td></tr>",
                        tmpAgentStat.AgentName, tmpAgentStat.Status, tmpAgentStat.LeadsSales, tmpAgentStat.Presentations, tmpAgentStat.Calls, tmpAgentStat.LeadSalesRatio, tmpAgentStat.TalkTime, tmpAgentStat.WaitingTime, tmpAgentStat.PauseTime, tmpAgentStat.WrapTime, tmpAgentStat.LoginDate, specialFormatting);    

                }
                sb.Append(@"</table>");
                sb.Append(@"</div>");
                boardContent = sb.ToString();
            }
            catch (Exception ex)
            {
                string strMsg = ex.Message;
                return string.Empty;
            }
            return boardContent;
        }        

    }
}
