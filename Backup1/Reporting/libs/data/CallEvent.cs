using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Rainmaker.Data 
	{
	public class CallEvent 
		{
		public CallEvent(Campaign campaign, long callListID) 
			{
			CallListID = callListID;
			string query = CreateCallEventQuery();
			SqlConnection conn = new SqlConnection(campaign.ConnString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			if(sdr.Read()) 
				{
                AgentName = (string)sdr[0];
				AgentID = (long)sdr[1];
				ResultCodeID = (long)sdr[2];
				ResultCode resultCode = new ResultCode(campaign, ResultCodeID);
				ResultCodeDescription = resultCode.Description;
				PhoneNumber = (string)sdr[3]; 
				CallDate = (DateTime)sdr[4];
				CallTime = (DateTime)sdr[5];
				CallDuration = (double)((sdr[6] != DBNull.Value) ? (decimal)sdr[6] : 0);
				CallCompletionTime = (DateTime)sdr[7];
				CallWrapTime = (DateTime)sdr[8];
				DateCreated = (DateTime)sdr[10];
				DateModified = (DateTime)sdr[11];
				}
			sdr.Close();
			conn.Close();
			}

		private string CreateCallEventQuery() 
			{
			string query = "SELECT \n";
			bool first = true;
			foreach(string columnName in CampaignColumnNames) 
				{
				if(!first) query += ", ";
				query += columnName;
				first = false;
				}
			query += " FROM CallList \n";
			query += "WHERE \n";
			query += " CallListID=" + CallListID;
			return query;
			}

		//
		// Private variables
		//
		private string []CampaignColumnNames = new string [] 
			{ 
            "AgentName",
			"AgentID",
			"ResultCodeID",
			"PhoneNumber",
			"CallDate",
			"CallTime",
			"CallDuration",
			"CallCompletionTime",
			"CallWrapTime",
			"IsBlocked",
			"DateCreated",
			"DateModified"
			};

		//
		// Public properties 
		//
		public long CallListID { get; set; }
        public string AgentName { get; set; }
        public long AgentID { get; set; }
		public long ResultCodeID { get; set; }
		public string ResultCodeDescription { get; set; }
		public string PhoneNumber { get; set; }
		public DateTime CallDate { get; set; }
		public DateTime CallTime { get; set; }
		public double CallDuration { get; set; }
		public DateTime CallCompletionTime { get; set; }
		public DateTime CallWrapTime { get; set; }
		public bool IsBlocked { get; set; }
		public DateTime DateCreated { get; set; }
		public DateTime DateModified { get; set; }
		}
	}
