<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ResultCodes.aspx.cs" Inherits="Rainmaker.Web.campaign.ResultCodes" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Result Codes</title>

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

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

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
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td align="right">
                                            <asp:LinkButton ID="LinkButton1" runat="server" PostBackUrl="~/campaign/ResultCodeDetail.aspx" CssClass="button blue small">Add</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                ID="LinkButton2" runat="server" PostBackUrl="~/campaign/Home.aspx" CausesValidation="false" CssClass="button blue small">Close</asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td valign="top">
                                                        <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td align="left" width="100%">
                                                                    <table cellpadding="2" cellspacing="0" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="middle" width="35%" align="left">
                                                                                <a href="Home.aspx" class="aHome">
                                                                                    <asp:Label ID="lblCampaign" runat="server" Text=""></asp:Label>
                                                                                </a>&nbsp;&nbsp;<img src="../images/arrowright.gif">&nbsp;&nbsp;<b>Result Codes</b>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" valign="top" width="100%">
                                                                    <table cellpadding="2" cellspacing="0" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="middle" align="center" width="100%">
                                                                                <table cellspacing="0" cellpadding="1" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <asp:GridView runat="server" AutoGenerateColumns="False" DataKeyNames="ResultCodeID"
                                                                                                ID="grdResultCodes" Width="100%" CellPadding="3" CellSpacing="1" BorderWidth="0px"
                                                                                                CssClass="tablecontentBlack" OnRowDataBound="grdResultCodes_RowDataBound" 
                                                                                                OnRowDeleting="grdResultCodes_RowDeleting" BorderStyle="Solid">
                                                                                                <RowStyle CssClass="tableRow" />
                                                                                                <EmptyDataRowStyle CssClass="tableHdr" />
                                                                                                <EmptyDataTemplate>
                                                                                                    No ResultCodes Found</EmptyDataTemplate>
                                                                                                <Columns>
                                                                                                    <asp:BoundField DataField="ResultCodeID" HeaderText="ID" HeaderStyle-CssClass="tableHdr"
                                                                                                        HeaderStyle-Width="4%" >
<HeaderStyle CssClass="tableHdr" Width="4%"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:TemplateField HeaderText="Result Code" HeaderStyle-CssClass="tableHdr" HeaderStyle-Width="23%">

<HeaderStyle CssClass="tableHdr" Width="23%"></HeaderStyle>

                                                                                                        <ItemStyle Width="25%" />
                                                                                                        <ItemTemplate>
                                                                                                            <asp:LinkButton ID="lbtnStatus" Text='<%# Eval("Description") %>' runat="server"
                                                                                                                CssClass="alink" CommandArgument='<%# Eval("ResultCodeID") %>' OnClick="lbtnStatus_Click"></asp:LinkButton>
                                                                                                            <asp:HiddenField ID="hdnDeleted" runat="server" Value='<%# Eval("DateDeleted") %>' />
                                                                                                        </ItemTemplate>
                                                                                                    </asp:TemplateField>
                                                                                                    <asp:BoundField DataField="RecycleInDays" HeaderText="Recycle in Days" HeaderStyle-CssClass="tableHdr"
                                                                                                        HeaderStyle-Width="9%" >
<HeaderStyle CssClass="tableHdr" Width="9%"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Presentation" HeaderText="Presentation" HeaderStyle-CssClass="tableHdr"
                                                                                                        HeaderStyle-Width="9%" >
<HeaderStyle CssClass="tableHdr" Width="9%"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Redialable" HeaderText="Redialable" HeaderStyle-CssClass="tableHdr"
                                                                                                        HeaderStyle-Width="9%" >
<HeaderStyle CssClass="tableHdr" Width="9%"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="Lead" HeaderText="Count as Lead" HeaderStyle-CssClass="tableHdr"
                                                                                                        HeaderStyle-Width="9%" >
<HeaderStyle CssClass="tableHdr" Width="9%"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="MasterDNC" HeaderText="MasterDNC" HeaderStyle-CssClass="tableHdr"
                                                                                                        HeaderStyle-Width="9%" >
<HeaderStyle CssClass="tableHdr" Width="9%"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="NeverCall" HeaderText="Never Call" HeaderStyle-CssClass="tableHdr"
                                                                                                        HeaderStyle-Width="9%" >
<HeaderStyle CssClass="tableHdr" Width="9%"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="VerifyOnly" HeaderText="Verify Only" HeaderStyle-CssClass="tableHdr"
                                                                                                        HeaderStyle-Width="9%" >
<HeaderStyle CssClass="tableHdr" Width="9%"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:BoundField DataField="CountAsLiveContact" HeaderText="Count as Live Contact"
                                                                                                        HeaderStyle-CssClass="tableHdr" HeaderStyle-Width="9%" >
<HeaderStyle CssClass="tableHdr" Width="9%"></HeaderStyle>
                                                                                                    </asp:BoundField>
                                                                                                    <asp:CommandField HeaderText="Delete" ShowDeleteButton="True" HeaderStyle-CssClass="tableHdr" 
                                                                                                        HeaderStyle-Width="5%" ><ControlStyle CssClass="button blue small"></ControlStyle>
                                                                                                    </asp:CommandField>
                                                                                                </Columns>
                                                                                            </asp:GridView>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td width="100%">
                                                                                            <img src="../images/spacer.gif" border="0" height="4px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td valign="middle" align="left">
                                                                                            <b>
                                                                                                <%--<asp:CheckBox ID="chkDialThroughAllNum" runat="server" Text='Dial through all numbers before implementing
                                                                                                                            "Recycle in Days"' AutoPostBack="true" OnCheckedChanged="chkDialThroughAllNum_CheckedChanged" /></b>--%>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td valign="middle" align="left">
                                                                                            <b>
                                                                                                <asp:CheckBox ID="chkShowDeletedCallRC" runat="server" Text="Show Deleted Call Result Codes"
                                                                                                    AutoPostBack="true" OnCheckedChanged="chkShowDeletedCallRC_CheckedChanged" Visible="false" /></b>
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
                                                <tr>
                                                    <td>
                                                        <table cellpadding="4" cellspacing="1" border="0" width="100%">
                                                            <tr>
                                                                <td align="right">
                                                                    <asp:LinkButton ID="lbtnAdd" runat="server" PostBackUrl="~/campaign/ResultCodeDetail.aspx" CssClass="button blue small">Add</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                        ID="lbtnClose" runat="server" PostBackUrl="~/campaign/Home.aspx" CausesValidation="false" CssClass="button blue small">Close</asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
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
