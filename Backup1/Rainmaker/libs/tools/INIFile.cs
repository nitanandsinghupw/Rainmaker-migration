using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Rainmaker.libs.tools
	{
	public class INIFile
		{
		public string path;

		//
		/// These import that native Win32 function calls to read INI files
		//
		[DllImport("kernel32")]
		private static extern long WritePrivateProfileString(string section, string key,string val,string filePath);
		[DllImport("kernel32")]
		private static extern int GetPrivateProfileString(string section, string key,string def, StringBuilder retVal, int size,string filePath);

		public INIFile(string INIPath)
			{
			path = INIPath;
			}
				
		public void IniWriteValue(string Section, string Key, string Value)
			{
			WritePrivateProfileString(Section, Key, Value, this.path);
			}
				
		public string IniReadValue(string Section, string Key)
			{
			StringBuilder temp = new StringBuilder(255);
			int i = GetPrivateProfileString(Section, Key ,"", temp, 255, this.path);
			return temp.ToString();
			}
		}
	}