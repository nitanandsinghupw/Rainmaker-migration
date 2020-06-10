using System;
using Rainmaker.Data;

namespace Rainmaker.Reports 
	{
	// Report type from which all others are created
	abstract public class GenericReport 
		{
		// Generates the HTML version of the report..
		abstract public string encodeHTMLReport(Campaign campaign, string startDate, string endDate, string startTime, string endTime);

		// Signals if the last report was created correctly
		public bool error;
		} 
	}
