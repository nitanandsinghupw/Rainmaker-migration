<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Opening.aspx.cs" Inherits="Rainmaker.Web.agent.Opening" %>
<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Opening</title>
   
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
    <script type="text/javascript" >
    </script>
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
                                    <!-- Header -->
                                    <RainMaker:Header ID="campaignHeader" runat="server"></RainMaker:Header>
                                    <!-- Header -->
                                    <!-- Body -->
                                    <table cellpadding="0" cellspacing="0" height="400px" border="0" width="100%">
                                        <tr>
                                            <td valign="middle" align="center">
                                                <table cellpadding="0" cellspacing="0" border="0" width="38%" class="tdWhite">
                                                    <tr>
                                                        <td>
                                                            <%--<table cellpadding="0" cellspacing="0" border="1" width="100%" class="tdWhite">
                                                                <tr>
                                                                    <td width="10px;">
                                                                    </td>
                                                                    <td>--%>
                                                                        <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <b>Select Campaign</b>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="left">
                                                                                    <asp:ListBox ID="lbCampaign" runat="server" CssClass="listBoxLarge">
                                                                                        <asp:ListItem Text="Circulation Newspaper" Value="Circulation Newspaper"></asp:ListItem>
                                                                                    </asp:ListBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right">
                                                                                    <asp:LinkButton ID="lbtnOk" runat="server" PostBackUrl="~/fckeditor.js"><img src="../images/ok.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                                        ID="lbtnLogoff" runat="server"><img src="../images/logoff.jpg" border="0" /></asp:LinkButton>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    <%--</td>
                                                                </tr>
                                                            </table>--%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- Body -->
                                    <!-- Footer -->
                                     <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                                    <!-- Footer -->
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

