using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Web.Configuration;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Net;


namespace Rainmaker.Web.DataAccess
{

    //---------------------------------------------------------------------
    //  Application Page Change Data, matches columns in DB.
    //---------------------------------------------------------------------
    public class DBCampaignData
    {

        public DateTime   dtDateCreated               { get; set; }
        public DateTime   dtDateModified              { get; set; }
        public DateTime   dtStartTime                 { get; set; }
        public DateTime   dtStopTime                  { get; set; }
        public Int64      iCampaignID                 { get; set; }
        public Int64      iStatusID                   { get; set; }
        public bool       bAllow10DigitNums           { get; set; }
        public bool       bAllow7DigitNums            { get; set; }
        public bool       bAllowDuplicatePhones       { get; set; }
        public bool       bDialAllNumbers             { get; set; }
        public bool       bEnableAgentTraining        { get; set; }
        public bool       bFlushCallQueueOnIdle       { get; set; }
        public bool       bFundRaiserDataTracking     { get; set; }
        public bool       bIsDeleted                  { get; set; }
        public bool       bOnsiteTransfer             { get; set; }
        public bool       bRecordLevelCallHistory     { get; set; }
        public string     strCampaignDBConnString     { get; set; }
        public string     strDescription              { get; set; }
        public string     strDuplicateRule            { get; set; }
        public string     strOutboundCallerID         { get; set; }
        public string     strShortDescription         { get; set; }

        public DBCampaignData()
        {
            iCampaignID              = 0;
            strDescription           = "";
            strShortDescription      = "";
            bFundRaiserDataTracking  = false;
            bRecordLevelCallHistory  = false;
            bOnsiteTransfer          = false;
            bEnableAgentTraining     = false;
            bAllowDuplicatePhones    = false;
            strCampaignDBConnString  = "";
            bFlushCallQueueOnIdle    = false;
            iStatusID                = 0;
            bDialAllNumbers          = false;
            bIsDeleted               = false;
            dtDateCreated            = Convert.ToDateTime("01/01/1900");
            dtDateModified           = Convert.ToDateTime("01/01/1900");
            strOutboundCallerID      = "";
            dtStartTime              = Convert.ToDateTime("01/01/1900");
            dtStopTime               = Convert.ToDateTime("01/01/1900");
            strDuplicateRule         = "";
            bAllow7DigitNums         = false;
            bAllow10DigitNums        = false;

        }

    };


    //-----------------------------------------------------------------
    //  Data manipulation class for Campaign table.
    //-----------------------------------------------------------------
    public class DBCampaign
    {

        //-------------------------------------------------------------
        // array list for the rows of data retrived from the database.
        //-------------------------------------------------------------
        public List<DBCampaignData> DataList = new List<DBCampaignData>();

        //-------------------------------------------------------------
        //  Obtains the Campaign table database based on ID.
        //
        //-------------------------------------------------------------
        public bool bGet(Int64 iCampaignID)
        {

            bool bReturn = true;
            DBAccess dbAccess = Global.dbAccess;

            //----------------------------------------------------------
            // Create the command and the Connection.
            //----------------------------------------------------------
            string strConnString = WebConfigurationManager.ConnectionStrings["RainmakerMasterConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            string strSQL = "pCampaign_Get ";
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@CampaignID", iCampaignID, SqlDbType.Int, true, null);

            SqlCommand cmd = new SqlCommand(strSQL, con);

            //----------------------------------------------------------
            // Open the Connection and get the DataReader.
            //----------------------------------------------------------
            con.Open();
            SqlDataReader sqlReader = cmd.ExecuteReader();

            bReturn = bLoadData(sqlReader);

            //----------------------------------------------------------
            // Close the DataReader and the Connection.
            //----------------------------------------------------------
            sqlReader.Close();
            con.Close();

            return (bReturn);

        }



        //-------------------------------------------------------------
        //  Obtains the list of campaigns which have been marked for 
        //  deletion.
        //
        //-------------------------------------------------------------
        public bool bSel_Campaign_DeletedList()
        {

            bool bReturn = true;
            DBAccess dbAccess = Global.dbAccess;

            //----------------------------------------------------------
            // Create the command and the Connection.
            //----------------------------------------------------------
            string strConnString = WebConfigurationManager.ConnectionStrings["RainmakerMasterConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            string strSQL = "Sel_Campaign_DeletedList ";

            SqlCommand cmd = new SqlCommand(strSQL, con);

            //----------------------------------------------------------
            // Open the Connection and get the DataReader.
            //----------------------------------------------------------
            con.Open();
            SqlDataReader sqlReader = cmd.ExecuteReader();

            bReturn = bLoadData(sqlReader);

            //----------------------------------------------------------
            // Close the DataReader and the Connection.
            //----------------------------------------------------------
            sqlReader.Close();
            con.Close();

            return (bReturn);

        }


        //-------------------------------------------------------------
        //  Deletes a particular row of data.  If -500 is passed in
        //  all of the rows of data in the table are deleted.
        //-------------------------------------------------------------
        public bool bDelete(Int64 iCampaignID)
        {

            bool bReturn = true;
            DBAccess dbAccess = Global.dbAccess;

            //----------------------------------------------------------
            // Create the command and the Connection.
            //----------------------------------------------------------
            string strConnString = WebConfigurationManager.ConnectionStrings["RainmakerMasterConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            string strSQL = "pCampaign_Delete ";
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@CampaignID", iCampaignID, SqlDbType.Int, true, null);

            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, con);

                //-------------------------------------------------------
                // Open the Connection and get the DataReader.
                //-------------------------------------------------------
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                bReturn = false;
            };

            //----------------------------------------------------------
            // Close the DataReader and the Connection.
            //----------------------------------------------------------
            con.Close();

            return (bReturn);

        }


        //-------------------------------------------------------------
        //  Log out agent.                                             
        //  Works agents the Agent activity table, put here to save time.
        //  Did not want to create a whole new class for one procedure. 
        //-------------------------------------------------------------
        public bool bLogAgentOut(Int64 iAgentID, 
                                 Int64 iAgentActivityID)
        {

            bool bReturn = true;
            DBAccess dbAccess = Global.dbAccess;

            //----------------------------------------------------------
            // Create the command and the Connection.
            //----------------------------------------------------------
            string strConnString = WebConfigurationManager.ConnectionStrings["RainmakerMasterConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            string strSQL = "pLogAgentOut ";
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@AgentID", iAgentID, SqlDbType.Int, false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@AgentActivityID", iAgentActivityID, SqlDbType.Int, true, null);

            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, con);

                //-------------------------------------------------------
                // Open the Connection and get the DataReader.
                //-------------------------------------------------------
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                bReturn = false;
            };

            //----------------------------------------------------------
            // Close the DataReader and the Connection.
            //----------------------------------------------------------
            con.Close();

            return (bReturn);

        }


        //-------------------------------------------------------------
        //  Log out all agents for a particular campaign.                                             
        //  Works agents the Agent activity table, put here to save time.
        //  Did not want to create a whole new class for one procedure. 
        //-------------------------------------------------------------
        public bool bLogAgentOutForCampaign(Int64 iCampaignID)
        {

            bool bReturn = true;
            DBAccess dbAccess = Global.dbAccess;

            //----------------------------------------------------------
            // Create the command and the Connection.
            //----------------------------------------------------------
            string strConnString = WebConfigurationManager.ConnectionStrings["RainmakerMasterConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            string strSQL = "pLogAgentOutForCampaign ";
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@CampaignID", iCampaignID, SqlDbType.Int, true, null);

            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, con);

                //-------------------------------------------------------
                // Open the Connection and get the DataReader.
                //-------------------------------------------------------
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                bReturn = false;
            };

            //----------------------------------------------------------
            // Close the DataReader and the Connection.
            //----------------------------------------------------------
            con.Close();

            return (bReturn);

        }


        //-------------------------------------------------------------
        //  Log out agent all agents for all campaigns.                                             
        //  Works agents the Agent activity table, put here to save time.
        //  Did not want to create a whole new class for one procedure. 
        //-------------------------------------------------------------
        public bool bLogAgentOutForAllCampaign()
        {

            bool bReturn = true;
            DBAccess dbAccess = Global.dbAccess;

            //----------------------------------------------------------
            // Create the command and the Connection.
            //----------------------------------------------------------
            string strConnString = WebConfigurationManager.ConnectionStrings["RainmakerMasterConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            string strSQL = "pLogAgentOutForAllCampaign ";
            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, con);

                //-------------------------------------------------------
                // Open the Connection and get the DataReader.
                //-------------------------------------------------------
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                bReturn = false;
            };

            //----------------------------------------------------------
            // Close the DataReader and the Connection.
            //----------------------------------------------------------
            con.Close();

            return (bReturn);

        }




        //-------------------------------------------------------------
        //  Sets all campaign to idle.                                             
        //
        // 
        //-------------------------------------------------------------
        public bool bSetCampaignsToIdle()
        {

            bool bReturn = true;
            DBAccess dbAccess = Global.dbAccess;

            //----------------------------------------------------------
            // Create the command and the Connection.
            //----------------------------------------------------------
            string strConnString = WebConfigurationManager.ConnectionStrings["RainmakerMasterConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            string strSQL = "pSetCampaignsToIdle ";
            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, con);

                //-------------------------------------------------------
                // Open the Connection and get the DataReader.
                //-------------------------------------------------------
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                bReturn = false;
            };

            //----------------------------------------------------------
            // Close the DataReader and the Connection.
            //----------------------------------------------------------
            con.Close();

            return (bReturn);

        }



        //-------------------------------------------------------------
        //  Saves a new record to the database.
        //
        //
        //-------------------------------------------------------------
        public bool bSave(DBCampaignData dbCampaignData)
        {

            bool bReturn = true;
            DBAccess dbAccess = Global.dbAccess;

            //----------------------------------------------------------
            // Create the Command and the Connection.
            //----------------------------------------------------------
            string strConnString = WebConfigurationManager.ConnectionStrings["RainmakerMasterConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            string strSQL = "pCampaign_Save ";

            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@CampaignDBConnString ",      dbCampaignData.strCampaignDBConnString    , SqlDbType.VarChar  , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@Description          ",      dbCampaignData.strDescription             , SqlDbType.VarChar  , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@DuplicateRule        ",      dbCampaignData.strDuplicateRule           , SqlDbType.VarChar  , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@OutboundCallerID     ",      dbCampaignData.strOutboundCallerID        , SqlDbType.VarChar  , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@ShortDescription     ",      dbCampaignData.strShortDescription        , SqlDbType.VarChar  , false, null);

            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@iCampaignID          ",      dbCampaignData.iCampaignID                , SqlDbType.Int      , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@iStatusID            ",      dbCampaignData.iStatusID                  , SqlDbType.Int      , false, null);

            if (dbCampaignData.bAllow7DigitNums == true)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@Allow7DigitNums   ",      1                                         , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@Allow7DigitNums   ",      0                                         , SqlDbType.Bit      , false, null);
            }


            if (dbCampaignData.bAllowDuplicatePhones == true)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@AllowDuplicatePhones   ",      1                                    , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@AllowDuplicatePhones   ",      0                                    , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bDialAllNumbers == true)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@DialAllNumbers   ",      1                                          , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@DialAllNumbers   ",      0                                          , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bEnableAgentTraining == true)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@EnableAgentTraining   ",      1                                     , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@EnableAgentTraining   ",      0                                     , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bFlushCallQueueOnIdle == true)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@FlushCallQueueOnIdle   ",      1                                    , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@FlushCallQueueOnIdle   ",      0                                    , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bFundRaiserDataTracking == true)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@FundRaiserDataTracking   ",      1                                  , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@FundRaiserDataTracking   ",      0                                  , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bIsDeleted == true)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@IsDeleted   ",      1                                               , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@IsDeleted   ",      0                                               , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bOnsiteTransfer == true)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@OnsiteTransfer   ",      1                                          , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@OnsiteTransfer   ",      0                                          , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bRecordLevelCallHistory == true)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@RecordLevelCallHistory   ",      1                                  , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@RecordLevelCallHistory   ",      0                                  , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bAllow10DigitNums == true)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@Allow10DigitNums   ",      1                                        , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@Allow10DigitNums   ",      0                                        , SqlDbType.Bit      , false, null);
            }

            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@DateCreated"             ,dbCampaignData.dtDateCreated        , SqlDbType.DateTime , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@DateModified"            ,dbCampaignData.dtDateModified       , SqlDbType.DateTime , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@StartTime"               ,dbCampaignData.dtStartTime          , SqlDbType.DateTime , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@StopTime"                ,dbCampaignData.dtStopTime           , SqlDbType.DateTime , true, null);

            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, con);

                //-------------------------------------------------------
                // Open the Connection and get the DataReader.
                //-------------------------------------------------------
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                bReturn = false;
            };

            con.Close();

            return (bReturn);

        }


        //-------------------------------------------------------------
        //  Updates the Campaign table data.
        //
        //
        //-------------------------------------------------------------
        public bool bUpdate(DBCampaignData dbCampaignData)
        {

            bool bReturn = true;
            DBAccess dbAccess = Global.dbAccess;

            //----------------------------------------------------------
            // Create the Command and the Connection.
            //----------------------------------------------------------
            string strConnString = WebConfigurationManager.ConnectionStrings["RainmakerMasterConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            string strSQL = "pCampaign_Update ";
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@CampaignDBConnString ",      dbCampaignData.strCampaignDBConnString    , SqlDbType.VarChar  , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@Description          ",      dbCampaignData.strDescription             , SqlDbType.VarChar  , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@DuplicateRule        ",      dbCampaignData.strDuplicateRule           , SqlDbType.VarChar  , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@OutboundCallerID     ",      dbCampaignData.strOutboundCallerID        , SqlDbType.VarChar  , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@ShortDescription     ",      dbCampaignData.strShortDescription        , SqlDbType.VarChar  , false, null);

            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@iCampaignID          ",      dbCampaignData.iCampaignID                , SqlDbType.Int      , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@iStatusID            ",      dbCampaignData.iStatusID                  , SqlDbType.Int      , false, null);

            if (dbCampaignData.bAllow7DigitNums == false)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@Allow7DigitNums   ",      1                                         , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@Allow7DigitNums   ",      0                                         , SqlDbType.Bit      , false, null);
            }


            if (dbCampaignData.bAllowDuplicatePhones == false)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@AllowDuplicatePhones   ",      1                                    , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@AllowDuplicatePhones   ",      0                                    , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bDialAllNumbers == false)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@DialAllNumbers   ",      1                                          , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@DialAllNumbers   ",      0                                          , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bEnableAgentTraining == false)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@EnableAgentTraining   ",      1                                     , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@EnableAgentTraining   ",      0                                     , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bFlushCallQueueOnIdle == false)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@FlushCallQueueOnIdle   ",      1                                    , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@FlushCallQueueOnIdle   ",      0                                    , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bFundRaiserDataTracking == false)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@FundRaiserDataTracking   ",      1                                  , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@FundRaiserDataTracking   ",      0                                  , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bIsDeleted == false)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@IsDeleted   ",      1                                               , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@IsDeleted   ",      0                                               , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bOnsiteTransfer == false)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@OnsiteTransfer   ",      1                                          , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@OnsiteTransfer   ",      0                                          , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bRecordLevelCallHistory == false)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@RecordLevelCallHistory   ",      1                                  , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@RecordLevelCallHistory   ",      0                                  , SqlDbType.Bit      , false, null);
            }

            if (dbCampaignData.bAllow10DigitNums == false)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@Allow10DigitNums   ",      1                                        , SqlDbType.Bit      , false, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@Allow10DigitNums   ",      0                                        , SqlDbType.Bit      , false, null);
            }

            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@DateCreated"             ,dbCampaignData.dtDateCreated        , SqlDbType.DateTime , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@DateModified"            ,dbCampaignData.dtDateModified       , SqlDbType.DateTime , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@StartTime"               ,dbCampaignData.dtStartTime          , SqlDbType.DateTime , false, null);
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@StopTime"                ,dbCampaignData.dtStopTime           , SqlDbType.DateTime , true, null);

            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, con);

                //-------------------------------------------------------
                // Open the Connection and get the DataReader.
                //-------------------------------------------------------
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch
            {
                bReturn = false;
            };

            con.Close();

            return (bReturn);

        }


        //-------------------------------------------------------------
        //  Sets the IsDeleted flag
        //
        //-------------------------------------------------------------
        public bool bSet_IsDeleted(Int64 iCampaignID, bool bIsDeleted)
        {


            bool bReturn = true;
            DBAccess dbAccess = Global.dbAccess;

            //----------------------------------------------------------
            // Create the command and the Connection.
            //----------------------------------------------------------
            string strConnString = WebConfigurationManager.ConnectionStrings["RainmakerMasterConnectionString"].ConnectionString;

            SqlConnection con = new SqlConnection(strConnString);

            string strSQL = "pCampaign_Set_IsDeleted ";
            strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@CampaignID", iCampaignID, SqlDbType.Int, false, null);

            if (bIsDeleted == true)
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@IsDeleted", 1, SqlDbType.Bit, true, null);
            }
            else
            {
               strSQL = dbAccess.AddParamToSQLCmd(strSQL, "@IsDeleted", 0, SqlDbType.Bit, true, null);
            }

            try
            {
                SqlCommand cmd = new SqlCommand(strSQL, con);

                //-------------------------------------------------------
                // Open the Connection and get the DataReader.
                //-------------------------------------------------------
                con.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                //-----------------------------------------------------
                // For debugging purposes.
                //-----------------------------------------------------
                string strMsg = e.Message;
                bReturn = false;
            };

            //---------------------------------------------------------
            // Close the DataReader and the Connection.
            //---------------------------------------------------------
            con.Close();

            return (bReturn);

        }


        //-------------------------------------------------------------
        //  Transfers all of the database value to dbAdmin class.
        //
        //-------------------------------------------------------------
        public bool bLoadData(SqlDataReader sqlReader)
        {
            //---------------------------------------------------------
            // If there are no data to be loaded we return false
            // for the condition.
            //---------------------------------------------------------
            bool bReturn = false;

            while (sqlReader.Read() == true)
            {
                bReturn = true;

                //-----------------------------------------------------
                // Creating a new data node and initializing the members.
                //-----------------------------------------------------
                DBCampaignData Data = new DBCampaignData();

                int iCount = sqlReader.FieldCount;
                string strFieldName = "";
                int i = 0;
                for (i = 0; i < iCount; i++)
                {
                    //-------------------------------------------------
                    // Since some of the stored procedures do not bring back
                    // data from all of the columns we have to protect
                    // against this situation by checking the list of
                    // field names.
                    //-------------------------------------------------
                    strFieldName = sqlReader.GetName(i).ToString();


                    if (sqlReader["DateCreated"] != DBNull.Value)
                    {
                        Data.dtDateCreated = Convert.ToDateTime(sqlReader["DateCreated"]);
                    }
                    else
                    {
                        Data.dtDateCreated = DateTime.Today;
                    }

                    if (sqlReader["DateModified"] != DBNull.Value)
                    {
                        Data.dtDateModified = Convert.ToDateTime(sqlReader["DateModified"]);
                    }
                    else
                    {
                        Data.dtDateModified = DateTime.Today;
                    }

                    if (sqlReader["StartTime"] != DBNull.Value)
                    {
                        Data.dtStartTime = Convert.ToDateTime(sqlReader["StartTime"]);
                    }
                    else
                    {
                        Data.dtStartTime = DateTime.Today;
                    }

                    if (sqlReader["StopTime"] != DBNull.Value)
                    {
                        Data.dtStopTime = Convert.ToDateTime(sqlReader["StopTime"]);
                    }
                    else
                    {
                        Data.dtStopTime = DateTime.Today;
                    }



                    if (sqlReader["Allow7DigitNums"] != DBNull.Value)
                    {
                       Data.bAllow7DigitNums = Convert.ToBoolean(sqlReader["Allow7DigitNums"]);
                    }
                    else
                    {
                       Data.bAllow7DigitNums = false;
                    }

                    if (sqlReader["AllowDuplicatePhones"] != DBNull.Value)
                    {
                       Data.bAllowDuplicatePhones = Convert.ToBoolean(sqlReader["AllowDuplicatePhones"]);
                    }
                    else
                    {
                       Data.bAllowDuplicatePhones = false;
                    }

                    if (sqlReader["DialAllNumbers"] != DBNull.Value)
                    {
                       Data.bDialAllNumbers = Convert.ToBoolean(sqlReader["DialAllNumbers"]);
                    }
                    else
                    {
                       Data.bDialAllNumbers = false;
                    }

                    if (sqlReader["EnableAgentTraining"] != DBNull.Value)
                    {
                       Data.bEnableAgentTraining = Convert.ToBoolean(sqlReader["EnableAgentTraining"]);
                    }
                    else
                    {
                       Data.bEnableAgentTraining = false;
                    }

                    if (sqlReader["FlushCallQueueOnIdle"] != DBNull.Value)
                    {
                       Data.bFlushCallQueueOnIdle = Convert.ToBoolean(sqlReader["FlushCallQueueOnIdle"]);
                    }
                    else
                    {
                       Data.bFlushCallQueueOnIdle = false;
                    }

                    if (sqlReader["FundRaiserDataTracking"] != DBNull.Value)
                    {
                       Data.bFundRaiserDataTracking = Convert.ToBoolean(sqlReader["FundRaiserDataTracking"]);
                    }
                    else
                    {
                       Data.bFundRaiserDataTracking = false;
                    }

                    if (sqlReader["IsDeleted"] != DBNull.Value)
                    {
                       Data.bIsDeleted = Convert.ToBoolean(sqlReader["IsDeleted"]);
                    }
                    else
                    {
                       Data.bIsDeleted = false;
                    }


                    if (sqlReader["RecordLevelCallHistory"] != DBNull.Value)
                    {
                       Data.bRecordLevelCallHistory = Convert.ToBoolean(sqlReader["RecordLevelCallHistory"]);
                    }
                    else
                    {
                       Data.bRecordLevelCallHistory = false;
                    }


                    if (sqlReader["CampaignDBConnString"] != DBNull.Value)
                    {
                       Data.strCampaignDBConnString = sqlReader["CampaignDBConnString"].ToString();
                    }
                    else
                    {
                       Data.strCampaignDBConnString = "";
                    }

                    if (sqlReader["Description"] != DBNull.Value)
                    {
                       Data.strDescription = sqlReader["Description"].ToString();
                    }
                    else
                    {
                       Data.strDescription = "";
                    }

                    if (sqlReader["DuplicateRule"] != DBNull.Value)
                    {
                       Data.strDuplicateRule = sqlReader["DuplicateRule"].ToString();
                    }
                    else
                    {
                       Data.strDuplicateRule = "";
                    }

                    if (sqlReader["OutboundCallerID"] != DBNull.Value)
                    {
                       Data.strOutboundCallerID = sqlReader["OutboundCallerID"].ToString();
                    }
                    else
                    {
                       Data.strOutboundCallerID = "";
                    }

                    if (sqlReader["ShortDescription"] != DBNull.Value)
                    {
                       Data.strShortDescription = sqlReader["ShortDescription"].ToString();
                    }
                    else
                    {
                       Data.strShortDescription = "";
                    }

                    if (sqlReader["Allow10DigitNums"] != DBNull.Value)
                    {
                       Data.bAllow10DigitNums = Convert.ToBoolean(sqlReader["Allow10DigitNums"]);
                    }
                    else
                    {
                       Data.bAllow10DigitNums = false;
                    }

                    if (strFieldName == "CampaignID" && sqlReader["CampaignID"] != DBNull.Value)
                    {
                        Data.iCampaignID = Convert.ToInt64(sqlReader["CampaignID"]);
                    }

                    else if (strFieldName == "StatusID" && sqlReader["StatusID"] != DBNull.Value)
                    {
                        Data.iStatusID = Convert.ToInt64(sqlReader["StatusID"]);
                    }

                }

                DataList.Add(Data);

            }

            return (bReturn);
        }


        //-------------------------------------------------------------
        //  Member for cleaing the datalist.
        //
        //-------------------------------------------------------------
        public void vEmptyDataList()
        {
            DataList.Clear();
        }


    }
}




