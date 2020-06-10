<%@ Page Language="C#" AutoEventWireup="true" Codebehind="AddDMColumn.aspx.cs"
    Inherits="Rainmaker.Web.campaign.AddDMColumn" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Add Data Manager Columns</title>
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
    
    function Close()
    {
        window.close();           
    }
    
    </script>

</head>
<body onload="ShowPageMessage();">
    <form id="form1" runat="server" defaultfocus="lbxColumns">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <div>
            <table cellpadding="0" cellspacing="1" border="0" width="450px" class="tdHeader">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                            <tr>
                                <td>
                                    <!-- Header -->
                                    <!-- Header -->
                                    <!-- Body -->
                                    <table cellpadding="0" cellspacing="0" height="350px" border="0" width="100%">
                                        <tr>
                                            <td width="100%" align="center">
                                                <table cellpadding="0" cellspacing="1" border="0" width="35%">
                                                    <tr>
                                                        <td colspan="2" valign="top" align="left">
                                                            &nbsp;<b>Edit Visible Columns</b>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <asp:ListBox ID="lbxColumns" runat="server" CssClass="listBoxXLarge"></asp:ListBox>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <table cellpadding="0" cellspacing="1" border="0" width="35%">
                                                    <tr>
                                                        <td width="100%" align="center">
                                                            <!-- Content Begin -->
                                                            <table cellpadding="4" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                       
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="lblCampaign" runat="server">Column</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlColumnName" runat="server" CssClass="dropDownList" AutoPostBack="false"></asp:DropDownList>
                                                                    </td>
                                                                    
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="Label1" runat="server">Position</asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlPosition" runat="server" CssClass="dropDownList" AutoPostBack="false"></asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table width="100%" cellpadding="2" cellspacing="2" border="0" id="tbData" runat="server">
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right" colspan="2">
                                                                        <asp:LinkButton ID="lbtnAdd" runat="server" OnClick="lbtnAdd_Click"><img src="../images/add.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                            ID="lbtnClose" runat="server" CausesValidation="false" OnClientClick="window.close();"><img src="../images/close.jpg" border="0"></asp:LinkButton>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <asp:SqlDataSource ID="dsColumns" runat="server"></asp:SqlDataSource>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- Body -->
                                    <%-- <!-- Footer -->
                                    <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                                    <!-- Footer -->--%>
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
