<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CreateCampaign.aspx.cs"
    Inherits="Rainmaker.Web.campaign.CreateCampaign" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Create Campaign</title>
    <style>
			#loading {Z-INDEX: 1000000000; 
				BACKGROUND-IMAGE: url(../images/greyout.png); 
				VISIBILITY: hidden; 
				BACKGROUND-REPEAT: repeat-x; 
				POSITION: absolute;
				filter: alpha(opacity=60);	
				opacity: 0.6;			
			}
    </style>
    
    <script language="javascript" type="text/javascript">
        
        var cbrowser = navigator.userAgent;        
        if(cbrowser.toLowerCase().indexOf("firefox") > 0){
            document.write('<link rel="stylesheet" href="../css/RainmakerNS.css" type="text/css" />');
        }else{
            document.write('<link rel="stylesheet" href="../css/Rainmaker.css" type="text/css" />');
        }
  
//    function ValidateDialingDigits()
//    {
//        if(!document.getElementById('chkTenDigitNums').checked)
//        {
//            if (!document.getElementById('chkSevenDigitNums').checked)
//            { 
//                // Don't let save without some dialing (7 or 10) allowed
//                alert("You must have 7 or 10 digit numbers allowed, or your campaign will have nothing to dial.");
//                return false;
//            }
//        }
//        return true;
//    }
//    
//    function CheckDialingDigits()
//    {
//        //alert("Checking ten:" + document.getElementById('chkTenDigitNums').checked + ", seven:" + document.getElementById('chkSevenDigitNums').checked)
//        if(document.getElementById('chkTenDigitNums').checked)
//        {
//            if (!document.getElementById('chkSevenDigitNums').checked)
//            { 
//                // Display label that only 10 digit numbers will be dialed
//                document.getElementById('trSevenDialingMessage').style.display = "none";
//                document.getElementById('trTenDialingMessage').style.display = "";
//                return;
//            }
//        }
//        if(document.getElementById('chkSevenDigitNums').checked)
//        {
//            if(!document.getElementById('chkTenDigitNums').checked)
//            {
//                document.getElementById('trSevenDialingMessage').style.display = "";
//                document.getElementById('trTenDialingMessage').style.display = "none";
//                return;
//            }
//        }
//        document.getElementById('trSevenDialingMessage').style.display = "none";
//        document.getElementById('trTenDialingMessage').style.display = "none";
//        return;
//    }
//    
        
    </script> 
    <script language="javascript" type="text/javascript" src="../js/RainMaker.js"></script>    
</head>
<%-- <body onload="ShowPageMessage();hideLoading();CheckDialingDigits();"> --%>
<body onload="ShowPageMessage();hideLoading();">
    <div id="loading">
    </div>
    <form id="form1" runat="server" defaultbutton="lbtnOk" defaultfocus="txtDescription">
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
                                                <table cellpadding="0" cellspacing="1" border="0" width="65%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="2" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <img src="../images/spacer.gif" height="20px" /></td>
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
                                                                    <td style="width: 50%;" valign="top">
                                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>Short Description</b>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblShortDesc" runat="server" Visible="false"></asp:Label>
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
                                                                                        <asp:CheckBox ID="chkDuplicatePh" runat="server" Text=" Allow Duplicate Phone #'s" /></b>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <%-- <b>
                                                                                    <asp:CheckBox ID="chkTenDigitNums"
                                                                                        runat="server" Text=" Allow 10 Digit Numbers" OnCheckedChanged="DigitCheck_Click" /></b> --%>
                                                                                    <asp:RadioButtonList ID="rlPhoneDigitsAllowed" runat="server">
                                                                                        <asp:ListItem Text="Allow 10 Digit Numbers" Value="10" />
                                                                                        <asp:ListItem Text="Allow 7 Digit Numbers" Value="7" />
                                                                                    </asp:RadioButtonList>
                                                                                </td>
                                                                                <td align="left">
                                                                                    &nbsp;</td>
                                                                        
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                <%-- 
                                                                                <b>
                                                                                   <asp:CheckBox ID="chkSevenDigitNums"
                                                                                        runat="server" Text=" Allow 7 Digit Numbers" OnCheckedChanged="DigitCheck_Click" /></b>
                                                                                        --%>
                                                                                        &nbsp;
                                                                                </td>
                                                                                <td align="left">
                                                                                    &nbsp;</td>
                                                                            </tr>
                                                                            <tr style="display:none">
                                                                                <td align="left">
                                                                                    <asp:RadioButton ID="rdoIgnore" runat="server" Text="Ignore Duplicate" GroupName="DuplicateRule"/>&nbsp;&nbsp;<asp:RadioButton ID="rdoUpdate" runat="server" Text="Replace Duplicate" GroupName="DuplicateRule"/>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td style="width: 50%;" valign="top">
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
                                                                            <%--<tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkRecordLevel" runat="server" Text="Record Level Call History" />
                                                                                </td>
                                                                            </tr>--%>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkOnSiteCallTransfer" runat="server" Text="On-site Call Transfer w/Voice&Data" />
                                                                                    <asp:HiddenField runat="server" ID="hdnCampaignId" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                    <td align="left">
                                                                                        <asp:CheckBox ID="chkUnmannedMode" runat="server" Text="Unmanned Mode"/>
                                                                                    </td>
                                                                                </tr>
                                                                            <%--<tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkTraining" runat="server" Text="Enable Agent Training" />
                                                                                </td>
                                                                            </tr>--%>
                                                                        </table>
                                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                                
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:label id="Label1" style="color:White" runat="server" Text="Note: Only seven digit numbers will be dialed."/>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:label id="trTenDialingMessage" style="color:Blue" runat="server" Text="Note: Only ten digit numbers will be dialed."/>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:Label id="trSevenDialingMessage" style="color:Blue" runat="server" Text="Note: Only seven digit numbers will be dialed."/>
                                                                                    </td>
                                                                                </tr>
                                                                        </table>
                                                                    </td>                                                                   
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="3">
                                                                        <img src="../images/spacer.gif" height="10px" /></td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" colspan="3">
                                                                        <%-- <asp:LinkButton ID="lbtnOk" OnClick="lbtnsave_Click" runat="server" OnClientClick="if(Page_ClientValidate() && ValidateDialingDigits()) displayLoading();else return false;"> --%>
                                                                        <asp:LinkButton ID="lbtnOk" OnClick="lbtnsave_Click" runat="server" OnClientClick="if(Page_ClientValidate()) displayLoading();else return false;">
                                                                        <img src="../images/ok.jpg" border="0" alt="" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                            ID="lbtnCancel" runat="server" CausesValidation="false" OnClick="lbtnCancel_Click"
                                                                            OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"><img src="../images/cancel.jpg" border="0"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                                ID="lbtnClose" PostBackUrl="~/campaign/CampaignList.aspx" CausesValidation="false"
                                                                                runat="server"><img src="../images/close.jpg" border="0" alt="" /></asp:LinkButton>&nbsp;
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
                                    <iframe id="iframeProgress" src="progressbar.html" width=0 height=0 frameborder=no></iframe>
                                    <!-- Footer -->
                                    <RainMaker:Footer ID="CampaignFooter" runat="server"></RainMaker:Footer>
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
