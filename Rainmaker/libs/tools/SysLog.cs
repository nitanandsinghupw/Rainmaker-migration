using System;
using System.IO;
using Rainmaker.Web.Properties;
using System.Web;

namespace Rainmaker.libs.tools
{
    public class SysLog
    {
        public static void Write(string system, string msg)
        {
            Settings _settings = Settings.Default;

            string sysLogPath = HttpContext.Current.Server.MapPath
            (
                string.Format("{0}{1}{2}", 
                    _settings.logsPath, 
                    _settings.sysLogPath, 
                    _settings.sysLogFilename)
            );

            FileInfo sysLogFile = new FileInfo(sysLogPath);
            if (!sysLogFile.Directory.Exists) sysLogFile.Directory.Create();

            using (StreamWriter w = new StreamWriter(sysLogFile.OpenWrite()))
            {
                w.WriteLine(String.Format("From: {0}, Msg: {1}.", system, msg));
                w.Close();
            }
        }
    }
}
