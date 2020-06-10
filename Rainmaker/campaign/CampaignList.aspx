<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CampaignList.aspx.cs" Inherits="Rainmaker.Web.campaign.CampaignList" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader2.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Campaign List</title>
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
<body onload="ShowPageMessage();">
    <form id="frmCampaignList" runat="server">
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
                                <table cellpadding="0" cellspacing="0" height="400px" border="0" width="100%">
                                    <tr>
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td valign="middle">
                                                        <table cellspacing="2" width="60%" cellpadding="2" border="0">
                                                            <tr>
                                                                <td align="right" width="35%">
                                                                    <b>Campaign Status&nbsp;:&nbsp;</b></td>
                                                                <td valign="middle">
                                                                    <asp:DropDownList ID="ddlCampaignStatus" runat="server" CssClass="dropDownList" AutoPostBack="true"
                                                                        OnSelectedIndexChanged="ddlCampaignStatus_SelectedIndexChanged">
                                                                    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        <table cellpadding="0" cellspacing="1" border="0" width="100%">
                                                            <tr>
                                                                <td align="left" width="100%">
                                                                    <table cellspacing="1" cellpadding="4" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="middle" align="center" width="100%">
                                                                                <asp:GridView runat="server" AutoGenerateColumns="False" DataKeyNames="CampaignID"
                                                                                    ID="grdCampaignList" Width="100%" CellPadding="3" CellSpacing="1" BorderWidth="0px"
                                                                                    CssClass="tablecontentBlack" OnRowDataBound="grdCampaignList_RowDataBound" 
                                                                                    BorderStyle="Solid">                                                                                    
                                                                                    <RowStyle CssClass="tableRow" />
                                                                                    <EmptyDataRowStyle CssClass="tableHdr" />
                                                                                    <EmptyDataTemplate>
                                                                                        No Campaign Found</EmptyDataTemplate>
                                                                                    <Columns>
                                                                                        <asp:TemplateField HeaderText="Campaign Name">
                                                                                            <HeaderStyle CssClass="tableHdr" Width="25%"></HeaderStyle>
                                                                                            <ItemStyle Width="25%" />
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton CssClass="alink" ID="lbtnCampaign" Text='<%# Eval("Description") %>'
                                                                                                    runat="server" CommandArgument='<%# Eval("CampaignID") %>' OnClick="lbtnCampaign_Click"></asp:LinkButton>
                                                                                            </ItemTemplate>
                                                                                        </asp:TemplateField>
                                                                                        <%--<asp:BoundField DataField="Description" HeaderText="Description" ItemStyle-Width="50%" HeaderStyle-CssClass="tableHdr" HeaderStyle-Width="50%"/>--%>
                                                                                        <asp:BoundField DataField="FundRaiserDataTracking" HeaderText="Fund Raiser Data Tracking" Visible="false"/>
                                                                                        <asp:BoundField DataField="RecordLevelCallHistory" HeaderText="Record Level Call History" Visible="false"/>
                                                                                        <asp:BoundField DataField="OnsiteTransfer" HeaderText="On-site Call Transfer" Visible="false"/>
                                                                                        <asp:TemplateField HeaderText="Created" HeaderStyle-CssClass="tableHdr" HeaderStyle-Width="15%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label runat="server" ID="lblDateCreated" Text='<%# Eval("DateCreated") %>'></asp:Label>
                                                                                            </ItemTemplate>

<HeaderStyle CssClass="tableHdr" Width="15%"></HeaderStyle>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Status" HeaderStyle-CssClass="tableHdr" HeaderStyle-Width="15%">
                                                                                            <ItemTemplate>
                                                                                                <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("StatusID") %>'></asp:Label>
                                                                                            </ItemTemplate>

<HeaderStyle CssClass="tableHdr" Width="15%"></HeaderStyle>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField HeaderText="Delete Campaign" HeaderStyle-CssClass="tableHdr" HeaderStyle-Width="10%">

<HeaderStyle CssClass="tableHdr" Width="10%"></HeaderStyle>

                                                                                            <ItemStyle Width="10%" HorizontalAlign="Center" />
                                                                                            <ItemTemplate>
                                                                                                <asp:LinkButton ID="lbtnDelete" Text="Delete" runat="server" class="button blue small" CommandArgument='<%# Convert.ToString(Eval("CampaignID")) + "," + Eval("ShortDescription") %>'
                                                                                                    CommandName='<%# Eval("StatusID") %>' OnClick="lbtnDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to mark this Campaign for deletion?');"></asp:LinkButton>
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
                                                        <asp:LinkButton ID="lbtnAddQuery" PostBackUrl="~/campaign/CreateCampaign.aspx" 
                                                            runat="server"><div class="button blue small" alt="" border="0" />Add Campaign</div></asp:LinkButton></td>
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
