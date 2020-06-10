<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Recordings.aspx.cs" Inherits="Rainmaker.Web.campaign.Recordings" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Digitized Recording</title>

    <script language="javascript" type="text/javascript">
    <!--
        var cbrowser = navigator.userAgent;
        if (cbrowser.toLowerCase().indexOf("firefox") > 0) {
            document.write('<link rel="stylesheet" href="../css/RainmakerNS.css" type="text/css" />');
        } else {
            document.write('<link rel="stylesheet" href="../css/Rainmaker.css" type="text/css" />');
        }
    //-->    
    </script>

    <script language="javascript" type="text/javascript" src="../js/Rainmaker.js"></script>

    <script language="javascript" type="text/javascript">
        function disableCheckBoxes() {
            if (!document.getElementById('chkEnableDigitizedRecording').checked) {

                //document.getElementById('chkStartRecordingWithBeep').disabled = true;
                //document.getElementById('chkWaveFormat').disabled = true;
                //document.getElementById('reqFileStoragePath').disabled = true;
                //document.getElementById('chkHigherQuality').disabled = true;
            }
            else {
                //document.getElementById('chkStartRecordingWithBeep').disabled = false;
                //document.getElementById('chkWaveFormat').disabled = false;
                //document.getElementById('reqFileStoragePath').disabled = false;
                //document.getElementById('chkHigherQuality').disabled = false;
            }
        }
    </script>

</head>
<body onload="ShowPageMessage();disableCheckBoxes();">
    <form id="fmRecordings" runat="server">
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
                                <table cellpadding="0" cellspacing="0" height="200px" border="0" width="100%">
   
                                    <tr>
                                        <td valign="top">
                                            <table cellpadding="2" cellspacing="0" border="0">
                                                <tr>
                                                    <td align="left" width="100%" valign="top">
                                                        <table cellpadding="2" cellspacing="0" width="100%" border="0">
                                                            <tr>
                                                                <td valign="middle" align="left">
                                                                    <a href="Home.aspx" class="aHome">
                                                                        <asp:Label ID="lblCampaign" runat="server" Text=""></asp:Label>
                                                                    </a>&nbsp;&nbsp;<img src="../images/arrowright.gif">&nbsp;&nbsp;<b>Recordings</b>
                                                                </td>
                                                            </tr>
                                                          
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr valign="top">
                                                    <td align="left" width="100%" valign="top">
                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td colspan="2" valign="top" align="left">
                                                                    <b>
                                                                        <asp:CheckBox ID="chkEnableDigitizedRecording" runat="server" Text="Enable Digitized Recording"
                                                                            Checked="true" onclick="disableCheckBoxes();" /></b>
                                                                </td>
                                                            </tr>
                                                           
                                                            <tr valign="top">
                                                                <td colspan="2">
                                                                    <img src="../images/spacer.gif" border="0" height="3px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <table border="0" cellpadding="2" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                                    <tr>
                                                                                        <td class="BlackOnWhite_TVA_14_NBL">
                                                                                            Digitized Recording File's Storage Path :&nbsp;
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtFileStoragePath" runat="server" Text="" CssClass="txtTooLarge"
                                                                                                Width="401px" Height="24px"></asp:TextBox>
                                                                                             <%--<asp:RequiredFieldValidator ID="reqFileStoragePath" runat="server" ControlToValidate="txtFileStoragePath"
                                                                                            ErrorMessage="Please enter file storage path" Display="dynamic" SetFocusOnError="true">*</asp:RequiredFieldValidator><asp:RegularExpressionValidator
                                                                                            ID="regFilePath" runat="server" ControlToValidate="txtFileStoragePath" ErrorMessage="Please enter valid file path"
                                                                                        <asp:ValidationSummary ID="valsumRecording" runat="server" ShowMessageBox="true"  ShowSummary="false" />--%>
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
                                        </td>
                                    </tr>
                                    <tr valign="bottom">
                                        <td valign="bottom" align="right" colspan="2">
                                            <table cellpadding="4" cellspacing="1" border="0" width="100%">
                                                <tr>
                                                    <td valign="middle" align="right">
                                                        <asp:LinkButton ID="lbtnSave" runat="server" OnClick="lbtnSave_Click" CssClass="button blue small">Save</asp:LinkButton>&nbsp;&nbsp;
                                                        <asp:LinkButton ID="lbtnClose" runat="server" CausesValidation="false" PostBackUrl="~/campaign/home.aspx" CssClass="button blue small">Close</asp:LinkButton>
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
