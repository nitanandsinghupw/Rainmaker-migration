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
    public static class CallListAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCampaignDBConn"></param>
        /// <param name="objOtherParameter"></param>
        /// <returns>OtherParameter</returns>
        public static CallList CallListInsertUpdate(Campaign campaign,
            CallList objCallList)
        {
            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@CallListID",objCallList.CallListID),
                            new SqlParameter("@AgentID",objCallList.AgentID),  
                            new SqlParameter("@ResultCodeID",objCallList.ResultCodeID),
                            new SqlParameter("@PhoneNumber",objCallList.PhoneNumber),
                            new SqlParameter("@CallDate",objCallList.CallDate),
                            new SqlParameter("@CallTime",objCallList.CallTime),
                            new SqlParameter("@CallDuration",objCallList.CallDuration),
                            new SqlParameter("@CallCompletionTime",objCallList.CallCompletionTime),
                            new SqlParameter("@CallWrapTime",objCallList.CallWrapTime),
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)
                };


                objCallList.CallListID = (long)SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "InsUpd_CallList", sparam_s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCallList;
        }

        public static ResultCode ResultCodeInsertUpdate(Campaign campaign,
            ResultCode objResultCode)
        {
            try
            {
                SqlParameter[] sparam_s = new SqlParameter[]{
                            new SqlParameter("@ResultCodeID",objResultCode.ResultCodeID),
                            new SqlParameter("@Description",objResultCode.Description),  
                            new SqlParameter("@Presentation",objResultCode.Presentation),
                            new SqlParameter("@Redialable",objResultCode.Redialable),
                            new SqlParameter("@RecycleInDays",objResultCode.RecycleInDays),
                            new SqlParameter("@Lead",objResultCode.Lead),
                            new SqlParameter("@Printable",objResultCode.Printable),
                            new SqlParameter("@NeverCall",objResultCode.NeverCall),
                            new SqlParameter("@VerifyOnly",objResultCode.VerifyOnly),
                            new SqlParameter("@DialThroughAll",objResultCode.DialThroughAll),
                            new SqlParameter("@ShowDeletedResultCodes",objResultCode.ShowDeletedResultCodes),
                            new SqlParameter("@DateDeleted",DateTime.Now.Date), 
                            new SqlParameter("@DateCreated",DateTime.Now.Date),  
                            new SqlParameter("@DateModified",DateTime.Now.Date)};


                objResultCode.ResultCodeID = (long)SqlHelper.ExecuteScalar(campaign.CampaignDBConnString,
                    CommandType.StoredProcedure, "InsUpd_ResultCode", sparam_s);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objResultCode;
        }
    }
}
