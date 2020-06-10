<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CampaignDeleted.aspx.cs" Inherits="Rainmaker.Web.campaign.CampaignDeleted" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader2.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Campaign List</title>
    
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

</head>

<body onload="ShowPageMessage();">
<form id="frmCampaignDeleted" runat="server">

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
              <td valign="middle"></td>
          </tr>
          <tr>
             <td valign="top">
             <table cellpadding="0" cellspacing="1" border="0" width="100%">
             <tr>
                <td align="left" width="100%">
                <table cellspacing="1" cellpadding="4" width="100%" border="0">
                <tr>
                <td style="text-align:center">
                <b>Campaigns Marked for Deletion</b>
                </td>
                </tr>
                <tr>
                   <td valign="middle" width="100%" >
                      <asp:GridView ID="GridView1" 
                      runat="server" AutoGenerateColumns="False" DataKeyNames="CampaignID" 
                      DataSourceID="SqlDataSource1" 
                      onselectedindexchanged="GridView1_SelectedIndexChanged" Width="100%" CellPadding="3" 
                           CellSpacing="1" AllowPaging="True" 
                      AllowSorting="True" CssClass="tablecontentBlack" BorderStyle="Solid" >
                      <RowStyle CssClass="tableRow"/>
                      <EmptyDataRowStyle CssClass="tableHdr" />
                      <Columns>
                          <asp:CommandField SelectText="Restore Campaign" ShowSelectButton="True" 
                              HeaderText="Restore" HeaderStyle-CssClass="tableHdr" ControlStyle-CssClass="alink">
<ControlStyle CssClass="alink"></ControlStyle>

<HeaderStyle CssClass="tableHdr"></HeaderStyle>

                          <ItemStyle Width="150px" HorizontalAlign="Center" />
                          </asp:CommandField>
                          <asp:BoundField DataField="CampaignID" HeaderText="ID" 
                              InsertVisible="False" ReadOnly="True" SortExpression="CampaignID" HeaderStyle-CssClass="tableHdr" HeaderStyle-ForeColor="#325072" >

<HeaderStyle CssClass="tableHdr" ForeColor="#325072"></HeaderStyle>

                          <ItemStyle Width="50px" HorizontalAlign="Center" />
                          </asp:BoundField>
                          <asp:BoundField DataField="Description" HeaderText="Description" 
                              SortExpression="Description" HeaderStyle-CssClass="tableHdr" HeaderStyle-ForeColor="#325072">
<HeaderStyle CssClass="tableHdr" ForeColor="#325072"></HeaderStyle>

                          <ItemStyle Width="250px" />
                          </asp:BoundField>
                          <asp:BoundField DataField="ShortDescription" HeaderText="Short Description" 
                              SortExpression="ShortDescription" HeaderStyle-CssClass="tableHdr" HeaderStyle-ForeColor="#325072">
<HeaderStyle CssClass="tableHdr" ForeColor="#325072"></HeaderStyle>

                          <ItemStyle Width="200px" HorizontalAlign="Center" />
                          </asp:BoundField>
                          <asp:BoundField DataField="DateModified" HeaderText="Date Modified" 
                              SortExpression="DateModified" HeaderStyle-CssClass="tableHdr" HeaderStyle-ForeColor="#325072">
<HeaderStyle CssClass="tableHdr" ForeColor="#325072"></HeaderStyle>

                          <ItemStyle Width="150px" />
                          </asp:BoundField>
                          <asp:TemplateField AccessibleHeaderText="Delete" FooterText="Delete" 
                              HeaderText="Remove Completely" HeaderStyle-CssClass="tableHdr">

<HeaderStyle CssClass="tableHdr"></HeaderStyle>

                              <ItemStyle Width="130px" HorizontalAlign="Center" />
                              <ItemTemplate>
                              <asp:LinkButton CssClass="button blue small" ID="btnDelete" Text="Delete" runat="server" CommandArgument='<%# Convert.ToString(Eval("CampaignID")) + "," + Eval("ShortDescription") %>'
                              OnClick="btnDelete_Click" OnClientClick="javascript:return confirm('Are you sure you want to delete this Campaign?');"></asp:LinkButton>
                              </ItemTemplate>
                              
                              
                          </asp:TemplateField>
                           </Columns>
                           <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                           <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                           <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                           <HeaderStyle BackColor="#000084" Font-Bold="True" ForeColor="White" />
                           
                       </asp:GridView>   

                       <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
                           ConnectionString="<%$ ConnectionStrings:RainmakerMasterConnectionString %>" 
                           SelectCommand="Sel_Campaign_DeletedList" SelectCommandType="StoredProcedure">
                       </asp:SqlDataSource>

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



