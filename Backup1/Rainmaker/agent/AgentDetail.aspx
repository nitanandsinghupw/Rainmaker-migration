<%@ Page Language="C#" AutoEventWireup="true" Codebehind="AgentDetail.aspx.cs" Inherits="Rainmaker.Web.agent.AgentDetail" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader2.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Agent Detail</title>
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
    <script language="javascript" type="text/javascript" src="../js/jquery.js"></script>
    
    <script language="javascript" type="text/javascript">

        function jqueryfunctions() {

            $(document).ready(function() {

                $("#lbtnSave").click(function() {

                    var message = "";

                    if ($("#txtAgentname").val() == "") {
                        message += "Agent Name\n\n";
                    }
                    if ($("#txtLoginName").val() == "") {
                        message += "Login Name\n\n";
                    }
                    if ($("#txtPassword").val() == "") {
                        message += "Password\n\n";
                    }
                    if ($("#chkIsAdmin").is(':checked') == false) {
                        if ($("#txtPhoneNumber").val() == "") {
                            message += "Phone Number\n";
                        }
                    }

                    if (message != "") {
                        var messageheader = "The following input fields are required and are missing: \n\n";
                        alert(messageheader + message);
                        return false;
                    }
                    return true;
                });

            });
        }
    
    </script>
    
</head>
<body onload="ShowPageMessage();">
    <form id="frmAgentDetail" runat="server" defaultbutton="lbtnSave" defaultfocus="txtAgentname">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <table cellpadding="0" cellspacing="1"  style="border:0" width="992px" class="tdHeader">
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0"  style="border:0" width="100%" class="tdWhite">
                        <tr>
                            <td>
                                <RainMaker:Header ID="campaignHeader" runat="server"></RainMaker:Header>
                                <!-- Body -->
                                <table cellpadding="0" cellspacing="0" style="border:0 ; width:100% ; height:400px ">
                                    <tr>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="5"  style="border:0" width="100%">
                                                <tr>
                                                    <td valign="top">
                                                        <table cellpadding="1" cellspacing="2"  style="border:0" width="100%">
                                                            <tr>
                                                                <td align="right" style="white-space:nowrap; width:10%">
                                                                    <b>Agent Name&nbsp;:&nbsp;</b></td>
                                                                <td align="left" style="width:70%">
                                                                    &nbsp;<asp:TextBox ID="txtAgentname" runat="server" CssClass="txtnormal" MaxLength="50"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" style="white-space:nowrap">
                                                                    <b>Login Name&nbsp;:&nbsp;</b></td>
                                                                <td align="left" style="width:70%">
                                                                    &nbsp;<asp:TextBox ID="txtLoginName" runat="server" CssClass="txtnormal" MaxLength="8"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="reqtxtLoginName" runat="server" ControlToValidate="txtLoginName" Text="*"
                                                                        ErrorMessage="Enter Login Name" Display="static" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" style="white-space:nowrap">
                                                                    <b>Password&nbsp;:&nbsp;</b></td>
                                                                <td align="left" style="width:70%">
                                                                    &nbsp;<asp:TextBox ID="txtPassword" runat="server" CssClass="txtnormal" TextMode="Password" MaxLength="8"></asp:TextBox><asp:RequiredFieldValidator
                                                                        ID="reqtxtPassword" runat="server" ControlToValidate="txtPassword" Text="*" ErrorMessage="Enter Password"
                                                                        Display="static"  SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td valign="middle" align="left" style="white-space:nowrap">
                                                                    <asp:CheckBox ID="chkIsAdmin" runat="server" Text="Is Administrator" CssClass="checkboxBold"/></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td valign="middle" align="left" style="white-space:nowrap">
                                                                    <asp:CheckBox ID="chkAllowManDial" runat="server" Text="Allow Manual Dial" CssClass="checkboxBold"/></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td valign="middle" align="left" style="white-space:nowrap">
                                                                    <asp:CheckBox ID="chkIsVerificationAgent" runat="server" Text="Is Verification Agent" CssClass="checkboxBold"/></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td valign="middle" align="left" style="white-space:nowrap">
                                                                    <asp:CheckBox ID="chkIsBoundAgent" runat="server" Text="Is In Bound Agent" CssClass="checkboxBold"/></td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" style="white-space:nowrap">
                                                                    <b>Phone Number&nbsp;:&nbsp;</b></td>
                                                                <td align="left" style="width:70%">
                                                                    &nbsp;<asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="txtnormal" MaxLength="20"></asp:TextBox>
                                                                    <%--<asp:RequiredFieldValidator
                                                                        ID="reqtxtPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" Text="*"
                                                                        ErrorMessage="Enter Phone Number" Display="static"></asp:RequiredFieldValidator>--%>
                                                                        <asp:HiddenField runat="server" ID="hdnAgentId" /><asp:HiddenField runat="server" ID="hdnAgentP" />
                                                                    <asp:ValidationSummary ID="valSummary" runat="server" ShowMessageBox="True"
                                                                        ShowSummary="False" />
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
                                            <table cellspacing="5" width="100%" cellpadding="5"  style="border:0" id="Table4">
                                                <tr valign="bottom">
                                                    <td align="right" colspan="2">
                                                        <asp:LinkButton ID="lbtnSave" OnClick="lbtnSave_Click" runat="server"><img src="../images/save.jpg" style="border:0"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                            ID="lbtnCancel" runat="server" CausesValidation="false" OnClick="lbtnCancel_Click"
                                                            OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"><img src="../images/cancel.jpg" style="border:0"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                ID="lbtnClose" PostBackUrl="~/agent/AgentList.aspx" CausesValidation="false"
                                                                runat="server"><img src="../images/close.jpg"  style="border:0" /></asp:LinkButton>&nbsp;
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
    
    <script language="javascript" type="text/javascript">
        if(document.getElementById("hdnAgentP").value != "")
        {
            ValidatorEnable(document.getElementById('<%=reqtxtPassword.ClientID%>'), false);
        }
    </script>
</body>
</html>
