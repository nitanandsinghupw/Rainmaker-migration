<%@ Page Language="C#" AutoEventWireup="true" Codebehind="DataPortal.aspx.cs" Inherits="Rainmaker.Web.campaign.DataPortal" %>
<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Data Management</title>
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
        function DeleteColumn()
        {
            window.showModalDialog('../campaign/DeleteDMColumn.aspx?'+ ( new Date() ).getTime(),'DeleteColumn','dialogWidth:455px;dialogHeight:355px;edge:Raised;center:Yes;resizable:No;status:No');
        }
        function AddColumn()
        {
            window.showModalDialog('../campaign/AddDMColumn.aspx?'+ ( new Date() ).getTime(),'AddColumn','dialogWidth:455px;dialogHeight:355px;edge:Raised;center:Yes;resizable:No;status:No');
        }
        function SaveView()
        {
            window.showModalDialog('../campaign/SaveDMView.aspx?'+ ( new Date() ).getTime(),'SaveView','dialogWidth:455px;dialogHeight:155px;edge:Raised;center:Yes;resizable:No;status:No');
        }
        function ExportData()
        {
//            window.showModalDialog('../campaign/ExportDMData.aspx?'+ ( new Date() ).getTime(),'ExportData','dialogWidth:355px;dialogHeight:175px;edge:Raised;center:Yes;resizable:No;status:No');
              window.open('../campaign/ExportDMData.aspx?'+ ( new Date() ).getTime(),'ExportData','directories=0,Width=410px,Height=195px,resizable=0,status=1,toolbar=0,top=300px,left=300px');
        }
    </script>
</head>
<body onload="ShowPageMessage();">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <div>
            <table cellpadding="0" cellspacing="1" border="0" width="100%" class="tdHeader">
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
                                        <tr><td></td></tr>
                                        <tr>
                                            <td valign="middle" width="35%" align="left">
                                                &nbsp;<a href="Home.aspx" class="aHome" runat="server" id="anchHome">Campaign</a>&nbsp;&nbsp;<img
                                                    src="../images/arrowright.gif">&nbsp;&nbsp;<b>Data Manager</b>
                                                <asp:HiddenField ID="hdnValidate" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table cellspacing="2" cellpadding="2" border="0">
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblCampaign" runat="server">Campaign</asp:Label>
                                                        </td>
                                                        <td>
                                                            <img src="../images/spacer.gif" height="1px" width="3px" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlCampaign" runat="server" CssClass="dropDownList" OnSelectedIndexChanged="ddlCampaigns_Change" AutoPostBack="true" ToolTip="Select a campaign."></asp:DropDownList>
                                                        </td>
                                                        <td></td>
                                                        
                                                        <td></td>
                                                        <td>
                                                            <img src="../images/spacer.gif" height="10px" width="100px" />
                                                        </td>
                                                        <td>  
                                                        </td>
                                                        <td align="left">
                                                            <asp:LinkButton ID="btnAddColumn" Text="Add Column" runat="server" Enabled="false" CssClass="aScoreBoard" ToolTip="Add columns from the data view."/>
                                                            <asp:LinkButton ID="btnDeleteColumn" Text="Delete Column" runat="server" Enabled="false" CssClass="aScoreBoard" ToolTip="Delete columns from the data view."/>
                                                            <%--<a href="DeleteDMColumn.aspx" class="aScoreBoard">Delete Column</a>
                                                            <img src="../images/spacer.gif" height="10px" width="1px" />--%>
                                                            
                                                        </td>
                                                        <td>
                                                            <asp:LinkButton ID="btnExport" Text="Export Data" runat="server" Enabled="false" CssClass="aScoreBoard" ToolTip="Save the data viewed to a file."/>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblQuery" runat="server">Query</asp:Label>
                                                        </td>
                                                        <td>
                                                            <img src="../images/spacer.gif" height="1px" width="1px" />
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlQuery" runat="server" CssClass="dropDownList" OnSelectedIndexChanged="ddlQuery_Change" AutoPostBack="true" ToolTip="Select a query to view."></asp:DropDownList>
                                                        </td>
                                                        <td align="left">
                                                            <asp:LinkButton ID="btnNewQuery" Text="New" runat="server" Enabled="false" CssClass="aScoreBoard" PostBackUrl="~/campaign/QueryDetailTree.aspx?DataManager=True" ToolTip="Create a new query."/>
                                                            <%--<asp:LinkButton ID="lbtnAddQuery" runat="server" PostBackUrl="~/campaign/queryDetail.aspx"><img src="../images/AddQuery.jpg" border="0" alt="" /></asp:LinkButton>--%>
                                                        </td>
                                                        <td>
                                                            <img src="../images/spacer.gif" height="10px" width="20px" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lblRecsPerPage" runat="server">Records Per Page</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRecPerPage" runat="server" CssClass="dropDownList" Width="55" OnSelectedIndexChanged="ddlRecPerPage_Change" AutoPostBack="true" ToolTip="Select the number of recrods shown per page."></asp:DropDownList>
                                                        </td>
                                                        <td align="right">
                                                            <asp:Label ID="lblViews" runat="server">Data View</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlViews" runat="server" CssClass="dropDownList" OnSelectedIndexChanged="ddlView_Change" AutoPostBack="true" ToolTip="Select a view configuration previously saved."></asp:DropDownList>
                                                        </td>
                                                        <td align="left">
                                                            <asp:LinkButton ID="btnSaveView" Text="Save" runat="server" Enabled="false" CssClass="aScoreBoard" ToolTip="Save the current view."/>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table>
                                                    <tr>
                                                        
                                                        <td align="left" style="width:765px">
                                                            <asp:Label ID="lblConditions" runat="server" ForeColor="#000066" Font-Size="Small"/>
                                                        </td>
                                                        <td>
                                                            <img src="../images/spacer.gif" height="1px" width="15px" />
                                                        </td>
                                                        <td align="right">
                                                            <asp:CheckBox ID="chkResultNames" runat="server" Text="Show call results as names" AutoPostBack="true" ToolTip="Show call results as text names, not numeric codes." OnCheckedChanged="chkResultNames_Change"/>
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
            <table cellpadding="5" cellspacing="5" border="0" width="100%">
                <%--<tr valign="top">
                    <td class="tdHeader" colspan="2" valign="top">
                        <img src="../images/spacer.gif" height="1px" alt="" />
                    </td>
                </tr>--%>
                <tr>    
                    <td valign="top">
                        <asp:Label ID="lblNoData" runat="server" ForeColor="#000066" Font-Size="Small"/>
                        <asp:DataGrid ID="grdDataPortal" runat="server" HeaderStyle-BackColor="#333366" 
                            HeaderStyle-ForeColor="#FFFFFF" GridLines="Both" BorderColor="DarkGray" CellPadding="5" 
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" EditItemStyle-BackColor="#CCFF66"  
                        AllowSorting="true" AllowPaging="true" PagerStyle-Mode="NumericPages" PagerStyle-PageButtonCount="20"
                        OnPageIndexChanged="grdDataPortal_PageIndexChanged" 
                            OnSortCommand="grdDataPortal_SortCommand" OnItemDataBound="grdDataPortal_DataBind" 
                        OnEditCommand="grdDataPortal_EditCommand" 
                            OnCancelCommand="grdDataPortal_CancelEdit" OnUpdateCommand="grdDataPortal_UpdateRecord"
                        OnDeleteCommand="grdDataPortal_DeleteRecord" 
                            onselectedindexchanged="grdDataPortal_SelectedIndexChanged">
                        <AlternatingItemStyle BackColor="#CCCCCC" />
                        <Columns>
                            <asp:EditCommandColumn EditText="Edit" 
                              ButtonType="LinkButton"
                              UpdateText="Update" CancelText="Cancel" HeaderStyle-Width="20px" ItemStyle-Width="20px" />
                              <asp:ButtonColumn Text="Delete" CommandName="Delete" HeaderStyle-Width="30px" ItemStyle-Width="30px"/>
                        </Columns>
                        </asp:DataGrid>
                        <asp:SqlDataSource ID="dsMainGrid" runat="server"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="dsCampaigns" runat="server"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="dsQueries" runat="server"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="dsQueryDetail" runat="server"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="dsViews" runat="server"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="dsViewDetail" runat="server"></asp:SqlDataSource>
                        <asp:SqlDataSource ID="dsCustomFields" runat="server"></asp:SqlDataSource>
                    </td>
                </tr>
                <tr>
                    <td>
                    <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                    </td>
                </tr>
            </table>
            <!-- Body -->
            <!-- Footer -->
            
            <!-- Footer -->
        </div>
    </form>
</body>
</html>
