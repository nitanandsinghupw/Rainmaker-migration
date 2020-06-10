using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;



namespace Rainmaker.Data 
	{
	// General statitical information about the campaign.
	public class CampaignSummary 
		{
		public CampaignSummary(Campaign campaign,
													 DateTime startDate, 
													 DateTime endDate, 
													 DateTime startTime, 
													 DateTime endTime)
			{
			this.campaign = campaign;
			StartDate = startDate;
			EndDate = endDate;
			StartTime = startTime;
			EndTime = endTime;
			conn = new SqlConnection(campaign.ConnString);
			conn.Open();
			GetAgentStats();
			GetCallEvents();
			Agents = Agent.SortAgents(campaign, AgentStats, CallEvents);
			conn.Close();
			}

		private void GetAgentStats() 
			{
			AgentStats = new List<AgentStat>();
			DateTime curDay = new DateTime(StartDate.Ticks);
			TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);
			while(curDay <= EndDate) 
				{
				GetAgentStatsForDay(curDay);
				curDay = curDay.Add(oneDay);
				}
			}

		private void GetAgentStatsForDay(DateTime curDay) 
			{
			SqlCommand cmd = new SqlCommand(CreateAgentStatQuery(curDay), conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			while(sdr.Read()) 
				{
				if(sdr[0] != DBNull.Value) 
					{
					AgentStats.Add(new AgentStat(campaign, (long)sdr[0]));
					}
				}
			sdr.Close();
			}

		private string CreateAgentStatQuery(DateTime curDay) 
			{
			string start = StartTime.ToString("HH:mm");
			string end = EndTime.ToString("HH:mm");
			string query =  "SELECT statid FROM AgentStat ";
						 query += "WHERE ";
						 query += "datemodified>='" + curDay.ToString("MM/dd/yyyy " + start) + "'"; 
						 query += " AND ";
						 query += "datemodified<='" + curDay.ToString("MM/dd/yyyy " + end) + "'";
			return query;
			}

		private void GetCallEvents() 
			{
			CallEvents = new List<CallEvent>();
			DateTime curDay = new DateTime(StartDate.Ticks);
			TimeSpan oneDay = new TimeSpan(1, 0, 0, 0);
			while(curDay <= EndDate) 
				{
				GetCallEventsForDay(curDay);
				curDay = curDay.Add(oneDay);
				}
			}

		private void GetCallEventsForDay(DateTime curDay) 
			{
			string query = CreateCallSelect(curDay);
			SqlCommand cmd = new SqlCommand(query, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			while(sdr.Read()) 
				{
				CallEvents.Add(new CallEvent(campaign, (long)sdr[0]));
				}
			sdr.Close();
			}
				
		private string CreateCallSelect(DateTime curDay) 
			{
			string start = StartTime.ToString("HH:mm");
			string end = EndTime.ToString("HH:mm");
			string query =  "SELECT CallListID FROM CallList \n";
						 query += "WHERE \n";
						 query += "  CallDate>='" + curDay.ToString("MM/dd/yyyy " + start) + "' ";
						 query += "AND ";
						 query += "  CallDate<='" + curDay.ToString("MM/dd/yyyy " + end) + "'";
			return query;
			}

		//
		// Private properties
		//
		private SqlConnection conn;

		//
		// Public properties 
		//
		public List<AgentStat> AgentStats { get; set; }
		public List<CallEvent> CallEvents { get; set; }
		public List<Agent> Agents { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public DateTime StartTime { get; set; }
		public DateTime EndTime { get; set; }
		private Campaign campaign;
		}
	}