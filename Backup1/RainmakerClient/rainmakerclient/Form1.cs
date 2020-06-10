
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;

using System.Runtime.InteropServices;
using mshtml;
using MsHtmHstInterop;
using AxSHDocVw;

namespace leadsweeper
{
    public partial class Form1 : Form , IDocHostUIHandler
    {
     

        public Form1()
        {
            InitializeComponent();

            
            object flags = 0;
            object targetFrame = String.Empty;
            object postData = String.Empty;
            object headers = String.Empty;
            this.axWebBrowser1.Navigate("about:blank"); //, ref flags, ref targetFrame,
                                     //ref postData, ref headers);
           
            ICustomDoc cDoc = (ICustomDoc)this.axWebBrowser1.Document;
            cDoc.SetUIHandler((IDocHostUIHandler)this);

            var starturl = ConfigurationManager.AppSettings["starturl"];
            
            this.axWebBrowser1.Navigate(starturl);
        }
        [DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);
        [DllImport("user32.dll")]
        private static extern Int32 EnableMenuItem ( System.IntPtr hMenu , Int32 uIDEnableItem, Int32 uEnable);
        private const Int32 HTCAPTION = 0x00000002;
        private const Int32 MF_BYCOMMAND =0x00000000;
        private const Int32 MF_ENABLED =0x00000000;
        private const Int32 MF_GRAYED =0x00000001;
        private const Int32 MF_DISABLED =0x00000002;
        private const Int32 SC_MOVE = 0xF010;
        private const Int32 WM_NCLBUTTONDOWN = 0xA1;
        private const Int32 WM_SYSCOMMAND = 0x112;
        private const Int32 WM_INITMENUPOPUP = 0x117;
        private bool Moveable = false;
        protected override void WndProc(ref System.Windows.Forms.Message m )
        {
        if( m.Msg == WM_INITMENUPOPUP )
        {
        //handles popup of system menu
        if ((m.LParam.ToInt32() / 65536) != 0 ) // 'divide by 65536 to get hiword
        {
        Int32 AbleFlags = MF_ENABLED;
        if (!Moveable)
        {
        AbleFlags = MF_DISABLED | MF_GRAYED; // disable the move
        }
        EnableMenuItem(m.WParam, SC_MOVE, MF_BYCOMMAND | AbleFlags);
        }
        }if(!Moveable)
        {
        if(m.Msg==WM_NCLBUTTONDOWN) //cancels the drag this is IMP
        {if(m.WParam.ToInt32()==HTCAPTION) return;
        }
        if (m.Msg==WM_SYSCOMMAND) // Cancels any clicks on move menu
        {
        if ((m.WParam.ToInt32() & 0xFFF0) == SC_MOVE) return;
        }
        }
        base.WndProc(ref m);
        }
        System.Drawing.Point initialLocation; 
        protected override void OnFormClosing(FormClosingEventArgs e)
        {

        //closereasons Summary
        //None = 0, //The cause of the closure was not defined or could not be determined.
        //WindowsShutDown = 1, //The operating system is closing all applications before shutting down.
        //MdiFormClosing = 2, //The parent form of this multiple document interface (MDI) form is closing.
        //UserClosing = 3, //The user is closing the form through the user interface (UI), for example
        //     by clicking the Close button on the form window, selecting Close from the
        //     window's control menu, or pressing ALT+F4.
        //TaskManagerClosing = 4, //The Microsoft Windows Task Manager is closing the application.        
        //FormOwnerClosing = 5, //The owner form is closing.
            //ApplicationExitCall = 6, //The System.Windows.Forms.Application.Exit() method of the System.Windows.Forms.Application
        //     class was invoked.



        /////////////////////////////////////////////////    
            
       
            
            if (e.CloseReason == CloseReason.UserClosing && (!axWebBrowser1.LocationURL.Contains("default.aspx")) && (!axWebBrowser1.LocationURL.Contains("/Default.aspx")) && !axWebBrowser1.LocationURL.Contains("reportmaker.aspx"))
            {
                
                MessageBox.Show("You cannot exit the program from here.  If you really want to close down the program go back to the login screen first. ", "FormClosing Event");
                e.Cancel = true;
            }
            else if (e.CloseReason == CloseReason.UserClosing && axWebBrowser1.LocationURL.Contains("reportmaker.aspx"))
            {
                
            }
         
            
        
            /*System.Text.StringBuilder messageBoxCS = new System.Text.StringBuilder();
            messageBoxCS.AppendFormat("{0} = {1}", "CloseReason", e.CloseReason);
            messageBoxCS.AppendLine();
            messageBoxCS.AppendFormat("{0} = {1}", "Cancel", e.Cancel);
            messageBoxCS.AppendLine();
            var form = new Form
            {
                StartPosition = FormStartPosition.Manual,
                ShowInTaskbar = false,
                Location = new Point(100, 100)
            };
            var text1 = new TextBox
            {
                Text = messageBoxCS.ToString()
            };
            text1.Width = 500;
            form.Controls.Add(text1);
           
            form.ShowDialog(); */

            //MessageBox.Show(messageBoxCS.ToString(), "FormClosing Event");
            
        }

        void axWebBrowser1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {

            switch (e.KeyData) {

                case Keys.Control | Keys.A: // Select All
                case Keys.Control | Keys.C: // Copy
                case Keys.Control | Keys.V: // Paste
                case Keys.Delete: // Delete
                    e.IsInputKey = true;
                    break;
                case Keys.Back:
                case Keys.BrowserBack:
                
                    e.IsInputKey = true;
                    break;
                default:
                    e.IsInputKey = false;
                    break;
                
            }
            
            if (this.ActiveControl.Enabled == false) {
                
                e.IsInputKey = false;
               
            }
            
        }

        // NewWindow3 event, used on Windows XP SP2 and higher
        private void axWebBrowser1_NewWindow3(object sender, AxSHDocVw.DWebBrowserEvents2_NewWindow3Event e)
        {
            Form1 frmWB;
            frmWB = new Form1();

            frmWB.axWebBrowser1.RegisterAsBrowser = true;
            e.ppDisp = frmWB.axWebBrowser1.Application;
            frmWB.Visible = true;

            frmWB.TopMost = true;
        }
        private void axWebBrowser1_NewWindow2(object sender, AxSHDocVw.DWebBrowserEvents2_NewWindow2Event e)
        {

            Form1 frmWB;
            frmWB = new Form1();

            frmWB.axWebBrowser1.RegisterAsBrowser = true;
            e.ppDisp = frmWB.axWebBrowser1.Application;
            frmWB.Visible = true;
            
        }
        /*bool axWebBrowser1_OnContextMenu(IHTMLEventObj e)
        {
            MessageBox.Show("No menu sorry!");
            return false;
        }*/

        private void Form1_Load(object sender, EventArgs e)
        {

           
            initialLocation = this.Location;
            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void Form1_LocationChanged(object sender, EventArgs e)
        {
            this.Location = initialLocation;
        }

        void IDocHostUIHandler.EnableModeless(int fEnable)
        {

        }

        void IDocHostUIHandler.FilterDataObject(MsHtmHstInterop.IDataObject pDO, out MsHtmHstInterop.IDataObject ppDORet)
        {
            ppDORet = null;
        }

        void IDocHostUIHandler.GetDropTarget(MsHtmHstInterop.IDropTarget pDropTarget, out MsHtmHstInterop.IDropTarget ppDropTarget)
        {
            ppDropTarget = null;
        }

        void IDocHostUIHandler.GetExternal(out object ppDispatch)
        {
            ppDispatch = null;
        }

        void IDocHostUIHandler.GetHostInfo(ref _DOCHOSTUIINFO pInfo)
        {

        }

        void IDocHostUIHandler.GetOptionKeyPath(out string pchKey, uint dw)
        {
            pchKey = null;
        }

        void IDocHostUIHandler.HideUI()
        {

        }

        void IDocHostUIHandler.OnDocWindowActivate(int fActivate)
        {

        }

        void IDocHostUIHandler.OnFrameWindowActivate(int fActivate)
        {

        }

        void IDocHostUIHandler.ResizeBorder(ref MsHtmHstInterop.tagRECT prcBorder, IOleInPlaceUIWindow pUIWindow, int fRameWindow)
        {

        }

        void IDocHostUIHandler.ShowContextMenu(uint dwID, ref MsHtmHstInterop.tagPOINT ppt, object pcmdtReserved, object pdispReserved)
        {
            const int Ok = 0;
            throw new COMException("", Ok); // returns HRESULT = S_OK;
        }

        void IDocHostUIHandler.ShowUI(uint dwID, IOleInPlaceActiveObject pActiveObject, IOleCommandTarget pCommandTarget, IOleInPlaceFrame pFrame, IOleInPlaceUIWindow pDoc)
        {

        }

        void IDocHostUIHandler.TranslateAccelerator(ref tagMSG lpmsg, ref Guid pguidCmdGroup, uint nCmdID)
        {
            const int Ok = 0;
            const int Error = 1;
            const int VK_BACK = 0x08;
            const int WM_KEYDOWN = 0x0100;
            const int WM_KEYUP = 0x0101;
            const int VK_CONTROL = 0x11;
            const int VK_ALT = 0x12;


            if (lpmsg.message != WM_KEYUP)
            {
                if (GetAsyncKeyState(VK_BACK) < 0)
                {
                    mshtml.IHTMLDocument2 htmlDoc = axWebBrowser1.Document as mshtml.IHTMLDocument2;
                    mshtml.IHTMLElement htmlElement = htmlDoc.activeElement as mshtml.IHTMLElement;
                    string activeTag = htmlElement.tagName.ToLower();
                    if (activeTag == "a")
                    {
                        throw new COMException("", Ok); // returns HRESULT = S_OK
                    }
                }

            }
            if (lpmsg.message != WM_KEYDOWN)
                // allow message
                throw new COMException("", Error); // returns HRESULT = S_FALSE

            if (GetAsyncKeyState(VK_CONTROL) < 0) //CONTROL KEY PRESSED
            {
                // disable all control keys except Ctrl-A Ctrl-C and Ctrl-V
                lpmsg.lParam &= 0xFF; // get the virtual keycode
                if ((lpmsg.lParam != 'A') || ((lpmsg.lParam != 'C')) || ((lpmsg.lParam != 'V')))
                    throw new COMException("", Ok); // returns HRESULT = S_OK
            }
            else if (GetAsyncKeyState(VK_ALT) < 0) //ALT KEY PRESSED
            {
                //disable all Alt key actions
                throw new COMException("", Ok); // returns HRESULT = S_OK

            }
            else
            {
                // Ctrl key not pressed: allow message
                mshtml.IHTMLDocument2 htmlDoc = axWebBrowser1.Document as mshtml.IHTMLDocument2;
                mshtml.IHTMLElement htmlElement = htmlDoc.activeElement as mshtml.IHTMLElement;
                string activeTag = htmlElement.tagName.ToLower();
                string activeElementType = htmlElement.getAttribute("type",0).ToString() ;
                
                if (GetAsyncKeyState(VK_BACK) < 0)
                {
                    if ((activeTag == "input" && activeElementType == "text") || activeTag == "textarea" || activeTag == "text" || activeTag == "password")
                    {
                        throw new COMException("", Error); // returns HRESULT = S_FALSE

                    }
                    else
                    {
                        throw new COMException("", Ok); // returns HRESULT = S_OK
                    }
                }
            }
            // allow everything else
            throw new COMException("", Error); // returns HRESULT = S_FALSE

        }

        void IDocHostUIHandler.TranslateUrl(uint dwTranslate, ref ushort pchURLIn, IntPtr ppchURLOut)
        {

        }

        void IDocHostUIHandler.UpdateUI()
        {

        }        
        
    }
    
    }
