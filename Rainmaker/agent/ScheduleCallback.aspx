<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ScheduleCallback.aspx.cs"
    Inherits="Rainmaker.Web.agent.ScheduleCallback" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self"/>
    <title>Schedule Calender</title>
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

    <script language="javascript" type="text/javascript">
    
    function Close()
    {
        if (window.opener) {
            window.opener.returnValue = "IsScheduled";
        }
        window.returnValue = "IsScheduled";
        self.close();
    }
    
    function ValidateDate()
    {
        var currentDate = new Date();
        var currentDate =  currentDate.getMonth()+1 + "/" +currentDate.getDate() + "/" + currentDate.getYear();
      if(Lessthantodaydate(currentDate,document.getElementById("txtDate").value,'Schedule '))
      {  
        return false;
      }
      else
      {
        return true;
      }
    }
    
    </script>

</head>
<body onload="ShowPageMessage();">
    <form id="form1" runat="server" defaultbutton="lbtnsave" defaultfocus="txtDate">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
        <asp:HiddenField ID="hdnPageFrom" runat="server" />
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <div>
            <table cellpadding="0" cellspacing="1" border="0" width="450px" class="tdHeader">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                            <tr>
                                <td style="height: 363px">
                                    <!-- Header -->
                                    <%-- <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <img src="../images/LeadSweepBannerLarge.jpg">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdHeader" height="1px">
                                                <img src="../images/spacer.gif" height="1px"></td>
                                        </tr>
                                    </table>--%>
                                    <!-- Header -->
                                    <!-- Body -->
                                    <table cellpadding="0" cellspacing="0" height="350px" border="0" width="100%">
                                        <tr>
                                            <td width="100%" align="center">
                                                <table cellpadding="4" cellspacing="1" border="0" width="100%">
                                                    <tr>
                                                        <td width="100%" align="center" height="200px">
                                                            <!-- Content Begin -->
                                                            <table cellpadding="4" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                <tr>
                                                                    <td align="right" width="35%">
                                                                        <b>Phone Number&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td valign="top" align="left" colspan="2" width="42%">
                                                                        <asp:TextBox ID="txtPhoneNo" runat="server" CssClass="txtmedium"></asp:TextBox>&nbsp;
                                                                        <asp:RequiredFieldValidator ID="reqPhoneNo" runat="server" Text="*" SetFocusOnError="true"
                                                                            ControlToValidate="txtPhoneNo" ErrorMessage="Enter Phone Number" Display="Static"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" width="35%">
                                                                        <b>First Name</b>
                                                                    </td>
                                                                    <td valign="top" align="left" colspan="2" width="42%">
                                                                        <asp:TextBox ID="txtFirstName" runat="server" CssClass="txtmedium"></asp:TextBox>&nbsp;
                                                                        <asp:RequiredFieldValidator ID="reqFirstName" runat="server" SetFocusOnError="true" Text="*"
                                                                            ControlToValidate="txtFirstName" ErrorMessage="Enter First Name" Display="Static"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" width="35%">
                                                                        <b>Last Name</b>
                                                                    </td>
                                                                    <td valign="top" align="left" colspan="2" width="42%">
                                                                        <asp:TextBox ID="txtLastName" runat="server" CssClass="txtmedium"></asp:TextBox>&nbsp;
                                                                        <asp:RequiredFieldValidator ID="reqLastName" runat="server" SetFocusOnError="true" Text="*"
                                                                            ControlToValidate="txtLastName" ErrorMessage="Enter Last Name" Display="Static"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" width="35%">
                                                                        <b>Schedule Date&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td valign="top" align="left" width="42%" colspan="2">
                                                                        <asp:TextBox ID="txtDate" runat="server" CssClass="txtmedium"></asp:TextBox>&nbsp;
                                                                        <asp:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDate">
                                                                        </asp:CalendarExtender>
                                                                        &nbsp;<asp:RequiredFieldValidator ID="reqDate" runat="server" SetFocusOnError="true"
                                                                            Text="*" ControlToValidate="txtDate" ErrorMessage="Enter Schedule Date" Display="Static"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                    <%-- <td align="left" >
                                                                       
                                                                    </td>--%>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>Time&nbsp;:&nbsp;&nbsp;</b></td>
                                                                    <td align="left" valign="top">
                                                                        <asp:DropDownList ID="ddlDailingHrs" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlDailingMinutes" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlDailing" runat="server" CssClass="select1">
                                                                            <asp:ListItem Text="AM"></asp:ListItem>
                                                                            <asp:ListItem Text="PM"></asp:ListItem>
                                                                        </asp:DropDownList>&nbsp;&nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" width="35%">
                                                                        <b>Schedule Notes&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td valign="top" align="left" width="42%" colspan="2">
                                                                        <asp:TextBox ID="txtNotes" runat="server" CssClass="txtlarge" TextMode="MultiLine" Height="40px"></asp:TextBox>
                                                                    </td>
                                                                    <%-- <td align="left" >
                                                                       
                                                                    </td>--%>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" align="right">
                                                            <table cellpadding="4" cellspacing="1" border="0" width="100%">
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:LinkButton ID="lbtnsave" OnClick="lbtnSave_Click" OnClientClick="return ValidateDate()"
                                                                            runat="server"><img src="../images/save.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                                ID="lbtnClose" OnClientClick='javascript:self.close();' runat="server" CausesValidation="false"><img src="../images/Close.jpg" border="0" /></asp:LinkButton>
                                                                        <asp:ValidationSummary ID="valsumOtherParams" runat="server" ShowMessageBox="true"
                                                                            ShowSummary="false" />
                                                                        <asp:HiddenField ID="hdnClose" runat="server" />
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
                                    <%-- <!-- Footer -->
                                    <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                                    <!-- Footer -->--%>
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
