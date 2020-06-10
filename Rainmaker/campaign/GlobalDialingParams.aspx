<%@ Page Language="C#" AutoEventWireup="true" Codebehind="GlobalDialingParams.aspx.cs"
    Inherits="Rainmaker.Web.campaign.GlobalDialingParams" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader2.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rainmaker Dialer - Global Dialing Params</title>
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
    <form id="form1" runat="server" defaultbutton="lbtnSave" defaultfocus="txtGlobalDialingPrefix">
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
                                        <%--<tr>
                                            <td align="left" width="100%">
                                                <table cellpadding="4" cellspacing="4" width="100%" border="0">
                                                    <tr>
                                                        <td valign="bottom" align="left">
                                                            <a href="Home.aspx" class="aHome" runat="server" id="anchHome">
                                                                <asp:Label ID="lblCampaign" runat="server" Text="(CompaignName)"></asp:Label>
                                                            </a>&nbsp;&nbsp;<img src="../images/arrowright.gif">&nbsp;&nbsp;<b>Import</b>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td valign="middle" align="center">
                                                <table cellpadding="0" cellspacing="1" border="0" width="60%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="2" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                <tr>
                                                                    <td>
                                                                        <img src="../images/spacer.gif" height="10px" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top">
                                                                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <label>
                                                                                        <b>Global Dialing Prefix&nbsp;:</b></label>&nbsp;&nbsp;
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="txtGlobalDialingPrefix" runat="server" Text=""  MaxLength="10" CssClass="txtmedium"></asp:TextBox>
                                                                                    <%--<asp:RequiredFieldValidator ID="reqGlobalDialingPrefix" runat="server" ControlToValidate="txtGlobalDialingPrefix"
                                                                                        ErrorMessage="Please Enter Global Dialing Prefix" SetFocusOnError="True" Display="static">*</asp:RequiredFieldValidator>--%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <label>
                                                                                        <b>Global Dialing Suffix&nbsp;:</b></label>&nbsp;&nbsp;
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:TextBox ID="txtGlobalDialingSuffix" runat="server" Text="" MaxLength="10" CssClass="txtmedium"></asp:TextBox>
                                                                                    <%--<asp:RequiredFieldValidator ID="reqGlobalDialingSuffix" runat="server" ControlToValidate="txtGlobalDialingSuffix"
                                                                                        ErrorMessage="Please Enter Global Dialing Suffix" SetFocusOnError="True" Display="static">*</asp:RequiredFieldValidator>--%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableViewState="False"
                                                                                        ShowMessageBox="True" ShowSummary="false" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr valign="bottom">
                                                                    <td valign="bottom" align="right" colspan="2">
                                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                            <tr>
                                                                                <td valign="middle" align="right">
                                                                                    <asp:LinkButton ID="lbtnSave" OnClick="lbtnSave_Click" runat="server" Text="Save" CssClass="button blue small"></asp:LinkButton>&nbsp;&nbsp;
                                                                                    <asp:LinkButton ID="lbtnCancel" runat="server" CausesValidation="false" OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"
                                                                                        OnClick="lbtnCancel_Click" Text="Cancel" CssClass="button blue small"></asp:LinkButton>&nbsp;&nbsp;
                                                                                    <asp:LinkButton ID="lbtnClose" runat="server" PostBackUrl="~/campaign/CampaignList.aspx"
                                                                                        CausesValidation="false" Text="Close" CssClass="button blue small"></asp:LinkButton>&nbsp;
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
