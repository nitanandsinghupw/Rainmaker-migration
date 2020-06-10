<%@ Page Language="C#" AutoEventWireup="true" Codebehind="StationDetail.aspx.cs"
    Inherits="Rainmaker.Web.agent.StationDetail" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader2.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Station Detail</title>
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
    <script language="javascript" type="text/javascript" src="../js/RainMaker.js"></script>

    <script language="JavaScript" type="text/javascript">
        function isValidIPAddress(oSrc, args) 
        {
            args.IsValid = false;
            var re = /^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$/;
            if (re.test(args.Value)) {
                var parts = args.Value.split(".");
                if (parseInt(parseFloat(parts[0])) == 0) { return; }
                for (var i=0; i<parts.length; i++) {
                    if (parseInt(parseFloat(parts[i])) > 255) { return; }
                }
                args.IsValid = true;
            }
        }
    </script>

</head>
<body onload="ShowPageMessage();">
    <form id="frmStationDetail" runat="server" defaultbutton="lbtnSave" defaultfocus="txtStationIP">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                        <tr>
                            <td>
                                <RainMaker:Header ID="campaignHeader" runat="server"></RainMaker:Header>
                                <!-- Body -->
                                <table cellpadding="0" cellspacing="0" height="200px" border="0" width="100%">
                                    <tr>
                                        <td valign="middle" align="center">
                                            <table cellpadding="2" cellspacing="0" border="0" width="60%">
                                                <tr>
                                                    <td valign="top">
                                                        <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                                            <tr>
                                                                <td align="right" nowrap>
                                                                    <b>Station IP/Name&nbsp;:&nbsp;</b></td>
                                                                <td align="left">
                                                                    &nbsp;<asp:TextBox ID="txtStationIP" runat="server" CssClass="txtnormal" MaxLength="255"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="reqStationIP" runat="server" SetFocusOnError="true" ControlToValidate="txtStationIP"
                                                                        Text="*" ErrorMessage="Enter Station IP" Display="static"></asp:RequiredFieldValidator><%--<asp:CustomValidator ID="CustomValidator1"
                                                                            runat="server" ControlToValidate="txtStationIP" ErrorMessage="Enter Valid IP Address"
                                                                            Text="*" ClientValidationFunction="isValidIPAddress" Display="static">
                                                                        </asp:CustomValidator>--%>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" nowrap>
                                                                    <b>Station Number&nbsp;:&nbsp;</b></td>
                                                                <td align="left">
                                                                    &nbsp;<asp:TextBox ID="txtStationNumber" runat="server" CssClass="txtnormal" MaxLength="50"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="reqStationNumber" runat="server" SetFocusOnError="true" ControlToValidate="txtStationNumber" Text="*"
                                                                        ErrorMessage="Enter Station Number" Display="static"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td align="left" nowrap>
                                                                    <asp:CheckBox ID="chkAllwaysOffhook" runat="server" checked="true"></asp:CheckBox><b>&nbsp;Allways
                                                                        Off-Hook</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left">
                                                                    <asp:HiddenField runat="server" ID="hdnStationId" />
                                                                    <asp:ValidationSummary ID="valSummary" runat="server" ShowMessageBox="True" ShowSummary="False" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr valign="bottom">
                                        <td align="right" valign="bottom">
                                            <table cellspacing="5" width="100%" cellpadding="5" border="0" id="Table4">
                                                <tr valign="bottom">
                                                    <td align="right" colspan="2">
                                                        <asp:LinkButton ID="lbtnSave" OnClick="lbtnSave_Click" runat="server"><img src="../images/save.jpg" border="0"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                            ID="lbtnCancel" runat="server" CausesValidation="false" OnClick="lbtnCancel_Click"
                                                            OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"><img src="../images/cancel.jpg" border="0"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                ID="lbtnClose" PostBackUrl="~/agent/StationList.aspx" CausesValidation="false"
                                                                runat="server"><img src="../images/close.jpg" border="0" /></asp:LinkButton>&nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <!-- Body -->
                                <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
