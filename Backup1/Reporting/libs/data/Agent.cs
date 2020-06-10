using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web;
using Nezasoft.Tools;

namespace Rainmaker.Data 
	{
	// Represents information about one agent. Some of this information is
	// generated dynamically from information in the DB.
	public class Agent : RainmakerData 
		{
		// connot directly instantiate
		private Agent(Campaign campaign) : 
						base("agent",
								 new string []{
																"AgentID", 
																"AgentName"
															}, 
								 "AgentID", 
								 "AgentName",
			 ConfigurationManager.AppSettings["RmConnectionString"]					 
           // ConfigurationManager.ConnectionStrings["ReportMaker"].ConnectionString
								 ) 
			{
			this.campaign = campaign;
			}

		public Agent(Campaign campaign, long agentID) : 
						base("agent",
								 new string []{
																"AgentID", 
																"AgentName"
															}, 
								 "agent", 
								 "AgentName",
			 ConfigurationManager.AppSettings["RmConnectionString"]					 
          //  ConfigurationManager.ConnectionStrings["ReportMaker"].ConnectionString
								 )
			{
			this.campaign = campaign;
			AgentID = agentID;
			if(AgentID < 1)
				{
				AgentName = "Summary";
				}
			else
				{
				ReadAgentInfo();
				}
			LoadResultCodes();
			} 
		
		// Gets a set of records from the Agent table.
		static public string FetchDataSetAsOptionString(Campaign campaign, string options, string order) 
			{
			Agent rc = new Agent(campaign);
			return rc.GetDataSetAsOptionString(options, order);
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
			Agent a = new Agent(campaign);
			a.AgentName = (string)sdr[1];
			Records.Add(a);
			}
				
		// This creates a list of Agent records by summarizing the data
		// in the 'agentStats' and 'callEvents' list. It is the only way
		// get an instance of Agent.
		//
		// NOTE: The first agent in the list of agents returned is a special
		// case. It contains a summary of all the agents in the set.
		public static List<Agent> SortAgents(Campaign campaign, List<AgentStat> agentStats, List<CallEvent> callEvents) 
			{
			List<Agent> agents = CreateAgents(campaign, agentStats);
			CreateCallResults(agents, callEvents);
			FinalizeAgents(agents);
			return agents;
			}

		//
		// Adds the 'srcAgent' data to the current agent record and recalcuates
		// the agents runtime statistics.
		//
		public void Add(Agent srcAgent)
			{
			// Merge the times
			WaitTime += srcAgent.WaitTime;
			TalkTime += srcAgent.TalkTime;
			PauseTime += srcAgent.PauseTime;
			WrapTime += srcAgent.WrapTime;
			Dials += srcAgent.Dials;
			TotalCalls += srcAgent.TotalCalls;
			Presentations += srcAgent.Presentations;
			Leads += srcAgent.Leads;
			Drops += srcAgent.Drops;
			// Recalculate the agent dialing information
			List<Agent> fakeAgentsList = new List<Agent>();
			fakeAgentsList.Add(this);
			FinalizeAgents(fakeAgentsList);
			}

		private void ReadAgentInfo() 
			{
			string query = CreateAgentQuery();
            SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["RmConnectionString"]);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			if(sdr.Read()) 
				{
				AgentName = (string)sdr[0];
				}
			sdr.Close();
			conn.Close();
			}

		private string CreateAgentQuery() 
			{
			string query =  "SELECT ";
						 query += "  agentname ";
						 query += "FROM Agent WHERE ";
						 query += " agentid=" + AgentID; 
			return query; 
			}

		private static void FinalizeAgents(List<Agent> agents) 
			{
			foreach(Agent agent in agents) 
				{
				agent.CleanResultCodes();
				agent.CalculateDialingStats();
				}
			}

		private static void CreateCallResults(List<Agent> agents, List<CallEvent> callEvents) 
			{
			Agent summary = agents[0];
			summary.Dials = callEvents.Count;
			foreach(CallEvent callEvent in callEvents) 
				{
				Agent agent = FindAgent(agents, callEvent.AgentID);
				if(agent != null) 
					{
					ResultCode resultCode = agent.FindResultCode(callEvent.ResultCodeID);
					if(resultCode != null) resultCode.Count++;
					ResultCode masterResultCode = summary.FindResultCode(callEvent.ResultCodeID);
					if(masterResultCode != null) masterResultCode.Count++;
					agent.TotalCalls++;
					summary.TotalCalls++;
					} 
				else 
					{
					summary.CountCommonResults(callEvent);
					}
				}
			}

		private void CountCommonResults(CallEvent callEvent) 
			{
			switch(callEvent.ResultCodeID) {
						case 1: // Answering Machine
								this.AnsweringMachines++;
								break;
						case 2: // Busy
								this.Busys++;
								break;
						case 3: // Operatorintercept
								this.OperatorIntercept++;
								break;
						case 4: // Dropped
								this.Drops++;
								break;
						case 5: // No Answer
								this.NoAnswers++;
								break;
						default: // ingore it I guess
								break;
						}
				}

		private static List<Agent> CreateAgents(Campaign campaign, List<AgentStat> agentStats) 
			{
			List<Agent> agents = new List<Agent>();
			agents.Add(new Agent(campaign, 0));
			Agent masterAgent = agents[0];
			foreach(AgentStat agentStat in agentStats) 
				{
				// See if we already have an agent
				Agent agent = FindAgent(agents, agentStat.AgentID);
				if(agent == null) 
					{
					agent = new Agent(campaign, agentStat.AgentID);
					agents.Add(agent);
					}
								
				// Update the agents stats
				agent.Leads += agentStat.Leads;
				agent.Presentations += agentStat.Presentations;
				agent.TalkTime += agentStat.TalkTime;
				agent.WaitTime += agentStat.WaitTime;
				agent.PauseTime += agentStat.PauseTime;
				agent.WrapTime += agentStat.WrapTime;
				masterAgent.Leads += agentStat.Leads;
				masterAgent.Presentations += agentStat.Presentations;
				masterAgent.TalkTime += agentStat.TalkTime;
				masterAgent.WaitTime += agentStat.WaitTime;
				masterAgent.PauseTime += agentStat.PauseTime;
				masterAgent.WrapTime += agentStat.WrapTime;
				}
			return agents;
			}

		public static Agent FindAgent(List<Agent> agents, long agentID) 
			{
			for(int i=1; i < agents.Count; i++) 
				{
				Agent agent = agents[i];
				if(agent.AgentID == agentID) return agent;
				}
				return null;
			}
				
		public ResultCode FindResultCode(long resultCodeID) 
			{
			foreach(ResultCode resultCode in ResultCodes) 
				{
				if(resultCode.ResultCodeID == resultCodeID)
					return resultCode;
				}
			return null;
			}

		// This does the magic. It takes all the call records and 
		// Calculates the statistics for the current agent.
		private void CalculateDialingStats() 
			{
			ManHours = TalkTime + PauseTime + WrapTime + WaitTime;
			DialingHours = TalkTime + WrapTime + WaitTime;
			TotalTalkTime = TalkTime + WrapTime;
			double divSafeDials = (Dials == 0) ? 1 : Dials;
			double divSafeTotalCalls = (TotalCalls == 0) ? 1 : TotalCalls;
			double divSafePresentations = (Presentations == 0) ? 1 : Presentations;
			double divSafeLeads = (Leads == 0) ? 1 : Leads;
			ConvertSecsToHours();
			ConnectPercentage = TotalCalls/divSafeDials;
			DropPercentage = Drops/divSafeDials;
			PresentationsPerHour = Presentations/DialingHours;
			PresentationsPerManHour = Presentations/ManHours;
			LeadsPerHour = Leads/DialingHours;
			LeadsPerManHour = Leads/ManHours;
			CallsPerHour = TotalCalls/DialingHours;
			CallsPerManHour = TotalCalls/ManHours;
			TalkPercentage = TalkTime/DialingHours;
			AveWrapTimeAll = WrapTime/divSafeTotalCalls; 
			AveWrapTimeLeads = WrapTime/divSafeLeads;
			AveWaitTime = WaitTime/divSafeTotalCalls;
			AveCallTime = TotalTalkTime/divSafeTotalCalls;
			AveCallTimePresentation = TotalTalkTime/divSafePresentations;
			AveCallTimeLead = TotalTalkTime/divSafeLeads;
			LeadsPerCall = Leads/divSafeTotalCalls;
			PresentationsPerCall = Presentations/divSafeTotalCalls;
			LeadsPerPresentation = Leads/divSafePresentations;
			foreach(ResultCode resultCode in ResultCodes) 
				{
				resultCode.TotalResultCodes = TotalCalls;
				resultCode.CalculateStats();
				}
			}

		// All of the dialer time is recorded in seconds. This simply
		// converts the time values to an hour orientated perspective
		private void ConvertSecsToHours() 
			{
			// Default all time values to at least .01 to avoid divide errors
			TalkTime = (TalkTime == 0) ? .01 : TalkTime;
			PauseTime = (PauseTime == 0) ? .01 : PauseTime;
			WrapTime = (WrapTime == 0) ? .01 : WrapTime;
			WaitTime = (WaitTime == 0) ? .01 : WaitTime;
			ManHours = (ManHours == 0) ? .01 : ManHours;
			DialingHours = (DialingHours == 0) ? .01 : DialingHours;
			TotalTalkTime = (TotalTalkTime == 0) ? .01 : TotalTalkTime;
			TalkTime = (TalkTime/3600);
			PauseTime = (PauseTime/3600);
			WrapTime = (WrapTime/3600);
			WaitTime = (WaitTime/3600);
			ManHours = (ManHours/3600);
			DialingHours = (DialingHours/3600);
			TotalTalkTime = (TotalTalkTime/3600);
			}

		private void LoadResultCodes() 
			{
			string query = CreateResultCodeQuery();
			SqlConnection conn = new SqlConnection(campaign.ConnString);
			conn.Open();
			SqlCommand cmd = new SqlCommand(query, conn);
			SqlDataReader sdr = cmd.ExecuteReader();
			while(sdr.Read()) 
				{
				long resultCodeID = (long)sdr[0];
				string Description = (string)sdr[1]; 
				ResultCode resultCode = new ResultCode(campaign, resultCodeID, Description);
				ResultCodes.Add(resultCode);
				}
			sdr.Close();
			conn.Close();
			}

		private string CreateResultCodeQuery() 
			{
			string query =  "SELECT ";
						 query += "   ResultCodeID, ";
						 query += "   Description ";
						 query += "FROM ResultCode ";
			return query;
			}

		private void CleanResultCodes() 
			{
			List<ResultCode> removeList = new List<ResultCode>();
			foreach(ResultCode resultCode in ResultCodes) 
				{
				if(resultCode.Count < 1) removeList.Add(resultCode);
				}
			foreach(ResultCode resultCode in removeList) 
				{
				ResultCodes.Remove(resultCode);
				}
			}

		public string PrintManHours() 
			{
			return FormatTimeValue(ManHours);
			}

		public string PrintDialingHours() 
			{
			return FormatTimeValue(DialingHours);            
			}

		public string PrintPauseTime() 
			{
			return FormatTimeValue(PauseTime);
			}

		// Converts time value in hh:mm:ss format
		static public string FormatTimeValue(double val) 
			{		
			int hours = (int)val;
			double rawMins = (60 * (val-hours));
			int mins = (int)rawMins;
			double rawSecs = (60 * (rawMins - mins));
			int secs = (int)rawSecs;            
			return String.Format("{0:00}:{1:00}:{2:00}", hours, mins, secs);
			}

		//
		/// Public properties
		//
		public List<ResultCode> ResultCodes = new List<ResultCode>();
		public long AgentID { get; set; }
		public string AgentName { get; set; }
		public double Leads { get; set; }
		public double Presentations { get; set; }
		public double TotalCalls { get; set; }
		public double TalkTime { get; set; }
		public double WaitTime { get; set; }
		public string FormatedWaitTime
			{
			get { return FormatTimeValue(WaitTime); }
			}
		public double TotalTalkTime { get; set; }
		public string FormatedTotalTalkTime
			{
			get { return FormatTimeValue(TotalTalkTime); }
			}
		public double PauseTime { get; set; }
		public string FormatedPauseTime
			{
			get { return FormatTimeValue(PauseTime); }
			}
		public double WrapTime { get; set; }
		public string FormatedWrapTime
			{
			get { return FormatTimeValue(WrapTime); }
			}
		public double ManHours { get; set; }
		public string FormatedManHours
			{
			get { return FormatTimeValue(ManHours); }
			}
		public double DialingHours { get; set; }
		public string FormatedDialingHours
			{
			get { return FormatTimeValue(DialingHours); }
			}
		public double LeadsPerHour { get; set; }
		public string FormatedLeadsPerHour 
			{ 
			get { return String.Format("{0:0.##}", LeadsPerHour); }
			}
		public double PresentationsPerHour { get; set; }
		public string FormatedPresentationsPerHour
			{ 
			get { return String.Format("{0:0.##}", PresentationsPerHour); }
			}
		public double LeadsPerManHour { get; set; }
		public double PresentationsPerManHour { get; set; }
		public double CallsPerHour { get; set; }
		public string FormatedCallsPerHour
			{ 
			get { return String.Format("{0:0.##}", CallsPerHour); }
			}
		public double CallsPerManHour { get; set; }
		public double TalkPercentage { get; set; }
		public string FormatedTalkPercentage
			{ 
			get { return String.Format("{0:0%}", TalkPercentage); }
			}
		public double AveWaitTime { get; set; }
		public string FormatedAveWaitTime
			{
			get { return FormatTimeValue(AveWaitTime); }
			}
		public double AveWrapTimeAll { get; set; }
		public string FormatedAveWrapTimeAll
			{
			get { return FormatTimeValue(AveWrapTimeAll); }
			}
		public double AveWrapTimeLeads { get; set; }
		public string FormatedAveWrapTimeLeads
			{
			get { return FormatTimeValue(AveWrapTimeLeads); }
			}
		public double AveCallTime { get; set; }
		public string FormatedAveCallTime
			{
			get { return FormatTimeValue(AveCallTime); }
			}
		public double AveCallTimePresentation { get; set; }
		public string FormatedAveCallTimePresentation
			{
			get { return FormatTimeValue(AveCallTimePresentation); }
			}
		public double AveCallTimeLead { get; set; }
		public string FormatedAveCallTimeLead
			{
			get { return FormatTimeValue(AveCallTimeLead); }
			}
		public double LeadsPerCall { get; set; }
		public string FormatedLeadsPerCall
			{ 
			get { return String.Format("{0:0.##}", LeadsPerCall); }
			}
		public double LeadsPerPresentation { get; set; }
		public string FormatedLeadsPerPresentation
			{ 
			get { return String.Format("{0:0.##}", LeadsPerPresentation); }
			}
		public double PresentationsPerCall { get; set; }
		public string FormatedPresentationsPerCall
			{ 
			get { return String.Format("{0:0.##}", PresentationsPerCall); }
			}
		// These variables only apply to the summary agent
		public double Dials { get; set; }
		public double AnsweringMachines { get; set; }
		public double NoAnswers { get; set; }
		public double Busys { get; set; }
		public double Drops { get; set; }
		public double OperatorIntercept { get; set; }
		public double ConnectPercentage { get; set; }
		public string FormatedConnectPercentage
			{ 
			get { return String.Format("{0:0%}", ConnectPercentage); }
			}
		public double DropPercentage { get; set; }
		public string FormatedDropPercentage
			{ 
			get { return String.Format("{0:0%}", DropPercentage); }
			}
		private Campaign campaign;
		}
	}
