<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportDNC.aspx.cs" Inherits="Rainmaker.Web.campaign.ImportDNC" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rainmaker Dialer - Import</title>

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
    <form id="frmImport" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

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
                                        <td align="left" width="100%">
                                            <table cellpadding="4" cellspacing="4" width="100%" border="0">
                                                <tr>
                                                    <td valign="bottom" align="left">
                                                        <a href="Home.aspx" class="aHome" runat="server" id="anchHome">
                                                            
                                                        </a>&nbsp;&nbsp;<img src="../images/arrowright.gif">&nbsp;&nbsp;<b>Master Do Not Call List Import</b>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="center">
                                            <table cellpadding="0" cellspacing="1" border="0" width="60%">
                                                <tr>
                                                    <td>
                                                        <table cellpadding="2" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                            <tr>
                                                                <td>
                                                                    <img src="../images/spacer.gif" height="10px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="top">
                                                                    <table cellpadding="2" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <label>
                                                                                    <b>Select File</b></label>&nbsp;<asp:Label ID="lblFilePath" runat="server" Text=""
                                                                                        CssClass="tdFooter"></asp:Label>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:FileUpload ID="fileUpload" runat="server" Width="98%" />
                                                                                <asp:RequiredFieldValidator runat="server" ID="rqFileUpload" ControlToValidate="fileUpload"
                                                                                    ErrorMessage="Please Select A File" Display="Static">*</asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                    <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                                                        <!-- Start -->
                                                                        
                                                                        <!-- Jeff End of options -->
                                                                        <tr>
                                                                            <td align="left">
                                                                                <!-- Header -->
                                                                                <table cellpadding="1" cellspacing="0" border="0" class="tdHeader">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table cellpadding="0" cellspacing="4" border="0" class="tdWhite" style="width: 550px;">
                                                                                                <tr>
                                                                                                    <td align="left">
                                                                                                        <label>
                                                                                                            <b>Import Options</b></label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="left">
                                                                                                        <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                                                                            <tr>
                                                                                                                <td valign="middle" width="5%" align="left" nowrap>
                                                                                                                    <img src="../images/spacer.gif" width="20px" />Delimiter&nbsp;:&nbsp;&nbsp;
                                                                                                                    <asp:DropDownList ID="ddlDelimiter" runat="server" CssClass="select1">
                                                                                                                        <asp:ListItem Text="Comma" Value=","></asp:ListItem>
                                                                                                                        <asp:ListItem Text="Tab" Value="t"></asp:ListItem>
                                                                                                                        <asp:ListItem Text="Semicolon" Value=";"></asp:ListItem>
                                                                                                                        <asp:ListItem Text="Space" Value=" "></asp:ListItem>
                                                                                                                    </asp:DropDownList>
                                                                                                                </td>
                                                                                                                <td valign="middle" align="left">
                                                                                                                    <img src="../images/spacer.gif" width="17px" /><asp:CheckBox ID="chkFirstLine" Checked="true"
                                                                                                                        runat="server" Text="First line contains field names" />
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
                                                                        <!-- Before final insert -->
                                                                        
                                                                    </table>
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
                                                                    <asp:LinkButton ID="lbtnNext" runat="server" OnClick="lbtnNext_Click"><img src="" class="myButton" alt="Next" border="0" /></asp:LinkButton>&nbsp;&nbsp;
                                                                    <asp:LinkButton ID="lbtnClose" runat="server" CausesValidation="false" PostBackUrl="~/campaign/home.aspx"><img src="" class="myButton" alt="Close" border="0" /></asp:LinkButton>&nbsp;
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                    <!-- Footer -->
                </td>
            </tr>
        </table>
        </td> </tr> </table>
    </div>
    </form>

    <script language="javascript" type="text/javascript">
        if(document.getElementById("lblFilePath").innerHTML != "")
        {
            ValidatorEnable(document.getElementById('<%=rqFileUpload.ClientID%>'), false);
        }
    </script>

</body>
</html>
