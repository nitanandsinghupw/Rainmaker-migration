<%@ Page Language="C#" AutoEventWireup="true" Codebehind="GetTransferNum.aspx.cs"
    Inherits="Rainmaker.Web.campaign.GetTransferNum" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self" />
    <title>Enter an Offsite Number for Transfer</title>
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
    <form id="form1" runat="server" defaultfocus="txtViewName">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <div>
            <!-- Body -->
            <table cellpadding="0" cellspacing="0" height = "150px" border="0" width="100%">
                <tr>
                    <td width="100%" align="center">
                        <table cellpadding="0" cellspacing="1" border="0" width="35%">
                            <tr>
                                <td colspan="2" valign="top" align="left">
                                    &nbsp;<b>Enter an Offsite Number:</b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:TextBox ID="txtTransNumber" runat="server" CssClass="txtnormal" MaxLength="50"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="1" border="0" width="35%">
                            <tr>
                                
                                <td width="100%" align="right">
                                    <!-- Content Begin -->
                                    <table cellpadding="4" cellspacing="0" border="0" width="100%" class="tdWhite">
                                        <tr>
                                            <%--<td><img src="../images/spacer.gif" height="10px" width="25px" /></td>--%>
                                            <td align="right" colspan="2" style="width: 300px">
                                                <asp:LinkButton ID="lbtnSave" runat="server" OnClick="lbtnSave_Click"><img src="../images/save.jpg" border="0" ToolTip="Click to save the current view." /></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton
                                                    ID="lbtnClose" runat="server" OnClientClick="window.close();"><img src="../images/cancel.jpg" border="0"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:SqlDataSource ID="dsViews" runat="server"></asp:SqlDataSource>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <!-- Body -->
        </div>
    </form>
</body>
</html>
