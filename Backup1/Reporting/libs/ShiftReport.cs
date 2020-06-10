using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using Rainmaker.Data;
using Antlr.StringTemplate;

namespace Rainmaker.Reports 
{
	// Builds a shift report which shows call history information and other
	// values that indicate the efficiency of operators and equipment...
	public class ShiftReport : GenericReport 
		{
		override public string encodeHTMLReport(Campaign campaign,
																								string startDate,
																								string endDate,
																								string startTime,
																								string endTime) 
			{
                //startDate = string.Format("{0:MM/dd/yyyy}", startDate);
               // endDate = string.Format("{0:MM/dd/yyyy}", endDate);
			CampaignSummary summary = new CampaignSummary(campaign,
																							 DateTime.Parse(startDate), 
																							 DateTime.Parse(endDate),
																							 DateTime.Parse(startTime),
																							 DateTime.Parse(endTime));	
			HttpServerUtility server = HttpContext.Current.Server;
			StringTemplateGroup group =  new StringTemplateGroup("myGroup", server.MapPath(".") + @"\templates");
			StringTemplate report = group.GetInstanceOf(@"shiftreport");
			report.SetAttribute("campaign", campaign);
			report.SetAttribute("startDate", startDate);

			report.SetAttribute("endDate", endDate);
			report.SetAttribute("endTime", endTime);
			report.SetAttribute("startTime", startTime);
			Agent summaryAgent = summary.Agents[0];
			report.SetAttribute("summaryAgent", summaryAgent);
			if(summary.Agents.Count > 0)
				summary.Agents.RemoveAt(0);
			report.SetAttribute("summary", summary);

            report.SetAttribute("manHours", summaryAgent.PrintManHours());
            report.SetAttribute("dialingHours", summaryAgent.PrintDialingHours());
            report.SetAttribute("pauseTime", summaryAgent.PrintPauseTime());

			report.SetAttribute("dials", summaryAgent.Dials);
			report.SetAttribute("answeringMachines", summaryAgent.AnsweringMachines);
			report.SetAttribute("noAnswers", summaryAgent.NoAnswers);
			report.SetAttribute("busys", summaryAgent.Busys);
			report.SetAttribute("operatorIntercept", summaryAgent.OperatorIntercept);
			report.SetAttribute("drops", summaryAgent.Drops);
			report.SetAttribute("connectPercentage", summaryAgent.FormatedConnectPercentage);
			report.SetAttribute("dropPercentage", summaryAgent.FormatedDropPercentage);
			
			report.SetAttribute("talkPercentage", summaryAgent.FormatedTalkPercentage);
			report.SetAttribute("leads", summaryAgent.Leads);
			report.SetAttribute("leadsPerHour", String.Format("{0:0.##}", summaryAgent.LeadsPerHour));
			report.SetAttribute("leadsPerManHour", String.Format("{0:0.##}", summaryAgent.LeadsPerManHour));
			report.SetAttribute("totalPresentations", summaryAgent.Presentations);
			report.SetAttribute("presentationsPerHour", String.Format("{0:0.##}", summaryAgent.PresentationsPerHour));
			report.SetAttribute("presentationsPerManHour", String.Format("{0:0.##}", summaryAgent.PresentationsPerManHour));
			report.SetAttribute("totalCalls", String.Format("{0:0.##}", summaryAgent.TotalCalls));
			report.SetAttribute("averageCallTime", String.Format("{0:0.##}", Agent.FormatTimeValue(summaryAgent.AveCallTime)));
			report.SetAttribute("callsPerHour", String.Format("{0:0.##}", summaryAgent.CallsPerHour));
			report.SetAttribute("avgCallTimePresentation", String.Format("{0:0.##}", Agent.FormatTimeValue(summaryAgent.AveCallTimePresentation)));
			report.SetAttribute("callsPerManHour", String.Format("{0:0.##}", summaryAgent.CallsPerManHour));
			report.SetAttribute("avgCallTimePerLead", String.Format("{0:0.##}", Agent.FormatTimeValue(summaryAgent.AveCallTimeLead)));
			report.SetAttribute("aveWrapTimeLeads", String.Format("{0:0.##}", Agent.FormatTimeValue(summaryAgent.AveWrapTimeLeads)));
			report.SetAttribute("aveWaitTime", String.Format("{0:0.##}", Agent.FormatTimeValue(summaryAgent.AveWaitTime)));
			return report.ToString();
			}
		}
}
