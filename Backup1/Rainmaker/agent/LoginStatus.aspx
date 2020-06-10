<%@ Page Language="C#" AutoEventWireup="true" Codebehind="LoginStatus.aspx.cs" Inherits="Rainmaker.Web.agent.LoginStatus" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Login Status</title>
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
  
     <script language="javascript" type="text/javascript">
         {
             document.write('<link rel="stylesheet" href="../css/menu_style.css" type="text/css" />');
         }
     
    </script>
</head>
<body onload="ShowPageMessage();">
    <form id="form1" runat="server" defaultbutton="lbtnOk" defaultfocus="lbxLoginStatus">
        
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Timer ID="Timer2" runat="server" ontick="Timer2_Tick">
        </asp:Timer>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <div border="0" width="992px" class="tdHeader" style="background-color:#FFFFFF">
            <div border="0" class="tdWhite" width="100%">
                <!-- Header -->
                <%--<RainMaker:Header ID="campaignHeader" runat="server"></RainMaker:Header>--%>
                <div width="100%">
                    <img src="../images/LeadSweepBannerLarge.jpg" /> 
                </div>
                <!-- Header -->
                <!-- Body -->
                <div style="width:100%" class="tdHeader" height="1px"><img src="../images/spacer.gif" height="1px" alt="spacer gif" /></div>
                <div height="30px"><img src="../images/spacer.gif" height="30px" alt="spacer gif" /></div>
                <div height="100px"><img src="../images/spacer.gif" height="100px" alt="spacer gif" /></div>
                <div height="400px" border="0" width="100%">                  
                    <table cellpadding="0" cellspacing="0" border="0" class="tdHeader" style="background-color:#FFFFFF">
                        <tr>
                            <td>
                                <table cellpadding="2" cellspacing="0" border="0" class="tdWhite" style="margin:auto">
                                    <tr>
                                        <td align="left" colspan="2">
                                            <b>Select An Available Option</b>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 5px;">
                                            &nbsp;</td>
                                        <td align="left" valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0" align="left" class="tableMainWhiteBG">
                                                <tr>
                                                    <td align="left" valign="top">
                                                        <asp:ListBox ID="lbxLoginStatus" runat="server" CssClass="listBoxLarge"></asp:ListBox>
                                                    </td>
                                                    <td valign="top">
                                                        <asp:RequiredFieldValidator ID="reqLoginStatus" runat="server" ControlToValidate="lbxLoginStatus"
                                                            ErrorMessage="Please select desired option" Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                        <asp:ValidationSummary ID="valCampaigns" runat="server" ShowMessageBox="true" ShowSummary="false" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    
                                                    <td colspan="2" align="left" style="padding-top:20px">
                                                        <asp:LinkButton ID="lbtnOk"  runat="server" OnClick="lbtnOk_Click"><div class="button green small">Ok</div></asp:LinkButton>
                                                    </td>
                                                    
                                                </tr>
                                            </table>
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
