using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Rainmaker.Data 
	{
	// Information a single login for an agent
	public class AgentStat 
		{
		public AgentStat() {}
		public AgentStat(Campaign campaign, long statID)
			{
			StatID = statID;
			string query = CreateAgentStatQuery();
			SqlConnection conn = new SqlConnection(campaign.ConnString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			if(sdr.Read()) 
				{
				AgentID = (long)sdr[0];
				Leads = (sdr[1] != DBNull.Value) ? (int)sdr[1] : 0;
				Presentations = (sdr[2] != DBNull.Value) ? (int)sdr[2] : 0;
				TotalCalls = (sdr[3] != DBNull.Value) ? (int)sdr[3] : 0;
				TalkTime = (double)((sdr[4] != DBNull.Value) ? (decimal)sdr[4] : 0);
				WaitTime = (double)((sdr[5] != DBNull.Value) ? (decimal)sdr[5] : 0);
				PauseTime = (double)((sdr[6] != DBNull.Value) ? (decimal)sdr[6] : 0);
				WrapTime = (double)((sdr[7] != DBNull.Value) ? (decimal)sdr[7] : 0);
				}
			sdr.Close();
			conn.Close();
			}

		private string CreateAgentStatQuery() 
			{
			string query =  "SELECT ";
						 query += "  AgentID, ";
						 query += "  LeadsSales, ";
						 query += "  Presentations, ";
						 query += "  Calls, ";
						 query += "  TalkTime, ";
						 query += "  WaitingTime, ";
						 query += "  PauseTime, ";
						 query += "  WrapTime ";
						 query += "FROM AgentStat WHERE ";
						 query += "  statid=" + StatID; 
			return query;
			}

		//
		// Properties about the agents
		//
		public long StatID { get; set; }
		public long AgentID { get; set; }
		public int Leads { get; set; }
		public int Presentations { get; set; }
		public int TotalCalls { get; set; }
		public double TalkTime { get; set; }
		public double WaitTime { get; set; }
		public double PauseTime { get; set; }
		public double WrapTime { get; set; }
		}
	}
