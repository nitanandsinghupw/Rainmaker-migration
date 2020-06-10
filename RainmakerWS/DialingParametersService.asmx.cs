using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using Rainmaker.DAL;
using Rainmaker.Common.DomainModel;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Configuration;
using System.Xml;

namespace Rainmaker.WebServices
{
    /// <summary>
    /// Summary description for DialingParametersService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class DialingParametersService : System.Web.Services.WebService
    {

        /// <summary>
        /// InsertUpdate DialingParameters
        /// </summary>
        [WebMethod]

        public XmlNode DialingParametersInsertUpdate(XmlNode xNodeDialingParameters)
        {

            string strCampaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            DialingParameter objDialingParameter;

            objDialingParameter = (DialingParameter)Serialize.DeserializeObject(xNodeDialingParameters, "DialingParameter");
            //objCampaign.CampaignDBConnString = strCampaignMasterDBConn.Replace("RainmakerMaster", "Campaign_");

            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.LoadXml((string)Serialize.SerializeObject(
                    DialingParameterAccess.DialingParameterInsertUpdate(strCampaignMasterDBConn,
                   objDialingParameter), "DialingParameter"));
            }
            catch (Exception ex)
            {
                //ExceptionManager.Publish(ex);
                throw new SoapException();
            }
            return xDoc;


        }
    }
}
