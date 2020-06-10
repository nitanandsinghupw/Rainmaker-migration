<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Campaigns.aspx.cs" Inherits="Rainmaker.Web.agent.Campaigns" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rainmaker Dialer - Campaigns</title>
    
    <script language="javascript" type="text/javascript">

        var cbrowser = navigator.userAgent;
        
        if(cbrowser.toLowerCase().indexOf("firefox") > 0){
            document.write('<link rel="stylesheet" href="../css/RainmakerNS.css" type="text/css" />');
        }else{
            document.write('<link rel="stylesheet" href="../css/Rainmaker.css" type="text/css" />');
        }
  
    </script> 

    <script language="javascript" type="text/javascript" src="../js/Rainmaker.js"></script>
    
    <script language="javascript" type="text/javascript" src="../js/jquery.js"></script>
    <script>
        $(document).ready(function() {
            $("#<%=lbxCampaign.ClientID%>").each(function(i, m) {
                $(this).bind("click", function() {
                    $("#<%= SelectedCampaignID.ClientID %>").val($(this).val());
                });
            });

            /*var prm = Sys.WebForms.PageRequestManager.getInstance();

            prm.add_beginRequest(function() {
                var mystring;
                var selectedcampainid = $("#<%= SelectedCampaignID.ClientID %>").val();
                //if the selectedvalue stored in hidden variable is not in listbox then set hidden value to ""
                $("#<%=lbxCampaign.ClientID%>").each(function(i, m) {
                    mystring += i;
                    mystring += " , ";
                });
                alert("PageRequestManager beginRequest hidden variable:" + selectedcampainid + " , list of indexes from listbox: " + mystring);
                
            });
            prm.add_endRequest(function() {
                $("#<%=lbxCampaign.ClientID%>").each(function(i, m) {
                    mystring += i;
                    mystring += " , ";
                });
                var selectedcampainid = $("#<%= SelectedCampaignID.ClientID %>").val();
                alert("PageRequestManager endRequest hidden variable:" + selectedcampainid + " , list of indexes from listbox: " + mystring);

            });*/
        });


    </script>
     <script language="javascript" type="text/javascript">
         {
             document.write('<link rel="stylesheet" href="../css/menu_style.css" type="text/css" />');
         }
         
              
    </script>
    

</head>
<body onload="ShowPageMessage();">
    <form id="form1" runat="server" defaultbutton="lbtnOk" defaultfocus="lbxCampaign">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server" Interval="5000"></asp:Timer>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <div width="992px" class="tdHeader">  
            <div width="100%" class="tdWhite">
                <!-- Header -->
                <div width="100%"><img src="../images/LeadSweepBannerLarge.jpg"></div>
                <div style="width:100%" class="tdHeader" height="1px"><img src="../images/spacer.gif" height="1px" alt="spacer gif" /></div>
                <div class="tdSetting">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td width="100%" class="tdHdr">
                                Login Details&nbsp;:&nbsp;<asp:Label ID="lblLoginDetails" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </div> 
                <!-- Header -->
                <div height="100px"><img src="../images/spacer.gif" height="100px" alt="spacer gif" /></div>
                <!-- Body -->
                <div height="400px" border="0">
                    <table cellpadding="0" cellspacing="0" border="0" class="tdWhite" style="margin:auto">
                        <tr>
                            <td>
                                <table cellpadding="2" cellspacing="0" border="0" width="300px">
                                    <tr>
                                        <td align="left" colspan="2">
                                            <b>Select Campaign</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top">
                                            
                                            <asp:ListBox ID="lbxCampaign" runat="server" CssClass="listBoxLarge"> 
                                                
                                                
                                                <asp:ListItem Text="No Active Campaigns" Value="0"></asp:ListItem>
                                            </asp:ListBox>&nbsp;
                                        </td>
                                        <td valign="top">
                                            <asp:RequiredFieldValidator ID="reqCampaign" runat="server" ControlToValidate="lbxCampaign"
                                                ErrorMessage="Please select a campaign" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                            <asp:ValidationSummary ID="valCampaigns" runat="server" ShowMessageBox="true" ShowSummary="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" colspan="2">
                                            <asp:LinkButton ID="lbtnOk" runat="server" OnClick="lbtnOk_Click" Width="45px"><div class="button green small">Ok</div></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;<asp:LinkButton
                                                ID="lbtnExit" runat="server" OnClick="lbtnExit_Click" 
                                                CausesValidation="false" Width="45px"><div class="button red small">Exit</div></asp:LinkButton>&nbsp;&nbsp;&nbsp;
                                        <asp:HiddenField ID="hdnLastRunCheck" runat="server" />
                                        <asp:HiddenField ID="hdnMaxNoCampaignMins" runat="server" />
                                        <asp:HiddenField ID="hdnLogoutAgent" runat="server" />
                                        <asp:HiddenField ID="SelectedCampaignID" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
               </div>
               <div height="100px"><img src="../images/spacer.gif" height="100px" alt="spacer gif" /></div>
                <!-- Body -->
               <div style="width:100%" class="tdHeader" height="1px"><img src="../images/spacer.gif" height="1px" alt="spacer gif" /></div> 
                <!-- Footer -->
                <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                <!-- Footer -->
            </div>
        </div>
    </form>
</body>
</html>
