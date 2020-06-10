using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Rainmaker.Common.DomainModel;

namespace Rainmaker.Web
{
    public class ActivityLogger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public static void WriteException(Exception ex, string interfaceType)
        {
            string strException = interfaceType + " Exception. ";
            strException += "Message : ";
            //strException += Environment.NewLine;
            strException += ex.Message;
            strException += Environment.NewLine;
            strException += "Date & Time : ";
            strException += System.DateTime.Now.ToString("MM/dd/yy HH:mm:ss.fff");
            strException += Environment.NewLine;
            strException += "Stack Trace : ";
            strException += Environment.NewLine;
            strException += ex.StackTrace;
            strException += Environment.NewLine;
            strException += "------------------------------------------------------------------------";
            strException += Environment.NewLine;
            try
            {
                string sPath = "";
                sPath = Global.strLogFilePath;

                if (sPath == "")
                {
                    System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
                    ass.CodeBase.Substring(0, ass.CodeBase.IndexOf("bin"));
                    sPath = sPath.Replace(@"file:///", "");
                    sPath = Path.Combine(sPath, "Logs");
                }

                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }

                string sFileName = string.Format(@"{0}\SiteExceptions_{1}.Log", sPath, DateTime.Today.ToString("MMddyyyy"));
                FileStream fs = null;
                try
                {

                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                    fs.Write(System.Text.Encoding.ASCII.GetBytes(strException), 0, strException.Length);
                    fs.Flush(); 

                }
                catch (Exception ee)
                {
                    throw ee;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                }
            }
            catch
            {
                // Don:  Have to look at this code.
                // throw ee;
            }

        }
        public static void WriteAdminEntry(Campaign objCampaign, string Message, params object[] args)
        {
            string strEntry = string.Format("{0}|", System.DateTime.Now.ToString("MM/dd/yy HH:mm:ss.fff"));
            string strMessage = string.Format(Message, args);
            if (objCampaign != null)
            {
                strEntry += string.Format("{0}|", objCampaign.ShortDescription);
            }
            strEntry += strMessage;
            strEntry += Environment.NewLine;

            try
            {
                string sPath = "";
                sPath = Global.strLogFilePath;

                if (sPath == "")
                {
                    System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
                    ass.CodeBase.Substring(0, ass.CodeBase.IndexOf("bin"));
                    sPath = sPath.Replace(@"file:///", "");
                    sPath = Path.Combine(sPath, "Logs");
                }
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }
                string sFileName = string.Format(@"{0}\AdminActivity_{1}.Log", sPath, DateTime.Today.ToString("MMddyyyy"));
                FileStream fs = null;
                try
                {
                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                    fs.Write(System.Text.Encoding.ASCII.GetBytes(strEntry), 0, strEntry.Length);
                    fs.Flush(); 
                }
                catch (Exception ee)
                {
                    throw ee;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }

        public static void WriteAgentEntry(Agent objAgent, string Message, params object[] args)
        {
            string strEntry = string.Format("{0}|", System.DateTime.Now.ToString("MM/dd/yy HH:mm:ss.fff"));
            string strMessage = string.Format(Message, args);
            if (objAgent != null)
            {
                switch (objAgent.AgentStatusID)
                {
                    case 1:
                        strEntry += string.Format("{0}|{1}|{2}|Ready|", objAgent.CampaignID, objAgent.AgentID, objAgent.AgentName);
                        break;
                    case 2:
                        strEntry += string.Format("{0}|{1}|{2}|Paused|", objAgent.CampaignID, objAgent.AgentID, objAgent.AgentName);
                        break;
                    case 3:
                        strEntry += string.Format("{0}|{1}|{2}|Busy|", objAgent.CampaignID, objAgent.AgentID, objAgent.AgentName);
                        break;
                    default:
                        strEntry += string.Format("{0}|{1}|{2}|Authenticated|", objAgent.CampaignID, objAgent.AgentID, objAgent.AgentName);
                        break;
                }
            }
            if (objAgent != null)
            {
                strEntry += string.Format("{0}|", objAgent.CallUniqueKey);
            }
            strEntry += strMessage;
            strEntry += Environment.NewLine;

            try
            {
                string sPath = "";                    
                sPath = Global.strLogFilePath;

                if (sPath == "")
                {     
                    System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
                    ass.CodeBase.Substring(0, ass.CodeBase.IndexOf("bin"));
                    sPath = sPath.Replace(@"file:///", "");
                    sPath = Path.Combine(sPath, "Logs");
                }

                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }

                string sFileName = string.Format(@"{0}\AgentActivity_{1}.Log", sPath, DateTime.Today.ToString("MMddyyyy"));
                FileStream fs = null;
                try
                {
                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                    fs.Write(System.Text.Encoding.ASCII.GetBytes(strEntry), 0, strEntry.Length);
                    fs.Flush();
                }
                catch (Exception ee)
                {
                    throw ee;
                }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                }
            }
            catch (Exception ee)
            {
                throw ee;
            }
        }
    }
}
