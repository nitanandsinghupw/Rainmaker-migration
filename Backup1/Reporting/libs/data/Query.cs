using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;
using Nezasoft.Tools;

namespace Rainmaker.Data 
	{
  // Represents a Query in the Rainmaker
	public class Query : RainmakerData 
		{
		// connot directly instantiate
		private Query(Campaign campaign) : 
			base("query",
						new string []
							{
							"QueryID", 
							"QueryName",
							"QueryCondition"
							}, 
						"QueryID", 
						"QueryName",
			ConfigurationManager.AppSettings["RmConnectionString"]			
         //   ConfigurationManager.ConnectionStrings["ReportMaker"].ConnectionString
					) 
			{
			this.campaign = campaign;
			}

		public Query(Campaign campaign, long queryID): 
			base("query",
						new string []
							{
							"QueryID", 
							"QueryName",
							"QueryCondition"
							}, 
						"QueryID", 
						"QueryName",
			ConfigurationManager.AppSettings["RmConnectionString"])		
         //   ConfigurationManager.ConnectionStrings["ReportMaker"].ConnectionString)
			{
			this.campaign = campaign;
			QueryID = queryID;
			ReadQueryInfo();
			} 
    
		// Gets a set of records from the Query table.
		static public string FetchDataSetAsOptionString(string options, string order, Campaign campaign) 
			{
			Query q = new Query(campaign);
			q.connString = campaign.ConnString;
			return q.GetDataSetAsOptionString(null, null);
			}
        
		// Gets a set of records from the Query table.
		public override List<RainmakerData> GetDataSet(string options, string order) 
			{
// TODO: figure out if this is a problem
//throw new NotSupportedException("You cannnot use Query::GetDataSet()!");
			Records = new List<RainmakerData>();
			DataLoaderDelegate loaderDelegate = new DataLoaderDelegate(RecordLoaded);
			DataLoader(loaderDelegate, options, order);
			return Records;
			}
        
		// Called each a new record has been loaded by the DataLoader().
		private void RecordLoaded(SqlDataReader sdr) 
			{
			Query q = new Query(campaign);
			q.QueryName = (string)sdr[1];
			q.QueryCondition = (string)sdr[2];
			Records.Add(q);
			}

		private void ReadQueryInfo() 
			{
			string query = CreateQueryQuery();
			SqlConnection conn = new SqlConnection(campaign.ConnString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			if(sdr.Read()) 
				{
				QueryName = (string)sdr[0];
				QueryCondition = (string)sdr[1];
				}
			conn.Close();
			}

		private string CreateQueryQuery() 
			{
			string query = "";
			query += "SELECT ";
			query += "	queryname, ";
			query += "	querycondition ";
			query += "FROM query WHERE ";
			query += "	queryid=" + QueryID;
			return query; 
			}

		//
		/// Public properties
		//
		public long QueryID { get; set; }
		public string QueryName { get; set; }
		public string QueryCondition { get; set; }
		private Campaign campaign;
		}
	}