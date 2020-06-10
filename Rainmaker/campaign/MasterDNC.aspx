<%@ Page Language="C#" AutoEventWireup="true" Codebehind="MasterDNC.aspx.cs" Inherits="Rainmaker.Web.campaign.MasterDNC" %>
<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Master Do Not Call Data Management</title>
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
    <script language="javascript" type="text/javascript">
        function DeleteColumn() {
            window.showModalDialog('../campaign/DeleteDMColumn.aspx?' + (new Date()).getTime(), 'DeleteColumn', 'dialogWidth:455px;dialogHeight:355px;edge:Raised;center:Yes;resizable:No;status:No');
        }
        function AddColumn() {
            window.showModalDialog('../campaign/AddDMColumn.aspx?' + (new Date()).getTime(), 'AddColumn', 'dialogWidth:455px;dialogHeight:355px;edge:Raised;center:Yes;resizable:No;status:No');
        }
        function SaveView() {
            window.showModalDialog('../campaign/SaveDMView.aspx?' + (new Date()).getTime(), 'SaveView', 'dialogWidth:455px;dialogHeight:155px;edge:Raised;center:Yes;resizable:No;status:No');
        }
        function ExportData() {
            //            window.showModalDialog('../campaign/ExportDMData.aspx?'+ ( new Date() ).getTime(),'ExportData','dialogWidth:355px;dialogHeight:175px;edge:Raised;center:Yes;resizable:No;status:No');
            window.open('../campaign/ExportDMData.aspx?' + (new Date()).getTime(), 'ExportData', 'directories=0,Width=410px,Height=195px,resizable=0,status=1,toolbar=0,top=300px,left=300px');
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
                                            <td valign="middle" width="25%" align="left">
                                                &nbsp;&nbsp;&nbsp;<img
                                                    src="../images/arrowright.gif">&nbsp;&nbsp;<b>Master Do Not Call Data Manager</b>
                                                <asp:HiddenField ID="hdnValidate" runat="server" />
                                                <asp:HiddenField ID="hdnPhone" runat="server" />
                                                <asp:HiddenField ID="hdnPhoneDelete" runat="server" />
                                                <asp:HiddenField ID="hdnAction" runat="server" />
                                            </td>
                                            <td valign="middle" width="50%" align="left">
                                            
                                                <asp:Label ID="lblMessage" runat="server" Font-Bold="True" ForeColor="#009933" 
                                                    Font-Size="Larger"></asp:Label>
                                            
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <table cellspacing="2" cellpadding="2" border="0">
                                                    <tr><td>&nbsp;</td></tr>
                                                    <tr>
                                                        
                                                        <td align="right">
                                                            <asp:Label ID="lblPhoneNumber" runat="server">Enter Phone Number</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPhoneNumber" runat="server" ></asp:TextBox>
                                                        </td>
                                                        <td>
                                                            
                                                            <asp:Button ID="btnAdd" runat="server" Text="Add" 
                                                                onclick="btnAdd_Click" />
                                                            
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                        <asp:Button ID="btnVerify" runat="server" Text="Verify" onclick="btnVerify_Click" />
                                                        </td>
                                                        <td>&nbsp;</td>
                                                        <td>
                                                        
                                                            <asp:Button ID="btnDelete" runat="server" onclick="btnDelete_Click" 
                                                                Text="Delete" />
                                                        
                                                        </td>
                                                         
                                                    </tr>
                                                    <tr>
                                                    <td align="center">or</td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:Button ID="btnImportNumbers" runat="server" Text="Import Numbers" 
                                                                onclick="btnImportNumbers_Click" />
                                                        </td>
                                                    </tr>
                                                    <tr><td>&nbsp;</td></tr>
                                                    <tr>
                                                        <td align="right">
                                                            <asp:Label ID="lblRecsPerPage" runat="server">Records Per Page</asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:DropDownList ID="ddlRecPerPage" runat="server" CssClass="dropDownList" Width="55" OnSelectedIndexChanged="ddlRecPerPage_Change" AutoPostBack="true" ToolTip="Select the number of recrods shown per page."></asp:DropDownList>
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
                
                <tr>    
                    <td valign="top">
                        <asp:Label ID="lblNoData" runat="server" ForeColor="#000066" Font-Size="Small"/>
                        <asp:DataGrid ID="grdDataPortal" runat="server" HeaderStyle-BackColor="#333366" 
                            HeaderStyle-ForeColor="#FFFFFF" BorderColor="DarkGray" CellPadding="5" 
                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" EditItemStyle-BackColor="#CCFF66"  
                        AllowSorting="True" AllowPaging="True" PagerStyle-Mode="NumericPages" PagerStyle-PageButtonCount="20"
                        OnPageIndexChanged="grdDataPortal_PageIndexChanged" 
                            OnSortCommand="grdDataPortal_SortCommand" 
                        OnDeleteCommand="grdDataPortal_DeleteRecord" ShowFooter="True" >
<EditItemStyle BackColor="#CCFF66"></EditItemStyle>

<PagerStyle Mode="NumericPages" PageButtonCount="20"></PagerStyle>

                        <AlternatingItemStyle BackColor="#CCCCCC" />
                        <Columns>
                              <asp:ButtonColumn Text="Delete" CommandName="Delete" HeaderStyle-Width="30px" 
                                ItemStyle-Width="30px">
<HeaderStyle Width="30px"></HeaderStyle>

<ItemStyle Width="30px"></ItemStyle>
                            </asp:ButtonColumn>
                        </Columns>

<HeaderStyle HorizontalAlign="Center" BackColor="#333366" Font-Bold="True" ForeColor="White"></HeaderStyle>
                        </asp:DataGrid>
                        <asp:SqlDataSource ID="dsMainGrid" runat="server"></asp:SqlDataSource>
                        
                        <asp:SqlDataSource ID="dsViews" runat="server"></asp:SqlDataSource>
                        
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