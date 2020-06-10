using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Microsoft.ApplicationBlocks.ExceptionManagement
{
    public class FileLogger
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public static void WriteEntry(string entry)
        {
            entry += Environment.NewLine;
            entry += "------------------------------------------------------------------------";
            entry += Environment.NewLine;
            try
            {
                System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
                string sPath = ass.CodeBase.Substring(0, ass.CodeBase.IndexOf("bin"));
                sPath = sPath.Replace(@"file:///", "");
                sPath = Path.Combine(sPath, "Logs");
                if (!Directory.Exists(sPath))
                {
                    Directory.CreateDirectory(sPath);
                }
                string sFileName = string.Format(@"{0}\RainmakerWS_{1}.Log", sPath, DateTime.Today.ToString("MMddyyyy"));
                FileStream fs = null;
                try
                {
                    fs = new FileStream(sFileName, FileMode.Append, FileAccess.Write);
                    fs.Write(System.Text.Encoding.ASCII.GetBytes(entry), 0, entry.Length);
                    fs.Flush();
                }
                catch { }
                finally
                {
                    if (fs != null)
                    {
                        fs.Close();
                        fs = null;
                    }
                }
            }
            catch {}
        }
    }
}
