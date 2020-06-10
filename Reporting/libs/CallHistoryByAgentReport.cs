using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;
using Rainmaker.Data;

namespace Rainmaker.Reports {

    public class CallHistoryByAgentReport : GenericReport {

        // Encodes the CallHistoryReport as an HTML document
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

			Agent agent = new Agent(campaign, AgentID);
			List<CallEvent> calls = filterCallHistory(cs);
            error = false;
            string html = "<div class=\"callHistoryReport\">";
            //
            /// Show the report header/information
            //
            html += "<div>";
            html += "  <p id=\"callHistoryReportHeader\">Call History Report By Agent</p>";
            html += "  <div style=\"position: relative; margin-top: 15px;\">";
            html += "    <div style=\"height: 80px; width: 790px;\">";
			html += "      	<div style=\"text-align: left;\">";
            html += "      		<span class=\"callHistorySecHeader\" style=\"margin-left: 100px; font-size: large; text-decoration: none\">";
            html += "        	For Campaign: &nbsp&nbsp&nbsp " + campaign.Description;
            html += "      		</span>";
			html += "      	</div>";
			html += "      	<div style=\"text-align: left;\">";
            html += "      		<span class=\"callHistorySecHeader\" style=\"margin-left: 100px; font-size: large; text-decoration: none\">";
            html += "        	Agent: &nbsp&nbsp&nbsp " + agent.AgentName;
            html += "      		</span>";
			html += "      	</div>";
			html += "      	<div style=\"text-align: left;\">";
            html += "      		<span class=\"callHistorySecHeader\" style=\"margin-left: 100px; font-size: large; text-decoration: none\">";
            html += "        	Total Calls: &nbsp&nbsp&nbsp " + calls.Count;
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
            html += "<div id=\"callHistoryResultsHeader\">";
            html += "  <span style=\"padding-left: 20px\">Date and Time</span>";
            html += "  <span style=\"position: absolute; left: 195px; width: 100px; text-align: left\">Phone Number</span>"; 
            html += "  <span style=\"position: absolute; left: 350px; width: 100px; text-align: left\">Call Result</span>"; 
            html += "  <span style=\"position: absolute; left: 600px; width: 100px; text-align: center\">Call Duration</span>"; 
            html += "  <span style=\"position: absolute; left: 740px; width: 100px; text-align: center\">Unique Key</span>"; 
		    html += "</div>";
			foreach(CallEvent ce in calls) {
				
				html += "<div id=\"callHistoryResultsRow\">";
				html += String.Format("  <span style=\"width: 100px; padding-left: 20px\">{0}</span>", ce.CallDate);
				html += String.Format("  <span style=\"position: absolute; left: 200px; width: 100px; text-align: left\">{0}</span>", ce.PhoneNumber); 
				html += String.Format("  <span style=\"position: absolute; left: 350px; width: 250px; text-align: left; text-align: left\">{0}</span>", ce.ResultCodeDescription);
				double callDuration = (ce.CallDuration/3600);
				html += String.Format("  <span style=\"position: absolute; left: 610px; width: 100px; text-align: left\">{0}</span>", Agent.FormatTimeValue(callDuration));
				html += String.Format("  <span style=\"position: absolute; left: 760px; width: 100px; text-align: left\">{0}</span>", ce.CallListID); 
				html += "</div>";
			}
			
            // End of document
            html += "</div>\n\n";
            return html;
        }

		private List<CallEvent> filterCallHistory(CampaignSummary cs) {
			
			List<CallEvent> agentEvents = new List<CallEvent>();
			foreach(CallEvent ce in cs.CallEvents) {
			
				if(ce.AgentID == AgentID) agentEvents.Add(ce);
			}
			return agentEvents;
		}
		
		public long AgentID { get; set; }
    } 
}
