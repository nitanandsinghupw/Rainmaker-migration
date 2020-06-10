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
using Rainmaker.Web.CampaignWS;
using Rainmaker.Web.DataAccess;

namespace Rainmaker.Web.common.controls
{
    public partial class CampaignHeader : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void lbtnShutdown_Click(object sender, EventArgs e)
        {
            ActivityLogger.WriteAdminEntry(null, "Admin Action:  lbtnShutdown button clicked by Admin. Global shutdown to idle all campaigns has been initiated.");
            CampaignService campaignService = new CampaignService();

            campaignService.ShutdownAllCampaigns();
        }
    }
}
