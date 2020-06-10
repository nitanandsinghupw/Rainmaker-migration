<%@ Page Language="C#" AutoEventWireup="true" Codebehind="QueryStatus.aspx.cs" Inherits="Rainmaker.Web.campaign.QueryStatus" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Query Status</title>
    <%-- <meta http-equiv="refresh" content="60">--%>
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
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
        <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="5000">
        </asp:Timer>
        <div>
            <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                            <tr>
                                <td>
                                    <RainMaker:Header ID="campaignHeader" runat="server"></RainMaker:Header>
                                    <!-- Body -->
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td valign="top">
                                                <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td valign="top">
                                                            <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                                <tr>
                                                                    <td valign="middle" width="35%" align="left">
                                                                        <a href="Home.aspx" class="aHome" id="anchHome" runat="server"></a>&nbsp;&nbsp;<img
                                                                            src="../images/arrowright.gif" />&nbsp;&nbsp;<b>Query Status List</b></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <table cellpadding="0" cellspacing="1" border="0" width="100%">
                                                                <tr>
                                                                    <td align="left" width="100%">
                                                                        <table cellpadding="1" cellspacing="1" width="100%" border="0">
                                                                            <tr>
                                                                                <td valign="middle" align="left">
                                                                                    <b>Currently Active</b></td>
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
                                                                                                        <asp:Label ID="lblNoActive" Text="There are currently no active queries." runat="server" Visible="false" />
                                                                                                        <asp:DataGrid ID="grdActiveQueries" runat="server" HeaderStyle-BackColor="#333366" HeaderStyle-ForeColor="#FFFFFF" GridLines="Both" BorderColor="DarkGray" CellPadding="5" 
                                                                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" EditItemStyle-BackColor="#CCFF66"  
                                                                                                            AllowSorting="false" AutoGenerateColumns="false"
                                                                                                            OnItemDataBound="grdActiveQueries_DataBind" 
                                                                                                            OnEditCommand="grdActiveQueries_EditCommand" OnCancelCommand="grdActiveQueries_CancelEdit" OnUpdateCommand="grdActiveQueries_UpdateRecord"
                                                                                                            OnDeleteCommand="grdActiveQueries_DeleteRecord">
                                                                                                            <AlternatingItemStyle BackColor="#CCFFFF" />
                                                                                                            <Columns>
                                                                                                                <asp:TemplateColumn HeaderText="Query Name" ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF" >
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton CssClass="alink" ID="lbtnQuery" Text='<%# Eval("QueryName") %>' runat="server"
                                                                                                                            CommandArgument='<%# Eval("QueryID") %>' OnClick="lbtnQuery_Click"></asp:LinkButton>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateColumn>
                                                                                                                <%--<asp:BoundColumn DataField="Priority" HeaderText="Priority" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />--%>
                                                                                                                <asp:BoundColumn DataField="Total" HeaderText="Total" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" ItemStyle-BackColor="LightGray" ItemStyle-Font-Bold="true"/>
                                                                                                                <asp:BoundColumn DataField="Available" HeaderText="Avail." ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Dials" HeaderText="Dials" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Talks" HeaderText="Talks" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="AnsweringMachine" HeaderText="Machine" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="NoAnswer" HeaderText="NoAns" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Busy" HeaderText="Busies" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="OpInt" HeaderText="OPI's" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Drops" HeaderText="Drops" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Failed" HeaderText="Failed" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:TemplateColumn HeaderText="Action"  ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF" >
                                                                                                                    <ItemStyle />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton ID="lbtnStandby" Text="Standby" runat="server" CssClass="alink" CommandArgument='<%# Eval("CampaignQueryID") %>'
                                                                                                                            OnClick="lbtnStandby_Click" ></asp:LinkButton>&nbsp;
                                                                                                                        <asp:LinkButton ID="lbtnHold" Text="Hold" runat="server" CssClass="alink" CommandArgument='<%# Eval("CampaignQueryID") %>'
                                                                                                                            OnClick="lbtnHold_Click" ></asp:LinkButton>
                                                                                                                            <asp:LinkButton ID="lbtnDelete" Text="Delete" runat="server" CssClass="alink" CommandArgument='<%# Eval("QueryID") %>'
                                                                                                                            OnClick="lbtnDelete_Click" ></asp:LinkButton>
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
                                                                                                        &nbsp;</td>
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
                                                                                    <b>StandBy</b></td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td valign="middle" align="center" width="100%">
                                                                                    <table cellspacing="0" cellpadding="1" width="100%" border="0">
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:UpdatePanel ID="updStandByQuery" runat="server" UpdateMode="conditional">
                                                                                                    <ContentTemplate>
                                                                                                        <asp:Label ID="lblNoStanByQueries" Text="There are currently no standby queries." runat="server" Visible="false" />
                                                                                                        <asp:DataGrid ID="grdStandbyQueries" runat="server" HeaderStyle-BackColor="#333366" HeaderStyle-ForeColor="#FFFFFF" GridLines="Both" BorderColor="DarkGray" CellPadding="5" 
                                                                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" EditItemStyle-BackColor="#CCFF66"  
                                                                                                            AllowSorting="false" AutoGenerateColumns="false"
                                                                                                            OnItemDataBound="grdStandbyQueries_DataBind" 
                                                                                                            OnEditCommand="grdQueries_EditCommand" OnCancelCommand="grdQueries_CancelEdit" OnUpdateCommand="grdQueries_UpdateRecord"
                                                                                                            OnDeleteCommand="grdQueries_DeleteRecord">
                                                                                                            <AlternatingItemStyle BackColor="#CCFFFF" />
                                                                                                            <Columns>
                                                                                                                <asp:TemplateColumn HeaderText="Name" ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF" >
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton CssClass="alink" ID="lbtnQuery" Text='<%# Eval("QueryName") %>' runat="server"
                                                                                                                            CommandArgument='<%# Eval("QueryID") %>' OnClick="lbtnQuery_Click"></asp:LinkButton>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateColumn>
                                                                                                                
                                                                                                                <%--<asp:EditCommandColumn EditText="Prioritize" ItemStyle-ForeColor="#4875A5"
                                                                                                                      ButtonType="LinkButton"
                                                                                                                      UpdateText="Update" CancelText="Cancel" ItemStyle-Width="6%"  />--%>
                                                                                                                <%--<asp:BoundColumn DataField="Priority" HeaderText="Priority" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />--%>
                                                                                                                <asp:BoundColumn DataField="Total" HeaderText="Total" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <%--<asp:BoundColumn DataField="Available" HeaderText="Available" ItemStyle-Width="8%" HeaderStyle-ForeColor="#FFFFFF"  />--%>
                                                                                                                <asp:BoundColumn DataField="Dials" HeaderText="Dials" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Talks" HeaderText="Talks" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="AnsweringMachine" HeaderText="Machine" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="NoAnswer" HeaderText="NoAns" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Busy" HeaderText="Busies" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="OpInt" HeaderText="OPI's" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Drops" HeaderText="Drops" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Failed" HeaderText="Failed" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:TemplateColumn HeaderText="Action"  ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF" >
                                                                                                                    <ItemStyle />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton ID="lbtnActivate" Text="Activate" runat="server" CssClass="alink" CommandArgument='<%# Eval("CampaignQueryID") %>'
                                                                                                                            OnClick="lbtnActivate_Click" ></asp:LinkButton>&nbsp;
                                                                                                                        <asp:LinkButton ID="lbtnHold" Text="Hold" runat="server" CssClass="alink" CommandArgument='<%# Eval("CampaignQueryID") %>'
                                                                                                                            OnClick="lbtnHold_Click" ></asp:LinkButton>
                                                                                                                            <asp:LinkButton ID="lbtnDelete" Text="Delete" runat="server" CssClass="alink" CommandArgument='<%# Eval("QueryID") %>'
                                                                                                                            OnClick="lbtnDelete_Click" ></asp:LinkButton>
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
                                                                                                &nbsp;</td>
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
                                                                                    <b>All Queries</b></td>
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
                                                                                                        <asp:Label ID="lblAllQueries" Text="There are currently no queries." runat="server" Visible="false" />
                                                                                                        <asp:DataGrid ID="grdAllQueries" runat="server" HeaderStyle-BackColor="#333366" HeaderStyle-ForeColor="#FFFFFF" GridLines="Both" BorderColor="DarkGray" CellPadding="5" 
                                                                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" EditItemStyle-BackColor="#CCFF66"  
                                                                                                            AllowSorting="false" AutoGenerateColumns="false"
                                                                                                            OnItemDataBound="grdAllQueries_DataBind" 
                                                                                                            OnEditCommand="grdQueries_EditCommand" OnCancelCommand="grdQueries_CancelEdit" OnUpdateCommand="grdQueries_UpdateRecord"
                                                                                                            OnDeleteCommand="grdQueries_DeleteRecord">
                                                                                                            <AlternatingItemStyle BackColor="#CCFFFF" />
                                                                                                            <Columns>
                                                                                                                <asp:TemplateColumn HeaderText="Name" ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF" >
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton CssClass="alink" ID="lbtnQuery" Text='<%# Eval("QueryName") %>' runat="server"
                                                                                                                            CommandArgument='<%# Eval("QueryID") %>' OnClick="lbtnQuery_Click"></asp:LinkButton>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateColumn>
                                                                                                                
                                                                                                                <%--<asp:EditCommandColumn EditText="Prioritize" ItemStyle-ForeColor="#4875A5"
                                                                                                                      ButtonType="LinkButton"
                                                                                                                      UpdateText="Update" CancelText="Cancel" ItemStyle-Width="6%"  />
                                                                                                                <asp:BoundColumn DataField="Priority" HeaderText="Priority" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF" />--%>
                                                                                                                <asp:BoundColumn DataField="Total" HeaderText="Total" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <%--<asp:BoundColumn DataField="Available" HeaderText="Available" ItemStyle-Width="8%" HeaderStyle-ForeColor="#FFFFFF"  />--%>
                                                                                                                <asp:BoundColumn DataField="Dials" HeaderText="Dials" ItemStyle-Width="6%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Talks" HeaderText="Talks" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="AnsweringMachine" HeaderText="Machine" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="NoAnswer" HeaderText="NoAns" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Busy" HeaderText="Busies" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="OpInt" HeaderText="OPI's" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Drops" HeaderText="Drops" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:BoundColumn DataField="Failed" HeaderText="Failed" ItemStyle-Width="5%" HeaderStyle-ForeColor="#FFFFFF"  />
                                                                                                                <asp:TemplateColumn HeaderText="Action"  ItemStyle-Width="20%" HeaderStyle-ForeColor="#FFFFFF" >
                                                                                                                    <ItemStyle />
                                                                                                                    <ItemTemplate>
                                                                                                                        <asp:LinkButton ID="lbtnActivate" Text="Activate" runat="server" CssClass="alink" CommandArgument='<%# Eval("CampaignQueryID") %>'
                                                                                                                            OnClick="lbtnActivate_Click" ></asp:LinkButton>&nbsp;
                                                                                                                        <asp:LinkButton ID="lbtnStandby" Text="Standby" runat="server" CssClass="alink" CommandArgument='<%# Eval("CampaignQueryID") %>'
                                                                                                                            OnClick="lbtnStandby_Click" ></asp:LinkButton>&nbsp;
                                                                                                                            <asp:LinkButton ID="lbtnDelete" Text="Delete" runat="server" CssClass="alink" CommandArgument='<%# Eval("QueryID") %>'
                                                                                                                            OnClick="lbtnDelete_Click" ></asp:LinkButton>
                                                                                                                    </ItemTemplate>
                                                                                                                </asp:TemplateColumn>
                                                                                                            </Columns>
                                                                                                        </asp:DataGrid>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td align="center">
                                                                                                        &nbsp;</td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </ContentTemplate>
                                                                                    </asp:UpdatePanel>
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
                                                            <table cellspacing="1" width="100%" cellpadding="2" border="0" id="Table4">
                                                                <tr valign="bottom">
                                                                    <td align="right" valign="bottom">
                                                                        <asp:LinkButton ID="lbtnAddQuery" runat="server" PostBackUrl="~/campaign/QueryDetailTree.aspx"><img src="../images/AddQuery.jpg" border="0" alt="" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                            ID="lbtnClose" runat="server" PostBackUrl="~/campaign/Home.aspx" CausesValidation="false"><img src="../images/Close.jpg" border="0" /></asp:LinkButton></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- Body -->
                                    <RainMaker:Footer ID="CampaignFooter" runat="server"></RainMaker:Footer>
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
