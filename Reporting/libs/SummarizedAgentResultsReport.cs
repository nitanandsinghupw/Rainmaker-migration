using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using Rainmaker.Data;

namespace Rainmaker.Reports {

    public class SummarizedAgentResultsReport : GenericReport {

        // Encodes the SummarizedAgentResultsReport as an HTML document
        //
        // NOTE: this is not a full HTML document containing a 'body',
        // 'html' tag etc. The entire report is written into a 'div'
        // that uses CSS style information that is found in the 
        // 'callhistoryreport.css' file.
        //
        // NOTE: In the future it would be good to pick all this HTML
        // up from a template engine and remove it directly from this
        // code. This was was the "get it done now" approach. 
        override public string encodeHTMLReport(Campaign campaign,
                                                string startDate,
                                                string endDate,
                                                string startTime,
                                                string endTime) {
           
            CampaignSummary cs = new CampaignSummary(campaign,
                                                     DateTime.Parse(startDate), 
                                                     DateTime.Parse(endDate),
                                                     DateTime.Parse(startTime),
                                                     DateTime.Parse(endTime));

			string agentName = "All Agents";
			if(AgentID > 0) {
				
				Agent agent = new Agent(campaign, AgentID);
				agentName = agent.AgentName;
			}
			List<Agent> agents = filterAgents(cs);
            error = false;
            string html = "<div class=\"summarizedAgentResultsReport\">";
            //
            /// Show the report header/information
            //
            html += "<div>";
            html += "  <p id=\"summarizedAgentResultsHeader\">Summarized Agent Results Report By Agent</p>";
            html += "  <div style=\"position: relative; margin-top: 15px;\">";
            html += "    <div style=\"height: 80px; width: 790px;\">";
			html += "      	<div style=\"text-align: left;\">";
            html += "      		<span class=\"summarizedAgentResultsSecHeader\" style=\"margin-left: 100px; font-size: large; text-decoration: none\">";
            html += "        	For Campaign: &nbsp&nbsp&nbsp " + campaign.Description;
            html += "      		</span>";
			html += "      	</div>";
			html += "      	<div style=\"text-align: left;\">";
            html += "      		<span class=\"summarizedAgentResultsSecHeader\" style=\"margin-left: 100px; font-size: large; text-decoration: none\">";
            html += "        	Agent: &nbsp&nbsp&nbsp " + agentName;
            html += "      		</span>";
			html += "      	</div>";
			html += "      	<div style=\"text-align: left;\">";
            html += "      		<span class=\"summarizedAgentResultsSecHeader\" style=\"margin-left: 100px; font-size: large; text-decoration: none\">";
            html += "        	&nbsp";
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

			// Display the Dialer results for agent
            html += "<div id=\"summarizedAgentsResultsHeaderTop\" style=\"position: relative; height: 20px; margine-bottom: 20px\">";
            html += "  <span style=\"position: absolute; left: 20px; width: 50px; text-align: center\">Agent</span>";
        	html += "  <span style=\"position: absolute; left: 70px; width: 200px; text-align: left\">&nbsp</span>";
			html += "  <span style=\"position: absolute; left: 270px; width: 60px; text-align: center\">Total</span>";
			html += "  <span style=\"position: absolute; left: 330px; width: 60px; text-align: center\">Leads</span>";
			html += "  <span style=\"position: absolute; left: 390px; width: 60px; text-align: center\">Leads</span>";
			html += "  <span style=\"position: absolute; left: 460px; width: 60px; text-align: center\">&nbsp</span>";
			html += "  <span style=\"position: absolute; left: 500px; width: 60px; text-align: center\">Pres.</span>";
			html += "  <span style=\"position: absolute; left: 560px; width: 60px; text-align: center\">Leads</span>";
			html += "  <span style=\"position: absolute; left: 620px; width: 60px; text-align: center\">Pres.</span>";
			html += "  <span style=\"position: absolute; left: 680px; width: 60px; text-align: center\">Leads/</span>";
			html += "  <span style=\"position: absolute; left: 745px; width: 60px; text-align: center\">Dialed</span>";
			html += "  <span style=\"position: absolute; left: 815px; width: 60px; text-align: center\">Man</span>";
			html += "  <span style=\"position: absolute; left: 880px; width: 60px; text-align: center\">Pause</span>";
			html += "  <span style=\"position: absolute; left: 940px; width: 60px; text-align: center\">Talk</span>";
			html += "</div>";
			
			html += "<div id=\"summarizedAgentsResultsHeaderBottom\" style=\"position: relative; height: 20px; margine-bottom: 20px\">";
            html += "  <span style=\"position: absolute; left: 20px; width: 50px; text-align: center\">ID</span>";
			html += "  <span style=\"position: absolute; left: 70px; width: 200px; text-align: left\">Agent Name</span>";			
			html += "  <span style=\"position: absolute; left: 270px; width: 60px; text-align: center\">Calls</span>";
			html += "  <span style=\"position: absolute; left: 330px; width: 60px; text-align: center\">(Sales)</span>";
			html += "  <span style=\"position: absolute; left: 390px; width: 60px; text-align: center\">/hr</span>";
			html += "  <span style=\"position: absolute; left: 460px; width: 60px; text-align: left\">Pres.</span>";
			html += "  <span style=\"position: absolute; left: 500px; width: 60px; text-align: center\">/hr</span>";
			html += "  <span style=\"position: absolute; left: 560px; width: 60px; text-align: center\">%</span>";
			html += "  <span style=\"position: absolute; left: 620px; width: 60px; text-align: center\">%</span>";
			html += "  <span style=\"position: absolute; left: 680px; width: 60px; text-align: center\">Pres. %</span>";
			html += "  <span style=\"position: absolute; left: 745px; width: 60px; text-align: center\">Hrs.</span>";
			html += "  <span style=\"position: absolute; left: 815px; width: 60px; text-align: center\">Hrs.</span>";
			html += "  <span style=\"position: absolute; left: 880px; width: 60px; text-align: center\">Time</span>";
			html += "  <span style=\"position: absolute; left: 940px; width: 60px; text-align: center\">%</span>";
			html += "</div>";
			
			foreach(Agent curAgent in agents) {
				
				html += "<div id=\"summarizedAgentsResultsRow\" style=\"position: relative; height: 20px\">";
				html += String.Format("  <span style=\"position: absolute; left: 20px; width: 50px; text-align: center\">{0}</span>", curAgent.AgentID);
				html += String.Format("  <span style=\"position: absolute; left: 70px; width: 200px; text-align: left\">{0}</span>", curAgent.AgentName);
				html += String.Format("  <span style=\"position: absolute; left: 270px; width: 60px; text-align: center\">{0}</span>", curAgent.TotalCalls);
				html += String.Format("  <span style=\"position: absolute; left: 330px; width: 60px; text-align: center\">{0}</span>", curAgent.Leads);
				html += String.Format("  <span style=\"position: absolute; left: 390px; width: 60px; text-align: center\">{0:0.##}</span>", curAgent.LeadsPerHour);
				html += String.Format("  <span style=\"position: absolute; left: 460px; width: 60px; text-align: left\">{0}</span>", curAgent.Presentations);
				html += String.Format("  <span style=\"position: absolute; left: 500px; width: 60px; text-align: center\">{0:0.##}</span>", curAgent.PresentationsPerHour);
				html += String.Format("  <span style=\"position: absolute; left: 560px; width: 60px; text-align: center\">{0:##%}</span>", curAgent.LeadsPerCall);
				html += String.Format("  <span style=\"position: absolute; left: 620px; width: 60px; text-align: center\">{0:##%}</span>", curAgent.PresentationsPerCall);
				html += String.Format("  <span style=\"position: absolute; left: 680px; width: 60px; text-align: center\">{0:##%}</span>", curAgent.LeadsPerPresentation);
				html += String.Format("  <span style=\"position: absolute; left: 745px; width: 60px; text-align: center\">{0}</span>", Agent.FormatTimeValue(curAgent.DialingHours));
				html += String.Format("  <span style=\"position: absolute; left: 815px; width: 60px; text-align: center\">{0}</span>", Agent.FormatTimeValue(curAgent.ManHours));
				html += String.Format("  <span style=\"position: absolute; left: 880px; width: 60px; text-align: center\">{0}</span>", Agent.FormatTimeValue(curAgent.PauseTime));
				html += String.Format("  <span style=\"position: absolute; left: 940px; width: 60px; text-align: center\">{0:##%}</span>", curAgent.TalkPercentage);
				html += "</div>";
			}
			
			html += "<div id=\"summarizedAgentsResultsFooter\" style=\"position: relative; height: 20px; margine-bottom: 20px\">";
			html += String.Format("  <span style=\"position: absolute; left: 270px; width: 60px; text-align: center\">{0}</span>", cs.Agents[0].TotalCalls);
			html += String.Format("  <span style=\"position: absolute; left: 330px; width: 60px; text-align: center\">{0}</span>", cs.Agents[0].Leads);
			html += String.Format("  <span style=\"position: absolute; left: 390px; width: 60px; text-align: center\">{0:0.##}</span>", cs.Agents[0].LeadsPerHour);
			html += String.Format("  <span style=\"position: absolute; left: 460px; width: 60px; text-align: left\">{0}</span>", cs.Agents[0].Presentations);
			html += String.Format("  <span style=\"position: absolute; left: 500px; width: 60px; text-align: center\">{0:0.##}</span>", cs.Agents[0].PresentationsPerHour);
			html += String.Format("  <span style=\"position: absolute; left: 560px; width: 60px; text-align: center\">{0:##%}</span>", cs.Agents[0].LeadsPerCall);
			html += String.Format("  <span style=\"position: absolute; left: 620px; width: 60px; text-align: center\">{0:##%}</span>", cs.Agents[0].PresentationsPerCall);
			html += String.Format("  <span style=\"position: absolute; left: 680px; width: 60px; text-align: center\">{0:##%}</span>", cs.Agents[0].LeadsPerPresentation);
			html += String.Format("  <span style=\"position: absolute; left: 745px; width: 60px; text-align: center\">{0}</span>", Agent.FormatTimeValue(cs.Agents[0].DialingHours));
			html += String.Format("  <span style=\"position: absolute; left: 815px; width: 60px; text-align: center\">{0}</span>", Agent.FormatTimeValue(cs.Agents[0].ManHours));

			html += String.Format("  <span style=\"position: absolute; left: 880px; width: 60px; text-align: center\">{0}</span>", Agent.FormatTimeValue(cs.Agents[0].PauseTime));

			html += String.Format("  <span style=\"position: absolute; left: 940px; width: 60px; text-align: center\">{0:##%}</span>", cs.Agents[0].TalkPercentage);
			html += "</div>";
			
            // End of document
            html += "</div>\n\n";
            return html;
        }

		private List<Agent> filterAgents(CampaignSummary cs) {
			
			List<Agent> agents = new List<Agent>();
			for(int i=1; i < cs.Agents.Count; i++) {
			
				Agent agent = cs.Agents[i];
				if(AgentID == 0)
				{		
					agents.Add(agent);
				}
				else
				{
					if(agent.AgentID == AgentID) agents.Add(agent);
				}
			}
			return agents;
		}
		
		public long AgentID { get; set; }
    } 
}
