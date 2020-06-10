<%@ Page Language="C#" AutoEventWireup="true" Codebehind="QueryList.aspx.cs" Inherits="Rainmaker.Web.campaign.QueryList" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Query List</title>
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
    <form id="frmQueryList" runat="server">
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
                                        <td valign="top">
                                            <table cellpadding="5" cellspacing="5" border="0" width="100%">
                                                <tr>
                                                    <td valign="top">
                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td valign="middle" width="35%" align="left">
                                                                    <a href="Home.aspx" class="aHome" id="anchHome" runat="server"></a>&nbsp;&nbsp;<img
                                                                        src="../images/arrowright.gif" alt="" />&nbsp;&nbsp;<b>Query List</b></td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td align="left" width="100%">
                                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="middle" align="center" width="100%">
                                                                                <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdQueryList" Width="100%"
                                                                                                CellPadding="3" CellSpacing="1" BorderWidth="0" CssClass="tablecontentBlack">
                                                                                                <HeaderStyle CssClass="tableHdr" />
                                                                                                <RowStyle CssClass="tableRow" />
                                                                                                <EmptyDataRowStyle CssClass="tableHdr" />
                                                                                                <EmptyDataTemplate>
                                                                                                    No Query Found</EmptyDataTemplate>
                                                                                                <Columns>
                                                                                                    <asp:TemplateField HeaderText="Query Name">
                                                                                                        <ItemStyle Width="70%" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton CssClass="alink" ID="lbtnQuery" Text='<%# Eval("QueryName") %>' runat="server"
                                                                                                                CommandArgument='<%# Eval("QueryID") %>' OnClick="lbtnQuery_Click"></asp:LinkButton>
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField DataField="DateCreated" HeaderText="Date Created" ItemStyle-Width="20%" />
                                                                                                    <asp:TemplateField HeaderText="Delete">
                                                                                                        <ItemStyle />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton ID="lbtnDelete" Text="Delete" runat="server" CssClass="alink" CommandArgument='<%# Eval("QueryID") %>'
                                                                                                                OnClick="lbtnDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to delete this query?');"></asp:LinkButton>
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
                                            <table cellspacing="5" width="100%" cellpadding="5" border="0" id="Table4">
                                                <tr valign="bottom">
                                                    <td align="right" valign="bottom">
                                                        <asp:LinkButton ID="lbtnAddQuery" runat="server" PostBackUrl="~/campaign/queryDetail.aspx"><img src="../images/AddQuery.jpg" border="0" alt="" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                            ID="lbtnClose" runat="server" PostBackUrl="~/campaign/home.aspx"><img alt="" src="../images/close.jpg" border="0" /></asp:LinkButton></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
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
    </form>
</body>
</html>
