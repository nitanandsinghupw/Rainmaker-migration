using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml;
using System.Text.RegularExpressions;

using Rainmaker.Common.DomainModel;
using Rainmaker.Web.CampaignWS;
using System.Web.Script.Serialization;
using System.Collections.Generic;



namespace Rainmaker.Web.agent
{
    public partial class PostCampaignDetails : PageBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack && Session["Campaign"] != null)
            {
                try
                {
                    UpdateData();
                }
                catch (Exception ex)
                {
                    ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "PostCampaignDetails Page_Load catch: " + ex.Message, "page load");
                }
            }
        }

        private void UpdateData()
        {
            try
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();

                List<Field> fields = jsSerializer.Deserialize<List<Field>>(Request.Form["fields"]);

                Campaign objcampaign = (Campaign)Session["Campaign"];
                XmlDocument xDocCampaign = new XmlDocument();
                xDocCampaign.LoadXml(Serialize.SerializeObject(objcampaign, "Campaign"));
                System.Text.StringBuilder sbQuery = new System.Text.StringBuilder();

                System.Collections.ArrayList alfields = new ArrayList();
                bool bolLastWasEncrypted = false;

                if (fields.Count > 0)
                {
                    sbQuery.Append("UPDATE Campaign SET ");
                    for (int i = 0; i < fields.Count; i++)
                    {
                        Field field = fields[i];

                        string fieldName = field.name;
                        string fieldValue = field.value;

                        if (i == (fields.Count - 1))
                        {
                            // uniquekey
                            sbQuery.AppendFormat("WHERE {0}={1} ", fieldName, fieldValue);
                            break;
                        }

                        if (alfields.Contains(fieldName))
                            continue;

                        alfields.Add(fieldName);

                        //if it is encrypted then run the algorythm
                        if (field.type == "encrypted")
                        {
                            if (fieldValue != "This is encrypted data")
                            {
                                string plainText = fieldValue;
                                string cipherText = "";                 // encrypted text
                                string passPhrase = "whatevesfasdfasdfr23";        // can be any string
                                string initVector = "Qt@&^SDF15F6g7H8"; // must be 16 bytes

                                // Before encrypting data, we will append plain text to a random
                                // salt value, which will be between 4 and 8 bytes long (implicitly
                                // used defaults).
                                RijndaelEnhanced rijndaelKey =
                                    new RijndaelEnhanced(passPhrase, initVector);

                                Console.WriteLine(String.Format("Plaintext   : {0}\n", plainText));

                                // Encrypt the same plain text data 10 time (using the same key,
                                // initialization vector, etc) and see the resulting cipher text;
                                // encrypted values will be different.
                                for (int ii = 0; ii < 10; ii++)
                                {
                                    cipherText = rijndaelKey.Encrypt(plainText);
                                    Console.WriteLine(
                                        String.Format("Encrypted #{0}: {1}", ii, cipherText));
                                    plainText = rijndaelKey.Decrypt(cipherText);
                                }

                                // Make sure we got decryption working correctly.
                                Console.WriteLine(String.Format("\nDecrypted   :{0}", plainText));
                                fieldValue = cipherText;
                                fieldValue = "'" + fieldValue + "'";
                            }
                        }
                        else
                        {
                            fieldValue = fieldValue.Replace("'", "''");
                            if (fieldValue.Trim() != "")
                            {
                                if (field.type == "s" || field.type == "dt" || IsCampaignDefined(fieldName))
                                {
                                    fieldValue = "'" + fieldValue + "'";
                                }
                                else if (field.type == "bool")
                                {
                                    bool value = false;
                                    try
                                    {
                                        if (IsInt(fieldValue))
                                        {
                                            value = Convert.ToBoolean(Convert.ToInt32(fieldValue));
                                        }
                                        else
                                        {
                                            value = Convert.ToBoolean(fieldValue);
                                        }
                                    }
                                    catch { }
                                    fieldValue = value ? "1" : "0";
                                }
                            }
                            else
                            {
                                fieldValue = "null";
                            }
                        }
                        if (fieldValue != "This is encrypted data")
                        {
                            if (!bolLastWasEncrypted)
                            {
                                sbQuery.AppendFormat("{2}{0}={1} ", fieldName, fieldValue, i == 0 ? "" : ",");
                            }
                            else
                            {
                                sbQuery.AppendFormat("{0}={1} ", fieldName, fieldValue);
                            }
                            bolLastWasEncrypted = false;
                        }
                        else
                        {
                            //set that last field in recordset an encrypted so don't use comma
                            bolLastWasEncrypted = true;
                        }

                    }
                    string sbQuerystring = sbQuery.ToString();

                    if (!sbQuerystring.StartsWith("UPDATE Campaign SET WHERE"))
                    {

                        CampaignService campService = new CampaignService();

                        if (Session["LoggedAgent"] != null)
                            ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "Posting campaign update query: '{0}'.", sbQuery.ToString());

                        campService.UpdateCampaignDetails(xDocCampaign, sbQuery.ToString());

                        long key = 0;
                        try
                        {
                            key = Convert.ToInt64(Session["UniqueKey"]);
                        }
                        catch { }

                        if (key > 0)
                        {
                            DataSet dsCampaigndtls = (DataSet)Session["CampaignDtls"];
                            long queryId = 0;
                            try
                            {
                                queryId = Convert.ToInt64(dsCampaigndtls.Tables[0].Rows[0]["QueryId"]);
                            }
                            catch { }
                            dsCampaigndtls = campService.GetCampaignDetailsByKey(objcampaign.CampaignDBConnString, key, queryId);
                            if (dsCampaigndtls != null)
                            {
                                if (dsCampaigndtls.Tables[0].Rows.Count > 0)
                                {
                                    Session["CampaignDtls"] = dsCampaigndtls;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (Session["LoggedAgent"] != null)
                    ActivityLogger.WriteAgentEntry((Agent)Session["LoggedAgent"], "PostCampaignDetails UpdateData exception : '{0}'.", ex.Message);
                throw ex;
            }
        }

        private bool IsCampaignDefined(string field)
        {
            string sFields = "\"callduration\",\"totalnumattempts\",\"numattemptsam\",\"numattemptswkend\","
                + "\"numattemptspm\",\"leadprocessed\"";
            return sFields.IndexOf(field) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eval"></param>
        /// <returns></returns>
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
    }

    public class Field
    {
        public string name { get; set; }
        public string value { get; set; }
        public string type { get; set; }
    }
}
