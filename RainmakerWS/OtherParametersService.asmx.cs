using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.ComponentModel;
using System.Xml;
using Rainmaker.DAL;
using Rainmaker.Common.DomainModel;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using System.Configuration;

namespace Rainmaker.WebServices
{
    /// <summary>
    /// Summary description for OtherParametersService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    public class OtherParametersService : System.Web.Services.WebService
    {

        /// <summary>
        /// InsertUpdate OtherParameters
        /// </summary>
        [WebMethod]

        public XmlNode OtherParametersInsertUpdate(XmlNode xNodeOtherParameters)
        {

            string strCampaignMasterDBConn = ConfigurationManager.AppSettings["CampaignMasterDBConn"];
            OtherParameter objOtherParameter;
            objOtherParameter = (OtherParameter)Serialize.DeserializeObject(xNodeOtherParameters, "OtherParameter");
            XmlDocument xDoc = new XmlDocument();
            try
            {
                xDoc.LoadXml((string)Serialize.SerializeObject(
                   OtherParametersAccess.OtherParameterInsertUpdate(strCampaignMasterDBConn,
                   objOtherParameter), "OtherParameter"));
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
