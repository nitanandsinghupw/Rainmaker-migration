<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportStats.aspx.cs" Inherits="Rainmaker.Web.campaign.ImportStats1" %>
<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Rainmaker Dialer - Import Stats</title>
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
<body onload="ShowPageMessage()">
    <form id="frmImportStats" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                        <tr>
                            <td>
                                <RainMaker:Header ID="campaignHeader" runat="server"></RainMaker:Header>
                                <!-- Body -->
                                <table cellpadding="0" cellspacing="0" height="200px" border="0" width="100%">
                                    <tr>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td valign="top">
                                                        <table cellpadding="0" cellspacing="1" border="0" width="100%">
                                                            <tr>
                                                                <td align="left" width="100%">
                                                                    <table cellpadding="1" cellspacing="1" width="50%" border="0">
                                                                        <tr>
                                                                            <td align="left" width="100%">
                                                                                <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td valign="bottom" align="left">
                                                                                            <b>Import Statastics</b>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="middle" align="center" width="100%">
                                                                                <table cellspacing="0" cellpadding="5" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td align="right" width="10%" nowrap>
                                                                                            Total Leads in import file&nbsp;:&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblTotalLeads" runat="server" Text="0"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right" width="10%" nowrap>
                                                                                            Leads successfully imported&nbsp;:&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblImported" runat="server" Text="0"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right" width="10%" nowrap>
                                                                                            Leads successfully updated&nbsp;:&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblUpdated" runat="server" Text="0"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right" width="10%" nowrap>
                                                                                            Leads with blank phone number&nbsp;:&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblBlank" runat="server" Text="0"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right" width="10%" nowrap>
                                                                                            Leads with special characters in phone number&nbsp;:&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblSpChar" runat="server" Text="0"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right" width="10%" nowrap>
                                                                                            Leads with invalid input data&nbsp;:&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblBadData" runat="server" Text="0"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right" width="10%" nowrap>
                                                                                            Leads with disallowed number lengths&nbsp;:&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblBadLength" runat="server" Text="0"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right" width="10%" nowrap>
                                                                                            Duplicate Leads not imported&nbsp;:&nbsp;&nbsp;
                                                                                        </td>
                                                                                        <td align="left">
                                                                                            <asp:Label ID="lblDuplicate" runat="server" Text="0"></asp:Label>
                                                                                            <input type="hidden" id="hdnExportToClient" name="hdnError" runat="server" value=""/>
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
                                                        <asp:LinkButton ID="lbtnBack" runat="server" PostBackUrl="~/campaign/ImportMappings.aspx" Text="Back" CssClass="button blue small"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                            ID="lbtnUpload" runat="server" PostBackUrl="~/campaign/home.aspx" Text="Ok" CssClass="button blue small"></asp:LinkButton>&nbsp;
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
    <script language="javascript" type="text/javascript">
			CheckExport();
			function CheckExport()
			{
			    if(document.getElementById('hdnExportToClient').value != "")
			    {
			        if(confirm('Do you want to save the duplicate records'))
			         document.frmImportStats.submit();
			    }
			}
    </script>
</body>
</html>
