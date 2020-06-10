using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Reflection;

namespace Rainmaker.RainmakerDialer
{
    public partial class Rainmaker : Form
    {

        #region Variables

        // Reference to dialor engine
        private DialerEngine dialerEngine = new DialerEngine();

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public Rainmaker()
        {
            System.Threading.Thread.CurrentThread.Name = "RainmakerMain";
            InitializeComponent();

            SetDefaultValues();
            dialerEngine.StartLogging();
        }

        #endregion

        #region Events

        /// <summary>
        /// Invoked when user clicks on connect button
        /// This will creates a connection to telephony server
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (DialerEngine.TelephonyServer != null) return;

            Cursor = Cursors.WaitCursor;
            try
            {
                bool bBadCredentials = false;

                if (txtTelephonyServer.Text == null || txtTelephonyServer.Text.Length == 0) bBadCredentials = true;
                if (txtUsername.Text == null || txtUsername.Text.Length == 0) bBadCredentials = true;
                if (txtPassword.Text == null || txtPassword.Text.Length == 0) bBadCredentials = true;

                if (bBadCredentials)
                {
                    MessageBox.Show("You must specify a Server, Username and Password to connect.");
                }
                try
                {

                    bool blConnectd = dialerEngine.Connect(txtTelephonyServer.Text,
                        txtUsername.Text, txtPassword.Text);
                    if (blConnectd)
                    {
                        lblStatus.Text = "CONNECTED";
                        lblStatus.ForeColor = Color.Blue;
                        btnStart.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    DialerEngine.Log.Write("Exception: " + ex.Message);
                    return;
                }
            }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Invoked when user clicks on dialer start button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialerEngine.TelephonyServer == null)
                {
                    MessageBox.Show("You must log into the telephony server first!");
                    return;
                }
                this.Cursor = Cursors.WaitCursor;
                btnStart.Enabled = false;
                dialerEngine.Start();

            }
            catch { }
            finally
            {
                Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// Invoked when user clicks on disconnect button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;
            btnStart.Enabled = true;
            if (DialerEngine.TelephonyServer != null)
            {
                dialerEngine.Disconnect();
                lblStatus.Text = "Not Connected";
                lblStatus.ForeColor = Color.Red;
            }
        }

        /// <summary>
        /// form closing event, dispose telephony server if exists
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rainmaker_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                dialerEngine.Disconnect();
            }
            catch { }

            Application.Exit();
        }

        /// <summary>
        /// Timer event to log actions
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer2_Tick(object sender, EventArgs e)
        {
            lock (DialerEngine.qMessageQueue)
            {
                while (DialerEngine.qMessageQueue.Count > 0)
                {
                    string message = DialerEngine.qMessageQueue.Dequeue();
                    if (txtLog.Text.Length > 15000)
                    {
                        txtLog.Text = txtLog.Text.Substring(txtLog.Text.Length - 15000) + message;
                    }
                    else
                    {
                        txtLog.Text = txtLog.Text + message;
                    }

                    txtLog.SelectionStart = txtLog.Text.Length;
                    txtLog.ScrollToCaret();

                }
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Set default telephony server settings
        /// </summary>
        private void SetDefaultValues()
        {
            txtTelephonyServer.Text = Utilities.GetAppSetting("TelephonyServer", "192.168.0.75");
            txtUsername.Text = Utilities.GetAppSetting("Username", "username");
            txtPassword.Text = Utilities.GetAppSetting("Password", "password");

            ID_TXT_Version.Text = String.Format("Dialer Version: {0}", Assembly.GetExecutingAssembly().GetName().Version);
        }

        #endregion

    }
}