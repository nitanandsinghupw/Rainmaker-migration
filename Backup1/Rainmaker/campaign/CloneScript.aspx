<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CloneScript.aspx.cs" Inherits="Rainmaker.Web.campaign.CloneScript" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Clone Script</title>

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
    <form id="frmCloneScript" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <div>
            <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                            <tr>
                                <td>
                                    <RainMaker:Header ID="campaignHeader" runat="server"></RainMaker:Header>
                                    <!-- Body -->
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%" height="400px">
                                        <tr>
                                            <td valign="top" align="center">
                                                <table cellpadding="0" cellspacing="1" border="0" width="50%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="2" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                <tr>
                                                                    <td align="left" width="50%">
                                                                        <table cellpadding="5" cellspacing="2" width="100%" border="0">
                                                                            <tr>
                                                                                <td align="Left" colspan="2">
                                                                                    <b>
                                                                                        Save Script '<asp:Label ID="lblScriptName" runat="server"></asp:Label>' as</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" width="1%" nowrap>
                                                                                    <b>Campaign&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                <td align="Left"><asp:DropDownList ID="ddlCampaign" runat="server" CssClass="select1" Width="150px">
                                                                                    </asp:DropDownList></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="Left">
                                                                                    <b>Save as&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                <td align="Left"><asp:TextBox ID="txtScript" runat="server" CssClass="txtmedium" MaxLength="50">
                                                                                    </asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="reqscript" runat="server" SetFocusOnError="true"
                                                                                        ControlToValidate="txtScript" ErrorMessage="Select Script Name" Text="*" Display="Static"></asp:RequiredFieldValidator>
                                                                                </td>
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
                                        <tr valign="bottom">
                                            <td align="right" valign="bottom">
                                                <table cellspacing="1" width="100%" cellpadding="4" border="0" id="Table4">
                                                    <tr valign="bottom">
                                                        <td align="right" valign="bottom">
                                                            <asp:LinkButton ID="lbtnsave" runat="server" OnClick="lbtnSave_Click"><img src="../images/save.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;
                                                            <asp:LinkButton ID="lbtnClose" runat="server" OnClick="lbtnClose_Click" CausesValidation="false"><img src="../images/close.jpg" border="0" /></asp:LinkButton></td>
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
        </div>
    </form>
</body>
</html>
