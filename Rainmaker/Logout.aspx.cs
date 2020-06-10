using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Rainmaker.Web
{
    public partial class Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //ClearCookies();

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
        }

        /* private void ClearCookies()
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
        } */
    }
}
