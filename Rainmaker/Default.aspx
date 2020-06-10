<%@ Page Language="C#" AutoEventWireup="true" Codebehind="Default.aspx.cs" Inherits="Rainmaker._Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Rainmaker Dialer - Login</title>
        
        <script language="javascript" type="text/javascript">
        <!--
            var cbrowser = navigator.userAgent;        
            if(cbrowser.toLowerCase().indexOf("firefox") > 0){
                document.write('<link rel="stylesheet" href="css/RainmakerNS.css" type="text/css" />');
            }else{
                document.write('<link rel="stylesheet" href="css/Rainmaker.css" type="text/css" />');
            }
        //-->    
        </script> 
        <script type="text/javascript" src="../js/jquery.js"></script>
        <script type="text/javascript" src="../js/json2.js"></script>
        <script type="text/javascript">
                       {
                document.write('<link rel="stylesheet" href="css/menu_style.css" type="text/css" />');
            }

           

                    function disablelogin() {
                   
                      $("#lbtnLogin").html("<div border=\"0\" class=\"button blue small\" title=\"Logging In\" alt=\"\" >Login</div>");
                      $("#lbtnLogin").attr("disabled", true);
                      return true;
                   
                   }
         
            
        </script>
        <script type="text/javascript" src="js/RainMaker.js"></script>
             
    </head>
    <body onload="ShowPageMessage();">
        <form id="frmLogin" runat="server" defaultfocus="txtUserName" defaultbutton="lbtnLogin" onsubmit="disablelogin();">
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <script language="javascript" type="text/javascript">
                Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
                function EndRequestHandler(sender, args)
                {
                  alert("End request event firing error is " + args.get_error());
                  if (args.get_error() != undefined)
                   {
                       var errorMessage;
                       alert("SOAP error trap firing, error code " + args.get_response().get_statusCode());
                       if (args.get_response().get_statusCode() == '12029')
                       {
                        args.set_errorHandled(true);
                       }
                       else
                       {
                           // not my error so let the default behavior happen       
                       }
                   }
                }
                
            </script>
            <div width="992px" class="tdHeader">  
                <div width="100%" class="tdWhite">
                    <!-- Header -->
                    <div width="100%"><img src="../images/LeadSweepBannerLarge.jpg"></div>
                    <div style="width:100%" class="tdHeader" height="1px"><img src="../images/spacer.gif" height="1px" alt="spacer gif" /></div>
                       
                    <!-- Header -->
                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td>

                           </td>
                        </tr>
                        <tr>
                            <td class="tdSpacer" height="1px">
                                <img src="../images/spacer.gif" height="1px"></td>
                        </tr>
                        <tr>
                            <td class="tdSetting">
                                <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                    <tr>
                                        <td width="100%" class="tdHdr">
                                            </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdSpacer" height="1px">
                                <img src="../images/spacer.gif" height="1px" alt=""></td>
                        </tr>
                    </table>
                    <!-- Header -->
                    <!-- Body -->
                    <table cellpadding="0" cellspacing="0" height="400px" border="0" width="100%">
                        <tr>
                            <td align="center">
                                <table cellpadding="2" cellspacing="2" border="0" width="60%">
                                    <tr>
                                        <br/> <br/><br/><br/>
                                        <div><h1>Leadsweeper - DEV - SOURCE Machine</h1></div>
                                        <br/> <br/><br/><br/><br/> <br/><br/><br/><br/> <br/><br/><br/>
                                        <td align="right">
                                            <b>User Name&nbsp;:&nbsp;&nbsp;</b>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtUserName" runat="server" CssClass="txtmedium"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqUserName" runat="server" SetFocusOnError="true"  ControlToValidate="txtUserName" Display="dynamic"  Text="*" ErrorMessage="Enter UserName"></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            <b>Password&nbsp;:&nbsp;&nbsp;</b>
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="txtmedium"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="reqPassword" SetFocusOnError="true"  runat="server" ControlToValidate="txtPassword" Display="dynamic"  Text="*" ErrorMessage="Enter Password"></asp:RequiredFieldValidator>
                                            <asp:HiddenField ID="hdnmessage" runat="server" />
                                            <asp:HiddenField ID="hdnexception" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                        <td align="left">
                                            <asp:LinkButton ID="lbtnLogin" OnClick="lbtnLogin_Click" runat="server" 
                                                Width="45px"><div border="0" class="button green small" title="Login" alt="" >Login</div></asp:LinkButton>&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;
                                            <asp:LinkButton ID="lbtnReset" CausesValidation="false"  runat="server" 
                                            OnClick="lbtnReset_Click" Width="45px"><div border="0" class="button red small" title="Reset" >Reset</div></asp:LinkButton>
                                            <asp:ValidationSummary ID="valsumLogin" runat="server" ShowMessageBox="true" ShowSummary="false"  />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <br/> <br/><br/><br/><br/> <br/><br/><br/><br/> <br/><br/><br/>
                    <!-- Body -->
                    <!-- Footer -->
                    <div style="width:100%" class="tdHeader" height="1px"><img src="../images/spacer.gif" height="1px" alt="spacer gif" /></div>
                    <table cellpadding="0" height="50px" cellspacing="0" border="0" width="100%">
                        <tr>
                            <td class="tdSpacer" height="1px" colspan="2">
                                <img src="../images/spacer.gif" height="1px"></td>
                        </tr>
                        <tr height="49px">
                            <td align="left" width="50%" class="tdFooter">
                                © 2011 Rainmaker Dialing Systems, inc.</td>
                            <td align="right" width="50%" class="tdFooter">
                                <img src="images/LeadSweepBannerSmall.jpg" alt="" /></td>
                        </tr>
                    </table>
                </div>            
            </div>            
        </form>
    </body>
</html>
