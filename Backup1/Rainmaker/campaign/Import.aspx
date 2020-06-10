<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Import.aspx.cs" Inherits="Rainmaker.Web.campaign.Import" %>

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
                                                            <asp:Label ID="lblCampaign" runat="server" Text="(CompaignName)"></asp:Label>
                                                        </a>&nbsp;&nbsp;<img src="../images/arrowright.gif">&nbsp;&nbsp;<b>Import</b>
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
                                                                        <tr>
                                                                            <td align="left">
                                                                                <!-- Header -->
                                                                                <table cellpadding="1" cellspacing="0" border="0" class="tdHeader">
                                                                                    <!-- Where I Added the table -->
                                                                                    <tr>
                                                                                        <td>
                                                                                            <table cellpadding="0" cellspacing="4" border="0" class="tdWhite" style="width: 550px;">
                                                                                                <!-- Next Level -->
                                                                                                <tr>
                                                                                                    <td align="left">
                                                                                                        <label>
                                                                                                            <b>Import Type</b></label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="left">
                                                                                                        <!-- Jeff Commented these out -->
                                                                                                        <asp:CheckBox ID="chkImpAsNever" runat="server" Text=" Import As Never Calls" Style="display: none" />
                                                                                                        <asp:ValidationSummary ID="ValidationSummary1" runat="server" EnableViewState="False"
                                                                                                            ShowMessageBox="True" ShowSummary="false" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="left" nowrap>
                                                                                                        <asp:RadioButton ID="importRule1" runat="server" Text="Insert New Data, Save Duplicates To File"
                                                                                                            GroupName="DuplicateRule" />
                                                                                                    </td>
                                                                                                    <td align="left">
                                                                                                        <asp:CheckBox ID="chkSevenDigitNums" runat="server" Text=" Allow 7 Digit Numbers"
                                                                                                            Style="display: none" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="left" nowrap>
                                                                                                        <asp:RadioButton ID="importRule2" runat="server" Text="Reset and Replace Data" GroupName="DuplicateRule" />
                                                                                                    </td>
                                                                                                    <td align="left">
                                                                                                        <asp:CheckBox ID="chkTenDigitNums" runat="server" Text=" Allow 10 Digit Numbers"
                                                                                                            Style="display: none" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="left" nowrap>
                                                                                                        <asp:RadioButton ID="importRule3" runat="server" Text="Append and Replace Data" GroupName="DuplicateRule" />
                                                                                                    </td>
                                                                                                    <td align="left">
                                                                                                        &nbsp;
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
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
                                                                                                            <b>Never Call Options</b></label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td valign="middle" colspan="2" align="left">
                                                                                                        <asp:RadioButton ID="exception1" runat="server" Text="Give priority to <i>Never Call Flag</i> in file."
                                                                                                            GroupName="ExceptionGroup" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td valign="middle" colspan="2" align="left">
                                                                                                        <asp:RadioButton ID="exception2" runat="server" Text="Overide <i>Never Call Flag</i>, set:"
                                                                                                            GroupName="ExceptionGroup" />
                                                                                                        <asp:RadioButton ID="setNeverCall" runat="server" Text="On" GroupName="Exception2Group" />
                                                                                                        <asp:RadioButton ID="unsetNeverCall" runat="server" Text="Off" GroupName="Exception2Group" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td valign="middle" colspan="2" align="left">
                                                                                                        <asp:RadioButton ID="exception3" runat="server" Text="Import as <i>Never Call.</i>"
                                                                                                            GroupName="ExceptionGroup" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                                
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <%--                                                                                            <td>
                                                                                            
                                                                                            </td>--%>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr id="trIgnore" runat="server">
                                                            </tr>
                                                            <tr id="trUpdate" runat="server">
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr valign="bottom">
                                                    <td valign="bottom" align="right" colspan="2">
                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td valign="middle" align="right">
                                                                    <asp:LinkButton ID="lbtnNext" runat="server" OnClick="lbtnNext_Click" CssClass="button blue small">Next</asp:LinkButton>&nbsp;&nbsp;
                                                                    <asp:LinkButton ID="lbtnClose" runat="server" CausesValidation="false" PostBackUrl="~/campaign/home.aspx" CssClass="button blue small">Close</asp:LinkButton>&nbsp;
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
