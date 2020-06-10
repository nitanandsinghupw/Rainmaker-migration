<%@ Page Language="C#" AutoEventWireup="true" Codebehind="TrainingList.aspx.cs" ValidateRequest="false"
    Inherits="Rainmaker.Web.campaign.TrainingList" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rainmaker Dialer - Script List</title>
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
    <script language="javascript" type="text/javascript" src="../js/Rainmaker.js"></script>
</head>
<body onload="ShowPageMessage();">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
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
                                            <td>
                                                <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td valign="top">
                                                            <table cellpadding="0" cellspacing="1" border="0" width="100%">
                                                                <tr>
                                                                    <td align="left" width="100%">
                                                                        <table cellpadding="2" cellspacing="0" width="100%" border="0">
                                                                            <tr>
                                                                                <td valign="middle" width="35%" align="left">
                                                                                    <a href="Home.aspx" class="aHome">
                                                                                        <asp:Label ID="lblCampaign" runat="server" Text=""></asp:Label>
                                                                                    </a><asp:Literal ID="ltrlPage" runat="server"></asp:Literal>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                <%--<td align="left" width="25%">
                                                                    <img src="../images/spacer.gif" height="1px" width="20px" />
                                                                </td>--%>
                                                                <td>
                                                                    <table cellspacing="0" cellpadding="0" border="0" width="100%">
                                                                        <tr>
                                                                            <td align ="left">
                                                                                <asp:RadioButton ID="rdoAll" runat="server" Text="Enable All Schemes" GroupName="ActiveSchemeRule" OnCheckedChanged="rdoActiveScheme_Change" AutoPostBack="true"/>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align ="left">
                                                                                <asp:RadioButton ID="rdoSelected" runat="server" Text="Enable Selected Scheme" GroupName="ActiveSchemeRule" OnCheckedChanged="rdoActiveScheme_Change" AutoPostBack="true"/>
                                                                            </td>
                                                                            <td align="right" width="40%">
                                                                            <asp:Label ID="lblSelectScheme" runat="server">Selected Scheme:</asp:Label>&nbsp
                                                                            <asp:DropDownList ID="ddlActiveScheme" runat="server" CssClass="dropDownList" OnSelectedIndexChanged="ddlActiveScheme_Change" AutoPostBack="true" ToolTip="Select a training scheme to active and view pages."></asp:DropDownList>&nbsp
                                                                            &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:TextBox ID="txtNewScheme" runat="server" MaxLength="255" CssClass="txtmedium" ToolTip="Enter the name for a new training scheme."></asp:TextBox>&nbsp;
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align ="left">
                                                                                <asp:RadioButton ID="rdoNone" runat="server" Text="Disable All Schemes" GroupName="ActiveSchemeRule" OnCheckedChanged="rdoActiveScheme_Change" AutoPostBack="true"/>
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:LinkButton ID="btnDeleteScheme" Text="Delete" runat="server" Enabled="true" CssClass="aScoreBoard" ToolTip="Delete the currently selected training scheme." OnClientClick="javascript:return confirm('Are you sure you want to delete this scheme?\r\nAll pages in this scheme will be deleted as well.');" OnClick="lbtnDeleteScheme_Click"></asp:LinkButton>
                                                                                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:LinkButton ID="btnNewScheme" Text="New Scheme" runat="server" Enabled="true" CssClass="aScoreBoard" ToolTip="Create a new training scheme." OnClick="lbtnNewScheme_Click"/>
                                                                            </td>
                                                                        </tr> 
                                                                    </table>
                                                                </td>
                                                                <%--<td align="left" width="25%">
                                                                    <img src="../images/spacer.gif" height="1px" width="20px" />
                                                                </td>--%>
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
            </table>
            <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                            <tr>
                                <td align="center">
                                    <asp:GridView runat="server" AutoGenerateColumns="False" DataKeyNames="TrainingPageID"
                                        ID="grdTrainingPageList" Width="100%" CellSpacing="1" BorderWidth="0"
                                        OnRowDataBound="grdTrainingPageList_rowdatabound" EmptyDataText="No Pages Found in Selected Scheme"
                                        HeaderStyle-BackColor="#333366" HeaderStyle-ForeColor="#FFFFFF" GridLines="Both" BorderColor="DarkGray" CellPadding="5" 
                                        HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true">
                                        <AlternatingRowStyle BackColor="#CCCCCC" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Page Name">
                                                <ItemStyle Width="37%" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnTrainingPageName" CssClass="alink" Text='<%# Eval("TrainingPageName") %>'
                                                        runat="server" CommandArgument='<%# Eval("TrainingPageID") %>' OnClick="lbtnTrainingPageName_Click"></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Display Time">
                                                <ItemStyle Width="21%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDisplayTime" runat="server" Text='<%# Eval("DisplayTime")  %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Date Created">
                                                <ItemStyle Width="21%" />
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDateCreated" runat="server" Text='<%# Convert.ToDateTime(Eval("DateCreated")).ToString("d") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Save Page As">
                                                <ItemStyle Width="17%" />
                                                <ItemTemplate>
                                                    <asp:LinkButton OnClick="lbtnTrainingPageSave_Click" CommandArgument='<%# Eval("TrainingPageID") %>' CommandName='<%# Eval("TrainingPageName") %>' ID="lbtnSaveTrainingPage" CssClass="alink" Text="Save Page As"
                                                        runat="server" ></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                            <asp:TemplateField HeaderText="Delete">
                                                <ItemStyle Width="21%" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnDelete" CssClass="alink" Text="Delete" runat="server" CommandArgument='<%# Eval("TrainingPageID") %>'
                                                        OnClick="lbtnDelete_Click" ></asp:LinkButton>
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
            <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                            <tr valign="bottom">
                                <td align="right" valign="bottom">
                                    <table cellspacing="1" width="100%" cellpadding="4" border="0" id="Table4">
                                        <tr valign="bottom">
                                            <td align="right" valign="bottom">
                                                <asp:LinkButton ID="lbtnAddPage" Text="AddPage" runat="server" OnClick="lbtnAddPage_Click"><img src="../images/addpage.jpg" border="0" /></asp:LinkButton>
                                                &nbsp;&nbsp;
                                                <asp:LinkButton ID="lbtnClose" runat="server" OnClick="lbtnClose_Click"><img src="../images/close.jpg" border="0" /></asp:LinkButton></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
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
