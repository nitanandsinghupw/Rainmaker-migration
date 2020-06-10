using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using Rainmaker.Common.DomainModel;
using Microsoft.ApplicationBlocks.Data;
using Microsoft.ApplicationBlocks.ExceptionManagement;
namespace Rainmaker.DAL
{
    public static class OtherParametersAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCampaignDBConn"></param>
        /// <param name="objOtherParameter"></param>
        /// <returns>OtherParameter</returns>
        public static OtherParameter OtherParameterInsertUpdate(string strCampaignMasterDBConn,
            OtherParameter objOtherParameter)
        {

            using (SqlConnection connect = new SqlConnection(strCampaignMasterDBConn))
            {
                connect.Open();

                using (SqlTransaction transaction = connect.BeginTransaction())
                {
                    try
                    {

                        SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@OtherParameterID",objOtherParameter.OtherParameterID),
                            new SqlParameter("@AllowCallTransfer",objOtherParameter.AllowCallTransfer),  
                            new SqlParameter("@StaticOffsiteNumber",objOtherParameter.StaticOffsiteNumber),
                            new SqlParameter("@AllowOnSiteTranferWData",objOtherParameter.AllowOnSiteTranferWData),
                            new SqlParameter("@TransferMessage",objOtherParameter.TransferMessage),
                            new SqlParameter("@AutoPlayMessage",objOtherParameter.AutoPlayMessage),
                            new SqlParameter("@HoldMessage",objOtherParameter.HoldMessage),
                            new SqlParameter("@AllowManualDial",objOtherParameter.AllowManualDial),
                            new SqlParameter("@ManualDialLines",objOtherParameter.ManualDialLines),
                            new SqlParameter("@AllowCallBacks",objOtherParameter.AllowCallBacks),
                            new SqlParameter("@QuaryStatisticsInPercent",objOtherParameter.QueryStatisticsInPercent),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)};


                        objOtherParameter.OtherParameterID = (long)SqlHelper.ExecuteScalar(strCampaignMasterDBConn,
                            CommandType.StoredProcedure, "InsUpd_OtherParameter", sparam_s);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {

                        throw ex;
                    }
                    finally
                    {
                        connect.Close();
                    }
                }
            }
            return objOtherParameter;

        }
    }
}
