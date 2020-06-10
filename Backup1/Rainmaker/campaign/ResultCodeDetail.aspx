<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ResultCodeDetail.aspx.cs"
    Inherits="Rainmaker.Web.campaign.ResultCodeDetail" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Result Code Detail</title>
    <script language="javascript" type="text/javascript">
    <!--
        var cbrowser = navigator.userAgent;        
        if(cbrowser.toLowerCase().indexOf("firefox") > 0){
            document.write('<link rel="stylesheet" href="../css/RainmakerNS.css" type="text/css" />');
        }else{
            document.write('<link rel="stylesheet" href="../css/Rainmaker.css" type="text/css" />');
        }
        
        function CheckNeverAndRedialable(chkBox)
        {
            var chkMasterDNC = document.getElementById('chkMasterDNC');
            var chkRedialable = document.getElementById('chkRedialable');
            var chkNeverCall = document.getElementById('chkNeverCall');
            var _chkNeverCall = document.getElementById('_chkNeverCall');

            switch (chkBox.id)
            {
                case "chkRedialable":
                    if (chkRedialable.checked)
                        chkNeverCall.disabled = chkMasterDNC.checked = chkNeverCall.checked = _chkNeverCall.value = false;
                    break;
                case "chkMasterDNC":
                    chkRedialable.checked = chkMasterDNC.checked ? false : chkRedialable.checked;
                    chkNeverCall.disabled = chkNeverCall.checked = _chkNeverCall.value = chkMasterDNC.checked;
                    break;
                case "chkNeverCall":
                    chkRedialable.checked = chkNeverCall.checked ? false : chkRedialable.checked;
                    break;
                default:
                    break;
            }
        }

        function AddToRunningCampaign()
        {
            if (document.getElementById('chkCampRunning').checked == true)
            {
                if(confirm('Addition or changing of result codes while a campaign is running may cause inconsistency.\nDo you want to continue?'))
                        {
                            return true;
                        }
                        else return false
            }
            else
            {
                return true;
            }
        }
    //-->    
    </script>   
    <script language="javascript" type="text/javascript" src="../js/RainMaker.js"></script>
</head>
<body onload="ShowPageMessage();">
    <form id="form1" runat="server" defaultfocus="txtDescription" defaultbutton="lbtnSave">
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
                                            <td valign="top">
                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td align="left" width="100%">
                                                            <table cellpadding="4" cellspacing="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="middle" width="35%" align="left">
                                                                        <a href="Home.aspx" class="aHome">
                                                                            <asp:Label ID="lblCampaign" runat="server" Text=""></asp:Label>
                                                                        </a>&nbsp;&nbsp;<img src="../images/arrowright.gif">&nbsp;&nbsp;<b>Result Code Detail</b></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="left" valign="top" width="100%">
                                                            <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="middle" align="center" width="100%">
                                                                        <table cellspacing="3" cellpadding="4" width="100%" border="0">
                                                                            <tr>
                                                                                <td align="right" width="10%" nowrap>
                                                                                    <b>Description&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                <td align="left" width="70%">
                                                                                    <asp:TextBox ID="txtDescription" runat="server" CssClass="txtmedium"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="reqDescription" runat="server" ControlToValidate="txtDescription"
                                                                                        Text="*" ErrorMessage="Enter Description" Display="static" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" nowrap>
                                                                                    <b>Recycle in Days&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                <td align="left" width="70%">
                                                                                    <asp:TextBox ID="txtRecInDays" runat="server" CssClass="txtmedium" MaxLength="4"></asp:TextBox>
                                                                                    <asp:RequiredFieldValidator ID="reqRecycleinDays" runat="server" ControlToValidate="txtRecInDays"
                                                                                        Text="*" ErrorMessage="Enter Recycle in Days" Display="static" SetFocusOnError="true"></asp:RequiredFieldValidator><asp:CompareValidator
                                                                                            ID="cmpRecInDays" runat="server" ControlToValidate="txtRecInDays" Operator="dataTypeCheck"
                                                                                            Type="integer" SetFocusOnError="true" Display="dynamic" ErrorMessage="Enter numeric values">*</asp:CompareValidator>
                                                                                    <asp:ValidationSummary ID="valsumResultCode" runat="server" ShowMessageBox="true"
                                                                                        ShowSummary="false" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" nowrap>
                                                                                    <b>Presentation&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                <td valign="middle" align="left" colspan="2" nowrap>
                                                                                    <asp:CheckBox ID="chkPresentation" runat="server" Text="" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" nowrap>
                                                                                    <b>Redialable&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                <td valign="middle" align="left" colspan="2" nowrap>
                                                                                    <asp:CheckBox ID="chkRedialable" runat="server" Text="" onclick="CheckNeverAndRedialable(this);RecycleDisable();"/></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" nowrap>
                                                                                    <b>Lead&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                <td valign="middle" align="left" colspan="2" nowrap>
                                                                                    <asp:CheckBox ID="chkLead" runat="server" Text="" onclick="RecycleDisable();"/></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" nowrap>
                                                                                    <b>Add to Master Do Not Call&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                <td valign="middle" align="left" colspan="2" nowrap>
                                                                                    <asp:CheckBox ID="chkMasterDNC" runat="server" Text="" onclick="CheckNeverAndRedialable(this);RecycleDisable();"/></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" nowrap>
                                                                                    <b>Never Call&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                <td valign="middle" align="left" colspan="2" nowrap>
                                                                                    <asp:CheckBox ID="chkNeverCall" runat="server" Text=""  onclick="CheckNeverAndRedialable(this);RecycleDisable();"/>
                                                                                    <asp:HiddenField ID="_chkNeverCall" runat="server" />
                                                                                    </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" nowrap>
                                                                                    <b>Verify Only&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                <td valign="middle" align="left" colspan="2" nowrap>
                                                                                    <asp:CheckBox ID="chkVerifyOnly" runat="server" Text="" /></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" nowrap>
                                                                                    <b>Live Contact&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                <td valign="middle" align="left" colspan="2" nowrap>
                                                                                    <asp:CheckBox ID="chkLiveContact" runat="server" Text="" /></td>
                                                                            </tr>
                                                                            </tr>
                                                                           
                                                           
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr valign="bottom">
                                                        <td align="left" width="50%">
                                                            <table cellpadding="4" cellspacing="1" border="0" width="100%" class="tableoverview">
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:LinkButton ID="lbtnSave" runat="server" OnClick="lbtnSave_Click" OnClientClick="return AddToRunningCampaign();"><img src="../images/save.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                            ID="lbtnCancel" runat="server" CausesValidation="false" OnClick="lbtnCancel_Click"
                                                                            OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"><img src="../images/cancel.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                                ID="lbtnClose" runat="server" PostBackUrl="~/campaign/ResultCodes.aspx" CausesValidation="false"><img src="../images/close.jpg" border="0" /></asp:LinkButton></td>
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
                                    <!-- Added hidden checkbox for CampaignRunning check for result codes while running GW 1.11.11 -->
                                    <asp:CheckBox ID="chkCampRunning" runat="server" Text="running" style="display:none"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
    <script language="javascript" type="text/javascript">
        ValidatorEnable(document.getElementById('<%=reqDescription.ClientID%>'),true);
        function RecycleDisable()
        {
            var enable = !( document.getElementById('chkNeverCall').checked || document.getElementById('chkMasterDNC').checked ||
                            document.getElementById('chkLead').checked );
            ValidatorEnable(document.getElementById('<%=reqRecycleinDays.ClientID%>'),enable);
            ValidatorEnable(document.getElementById('<%=cmpRecInDays.ClientID%>'),enable);
        }
        RecycleDisable();
    </script>
</body>
</html>
