using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Rainmaker.Common.DomainModel
{
    public static class Serialize
    {
        private static String UTF8ByteArrayToString(Byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            String constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private static Byte[] StringToUTF8ByteArray(String pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        /// <summary>
        /// Serializes the object.
        /// </summary>
        /// <param name="pObject">The p object.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        public static String SerializeObject(Object pObject, Type objectType)
        {
            try
            {
                String XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(objectType);
                xs.Serialize(memoryStream, pObject);
                XmlizedString = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
                memoryStream.Close();

                return XmlizedString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Deserializes the object.
        /// </summary>
        /// <param name="xn">The xn.</param>
        /// <param name="objectType">Type of the object.</param>
        /// <returns></returns>
        public static Object DeserializeObject(XmlNode xn, Type objectType)
        {
            try
            {
                XmlSerializer xs = new XmlSerializer(objectType);
                XmlNodeReader xnReader = new XmlNodeReader(xn);
                MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xn.OuterXml));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                return xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static String SerializeObject(Object pObject, string objectType)
        {
            try
            {
                String XmlizedString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = XmlSerializerObjectType(objectType);
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, null);
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                xs.Serialize(xmlTextWriter, pObject);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                XmlizedString = UTF8ByteArrayToString(memoryStream.ToArray());
                return XmlizedString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Object DeserializeObject(XmlNode xn, string objectType)
        {
            try
            {
                XmlSerializer xs = XmlSerializerObjectType(objectType);
                XmlNodeReader xnReader = new XmlNodeReader(xn);
                MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xn.OuterXml));
                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                return xs.Deserialize(memoryStream);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static XmlSerializer XmlSerializerObjectType(string objectType)
        {
            switch (objectType)
            {
                case "Campaign":
                    {
                        return new XmlSerializer(typeof(Campaign));
                    }
                case "Agent":
                    {
                        return new XmlSerializer(typeof(Agent));
                    }
                case "DialingParameter":
                    {
                        return new XmlSerializer(typeof(DialingParameter));
                    }
                case "OtherParameter":
                    {
                        return new XmlSerializer(typeof(OtherParameter));
                    }
                case "Script":
                    {
                        return new XmlSerializer(typeof(Script));
                    }
                case "DigitalizedRecording":
                    {
                        return new XmlSerializer(typeof(DigitalizedRecording));
                    }
                case "Query":
                    {
                        return new XmlSerializer(typeof(Query));
                    }
                case "QueryDetail":
                    {
                        return new XmlSerializer(typeof(QueryDetail));
                    }
                case "ResultCode":
                    {
                        return new XmlSerializer(typeof(ResultCode));
                    }
                case "AreaCodeRule":
                    {
                        return new XmlSerializer(typeof(AreaCodeRule));
                    }
                case "AreaCode":
                    {
                        return new XmlSerializer(typeof(AreaCode));
                    }
                case "GlobalDialingParams":
                    {
                        return new XmlSerializer(typeof(GlobalDialingParams));
                    }
                case "AgentCampaignMap":
                    {
                        return new XmlSerializer(typeof(AgentCampaignMap));
                    }
                case "AgentLogin":
                    {
                        return new XmlSerializer(typeof(AgentLogin));
                    }
                case "AgentActivity":
                    {
                        return new XmlSerializer(typeof(AgentActivity));
                    }
                case "CampaignQueryStatus":
                    {
                        return new XmlSerializer(typeof(CampaignQueryStatus));
                    }
                case "CampaignDetails":
                    {
                        return new XmlSerializer(typeof(CampaignDetails));
                    }
                case "CampaignFields":
                    {
                        return new XmlSerializer(typeof(CampaignFields));
                    }
                case "AgentStat":
                    {
                        return new XmlSerializer(typeof(AgentStat));
                    }
                case "SilentCall":
                    {
                        return new XmlSerializer(typeof(SilentCall));
                    }
                case "CallList":
                    {
                        return new XmlSerializer(typeof(CallList));
                    }
                case "Station":
                    {
                        return new XmlSerializer(typeof(Station));
                    }
                case "CloneInfo":
                    {
                        return new XmlSerializer(typeof(CloneInfo));
                    }
                case "TrainingScheme":
                    {
                        return new XmlSerializer(typeof(TrainingScheme));
                    }
                case "TrainingPage":
                    {
                        return new XmlSerializer(typeof(TrainingPage));
                    }
                default:
                    {
                        return new XmlSerializer(typeof(Campaign));
                    }
            }
        }
    }
}
