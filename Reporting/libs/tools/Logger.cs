using System;
using System.IO;

namespace Nezasoft.Tools 
	{
	// Simplest Logger tool in the Universe!!!!
	//
	// Is Singleton, get instance with 'Instance()'
	public class Logger 
		{

		private Logger() 
			{
			if(disabled) return;
			fs = new StreamWriter(@"c:\MyTemp\debug.txt");
			Msg("Opened");
			}

		public void Msg(string msg) 
			{
			if(disabled) return;
			fs.WriteLine("{0}: {1}", DateTime.Now, msg);
			}

		public void Flush() 
			{
			if(disabled) return;
			fs.Flush();
			}

		// Should call before exiting program, DO NOT USE LOG AFTER CALL
		public void Close() 
			{
			if(disabled) return;
			fs.Close();
			}

		public static Logger Instance() 
			{
			if(logger == null) logger = new Logger();
			return logger;
			}

		//
		/// Private variables
		//
		private StreamWriter fs;
		private static Logger logger = null;
		private bool disabled = true;
		}
	}
