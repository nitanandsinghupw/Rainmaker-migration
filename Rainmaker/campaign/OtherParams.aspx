<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OtherParams.aspx.cs" Inherits="Rainmaker.Web.campaign.OtherParams" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Other Parameters</title>

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

    <script language="javascript" type="text/javascript" src="../js/RainMaker.js"></script>

    <script language="javascript" type="text/javascript">
        function CallTransferOptions() {
            if (document.getElementById("rbtnAFCT").checked) {
                document.getElementById("chkStaticOffSite").disabled = false;
                if (document.getElementById("chkStaticOffSite").checked) {
                    document.getElementById("txtPhoneNo").disabled = false;
                }
                else {
                    document.getElementById("txtPhoneNo").value = "";
                    document.getElementById("txtPhoneNo").disabled = true;
                }
            }
            else {
                document.getElementById("chkStaticOffSite").disabled = true;
                document.getElementById("txtPhoneNo").value = "";
                document.getElementById("txtPhoneNo").disabled = true;
                document.getElementById("chkStaticOffSite").checked = false;
                //document.getElementById("txtPhoneNo").value="";
            }
            if (document.getElementById("rbtnAOCT").checked) {
                document.getElementById("chkTAPM").disabled = false;
                if (document.getElementById("chkTAPM").checked) {
                    document.getElementById("FileUploadautoplaymsg").disabled = false;
                    document.getElementById("FileUploadholdmsg").disabled = false;
                }
                else {
                    document.getElementById("FileUploadautoplaymsg").disabled = true;
                    document.getElementById("FileUploadholdmsg").disabled = true;
                    document.getElementById('hdnAutoPlayMesssage').value = "";
                    document.getElementById('hdnHoldMessage').value = "";
                    document.getElementById('lblAutoPlayMessage').innerHTML = "";
                    document.getElementById('lblHoldMessage').innerHTML = "";
                    document.getElementById('hdnPlayAutoMessagePath').value = "";
                    document.getElementById('hdnPlayHoldMessagePath').value = "";
                }
            }
            else {
                document.getElementById("chkTAPM").disabled = true;
                document.getElementById("chkTAPM").checked = false;
                document.getElementById("FileUploadautoplaymsg").disabled = true;
                document.getElementById("FileUploadholdmsg").disabled = true;
            }

            if (document.getElementById("chkAllow").checked) {
                document.getElementById("txtStartingLine").disabled = false;
                document.getElementById("txtEndingLine").disabled = false;
            }
            else {
                document.getElementById("txtStartingLine").value = "";
                document.getElementById("txtEndingLine").value = "";
                document.getElementById("txtStartingLine").disabled = true;
                document.getElementById("txtEndingLine").disabled = true;
                ValidatorEnable(document.getElementById('reqStartingLine'), false);
                ValidatorEnable(document.getElementById('reqEndingLine'), false);
            }
        }
        function Validate() {
            if (document.getElementById("chkAllow").checked) {
                ValidatorEnable(document.getElementById('reqStartingLine'), true);
                ValidatorEnable(document.getElementById('reqEndingLine'), true);
            }
            else {
                ValidatorEnable(document.getElementById('reqStartingLine'), false);
                ValidatorEnable(document.getElementById('reqEndingLine'), false);
            }
            if (document.getElementById("chkTAPM").checked) {
                if (document.getElementById('hdnAutoPlayMesssage').value == "")
                    ValidatorEnable(document.getElementById('reqAutoFileUpload'), true);
                if (document.getElementById('hdnHoldMessage').value == "")
                    ValidatorEnable(document.getElementById('reqHoldFileUpload'), true);
            }
            else {
                ValidatorEnable(document.getElementById('reqAutoFileUpload'), false);
                ValidatorEnable(document.getElementById('reqHoldFileUpload'), false);
            }
            if (document.getElementById("chkStaticOffSite").checked) {
                ValidatorEnable(document.getElementById('reqPhoneNo'), true);
            }
            else {
                ValidatorEnable(document.getElementById('reqPhoneNo'), false);
            }
        }
    
    </script>

</head>
<body onload="ShowPageMessage();">
    <form id="form1" runat="server" defaultfocus="txtStartingLine" defaultbutton="lbtnsave">
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
                                <table cellpadding="0" cellspacing="0" height="400px" border="0" width="100%">
                                    <tr>
                                        <td valign="bottom" align="right">
                                            <asp:LinkButton ID="LinkButton1" OnClick="lbtnSave_Click" OnClientClick="Validate()"
                                                runat="server" CssClass="button blue small">Save</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                    ID="LinkButton2" CausesValidation="false" OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"
                                                    OnClick="lbtnCancel_Click" runat="server" CssClass="button blue small">Cancel</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                        ID="LinkButton3" runat="server" PostBackUrl="~/campaign/Home.aspx" CausesValidation="false" CssClass="button blue small">Close</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td align="left" width="100%" colspan="2">
                                                        <table cellpadding="2" cellspacing="1" width="100%" border="0">
                                                            <tr>
                                                                <td valign="middle" width="35%" align="left">
                                                                    <a href="Home.aspx" id="anchHome" class="aHome" runat="server"></a>&nbsp;&nbsp;<img
                                                                        src="../images/arrowright.gif">&nbsp;&nbsp;<b>Other Parameters<b>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" width="50%">
                                                        <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <label>
                                                                        <b>Call Transfer Options</b></label>
                                                                    <asp:HiddenField ID="hdnPlayHoldMessagePath" runat="server" />
                                                                    <asp:HiddenField ID="hdnPlayAutoMessagePath" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="10px">
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:RadioButton ID="rbtnDACT" GroupName="rdgpCallTransfer" runat="server" Checked="true"
                                                                                    onclick="CallTransferOptions();" />
                                                                                Don't Allow Call Transfer
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:RadioButton ID="rbtnAFCT" runat="server" GroupName="rdgpCallTransfer" onclick="CallTransferOptions()" />Allow
                                                                                Offsite Call Transfer
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="3%">
                                                                            </td>
                                                                            <td>
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <asp:CheckBox ID="chkStaticOffSite" runat="Server" onclick="CallTransferOptions()"
                                                                                                Text="Static Offsite Phone Number"></asp:CheckBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td width="5%">
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="txtmedium"></asp:TextBox>
                                                                                            &nbsp<asp:RequiredFieldValidator ID="reqPhoneNo" Enabled="false" runat="server" ControlToValidate="txtPhoneNo"
                                                                                                SetFocusOnError="True" ErrorMessage="Enter Phone Number" Text="*" Display="static"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:RadioButton ID="rbtnAOCT" runat="Server" GroupName="rdgpCallTransfer" onclick="CallTransferOptions()">
                                                                                </asp:RadioButton>Allow On-Site Call Transfer w/Data
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="15px">
                                                                            </td>
                                                                            <td>
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <asp:CheckBox runat="server" ID="chkTAPM" onclick="CallTransferOptions()" />
                                                                                            Transfer to Auto Play Message
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td width="15px">
                                                                                        </td>
                                                                                        <td>
                                                                                            <table cellpadding="0" cellspacing="1" border="0" width="50%" class="tdHeader">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <table cellpadding="5" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                                                            <tr>
                                                                                                                <td align="left" class="tdSetting" valign="top">
                                                                                                                    Auto Play Message&nbsp;<asp:Label ID="lblAutoPlayMessage" runat="server"></asp:Label>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="center">
                                                                                                                    <asp:FileUpload ID="FileUploadautoplaymsg" runat="server" onchange="fileChange('FileUploadautoplaymsg','wav','Please select a wav file.');"
                                                                                                                        CssClass="fileUpload" />&nbsp;<asp:RequiredFieldValidator ID="reqAutoFileUpload"
                                                                                                                            runat="server" SetFocusOnError="True" ControlToValidate="FileUploadautoplaymsg"
                                                                                                                            ErrorMessage="Select Auto Play Message To Play" Text="*" Display="static" Enabled="false"></asp:RequiredFieldValidator>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td align="center">
                                                                                                                    <asp:LinkButton ID="lbtnAutoPlay" OnClientClick="javascript:if(!document.getElementById('chkTAPM').checked) return false; return OpenAudioFile('hdnPlayAutoMessagePath','FileUploadautoplaymsg','Select Auto Play Message To Play');"
                                                                                                                        runat="server" CssClass="button blue small">Play</asp:LinkButton>&nbsp;&nbsp;
                                                                                                                    <asp:LinkButton ID="lbtnAutoStop" OnClientClick="javascript:if(!document.getElementById('chkTAPM').checked) return false; return CloseWindow('FileUploadautoplaymsg');"
                                                                                                                        runat="server" CssClass="button blue small">Stop</asp:LinkButton>&nbsp;
                                                                                                                    <%--<asp:LinkButton ID="lbtnAutoRecord" runat="server"><img src="../images/record.jpg" border="0" /></asp:LinkButton>&nbsp;--%>
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                        <td>
                                                                                            <tr>
                                                                                                <td height="8px">
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td width="15px">
                                                                                                </td>
                                                                                                <td>
                                                                                                    <table cellpadding="0" cellspacing="1" border="0" width="50%" class="tdHeader">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <table cellpadding="5" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                                                                    <tr>
                                                                                                                        <td align="left" class="tdSetting" valign="top">
                                                                                                                            Hold Message&nbsp;<asp:Label ID="lblHoldMessage" runat="server"></asp:Label>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="center">
                                                                                                                            <asp:FileUpload ID="FileUploadholdmsg" runat="server" onchange="fileChange('FileUploadholdmsg','wav','Please select a wav file.');"
                                                                                                                                CssClass="fileUpload" />&nbsp;<asp:RequiredFieldValidator ID="reqHoldFileUpload"
                                                                                                                                    runat="server" SetFocusOnError="True" ControlToValidate="FileUploadholdmsg" ErrorMessage="Select Hold Message To Play"
                                                                                                                                    Text="*" Display="static" Enabled="false"></asp:RequiredFieldValidator>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                    <tr>
                                                                                                                        <td align="center">
                                                                                                                            <asp:LinkButton ID="lbtnHoldplay" OnClientClick="javascript:if(!document.getElementById('chkTAPM').checked) return false; return OpenAudioFile('hdnPlayHoldMessagePath','FileUploadholdmsg','Select Hold Message To Play')"
                                                                                                                                runat="server" CssClass="button blue small">Play</asp:LinkButton>&nbsp;&nbsp;
                                                                                                                            <asp:LinkButton ID="lbtnHoldstop" OnClientClick="javascript:if(!document.getElementById('chkTAPM').checked) return false; return CloseWindow('FileUploadholdmsg');"
                                                                                                                                runat="server" CssClass="button blue small">Stop</asp:LinkButton>&nbsp;
                                                                                                                            <%--<asp:LinkButton ID="lbtnHoldrecord" runat="server"><img src="../images/record.jpg" border="0" /></asp:LinkButton>&nbsp;--%>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
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
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="top" width="50%">
                                                        <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                                            <tr>
                                                                <td colspan="2">
                                                                    <label>
                                                                        <b>Manual Dial Options</b></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="8px">
                                                                </td>
                                                                <td>
                                                                    <table cellpadding="1" cellspacing="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:CheckBox ID="chkAllow" onclick="CallTransferOptions()" runat="server" Checked="true" />Allow
                                                                                Manual Dial
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td width="45%" valign="top">
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%" valign="top">
                                                                                    <tr>
                                                                                        <td>
                                                                                            &nbsp;Limit Manualy Dialed Calls to&nbsp;:&nbsp;&nbsp;<br />
                                                                                            &nbsp;(-1 allows any line to be used)
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td width="55%">
                                                                                <table cellpadding="0" cellspacing="0" border="0" width="100%" valign="top">
                                                                                    <tr>
                                                                                        <td align="right" width="30%" nowrap>
                                                                                            Starting Line&nbsp;:&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left" width="70%">
                                                                                            <asp:TextBox ID="txtStartingLine" MaxLength="4" runat="server" class="txtsmall"></asp:TextBox>
                                                                                            &nbsp;<asp:RequiredFieldValidator ID="reqStartingLine" Enabled="false" runat="server"
                                                                                                SetFocusOnError="True" ControlToValidate="txtStartingLine" ErrorMessage="Enter Starting Line Value"
                                                                                                Text="*" Display="static"></asp:RequiredFieldValidator><asp:CompareValidator ID="cmpStartingLine2"
                                                                                                    runat="server" ControlToValidate="txtStartingLine" ErrorMessage="Starting Line value should be numeric"
                                                                                                    Enabled="true" SetFocusOnError="true" Operator="DataTypeCheck" Type="Integer"
                                                                                                    Text="*" Display="static"></asp:CompareValidator><%--&nbsp;<asp:CompareValidator ID="cmpStartingLine"
                                                                                runat="server" ControlToValidate="txtStartingLine" Operator="LessThanEqual" Type="integer"
                                                                                SetFocusOnError="true" ValueToCompare="32767" ErrorMessage="Starting Line Value should be less than 32767"
                                                                                Display="static">*</asp:CompareValidator>--%>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right">
                                                                                            Ending Line&nbsp;:&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:TextBox ID="txtEndingLine" MaxLength="4" runat="server" class="txtsmall"></asp:TextBox>
                                                                                            &nbsp;<asp:RequiredFieldValidator ID="reqEndingLine" Enabled="false" runat="server"
                                                                                                SetFocusOnError="True" ControlToValidate="txtEndingLine" ErrorMessage="Enter Ending Line Value"
                                                                                                Text="*" Display="static"></asp:RequiredFieldValidator><asp:CompareValidator ID="cmpEndingLine"
                                                                                                    runat="server" ControlToValidate="txtEndingLine" ErrorMessage="Ending Line value should be numeric"
                                                                                                    Enabled="true" SetFocusOnError="true" Operator="DataTypeCheck" Type="Integer"
                                                                                                    Text="*" Display="static"></asp:CompareValidator>
                                                                                            <%--  <asp:CompareValidator ID="cmpELN" runat="server" ControlToValidate="txtEndingLine" Operator="GreaterThanEqual"
                                                                                                    Type="Integer" ValueToCompare="-1" ErrorMessage="Ending Line should not be less than -1"
                                                                                                    Text="*" Display="static"></asp:CompareValidator>--%>
                                                                                            <asp:HiddenField ID="hdnAutoPlayMesssage" runat="server" />
                                                                                            <asp:HiddenField ID="hdnHoldMessage" runat="server" />
                                                                                            <%-- <asp:CompareValidator ID="CompareValidator1"  runat="server" ControlToValidate="txtELN" Operator="GreaterThanEqual" Type="Integer" ControlToCompare="txtSL" ErrorMessage="Ending Line should not be less than Starting Line" Text="*" Display="static"  ></asp:CompareValidator>--%>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <label>
                                                                        <b>Callback Options</b></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="8px">
                                                                </td>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:RadioButtonList RepeatDirection="Vertical" ID="rbtnCallBackOptions" runat="server">
                                                                                </asp:RadioButtonList>
                                                                                <asp:RadioButton ID="rbtnDAC" runat="Server" GroupName="rbtnCallBacks" Checked="true">
                                                                                </asp:RadioButton>Don't Allow Callbacks
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:RadioButton ID="rbtnAAC" runat="Server" GroupName="rbtnCallBacks"></asp:RadioButton>Allow
                                                                                Agent Callbacks
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:RadioButton ID="rbtnASC" runat="Server" GroupName="rbtnCallBacks"></asp:RadioButton>Allow
                                                                                Station Callbacks
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:RadioButton ID="rbtnAsys" runat="Server" GroupName="rbtnCallBacks"></asp:RadioButton>Allow
                                                                                System Callbacks
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                &nbsp;Alert Supervisor after an agent sets
                                                                                <asp:TextBox ID="txtAlertSupervisor" MaxLength="4" runat="server" CssClass="txtmini"></asp:TextBox>
                                                                                Callbacks&nbsp;
                                                                                <%--<asp:RequiredFieldValidator ID="reqEL" runat="server" ControlToValidate="txtAlertSupervisor"
                                                                                        ErrorMessage="Enter Alert Supervisor after an agent sets CallBacks" Text="*"
                                                                                        Display="static"></asp:RequiredFieldValidator>--%>
                                                                                &nbsp;
                                                                                <asp:CompareValidator ID="cmpAlertSupervisor" runat="server" ControlToValidate="txtAlertSupervisor"
                                                                                    ErrorMessage="Alert Supervisor value should be numeric" Enabled="true" SetFocusOnError="true"
                                                                                    Operator="DataTypeCheck" Type="Integer" Text="*" Display="static"></asp:CompareValidator>
                                                                                <%--&nbsp;<asp:CompareValidator ID="cmpAlertSupervisor2"
                                                                                runat="server" ControlToValidate="txtAlertSupervisor" Operator="LessThanEqual" Type="integer"
                                                                                SetFocusOnError="true" ValueToCompare="32767" ErrorMessage="Alert Supervisor value should be less than 32767"
                                                                                Display="static">*</asp:CompareValidator>--%>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">
                                                                                ( <1 turns notifications off )
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="2">
                                                                    <label>
                                                                        <b>Miscellaneous</b></label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="8px">
                                                                </td>
                                                                <td>
                                                                    <table cellpadding="1" cellspacing="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td colspan="2">
                                                                                <asp:CheckBox ID="chkQSP" runat="server" Checked="true" />Show Query Statistics
                                                                                as Percentages
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
                                        <td valign="top" align="right" colspan="2">
                                            <table cellpadding="4" cellspacing="1" border="0" width="100%">
                                                <tr>
                                                    <td valign="bottom" align="right">
                                                        <asp:LinkButton ID="lbtnsave" OnClick="lbtnSave_Click" OnClientClick="Validate()"
                                                            runat="server" CssClass="button blue small">Save</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                ID="lbtnCancel" CausesValidation="false" OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"
                                                                OnClick="lbtnCancel_Click" runat="server" CssClass="button blue small">Cancel</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                    ID="lbtnClose" runat="server" PostBackUrl="~/campaign/Home.aspx" CausesValidation="false" CssClass="button blue small">Close</asp:LinkButton>
                                                        <asp:ValidationSummary ID="valsumOtherParams" runat="server" ShowMessageBox="true"
                                                            ShowSummary="false" />
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

    <script language="javascript" type="text/javascript">
        CallTransferOptions();
    </script>

</body>
</html>
