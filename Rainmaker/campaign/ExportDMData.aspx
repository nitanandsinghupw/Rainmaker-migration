<%@ Page Language="C#" AutoEventWireup="true" Codebehind="ExportDMData.aspx.cs"
    Inherits="Rainmaker.Web.campaign.ExportDMData" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<base target="_self" />--%>
    <title>Export Data from View to File</title>
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

    <script language="javascript" type="text/javascript" src="../js/RainMaker.js"></script>

    <script language="javascript" type="text/javascript">
    
    function Close()
    {
        window.close();           
    }
    
    function ConfirmDelete()
    {
        var resp = confirm("You are about to delete the original records from the database.  Would you like to continue?");
        if(resp==true)
        {
            document.getElementById("hdnDeleteConfirmed").value = "True";
        }
        else
        {
            document.getElementById("hdnDeleteConfirmed").value = "False";
        }
    }
    
    function ConfirmOverwrite()
    {
        var resp = confirm("The file you selected already exists.  Overwrite?");
        if(resp==true)
        {
            document.getElementById("hdnOverwriteConfirmed").value = "True";
        }
        else
        {
            document.getElementById("hdnOverwriteConfirmed").value = "False";
        }
    }
    
    </script>

</head>
<body onload="ShowPageMessage();hideLoading();">
    <div id="loading">
    </div>
    <form id="form1" runat="server" defaultfocus="txtFileName">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <div>
            <!-- Body -->
            <table cellpadding="5" cellspacing="5" height = "150px" border="0" width="100%">
                <tr>
                    <td width="100%" align="center">
                        <table cellpadding="2" cellspacing="0" border="0" width="100%" class="tdWhite">
                            <tr>
                                <td colspan="2" valign="top" align="left">
                                    &nbsp;<b>File to Export:</b>
                                </td>
                            </tr>
                            <tr></tr>
                            <tr>
                                <td style="width: 439px" align="left">
                                    <asp:TextBox ID="txtFileName" runat="server" CssClass="txtnormal" MaxLength="150" Width="290px"></asp:TextBox>
                                    
                                </td>
                            </tr>
                            <%--<tr>
                                <td align="left" style="width: 748px">
                                    <label>
                                        <b>Select File</b></label>&nbsp;<asp:Label ID="lblFilePath" runat="server" Text=""
                                            CssClass="tdFooter"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 748px">
                                    <asp:FileUpload ID="fileUpload" runat="server" Width="98%"/>
                                    <asp:RequiredFieldValidator runat="server" ID="rqFileUpload" ControlToValidate="fileUpload"
                                        ErrorMessage="Please Select File" Display="Static">*</asp:RequiredFieldValidator>
                                </td>
                            </tr>--%>
                            <tr>
                                <td align="left" style="width: 439px">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 439px">
                                    <asp:RadioButton ID="rdoNoDelete" runat="server" Text="Retain data after exporting." GroupName="DuplicateRule"/>
                                </td>
                                <td align="left">
                                </td>   
                            </tr>
                            <tr>
                                <td align="left" style="width: 439px">
                                    <asp:RadioButton ID="rdoDelete" runat="server" Text="Delete data after exporting." GroupName="DuplicateRule"/>
                                </td>
                                <td align="left">
                                </td>
                            </tr>
                            <tr></tr>
                        </table>
                        <table cellpadding="0" cellspacing="1" border="0" width="35%">
                            <tr></tr>
                            <tr>
                                <td width="100%" align="right">
                                    <!-- Content Begin -->
                                    <table cellpadding="4" cellspacing="0" border="0" width="100%" class="tdWhite">
                                        <tr>
                                            <td>
                                                <img src="../images/spacer.gif" height="1px" width="3px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <%--<td><img src="../images/spacer.gif" height="10px" width="25px" /></td>--%>
                                            <td align="right" colspan="2" style="width: 300px">
                                                <asp:LinkButton ID="lbtnSave" runat="server" OnClick="lbtnSave_Click" OnClientClick="alert('Please wait for the Save As dialog to appear. \nThe system is working on your export.\nDepending on your export size, this may take a few minutes. \nOnly close the window after you have been prompted to save.');"><img src="../images/save.jpg" border="0" ToolTip="Click to save the current view." /></asp:LinkButton>
                                            </td>
                                            <td>
                                                <asp:LinkButton
                                                    ID="lbtnClose" runat="server" OnClientClick="window.close();"><img src="../images/close.jpg" border="0"></asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:SqlDataSource ID="dsExportData" runat="server"></asp:SqlDataSource>
                                    <asp:SqlDataSource ID="dsCallListDelWorker" runat="server"></asp:SqlDataSource>
                                    <asp:HiddenField ID="hdnDeleteConfirmed" runat="server" Value="False"></asp:HiddenField>
                                    <asp:HiddenField ID="hdnOverwriteConfirmed" runat="server" Value="False"></asp:HiddenField>
                                </td>
                            </tr>
                        </table>
                        <iframe id="iframeProgress" src="exportbar.html" width=0 height=0 frameborder=no></iframe>
                        <%--<iframe id="iframe1" src="progressbar.html" width=0 height=0 frameborder=no></iframe>--%>
                    </td>
                </tr>
            </table>
            <!-- Body -->
        </div>
    </form>
</body>
</html>
