using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Globalization;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using Rainmaker.Common.DomainModel;
using Rainmaker.Common.Csv;
using Rainmaker.Web.common;
using System.Xml;
using Rainmaker.Web.CampaignWS;
using Rainmaker.libs.tools;
using System.Text;
using System.Text.RegularExpressions;

namespace Rainmaker.Web.campaign
{
    public partial class ImportMappingsDNC : PageBase
    {
        #region Private Variables
        private ArrayList dataMap = null;
        private ArrayList dataMapIdx = null;
        private ArrayList sourceColumns = new ArrayList();
        private ImportStats importStats = new ImportStats();
        private bool allowSevenDigitNums = true;
        private bool allowTenDigitNums = true;
        //private int curImportMapID = 0;
        private string sqlCols = string.Empty;
        private string sqlValues = string.Empty;
        private string strUpdate = string.Empty;
        private string strPhoneNumber = string.Empty;
        private string sDupFilePath = string.Empty;
        private List<string[]> campaignFields = null;
        private string resetQuery = "UPDATE Campaign SET ";
        #endregion

        #region Events

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                handleRequestAction();
                //createImportMapList();
                ReadCampaignInfo();
                BindMappingsGrid();
            }
        }


        //-------------------------------------------------------------
        //
        //
        //------------------------------------------------------------- 
        private void handleRequestAction()
        {
            /*
            string action = Request["action"];
            curImportMapID = Convert.ToInt32(Request["curImportMapID"]);
            switch (action)
            {
                case "save_import_map":
                    saveImportMap();
                    break;
                case "rename_import_map":
                    renameImportMap();
                    break;
                case "load_import_map":
                    loadImportMap();
                    break;
                case "create_import_map":
                    createImportMap();
                    break;
                case "delete_import_map":
                    deleteImportMap();
                    break;
            }*/
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        /*private void createImportMapList()
        {
            curImportMapIDValue.Text = "" + curImportMapID;
            string chooseState = (curImportMapID > 0) ? "" : "selected=\"true\"";
            importMapList.Text = String.Format("<option value=\"0\" {0}>--Select--</option>\n", chooseState);

            if (Directory.Exists(Global.strImportMapPath) == false)
            {
                Directory.CreateDirectory("C:\\RainMakerWeb\\ImportMap\\");
            }
            DirectoryInfo dirInfo = new DirectoryInfo("C:\\RainMakerWeb\\ImportMap\\");
            SysLog.Write("IMPORTMAP", "C:\\RainMakerWeb\\ImportMap\\");
            FileInfo[] fileNames = dirInfo.GetFiles("*.ini");

            for (int i = 0; i < fileNames.Length; i++)
            {
                chooseState = (curImportMapID == (i + 1)) ? "selected=\"true\"" : "";
                FileInfo file = fileNames[i];
                string displayName = Path.GetFileNameWithoutExtension(file.FullName);
                importMapList.Text += String.Format("<option value=\"{1}\" {2}>{0}</option>\n", displayName, "" + (i + 1), chooseState);
            }
        }*/

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        /*private void renameImportMap()
        {
            FileInfo file = findImportMapByID();
            string src = @"C:\RainMakerWeb\ImportMap\" + file.Name;
            string dest = @"C:\RainMakerWeb\ImportMap\" + Request["name"] + ".ini";
            File.Move(src, dest);
            curImportMapID = findImportMapID(Request["name"] + ".ini");
        }*/

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        /*private void loadImportMap()
        {
            curImportMapIDValue.Text = "" + curImportMapID;
            FileInfo file = findImportMapByID();
            if (file == null) return;
            INIFile f = new INIFile(@"C:\RainMakerWeb\ImportMap\" + file.Name);
            int mapCnt = Convert.ToInt32(f.IniReadValue("Information", "count"));
            curImportMap.Text = "";
            for (int i = 0; i < mapCnt; i++)
            {
                string mapValue = f.IniReadValue("Import Map", "" + i);
                if (curImportMap.Text.Length > 0)
                {
                    curImportMap.Text += ", ";
                }
                curImportMap.Text += "{value: '" + mapValue + "'}";
            }
        }*/

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        /*private void createImportMap()
        {
            DateTime cur = DateTime.Now;
            string fileName = String.Format("map_{0}.ini", cur.ToString("MM_dd_yyyy_HH_mm_ss"));
            INIFile f = new INIFile("C:\\RainMakerWeb\\ImportMap\\" + fileName);
            f.IniWriteValue("Information", "created", "" + DateTime.Now);
            f.IniWriteValue("Information", "count", "" + 0);
            curImportMapID = findImportMapID(fileName);
        }
        */
        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        /*private void deleteImportMap()
        {
            FileInfo file = findImportMapByID();
            string src = @"C:\RainMakerWeb\ImportMap\" + file.Name;
            File.Delete(src);
            curImportMapID = 0;
        }
        */
        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        /*private void saveImportMap()
        {
            FileInfo file = findImportMapByID();
            INIFile f = new INIFile(@"C:\RainMakerWeb\ImportMap\" + file.Name);
            // Just check all possible fields until we run out.
            int i = 0;
            for (; i < 1000; i++)
            {
                string mapValue = Request["" + i];
                if (mapValue != null)
                {
                    f.IniWriteValue("Import Map", "" + i, mapValue);
                }
                else
                {
                    break;
                }
            }
            f.IniWriteValue("Information", "count", "" + i);
            loadImportMap();
        }*/

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        /*private FileInfo findImportMapByID()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"C:\RainMakerWeb\ImportMap");
            FileInfo[] fileNames = dirInfo.GetFiles("*.ini");
            for (int i = 0; i < fileNames.Length; i++)
            {
                if (curImportMapID == (i + 1))
                {
                    return fileNames[i];
                }
            }
            return null;
        }
        */
        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        /*private int findImportMapID(string fileName)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(@"C:\RainMakerWeb\ImportMap");
            FileInfo[] fileNames = dirInfo.GetFiles("*.ini");
            for (int i = 0; i < fileNames.Length; i++)
            {
                if (fileNames[i].Name == fileName)
                {
                    return (i + 1);
                }
            }

            return 0;
        }*/

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private Campaign ReadCampaignInfo()
        {
            /*if (Session["Campaign"] != null)
            {
                Campaign objCampaign = (Campaign)Session["Campaign"];
                anchHome.InnerText = objCampaign.Description;

                // 2012-06-13 Dave Pollastrini
                // The following values were transferred via redirect of session vars (ImportParams) from Import.aspx
                // and are based on NONBOUND UI elements on that page.  Consequently, the values were wrong.
                // setting values here directly from campaign values.
            */
                allowSevenDigitNums = false;
                allowTenDigitNums = true;

            //    return objCampaign;
            //}
            return null;
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        // Jeff Figure out what to do with this
        int defaultMappingIndex = 0;
        protected void grdMappingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                BindCampaignFieldColumn(e);
                BindSourceFieldColumn(e);
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        // This builds the list of columns you can choose form the source file
        private void BindSourceFieldColumn(GridViewRowEventArgs e)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("sourceColumns");
            ddl.Items.Clear();
            for (int i = 0; i < sourceColumns.Count; i++)
            {
                ddl.Items.Add(new ListItem(Convert.ToString(sourceColumns[i]), Convert.ToString(i)));
            }
            ddl.SelectedValue = Convert.ToString(defaultMappingIndex++);
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        // This creates the drop-down list for the 'Table Column' select
        private void BindCampaignFieldColumn(GridViewRowEventArgs e)
        {
            DropDownList ddl = (DropDownList)e.Row.FindControl("campaignColumns");
            BindColumnsDropdownDNC(ddl, new ListItem("-Select-", "-Select-"));

            /*Campaign objCampaign = (Campaign)Session["Campaign"];
            DataSet dsFields;
            CampaignService objCampService = new CampaignService();
            XmlDocument xDocCampaign = new XmlDocument();

            xDocCampaign.LoadXml(Serialize.SerializeObject(objCampaign, "Campaign"));
            dsFields = objCampService.GetCampaignFields(xDocCampaign);

            foreach (DataRow row in dsFields.Tables[0].Rows)
            {
                if (!(bool)(row["IsDefault"]))
                {
                    ddl.Items.Add(new ListItem(row["FieldName"].ToString(), row["FieldName"].ToString() + ":" + row["FieldType"].ToString()));
                }
            }*/

        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        // This is where the file import actually happens      
        protected void lbtnUpload_Click(object sender, EventArgs e)
        {
            if (ValidateDataMapping())
            {
                ImportParams importParams = (ImportParams)Session["importparams"];
                char delimiterchar = Convert.ToChar(importParams.delimter == "t" ? "\t" : importParams.delimter);
                List<ImportFieldRow> list = ReadCSVData(importParams.filePath, importParams.isFirstRowHeader, delimiterchar);
                if (list.Count > 0)
                {
                    createDuplicateFile();

                    SetupDataImport(list);

                    Session["DupFilePath"] = sDupFilePath;
                    Session["ImportStats"] = importStats;
                    Response.Redirect("ImportStatsDNC.aspx");
                }
                else
                {
                    PageMessage = "No Valid data found in this file, Please check file and mappings";
                }
            }
            else
            {
                PageMessage = "Columns not assigned correctly.";
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void createDuplicateFile()
        {
            ImportParams importParams = (ImportParams)Session["importparams"];
            Campaign campaign = ReadCampaignInfo();
            if (importParams.importRule == 1)
            {
                sDupFilePath = importParams.uploadDirectory;
                sDupFilePath += Path.GetFileNameWithoutExtension(importParams.filePath);
                sDupFilePath += "_DUP_" + DateTime.Today.ToString("MMddyyyy");
                sDupFilePath += Path.GetExtension(importParams.filePath);
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void SetupDataImport(List<ImportFieldRow> list)
        {
            Campaign campaign = ReadCampaignInfo();

            
            string campaignMasterDBConn = ConfigurationManager.AppSettings["MasterConnectionString"];
            campaignMasterDBConn = "Server=leadsweeper-dev\\SQLEXPRESS;Database=RainmakerMaster;User Id=sa;Password=R@inM@ker;";

            using (SqlConnection connection = new SqlConnection(campaignMasterDBConn))
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        command.Transaction = transaction;
                        ImportData(command, list);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        SysLog.Write("Data Importer", String.Format("Failed Database Operation: {0}.", ex.Message));
                        try
                        {
                            transaction.Rollback();
                        }
                        catch (Exception ex2)
                        {
                            SysLog.Write("Data Importer", String.Format("Failed Database Rollback: {0}.", ex2.Message));
                        }
                    }
                }
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void ImportData(SqlCommand command, List<ImportFieldRow> list)
        {

            int iCount = campaignFields.Count;
            if (iCount <= 0)
            {
                CreateCampaignFieldList(command);
            }

            for (int i = 0; i < list.Count; i++)
            {
                ImportFieldRow importRow = (ImportFieldRow)list[i];

                sqlCols = string.Empty;
                sqlValues = string.Empty;
                strPhoneNumber = string.Empty;
                createImportFieldList(importRow);

                if (!isPhoneNumberInSystem(command))
                {
                    command.CommandText = "INSERT INTO [dbo].[MasterDNC](" + sqlCols + ") values(" + sqlValues + ")";
                    SysLog.Write("RAINYDAY", command.CommandText);
                    command.ExecuteNonQuery();
                    importStats.LeadsImported++;
                }
                else
                {
                    if (ruleAllowsUpdates())
                    {
                        if (ruleUpdatesResetAllFields())
                        {
                            ClearColumnData(command, strPhoneNumber);
                        }

                        command.CommandText = string.Format("Update [dbo].[MasterDNC] set {0} where PhoneNum = '{1}'", strUpdate, strPhoneNumber);
                        SysLog.Write("RAINYDAY", command.CommandText);
                        command.ExecuteNonQuery();
                        importStats.LeadsUpdated++;
                    }
                    else if (ruleSavesDuplicates())
                    {
                        File.AppendAllText(sDupFilePath, sqlValues);
                        importStats.LeadsDuplicate++;
                    }
                }
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void createImportFieldList(ImportFieldRow importRow)
        {

            ImportParams importParams = (ImportParams)Session["importparams"];
            List<ImportField> fieldList = importRow.ImportFieldsList;

            for (int j = 0; j < fieldList.Count; j++)
            {
                ImportField field = (ImportField)fieldList[j];
                string value = "";
                field.FieldValue = field.FieldValue.Replace("'", "''");
                switch (field.FieldType.ToLower())
                {
                    case "decimal":
                    case "money":
                        value = field.FieldValue.Trim() == "" ? "null" : field.FieldValue;
                        Double dValue = 0;
                        if (value != null && value != "null")
                        {
                            try
                            {
                                //-------------------------------------
                                // We check to see if it is a proper 
                                // value being passed, if not we reset it.
                                //-------------------------------------
                                dValue = Convert.ToDouble(value);
                            }
                            catch
                            {
                                field.FieldValue = "null";
                                value = "null";
                            }
                        }
                        break;

                    case "integer":
                        value = field.FieldValue.Trim() == "" ? "null" : field.FieldValue;
                        Int16 iValue = 0;
                        if (value != null && value != "null")
                        {
                            try
                            {
                                //-------------------------------------
                                // We check to see if it is a proper 
                                // value being passed, if not we reset it.
                                //-------------------------------------
                                iValue = Convert.ToInt16(value);
                            }
                            catch
                            {
                                field.FieldValue = "null";
                                value = "null";
                            }
                        }


                        break;

                    case "date":
                        value = field.FieldValue.Trim() == "" ? "null" : "'" + field.FieldValue + "'";
                        break;

                    case "boolean":
                        value = field.FieldValue.Trim() == "" ? "null" : field.FieldValue;
                        if (value != null && value != "null" &&
                            (value.ToUpper() == "TRUE" || value != "0"))
                        {
                            value = "1";
                        }
                        else
                        {
                            value = "0";
                        }

                        break;

                    default:
                        value = field.FieldValue.Trim() == "" ? "null" : "'" + field.FieldValue + "'";
                        break;
                }


                if (field.FieldName != "NeverCallFlag" || importParams.exceptionType == 1)
                {
                    if (sqlCols == string.Empty)
                    {
                        sqlCols = field.FieldName;
                        sqlValues = value;
                        strUpdate = String.Format("{0}={1}", field.FieldName, value);
                    }
                    else
                    {
                        sqlCols += "," + field.FieldName;
                        sqlValues += "," + value;
                        strUpdate += ", " + String.Format("{0}={1}", field.FieldName, value);
                    }
                }
                if (field.FieldName == "PhoneNum")
                {
                    strPhoneNumber = field.FieldValue;
                }
            }// end for loop
            forceNeverCallFlag();
        }

        //-------------------------------------------------------------
        // Don:  Routine for truncating the data coming in.
        //
        //-------------------------------------------------------------
        private int iGetFieldSize(string strFieldName)
        {
            int iReturn = -1;
            int i = 0;
            string[] strData = null;

            if (campaignFields == null)
            {
                Campaign campaign = ReadCampaignInfo();
                string campaignMasterDBConn = ConfigurationManager.AppSettings["MasterConnectionString"];
                
                campaignMasterDBConn = "Server=leadsweeper-dev\\SQLEXPRESS;Database=RainmakerMaster;User Id=sa;Password=R@inM@ker;";
                using (SqlConnection connection = new SqlConnection(campaignMasterDBConn))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    CreateCampaignFieldList(command);
                }
            }

            for (i = 0; i < campaignFields.Count && iReturn == -1; i++)
            {
                strData = campaignFields[i];
                if (strData[0].ToUpper() == strFieldName.ToUpper())
                {
                    if (strData[1] != null && strData[2] == "1")
                    {
                        iReturn = Convert.ToInt16(strData[1]);
                    }
                }
            }

            return (iReturn);

        }



        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        // If the never call flag is being forced then add it to the query
        private void forceNeverCallFlag()
        {
            ImportParams importParams = (ImportParams)Session["importparams"];
            bool forceNeverCall = false;
            int neverCallType = 0;
            if (importParams.exceptionType == 3)
            {
                forceNeverCall = true;
                neverCallType = 1;
            }
            if (importParams.exceptionType == 2)
            {
                forceNeverCall = true;
                neverCallType = importParams.neverCallType;
            }
            if (forceNeverCall)
            {
                if (sqlCols == string.Empty)
                {
                    sqlCols = "NeverCallFlag";
                    sqlValues = ("" + importParams.neverCallType);
                    strUpdate = String.Format("{0}={1}", "NeverCallFlag", ("" + neverCallType));
                }
                else
                {
                    sqlCols += "," + "NeverCallFlag";
                    sqlValues += "," + ("" + importParams.neverCallType);
                    strUpdate += ", " + String.Format("{0}={1}", "NeverCallFlag", ("" + neverCallType));
                }
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private bool isPhoneNumberInSystem(SqlCommand command)
        {
            bool found = false;
            command.CommandText = String.Format("SELECT count(*) FROM [dbo].[MasterDNC] WHERE PhoneNum = '{0}'", strPhoneNumber);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                if (Convert.ToInt32(reader[0]) > 0)
                {
                    found = true;
                }
            }
            reader.Close();

            return found;
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void ClearColumnData(SqlCommand command, string phoneNumber)
        {
            command.CommandText = String.Format(resetQuery, phoneNumber);
            command.ExecuteNonQuery();
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        // Builds a list of fields in teh campaign
        private void CreateCampaignFieldList(SqlCommand command)
        {
            
            campaignFields = new List<string[]>();


            //--------------------------------------------------------------
            // Don:
            // Joined the two tables so we canget the column size information 
            // for truncating long data fields.
            //--------------------------------------------------------------
            /*string strSQL = " SELECT column_name, [CampaignFields].value, [CampaignFields].FieldTypeID from Information_Schema.Columns  ";
            strSQL += " inner join CampaignFields On [CampaignFields].FieldName = column_name ";
            strSQL += " WHERE table_name='MasterDNC'";
            
            command.CommandText = strSQL;
            SqlDataReader reader = command.ExecuteReader();

            // Add "reset" for "campaign" fields.
            while (reader.Read())
            {
             */
                string colName = "PhoneNumber";

                string colSize = "10";

                string[] strData = new string[3];

                strData[0] = colName;
                strData[1] = colSize;
                strData[2] = "1";

                campaignFields.Add(strData);

                /*if (reader["value"] != null)
                {
                    colSize = reader["value"].ToString();
                }
                else
                {
                    colSize = "-1";
                }*/

                /*if (colName != "UniqueKey" && colName != "DateTimeofImport" &&
                 colName != "PhoneNum" && colName != "isManualDial")
                {
                    string[] strData = new string[3];

                    strData[0] = colName;
                    strData[1] = colSize;
                    strData[2] = reader["FieldTypeID"].ToString();
                    campaignFields.Add(strData);

                    if (!firstCol)
                    {
                        resetQuery += ", ";
                    }

                    resetQuery += string.Format("{0}=NULL", colName);
                    firstCol = false;

                }*/
            //}

            // 2012-06-07 Dave Pollastrini
            // Added "reset" for "system" fields to resetQuery string.
            resetQuery += @", 
                AgentName               = null,
                AgentID                 = null,
                DBCompany               = null,
                NeverCallFlag           = null, 
                OffsiteTransferNumber   = null,
                VerificationAgentID     = null, 
                CallResultCode          = null,
                DateTimeofCall          = null,
                CallDuration            = null,
                CallSenttoDialTime      = null,
                CalltoAgentTime         = null,
                CallHangupTime          = null,
                CallCompletionTime      = null,
                CallWrapUpStartTime     = null,
                CallWrapUpStopTime      = null,
                ResultCodeSetTime       = null,
                TotalNumAttempts        = null,
                NumAttemptsAM           = null,
                NumAttemptsWkEnd        = null,
                NumAttemptsPM           = null,
                LeadProcessed           = null,
                FullQueryPassCount      = null,
                scheduledate            = null,
                schedulenotes           = null,
                isManualDial            = 0
                ";

            resetQuery += " WHERE (PhoneNum='{0}')";

            //reader.Close();

        }
        #endregion

        #region Method that decide how rules are interpreted


        //-----------------------------------------------------------------
        //
        //   
        //-----------------------------------------------------------------
        private bool ruleSavesDuplicates()
        {
            ImportParams importParams = (ImportParams)Session["importparams"];
            switch (importParams.importRule)
            {
                case 1:
                    return true;
                default:
                    return false;
            }
        }


        //-----------------------------------------------------------------
        //
        //   
        //-----------------------------------------------------------------
        private bool ruleUpdatesResetAllFields()
        {
            ImportParams importParams = (ImportParams)Session["importparams"];
            switch (importParams.importRule)
            {
                case 2:
                    return true;
                default:
                    return false;
            }
        }


        //-----------------------------------------------------------------
        //
        //   
        //-----------------------------------------------------------------
        private bool ruleAllowsUpdates()
        {
            ImportParams importParams = (ImportParams)Session["importparams"];
            switch (importParams.importRule)
            {
                case 2:
                case 3:
                    return true;
                default:
                    return false;
            }
        }
        #endregion

        #region PrivateMethods
        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void BindMappingsGrid()
        {
            ImportParams importParams = (ImportParams)Session["importparams"];
            string filePath = importParams.filePath;
            bool hasHeader = importParams.isFirstRowHeader;
            string delimeter = importParams.delimter;

            // 2012-06-13 Dave Pollastrini
            // See notes in ReadCampaignInfo().  These values are being set there.
            //allowSevenDigitNums = importParams.allowSevenDigitNums;
            //allowTenDigitNums = importParams.allowTenDigitNums;

            char delimiterchar = Convert.ToChar(delimeter == "t" ? "\t" : delimeter);
            grdMappingList.DataSource = GetHeader(filePath, hasHeader, delimiterchar);
            grdMappingList.DataBind();
        }

        private DataSet GetHeader(string filePath, bool hasHeader, char Delimiter)
        {
            DataSet source = GetCsvDataSet(filePath, hasHeader);
            DataSet dsOut = new DataSet();
            DataTable dt = dsOut.Tables.Add();
            dt.Columns.Add("RecordHeader");
            dt.Columns.Add("SampleData");
            CreateHeaders(source, dt, hasHeader);
            return dsOut;
        }

        //-----------------------------------------------------------------
        //
        //   
        //-----------------------------------------------------------------
        private void CreateHeaders(DataSet source, DataTable dt, bool hasHeader)
        {
            DataRow dr = null;
            sourceColumns = new ArrayList();
            if (hasHeader)
            {
                // Use the header label found in the file
                for (int i = 0; i < source.Tables[0].Columns.Count; i++)
                {
                    string colName = source.Tables[0].Columns[i].ColumnName;
                    dr = dt.NewRow();
                    dr["RecordHeader"] = colName;
                    dt.Rows.Add(dr);
                    sourceColumns.Add(colName);
                    ReadColumnSample(source, dr, i);
                }
            }
            else
            {
                // Or just generate columns names if none are provided
                for (int i = 0; i < source.Tables[0].Columns.Count; i++)
                {
                    sourceColumns.Add("Column-" + Convert.ToString(i + 1));
                    dr = dt.NewRow();
                    dt.Rows.Add(dr);
                    ReadColumnSample(source, dr, i);
                }
            }
        }

        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        private void ReadColumnSample(DataSet source, DataRow dr, int idx)
        {
            if (source.Tables[0].Rows.Count > 0)
            {
                dr["SampleData"] = source.Tables[0].Rows[0][idx].ToString();
            }
        }


        //-------------------------------------------------------------
        //
        //
        //-------------------------------------------------------------
        // This is the function that creates the mapping between source data fields and the database columns
        private bool ValidateDataMapping()
        {

            bool blnResult = true;
            bool blnDuplicate = false;
            dataMap = new ArrayList();
            dataMapIdx = new ArrayList();

            for (int i = 0; i < grdMappingList.Rows.Count; i++)
            {

                DropDownList campaignCols = (DropDownList)grdMappingList.Rows[i].FindControl("campaignColumns");
                DropDownList sourceCols = (DropDownList)grdMappingList.Rows[i].FindControl("sourceColumns");

                string strMap = campaignCols.SelectedValue;
                int iIndex = Convert.ToInt32(sourceCols.SelectedValue);
                if (strMap.ToLower() != "-select-")
                {
                    if (dataMap.Contains(strMap))
                    {
                        blnDuplicate = true;
                        break;
                    }

                    dataMap.Add(strMap);
                    dataMapIdx.Add(iIndex);
                }
            }

            if (blnDuplicate)
            {
                PageMessage = "Duplicate mappings.";
                blnResult = false;
            }
            else if (dataMap.Count == 0)
            {
                PageMessage = "Select at least one mapping.";
                blnResult = false;
            }

            if (!dataMap.Contains("PhoneNum:String"))
            {
                PageMessage = "Please select a Phone Number mapping.";
                blnResult = false;
            }

            return blnResult;

        }


        //------------------------------------------------------------------
        //
        //
        //------------------------------------------------------------------
        private List<ImportFieldRow> ReadCSVData(string Filepath, bool IsHeadrer, char Delimiter)
        {

            List<ImportFieldRow> list = new List<ImportFieldRow>();

            DataSet ds = GetCsvDataSet(Filepath, IsHeadrer);
            string value = "";
            // bool bCheck = false;
            string strFieldName = "";
            int iFieldLength = 0;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                try
                {
                    importStats.TotalLeads += 1;
                    ImportFieldRow row = new ImportFieldRow();

                    for (int i = 0; i < dataMap.Count; i++)
                    {
                        value = dr[(int)dataMapIdx[i]].ToString().Trim();
                        string[] columnsData = dataMap[i].ToString().Split(':');
                        //---------------------------------------------------------
                        // Making sure the length does not exceed database
                        // column size.
                        //---------------------------------------------------------    
                        strFieldName = columnsData[0];
                        iFieldLength = iGetFieldSize(strFieldName);
                        if (iFieldLength != -1 && value.Length > iFieldLength)
                        {
                            value = value.Substring(0, iFieldLength);
                        }

                        if (strFieldName.Equals("phonenum", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (string.IsNullOrEmpty(value))
                            {
                                throw new Exception("NumberEmpty");
                            }

                            // 2012-06-13 Dave Pollastrini
                            // Remove all non-digit characters from phone number.
                            value = Regex.Replace(value, @"[^\d]", "");

                            //------------------------------------------------------
                            // Added to remove dashes, etc.
                            //------------------------------------------------------
                            //value = value.Replace("-", "");
                            //value = value.Replace(" ", "");
                            //value = value.Replace(".", "");
                            //value = value.Replace(")", "");
                            //value = value.Replace("(", "");

                            //bCheck = CheckForSpChars(value);
                            //if (bCheck == true)
                            //{
                            //    throw new Exception("NumberHasSpecialChars");
                            //}

                            //-----------------------------------------------------
                            // Ignoring the area code if necessary.
                            // Only 7 digit numbers are allowed.
                            //-----------------------------------------------------
                            // 2012-06-13 Dave Pollastrini
                            // Cannot simply "ignore" area code on allow7 and number is 10 digits.  Number may
                            // be "do not call" in local area code.  Also, the same numbers with different area
                            // codes would essentially be overwritten by the last similar number in the import.
                            // Per James, numbers in import file will always be either 7 or 10 digits, and the
                            // import process should support import as EITHER (mutually exclusive) 7 OR 10 digits
                            // per campaign.

                            switch (value.Length)
                            {
                                case 7:
                                    if (!allowSevenDigitNums)
                                        throw new Exception("NumberInvalidLength");
                                    break;
                                case 10:
                                    if (!allowTenDigitNums)
                                        throw new Exception("NumberInvalidLength");
                                    break;
                                default:
                                    throw new Exception("NumberInvalidLength");
                            }

                            //if (allowSevenDigitNums && value.Length != 7)
                            //{
                            //    // value = value.Substring(3, 7);
                            //    throw new Exception("NumberInvalidLength");
                            //}
                            //else if (allowTenDigitNums && value.Length != 10)
                            //{
                            //    throw new Exception("NumberInvalidLength");
                            //}

                            //if (!((allowSevenDigitNums && value.Length == 7) ||
                            //     (allowTenDigitNums && value.Length == 10)))
                            //{
                            //    throw new Exception("NumberInvalidLength");
                            //}
                        }

                        //value = Format(value);
                        SetFieldValue(row, columnsData[0], value, columnsData[1]);
                    }

                    list.Add(row);
                }
                catch (Exception ex)
                {
                    if (ex.Message == "NumberEmpty")
                    {
                        importStats.LeadsBlankPhoneNumber += 1;
                    }
                    else if (ex.Message == "NumberHasSpecialChars")
                    {
                        importStats.LeadsSPCharPhoneNumber += 1;
                    }
                    else if (ex.Message == "BadInputData")
                    {
                        importStats.LeadsBadData += 1;
                    }
                    else if (ex.Message == "NumberInvalidLength")
                    {
                        importStats.LeadsInvalidNumberLength += 1;
                    }
                }
            }

            return list;

        }


        //-----------------------------------------------------------------
        //
        //   
        //-----------------------------------------------------------------     
        private void SetFieldValue(ImportFieldRow row, string field, string value, string datatype)
        {
            List<ImportField> list = row.ImportFieldsList;
            switch (datatype.ToLower())
            {
                case "integer":
                    if (value.Trim() != "" && (!IsInt(value)))
                    {
                        value = value.Trim().ToLower();
                        if (value == "true" || value == "yes")
                        {
                            value = "1";
                        }
                        else if (value == "false" || value == "no")
                        {
                            value = "0";
                        }
                        else
                        {
                            throw new Exception("BadInputData");
                        }
                    }
                    break;

                case "boolean":
                    if (value.Trim() != "")
                    {
                        value = value.Trim().ToLower();
                        if (value == "true" || value == "yes")
                        {
                            value = "1";
                        }
                        else if (value == "false" || value == "no")
                        {
                            value = "0";
                        }
                        else if (IsBool(value))
                        {
                            value = Convert.ToBoolean(value) ? "1" : "0";
                        }
                        else if (IsInt(value))
                        {
                            value = Convert.ToBoolean(Convert.ToInt32(value)) ? "1" : "0";
                        }
                        else
                        {
                            throw new Exception("BadInputData");
                        }
                    }
                    break;

                case "date":
                    if (value.Trim() != "" && (!IsDateTime(value)))
                    {
                        throw new Exception("BadInputData");
                    }
                    break;

                case "decimal":
                case "money":
                    if (value.Trim() != "" && (!IsDouble(value)))
                    {
                        throw new Exception("BadInputData");
                    }
                    break;
            }
            ImportField impField = new ImportField();
            impField.FieldName = field;
            impField.FieldValue = value; //Server.UrlEncode(value);
            impField.FieldType = datatype;
            list.Add(impField);
        }

        //-----------------------------------------------------------------
        //
        //   
        //-----------------------------------------------------------------
        private bool CheckForSpChars(string str)
        {
            bool result = false;
            foreach (char ch in str)
            {
                if (Char.IsDigit(ch))
                {
                    continue;
                }
                else
                {
                    return true;
                }
            }
            return result;
        }


        //-----------------------------------------------------------------
        //
        //   
        //-----------------------------------------------------------------     
        private bool IsInt(string eval)
        {
            try
            {
                int i = Convert.ToInt32(eval);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        //-----------------------------------------------------------------
        //
        //   
        //-----------------------------------------------------------------
        private bool IsDouble(string eval)
        {
            try
            {
                double i = Convert.ToDouble(eval);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        //-----------------------------------------------------------------
        //
        //   
        //-----------------------------------------------------------------
        private bool IsDateTime(string eval)
        {
            try
            {
                DateTime i = Convert.ToDateTime(eval);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        //-----------------------------------------------------------------
        //
        //   
        //-----------------------------------------------------------------
        private bool IsBool(string eval)
        {
            try
            {
                bool i = Convert.ToBoolean(eval);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        //-----------------------------------------------------------------
        //
        //   
        //-----------------------------------------------------------------
        private string GetServerPath()
        {
            string serverPath = Server.MapPath("");
            serverPath = serverPath.Substring(0, serverPath.LastIndexOf(@"\"));
            return serverPath;
        }
        #endregion
    }
}
