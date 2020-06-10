using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Rainmaker.DAL
{
    public class FileAccess
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        public static void WriteEntry(string entry, string sPath)
        {
            try
            {
                if (sPath != string.Empty)
                {
                    entry += Environment.NewLine;
                    string sDirectory = sPath.Replace(Path.GetFileName(sPath), "");
                    if (!Directory.Exists(sDirectory))
                    {
                        Directory.CreateDirectory(sDirectory);
                    }
                    FileStream fs = null;
                    try
                    {
                        fs = new FileStream(sPath, FileMode.Append, System.IO.FileAccess.Write);
                        fs.Write(System.Text.Encoding.ASCII.GetBytes(entry), 0, entry.Length);
                        fs.Flush();
                    }
                    catch (Exception ex)
                    {
                        throw ex;
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
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
