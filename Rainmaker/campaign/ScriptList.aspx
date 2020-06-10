<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ScriptList.aspx.cs" ValidateRequest="false"
    Inherits="Rainmaker.Web.campaign.ScriptList" %>

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
                                    <table cellpadding="0" cellspacing="0" height="400px" border="0" width="100%">
                                        <tr>
                                            <td valign="top">
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
                                                                    <td align="left" width="100%">
                                                                        <table cellpadding="1" cellspacing="1" width="100%" border="0">
                                                                            <tr>
                                                                                <td valign="middle" align="center" width="100%">
                                                                                    <table cellspacing="0" cellpadding="4" width="100%" border="0">
                                                                                        <asp:Literal ID="ltrlParentScriptName" runat="server"></asp:Literal><tr>
                                                                                            <td align="center">
                                                                                                <asp:GridView runat="server" AutoGenerateColumns="False" DataKeyNames="ScriptID"
                                                                                                    ID="grdScriptList" Width="100%" CellPadding="3" CellSpacing="1" BorderWidth="0"
                                                                                                    CssClass="tablecontentBlack" OnRowDataBound="grdScriptList_rowdatabound" 
                                                                                                    EmptyDataText="No Script Found" BorderStyle="Solid">
                                                                                                    <HeaderStyle CssClass="tableHdr" />
                                                                                                    <RowStyle CssClass="tableRow" />
                                                                                                    <EmptyDataRowStyle CssClass="tableHdr" />
                                                                                                    <Columns>
                                                                                                        
                                                                                                        <asp:TemplateField HeaderText="Script Name">
                                                                                                            <ItemStyle Width="40%" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:LinkButton ID="lbtnScriptName" CssClass="alink" Text='<%# Eval("ScriptName") %>'
                                                                                                                    runat="server" CommandArgument='<%# Eval("ScriptID") %>' OnClick="lbtnScriptName_Click"></asp:LinkButton>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Script Pages">
                                                                                                            <ItemStyle Width="20%" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:LinkButton OnClick="lbtnScriptPages_Click" CommandArgument='<%# Eval("ScriptID") %>' CommandName='<%# Eval("ScriptName") %>'  ID="lbtnScriptPages" CssClass="alink" Text="Script Pages"
                                                                                                                    runat="server" ></asp:LinkButton>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Save Script As">
                                                                                                            <ItemStyle Width="20%" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:LinkButton OnClick="lbtnScriptSave_Click" CommandArgument='<%# Eval("ScriptID") %>' CommandName='<%# Eval("ScriptName") %>' ID="lbtnSaveScript" CssClass="alink" Text="Save Script As"
                                                                                                                    runat="server" ></asp:LinkButton>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Date Created">
                                                                                                            <ItemStyle Width="13%" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:Label ID="lblDateCreated" runat="server" Text='<%# Convert.ToDateTime(Eval("DateCreated")).ToString("d") %>'></asp:Label>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                        <asp:TemplateField HeaderText="Delete">
                                                                                                            <ItemStyle Width="7%" />
                                                                                                            <ItemTemplate>
                                                                                                                <asp:LinkButton ID="lbtnDelete" CssClass="alink" Text="Delete" runat="server" CommandArgument='<%# Eval("ScriptID") %>'
                                                                                                                    OnClick="lbtnDelete_Click" ></asp:LinkButton>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:TemplateField>
                                                                                                    </Columns>
                                                                                                    
                                                                                                </asp:GridView>
                                                                                                <asp:HiddenField ID="hdnScriptHeader" runat="server" />
                                                                                                    <asp:HiddenField ID="hdnScriptBody" runat="server" />
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
                                                            <asp:LinkButton ID="lbtnAddPage" Text="AddPage" runat="server" Visible="false" OnClick="lbtnAddPage_Click"><img src="" class="myButton" alt="Add" border="0" /></asp:LinkButton>
                                                            <asp:LinkButton ID="lbtnAddScript" Text="AddScript" PostBackUrl="~/campaign/ScriptEditor.aspx"
                                                                runat="server" CssClass="button blue small">Add Script</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                    ID="lbtnClose" runat="server" OnClick="lbtnClose_Click" CssClass="button blue small">Close</asp:LinkButton></td>
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
        </div>
    </form>
</body>
</html>
