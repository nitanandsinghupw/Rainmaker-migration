<%@ WebHandler Language="C#" Debug="true" Class="ReportHandler" %>
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections.Generic;
using System.Web;
using System.Web.Script.Serialization;
using Rainmaker.Reports;
using Rainmaker.Data;
using Nezasoft.Tools;

// You can map this to URL like this:
//
//  <system.web>
//      <urlMappings enabled="true">
//          <add url="~/Default.aspx" mappedUrl="~/Handler.ashx"/>
//      </urlMappings>
//      ...
//
public class ReportHandler : IHttpHandler 
	{
	// Essentially this acts like a giant switchboard. It looks at the request
	// and then called the correct library to make it happen.
	public void ProcessRequest(HttpContext context)
		{
		// Handle the request
		extractCommonReportOptions(context);
		int actionType;
		Int32.TryParse(context.Request.Params["actionType"], out actionType);
		bool noError = decodeActionRequest(context, (ActionType)actionType);
		// Then send the response
		if(actionType == 6)
			{
			int queryID;
			Int32.TryParse(context.Request.Params["queryID"], out queryID);
			QueryRunner queryRunner = new QueryRunner();
			context.Response.ContentType = "text/csv";
			context.Response.AddHeader("content-disposition", "attachment; filename=data.csv");
			context.Response.Write(queryRunner.CreateCSVView(campaign, queryID));
			string deleteDownload = (string)context.Request.Params["deleteDownload"];
			if(deleteDownload == "DELETE")
				{
				queryRunner.deleteQuery(campaign, queryID);
				}
			//context.Response.WriteFile("~/test.png");
			}
		else
			{
			context.Response.ContentType = "text/plain";
            context.Response.AddHeader("Cache-Control", "no-cache");
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			context.Response.Write(serializer.Serialize(ajaxResponse));
			}
		}	
	
	// Looks at the current action and decide how to handle it.
	private bool decodeActionRequest(HttpContext context, ActionType actionType)
		{
		bool noError;
		int value;
		switch(actionType) 
			{
			case ActionType.CREATE_REPORT:
				noError = createReport(context);
				break;
			case ActionType.GET_SHIFT_LIST:
				Int32.TryParse(context.Request.Params["allCampaignsList"], out value);
				noError = getCampaignList((value > 0) ? true : false);
				break;
			case ActionType.GET_AGENT_LIST:
				Int32.TryParse(context.Request.Params["allAgentsList"], out value);
				noError = getAgentList((value > 0) ? true : false);
				break;
			case ActionType.GET_QUERY_LIST:
				Int32.TryParse(context.Request.Params["allQueryList"], out value);
				noError = getQueryList((value > 0) ? true : false, context);
				break;
			case ActionType.CREATE_QUERY_VIEW:
				Int32.TryParse(context.Request.Params["queryID"], out queryID);
				noError = createQueryView(context, queryID, sortOn, sortColumn, sortDirection);
				break;
			case ActionType.UPDATE_OPTION:
				Int32.TryParse(context.Request.Params["queryID"], out queryID);
				Int32.TryParse(context.Request.Params["rowLimit"], out rowLimit);
				Int32.TryParse(context.Request.Params["showCSVHeaders"], out showCSVHeaders);
				Int32.TryParse(context.Request.Params["sortOn"], out sortOn);
				Int32.TryParse(context.Request.Params["sortColumn"], out sortColumn);
				Int32.TryParse(context.Request.Params["sortDirection"], out sortDirection);
				noError = updateOption();
				break;
			case ActionType.UPDATE_COLUMN_HIDDEN:
				Int32.TryParse(context.Request.Params["queryID"], out queryID);
				columnName = context.Request.Params["columnName"];
				Int32.TryParse(context.Request.Params["hidden"], out hidden);
				noError = updateColumnHidden();
				break;
			case ActionType.UPDATE_COLUMN_WIDTH:
				Int32.TryParse(context.Request.Params["queryID"], out queryID);
				columnName = context.Request.Params["columnName"];
				Int32.TryParse(context.Request.Params["width"], out width);
				noError = updateColumnWidth();
				break;
			case ActionType.RESET_HIDDEN:
				Int32.TryParse(context.Request.Params["queryID"], out queryID);
				noError = resetHiddenColumns();
				break;
			case ActionType.UPDATE_FIELD:
				Int32.TryParse(context.Request.Params["UniqueKey"], out value);
				string colName = context.Request.Params["colName"];
				string contents = context.Request.Params["value"];
				string dataType = context.Request.Params["dataType"];
				noError = UpdateCampaignField(value, colName, contents, dataType);
				break;
			case ActionType.GET_SETTINGS_LIST:
				noError = getNamedSettingsList(0);
				break;
			case ActionType.CREATE_SETTINGS:
				Int32.TryParse(context.Request.Params["queryID"], out queryID);
				string name = context.Request.Params["name"];
				noError = createNamedSettings(name);
				break;
			case ActionType.DELETE_SETTINGS:
				Int32.TryParse(context.Request.Params["nameSettingId"], out value);
				noError = deleteNamedSettings(value);
				break;
			case ActionType.SAVE_SETTINGS:
				Int32.TryParse(context.Request.Params["queryID"], out queryID);
				Int32.TryParse(context.Request.Params["nameSettingId"], out value);
				noError = saveSettings(value);
				break;
			case ActionType.LOAD_SETTINGS:
				Int32.TryParse(context.Request.Params["queryID"], out queryID);
				Int32.TryParse(context.Request.Params["nameSettingId"], out value);
				noError = loadSettings(value);
				break;
			default:
				noError = false;
				break;
			}
		return noError;
		}

	private bool loadSettings(int id)
		{
		DataManagerOption dmo = DataManagerOption.GetInstance(campaignID, queryID, false);
		if(dmo != null)
			{
			dmo.delete();
			dmo = DataManagerOption.GetInstance(id);
			dmo = dmo.copy();
			dmo.QueryId = queryID;
			dmo.Name = "";
			dmo.IsNamedQuery = 0;
			dmo.save();
			getNamedSettingsList((int)dmo.Id);
			ajaxResponse.message = "Loaded Name Settings";
			}
		return true;
		}

	private bool saveSettings(int id)
		{
		DataManagerOption dmo = DataManagerOption.GetInstance(id);
		string name = dmo.Name;
		dmo.delete();
		return createNamedSettings(name);
		}

	private bool deleteNamedSettings(int id)
		{
		DataManagerOption dmo = DataManagerOption.GetInstance(id);
		dmo.delete();
		getNamedSettingsList(0);
		ajaxResponse.message = "Deleted Name Settings";
		return true;
		}

	private bool getNamedSettingsList(int Id)
		{
		ajaxResponse = new AjaxResponse();
		ajaxResponse.message = "Named Settings List Ready";
		ajaxResponse.contents = DataManagerOption.FetchDataSetAsOptionString("is_named_query=1 AND campaignid=" + campaignID, null, Id);
		ajaxResponse.contents = "<option value=\"0\">Select</option>" + ajaxResponse.contents;
		ajaxResponse.error = false;
		return true;
		}

	private bool createNamedSettings(string name)
		{
		DataManagerOption dmo = DataManagerOption.GetInstance(campaignID, queryID, false);
		if(dmo != null)
			{
			dmo = dmo.copy();
			dmo.QueryId = 0;
			dmo.Name = name;
			dmo.IsNamedQuery = 1;
			dmo.save();
			//getNamedSettingsList((int)dmo.Id);
			getNamedSettingsList(0);
			ajaxResponse.message = "Created Name Settings";
			}
		return true;
		}

	// Updates the options for a given campaign and query
	private bool updateOption()
		{
		DataManagerOption dmo = DataManagerOption.GetInstance(campaignID, queryID, true);
		dmo.RowLimit = rowLimit;
		dmo.SortActive = sortOn;
		if(sortColumn < 1) sortColumn = 1;
		dmo.SortColumn = sortColumn;
		dmo.SortDirection = sortDirection;
		dmo.ShowCSVHeaders = showCSVHeaders;
		dmo.save();
		return true;
		}

	private bool updateColumnWidth()
		{
		DataManagerOption dmo = DataManagerOption.GetInstance(campaignID, queryID, true);
		DataManagerColumn dmc = dmo.getColumn(columnName);
		dmc.Width = width;
		dmc.save();
		return true;
		}

	private bool resetHiddenColumns()
		{
		DataManagerOption dmo = DataManagerOption.GetInstance(campaignID, queryID, true);
		dmo.resetHiddenColumns();
		return true;
		}

	private bool updateColumnHidden()
		{
		DataManagerOption dmo = DataManagerOption.GetInstance(campaignID, queryID, true);
		DataManagerColumn dmc = dmo.getColumn(columnName);
		dmc.Hidden = hidden;
		dmc.save();
		return true;
		}

	// Updates specific row and column value in the Campaign table.
	private bool UpdateCampaignField(int uniqueKey, string colName, string contents, string dataType)
		{
		ajaxResponse = new AjaxResponse();
		QueryRunner queryRunner = new QueryRunner();
		queryRunner.SaveField(uniqueKey, colName, contents, dataType, campaign.ConnString);
		ajaxResponse.contents = queryRunner.contents;
		ajaxResponse.error = false;
		return true;
		}
	
	// Gets the HTML encoded list of in the current campaigns on the server
	private bool getCampaignList(bool allCampaigns)
		{
		ajaxResponse = new AjaxResponse();
		ajaxResponse.message = "Campaign List Ready";
		ajaxResponse.contents = Campaign.FetchDataSetAsOptionString("IsDeleted=0", null);
		if(allCampaigns)
			{
			ajaxResponse.contents = "<option value=\"0\">All Campaigns</option>" + ajaxResponse.contents;
			}
		ajaxResponse.error = false;
		return true;
		}

	// Gets the HTML encoded list of in the current agents on the server
	private bool getAgentList(bool allAgents)
		{
		ajaxResponse = new AjaxResponse();
		ajaxResponse.message = "Agent List Ready";
		ajaxResponse.contents = Agent.FetchDataSetAsOptionString(campaign, null, null);
		if(allAgents)
			{
			ajaxResponse.contents = "<option value=\"0\">All Agents</option>" + ajaxResponse.contents;
			}
		ajaxResponse.error = false;
		return true;
		}

	// Gets the HTML encoded list of in the current queries for the target campaign
	private bool getQueryList(bool allQueries, HttpContext context)
		{
		ajaxResponse = new AjaxResponse();
		ajaxResponse.message = "Query List Ready";
		ajaxResponse.contents = Query.FetchDataSetAsOptionString(null, null, campaign);
		if(allQueries)
			{
			ajaxResponse.contents = "<option value=\"0\">All Queries</option>" + ajaxResponse.contents;
			}
		ajaxResponse.error = false;
		return true;
		}
	
	private bool createQueryView(HttpContext context, int queryID, int sortOn, int sortColumn, int sortDirection)
		{
		DataManagerOption dmo = DataManagerOption.GetInstance(campaign.Id, queryID, true);
		ajaxResponse = new AjaxResponse();
		ajaxResponse.message = "Query View Ready";
		QueryRunner queryRunner = new QueryRunner();
		queryRunner.columnWidth = 300;
		queryRunner.RunQuery(campaign, queryID, (int)dmo.SortActive, (int)dmo.SortColumn, (int)dmo.SortDirection);
		ajaxResponse.contents = queryRunner.contents;
		ajaxResponse.tablePixelWidth = queryRunner.tableWidth.ToString();
		ajaxResponse.tableWidth = queryRunner.colCount.ToString();
		ajaxResponse.tableHeight = queryRunner.rowCount.ToString();
		ajaxResponse.tableColumns = queryRunner.tableColumns;
		ajaxResponse.rowLimit = dmo.RowLimit;
		ajaxResponse.sortOn = dmo.SortActive.ToString();
		ajaxResponse.showCSVHeaders = dmo.ShowCSVHeaders;
		ajaxResponse.rowKeys = queryRunner.rowKeys;
		ajaxResponse.rowCount = (queryRunner.rowCount-1);
		ajaxResponse.sqlOptions = queryRunner.queryOptions;
		ajaxResponse.error = false;
		return true;
		}
	
	// Creates a new report...
	private bool createReport(HttpContext context)
		{
		int value;
		long agentID;
		bool noError = Int32.TryParse(context.Request.Params["reportType"], out value);
		ReportType reportType = (ReportType)value;
		GenericReport report = null;
		ajaxResponse = new AjaxResponse();
		switch(reportType) 
			{
			case ReportType.SHIFT_REPORT:
				report = new ShiftReport();
				ajaxResponse.message = "Shift Report Ready";
				ajaxResponse.contents = report.encodeHTMLReport(campaign, startDate, endDate, startTime, endTime);
				ajaxResponse.error = report.error;
				break;
			case ReportType.CALL_HISTORY_BY_PHONE_REPORT:
				CallHistoryByPhoneReport phoneReport = new CallHistoryByPhoneReport();
				phoneReport.PhoneNumber = context.Request.Params["phonenumber"];
				ajaxResponse.message = "Call History By Phone Report Ready";
				ajaxResponse.contents = phoneReport.encodeHTMLReport(campaign, startDate, endDate, startTime, endTime);
				ajaxResponse.error = phoneReport.error;
				break;
			case ReportType.CALL_HISTORY_BY_AGENT_REPORT:
				CallHistoryByAgentReport agentReport = new CallHistoryByAgentReport();
				Int64.TryParse(context.Request.Params["agentid"], out agentID);
				agentReport.AgentID = agentID;
				ajaxResponse.message = "Call History By Agent Report Ready";
				ajaxResponse.contents = agentReport.encodeHTMLReport(campaign, startDate, endDate, startTime, endTime);
				ajaxResponse.error = agentReport.error;
				break;
			case ReportType.SUMMARIZED_RESULTS_REPORT:
				SummarizedAgentResultsReport summaryReport = new SummarizedAgentResultsReport();
				Int64.TryParse(context.Request.Params["agentid"], out agentID);
				summaryReport.AgentID = agentID;
				ajaxResponse.message = "Summarized Agents Dialer Results Report Ready";
				ajaxResponse.contents = summaryReport.encodeHTMLReport(campaign, startDate, endDate, startTime, endTime);
				ajaxResponse.error = summaryReport.error;
				break;
			case ReportType.AGENTS_DIALER_RESULTS_REPORT:
				AgentsDialerResultsReport dialerResultsReport = new AgentsDialerResultsReport();
				Int64.TryParse(context.Request.Params["agentid"], out agentID);
				dialerResultsReport.AgentID = agentID;
				dialerResultsReport.CampaignID = campaignID;
				ajaxResponse.message = "Agents Dialer Results Report Ready";
				ajaxResponse.contents = dialerResultsReport.encodeHTMLReport(campaign, startDate, endDate, startTime, endTime);
				ajaxResponse.error = dialerResultsReport.error;
				break;
			default:
				noError = false;
				break;
			}
		return noError;
		}

	// Reads POST options that can be present for any incomming request
	private void extractCommonReportOptions(HttpContext context) 
		{
		Int64.TryParse(context.Request.Params["campaignName"], out campaignID);
		string campaignName = context.Request.Params["campaignName"];
		if(campaignID > 0)
			{
			Campaign loader = new Campaign();
			List<RainmakerData> campaigns = loader.GetDataSet("CampaignID=" + campaignName, null);
			campaign = (Campaign)campaigns.GetRange(0, 1).ToArray()[0];
			}
		else
			{
			campaign = null;
			}
		startDate = context.Request.Params["startDate"];
		endDate = context.Request.Params["endDate"];
		startTime = context.Request.Params["startTime"];
		endTime = context.Request.Params["endTime"];
		}
		
	//
	// Private variables
	//
	private AjaxResponse ajaxResponse;
	private long campaignID;
	private int queryID;
	private int sortColumn;
	private int sortOn;
	private int sortDirection;
	private int hidden;
	private int showCSVHeaders;
	private int rowLimit;
	private int width;
	private string columnName;
	private Campaign campaign;
	private string startDate;
	private string endDate;
	private string startTime;
	private string endTime;
	//
	// Getters/Setters
	//
	public bool IsReusable 
		{
		get { return false; }
		}
	}
