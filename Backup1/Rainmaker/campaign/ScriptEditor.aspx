<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScriptEditor.aspx.cs" ValidateRequest="false"
    Inherits="Rainmaker.Web.campaign.ScriptEditor" %>

<%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Script Editor</title>
    <%--
    DRP 2012-04-23
    <style>
			#loading {Z-INDEX: 1000000000; 
				BACKGROUND-IMAGE: url(../images/greyout.png); 
				VISIBILITY: hidden; 
				BACKGROUND-REPEAT: repeat-x; 
				POSITION: absolute;
				filter: alpha(opacity=60);	
				opacity: 0.6;			
			}
    </style>
     --%>

    <script language="javascript" type="text/javascript">
    
    //fix for IE missing the indexOf function call
    if (!Array.prototype.indexOf) {
        Array.prototype.indexOf = function (searchElement /*, fromIndex */ ) {
            "use strict";
            if (this == null) {
                throw new TypeError();
            }
            var t = Object(this);
            var len = t.length >>> 0;
            if (len === 0) {
                return -1;
            }
            var n = 0;
            if (arguments.length > 1) {
                n = Number(arguments[1]);
                if (n != n) { // shortcut for verifying if it's NaN
                    n = 0;
                } else if (n != 0 && n != Infinity && n != -Infinity) {
                    n = (n > 0 || -1) * Math.floor(Math.abs(n));
                }
            }
            if (n >= len) {
                return -1;
            }
            var k = n >= 0 ? n : Math.max(len - Math.abs(n), 0);
            for (; k < len; k++) {
                if (k in t && t[k] === searchElement) {
                    return k;
                }
            }
            return -1;
        }
    }  //end of fix IndexOf
    
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


    <script type="text/javascript" src="../js/jquery.js"></script>

    

    <script language="javascript" type="text/javascript">

	    function CheckFileds(){
	        return true;
		    //alert(window.document.getElementById('hdnScriptBody').value)
		    window.frames['iframeScriptEditor'].window.document.getElementById('divHtml').innerHTML = window.document.getElementById('hdnScriptBody').value;
		    var isCheckPass = window.frames['iframeScriptEditor'].window.CheckFileds();	
		    return isCheckPass;
	    }
	    
	    function ScriptCloneConfirm(){
	         
	        if(confirm("If you copy the script, fields used in the script must match exactly with the destination \ncampaign fields or errors will occur. Do you want to save the script now?"))
	        {
//	            return GetInnerHTML();
                return true; 
	        }
	        return false;
	    }

    </script>
    
</head>
<body onload="ShowPageMessage();hideLoading();">
    <div id="loading">
    </div>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>

    <div>
        <table cellpadding="0" style="overflow: visible" cellspacing="1" border="0" width="992px"
            class="tdHeader">
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
                                        <td valign="top">
                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                <tr>
                                                    <td valign="top">
                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td align="left" width="100%">
                                                                    <table cellpadding="4" cellspacing="0" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="middle" width="35%" align="left">
                                                                                <a href="Home.aspx" class="aHome">
                                                                                    <asp:Label ID="lblCampaignName" runat="server" Text=""></asp:Label>
                                                                                </a>&nbsp;&nbsp;<img src="../images/arrowright.gif">&nbsp;&nbsp;<b>Script Editor</b>
                                                                                <asp:Literal ID="ltrlPageHeader" runat="server"></asp:Literal>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="left" width="100%" valign="top">
                                                                    <table cellpadding="3" cellspacing="1" border="0" width="100%">
                                                                        <tr>
                                                                            <td align="left" width="100%">
                                                                                <table cellpadding="2" cellspacing="0" width="100%" border="0">
                                                                                    <asp:Literal ID="ltrlScriptName" runat="server"></asp:Literal><tr>
                                                                                        <td align="left" colspan="2">
                                                                                            <b>
                                                                                                <asp:Label ID="lblScriptname" runat="server" Text="SCRIPT NAME"></asp:Label>&nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;</b><asp:TextBox
                                                                                                    ID="txtScriptName" runat="server" Text="" MaxLength="255" CssClass="txtnormal"></asp:TextBox>
                                                                                            <asp:RequiredFieldValidator ID="reqScriptName" runat="server" ControlToValidate="txtScriptName"
                                                                                                ErrorMessage="Please enter script name" Display="Dynamic" SetFocusOnError="true">*</asp:RequiredFieldValidator>
                                                                                            <asp:ValidationSummary ID="valsumScript" runat="server" ShowMessageBox="true" ShowSummary="false" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td height="4px" colspan="2">
                                                                                            <img src="../images/spacer.gif" border="0" height="4px" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td valign="middle" align="left" colspan="2">
                                                                                            <%--<b>SCRIPT HEADER</b>--%><asp:HiddenField ID="hdnScriptHeader" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                    <%--<tr>
                                                                                            <td colspan="2">
                                                                                                <asp:Literal ID="ltrlCampaignFldScript" runat="server"></asp:Literal>
                                                                                                <script type="text/javascript">
                                                                                                    <!--
                                                                                                    
                                                                                                    var sBasePath1 = document.location.href.substring(0,document.location.href.lastIndexOf('campaign')) ;
                                                                                                    //alert(sBasePath);
                                                                                                    sBasePath1 = sBasePath1 + 'fckeditor/';
                                                                                                    var oFCKeditor1 = new FCKeditor( 'FCKeditor1' ) ;
                                                                                                    oFCKeditor1.BasePath	= sBasePath1 ;//                                                                                                            
                                                                                                    oFCKeditor1.Height	= 220 ;
                                                                                                    oFCKeditor1.Width = 900 ;
                                                                                                    oFCKeditor1.Config["EnterMode"] = "br" ;
                                                                                                    oFCKeditor1.Value	= document.getElementById('hdnScriptHeader').value ;
                                                                                                    oFCKeditor1.Create() ;
                                                                                                    //-->
                                                                                                </script>

                                                                                            </td>
                                                                                        </tr>--%>
                                                                                    <tr>
                                                                                        <td valign="middle" align="left" colspan="2">
                                                                                            <b>SCRIPT</b>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td colspan="2">
                                                                                            <asp:HiddenField ID="hdnScriptBody" runat="server" />
                                                                                            <asp:Literal ID="ltrlCampaignFldScript" runat="server"></asp:Literal>
                                                                                            <CKEditor:CKEditorControl ID="CKEditor1" Height="680" Width="900" EnterMode="BR"
                                                                                                runat="server" HtmlEncodeOutput="true"></CKEditor:CKEditorControl>
                                                                                            <%--
                                                                                            <script type="text/javascript">
                                                                                                var sBasePath = document.location.href.substring(0,document.location.href.lastIndexOf('campaign')) ;
                                                                                                sBasePath = sBasePath + 'fckeditor/';

                                                                                                var oFCKeditor = new FCKeditor( 'FCKeditor3' ) ;
                                                                                                oFCKeditor.BasePath	= sBasePath ;
                                                                                                oFCKeditor.Height	= 680;
                                                                                                oFCKeditor.Width=900;
                                                                                                oFCKeditor.Config["EnterMode"] = "br" ;
                                                                                                oFCKeditor.Value = document.getElementById('hdnScriptBody').value ;
                                                                                                oFCKeditor.Create() ;
                                                                                                    
	                                                                                            // CAlled when the script is saved
	                                                                                            function GetInnerHTML()
		                                                                                            {
		                                                                                            //alert('InnerHTML get fired.');
		                                                                                            if(trimData(document.getElementById('txtScriptName').value) != "")
			                                                                                            {
			                                                                                            displayLoading();
			                                                                                            }                                                                                                    
		                                                                                            // Get the editor instance that we want to interact with.
		                                                                                            var oEditor3 = FCKeditorAPI.GetInstance('FCKeditor3') ;
		                                                                                            document.getElementById('hdnScriptBody').value =	oEditor3.EditorDocument.body.innerHTML;
		                                                                                            return CheckFileds();
		                                                                                            }
                                                                                            </script>
                                                                                            --%>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="bottom">
                                                                    <table cellpadding="4" cellspacing="1" border="0" width="100%">
                                                                        <tr valign="bottom" align="right">
                                                                            <td align="left" width="50%">
                                                                                <asp:LinkButton ID="lbtnAddPage" runat="server" OnClick="lbtnAddPage_Click" Visible="false" Text="Script Pages" CssClass="button blue small"></asp:LinkButton>&nbsp;&nbsp;
                                                                                <asp:LinkButton ID="lbtnSaveScripAs" runat="server" OnClientClick="return ScriptCloneConfirm();"
                                                                                    OnClick="lbtnSaveScripAs_Click" Visible="false" Text="Save Script As" CssClass="button blue small"></asp:LinkButton>
                                                                            </td>
                                                                            <td align="right" width="50%">
                                                                                <%--<asp:LinkButton ID="lbtnsave" runat="server" OnClick="lbtnsave_Click"><img src="../images/save.jpg" border="0" /></asp:LinkButton>--%>
                                                                                <asp:LinkButton ID="lbtnsave" runat="server"
                                                                                    OnClick="lbtnsave_Click" Text="Save" CssClass="button blue small"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                                        ID="lbtnCancel" runat="server" CausesValidation="false" OnClick="lbtnCancel_Click"
                                                                                        OnClientClick="javascript:return confirm('Do you want to cancel the changes?');" Text="Cancel" CssClass="button blue small"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                                            ID="lbtnClose" runat="server" CausesValidation="false" OnClick="lbtnClose_Click" Text="Close" CssClass="button blue small"></asp:LinkButton>
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
                                <iframe id="iframeScriptEditor" name="iframeScriptEditor" src="ValidateScriptFields.htm"
                                    width="0" height="0" frameborder="no"></iframe>
                                <!-- Body -->
                                <iframe id="iframeProgress" name="iframeProgress" src="progressbar.html" width="0"
                                    height="0" frameborder="no"></iframe>
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
