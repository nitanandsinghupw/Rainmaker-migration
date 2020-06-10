<%@ Page Language="C#" AutoEventWireup="true" Codebehind="QueryDetail.aspx.cs" Inherits="Rainmaker.Web.campaign.QueryDetails" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Query Detail</title>
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

    <script type="text/javascript">
  
    function ValidatorsEnabled(selectedOperator)
    {  
        var count =parseInt(document.getElementById('hdnCount').value);
        //alert('validator enabled.');
        count++;
        for(var i=2;i<=count;i++)
        {
            var controlcount="0" + i + "";
            var ddlOperator ='grdQueryConditions_ctl' + controlcount + '_ddlOperator';
            var ddlCriteria ='grdQueryConditions_ctl' + controlcount + '_ddlCriteria';
            var reqEnterValue ='grdQueryConditions_ctl' + controlcount + '_reqEnterValue';
            var cmpEnterValue ='grdQueryConditions_ctl' + controlcount + '_cmpEnterValue';
            var cmpEnterIntValue ='grdQueryConditions_ctl' + controlcount + '_cmpEnterIntValue';
            var txtEnterValue ='grdQueryConditions_ctl' + controlcount + '_txtEnterValue';
            var ddlPickByName ='grdQueryConditions_ctl' + controlcount + '_ddlPickByName';
//            var ddlSelectAgentName = 'grdQueryConditions_ctl' + controlcount + '_ddlSelectAgentName'
//            var ddlSelectResultName = 'grdQueryConditions_ctl' + controlcount + '_ddlSelectResultName'
            var isnullCondition = "{0} Is Null {1}";
            var isnotnullCondition = "{0} Is Not Null {1}";
            var eqaulsStringCondition = "{0} = '{1}'"
            var isdatetimeofCall = "DateTimeofCall:date";
            var isAgentId="AgentID:String";
            var isfullquerypassCount="FullQueryPassCount:Integer";
            var isnevercallFlag="NeverCallFlag:Integer";
            var iscallresultCode="CallResultCode:Integer";
            
            //alert("Pick By Name ref " + document.getElementById(ddlPickByName)); // Could trap by null  
            
            if(selectedOperator.id==document.getElementById(ddlOperator).id && selectedOperator!="IsOnload")
            {
                if(document.getElementById(ddlOperator).value==isnullCondition || 
                document.getElementById(ddlOperator).value==isnotnullCondition)
                {
                    document.getElementById(txtEnterValue).value = "";
                    document.getElementById(txtEnterValue).disabled = true;
                    ValidatorEnable(document.getElementById(reqEnterValue),false);
                }
                else
                {
                    // Where operator has been selected and text box for value is enabled ****
//                    alert("Crit Value " + document.getElementById(ddlCriteria).value + ", Operator " + document.getElementById(ddlOperator).value);
//                    if ( document.getElementById(ddlCriteria).value==isAgentId && document.getElementById(ddlOperator).value==eqaulsStringCondition)
//                    {
//                        document.getElementById(txtEnterValue).disabled = true;
//                        document.getElementById(txtEnterValue).visible = false;
//                        document.getElementById(ddlSelectAgentName).enabled = true;
//                        document.getElementById(ddlSelectAgentName).visible = true;
//                        ValidatorEnable(document.getElementById(reqAgentName), true);
//                    }
//                    else
//                    {
                        document.getElementById(txtEnterValue).disabled = false;
                        ValidatorEnable(document.getElementById(reqEnterValue),true);
                    
                }
            }
            else if((document.getElementById(ddlOperator).value==isnullCondition 
                        || document.getElementById(ddlOperator).value==isnotnullCondition)
                        && selectedOperator=="IsOnload" && document.getElementById(ddlPickByName).visible=="false")
            {
                document.getElementById(txtEnterValue).value = "";
                document.getElementById(txtEnterValue).disabled = "true";
                ValidatorEnable(document.getElementById(reqEnterValue),false);
               
            } 
            else if((document.getElementById(ddlOperator).value!=isnullCondition 
                       && document.getElementById(ddlOperator).value!=isnotnullCondition 
                       && document.getElementById(txtEnterValue).value == "")
                       && (selectedOperator=="EnterValue")
                       && (document.getElementById(ddlOperator).value!="0" || document.getElementById(ddlCriteria).value!="0"))
            { 
            
                ValidatorEnable(document.getElementById(reqEnterValue),true);
               
            } 
            
            if(document.getElementById(ddlCriteria).value.indexOf(":date") > 0 
                && selectedOperator=="IsOnload")
            {
                ValidatorEnable(document.getElementById(cmpEnterValue),true);
            }
        
            if((document.getElementById(ddlCriteria).value==isAgentId 
            || document.getElementById(ddlCriteria).value==isfullquerypassCount 
            || document.getElementById(ddlCriteria).value==isnevercallFlag ) 
            && selectedOperator=="IsOnload" || document.getElementById(ddlCriteria).value==iscallresultCode)
            {
                ValidatorEnable(document.getElementById(cmpEnterIntValue),true);
            }
           
            if(selectedOperator.id==document.getElementById(ddlCriteria).id
             && selectedOperator!="IsOnload")
            {
                if(document.getElementById(ddlCriteria).value.indexOf(":date") > 0)
                {
                    ValidatorEnable(document.getElementById(cmpEnterValue),true);
                    BindOperatorDropdown(ddlOperator, true);
                }
                else
                {
                    ValidatorEnable(document.getElementById(cmpEnterValue),false);
                    BindOperatorDropdown(ddlOperator, false);
                }
                
               if((document.getElementById(ddlCriteria).value==isAgentId 
               || document.getElementById(ddlCriteria).value==isfullquerypassCount 
               || document.getElementById(ddlCriteria).value==isnevercallFlag || document.getElementById(ddlCriteria).value==iscallresultCode) 
               && selectedOperator!="IsOnload")
               {
                    ValidatorEnable(document.getElementById(cmpEnterIntValue),true);
               }
              else
               {
                   ValidatorEnable(document.getElementById(cmpEnterIntValue),false);
               }
            }    
        }
    }
    
    function BindOperatorDropdown(ddlOperatorId, isdate)
    {
         var ddlOptr = document.getElementById(ddlOperatorId);
         var l = 0;
         var listLength = ddlOptr.options.length;
		 for(l=listLength-1;l>-1;l--)
         {
	        ddlOptr.options[l] = null;
         }
         
         if(isdate == true)
         {
            ddlOptr.options[0] = new Option("Select Operator", "0");
            ddlOptr.options[1] = new Option("Equals", "{0} = '{1}'");
            ddlOptr.options[2] = new Option("Greater Than", "{0} > '{1}'");
            ddlOptr.options[3] = new Option("Less than", "{0} < '{1}'");
            ddlOptr.options[4] = new Option("Greater Than Equal", "{0} >= '{1}'");
            ddlOptr.options[5] = new Option("Less than Equal", "{0} <= '{1}'");
            ddlOptr.options[6] = new Option("Is Null", "{0} Is Null {1}");
            ddlOptr.options[7] = new Option("Is Not Null", "{0} Is Not Null {1}");
            ddlOptr.options[8] = new Option("Does Not Equal", "{0} <> '{1}'");
         }
         else
         {
            ddlOptr.options[0] = new Option("Select Operator", "0");
            ddlOptr.options[1] = new Option("Equals", "{0} = '{1}'");
            ddlOptr.options[2] = new Option("Contains", "{0} LIKE '%{1}%'");
            ddlOptr.options[3] = new Option("Does Not Contain", "{0} NOT LIKE '%{1}%'");
            ddlOptr.options[4] = new Option("BeginsWith", "{0} LIKE '{1}%'");
            ddlOptr.options[5] = new Option("EndsWith", "{0} LIKE '%{1}'");
            ddlOptr.options[6] = new Option("Greater Than", "{0} > '{1}'");
            ddlOptr.options[7] = new Option("Less than", "{0} < '{1}'");
            ddlOptr.options[8] = new Option("Greater Than Equal", "{0} >= '{1}'");
            ddlOptr.options[9] = new Option("Less than Equal", "{0} <= '{1}'");
            ddlOptr.options[10] = new Option("Is Null", "{0} Is Null {1}");
            ddlOptr.options[11] = new Option("Is Not Null", "{0} Is Not Null {1}");
            ddlOptr.options[12] = new Option("Does Not Equal", "{0} <> '{1}'");
         }
    }
    
    function onGridViewRowSelected(rowIdx)
    {
        document.getElementById("hdnDeleteCount").value=rowIdx;
        return confirm('Are you sure you want to delete this query condition?');
    }
    
    function TestQuery()
    {
        if(document.getElementById("hdnQueryCount").value!="")
        {
        if(document.getElementById("hdnQueryCount").value!="false")
        {
            alert("Query Executed Successfully Available Count: " + document.getElementById("hdnQueryCount").value);  
        }
        }
        else
        {
            alert("At least one Query Condition is Required to test the query!");
        }
        return false;
    }
       
    function CheckQueryOverwrite()
    {
        if ( document.getElementById("hdnDuplicateQuery").value=="true")
        {
            // We have a duplicate query, alert for overwrite or not
            if (confirm("A query with the specified name already exists.  Hit ok to overwrite, cancel to try another name."))
            {
                document.getElementById("hdnQueryToOverwrite").value = "true";
                __doPostBack('__Page', 'MyCustomArgument');   
            }
        }   
    }
    
    function AddSubQuery()
    {
        window.showModalDialog('../campaign/AddSubQuery.aspx?'+ ( new Date() ).getTime(),'AddSubQuery','dialogWidth:455px;dialogHeight:355px;edge:Raised;center:Yes;resizable:No;status:No');
    }
    
    </script>

</head>
<%--<body onload="ValidatorsEnabled('IsOnload');ShowPageMessage();">--%>
<body onload="ShowPageMessage();CheckQueryOverwrite();">
    <form id="form1" runat="server" defaultfocus="txtQueryName">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
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
                                                        <table cellpadding="4" cellspacing="0" border="0" width="100%">
                                                            <tr>
                                                                <td valign="middle" width="35%" align="left">
                                                                    <a href="Home.aspx" class="aHome" runat="Server" id="anchHome"></a>&nbsp;&nbsp;<img
                                                                        src="../images/arrowright.gif">&nbsp;&nbsp;<b>Query Detail</b>
                                                                    <asp:HiddenField ID="hdnDeleteCount" runat="server" />
                                                                    <asp:HiddenField ID="hdnQueryCount" runat="server" />
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
                                                                    <table cellpadding="1" cellspacing="1" width="100%" border="0">
                                                                        <tr>
                                                                            <td valign="middle" align="center" width="100%">
                                                                                <table cellspacing="0" cellpadding="2" width="100%" border="0">
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <table cellspacing="1" width="100%" cellpadding="0" border="0" id="Table3" class="tablecontentBlack">
                                                                                                <tr valign="top">
                                                                                                    <td height="20px" valign="middle" class="tdWhite" align="left" width="100%">
                                                                                                        &nbsp;&nbsp;<b>Overview</b>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr valign="top">
                                                                                                    <td valign="middle" class="tdWhite" align="left" width="100%">
                                                                                                        <table cellpadding="2" cellspacing="5" border="0" width="100%">
                                                                                                            <tr>
                                                                                                                <td valign="middle" width="15%" align="right">
                                                                                                                    <b>Query Name&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                                                <td valign="middle" align="left" style="width: 51%">
                                                                                                                    <asp:TextBox ID="txtQueryName" runat="server" CssClass="txtmedium"></asp:TextBox>
                                                                                                                    <asp:RequiredFieldValidator ID="reqQueryName" runat="server" SetFocusOnError="true" Text="*" ControlToValidate="txtQueryName"
                                                                                                                        ErrorMessage="Enter a name for the query." Display="Static"></asp:RequiredFieldValidator></td>
                                                                                                                
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td valign="middle" width="15%" align="right">
                                                                                                                    <b>Date Created&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                                                <td valign="middle" align="left" style="width: 51%">
                                                                                                                    <asp:Label ID="lblRODate" runat="server" Text="R/O Date" class="readonlyText"></asp:Label></td>
                                                                                                            </tr>
                                                                                                            <tr>
                                                                                                                <td valign="middle" width="15%" align="right">
                                                                                                                    <b>Last Modified&nbsp;:&nbsp;&nbsp;</b></td>
                                                                                                                <td valign="middle" align="left" style="width: 51%">
                                                                                                                    <asp:Label ID="lblROModifiedDate" runat="server" Text="R/O Date" class="readonlyText"></asp:Label></td>
                                                                                                                    <td valign="middle" align="right" width="55%">
                                                                                                                    <asp:LinkButton ID="lbtnShowSQL" OnClick="lbtnShowHideSQL_Click" CommandName="show" CssClass="aScoreBoard" 
                                                                                                                        runat="server" Text="Show SQL" ToolTip="Show the raw SQL generated for this query."></asp:LinkButton>   
                                                                                                                    <asp:LinkButton ID="lbtnHideSQL" OnClick="lbtnShowHideSQL_Click" CommandName="hide" CssClass="aScoreBoard" 
                                                                                                                        runat="server" Text="Hide SQL" ToolTip="Hide the raw SQL generated for this query."></asp:LinkButton>
                                                                                                                    <asp:HiddenField ID="hdnShowSQL" runat="server" /> 
                                                                                                                </td>
                                                                                                            </tr>
                                                                                                        </table>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr id="rowSQL">
                                                                                        <td align="center">
                                                                                            <table cellspacing="1" width="100%" cellpadding="0" border="0" id="tblSQL" class="tablecontentBlack" runat="server">
                                                                                                <tr valign="top">
                                                                                                    <td height="20px" valign="middle" align="left" class="tdWhite" width="13%">
                                                                                                        &nbsp;&nbsp;<b>Query SQL String</b></td>
                                                                                                    <td align="left" style="width:765px" class="tdWhite">
                                                                                                        <asp:Label ID="lblSQL" runat="server" ForeColor="#000066" Font-Size="Small" BackColor="white"/>
                                                                                                    </td>
                                                                                               </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="center">
                                                                                            <table cellspacing="1" width="100%" cellpadding="0" border="0" id="Table1" class="tablecontentBlack">
                                                                                                <tr valign="top">
                                                                                                    <%--<td height="20px" valign="middle" class="tdSetting" align="left" width="100%">
                                                                                                        &nbsp;&nbsp;Query Conditions
                                                                                                    </td>--%>
                                                                                                    <td height="20px" valign="middle" align="left" class="tdWhite">
                                                                                                        &nbsp;&nbsp;<b>Query Builder</b>
                                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                                        <%--<asp:LinkButton ID="lbtnAddFilter" CssClass="alink" OnClick="lbtnAddFilter_Click" CausesValidation="true"
                                                                                                        runat="server" Text="Add Condition" ToolTip="Add a query condition using the selected operator."></asp:LinkButton>&nbsp;&nbsp;
                                                                                                        <asp:LinkButton ID="lbtnAddSubQuery" CssClass="alink" OnClientClick="return AddSubQuery();" OnClick="lbtnAddSubQuery_Click"
                                                                                                        runat="server" Text="Add Query" ToolTip="Add a previously created query as a condition using the selected operator."></asp:LinkButton>&nbsp;&nbsp;
                                                                                                        <asp:LinkButton ID="lbtnNewSubQuery" CssClass="alink" OnClientClick="return AddSubQuery();" OnClick="lbtnAddSubQuery_Click"
                                                                                                        runat="server" Text="New Sub-Query" ToolTip="Create a new custom sub query nested within this query using the selected operator."></asp:LinkButton>--%>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr valign="top">
                                                                                                    <%--<td height="20px" valign="middle" class="tdSetting" align="left" width="100%">
                                                                                                        &nbsp;&nbsp;Query Conditions
                                                                                                    </td>--%>
                                                                                                    <td height="20px" valign="middle" align="right" class="tdWhite">
                                                                                                        <%--&nbsp;&nbsp;<b>Query Builder</b>
                                                                                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                                                                        <asp:LinkButton ID="lbtnAddFilter" OnClick="lbtnAddFilter_Click" CausesValidation="true" CssClass="aScoreBoard" 
                                                                                                        runat="server" Text="Add Condition" ToolTip="Add a query condition using the selected operator."></asp:LinkButton>&nbsp;&nbsp;
                                                                                                        <asp:LinkButton ID="lbtnAddSubQuery" OnClientClick="return AddSubQuery();" OnClick="lbtnAddSubQuery_Click" CssClass="aScoreBoard" 
                                                                                                        runat="server" Text="Add Query" ToolTip="Add a previously created query as a condition using the selected operator."></asp:LinkButton>&nbsp;&nbsp;
                                                                                                        <asp:LinkButton ID="lbtnNewSubQuery" OnClientClick="return AddSubQuery();" OnClick="lbtnAddSubQuery_Click" CssClass="aScoreBoard" 
                                                                                                        runat="server" Text="New Sub-Query" ToolTip="Create a new custom sub query nested within this query using the selected operator."></asp:LinkButton>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr valign="top">
                                                                                                    <td valign="middle" class="tdWhite" align="center" width="100%">
                                                                                                        <img src="../images/spacer.gif" height="2px" width="15px" alt="" />
                                                                                                        <%--<table cellpadding="2" cellspacing="5" border="0" width="100%">--%>
                                                                                                        <table cellspacing="0" cellpadding="0" width="100%" border="0">
                                                                                                                <asp:GridView ID="grdQueryConditions" runat="server" HeaderStyle-BackColor="#333366" HeaderStyle-ForeColor="#FFFFFF" GridLines="Both" BorderColor="DarkGray" CellPadding="5" 
                                                                                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Bold="true" 
                                                                                                                AllowSorting="false" AutoGenerateColumns="false"
                                                                                                                OnRowDataBound="grdQueryConditions_RowDataBound" >
                                                                                                                <Columns>
                                                                                                                    <asp:TemplateField HeaderText="Search Criteria" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                                                                                        <ItemStyle  />
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:Label ID="lblLevelIndicator" 
                                                                                                                                runat="server"/>
                                                                                                                            &nbsp;
                                                                                                                            <asp:Label ID="lblSubQuery" 
                                                                                                                                runat="server"/>
                                                                                                                            <asp:DropDownList ID="ddlCriteria" CausesValidation="false" OnSelectedIndexChanged="ddlCriteria_Change" AutoPostBack="true"
                                                                                                                                CssClass="dropDownList" runat="server">
                                                                                                                            </asp:DropDownList>&nbsp;<asp:CompareValidator ID="cmpCriteria" runat="server" SetFocusOnError="true" ControlToValidate="ddlCriteria"
                                                                                                                                ErrorMessage="Please enter a valid search criteria." ValueToCompare="0" Operator="NotEqual"
                                                                                                                                Type="string" Text="*" Display="Static"></asp:CompareValidator>
                                                                                                                            <asp:HiddenField ID="hdnCriteria" Value='<%# Eval("SearchCriteria") %>' runat="server" />
                                                                                                                            <asp:HiddenField ID="hdnQueryDetailID" Value='<%# Eval("QueryDetailID") %>' runat="server" />
                                                                                                                            <asp:HiddenField ID="hdnSubQueryID" runat="server" />
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Search Operator" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemStyle />
                                                                                                                        <ItemTemplate>
                                                                                                                            <%--<asp:DropDownList ID="ddlOperator" onchange="ValidatorsEnabled(this)" CausesValidation="false"
                                                                                                                                CssClass="dropDownList" runat="server">
                                                                                                                            </asp:DropDownList>     --%>
                                                                                                                            <asp:DropDownList ID="ddlOperator"  CausesValidation="false" OnSelectedIndexChanged="ddlOperator_Change" AutoPostBack="true"
                                                                                                                                CssClass="dropDownList" runat="server">
                                                                                                                            </asp:DropDownList>&nbsp;<asp:CompareValidator ID="cmpOperator" runat="server" ControlToValidate="ddlOperator"
                                                                                                                                ErrorMessage="Please select a search operator." ValueToCompare="0" SetFocusOnError="true"
                                                                                                                                Operator="NotEqual" Type="string" Text="*" Display="Static"></asp:CompareValidator>
                                                                                                                            <asp:HiddenField ID="hdnOperator" Value='<%# Eval("SearchOperator") %>' runat="server" />
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField HeaderText="Value" HeaderStyle-HorizontalAlign="Left">
                                                                                                                        <ItemStyle VerticalAlign="Bottom"> </ItemStyle>
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:TextBox ID="txtEnterValue" runat="server" CausesValidation="true"
                                                                                                                                CssClass="txtmedium" Text='<%# Eval("SearchString") %>'></asp:TextBox>
                                                                                                                            <asp:DropDownList ID="ddlPickByName" CausesValidation="false"
                                                                                                                                CssClass="dropDownList" runat="server" visible="false">
                                                                                                                            </asp:DropDownList>
                                                                                                                            <asp:CustomValidator id="vldValueValidate" runat="server" 
                                                                                                                                ErrorMessage="<<<" 
                                                                                                                                OnServerValidate="ValueEntryValidate">
                                                                                                                            </asp:CustomValidator>
                                                                                                                            <%--<asp:RequiredFieldValidator ID="reqEnterValue" runat="server" SetFocusOnError="true"
                                                                                                                                ControlToValidate="txtEnterValue" Text="*" Enabled="false" ErrorMessage="Enter Search Value"
                                                                                                                                Display="Static"></asp:RequiredFieldValidator>&nbsp;<asp:CompareValidator ID="cmpEnterValue"
                                                                                                                                    runat="server" ControlToValidate="txtEnterValue" ErrorMessage="Enter Valid Date"
                                                                                                                                    ValueToCompare="0" Enabled="false" SetFocusOnError="true" Operator="DataTypeCheck"
                                                                                                                                    Type="Date" Text="*" Display="Static"></asp:CompareValidator>
                                                                                                                            &nbsp;<asp:CompareValidator ID="cmpEnterIntValue" runat="server" ControlToValidate="txtEnterValue"
                                                                                                                                ErrorMessage="Enter Numeric Value" Enabled="false" SetFocusOnError="true" Operator="DataTypeCheck"
                                                                                                                                Type="Integer" Text="*" Display="Static"></asp:CompareValidator>--%>
                                                                                                                            <%--<asp:DropDownList ID="ddlSelectAgentName" onchange="ValidatorsEnabled(this)" CausesValidation="false"
                                                                                                                                CssClass="dropDownList" runat="server" Visible="false">
                                                                                                                            </asp:DropDownList>
                                                                                                                            <asp:RequiredFieldValidator ID="reqAgentName" runat="server" SetFocusOnError="true"
                                                                                                                                ControlToValidate="ddlSelectAgentName" Text="*" Enabled="false" ErrorMessage="Enter Search Value"
                                                                                                                                Display="Static"></asp:RequiredFieldValidator> --%>   
                                                                                                                            <asp:HiddenField ID="hdnSearchString" Value='<%# Eval("SearchString") %>' runat="server" />
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <%--<asp:TemplateField HeaderText="Available Actions">
                                                                                                                        <ItemStyle />
                                                                                                                        <ItemTemplate>
                                                                                                                            <%--<asp:LinkButton ID="lbtnAddFilter" OnClick="lbtnAddFilter_Click" CausesValidation="true"
                                                                                                                                runat="server"><img src="../images/addfilter.jpg" border="0"></asp:LinkButton>
                                                                                                                                <asp:LinkButton ID="lbtnAddFilter" CssClass="alink" OnClick="lbtnAddFilter_Click" CausesValidation="true"
                                                                                                                                runat="server" Text="Add Condition" ToolTip="Add a query condition using the selected operator."></asp:LinkButton>
                                                                                                                                or
                                                                                                                                <asp:LinkButton ID="lbtnAddSubQuery" CssClass="alink" OnClientClick="return AddSubQuery();" OnClick="lbtnAddSubQuery_Click"
                                                                                                                                runat="server" Text="Add Query" ToolTip="Add a previously created query as a condition using the selected operator."></asp:LinkButton>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>--%>
                                                                                                                    <asp:TemplateField HeaderText="Operator">
                                                                                                                        <ItemStyle />
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:DropDownList ID="ddlLogical" CausesValidation="false" CssClass="select" runat="server">
                                                                                                                            </asp:DropDownList>
                                                                                                                            <asp:HiddenField ID="hdnLogical" Value='<%# Eval("LogicalOperator") %>' runat="server" />
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                    <asp:TemplateField>
                                                                                                                        <ItemStyle />
                                                                                                                        <ItemTemplate>
                                                                                                                            <asp:LinkButton ID="lbtnDelete" Text="Delete" CausesValidation="false" runat="server"
                                                                                                                                CssClass="alink" CommandName='<%# Eval("QueryDetailID") %>' CommandArgument="Delete"
                                                                                                                                OnClick="lbtnAddFilter_Click"></asp:LinkButton>
                                                                                                                        </ItemTemplate>
                                                                                                                    </asp:TemplateField>
                                                                                                                </Columns>
                                                                                                            </asp:GridView>
                                                                                                            <img src="../images/spacer.gif" height="2px" width="15px" alt="" />
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
                                                        <asp:LinkButton ID="lbtnSave" OnClick="lbtnSave_Click" runat="server"><img src="../images/save.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                            ID="lbtnCancel" OnClick="lbtnCancel_Click" OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"
                                                            runat="server" CausesValidation="false"><img src="../images/cancel.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                ID="lbtnTestQuery" OnClick="lbtnTestQuery_Click" CausesValidation="true" runat="server"><img src="../images/testquery.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                    ID="lbtnClose" OnClick="lbtnClose_Click" CausesValidation="false"
                                                                    runat="server"><img src="../images/close.jpg" border="0" /></asp:LinkButton>
                                                        <asp:ValidationSummary ID="valsumQueryDetail" runat="server" ShowMessageBox="true"
                                                            ShowSummary="false" BackColor="Black" />
                                                        <asp:HiddenField ID="hdnCount" Value="0" runat="server" /></td>
                                                        <asp:HiddenField ID="hdnQueryToOverwrite" runat="server"/>
                                                        <asp:HiddenField ID="hdnDuplicateQuery" runat="server"/>  
                                                </tr>
                                            </table>
                                            <!-- Footer -->
                                            <RainMaker:Footer ID="CampaignFooter" runat="server"></RainMaker:Footer>
                                            <!-- Footer -->
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
    </form>
</body>
</html>
