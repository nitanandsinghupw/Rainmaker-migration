<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AgentList.aspx.cs" Inherits="Rainmaker.Web.agent.AgentList" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader2.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Agent List</title>

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
<body onload="ShowPageMessage()">
    <form id="frmAgentList" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>

    <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                    <tr>
                        <td>
                            <RainMaker:Header ID="campaignHeader" runat="server"></RainMaker:Header>
                            <!-- Body -->
                            <table cellpadding="0" cellspacing="0" height="400px" border="0" width="100%">
                                <tr>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                            <tr>
                                                <td valign="top">
                                                    <table cellpadding="0" cellspacing="1" border="0" width="100%">
                                                        <tr>
                                                            <td align="left" width="100%">
                                                                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                    <tr>
                                                                        <td valign="middle" align="center" width="100%">
                                                                            <table cellspacing="1" cellpadding="4" width="100%" border="0">
                                                                                <tr>
                                                                                <td class="tdRight" colspan="2">
                                                                                    &nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td valign="middle" align="center" width="100%">
                                                                                        <asp:GridView runat="server" AutoGenerateColumns="False" DataKeyNames="AgentID" ID="grdAgentList"
                                                                                            Width="100%" CellPadding="3" CellSpacing="1" BorderWidth="0" 
                                                                                            CssClass="tablecontentBlack" BorderStyle="Solid">
                                                                                            <HeaderStyle CssClass="tableHdr" />
                                                                                            <RowStyle CssClass="tableRow" />
                                                                                            <EmptyDataRowStyle CssClass="tableHdr" />
                                                                                            <EmptyDataTemplate>
                                                                                                No Agent Found</EmptyDataTemplate>
                                                                                            <Columns>
                                                                                                <asp:BoundField DataField="AgentID" HeaderText="ID" />
                                                                                                <asp:TemplateField HeaderText="Agent Name">
                                                                                                    <ItemStyle Width="25%" />
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton CssClass="alink" ID="lbtnCampaign" Text='<%# Eval("AgentName") %>'
                                                                                                            runat="server" CommandArgument='<%# Eval("AgentID") %>' OnClick="lbtnAgent_Click"></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:BoundField DataField="LoginName" HeaderText="Login Name" />
                                                                                                <asp:BoundField DataField="IsAdministrator" HeaderText="IsAdmin" />
                                                                                                <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
                                                                                                <asp:TemplateField HeaderText="Delete">
                                                                                                    <ItemStyle Width="7%" HorizontalAlign="Center" />
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="lbtnDelete" runat="server" CssClass="alink" CommandName='<%# Eval("IsDefault") %>'
                                                                                                            CommandArgument='<%# Eval("AgentID") %>' OnClick="lbtnDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to delete this agent?');"><div class="button blue small">Delete</div></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Reset Agent">
                                                                                                    <ItemStyle Width="7%" HorizontalAlign="Center" />
                                                                                                    <ItemTemplate>
                                                                                                        <asp:LinkButton ID="lbtnReset" runat="server" CssClass="alink" CommandName='<%# Eval("IsDefault") %>'
                                                                                                            CommandArgument='<%# Eval("AgentID") %>' OnClick="lbtnReset_Click" OnClientClick="javascript:return confirm('Are you sure you want to reset this agent?');"><div class="button blue small">Reset</div></asp:LinkButton>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                            </Columns>
                                                                                        </asp:GridView>
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
                                        </table>
                                    </td>
                                </tr>
                                <tr valign="bottom">
                                    <td align="right" valign="bottom">
                                        <table cellspacing="1" width="100%" cellpadding="4" border="0" id="Table4">
                                            <tr valign="bottom">
                                                <td align="right" valign="bottom">
                                                    <asp:LinkButton ID="lbtnAdd" PostBackUrl="~/agent/AgentDetail.aspx" runat="server" Text="Add" CssClass="button blue small"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                        ID="lbtnClose" runat="server"  PostBackUrl="~/campaign/CampaignList.aspx"
                                                        CausesValidation="false" Text="Close" CssClass="button blue small"></asp:LinkButton>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <!-- Body -->
                            <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
