using System;

namespace Rainmaker.Reports 
	{
	// All responses to the reporting front end are encoded in
	// an instance of AjaxResponse.
	public class AjaxResponse 
		{
		public AjaxResponse() 
			{
			message = "";
			contents = "<div>empty</div>";
			error = false;
			}

		public string message;
		public string contents;
		public string tablePixelWidth;
		public string tableWidth;
		public string tableHeight;
		public string tableColumns;
		public string rowKeys;
		public string sortOn;
		public string sqlOptions;
		public int rowCount;
		public int showCSVHeaders;
		public long rowLimit;
		public bool error;
		}
}
