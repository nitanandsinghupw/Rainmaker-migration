<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CallMeRecorder.aspx.cs"
    Inherits="Rainmaker.Web.campaign.CallMeRecorder" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader2.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>"Call Me" - Prompt Recorder</title>
    <script language="javascript" type="text/javascript">
    <!--
        var cbrowser = navigator.userAgent;        
        if(cbrowser.toLowerCase().indexOf("firefox") > 0){
            document.write('<link rel="stylesheet" href="../css/RainmakerNS.css" type="text/css" />');
        }else{
            document.write('<link rel="stylesheet" href="../css/Rainmaker.css" type="text/css" />');
        }
    //--> 
    function AlertRequestSubmitted()
    {
        if ( document.getElementById("hdnSubmitted").value=="yes")
        {
            alert('Your call request has been submitted.\r\nThe dialer will call your number shortly.\r\nMake sure the dialer is running, if not your call request will expire in 15 minutes.');
            __doPostBack('__Page', 'MyCustomArgument');   
        }   
    }   
    </script> 

    <script language="javascript" type="text/javascript" src="../js/Rainmaker.js"></script>

</head>
<body onload="ShowPageMessage();AlertRequestSubmitted();">
    <form id="form1" runat="server" defaultbutton="lbtnOK" defaultfocus="txtPhoneNumber">
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
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td valign="middle" align="center">
                                                <table cellpadding="0" cellspacing="1" border="0" width="60%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="2" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                <tr>
                                                                    <td><img src="../images/spacer.gif" height="5px" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="center">
                                                                        <asp:label runat="server" Font-Size="Smaller" Text="This feature will trigger the dialer to call the number you enter and provide you with an IVR to record prompts to sound files on the server.">
                                                                        </asp:label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="../images/spacer.gif" height="10px" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <label>
                                                                                        <b>Phone Number to Call:&nbsp;</b></label>&nbsp;&nbsp;
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="txtPhoneNumber" runat="server" Text=""  MaxLength="15" CssClass="txtmedium"></asp:TextBox>
                                                                                    <%--<asp:RequiredFieldValidator ID="reqGlobalDialingPrefix" runat="server" ControlToValidate="txtGlobalDialingPrefix"
                                                                                        ErrorMessage="Please Enter Global Dialing Prefix" SetFocusOnError="True" Display="static">*</asp:RequiredFieldValidator>--%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td><img src="../images/spacer.gif" height="10px" /></td>
                                                                </tr>
                                                                <tr valign="bottom">
                                                                    <td valign="bottom" align="left" colspan="2">
                                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                            <tr>
                                                                                <td valign="middle" align="right">
                                                                                    <asp:LinkButton ID="lbtnOK" OnClick="lbtnOK_Click" runat="server" CausesValidation="false" OnClientClick="javascript:return confirm('Do you want the dialer to call and record prompts now?');"
                                                                                        ToolTip="Enter a phone number, click here and the dialer will call you." Text="Ok" CssClass="button blue small"></asp:LinkButton>&nbsp;&nbsp;
                                                                                    <asp:LinkButton ID="lbtnCancel" runat="server" CausesValidation="false" OnClientClick="javascript:return confirm('Do you want to cancel the call?');"
                                                                                        OnClick="lbtnCancel_Click" Text="Cancel" CssClass="button blue small"></asp:LinkButton>&nbsp;&nbsp;
                                                                                    <asp:HiddenField ID="hdnSubmitted" runat="server" />
                                                                                    <asp:Literal ID="ltrlClose" runat="server"></asp:Literal>
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
</body>
</html>
