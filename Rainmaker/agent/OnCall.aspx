<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OnCall.aspx.cs" Inherits="Rainmaker.Web.agent.OnCall" %>
<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>On Call</title>
    <script language="javascript" type="text/javascript">
    <!--
        var cbrowser = navigator.userAgent;        
        if(cbrowser.toLowerCase().indexOf("firefox") > 0){
            document.write('<link rel="stylesheet" href="../css/RainmakerNS.css" type="text/css" />');
        }else{
            document.write('<link rel="stylesheet" href="../css/Rainmaker.css" type="text/css" />');
        }
    //-->    
    </script> 
    <script language="javascript" type="text/javascript" src="../js/Rainmaker.js"></script>
</head>
<body onload="ShowPageMessage();">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
    <div>
        <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                    <tr>
                        <td>
                            <!-- Header -->
                            <RainMaker:Header ID="campaignHeader" runat="server"></RainMaker:Header>
                            <!-- Header -->
                            <!-- Body -->
                            <table cellpadding="0" cellspacing="0" height="400px" border="0" width="100%">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%" style="height:450px;">
                                                        <tr>
                                                            <td width="90%">
                                                              
                                                            </td>
                                                            <td width="10%" class="tdSideBar">
                                                                <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <a id="lbtnPause">
                                                                                <img src="" alt="Pause/Ready" /></a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <a id="lbtnCallback" disabled="disabled">
                                                                                <img src="" alt="Callback" /></a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <a id="lbtnScheduleCalendar" disabled="disabled">
                                                                                <img src="" alt="ScheduleCalendar" /></a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <a id="lbtnRecord" disabled="disabled">
                                                                                <img src="" alt="Record" /></a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <a id="lbtnConference" disabled="disabled">
                                                                                <img src="" alt="Conference" /></a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <a id="lbtnTransfer" disabled="disabled">
                                                                                <img src="" alt="Transfer" /></a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <a id="lbtnHangup" disabled="disabled">
                                                                                <img src="" alt="Hangup" /></a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <a id="lbtnCallDisposition" disabled="disabled">
                                                                                <img src="" alt="Call Disposition"></a></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <!-- Body -->
                            <!-- Footer -->
                          <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                            <!-- Footer -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </div>
    </form>
<%--    <script language="javascript" type="text/javascript">
        function window.onload()
        {
            window.attachEvent("onbeforeunload", OnClose);
        }

        function OnClose()
        {
            event.returnValue = "Are you sure you want to close the browser?  If you are in a call, this could cause the loss of important data.";
        }
    </script>--%>
</body>
</html>
