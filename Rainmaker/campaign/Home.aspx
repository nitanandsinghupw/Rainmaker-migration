<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" ValidateRequest="false"
    Inherits="Rainmaker.Web.campaign.Home" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Home</title>

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
<%--Comments below with '&&&' have been removed when home page was merged with query status, version 1.5.2.  They have been left in case revival of query status is required.--%>
<body>
    <form id="frmHome" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>

    <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="5000">
    </asp:Timer>
    <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                    <tr>
                        <td>
                            <RainMaker:Header ID="campaignHeader" runat="server"></RainMaker:Header>
                            <!-- Body -->
                            <asp:UpdatePanel ID="updHome" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                </Triggers>
                                <ContentTemplate>
                                    <table cellpadding="0" cellspacing="0" height="275px" border="0" width="100%">
                                        <tr>
                                            <td valign="top">
                                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td valign="top" width="65%">
                                                            <table cellpadding="2" cellspacing="5" border="0" width="100%">
                                                                <tr>
                                                                    <td valign="middle" width="55%" align="right">
                                                                        <b>Campaign&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td valign="middle" align="left">
                                                                        <asp:Label ID="lblCampaign" runat="server" Style="font-weight: bold"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="middle" width="55%" align="right">
                                                                        <b>Date of Creation&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td valign="middle" align="left">
                                                                        <asp:Label ID="lblDateCreated" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="middle" width="55%" align="right">
                                                                        <b>Number of Records in Dialer Queue&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td valign="middle" align="left">
                                                                        <asp:Label ID="lblDialerQueue" runat="server">0</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="middle" width="55%" align="right">
                                                                        <b>Active Query List Count&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td valign="middle" align="left">
                                                                        <asp:Label ID="lblQueryListCount" runat="server">0</asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <%--<tr>
                                                                        &&&<td valign="middle" width="55%" align="right">
                                                                            <b>Number of calls in Inbound Queue&nbsp;:&nbsp;&nbsp;</b></td>
                                                                        <td valign="middle" align="left">
                                                                            <asp:Label ID="lblInboundQueue" runat="server">0</asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="middle" width="55%" align="right">
                                                                            <b>Average Time(in seconds) in Inbound Hold Queue&nbsp;:&nbsp;&nbsp;</b></td>
                                                                        <td valign="middle" align="left">
                                                                            <asp:Label ID="lblAvgInboundQueue" runat="server">0</asp:Label></td>
                                                                    </tr>--%>
                                                                <tr>
                                                                    <td valign="middle" align="right">
                                                                        <b>Last Run Time&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td valign="middle" align="left">
                                                                        <asp:Label ID="lblTimeStarted" runat="server"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="middle" align="right">
                                                                        <asp:CheckBox ID="chkFCQI" runat="server" />
                                                                        <b>
                                                                            <asp:Label ID="lblFCQI" runat="server">Flush Call Queue on Idle&nbsp;&nbsp;&nbsp;&nbsp;</asp:Label></b>
                                                                    </td>
                                                                    <td>
                                                                        <td align="left">
                                                                            <asp:CheckBox ID="chkQSP" runat="server" Checked="false" AutoPostBack="True" OnCheckedChanged="chkQSP_CheckedChanged" />Show
                                                                            Query Statistics as Percentages
                                                                        </td>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                            <td valign="top" width="35%">
                                                <table cellpadding="2" cellspacing="5" border="0" width="100%">
                                                    <tr>
                                                        <td align="left">
                                                            <table cellspacing="1" width="100%" cellpadding="0" border="0" id="Table3" class="tablecontentBlack">
                                                                <tr valign="top" style="background-color: #333366; fore-color: #FFFFFF; font-weight: bold">
                                                                    <td height="20px" valign="middle" style=" font-weight: bold" align="center"
                                                                        width="80%" class="tableHdr">
                                                                        Campaign Tools
                                                                    </td>
                                                                    <td align="center" valign="middle" class="tableHdr" style=" font-weight: bold" width="20%">
                                                                        Info
                                                                    </td>
                                                                </tr>
                                                                <tr valign="top">
                                                                    <td height="20px" valign="middle" class="tdWhite" colspan="2">
                                                                        <a href="#" class="aScoreBoard">Status</a> &nbsp;&nbsp;
                                                                        <asp:LinkButton runat="server" ID="lbtnIdle" OnClick="lbtnIdle_Click" CssClass="button red small">Idle</asp:LinkButton>&nbsp;&nbsp;
                                                                        <asp:LinkButton runat="server" ID="lbtnRun" CssClass="button red small" OnClick="lbtnRun_Click">Run</asp:LinkButton>&nbsp;&nbsp;
                                                                        <asp:LinkButton runat="server" ID="lbtnPause" CssClass="button red small" OnClick="lbtnPause_Click">Pause</asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                                <!--      <tr valign="top">
                                                                        <td height="20px" valign="middle" class="tdWhite" align="left">
                                                                            <a href="ResultCodes.aspx" class="aScoreBoard">Result Codes</a>
                                                                        </td>
                                                                        <td valign="middle" class="tdWhite" align="center">
                                                                            <a href="ResultCodes.aspx" class="aScoreBoard" runat="server" id="anchResultCodes">N/A</a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr valign="top">
                                                                        <td height="20px" valign="middle" class="tdWhite" align="left">
                                                                            <a href="DialParams.aspx" class="aScoreBoard">Dialing Parameters</a>
                                                                        </td>
                                                                        <td align="center" valign="middle" class="tdWhite">
                                                                            <a href="DialParams.aspx" class="aScoreBoard" runat="server" id="anchDialParams">N/A</a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr valign="top">
                                                                        <td height="20px" valign="middle" class="tdWhite" align="left">
                                                                            <a href="OtherParams.aspx" class="aScoreBoard">Other Parameters</a>
                                                                        </td>
                                                                        <td align="center" valign="middle" class="tdWhite">
                                                                            <a href="OtherParams.aspx" class="aScoreBoard" runat="server" id="anchOtherParams">N/A</a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr valign="top">
                                                                        <td height="20px" valign="middle" class="tdWhite" align="left">
                                                                            <a href="Recordings.aspx" class="aScoreBoard">Digitized Recording</a>
                                                                        </td>
                                                                        <td align="center" valign="middle" class="tdWhite">
                                                                            <a href="Recordings.aspx" class="aScoreBoard" runat="server" id="anchRecordings">N/A</a>
                                                                        </td>
                                                                    </tr> -->
                                                                <tr valign="top">
                                                                    <td height="20px" valign="middle" class="tdWhite" align="left">
                                                                        <a href="Agents.aspx" class="aScoreBoard">Agent Stats</a>
                                                                    </td>
                                                                    <td align="center" valign="middle" class="tdWhite">
                                                                        <a href="Agents.aspx" class="aScoreBoard" runat="server" id="anchAgents">N/A</a>
                                                                    </td>
                                                                </tr>
                                                                <!--     <tr valign="top">
                                                                        <td height="20px" valign="middle" class="tdWhite" align="left">
                                                                            <a href="queryStatus.aspx" class="aScoreBoard">Queries</a>
                                                                        </td>
                                                                        <td align="center" valign="middle" class="tdWhite">
                                                                            <a href="queryStatus.aspx" class="aScoreBoard" runat="server" id="anchqueryStatus">N/A</a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr valign="top">
                                                                        <td height="20px" valign="middle" class="tdWhite" align="left">
                                                                            <a href="ScriptList.aspx" class="aScoreBoard">Scripts</a>
                                                                        </td>
                                                                        <td align="center" valign="middle" class="tdWhite">
                                                                            <a href="ScriptList.aspx" class="aScoreBoard" runat="server" id="anchScriptList">0</a>
                                                                        </td>
                                                                    </tr>
                                                                  <tr valign="top" id="trTraining" runat="server">
                                                                        <td height="20px" valign="middle" class="tdWhite" align="left">
                                                                            <a href="TrainingList.aspx" class="aScoreBoard">Training Schemes</a>
                                                                        </td>
                                                                        <td align="center" valign="middle" class="tdWhite">
                                                                            <a href="TrainingList.aspx" class="aScoreBoard" runat="server" id="anchTrainingList">0</a>
                                                                        </td>
                                                                    </tr>
                                                                   
                                                                    <tr valign="top">
                                                                        <td height="20px" valign="middle" class="tdWhite" align="left" colspan="2">
                                                                            <a href="CampaignFieldsList.aspx" class="aScoreBoard">Field Manager</a>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="20px" valign="middle" class="tdWhite" align="left" colspan="2">
                                                                            <a href="Import.aspx" class="aScoreBoard">Import Phone Numbers</a>
                                                                        </td>  -->
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr valign="bottom">
                                            <td align="right" valign="bottom" colspan="2" width="100%">
                                                <table cellspacing="2" width="100%" cellpadding="4" border="0" id="Table4">
                                                    <tr valign="bottom">
                                                        <td align="right" valign="bottom">
                                                            <asp:LinkButton ID="lbtnCloneCampaign" 
                                                                PostBackUrl="~/campaign/CloneCampaign.aspx" runat="server"><div class="button blue small" alt="" border="0" />Save As</div></asp:LinkButton>&nbsp;&nbsp;
                                                            <asp:LinkButton ID="lbtnEdit" PostBackUrl="~/campaign/CreateCampaign.aspx" runat="server"><div class="button blue small" alt="" border="0" />Edit</div></asp:LinkButton>&nbsp;&nbsp;
                                                            
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    </td> </tr>
                                    <tr>
                                        <td colspan="2" width="100%">
                                            <table cellpadding="0" cellspacing="1" width="100%" border="0">
                                                <%-- Main container for query tables.  May use bkgnd color or borders--%>
                                                <tr>
                                                    <td valign="middle" align="left" style="color: #4875A5; font-size: small">
                                                        <b>Current Campaign Queries</b>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="middle" align="center" width="100%">
                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                            <tr>
                                                                <td align="left" width="100%">
                                                                    <table cellpadding="1" cellspacing="1" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="middle" align="left">
                                                                                <b>Currently Active</b>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle" align="center" width="100%">
                                                                                <asp:UpdatePanel ID="updatePanelAQ" runat="server" ChildrenAsTriggers="true">
                                                                                    <Triggers>
                                                                                        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                                                        <%--<asp:PostBackTrigger ControlID="Timer1" />--%>
                                                                                    </Triggers>
                                                                                    <ContentTemplate>
                                                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:Label ID="lblNoActive" Text="There are currently no active queries." runat="server"
                                                                                                        Visible="false" />
                                                                                                    <asp:DataGrid ID="grdActiveQueries" runat="server" HeaderStyle-BackColor="#333366"
                                                                                                        HeaderStyle-ForeColor="#FFFFFF" GridLines="Both" BorderColor="DarkGray" CellPadding="5"
                                                                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" EditItemStyle-BackColor="#CCFF66"
                                                                                                        AllowSorting="false" AutoGenerateColumns="false" OnItemDataBound="grdActiveQueries_DataBind">
                                                                                                        <AlternatingItemStyle BackColor="#CCFFFF" />
                                                                                                        <Columns>
                                                                                                            <asp:TemplateColumn HeaderText="Query Name" ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton CssClass="alink" ID="lbtnQuery" Text='<%# Eval("QueryName") %>' runat="server"
                                                                                                                        CommandArgument='<%# Eval("QueryID") %>' OnClick="lbtnQuery_Click"></asp:LinkButton>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateColumn>
                                                                                                            <%--<asp:BoundColumn DataField="Priority" HeaderText="Priority" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />--%>
                                                                                                            <asp:BoundColumn DataField="Total" HeaderText="Total" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF"
                                                                                                                ItemStyle-BackColor="LightGray" ItemStyle-Font-Bold="true" />
                                                                                                            <asp:BoundColumn DataField="Available" HeaderText="Avail." ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Dials" HeaderText="Dials" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Talks" HeaderText="Talks" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="AnsweringMachine" HeaderText="Machine" ItemStyle-Width="5%"
                                                                                                                HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="NoAnswer" HeaderText="NoAns" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Busy" HeaderText="Busies" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="OpInt" HeaderText="OPI's" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Drops" HeaderText="Drops" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Failed" HeaderText="Failed" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:TemplateColumn HeaderText="Action" ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF">
                                                                                                                <ItemStyle />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lbtnStandby" Text="Standby" runat="server" class="myButton" alt="Standby"
                                                                                                                        CommandArgument='<%# Eval("CampaignQueryID") %>' OnClick="lbtnStandby_Click"></asp:LinkButton>&nbsp;
                                                                                                                    <asp:LinkButton ID="lbtnHold" Text="Hold" runat="server" class="myButton" alt="Hold"
                                                                                                                        CommandArgument='<%# Eval("CampaignQueryID") %>' OnClick="lbtnHold_Click"></asp:LinkButton>
                                                                                                                    <asp:LinkButton ID="lbtnDelete" Text="Delete" runat="server" class="myButton" alt="Delete"
                                                                                                                        CommandArgument='<%# Eval("QueryID") %>' OnClick="lbtnDelete_Click"></asp:LinkButton>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateColumn>
                                                                                                        </Columns>
                                                                                                    </asp:DataGrid>
                                                                                                    <asp:SqlDataSource ID="dsActiveQueries" runat="server"></asp:SqlDataSource>
                                                                                                    <asp:SqlDataSource ID="dsStandbyQueries" runat="server"></asp:SqlDataSource>
                                                                                                    <asp:SqlDataSource ID="dsAllQueries" runat="server"></asp:SqlDataSource>
                                                                                                    <asp:SqlDataSource ID="dsPriority" runat="server"></asp:SqlDataSource>
                                                                                                    <asp:SqlDataSource ID="dsQueryStats" runat="server"></asp:SqlDataSource>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" width="100%">
                                                                    <table cellpadding="1" cellspacing="1" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="middle" align="left">
                                                                                <b>StandBy</b>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle" align="center" width="100%">
                                                                                <table cellspacing="0" cellpadding="1" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:UpdatePanel ID="updStandByQuery" runat="server" UpdateMode="conditional">
                                                                                                <ContentTemplate>
                                                                                                    <asp:Label ID="lblNoStanByQueries" Text="There are currently no standby queries."
                                                                                                        runat="server" Visible="false" />
                                                                                                    <asp:DataGrid ID="grdStandbyQueries" runat="server" HeaderStyle-BackColor="#333366"
                                                                                                        HeaderStyle-ForeColor="#FFFFFF" GridLines="Both" BorderColor="DarkGray" CellPadding="5"
                                                                                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" EditItemStyle-BackColor="#CCFF66"
                                                                                                        AllowSorting="false" AutoGenerateColumns="false" OnItemDataBound="grdStandbyQueries_DataBind">
                                                                                                        <AlternatingItemStyle BackColor="#CCFFFF" />
                                                                                                        <Columns>
                                                                                                            <asp:TemplateColumn HeaderText="Name" ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton CssClass="alink" ID="lbtnQuery" Text='<%# Eval("QueryName") %>' runat="server"
                                                                                                                        CommandArgument='<%# Eval("QueryID") %>' OnClick="lbtnQuery_Click"></asp:LinkButton>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateColumn>
                                                                                                            <%--<asp:EditCommandColumn EditText="Prioritize" ItemStyle-ForeColor="#4875A5"
                                                                                                                      ButtonType="LinkButton"
                                                                                                                      UpdateText="Update" CancelText="Cancel" ItemStyle-Width="6%"  />--%>
                                                                                                            <%--<asp:BoundColumn DataField="Priority" HeaderText="Priority" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />--%>
                                                                                                            <asp:BoundColumn DataField="Total" HeaderText="Total" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <%--<asp:BoundColumn DataField="Available" HeaderText="Available" ItemStyle-Width="8%" HeaderStyle-ForeColor="#FFFFFF"  />--%>
                                                                                                            <asp:BoundColumn DataField="Dials" HeaderText="Dials" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Talks" HeaderText="Talks" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="AnsweringMachine" HeaderText="Machine" ItemStyle-Width="5%"
                                                                                                                HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="NoAnswer" HeaderText="NoAns" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Busy" HeaderText="Busies" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="OpInt" HeaderText="OPI's" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Drops" HeaderText="Drops" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Failed" HeaderText="Failed" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:TemplateColumn HeaderText="Action" ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF">
                                                                                                                <ItemStyle />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lbtnActivate" Text="Activate" runat="server" class="myButton"
                                                                                                                        alt="Activate" CommandArgument='<%# Eval("CampaignQueryID") %>' OnClick="lbtnActivate_Click"></asp:LinkButton>&nbsp;
                                                                                                                    <asp:LinkButton ID="lbtnHold" Text="Hold" runat="server" class="myButton" alt="Hold"
                                                                                                                        CommandArgument='<%# Eval("CampaignQueryID") %>' OnClick="lbtnHold_Click"></asp:LinkButton>
                                                                                                                    <asp:LinkButton ID="lbtnDelete" Text="Delete" runat="server" class="myButton" alt="Delete"
                                                                                                                        CommandArgument='<%# Eval("QueryID") %>' OnClick="lbtnDelete_Click"></asp:LinkButton>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateColumn>
                                                                                                        </Columns>
                                                                                                    </asp:DataGrid>
                                                                                                </ContentTemplate>
                                                                                            </asp:UpdatePanel>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            &nbsp;
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" width="100%">
                                                                    <table cellpadding="1" cellspacing="1" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="middle" align="left">
                                                                                <b>All Queries</b>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle" align="center" width="100%">
                                                                                <asp:UpdatePanel ID="updAllQueries" runat="server" ChildrenAsTriggers="true">
                                                                                    <Triggers>
                                                                                        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                                                        <%--<asp:PostBackTrigger ControlID="Timer1" />--%>
                                                                                    </Triggers>
                                                                                    <ContentTemplate>
                                                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    <asp:Label ID="lblAllQueries" Text="There are currently no queries." runat="server"
                                                                                                        Visible="false" />
                                                                                                    <asp:DataGrid ID="grdAllQueries" runat="server" HeaderStyle-BackColor="#333366" HeaderStyle-ForeColor="#FFFFFF"
                                                                                                        GridLines="Both" BorderColor="DarkGray" CellPadding="5" HeaderStyle-HorizontalAlign="Center"
                                                                                                        HeaderStyle-Font-Bold="true" EditItemStyle-BackColor="#CCFF66" AllowSorting="false"
                                                                                                        AutoGenerateColumns="false" OnItemDataBound="grdAllQueries_DataBind">
                                                                                                        <AlternatingItemStyle BackColor="#CCFFFF" />
                                                                                                        <Columns>
                                                                                                            <asp:TemplateColumn HeaderText="Name" ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton CssClass="alink" ID="lbtnQuery" Text='<%# Eval("QueryName") %>' runat="server"
                                                                                                                        CommandArgument='<%# Eval("QueryID") %>' OnClick="lbtnQuery_Click"></asp:LinkButton>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateColumn>
                                                                                                            <%--<asp:EditCommandColumn EditText="Prioritize" ItemStyle-ForeColor="#4875A5"
                                                                                                                      ButtonType="LinkButton"
                                                                                                                      UpdateText="Update" CancelText="Cancel" ItemStyle-Width="6%"  />
                                                                                                                <asp:BoundColumn DataField="Priority" HeaderText="Priority" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />--%>
                                                                                                            <asp:BoundColumn DataField="Total" HeaderText="Total" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <%--<asp:BoundColumn DataField="Available" HeaderText="Available" ItemStyle-Width="8%" HeaderStyle-ForeColor="#FFFFFF"  />--%>
                                                                                                            <asp:BoundColumn DataField="Dials" HeaderText="Dials" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Talks" HeaderText="Talks" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="AnsweringMachine" HeaderText="Machine" ItemStyle-Width="5%"
                                                                                                                HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="NoAnswer" HeaderText="NoAns" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Busy" HeaderText="Busies" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="OpInt" HeaderText="OPI's" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Drops" HeaderText="Drops" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:BoundColumn DataField="Failed" HeaderText="Failed" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF" />
                                                                                                            <asp:TemplateColumn HeaderText="Action" ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF">
                                                                                                                <ItemStyle />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lbtnActivate" Text="Activate" runat="server" class="myButton"
                                                                                                                        alt="Activate" CommandArgument='<%# Eval("CampaignQueryID") %>' OnClick="lbtnActivate_Click"></asp:LinkButton>&nbsp;
                                                                                                                    <asp:LinkButton ID="lbtnStandby" Text="Standby" runat="server" class="myButton" alt="Standby"
                                                                                                                        CommandArgument='<%# Eval("CampaignQueryID") %>' OnClick="lbtnStandby_Click"></asp:LinkButton>&nbsp;
                                                                                                                    <asp:LinkButton ID="lbtnDelete" Text="Delete" runat="server" class="myButton" alt="Delete"
                                                                                                                        CommandArgument='<%# Eval("QueryID") %>' OnClick="lbtnDelete_Click"></asp:LinkButton>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateColumn>
                                                                                                        </Columns>
                                                                                                    </asp:DataGrid>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="center">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <%--&&&<tr>
                                                                        <td align="center">
                                                                            <asp:GridView runat="server" AutoGenerateColumns="False" 
                                                                                ID="grdActiveQueries" Width="100%" CellPadding="3" CellSpacing="1" BorderWidth="0"
                                                                                CssClass="tablecontentBlack" OnRowDataBound="grdActiveQueries_RowDataBound">
                                                                                <HeaderStyle CssClass="tableHdr" />
                                                                                <RowStyle CssClass="tableRow" />
                                                                                <EmptyDataRowStyle CssClass="tableHdr" />
                                                                                <EmptyDataTemplate>
                                                                                    No Active Queries</EmptyDataTemplate>
                                                                                <Columns>
                                                                                    <asp:BoundField DataField="QueryName" HeaderText="Query Name" ItemStyle-Width="25%" />
                                                                                    <asp:BoundField DataField="Total" HeaderText="Total" ItemStyle-Width="8%" />
                                                                                    <asp:BoundField DataField="Available" HeaderText="Available" ItemStyle-Width="10%" />
                                                                                    <asp:BoundField DataField="Dials" HeaderText="Dials" ItemStyle-Width="7%" />
                                                                                    <asp:BoundField DataField="Talks" HeaderText="Talks" ItemStyle-Width="7%" />
                                                                                    <asp:BoundField DataField="AnsweringMachine" HeaderText="AnsMach" ItemStyle-Width="8%" />
                                                                                    <asp:BoundField DataField="NoAnswer" HeaderText="NoAns" ItemStyle-Width="7%" />
                                                                                    <asp:BoundField DataField="Busy" HeaderText="Busy" ItemStyle-Width="7%" />
                                                                                    <asp:BoundField DataField="OpInt" HeaderText="OpInt" ItemStyle-Width="7%" />
                                                                                    <asp:BoundField DataField="Drops" HeaderText="Drops" ItemStyle-Width="7%" />
                                                                                    <asp:BoundField DataField="Failed" HeaderText="Failed" ItemStyle-Width="7%" />
                                                                                    <asp:TemplateField HeaderText="Clear">
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lbtnClear" Text="Clear" runat="server" CssClass="alink" CommandArgument='<%# Eval("CampaignQueryID") %>' CommandName='<%# Eval("QueryCondition")%>'  OnClick="lbtnClear_Click" OnClientClick="return confirm('Are you sure you want to clear the previous calls?');"></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                                <td align="left">
                                                                                    <asp:CheckBox ID="chkQSP" runat="server" Checked="false" AutoPostBack="True" OnCheckedChanged="chkQSP_CheckedChanged" />Show Query Statistics
                                                                                    as Percentages</td>
                                                                            </tr>
                                                                    <tr>
                                                                        <td align="center">
                                                                            &nbsp;</td>
                                                                    </tr>--%>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr valign="bottom">
                                                    <td align="right" valign="bottom">
                                                        <table cellspacing="2" width="100%" cellpadding="4" border="0" id="Table1">
                                                            <tr valign="bottom">
                                                                <td align="right" valign="bottom">
                                                                    <asp:LinkButton ID="lbtnAddQuery" runat="server" PostBackUrl="~/campaign/QueryDetailTree.aspx"><div class="button blue small" alt="" border="0" style="width:50px" >Add Query</div></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <!-- Body -->
                            <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>

    <script language="javascript" type="text/javascript">
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args)
        {
           if (args.get_error() != undefined)
           {
               if ((args.get_response().get_statusCode() == '12007') || (args.get_response().get_statusCode() == '12029'))
               {
                //Show a Message like 'Please make sure you are connected to internet';
                alert('Please make sure you are connected to internet');
                args.set_errorHandled(true); 
               }
           }
        }
    
    </script>

</body>
</html>
