<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CallDisposition.aspx.cs"
    Inherits="Rainmaker.Web.agent.CallDisposition" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self"/>
    <title>Call Disposition</title>
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
    <script language="javascript" type="text/javascript" src="../js/jquery.js"></script>
    <script language="javascript" type="text/javascript" src="../js/RainMaker.js"></script>
    


    <script language="javascript" type="text/javascript">
    
    function Close() {
    
        
        /* if(document.getElementById("hdnClose").value=="false")
        {
        
            top.window.opener.document.getElementById("hdnDispose").value="IsDispose";
            top.window.opener.document.forms[0].submit();
            window.close();           
        } */
        //alert($("#hdnClose").val());
        //alert('got here');
        //if (document.getElementById("hdnClose").value == "false")
        //{
            if (window.opener) {
                window.opener.returnValue = "IsDispose";
            }
            window.returnValue = "IsDispose";
            self.close();
            
            //top.window.opener.document.getElementById("hdnDispose").value = "IsDispose";
            //top.window.opener.document.forms[0].submit();
        
        //}
        
        
    }
    
    </script>

</head>
<body onload="ShowPageMessage();">
    <form id="form1" runat="server" defaultfocus="lbxCallDisposition">
    <asp:SqlDataSource ID="dsViews" runat="server"></asp:SqlDataSource>
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <div>
            <table cellpadding="0" cellspacing="1" border="0" width="450px" class="tdHeader">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                            <tr>
                                <td>
                                    <!-- Body -->
                                    <table cellpadding="0" cellspacing="0" height="350px" border="0" width="100%">
                                        <tr>
                                            <td width="100%" align="center">
                                                <table cellpadding="0" cellspacing="1" border="0" width="35%">
                                                    <tr>
                                                        <td width="100%" align="center">
                                                            <!-- Content Begin -->
                                                            <table cellpadding="4" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                <tr>
                                                                    <td colspan="2" valign="top" align="left">
                                                                        &nbsp;<b>Call Disposition</b>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ListBox ID="lbxCallDisposition" runat="server" CssClass="listBoxLarge"></asp:ListBox>
                                                                    </td>
                                                                    <td valign="top">
                                                                        &nbsp;<asp:RequiredFieldValidator ID="reqCallDisposition" runat="server" ControlToValidate="lbxCallDisposition"
                                                                            ErrorMessage="Please select at least one call disposition." Text="*" SetFocusOnError="true"></asp:RequiredFieldValidator>
                                                                        <asp:ValidationSummary ID="valCallDisposition" runat="server" ShowMessageBox="true"
                                                                            ShowSummary="false" />
                                                                        <asp:HiddenField ID="hdnClose" runat="server" />
                                                                        <asp:HiddenField ID="hdnUniqueKey" runat="server" />
                                                                        
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
                                                                        <asp:LinkButton ID="lbtnOk" runat="server" OnClick="lbtnOk_Click"><img src="../images/ok.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                            ID="lbtnCancel" runat="server" CausesValidation="false" OnClientClick="window.close();"><img src="../images/cancel.jpg" border="0"></asp:LinkButton>
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
