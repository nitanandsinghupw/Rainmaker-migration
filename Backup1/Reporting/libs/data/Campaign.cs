using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace Rainmaker.Data 
	{
        // ConfigurationManager.ConnectionStrings["ReportMaker"].ConnectionString
        
    /// <summary>
    ///  THi is the good ONE!!
    /// </summary>
	public class Campaign : RainmakerData 
		{
		// Constructor: just tells base class which columns are important
		public Campaign() : 
						base("campaign",
								 new string []{
                                    "CampaignID", 
                                    "Description",
                                    "CampaignDBConnString"
                                    }, 
								 "CampaignID", 
								 "Description",
                                 ConfigurationManager.AppSettings["RmConnectionString"]
								 ) { }

		// Gets a set of records from the Campaign table.
		public override List<RainmakerData> GetDataSet(string options, string order) 
			{
			Records = new List<RainmakerData>();
			DataLoaderDelegate loaderDelegate = new DataLoaderDelegate(RecordLoaded);
			DataLoader(loaderDelegate, options, order);
			return Records;
			}

		// Called each a new record has been loaded by the DataLoader().
		private void RecordLoaded(SqlDataReader sdr) 
			{
			Campaign c = new Campaign();
			c.Id = (long)sdr[0];
			c.Description = (string)sdr[1];
			c.ConnString = (string)sdr[2];
			Records.Add(c);
			}

		// Gets a set of records from the Campaign table.
		static public List<RainmakerData> FetchDataSet(string options, string order) 
			{
			Campaign rc = new Campaign();
			return rc.GetDataSet(null, null);
			}

		// Gets a set of records from the Campaign table.
		static public string FetchDataSetAsOptionString(string options, string order) 
			{
			Campaign rc = new Campaign();
			return rc.GetDataSetAsOptionString(options, order);
			}

		//
		// Public properties
		//
		public long Id { get; set; }
		public string Description { get; set; }
		public string ConnString { get; set; }
		public bool IsDeleted { get; set; }
		}
	}
