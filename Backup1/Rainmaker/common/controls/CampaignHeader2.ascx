<%@ Control Language="C#" AutoEventWireup="true" Codebehind="CampaignHeader.ascx.cs"
    Inherits="Rainmaker.Web.common.controls.CampaignHeader" %>
<!-- Header -->
<script type="text/javascript" language="javascript">
function showHideMenus(){
    var imgsrc = document.getElementById('imgMenuToggle').src;
    if(imgsrc.indexOf("uparrows.gif") == -1){
        document.getElementById('tdMenus').style.display = 'block';    
        document.getElementById('tdMenuSpacer').style.display = 'block';        
        document.getElementById('imgMenuToggle').src = '../images/uparrows.gif';
    }else{
   	    document.getElementById('tdMenus').style.display = 'none';     
   	    document.getElementById('tdMenuSpacer').style.display = 'none';       
        document.getElementById('imgMenuToggle').src = '../images/downarrows.gif';
    }
}
</script>

<link rel="stylesheet" href="../css/menu_style.css" type="text/css" />

<table cellpadding="0" cellspacing="0" border="0" width="100%">
    <tr>
        <td>
	<OBJECT classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000"
  codebase="http://active.macromedia.com/flash2/cabs/swflash.cab#version=4,0,0,0"
  ID=Banner1 WIDTH=489 HEIGHT=140>
<PARAM NAME=movie VALUE="../images/rainmakerclick.swf">
<PARAM NAME=quality VALUE=high>
<PARAM NAME=loop VALUE=false>
<EMBED src="../images/rainmakerclick.swf" loop=false quality=high
  WIDTH=489 HEIGHT=140 TYPE="application/x-shockwave-flash"
  PLUGINSPAGE="http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash">
</EMBED>
</OBJECT>


            </td>
    </tr>
    <tr>
        <td class="tdSpacer">
            <img src="../images/spacer.gif" height="1px" alt=""/></td>
    </tr>
    <tr>
        <td class="tdSetting">
            <table cellpadding="0" border="0" cellspacing="0" width="100%">
                <tr>
                    <td width="81%" class="tdHdr">
                        Dialer Global Settings</td>
                    <td width="7%" align="right" class="tdHdr">
                        <asp:LinkButton ID="lbtnShutown" Text="Shutdown" OnClientClick="return confirm('Are you sure you would like to shut down all currently running campaigns?')" OnClick="lbtnShutdown_Click" runat="server" class="aMenu" ToolTip="Shutdown all running campaigns."></asp:LinkButton> 
                    <td width="7%" align="left" class="tdHdr">
                        <a href="../Logout.aspx" class="aMenu">Logout</a></td>
                    <td width="5%" align="center">
                        <a href="javascript:void(0);" onclick="javascript:showHideMenus();"><img src="../images/uparrows.gif" alt="Click Here Hide or Display Menus" border="0" id="imgMenuToggle" /></a></td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tdSpacer">
            <img src="../images/spacer.gif" height="1px" /></td>
    </tr>
    <tr>
        <td height="25px" id="tdMenus">
            <table cellpadding="0" border="0" cellspacing="0" width="100%">
                <tr>
                    <td width="100%">
                        <%--<a href="../campaign/CampaignList.aspx" class="aMenu">Campaign</a>|<a href="#" onmouseover='FW_showMenu(window.Menu01,Menu01Left,MenuTop);' onmouseout='mouseoutMenu();' class="aMenu">Configuration</a>|<a href="#" class="aMenu">Reports</a>|<a href="#" class="aMenu">Help</a></td>--%>
                   <!--     <a href="../campaign/CampaignList.aspx" class="aMenu">Campaign</a>|<a href="#" onmouseover='FW_showMenu(window.Menu01,Menu01Left,MenuTop);' onmouseout='mouseoutMenu();' class="aMenu">Configuration</a>|<a href="../campaign/DataPortal.aspx" class="aMenu">Data Manager</a>|<a href="#" class="aMenu">Help</a></td> -->
                        				   <div id="menu">
    <ul>
       <li><a href="../campaign/Campaignlist.aspx" target="_self" title="Campaign"><span>Campaigns</span></a></li>
	       <li><a href="../agent/agentlist.aspx" target="_self" title="agent List"><span>Agents</span></a></li>
       <li><a href="../agent/stationlist.aspx" target="_self" title="Stations"><span>Stations</span></a></li>
       <li><a href="../campaign/Callmerecorder.aspx" target="_self" title="Call Me Recorder"><span>Call Me Recorder</span></a></li>
       <li><a href="../campaign/globaldialingparams.aspx" target="_self" title="Global"><span>Global Parameters</span></a></li>
       <%-- <li><a href="../campaign/dataportal.aspx" target="_self" title="Data Manager"><span>Data Manager</span></a> </li> --%>
	   <li><a href="../campaign/CampaignDeleted.aspx" target="_parent" title="Deleted Campaigns"><span>Deleted Campaigns</span></a></li>
	   <li><a href="../campaign/MasterDNC.aspx" target="_parent" title="Master DNC"><span>Master DNC</span></a></li>
	   <li><a onclick="var win = window.open('../reporting/reportmaker.aspx','rainmakerpopupwindow','scrollbars=1,resizable=yes');win.moveTo(0,0);win.resizeTo(screen.width,screen.height);win.focus();" title="Rainmaker Reports">
                        <span>Reports</span></a> </li>
	   <%-- <li><a href="../help/manual.aspx" target="_New" title="Manual"><span>Help</span></a></li> --%>
	   
				 </ul> 
			    </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td class="tdSpacer" id="tdMenuSpacer">
            <img src="../images/spacer.gif" height="1px" /></td>
    </tr>

</table>
<!-- Header -->
