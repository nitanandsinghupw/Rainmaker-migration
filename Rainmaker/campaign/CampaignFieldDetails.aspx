<%@ Page Language="C#" AutoEventWireup="true" Codebehind="CampaignFieldDetails.aspx.cs"
    Inherits="Rainmaker.Web.campaign.CampaignFieldDetails" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - CampaignFields Detail</title>
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
    
    <script language="javascript" type="text/javascript" src="../js/jquery.js"></script>
    
    <script language="javascript" type="text/javascript">
    
      function Validate()
      {
          if(document.getElementById('txtFieldname').value != "")
          {
            if(document.getElementById('txtFieldname').value.substring(0,1) <= "9")
            {
                alert("Field names must begin with a letter.");
                return false;
            }
            else if(document.getElementById('ddlfieldtype').value == "1")
            { 
                ValidatorEnable(document.getElementById('reqtxtLength'),true);
                ValidatorEnable(document.getElementById('cmpnumericlength'),true);
                return true;
            }
          }
          return true;            
      }
      
      function ShowFieldlength() {
          
          document.getElementById('txtLength').value = "";
          document.getElementById('txtLength').style.display = "none";
          document.getElementById('lblfieldlength').style.display = "none";
          
          if ((document.getElementById('ddlfieldtype').value) != "1" && document.getElementById('ddlfieldtype').value != "7") { 
            ValidatorEnable(document.getElementById('reqtxtLength'),false);
            ValidatorEnable(document.getElementById('cmpnumericlength'),false);
        }
        else {

            document.getElementById('lblfieldlength').style.display = "";
            $("#txtLength").removeAttr("disabled");
            document.getElementById('txtLength').style.display = "";
            
            //            ValidatorEnable(document.getElementById('reqtxtLength'),true);
            //            ValidatorEnable(document.getElementById('cmpnumericlength'),true);

        }
        if (document.getElementById('ddlfieldtype').value == "7") {

            document.getElementById('lblfieldlength').style.display = "";
            document.getElementById('txtLength').value = "1024";
            $("#txtLength").attr("disabled", true);
           
        }
      }
      
      function CheckKeyWords(sender,eArgs)
      {
	    var fieldName = trimData(eArgs.Value.toLowerCase());
	    
	    if(sender == 'undefined')
	    {
	        alert("No Object is there");
	        return false; 
	    }
	    var keywordList  = new Array("ADD","ALL","ALTER","AND","ANY","AS","ASC","AUTHORIZATION","BACKUP",
	        "BEGIN","BETWEEN","BREAK","BROWSE","BULK","BY","CASCADE","CASE","CHECK","CHECKPOINT","CLOSE",
	        "CLUSTERED","COALESCE","COLLATE","COLUMN","COMMIT","COMPUTE","CONSTRAINT","CONTAINS","CONTAINSTABLE",
	        "CONTINUE","CONVERT","CREATE","CROSS","CURRENT","CURRENT_DATE","CURRENT_TIME","CURRENT_TIMESTAMP",
	        "CURRENT_USER","CURSOR","DATABASE","DBCC","DEALLOCATE","DECLARE","DEFAULT","DELETE","DENY","DESC",
	        "DISK","DISTINCT","DISTRIBUTED","DOUBLE","DROP","DUMMY","DUMP","ELSE","END","ERRLVL","ESCAPE","EXCEPT",
	        "EXEC","EXECUTE","EXISTS","EXIT","FETCH","FILE","FILLFACTOR","FOR","FOREIGN","FREETEXT","FREETEXTTABLE",
	        "FROM","FULL","FUNCTION","GOTO","GRANT","GROUP","HAVING","HOLDLOCK","IDENTITY","IDENTITY_INSERT",
	        "IDENTITYCOL","IF","IN","INDEX","INNER","INSERT","INTERSECT","INTO","IS","JOIN","KEY","KILL",
	        "LEFT","LIKE","LINENO","LOAD","NATIONAL","NOCHECK","NONCLUSTERED","NOT","NULL","NULLIF","OF",
	        "OFF","OFFSETS","ON","OPEN","OPENDATASOURCE","OPENQUERY","OPENROWSET","OPENXML","OPTION","OR",
	        "ORDER","OUTER","OVER","PERCENT","PLAN","PRECISION","PRIMARY","PRINT","PROC","PROCEDURE","PUBLIC",
	        "RAISERROR","READ","READTEXT","RECONFIGURE","REFERENCES","REPLICATION","RESTORE","RESTRICT","RETURN",
	        "REVOKE","RIGHT","ROLLBACK","ROWCOUNT","ROWGUIDCOL","RULE","SAVE","SCHEMA","SELECT","SESSION_USER",
	        "SET","SETUSER","SHUTDOWN","SOME","STATISTICS","SYSTEM_USER","TABLE","TEXTSIZE","THEN","TO","TOP",
	        "TRAN","TRANSACTION","TRIGGER","TRUNCATE","TSEQUAL","UNION","UNIQUE","UPDATE","UPDATETEXT","USE",
	        "USER","VALUES","VARYING","VIEW","WAITFOR","WHEN","WHERE","WHILE","WITH","WRITETEXT");
	        
	    for(i=0;i<keywordList.length;i++){
            if(keywordList[i].toLowerCase() == fieldName)
            {
                eArgs.IsValid = false;
                return false;
            }
		}
	    eArgs.IsValid = true;
	    return true;
      }
    </script>

</head>
<body onload="ShowPageMessage();">
    <form id="frmCampaignFieldDetail" runat="server" defaultbutton="lbtnSave" defaultfocus="txtFieldname">
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
                                <table cellpadding="0" cellspacing="0" height="200px" border="0" width="100%">
                                    <tr>
                                        <td valign="top" align="center">
                                            <table cellpadding="0" cellspacing="2" border="0" width="100%">
                                                <tr>
                                                    <td valign="top">
                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td align="left" width="100%" colspan="2">
                                                                    <table cellpadding="2" cellspacing="1" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="Top" width="35%" align="left">
                                                                                <a href="Home.aspx" class="aHome" runat="server" id="anchHome">Campaign</a>&nbsp;&nbsp;<img
                                                                                    src="../images/arrowright.gif">&nbsp;&nbsp;<b>Campaignfield Detail</b>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                        <tr>
                                                                            <td>
                                                                                <table cellpadding="5" cellspacing="0" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td align="right" width="40%" nowrap>
                                                                                            <b>Field Name&nbsp;:&nbsp;</b></td>
                                                                                        <td align="left" width="60%" nowrap>
                                                                                            &nbsp;<asp:TextBox ID="txtFieldname" runat="server" CssClass="txtnormal" MaxLength="50"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                                                                                ID="reqtxtFieldname" runat="server" SetFocusOnError="true" ControlToValidate="txtFieldname"
                                                                                                Text="*" ErrorMessage="Please enter a field name." Display="static"></asp:RequiredFieldValidator>
                                                                                            <asp:RegularExpressionValidator ID="regFieldname" runat="Server" SetFocusOnError="true"
                                                                                                ControlToValidate="txtFieldname" ValidationExpression="^[0-9a-zA-Z_]+$" Text="*"
                                                                                                ErrorMessage="Field names cannot contain punctuation or spaces, try using letters and numbers only."></asp:RegularExpressionValidator>
                                                                                            <asp:CustomValidator ID="custFieldName" runat="server" ControlToValidate="txtFieldname" Text="*"
                                                                                                ClientValidationFunction="CheckKeyWords" ErrorMessage="This is SQL Server reserved keyword, Please enter a different field name."
                                                                                                SetFocusOnError="true"></asp:CustomValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="right" nowrap>
                                                                                            <b>Field Type&nbsp;:&nbsp;</b></td>
                                                                                        <td align="left" width="60%">
                                                                                            &nbsp;<asp:DropDownList ID="ddlfieldtype" CausesValidation="false" onchange="ShowFieldlength();"
                                                                                                CssClass="dropDownList" runat="server">
                                                                                            </asp:DropDownList>&nbsp;<asp:CompareValidator ID="cmpFT" runat="server" SetFocusOnError="true"
                                                                                                ControlToValidate="ddlfieldtype" ErrorMessage="Please select a field type." ValueToCompare="0"
                                                                                                Operator="GreaterThan" Type="Integer" Text="*" Display="static"></asp:CompareValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <%--<asp:Panel ID="pnllength" runat="server" Visible="false" >--%>
                                                                                        <td align="right" nowrap>
                                                                                            <b>
                                                                                                <asp:Label ID="lblfieldlength" runat="server" Style="display: none;">Field Length&nbsp;:&nbsp;</asp:Label></b></td>
                                                                                        <td align="left" width="60%">
                                                                                            &nbsp;<asp:TextBox ID="txtLength" MaxLength="3" runat="server" CssClass="txtsmall"
                                                                                                Style="display: none;"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator ID="reqtxtLength"
                                                                                                    runat="server" SetFocusOnError="true" ControlToValidate="txtLength" Text="*"
                                                                                                    ErrorMessage="Please enter field length" Display="static" Enabled="false"></asp:RequiredFieldValidator><asp:CompareValidator
                                                                                                        ID="cmpnumericlength" runat="server" SetFocusOnError="true" ControlToValidate="txtLength"
                                                                                                        Type="Integer" Operator="DataTypeCheck" Text="*" ErrorMessage="Please enter numeric value"
                                                                                                        Enabled="false"></asp:CompareValidator>&nbsp;
                                                                                        </td>
                                                                                        <%--</asp:Panel>--%>
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
                                        </td>
                                    </tr>
                                    <tr valign="bottom">
                                        <td align="right" valign="bottom">
                                            <table cellspacing="1" width="100%" cellpadding="4" border="0">
                                                <tr valign="bottom">
                                                    <td align="right" colspan="2">
                                                        <asp:LinkButton ID="lbtnSave" OnClick="lbtnSave_Click" OnClientClick="return Validate()"
                                                            runat="server"><img src="../images/save.jpg" border="0"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                ID="lbtnCancel" OnClick="lbtnCancel_Click" runat="server" PostBackUrl="~/campaign/CampaignFieldDetails.aspx"
                                                                CausesValidation="false" OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"><img src="../images/cancel.jpg" border="0"></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                    ID="lbtnClose" PostBackUrl="~/campaign/CampaignFieldsList.aspx" CausesValidation="false"
                                                                    runat="server"><img src="../images/close.jpg" border="0" /></asp:LinkButton>
                                                        <asp:ValidationSummary ID="valsum" runat="server" ShowMessageBox="true" ShowSummary="false" />
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

    <script language="javascript" type="text/javascript">
        ShowFieldlength();
    </script>

</body>
</html>
