using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Rainmaker.RainmakerDialer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            try
            {
                Application.Run(new Rainmaker());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
                if (ex.InnerException != null) System.Diagnostics.Debug.WriteLine("  " + ex.InnerException.Message);

                if (ex.InnerException == null)
                {
                    MessageBox.Show(ex.Message, "Exception");
                }
                else
                {
                    MessageBox.Show(ex.Message + "\r\n\r\n" + ex.InnerException.Message, "Exception");
                }
                Application.Exit();
            }
        }
    }
}