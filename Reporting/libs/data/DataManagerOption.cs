using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Nezasoft.Tools;

namespace Rainmaker.Data 
	{
	// Each query in the database will save its current options into one of 
	// these. There column of data will have a DataManagerColumn record
	// created for it as well.
	public class DataManagerOption : RainmakerData 
		{
		private DataManagerOption() : 
						base("datamanageroption",
								 new string []{
																"id",
																"campaignid", 
																"queryid",
																"rowlimit",
																"sortcolumn",
																"sortactive",
																"sortdirection",
																"description",
																"showcsvheaders",
																"is_named_query",
																"name"
															}, 
								 "id", 
								 "name",
			ConfigurationManager.AppSettings["RmConnectionString"]					
         //   ConfigurationManager.ConnectionStrings["ReportMaker"].ConnectionString
								 ) 
			{ 
			Id = 0;
			RowLimit = 20;
			}
				
		// Gets a set of records from the Agent table.
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
			DataManagerOption dmo = new DataManagerOption();			
			dmo.Id = (int)sdr[sdr.GetOrdinal("id")];
			dmo.CampaignId = (int)sdr[sdr.GetOrdinal("campaignid")];
			dmo.QueryId = (int)sdr[sdr.GetOrdinal("queryid")];
			dmo.RowLimit = (int)sdr[sdr.GetOrdinal("rowlimit")];
			dmo.SortColumn = (int)sdr[sdr.GetOrdinal("sortcolumn")];
			dmo.ShowCSVHeaders = (int)sdr[sdr.GetOrdinal("showcsvheaders")];
			dmo.SortActive = (int)sdr[sdr.GetOrdinal("sortactive")];
			dmo.SortDirection = (int)sdr[sdr.GetOrdinal("sortdirection")];
			dmo.Description = (string)sdr[sdr.GetOrdinal("description")];
			Records.Add(dmo);
			}

		// Gets a set of records from the Campaign table.
		static public List<RainmakerData> FetchDataSet(string options, string order) 
			{
			DataManagerOption rc = new DataManagerOption();
			return rc.GetDataSet(options, order);
			}

		public void resetHiddenColumns()
			{
			List<RainmakerData> columns = DataManagerColumn.FetchDataSet((" datamanageroption_id=" + Id), null);
			foreach(RainmakerData rd in columns)
				{
				DataManagerColumn dmc = (DataManagerColumn)rd;
				dmc.Hidden = 0;
				dmc.save();
				}
			}

		// Gets an option column. If that column does not exist it will be
		// created with default values.
		public DataManagerColumn getColumn(string columnName)
			{
			List<RainmakerData> columns = DataManagerColumn.FetchDataSet(("name='" + columnName + "' AND datamanageroption_id=" + Id), null);
			if(columns.Count >= 1)
				{
				return (DataManagerColumn)columns[0];
				}
			else
				{
				return DataManagerColumn.CreateInstance(Id, columnName);
				}
			}

		// Gets an instance associated with a campaign and query
		static public DataManagerOption GetInstance(long campaignId, long queryId, bool create)
			{
			List<RainmakerData> options = DataManagerOption.FetchDataSet(("campaignid=" + campaignId + " AND queryid=" + queryId + " AND is_named_query=0"), null);
			if(options.Count >= 1)
				{
				return (DataManagerOption)options[0];
				}
			else
				{
				return (create) ? DataManagerOption.CreateInstance(campaignId, queryId) : null;
				}
			}

		// Gets a named instance.
		static public DataManagerOption GetInstance(long campaignId, string name, bool create)
			{
			List<RainmakerData> options = DataManagerOption.FetchDataSet(("name='" + name + "' AND is_named_query=1"), null);
			if(options.Count >= 1)
				{
				return (DataManagerOption)options[0];
				}
			else
				{
				return (create) ? DataManagerOption.CreateInstance(campaignId, 0) : null;
				}
			}

		// Gets a DataManagerOption instance directly from its id
		static public DataManagerOption GetInstance(int id)
			{
			List<RainmakerData> options = DataManagerOption.FetchDataSet(("id=" + id), null);
			return (options.Count >= 1) ? (DataManagerOption)options[0] : null;
			}

		// Creates a new instance.
		static public DataManagerOption CreateInstance(long campaignId, long queryId)
			{
			DataManagerOption dmo = new DataManagerOption();
			dmo.CampaignId = campaignId;
			dmo.QueryId = queryId;
			dmo.save();
			return dmo;
			}

		// Gets a complete list of all the DataManagerOptions for the campaign
		static public string FetchDataSetAsOptionString(string options, string order, int Id) 
			{
			DataManagerOption dmo = new DataManagerOption();
			return dmo.GetDataSetAsOptionString(options, order, Id);
			}

		public void delete()
			{
			if(this.Id > 0)
				{
				SqlConnection conn = new SqlConnection(connString);
				conn.Open();
				string query = String.Format(@"DELETE FROM datamanagercolumn WHERE datamanageroption_id=" + this.Id);
				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.ExecuteNonQuery();
				query = String.Format(@"DELETE FROM datamanageroption WHERE id=" + this.Id);
				cmd = new SqlCommand(query, conn);
				cmd.ExecuteNonQuery();
				conn.Close();
				}
			this.Id = 0;
			}

		public DataManagerOption copy()
			{
			DataManagerOption dmo = CreateInstance(this.CampaignId, 0);
			dmo.IsNamedQuery = this.IsNamedQuery;
			dmo.Name = this.Name;
			dmo.RowLimit = this.RowLimit;		
			dmo.ShowCSVHeaders = this.ShowCSVHeaders;
			dmo.SortColumn = this.SortColumn;
			dmo.SortActive = this.SortActive;
			dmo.SortDirection = this.SortDirection;
			dmo.Description = this.Description;
			dmo.save();
			copyColumns((int)dmo.Id);
			return dmo;
			}

		private void copyColumns(int newId)
			{
			List<RainmakerData> columns = DataManagerColumn.FetchDataSet((" datamanageroption_id=" + Id), null);
			foreach(RainmakerData rd in columns)
				{
				DataManagerColumn dmc = (DataManagerColumn)rd;
				DataManagerColumn newDmc = DataManagerColumn.CreateInstance(newId, dmc.Name);
				newDmc.Hidden = dmc.Hidden;
				newDmc.Width = dmc.Width;
				newDmc.Name = dmc.Name;
				newDmc.save();
				}
			}

		// Saves the instance back to the database
		public void save()
			{
			if(this.Id == 0)
				{
				insert();
				}
			else
				{
				update();
				}
			}

		private void insert()
			{
			string query = String.Format(@"
				INSERT INTO datamanageroption
					(
					campaignid,
					queryid,
					rowlimit,
					sortcolumn,
					sortactive,
					sortdirection,
					description,
					showcsvheaders,
					is_named_query,
					name
					)
				VALUES
					(
					{0},
					{1},
					{2},
					{3},
					{4},
					{5},
					'{6}',
					{7},
					{8},
					'{9}'
					);
				SELECT id FROM datamanageroption WHERE id = @@IDENTITY
				",
				CampaignId, QueryId, RowLimit, SortColumn, SortActive, SortDirection, Description, ShowCSVHeaders, IsNamedQuery, Name);
			SqlConnection conn = new SqlConnection(connString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			Id = (int)cmd.ExecuteScalar();
			conn.Close();
			}

		private void update()
			{
			string query = String.Format(@"
				UPDATE datamanageroption
				SET
					campaignid={0},
					queryid={1},
					rowlimit={2},
					sortcolumn={3},
					sortactive={4},
					sortdirection={5},
					description='{6}',
					showcsvheaders={7},
					is_named_query={9},
					name='{10}'
				WHERE
					id={8}
				",
				CampaignId, QueryId, RowLimit, SortColumn, SortActive, SortDirection, Description, ShowCSVHeaders, Id, IsNamedQuery, Name);
			SqlConnection conn = new SqlConnection(connString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			cmd.ExecuteNonQuery();
			conn.Close();
			}

		//
		/// Public properties
		//
		public long Id { get; set; }
		public long CampaignId { get; set; }
		public long QueryId { get; set; }
		public int IsNamedQuery { get; set; }
		public string Name { get; set; }
		public long RowLimit { get; set; }
		public int ShowCSVHeaders { get; set; }
		public long SortColumn { get; set; }
		public long SortActive { get; set; }
		public long SortDirection { get; set; }
		public string Description { get; set; }
		}
	}
