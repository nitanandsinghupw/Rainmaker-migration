<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CloneCampaign.aspx.cs"
    Inherits="Rainmaker.Web.campaign.CloneCampaign" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rainmaker Dialer - Clone Campaign</title>
    <style>
        #loading
        {
            z-index: 1000000000;
            background-image: url(../images/greyout.png);
            visibility: hidden;
            background-repeat: repeat-x;
            position: absolute;
            filter: alpha(opacity=60);
            opacity: 0.6;
        }
    </style>

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
        function EnableDisable(chk) {
            var isChecked = chk.checked;
            document.getElementById("chkData").checked = isChecked;
            document.getElementById("chkFields").checked = isChecked;
            document.getElementById("chkQueries").checked = isChecked;
            document.getElementById("chkOptions").checked = isChecked;
            document.getElementById("chkResultcodes").checked = isChecked;
            document.getElementById("chkScripts").checked = isChecked;
        }

        function CheckAll(chk) {
            if (chk.checked == false && document.getElementById("chkAll").checked == true) {
                document.getElementById("chkAll").checked = false;
            }
        }

        function ValidateDialingDigits() {
            if (!document.getElementById('chkTenDigitNums').checked) {
                if (!document.getElementById('chkSevenDigitNums').checked) {
                    // Don't let save without some dialing (7 or 10) allowed
                    alert("You must have 7 or 10 digit numbers allowed, or your campaign will have nothing to dial.");
                    return false;
                }
            }
            return true;
        }

        function ValidateData() {
            if (Page_ClientValidate()) {
                if (!document.getElementById('chkTenDigitNums').checked) {
                    if (!document.getElementById('chkSevenDigitNums').checked) {
                        // Don't let save without some dialing (7 or 10) allowed
                        alert("You must have 7 or 10 digit numbers allowed, or your campaign will have nothing to dial.");
                        return false;
                    }
                }

                var valid = true;
                if (document.getElementById("chkScripts").checked == true
                        && document.getElementById("chkFields").checked == false) {
                    if (confirm('Selection of scripts without selecting fields may result in errors.\nDo you want to continue?')) {
                        valid = true;
                    }
                    else return false
                }

                if (document.getElementById("chkOptions").checked == true
                        && document.getElementById("chkScripts").checked == false) {
                    if (confirm('Selection of options(dialing parameters) without selecting scripts may result in errors.\nDo you want to continue?')) {
                        valid = true;
                    }
                    else return false
                }

                if (document.getElementById("chkDuplicatePh").checked == false &&
                    document.getElementById("hdnDup").value == "true" &&
                    document.getElementById("chkData").checked == true) {
                    alert("The parent campaign may contain duplicate data, please allow duplicates or uncheck cloning of data");
                    return false;
                }

                if (valid)
                    displayLoading();
            }
            else {
                return false;
            }
        }
    </script>

</head>
<body onload="ShowPageMessage();hideLoading();">
    <div id="loading">
    </div>
    <form id="form1" runat="server" defaultbutton="lbtnOk" defaultfocus="txtDescription">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>

    <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>

    <asp:HiddenField runat="server" ID="hdnCampaignId" />
    <div>
        <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                        <tr>
                            <td>
                                <!-- Header -->
                                <RainMaker:Header ID="campaignHeader" runat="server">
                                </RainMaker:Header>
                                <!-- Header -->
                                <!-- Body -->
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td valign="middle" align="center">
                                            <table cellpadding="0" cellspacing="1" border="0" width="85%">
                                                <tr>
                                                    <td>
                                                        <table cellpadding="2" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                            <tr>
                                                                <td colspan="3">
                                                                    <img src="../images/spacer.gif" height="20px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" align="left">
                                                                    <b>
                                                                        <asp:Label ID="lblParentCampaign" runat="server"></asp:Label></b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" align="left">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" align="left">
                                                                    <b>Campaign Description</b>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3" align="left">
                                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="txtTooLarge" MaxLength="255"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                                                        ID="reqDescription" runat="server" ControlToValidate="txtDescription" ErrorMessage="Campaign Description is required."
                                                                        SetFocusOnError="True" Display="static">*</asp:RequiredFieldValidator>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="width: 27%;" valign="top">
                                                                    <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>Short Description</b>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtShortDesc" runat="server" CssClass="txtmedium" MaxLength="8"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="reqShortDesc" runat="server" ControlToValidate="txtShortDesc"
                                                                                    ErrorMessage="Short Description is required." Display="static" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td height="5px;">
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>Outbound Caller ID</b>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <asp:TextBox ID="txtOutboundCallerID" runat="server" CssClass="txtmedium" MaxLength="20"></asp:TextBox>
                                                                                <asp:RequiredFieldValidator ID="reqOutBoundCID" runat="server" ControlToValidate="txtOutboundCallerID"
                                                                                    ErrorMessage="Outbound Caller ID is required." Display="static" SetFocusOnError="True">*</asp:RequiredFieldValidator>
                                                                                <asp:ValidationSummary ID="valsumCreateCampaign" runat="server" ShowMessageBox="true"
                                                                                    ShowSummary="false" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:CheckBox ID="chkDuplicatePh" runat="server" Text="Allow Duplicate Phone #'s" /></b>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:CheckBox ID="chkTenDigitNums" runat="server" Text=" Allow 10 Digit Numbers" /></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left">
                                                                                <b>
                                                                                    <asp:CheckBox ID="chkSevenDigitNums" runat="server" Text=" Allow 7 Digit Numbers" /></b>
                                                                            </td>
                                                                            <td align="left">
                                                                                &nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr style="display: none">
                                                                            <td align="left" nowrap>
                                                                                <asp:RadioButton ID="rdoIgnore" runat="server" Text="Ignore Duplicate" GroupName="DuplicateRule" />&nbsp;<asp:RadioButton
                                                                                    ID="rdoUpdate" runat="server" Text="Replace Duplicate" GroupName="DuplicateRule" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                                <td style="width: 33%;" valign="top">
                                                                    &nbsp;
                                                                    <%--
                                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td valign="top" align="left">
                                                                                    <b>Optional Campaign Features</b>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkFundRaiser" runat="server" Text="Fund Raiser Data Tracking" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkRecordLevel" runat="server" Text="Record Level Call History" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkOnSiteCallTransfer" runat="server" Text="On-site Call Transfer w/Voice&Data" />
                                                                                    <asp:HiddenField runat="server" ID="hdnCampaignId" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkTraining" runat="server" Text="Enable Agent Training" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        --%>
                                                                </td>
                                                                <td valign="top" style="width: 40%;" align="left">
                                                                    <asp:Panel ID="pnlSaveAs" runat="server" BorderStyle="Solid" BorderColor="#325072"
                                                                        BorderWidth="1">
                                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td colspan="2" align="center" runat="server">
                                                                                    <b>Duplication Mode</b>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <asp:Label ID="lblCampaign" runat="server" Visible="false"></asp:Label>
                                                                                <td align="left">
                                                                                    <asp:RadioButton ID="RdoCopy" runat="server" Text=" Copy Selected Data" GroupName="CopyMode"
                                                                                        
                                                                                        ToolTip="This option only includes current data without any call history or current call stats..  It is the old campaign without any call information or datetime, callresults, durations etc" 
                                                                                        Font-Size="Small" Checked="true"
                                                                                        OnCheckedChanged="RdoDuplicateMode_Changed" AutoPostBack="true" />
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:RadioButton ID="RdoClone" runat="server" Text=" Clone All Data" GroupName="CopyMode"
                                                                                        
                                                                                        ToolTip="Clone all call history and current call results..exact copy of database and it's history." Font-Size="Small"
                                                                                        Checked="false" OnCheckedChanged="RdoDuplicateMode_Changed" 
                                                                                        AutoPostBack="true" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkData" runat="server" onclick="CheckAll(this)" Text="Include Data" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkFields" runat="server" onclick="CheckAll(this)" Text="Include Fields" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkQueries" runat="server" onclick="CheckAll(this)" Text="Include Queries" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkOptions" runat="server" onclick="CheckAll(this)" Text="Include Options" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkResultcodes" runat="server" onclick="CheckAll(this)" Text="Include Result Codes" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkScripts" runat="server" onclick="CheckAll(this)" Text="Include Scripts" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkAll" runat="server" Text="Include ALL" onclick="EnableDisable(this)" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td colspan="3">
                                                                    <img src="../images/spacer.gif" height="10px" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="right" colspan="3">
                                                                    <asp:LinkButton ID="lbtnOk" OnClick="lbtnsave_Click" runat="server" OnClientClick="return ValidateData();"><img src="../images/ok.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                        ID="lbtnCancel" runat="server" CausesValidation="false" OnClick="lbtnCancel_Click"
                                                                        OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"><img src="../images/cancel.jpg" border="0"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                            ID="lbtnClose" PostBackUrl="~/campaign/CampaignList.aspx" CausesValidation="false"
                                                                            runat="server"><img src="../images/close.jpg" border="0" /></asp:LinkButton>&nbsp;<asp:HiddenField
                                                                                runat="server" ID="hdnDup" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <!-- Body -->
                                <iframe id="iframeProgress" src="progressbar.html" width="0" height="0" frameborder="no">
                                </iframe>
                                <!-- Footer -->
                                <RainMaker:Footer ID="CampaignFooter" runat="server">
                                </RainMaker:Footer>
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
