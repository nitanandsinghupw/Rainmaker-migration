using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Collections;
using System.IO;

namespace Rainmaker.RainmakerDialer
{
    public class Utilities
    {
        /// <summary>
        /// returns application settings value for that key, if not
        /// exists returns default value.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="DefaultValue"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key, string DefaultValue)
        {
            try
            {
                string value = ConfigurationManager.AppSettings[key];
                return (value != null && value != string.Empty) ? value : DefaultValue;
            }
            catch
            {
                return DefaultValue;
            }
        }

        /// <summary>
        /// Creates directory if not exists
        /// </summary>
        /// <param name="strPath"></param>
        public static void CreateDirectory(string strPath)
        {
            try
            {
                if (!Directory.Exists(strPath))
                {
                    Directory.CreateDirectory(strPath);
                }
            }
            catch{
            }
            finally
            {
            }
        }

        /// <summary>
        /// Combines two paths 
        /// </summary>
        /// <param name="strPath1"></param>
        /// <param name="strPath2"></param>
        /// <returns></returns>
        public static string CombinePaths(string strPath1, string strPath2)
        {
            try
            {
                if (strPath1.LastIndexOf(@"\") != strPath1.Length - 1)
                {
                    strPath1 = strPath1 + @"\";
                }

                if (strPath2.IndexOf(@"\") == 0)
                {
                    strPath2 = strPath2.Substring(1);
                }
                return System.IO.Path.Combine(strPath1, strPath2);
            }
            catch{}
            return strPath1 + strPath2;
        }
    }
}
