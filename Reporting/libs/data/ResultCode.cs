using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Rainmaker.Data 
	{
	// Creates summary statistics from lists of CallEvent entries..
	public class ResultCode 
		{
		public ResultCode(Campaign campaign, long resultCodeID, string description) 
			{
			this.campaign = campaign;
			ResultCodeID = resultCodeID;
			Description = description;
			}
				
		public ResultCode(Campaign campaign, long resultCodeID) 
			{
			this.campaign = campaign;
			LoadResultCode(resultCodeID);
			}

		public void CalculateStats() 
			{
			PercentageOfTotal = Count/TotalResultCodes;
			}

		private void LoadResultCode(long resultCodeID) 
			{
			string query = CreateResultCodeQuery(resultCodeID);
			SqlConnection conn = new SqlConnection(campaign.ConnString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			if(sdr.Read()) 
				{
				resultCodeID = (long)sdr[0];
				Description = (string)sdr[1];
				}
			sdr.Close();
			conn.Close();
			}

		private string CreateResultCodeQuery(long resultCodeID) 
			{
			string query =  "SELECT ";
						 query += "   ResultCodeID, ";
						 query += "   Description ";
						 query += "FROM ResultCode ";
						 query += "WHERE resultcodeid=" + resultCodeID;
			return query;
			}

		//
		// Public properties
		//
		public string Description { get; set; }
		public long ResultCodeID { get; set; }
		public double Count { get; set; }
		public double TotalResultCodes { get; set; }
		public double PercentageOfTotal { get; set; }
		public string FormatedPercentageOfTotal
			{ 
			get { return String.Format("{0:0%}", PercentageOfTotal); }
			}
		private Campaign campaign;
		}
	}