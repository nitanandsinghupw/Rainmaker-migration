<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Agents.aspx.cs" Inherits="Rainmaker.Web.campaign.Agents" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Agent Stats</title>

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

</head>
<body onload="ShowPageMessage();">
    <form id="frmAgentStats" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <%--<asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="10000">
        </asp:Timer>--%>

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
                                            <asp:UpdatePanel ID="updHome" runat="server">
                                                <%--<Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                    </Triggers>--%>
                                                <ContentTemplate>
                                                    <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="10000">
                                                    </asp:Timer>
                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tr>
                                                            <td valign="top">
                                                                <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                                    <tr>
                                                                        <td align="left" width="100%">
                                                                            <table cellpadding="2" cellspacing="1" width="100%" border="0">
                                                                                <tr>
                                                                                    <td valign="middle" width="35%" align="left">
                                                                                        <a href="Home.aspx" class="aHome" id="anchHome" runat="server">PopCorn</a>&nbsp;&nbsp;<img
                                                                                            src="../images/arrowright.gif">&nbsp;&nbsp;<b>Agents</b>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td align="left" width="100%">
                                                                            <table cellpadding="1" cellspacing="1" width="100%" border="0" class="tableoverview">
                                                                                <tr>
                                                                                    <td valign="middle" align="center" width="100%">
                                                                                        <table cellspacing="0" cellpadding="2" width="100%" border="0">
                                                                                            <%--<tr>
                                                                                                    <td align="center">
                                                                                                        <asp:GridView ID="grdCurrentlySelectedAgent" runat="server" AutoGenerateColumns="False"
                                                                                                            Width="100%">
                                                                                                            <EmptyDataRowStyle CssClass="tableHdr" />
                                                                                                            <EmptyDataTemplate>
                                                                                                                No Online Agents Found</EmptyDataTemplate>
                                                                                                            <Columns>
                                                                                                                <asp:BoundField HeaderText="L/S" />
                                                                                                                <asp:BoundField HeaderText="Press." />
                                                                                                                <asp:BoundField HeaderText="Calls" />
                                                                                                                <asp:BoundField HeaderText="Ratio" />
                                                                                                                <asp:BoundField HeaderText="Talk" />
                                                                                                                <asp:BoundField HeaderText="Ready" />
                                                                                                                <asp:BoundField HeaderText="Pause" />
                                                                                                                <asp:BoundField HeaderText="Wrap" />
                                                                                                            </Columns>
                                                                                                            <HeaderStyle CssClass="tablecontentBG" />
                                                                                                            <RowStyle CssClass="tdWhite" />
                                                                                                        </asp:GridView>
                                                                                                    </td>
                                                                                                </tr>--%>
                                                                                            <tr>
                                                                                                <td valign="middle" align="center" width="100%">
                                                                                                    <asp:Panel ID="pnlNormalView" runat="server">
                                                                                                        <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdAgent" ShowFooter="true"
                                                                                                            OnRowDataBound="grdAgent_RowDataBound" Width="100%" CellSpacing="1" BorderWidth="1"
                                                                                                            GridLines="Both" BorderColor="#000000" CellPadding="5" EmptyDataText="No Online Agents Found"
                                                                                                            HeaderStyle-BackColor="#333366" HeaderStyle-ForeColor="#FFFFFF" HeaderStyle-HorizontalAlign="Center"
                                                                                                            HeaderStyle-Font-Bold="true" FooterStyle-BackColor="#99CC66">
                                                                                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                                                                                            <Columns>
                                                                                                                <asp:BoundField HeaderText="Station" DataField="StationIP" />
                                                                                                                <asp:BoundField HeaderText="Name" DataField="AgentName" />
                                                                                                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                                                                                                <asp:BoundField HeaderText="L/S" DataField="LeadsSales" />
                                                                                                                <asp:BoundField HeaderText="Press." DataField="Presentations" />
                                                                                                                <asp:BoundField HeaderText="Calls" DataField="Calls" />
                                                                                                                <asp:BoundField HeaderText="Ratio" DataField="LeadSalesRatio" />
                                                                                                                <asp:BoundField HeaderText="Talk" DataField="TalkTime" />
                                                                                                                <asp:BoundField HeaderText="Ready" DataField="WaitingTime" />
                                                                                                                <asp:BoundField HeaderText="Pause" DataField="PauseTime" />
                                                                                                                <asp:BoundField HeaderText="Wrap" DataField="WrapTime" />
                                                                                                                <asp:BoundField HeaderText="Log-In" DataField="LoginDate" DataFormatString="{0:MM/dd/yyyy HH:mm:ss}" />
                                                                                                                <asp:BoundField HeaderText="Log-Off" DataField="LogOffDate" DataFormatString="{0:MM/dd/yyyy HH:mm:ss}" />
                                                                                                                <asp:BoundField HeaderText="Last Rslt" DataField="ResultCodeDesc" />
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </asp:Panel>
                                                                                                    <asp:Panel ID="pnlFundraiserView" runat="server">
                                                                                                        <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdFundraiserAgent"
                                                                                                            ShowFooter="true" OnRowDataBound="grdFundAgent_RowDataBound" runat="server" Width="100%"
                                                                                                            CellSpacing="1" BorderWidth="1" GridLines="Both" BorderColor="#000000" CellPadding="5"
                                                                                                            EmptyDataText="No Online Agents Found" HeaderStyle-BackColor="#333366" HeaderStyle-ForeColor="#FFFFFF"
                                                                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" FooterStyle-BackColor="#99CC66">
                                                                                                            <AlternatingRowStyle BackColor="#CCCCCC" />
                                                                                                            <Columns>
                                                                                                                <asp:BoundField HeaderText="Station" DataField="StationIP" />
                                                                                                                <asp:BoundField HeaderText="Name" DataField="AgentName" />
                                                                                                                <asp:BoundField HeaderText="Status" DataField="Status" />
                                                                                                                <asp:BoundField HeaderText="Pledges" DataField="PledgeAmount" />
                                                                                                                <asp:BoundField HeaderText="Press." DataField="Presentations" />
                                                                                                                <asp:BoundField HeaderText="Calls" DataField="Calls" />
                                                                                                                <asp:BoundField HeaderText="Ratio" DataField="LeadSalesRatio" />
                                                                                                                <asp:BoundField HeaderText="Talk" DataField="TalkTime" />
                                                                                                                <asp:BoundField HeaderText="Ready" DataField="WaitingTime" />
                                                                                                                <asp:BoundField HeaderText="Pause" DataField="PauseTime" />
                                                                                                                <asp:BoundField HeaderText="Wrap" DataField="WrapTime" />
                                                                                                                <asp:BoundField HeaderText="Log-In" DataField="LoginDate" DataFormatString="{0:MM/dd/yyyy HH:mm:ss}" />
                                                                                                                <asp:BoundField HeaderText="Log-Off" DataField="LogOffDate" DataFormatString="{0:MM/dd/yyyy HH:mm:ss}" />
                                                                                                                <asp:BoundField HeaderText="Last Rslt" DataField="ResultCodeDesc" />
                                                                                                            </Columns>
                                                                                                        </asp:GridView>
                                                                                                    </asp:Panel>
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
                                                                <table cellpadding="4" cellspacing="1" border="0" width="100%" class="tableoverview">
                                                                    <tr>
                                                                        <td valign="bottom" align="right">
                                                                            <asp:LinkButton ID="lbtnClose" runat="server" PostBackUrl="~/campaign/Home.aspx"
                                                                                CausesValidation="false" CssClass="button blue small">Close</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <!-- Body -->
                                            <!-- Footer -->
                                            <RainMaker:Footer ID="CampaignFooter" runat="server"></RainMaker:Footer>
                                            <!-- Footer -->
                                        </td>
                                    </tr>
                                </table>
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
