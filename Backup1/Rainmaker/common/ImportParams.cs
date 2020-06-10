using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Rainmaker.Web.common
	{
	public class ImportParams
		{
		public string filePath;
		public string uploadDirectory;
		public string delimter;
		public bool isFirstRowHeader;
		public bool allowSevenDigitNums;
		public bool allowTenDigitNums;
		public int importRule;
		public int exceptionType;
		public int neverCallType;
		}
	}