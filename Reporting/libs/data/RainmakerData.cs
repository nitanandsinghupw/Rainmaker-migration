using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Configuration;

namespace Rainmaker.Data 
	{
	public delegate void DataLoaderDelegate(SqlDataReader sdr);

	// Provides a thin wrapper around the database and provides several
	// utility methods to help translate the information in the DB
	// to HTML
	abstract public class RainmakerData 
		{
		// Exists to make the C# compiler happy :)
		public RainmakerData() 
			{
			throw new NotSupportedException("The 0 argument constructor is not supported!");
			}

		// Sets the class up to start working with the Rainmaker data
		public RainmakerData(string tableName, string []columnNames, string idField, string titleField, string connString) 
			{
			this.connString = connString;
			TableName = tableName;
			ColumnNames = columnNames;
			IdField = idField;
			TitleField = titleField;
			}

		// Call this to get a set of data for this table
		abstract public List<RainmakerData> GetDataSet(string options, string order);

		// Runs a SQL Query and loads data from the database. For each record found the
		// 'callback' method will be triggered giving the a chance to handle it. If the
		// child ignores the callback then nothing will be done.
		protected void DataLoader(DataLoaderDelegate loaderDelegate, string options, string order) 
			{
			string query = CreateSelect(options, order);
			SqlConnection conn = new SqlConnection(connString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			List<RainmakerData> campaigns = new List<RainmakerData>();
			while(sdr.Read()) 
				{
				loaderDelegate(sdr);
				}
			conn.Close();
			}

		// Queries the database and creates an HTML option list from the data
		public string GetDataSetAsOptionString(string options, string order) 
			{
			string query = CreateSelect(options, order);
			SqlConnection conn = new SqlConnection(connString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			string html = "";
			while(sdr.Read())
				{
				html += String.Format
										(
										"<option value=\"{0}\">{1}</option>", 
										sdr[sdr.GetOrdinal(IdField)],
										sdr[sdr.GetOrdinal(TitleField)]
										);
				}
			conn.Close();
			return html; 
			}

		// Queries the database and creates an HTML option list from the data
		public string GetDataSetAsOptionString(string options, string order, int selectedId) 
			{
			string query = CreateSelect(options, order);
			SqlConnection conn = new SqlConnection(connString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			string html = "";
			while(sdr.Read())
				{
				int id = (int)sdr[sdr.GetOrdinal(IdField)];
				if(id == selectedId)
					{
					html += String.Format
										(
										"<option value=\"{0}\" selected>{1}</option>", 
										sdr[sdr.GetOrdinal(IdField)],
										sdr[sdr.GetOrdinal(TitleField)]
										);
					}
				else
					{
					html += String.Format
										(
										"<option value=\"{0}\">{1}</option>", 
										sdr[sdr.GetOrdinal(IdField)],
										sdr[sdr.GetOrdinal(TitleField)]
										);
					}
				}
			conn.Close();
			return html; 
			}

		// Creates a select statement for this type
		protected string CreateSelect(string options, string order) 
			{
			string query = "SELECT \n";
			bool first = true;
			foreach(string columnName in ColumnNames) 
				{
				if(!first) query += ", ";
				query += columnName;
				first = false;
				}
			query += " FROM " + TableName + " \n";
			if(options != null) 
				{
				query += "WHERE " + options;
				}
			if(order != null) 
				{
				query += "ORDER BY " + order;
				}
			return query;
			}

		//
		// Private data
		//
		protected List<RainmakerData> Records { get; set; }
		public string []ColumnNames { get; set; }
		public string IdField { get; set; }
		public string TitleField { get; set; }
		public string TableName { get; set; }
		protected string connString;
		}
	}
