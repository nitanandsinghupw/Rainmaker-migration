using System;
using System.Data;
using System.Reflection;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.SessionState;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Rainmaker.Web
{
    public partial class SessionManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            msg.Text = "From Page Load: ";
            List<String> str = getOnlineUsers();
            str.Add("Jeff");
            foreach(String s in str)

             {
             tb.Text += s;
             }
            /*

            ClearCookies();

            if (Request.QueryString["closewindow"] != null && Request.QueryString["closewindow"].ToString() == "yes")
            {
                Session.Abandon();
                ScriptManager.RegisterStartupScript(this, typeof(Page), this.UniqueID, "closewindow();", true);
            }
            else
            {
                Session.RemoveAll();
                Session.Abandon();
                Response.Redirect("Default.aspx", true);
            }
             */
        }


        private List<String> getOnlineUsers()
        {
            List<String> activeSessions = new List<String>();
            object obj = typeof(HttpRuntime).GetProperty("CacheInternal", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
            object[] obj2 = (object[])obj.GetType().GetField("_caches", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj);
            for (int i = 0; i < obj2.Length; i++)
            {
                Hashtable c2 = (Hashtable)obj2[i].GetType().GetField("_entries", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(obj2[i]);
                foreach (DictionaryEntry entry in c2)
                {
                    object o1 = entry.Value.GetType().GetProperty("Value", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(entry.Value, null);
                    if (o1.GetType().ToString() == "System.Web.SessionState.InProcSessionState")
                    {
                        SessionStateItemCollection sess = (SessionStateItemCollection)o1.GetType().GetField("_sessionItems", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(o1);
                        if (sess != null)
                        {
                            if (sess["loggedInUserId"] != null)
                            {
                                activeSessions.Add(sess["loggedInUserId"].ToString());
                            }
                        }
                    }
                }
            }
            return activeSessions;
        }


        private void ClearCookies()
        {
            try
            {
                HttpCookie cookie = Response.Cookies["InventiveGuid"];
                cookie.Expires = DateTime.Now;
                //if (cookie != null)
                //{
                //    this.Request.Cookies.Remove("InventiveGuid");
                //}

                //Response.Cookies.Clear();
            }
            catch { }
        }
    }
}
