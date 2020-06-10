<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CampaignFieldsList.aspx.cs"
    Inherits="Rainmaker.Web.campaign.CampaignFieldsList" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - CampaignFieldsList</title>

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
                                <tr valign="bottom">
                                    <td align="right" valign="bottom">
                                        <asp:LinkButton ID="LinkButton1" PostBackUrl="~/campaign/CampaignFieldDetails.aspx"
                                            runat="server" CssClass="button blue small">Add</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                ID="LinkButton2" runat="server" Text="Close" PostBackUrl="~/campaign/Home.aspx"
                                                CausesValidation="false" CssClass="button blue small">Close</asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                            <tr>
                                                <td valign="top">
                                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                        <tr>
                                                            <td align="left" width="100%" colspan="2">
                                                                <table cellpadding="4" cellspacing="1" width="100%" border="0">
                                                                    <tr>
                                                                        <td valign="middle" width="35%" align="left">
                                                                            <a href="Home.aspx" class="aHome" runat="server" id="anchHome">Campaign</a>&nbsp;&nbsp;<img
                                                                                src="../images/arrowright.gif">&nbsp;&nbsp;<b>Campaign Fields</b>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" width="100%">
                                                                <table cellpadding="2" cellspacing="0" width="100%" border="0" class="tdheader">
                                                                    <tr>
                                                                        <td valign="middle" align="center" width="100%">
                                                                            <table cellspacing="0" cellpadding="1" width="100%" border="0" class="white">
                                                                                <tr>
                                                                                    <td align="center" width="100%">
                                                                                        <asp:DataGrid ID="grdCampaignFields" runat="server" HeaderStyle-BackColor="#FFFFEE"
                                                                                            GridLines="Both" CellPadding="3" CssClass="tablecontentBlack" 
                                                                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" EditItemStyle-BackColor="#CCFF66"
                                                                                            AllowSorting="true" PagerStyle-Mode="NumericPages" PagerStyle-PageButtonCount="20"
                                                                                            PageSize="15" AutoGenerateColumns="false" OnPageIndexChanged="grdCampaignFields_PageIndexChanged"
                                                                                            OnSortCommand="grdCampaignFields_SortCommand" OnItemDataBound="grdCampaignFields_DataBind"
                                                                                            OnEditCommand="grdCampaignFields_EditCommand" OnCancelCommand="grdCampaignFields_CancelEdit"
                                                                                            OnUpdateCommand="grdCampaignFields_UpdateRecord" 
                                                                                            OnDeleteCommand="grdCampaignFields_DeleteRecord" Width="100%" 
                                                                                            BorderStyle="Solid" BorderWidth="0px" CellSpacing="1" ItemStyle-CssClass='tableRow'>
                                                                                            
                                                                                            <EditItemStyle BackColor="#CCFF66"></EditItemStyle>

                                                                                            <PagerStyle Mode="NumericPages" PageButtonCount="20"></PagerStyle>

                                                                                            <Columns>
                                                                                                <asp:EditCommandColumn EditText="Edit" ButtonType="LinkButton" UpdateText="Update"
                                                                                                    CancelText="Cancel" HeaderStyle-Width="20px" ItemStyle-Width="20px" 
                                                                                                    HeaderStyle-CssClass="tableHdr" >
                                                                                                 <HeaderStyle CssClass="tableHdr" Width="20px"></HeaderStyle>

                                                                                                 <ItemStyle Width="20px"></ItemStyle>
                                                                                                </asp:EditCommandColumn>
                                                                                                <asp:ButtonColumn Text="Delete" CommandName="Delete" HeaderStyle-Width="30px" 
                                                                                                    ItemStyle-Width="30px" HeaderStyle-CssClass="tableHdr" >
<HeaderStyle CssClass="tableHdr" Width="30px"></HeaderStyle>

<ItemStyle Width="30px"></ItemStyle>
                                                                                                </asp:ButtonColumn>
                                                                                                <asp:BoundColumn DataField="FieldName" HeaderText="Field Name" HeaderStyle-CssClass="tableHdr" SortExpression="FieldName">
<HeaderStyle CssClass="tableHdr"></HeaderStyle>
                                                                                                </asp:BoundColumn>
                                                                                                <asp:TemplateColumn HeaderText="Data Type" HeaderStyle-CssClass="tableHdr" >
                                                                                                    <ItemTemplate>
                                                                                                        <%# DataBinder.Eval(Container.DataItem, "FieldTypeName") %>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:DropDownList ID="ddlTypeNames" runat="server" DataValueField="FieldTypeID" DataTextField="FieldType" CssClass="tableHdr">
                                                                                                        </asp:DropDownList>
                                                                                                    </EditItemTemplate>

<HeaderStyle CssClass="tableHdr"></HeaderStyle>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:BoundColumn DataField="Value" HeaderText="Length" HeaderStyle-CssClass="tableHdr" >
<HeaderStyle CssClass="tableHdr"></HeaderStyle>
                                                                                                </asp:BoundColumn>
                                                                                                <asp:TemplateColumn HeaderText="Read Only" HeaderStyle-CssClass="tableHdr" >
                                                                                                    <ItemTemplate>
                                                                                                        <%# DataBinder.Eval(Container.DataItem, "ReadOnly") %>
                                                                                                    </ItemTemplate>
                                                                                                    <EditItemTemplate>
                                                                                                        <asp:CheckBox ID="chkReadOnly" runat="server"></asp:CheckBox>
                                                                                                    </EditItemTemplate>

<HeaderStyle CssClass="tableHdr"></HeaderStyle>
                                                                                                </asp:TemplateColumn>
                                                                                                <asp:BoundColumn DataField="IsDefault" HeaderText="System Default" HeaderStyle-CssClass="tableHdr">
<HeaderStyle CssClass="tableHdr"></HeaderStyle>
                                                                                                </asp:BoundColumn>
                                                                                                <asp:BoundColumn DataField="FieldID" HeaderText="Database ID" HeaderStyle-CssClass="tableHdr">
<HeaderStyle CssClass="tableHdr"></HeaderStyle>
                                                                                                </asp:BoundColumn>
                                                                                            </Columns>

<HeaderStyle HorizontalAlign="Center" BackColor="#333366" Font-Bold="True"></HeaderStyle>
                                                                                        </asp:DataGrid>
                                                                                        <asp:SqlDataSource ID="dsMainGrid" runat="server"></asp:SqlDataSource>
                                                                                        <asp:SqlDataSource ID="dsIsDefault" runat="server"></asp:SqlDataSource>
                                                                                        <asp:SqlDataSource ID="dsTypeNames" runat="server"></asp:SqlDataSource>
                                                                                        <asp:SqlDataSource ID="dsControls" runat="server"></asp:SqlDataSource>
                                                                                        <asp:SqlDataSource ID="dsControlValues" runat="server"></asp:SqlDataSource>
                                                                                       
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
                                                    <asp:LinkButton ID="lbtnAdd" PostBackUrl="~/campaign/CampaignFieldDetails.aspx" runat="server" CssClass="button blue small">Add</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                        ID="lbtnClose" runat="server" Text="Close" PostBackUrl="~/campaign/Home.aspx"
                                                        CausesValidation="false" CssClass="button blue small">Close</asp:LinkButton>
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
