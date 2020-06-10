using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using Rainmaker.Data;

namespace Rainmaker.Reports 
	{

	public class AgentsDialerResultsReport : GenericReport 
		{
		// Encodes the CallHistoryReport as an HTML document
		//
		// NOTE: This report has not been finished.
		override public string encodeHTMLReport(Campaign campaign,
																							string startDate,
																							string endDate,
																							string startTime,
																							string endTime) 
			{
			string campaignName = "All Campaigns";			
			Agent agent = new Agent(campaign, AgentID);
			Agent summaryAgent = new Agent(campaign, 0);
			Campaign loader = new Campaign();
			List<CampaignSummary> activeCampaigns = new List<CampaignSummary>();
			if(CampaignID > 0)
				{
				List<RainmakerData> campaigns = loader.GetDataSet("CampaignID=" + CampaignID, null);
				campaign = (Campaign)campaigns.GetRange(0, 1).ToArray()[0];
				campaignName = campaign.Description;
				CampaignSummary cs = new CampaignSummary(campaign,
															DateTime.Parse(startDate), 
															DateTime.Parse(endDate),
															DateTime.Parse(startTime),
															DateTime.Parse(endTime));
				Agent curAgent = Agent.FindAgent(cs.Agents, AgentID);
				if(curAgent != null) summaryAgent = curAgent;
				} 
			else 
				{
				Campaign []campaigns = (Campaign [])loader.GetDataSet(null, null).ToArray();
				foreach(Campaign curCampaign in campaigns)
					{
					CampaignSummary cs = new CampaignSummary(curCampaign,
															 DateTime.Parse(startDate), 
															 DateTime.Parse(endDate),
															 DateTime.Parse(startTime),
															 DateTime.Parse(endTime));
					Agent curAgent = Agent.FindAgent(cs.Agents, AgentID);
					if((curAgent != null) && curAgent.TotalCalls > 0) activeCampaigns.Add(cs);
					summaryAgent.Add(curAgent);
					}
				}
				error = false;
				string html = "<div class=\"agentDialerResultsReport\">";
						//
						/// Show the report header/information
						//
						html += "<div>";
						html += "  <p id=\"agentDialerResultsReportHeader\">Agent Dialer Results Report By Agent</p>";
						html += "  <div style=\"position: relative; margin-top: 15px;\">";
						html += "    <div style=\"height: 80px; width: 790px;\">";
			html += "      	<div style=\"text-align: left;\">";
						html += "      		<span class=\"agentDialerResultsSecHeader\" style=\"margin-left: 100px; font-size: large; text-decoration: none\">";
						html += "        	For Campaign: &nbsp&nbsp&nbsp " + campaignName;
						html += "      		</span>";
			html += "      	</div>";
			html += "      	<div style=\"text-align: left;\">";
						html += "      		<span class=\"agentDialerResultsSecHeader\" style=\"margin-left: 100px; font-size: large; text-decoration: none\">";
						html += "        	Agent: &nbsp&nbsp&nbsp " + agent.AgentName;
						html += "      		</span>";
			html += "      	</div>";
			html += "      	<div style=\"text-align: left;\">";
						html += "      		<span class=\"agentDialerResultsSecHeader\" style=\"margin-left: 100px; font-size: large; text-decoration: none\">";
						html += "        	Total Calls: &nbsp&nbsp&nbsp 0" ;
						html += "      		</span>";
			html += "      	</div>";
			html += "    </div>";	
						html += "    <div style=\"position: absolute; left: 800px; top: -10px; width: 170px; font-weight: bold\">";
						html += "      <div style=\"text-align: right;\">";
						html += "        <p style=\"\">Start Date: " + startDate + "</p>";
						html += "        <p style=\"\">End Date: " + endDate + "</p>";
						html += "      </div>";
						html += "      <p style=\"text-decoration: underline\">Between Times</p>";
						html += "      <div style=\"text-align: right;\">";
						html += "        <p style=\"\">Start Time: " + startTime + " </p>";
						html += "        <p style=\"\">End Time: " + endTime + "</p>"; 
						html += "      </div>";
						html += "    </div>";
						html += "  </div>";
						html += "</div>";


			// Display the the summary information header
						html += "<div id=\"agentDialerResultsHeaderTop\" style=\"position: relative; height: 20px; margine-bottom: 20px\">";
						html += "  <span style=\"position: absolute; left: 20px; width: 50px; text-align: center\">Agent</span>";
					html += "  <span style=\"position: absolute; left: 70px; width: 200px; text-align: left\">&nbsp</span>";
			html += "  <span style=\"position: absolute; left: 200px; width: 60px; text-align: center\">Leads</span>";
			html += "  <span style=\"position: absolute; left: 280px; width: 60px; text-align: center\">Leads</span>";
			html += "  <span style=\"position: absolute; left: 380px; width: 60px; text-align: center\">&nbsp</span>";
			html += "  <span style=\"position: absolute; left: 460px; width: 60px; text-align: center\">Pres./</span>";
			html += "  <span style=\"position: absolute; left: 550px; width: 60px; text-align: center\">Leads/</span>";
			html += "  <span style=\"position: absolute; left: 670px; width: 90px; text-align: center\">Presentations</span>";
			html += "  <span style=\"position: absolute; left: 800px; width: 140px; text-align: center\">Leads/</span>";
			html += "</div>";
			
			html += "<div id=\"agentDialerResultsHeaderBottom\" style=\"position: relative; height: 20px; margine-bottom: 20px\">";
						html += "  <span style=\"position: absolute; left: 20px; width: 50px; text-align: center\">ID</span>";
			html += "  <span style=\"position: absolute; left: 70px; width: 200px; text-align: left\">Agent Name</span>";			
			html += "  <span style=\"position: absolute; left: 200px; width: 60px; text-align: center\">(Sales)</span>";
			html += "  <span style=\"position: absolute; left: 280px; width: 60px; text-align: center\">(Sales)/hr</span>";
			html += "  <span style=\"position: absolute; left: 380px; width: 60px; text-align: left\">Pres.</span>";
			html += "  <span style=\"position: absolute; left: 460px; width: 60px; text-align: center\">/hr</span>";
			html += "  <span style=\"position: absolute; left: 550px; width: 60px; text-align: center\">Sales %</span>";
			html += "  <span style=\"position: absolute; left: 670px; width: 90px; text-align: center\">%</span>";
			html += "  <span style=\"position: absolute; left: 800px; width: 140px; text-align: center\">Presentations %</span>";
			html += "</div>";
// JMA: End Insert


// JMA: Start Print Campaign Summaries



			// End of document
			html += "</div>\n\n";
			return html;
			}

		public long AgentID { get; set; }
		public long CampaignID { get; set; }
		} 
	}
