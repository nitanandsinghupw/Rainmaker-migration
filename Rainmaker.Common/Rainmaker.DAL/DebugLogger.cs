using System;
using System.Collections.Generic;
using Microsoft.ApplicationBlocks.ExceptionManagement;
using Rainmaker.DAL.Properties;
using System.Text;

namespace Rainmaker.DAL
{
    public static class DebugLogger
    {
        public static void Write(string logEntry)
        {
            if (!Settings.Default.DebugLogging) return;
            try
            {
                FileLogger.WriteEntry(string.Format("{0} : {1}", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"), logEntry));
            }
            catch{}
            return;
        }
    }
}
