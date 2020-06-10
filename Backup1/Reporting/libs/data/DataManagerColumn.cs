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
	public class DataManagerColumn : RainmakerData 
		{
		private DataManagerColumn() : 
						base("datamanagercolumn",
								 new string []{
																"id",
																"width", 
																"name",
																"hidden",
																"datamanageroption_id"
															}, 
								 "id", 
								 "name",
			ConfigurationManager.AppSettings["RmConnectionString"]					 
        //    ConfigurationManager.ConnectionStrings["ReportMaker"].ConnectionString
								 ) 
			{
			this.Id = 0;
			this.Width = 300;
			this.Name = "";
			this.Hidden = 0;
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
			DataManagerColumn dmc = new DataManagerColumn();			
			dmc.Id = (int)sdr[sdr.GetOrdinal("id")];
			dmc.Width = (int)sdr[sdr.GetOrdinal("width")];
			dmc.Hidden = (int)sdr[sdr.GetOrdinal("hidden")];
			dmc.Name = (string)sdr[sdr.GetOrdinal("name")];
			dmc.DataManagerOptionId = (int)sdr[sdr.GetOrdinal("datamanageroption_id")];
			Records.Add(dmc);
			}

		// Gets a set of records from the Campaign table.
		static public List<RainmakerData> FetchDataSet(string options, string order) 
			{
			DataManagerColumn rc = new DataManagerColumn();
			return rc.GetDataSet(options, order);
			}

		// Creates a new instance.
		static public DataManagerColumn CreateInstance(long dataManagerOptionId, string name)
			{
			DataManagerColumn dmc = new DataManagerColumn();
			dmc.DataManagerOptionId = dataManagerOptionId;
			dmc.Name = name;
			dmc.save();
			return dmc;
			}

		public void delete()
			{
			if(this.Id > 0)
				{
				string query = String.Format(@"DELETE FROM datamanagercolumn WHERE id=" + this.Id);
				SqlConnection conn = new SqlConnection(connString);
				conn.Open();
				SqlCommand cmd = new SqlCommand(query, conn);
				cmd.ExecuteNonQuery();
				conn.Close();
				}
			this.Id = 0;
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
				INSERT INTO datamanagercolumn
					(
					width,
					name,
					hidden,
					datamanageroption_id
					)
				VALUES
					(
					{0},
					'{1}',
					{2},
					{3}
					);
				SELECT id FROM datamanagercolumn WHERE id = @@IDENTITY
				",
				(int)Width, Name, (int)Hidden, (int)DataManagerOptionId);
			SqlConnection conn = new SqlConnection(connString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			Id = (int)cmd.ExecuteScalar();
			conn.Close();
			}

		private void update()
			{
			string query = String.Format(@"
				UPDATE datamanagercolumn
				SET
					width={0},
					name='{1}',
					hidden={2},
					datamanageroption_id={3}
				WHERE
					id={4}
				",
				(int)Width, Name, (int)Hidden, (int)DataManagerOptionId, (int)Id);
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
		public long Width { get; set; }
		public long Hidden { get; set; }
		public long DataManagerOptionId { get; set; }
		public string Name { get; set; }
		}
	}