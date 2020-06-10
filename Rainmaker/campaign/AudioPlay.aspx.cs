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
using Rainmaker.Common.DomainModel;

namespace Rainmaker.Web.campaign
{
    public partial class AudioPlay : System.Web.UI.Page
    {
        public string Audio = "";

        #region Events

        /// <summary>
        /// It plays the selected wave file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Campaign objCampaign = (Campaign)Session["Campaign"];
            if (Request.QueryString["IsEditAudioFileName"] != null)
            {
                //string strFilePath = Request.Url.ToString();
                //strFilePath = strFilePath.Substring(0, strFilePath.IndexOf("/Campaign"));
                //Audio = strFilePath + Request.QueryString["IsEditAudioFileName"].ToString();
                
                Audio = Request.QueryString["IsEditAudioFileName"].ToString();
                ActivityLogger.WriteAdminEntry(objCampaign, "Playing media file: '{0}'", Audio); 
                //Audio = Audio.Replace(@"\\\\", @"\");
                hdnFileToPlay.Value = Audio;
                //lblFileToPlay.Text = Audio;
            }
            if (Request.QueryString["IsBrowseAudioFileName"] != null)
            {
                Audio = Request.QueryString["IsBrowseAudioFileName"].ToString();
            }
        }

        #endregion
    }
}
