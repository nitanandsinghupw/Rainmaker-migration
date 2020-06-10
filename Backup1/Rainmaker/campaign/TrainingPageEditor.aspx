<%@ Page Language="C#" AutoEventWireup="true" Codebehind="TrainingPageEditor.aspx.cs" ValidateRequest="false"
    Inherits="Rainmaker.Web.campaign.TrainingPageEditor" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Script Editor</title>
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

    <script type="text/javascript" src="../FCKeditor/fckeditor.js"></script>
    <script language="javascript" type="text/javascript">
        var arrRMFields = new Array();
        var arrScripts = new Array();
        var arrRMFields_b = new Array();
        var arrResultcodes = new Array();
    </script>
    
    <script type="text/javascript" src="../js/jquery.js"></script>
    <script language="javascript" type="text/javascript">

	    function CheckFileds(){
	        return true;
		    //alert(window.document.getElementById('hdnTrainingPageContent').value)
		    window.frames['iframeScriptEditor'].window.document.getElementById('divHtml').innerHTML = window.document.getElementById('hdnTrainingPageContent').value;
		    var isCheckPass = window.frames['iframeScriptEditor'].window.CheckFileds();	
		    return isCheckPass;
	    }
	    
	    function ScriptCloneConfirm(){
	         
	        if(confirm("If you copy the script, fields used in the script must match exactly with the destination \ncampaign fields or errors will occur. Do you want to save the script now?"))
	        {
	            return GetInnerHTML();
	        }
	        return false;
	    }

    </script>
</head>
<body onload="ShowPageMessage();hideLoading();">
    <div id="loading">
    </div>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
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
                                                                                    </a>&nbsp;&nbsp;<img src="../images/arrowright.gif">&nbsp;&nbsp;<b>Training Page Editor</b>
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
                                                                                                <b><asp:Label ID="lblTrainingPageName" runat="server" Text="Training Page Name:"></asp:Label>&nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;</b><asp:TextBox ID="txtTrainingPageName" runat="server" Text="" MaxLength="255" CssClass="txtnormal"></asp:TextBox>
                                                                                                <asp:RequiredFieldValidator ID="reqTrainingPageName" runat="server" ControlToValidate="txtTrainingPageName"
                                                                                                    ErrorMessage="Please enter training page name" Display="Dynamic" SetFocusOnError="true">*</asp:RequiredFieldValidator>
                                                                                                <asp:ValidationSummary ID="valsumTrainingPage" runat="server" ShowMessageBox="true" ShowSummary="false" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" colspan="2">
                                                                                                <b><asp:Label ID="lblDisplayTime" runat="server" Text="Seconds for Page to Display:"></asp:Label>&nbsp;:&nbsp;&nbsp;&nbsp;&nbsp;</b><asp:TextBox ID="txtDisplayTime" runat="server" Text="" MaxLength="5" CssClass="txtnormal"></asp:TextBox>
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
                                                                                                <b>Training Page</b></td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td colspan="2">
                                                                                                <asp:HiddenField ID="hdnTrainingPageContent" runat="server" />
                                                                                                <asp:Literal ID="ltrlCampaignFldScript" runat="server"></asp:Literal>
                                                                                                <script type="text/javascript">
                                                                                                    <!--

                                                                                                    var sBasePath = document.location.href.substring(0,document.location.href.lastIndexOf('campaign')) ;
                                                                                                    sBasePath = sBasePath + 'fckeditor/';

                                                                                                    var oFCKeditor = new FCKeditor( 'FCKeditor3' ) ;
                                                                                                    oFCKeditor.BasePath	= sBasePath ;
                                                                                                    oFCKeditor.Height	= 680;
                                                                                                    oFCKeditor.Width=900;
                                                                                                    oFCKeditor.Config["EnterMode"] = "br" ;
                                                                                                    oFCKeditor.Value = document.getElementById('hdnTrainingPageContent').value ;
                                                                                                    oFCKeditor.Create() ;
                                                                                                    //-->
                                                                                                    
                                                                                                    function GetInnerHTML()
                                                                                                    {
                                                                                                        //alert('InnerHTML get fired.');
                                                                                                        if(trimData(document.getElementById('txtTrainingPageName').value) != "")
                                                                                                        {
                                                                                                            displayLoading();
                                                                                                        }
                                                                                                    
	                                                                                                    // Get the editor instance that we want to interact with.
	                                                                                                    //var oEditor1 = FCKeditorAPI.GetInstance('FCKeditor1') ;
                                                                                                        //document.getElementById('hdnScriptHeader').value =	oEditor1.EditorDocument.body.innerHTML;
	                                                                                                    //var oEditor2 = FCKeditorAPI.GetInstance('FCKeditor2') ;
                                                                                                        //document.getElementById('hdnScriptSubHeader').value =	oEditor2.EditorDocument.body.innerHTML;
                                                                                                        var oEditor3 = FCKeditorAPI.GetInstance('FCKeditor3') ;
                                                                                                        document.getElementById('hdnTrainingPageContent').value =	oEditor3.EditorDocument.body.innerHTML;
                                                                                                        //alert('Going to checkfields');
                                                                                                        return CheckFileds();
                                                                                                    }               
                                                                                                </script>

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
                                                                                    <%--<asp:LinkButton ID="lbtnAddPage" runat="server" OnClick="lbtnAddPage_Click" Visible="false"><img src="../images/scriptpages.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;
                                                                                    <asp:LinkButton ID="lbtnSaveTrainingPageAs" runat="server" OnClientClick="return ScriptCloneConfirm();" OnClick="lbtnSaveScripAs_Click" Visible="false"><img src="../images/savescriptas.jpg" border="0" /></asp:LinkButton>--%>
                                                                                </td>
                                                                                <td align="right" width="50%">
                                                                                    <asp:LinkButton ID="lbtnSaveTrainingPage" OnClientClick="return GetInnerHTML();" runat="server" OnClick="lbtnSaveTrainingPage_Click"><img src="../images/save.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                                        ID="lbtnCancel" runat="server" CausesValidation="false" OnClick="lbtnCancel_Click"
                                                                                        OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"><img src="../images/cancel.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                                            ID="lbtnClose" runat="server" CausesValidation="false" OnClick="lbtnClose_Click"><img src="../images/Close.jpg" border="0" /></asp:LinkButton></td>
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
                                    <iframe id="iframeScriptEditor" name="iframeScriptEditor" src="ValidateScriptFields.htm" width=0 height=0 frameborder=no></iframe>
                                    <!-- Body -->
                                    <iframe id="iframeProgress" name="iframeProgress" src="progressbar.html" width=0 height=0 frameborder=no></iframe>
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
