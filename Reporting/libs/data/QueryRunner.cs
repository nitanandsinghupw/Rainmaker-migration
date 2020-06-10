using System;
using System.Data;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using Nezasoft.Tools;

namespace Rainmaker.Data 
	{
	public class QueryRunner
		{
		public void deleteQuery(Campaign campaign, int queryID)
			{
			Query query = new Query(campaign, queryID);
			conn = new SqlConnection(campaign.ConnString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(createDelete(query.QueryCondition), conn);
			cmd.ExecuteNonQuery();
			conn.Close();
			}

		//
		// When called, this will execute the matching query in the current campaign
		// and build the information required for the QueryViewer to interact with it.
		//
		public void RunQuery(Campaign campaign, int queryID, int sortOn, int sortColumn, int sortDirection)
			{
			this.campaign = campaign;
			dmo = DataManagerOption.GetInstance(campaign.Id, queryID, true);
			Query query = new Query(campaign, queryID);
			conn = new SqlConnection(campaign.ConnString);
			conn.Open();
			string queryStr = setupQuerySorting(query.QueryCondition, sortOn, sortColumn, sortDirection);
			SqlCommand cmd = new SqlCommand(queryStr, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			contents = GetColumnHeaders(sdr);
			rowCount = 1;
			rowKeys = "rowKeys=new Array();\n";
			contents += "<tbody>\n";
			string curRow = "";
			while(sdr.Read() && (rowCount <= dmo.RowLimit))
				{
				// Operate with a 1 row delay so we can detect the last row
				contents += curRow;
				curRow = GetRow(sdr);
				}
			contents += "</tbody><tfoot>";
			contents += curRow;
			contents += "</tfoot>";
			sdr.Close();
			cmd = new SqlCommand(countRowsQuery(query.QueryCondition), conn);
			rowCount = (int)cmd.ExecuteScalar();
			conn.Close();
			}

		private string countRowsQuery(string query)
			{
			string options = " " + query.Substring(query.IndexOf("FROM"));
			string vars = (" UniqueKey, PhoneNum, " + getColumnList());
			return "SELECT count(*) " + options;
			}

		// If sorting is turned on (sortOn==1) this will ensure that the database sorts
		// the data before returning it.
		//
		// 1 - asc sort, anything is desc
		private string setupQuerySorting(string query, int sortOn, int sortColumn, int sortDirection)
			{
			query = reformatQuery(query);
			if(sortOn != 1) return query;
			return query + " ORDER BY " + sortColumn + ((sortDirection==1) ? " asc" : " desc");
			}

		// Reformats the query so it grabs every field and places the the UniqueID
		// and PhoneNum first.
		private string reformatQuery(string query)
			{
			queryOptions = " " + query.Substring((query.IndexOf("WHERE") + 5));
			string options = " " + query.Substring(query.IndexOf("FROM"));
			string vars = (" UniqueKey, PhoneNum, " + getColumnList());
			return "SELECT " + vars + options;
			}
	
		// Creates a DELETE statement based on the current query
		private string createDelete(string query)
			{
			return "DELETE " + query.Substring(query.IndexOf("FROM"));
			}

		private string getColumnList()
			{
			SqlCommand cmd = new SqlCommand("select name from syscolumns where id=object_id('campaign')", conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			string columns = "";
			while(sdr.Read())
				{
				string name = (string)sdr[0];
				if((name != "PhoneNum") && (name != "UniqueKey"))
					{
					if(columns.Length > 0)
						columns += ", ";
					columns += name;
					}
				}
			sdr.Close();
			return columns;
			}

		// Creates a CSV view of the data
		//public string CreateCSVView(int queryID, string connString)
		public string CreateCSVView(Campaign campaign, int queryID)
			{
			dmo = DataManagerOption.GetInstance(campaign.Id, queryID, true);
			Query query = new Query(campaign, queryID);
			conn = new SqlConnection(campaign.ConnString);
			conn.Open();
			string queryStr = reformatQuery(query.QueryCondition);
			SqlCommand cmd = new SqlCommand(queryStr, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			string csv = "";
			if(dmo.ShowCSVHeaders == 1)
				csv = GetCSVColumnHeaders(sdr);
			while(sdr.Read()) 
				{
				csv += GetCSVRow(sdr);
				}
			conn.Close();
			return csv;
			}

		private string makeSQLSafe(string value)
			{
			return value.Replace("'", "''");
			}

		//
		// Saves a changed (or unchanged) field back to the database location it came from.
		//
		// NOTE: Should there be more type checking going on here?
		public bool SaveField(int uniqueID, string colName, string value, string dataType, string connString)
			{
			string sql;
			if((dataType.CompareTo("System.String") == 0) ||
			   (dataType.CompareTo("System.DateTime") == 0))
				{
				value = makeSQLSafe(value);
				sql = String.Format(@"
					UPDATE campaign
					SET
						{0}='{1}'
					WHERE
						UniqueKey={2}
					", colName, value, uniqueID);
				}
			else
				{
				sql = String.Format(@"
					UPDATE campaign
					SET
						{0}={1}
					WHERE
						UniqueKey={2}
					", colName, value, uniqueID);
				}
			conn = new SqlConnection(connString);
			conn.Open();
//contents = sql;
			SqlCommand cmd = new SqlCommand(sql, conn);
			cmd.ExecuteNonQuery();
			conn.Close();
			return true;
			}

		private string GetRow(SqlDataReader sdr)
			{
			ToggleBackgroundColor();
			string row = "<tr>";
			for (int i = 0; i < sdr.FieldCount; i++)
				{
				string dataType = (string)sdr.GetFieldType(i).ToString();
				string value = (string)sdr[i].ToString();
				value = value.Replace("'", "");
				if(i == uniqueKeyCol)
					{
					// Just record this value so it can be tracked later.
					rowKeys += String.Format("rowKeys[{0}] = {{id: {1}}};\n", (rowCount-1), value);
					}
				else
					{
					string colName = sdr.GetName(i);
					string chooseAction = String.Format(@"onclick=""chooseCell({0}, {1})""", i, rowCount);
					if(colName == "PhoneNum") chooseAction = "";
					DataManagerColumn dmc = dmo.getColumn(colName);
					if(dmc.Hidden != 1)
						{
						// Add this column to the data set that will be displayed.
						if(i == 1)
							{
							row += String.Format("<td id=\"{0}x{1}\" dataType=\"{3}\" class=\"left\" style=\"background-color: {4}\" {5}>{2}</td>", i, rowCount, value, dataType, backgroundColor, chooseAction);
							}
						else if (i == (sdr.FieldCount - 1))
							{
							row += String.Format("<td id=\"{0}x{1}\" dataType=\"{3}\" class=\"right\" style=\"background-color: {4}\" {5}>{2}</td>", i, rowCount, value, dataType, backgroundColor, chooseAction);
							}
						else
							{
							row += String.Format("<td id=\"{0}x{1}\" dataType=\"{3}\" style=\"background-color: {4}\" {5}>{2}</td>", i, rowCount, value, dataType, backgroundColor, chooseAction);
							}
						}
					}
				}
			row += "</tr>\n\n";
			rowCount++;
			return row;
			}

		private void ToggleBackgroundColor()
			{
				backgroundColor = (backgroundColor == "#FFFFFF") ? "#EEEEEE" : "#FFFFFF";
			}

		private string GetCSVRow(SqlDataReader sdr)
			{
			string csv = "";
			for (int i = 0; i < sdr.FieldCount; i++)
				{
				string colName = sdr.GetName(i);
				DataManagerColumn dmc = dmo.getColumn(colName);
				if(dmc.Hidden != 1)
					{
					string value = (string)sdr[i].ToString();
// THIS MAY BE BAD BUT IT HIDES COMMAS FROM TH OUTPUT
					value = value.Replace(",", "");
					if(csv.Length > 0) csv += ", ";
					csv += String.Format("{0}", value);
					}
				}
			csv += "\n";
			return csv;
			}

		private string GetColumnHeaders(SqlDataReader sdr)
			{
			tableWidth = 0;
			tableColumns = "tableCols=new Array();\n";
			string header = "<thead><tr>";
			for (colCount = 0; colCount < sdr.FieldCount; colCount++)
				{
				string colName = sdr.GetName(colCount);
				DataManagerColumn dmc = dmo.getColumn(colName);
				if(colName.ToLower().CompareTo("uniquekey") == 0)
					{
					uniqueKeyCol = 0;
					}
				else 
					{
					if(dmc.Hidden != 1)
						{
						tableWidth += columnWidth;
						CreateTableColumnDefinition(colName);
						header += HeaderCol(sdr, dmc);
						}
					}
				}
			header += "</tr></thead>\n";
			return header;
			}

		private string GetCSVColumnHeaders(SqlDataReader sdr)
			{
			string csv = "";
			for(colCount = 0; colCount < sdr.FieldCount; colCount++)
				{
				string colName = sdr.GetName(colCount);
				DataManagerColumn dmc = dmo.getColumn(colName);
				if(dmc.Hidden != 1)
					{
					if(csv.Length > 0) csv += ", ";
					csv += String.Format("{0}", colName);
					}
				}
			csv += "\n";
			return csv;
			}

		// This creates the Javascript datastructures that will be used to track
		// table information on the client side.
		private void CreateTableColumnDefinition(string colName)
			{
			tableColumns += String.Format("tableCols[{0}] = {{width: {1}, direction: 'dsc', name: '{2}'}};\n", colCount, columnWidth, colName);
			}

		private string HeaderCol(SqlDataReader sdr, DataManagerColumn dmc)
			{
			string sortMsg = "";
			if(colCount == (dmo.SortColumn-1) && (dmo.SortActive == 1))
				{
				if(dmo.SortDirection == 1)
					{					
					sortMsg = "<div class='columnSortDirection'><img src='resources/up.gif' width='15px'/></div>";
					}
				else
					{
					sortMsg = "<div class='columnSortDirection'><img src='resources/down.gif' width='15px'/></div>";					
					}
				}
			columnWidth = (int)dmc.Width;
			if((colCount == (sdr.FieldCount - 1)))
				{
				return String.Format(@"
						<td id=""sizer{0}"" class=""gridHeader"" style=""width: {2}px"">
							<table style=""width: 100%; background-color: #CCCCCC; border: 0px solid black"" cellspacing=""0"" cellpadding=""0"">
							<tr>
								<td style=""background-color: #CCCCCC; width: 30px""><input type=""checkbox"" style=""width: 0px; height: 0px; margin-left: 10px"" onclick=""hideColumn('{3}')""/></td>
								<td style=""background-color: #CCCCCC; padding-left: 5px"" onclick=""sortByColumn({0})"">{3}</td>
								<td style=""background-color: #CCCCCC"">{4}</td>
								<td style=""background-color: #CCCCCC"">&nbsp</td>
							</tr>
							</table>
						</td>",
						colCount, colCount, columnWidth, dmc.Name, sortMsg);
				}
			else
				{
				return String.Format(@"
						<td id=""sizer{0}"" class=""gridHeader"" style=""width: {2}px"">
							<table style=""width: 100%; background-color: #CCCCCC; border: 0px solid black"" cellspacing=""0"" cellpadding=""0"">
							<tr>
								<td style=""background-color: #CCCCCC; width: 30px""><input type=""checkbox"" style=""width: 0px; height: 0px; margin-left: 10px"" onclick=""hideColumn('{3}')""/></td>
								<td style=""background-color: #CCCCCC; padding-left: 5px"" onclick=""sortByColumn({0})"">{3}</td>
								<td style=""background-color: #CCCCCC"">{4}</td>
								<td class=""columnDragger"" style=""background-color: #666666"" onmousedown=""resizeColumnStart(event, '{0}', '{1}')"">&nbsp</td>
							</tr>
							</table>
						</td>",
						colCount, (colCount+1), columnWidth, dmc.Name, sortMsg);
				}
			}

		// Instance variables
		private SqlConnection conn;
		private DataManagerOption	dmo;
		private string backgroundColor = "#CCCCCC";
		public int colCount { get; set; }
		public int rowCount { get; set;  }
		public int tableWidth { get; set; }
		public string contents { get; set; }
		public string tableColumns { get; set; }
		public string rowKeys { get; set; }
		public string queryOptions { get; set; }
		public int columnWidth { get; set; }
		public int uniqueKeyCol { get; set; }
		public int sortOn { get; set; }
		private Campaign campaign;
		}
	}