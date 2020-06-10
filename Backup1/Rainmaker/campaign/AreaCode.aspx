<%@ Page Language="C#" AutoEventWireup="true" Codebehind="AreaCode.aspx.cs" Inherits="Rainmaker.Web.campaign.AreaCode" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Area Code</title>
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
    function disableRadioButtons()
    {
        if(document.getElementById('rbtnAllNumbersDial').checked)
        {
            document.getElementById('pnlAreaCode').style.display = 'none';
            document.getElementById('pnlAllNumDial').disabled = false;
            document.getElementById('pnlCustomeDialing').disabled = true;
            
//            document.getElementById('rbtnElevenDigitDialing').disabled = false;
//            document.getElementById('rbtnTenDigitDialing').disabled = false;
        }
        else if(document.getElementById('rbtnCustomeDialing').checked)
        {
            document.getElementById('pnlAllNumDial').disabled = true;
            document.getElementById('pnlCustomeDialing').disabled = false;
//            document.getElementById('rbtnElevenDigitDialing').checked = false;
//            document.getElementById('rbtnTenDigitDialing').checked = false;
//            
//            document.getElementById('rbtnTenDigitDialing').disabled = true;
//            document.getElementById('rbtnElevenDigitDialing').disabled = true;
        }
        ValidatorsEnable();
    }
    
    function returnFalse()
    {
        if(document.getElementById('rbtnAllNumbersDial').checked)
        {
            return false;
        }
    }
    </script>

    <script language="javascript" type="text/javascript">
    var gridViewCtlId = '<%=grdAreaCode.ClientID%>';
    var gridViewCtl = null;
    var curSelRow = null;
    function getGridViewControl()
    {
        if (null == gridViewCtl)
        {
            gridViewCtl = document.getElementById(gridViewCtlId);
        }
    }
    
    function onGridViewRowSelected(rowIdx,ID)
    {
        var selRow = getSelectedRow(rowIdx);
       
        if (curSelRow != null)
        {
            curSelRow.style.backgroundColor = '#ffffff';
        }
        
        if (null != selRow)
        {
            curSelRow = selRow;
            document.getElementById('hdnAreaCodeID').value = ID;
            curSelRow.style.backgroundColor = '#ffffff';
        }
    }
    
    function getSelectedRow(rowIdx)
    {
        getGridViewControl();
        if (null != gridViewCtl)
        {
            return gridViewCtl.rows[rowIdx];
        }
        return null;
    }
    </script>

</head>
<body onload="ShowPageMessage();">
    <form id="frmAreaCode" runat="server">
        <div>
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
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <table cellpadding="4" cellspacing="0" border="0" width="100%">
                                                    <%--<tr>
                                                        <td align="left" width="100%">
                                                            <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="bottom" align="left">
                                                                        <a href="Home.aspx" class="aHome" runat="server" id="anchHome">
                                                                            <asp:Label ID="lblCampaign" runat="server" Text="(CompaignName)"></asp:Label>
                                                                        </a>&nbsp;&nbsp;<img src="../images/arrowright.gif">&nbsp;&nbsp;<b>Area Code Rules</b>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>--%>
                                                    <tr>
                                                        <td valign="top">
                                                            <table border="0" cellpadding="2" cellspacing="2" width="100%">
                                                                <tr>
                                                                    <td colspan="2" valign="top">
                                                                        <asp:RadioButton ID="rbtnAllNumbersDial" runat="server" Text="All Numbers dial the same"
                                                                            GroupName="LikeDialing" onclick="disableRadioButtons();" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="2%">
                                                                        &nbsp;</td>
                                                                    <td align="left" width="98%">
                                                                        <asp:Panel ID="pnlAllNumDial" runat="server">
                                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:RadioButton ID="rbtnElevenDigitDialing" runat="server" Text="1 + 10 - digit dialing"
                                                                                            GroupName="rbtnTenDigitDialing" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left">
                                                                                        <asp:RadioButton ID="rbtnTenDigitDialing" runat="server" Text="10 - digit dialing"
                                                                                            GroupName="rbtnTenDigitDialing" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" align="left" colspan="2">
                                                                        <asp:RadioButton ID="rbtnCustomeDialing" runat="server" Text="Custom Dialing" Checked="true"
                                                                            GroupName="LikeDialing" onclick="disableRadioButtons();" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="2%">
                                                                        &nbsp;</td>
                                                                    <td align="left" width="98%">
                                                                        <asp:Panel ID="pnlCustomeDialing" runat="server">
                                                                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                                <tr>
                                                                                    <td>
                                                                                        <img src="../images/spacer.gif" height="5px" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <b>Local Calls</b>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <%--  <td width="5%">
                                                                                    &nbsp;</td>--%>
                                                                                    <td width="95%" align="left" colspan="2">
                                                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td align="left">
                                                                                                    <asp:RadioButton ID="rbtnDialSevenDigits" runat="server" Text="Dial 7 - digits" GroupName="rbtnLocalDialing" />
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td align="left">
                                                                                                    <asp:RadioButton ID="rbtnDialTenDigits" runat="server" Text="Dial 10 - digits" GroupName="rbtnLocalDialing" />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <img src="../images/spacer.gif" height="5px" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" colspan="2" class="aHome">
                                                                                        <b>Local Calls have following Area Code & Prefix</b>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="left" width="50%">
                                                                                        <table border="0" cellpadding="2" cellspacing="6" width="100%">
                                                                                            <tr>
                                                                                                <td width="60%" align="center">
                                                                                                    <%--<asp:HiddenField ID="hdnAreaCodeRuleID" runat="server" Value="" />--%>
                                                                                                    <asp:HiddenField ID="hdnAreaCodeID" runat="server" Value="" />
                                                                                                    <asp:GridView runat="server" AutoGenerateColumns="False" DataKeyNames="AreaCodeID"
                                                                                                        ID="grdAreaCode" Width="100%" CellPadding="3" CellSpacing="1" BorderWidth="0"
                                                                                                        CssClass="tablecontentBlack" OnRowDataBound="OnRowCreated">
                                                                                                        <HeaderStyle CssClass="tableHdr" />
                                                                                                        <RowStyle CssClass="tableRow" />
                                                                                                        <EmptyDataRowStyle CssClass="tableHdr" />
                                                                                                        <EmptyDataTemplate>
                                                                                                            No AreaCode and Prefix Found</EmptyDataTemplate>
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField HeaderText="Area Code">
                                                                                                                <ItemStyle Width="50%" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lbtnAreaCode" CssClass="alink" Text='<%# Eval("AreaCode") %>'
                                                                                                                        CommandArgument='<%# DataBinder.Eval(Container.DataItem,"AreaCodeID") + "," + DataBinder.Eval(Container.DataItem,"Prefix") %>'
                                                                                                                        runat="server" CausesValidation="false" OnClick="lbtnAreaCode_Click" OnClientClick="return returnFalse();"></asp:LinkButton>
                                                                                                                    <asp:HiddenField ID="hdnGrdAreaCodeID" runat="server" Value='<%# Eval("AreaCodeID") %>' />
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="Prefix" HeaderText="Prefix" ItemStyle-Width="40%" />
                                                                                                            <asp:TemplateField HeaderText="Delete">
                                                                                                                <ItemStyle Width="10%" />
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lbtnDelete" CssClass="alink" Text="Delete" runat="server" CausesValidation="false"
                                                                                                                        OnClick="lbtnDelete_Click" CommandArgument='<%# Eval("AreaCodeID") %>' OnClientClick="javascript:if(document.getElementById('rbtnAllNumbersDial').checked){return false;}else{ return confirm('Do you want to delete the area code!');}"></asp:LinkButton>
                                                                                                                </ItemTemplate>
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                    </asp:GridView>
                                                                                                </td>
                                                                                                <td align="left" width="40%">
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table border="0" cellpadding="2" cellspacing="0" width="40%">
                                                                                                        <tr>
                                                                                                            <td colspan="2" align="left">
                                                                                                                <asp:LinkButton ID="lbtnAdd" runat="server" CausesValidation="false" Text="Add" OnClick="lbtnAdd_Click"
                                                                                                                    OnClientClick="return returnFalse();"><img src="../images/add.jpg" border="0" /></asp:LinkButton></td>
                                                                                                            <asp:HiddenField ID="hdnGrdAreaCode" runat="server" Value="" />
                                                                                                            <asp:HiddenField ID="hdnGrdPrefix" runat="server" Value="" />
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                                <td>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <div id="pnlAreaCode" runat="server">
                                                                                                        <table cellpadding="1" cellspacing="0" border="0" class="tdHeader">
                                                                                                            <tr>
                                                                                                                <td>
                                                                                                                    <table cellpadding="0" cellspacing="4" border="0" class="tdWhite">
                                                                                                                        <tr>
                                                                                                                            <td align="left">
                                                                                                                                <label>
                                                                                                                                    <b>Add/Edit Area Code</b></label></td>
                                                                                                                        </tr>
                                                                                                                        <tr>
                                                                                                                            <td align="left">
                                                                                                                                <table border="0" cellpadding="2" cellspacing="0" class="tdWhite">
                                                                                                                                    <tr>
                                                                                                                                        <td align="right">
                                                                                                                                            Area Code&nbsp;&nbsp;:&nbsp;
                                                                                                                                        </td>
                                                                                                                                        <td align="left">
                                                                                                                                            <asp:TextBox ID="txtLocalAreaCode" MaxLength="4" runat="server" Text="" CssClass="txtmedium"></asp:TextBox>
                                                                                                                                            <asp:RequiredFieldValidator ID="reqLocalAreaCode" runat="server" ControlToValidate="txtLocalAreaCode"
                                                                                                                                                ErrorMessage="Please Enter Area Code" SetFocusOnError="True" Display="static"
                                                                                                                                                Enabled="false">*</asp:RequiredFieldValidator>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td align="right">
                                                                                                                                            Prefix&nbsp;&nbsp;:&nbsp;
                                                                                                                                        </td>
                                                                                                                                        <td align="left">
                                                                                                                                            <asp:TextBox ID="txtLocalPrefix" MaxLength="4" runat="server" Text="" CssClass="txtmedium"></asp:TextBox>
                                                                                                                                            <asp:RequiredFieldValidator ID="reqLocalPrefix" runat="server" ControlToValidate="txtLocalPrefix"
                                                                                                                                                ErrorMessage="Please Enter Area Code Prefix" SetFocusOnError="True" Display="static"
                                                                                                                                                Enabled="false">*</asp:RequiredFieldValidator>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td colspan="2">
                                                                                                                                            <asp:LinkButton ID="lbtnAreaCodeSave" OnClick="lbtnAreaCodeSave_Click" runat="server"
                                                                                                                                                OnClientClick="enableAreaCodeValidators();"><img src="../images/save.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;
                                                                                                                                            <asp:LinkButton ID="lbtnAreaCodeCancel" OnClick="lbtnAreaCodeCancel_Click" OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"
                                                                                                                                                runat="server" CausesValidation="false"><img src="../images/cancel.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;
                                                                                                                                            <asp:LinkButton ID="lbtnAreaCodeClose" CausesValidation="false" runat="server" OnClick="lbtnAreaCodeClose_Click"><img src="../images/close.jpg" border="0" /></asp:LinkButton>&nbsp;
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
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <table border="0" cellpadding="2" cellspacing="0" width="80%">
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <img src="../images/spacer.gif" height="5px" /></td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <b>Intra-Lata Long Distance Calls</b>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td width="2%">
                                                                                                    &nbsp;
                                                                                                </td>
                                                                                                <td>
                                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                        <tr>
                                                                                                            <td align="left" width="40%">
                                                                                                                Are Non-Local calls with the following area codes&nbsp;&nbsp;:&nbsp;
                                                                                                            </td>
                                                                                                            <td align="left" width="60%">
                                                                                                                <asp:TextBox ID="txtAreaCode" MaxLength="4" runat="server" Text="0" CssClass="txtsmall"></asp:TextBox>&nbsp
                                                                                                                <asp:CompareValidator ID="cmpAreaCode" runat="server" ControlToValidate="txtAreaCode"
                                                                                                                    ErrorMessage="Enter numeric value" Enabled="true" SetFocusOnError="true" Operator="DataTypeCheck"
                                                                                                                    Type="Integer" Text="*" Display="static"></asp:CompareValidator>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td colspan="2" align="left">
                                                                                                                <asp:RadioButton ID="rbtnILDialTenDigit" runat="server" Text="Dial 10 - digits" GroupName="ILDialer" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td colspan="2" align="left">
                                                                                                                <asp:RadioButton ID="rbtnILDialElevenDigit" runat="server" Text="Dial 1 + 10 - digits"
                                                                                                                    GroupName="ILDialer" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left" width="40%">
                                                                                                                <asp:CheckBox ID="chkReplaceAreaCode" runat="server" Text="Replace area code with"
                                                                                                                    onclick="disReplaceAreaCode();" />&nbsp;&nbsp;:&nbsp;
                                                                                                            </td>
                                                                                                            <td align="left" width="60%">
                                                                                                                <asp:TextBox ID="txtReplaceAreaCode" runat="server" MaxLength="4" CssClass="txtsmall"></asp:TextBox>
                                                                                                                <asp:RequiredFieldValidator ID="reqtxtReplaceAreaCode" runat="server" ErrorMessage="Please Enter Replace Area Code"
                                                                                                                    ControlToValidate="txtReplaceAreaCode" SetFocusOnError="true">*</asp:RequiredFieldValidator>
                                                                                                                <asp:CompareValidator ID="cmptxtReplaceAreaCode" runat="server" ControlToValidate="txtReplaceAreaCode"
                                                                                                                    Operator="DataTypeCheck" Type="integer" SetFocusOnError="true" ErrorMessage="Enter numeric value">*</asp:CompareValidator>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <img src="../images/spacer.gif" height="5px" /></td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                        <table border="0" cellpadding="2" cellspacing="0" width="100%">
                                                                                            <tr>
                                                                                                <td colspan="2">
                                                                                                    <b>Long Distance Calls</b>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td width="2%">
                                                                                                    &nbsp;</td>
                                                                                                <td>
                                                                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                                                        <tr>
                                                                                                            <td align="left">
                                                                                                                <asp:RadioButton ID="rbntLDDialTenDigits" runat="server" Text="Dial 10 - digits"
                                                                                                                    GroupName="LongDistanceDial" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td align="left">
                                                                                                                <asp:RadioButton ID="rbntLDDialElevenTenDigits" runat="server" Text="Dial 1 + 10 - digits"
                                                                                                                    GroupName="LongDistanceDial" />
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </asp:Panel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="right" colspan="2">
                                                            <asp:LinkButton ID="lbtnSave" OnClick="lbtnSave_Click" runat="server" OnClientClick="disableAreaCodeValidators();"><img src="../images/save.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                ID="lbtnClose" runat="server" Text="Close" PostBackUrl="~/campaign/CampaignList.aspx"
                                                                CausesValidation="false"><img src="../images/close.jpg" border="0" /></asp:LinkButton><asp:ValidationSummary
                                                                    ID="valsumAreaCode" runat="server" ShowMessageBox="true" ShowSummary="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                    <!-- Body -->
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

    <script language="javascript" type="text/javascript">
        disableRadioButtons();
        disReplaceAreaCode();
        function disReplaceAreaCode()
        {
            if(!document.getElementById("chkReplaceAreaCode").checked)
            {
                document.getElementById("txtReplaceAreaCode").disabled = true;
            }
            else
            {
                document.getElementById("txtReplaceAreaCode").disabled = false;
            }
            ValidatorsEnable();
        }
        
        function ValidatorsEnable()
        {
            var enbale = false;
            if(document.getElementById('rbtnCustomeDialing').checked && document.getElementById("chkReplaceAreaCode").checked)
            {
                enbale = true;
            }
            ValidatorEnable(document.getElementById('<%=reqtxtReplaceAreaCode.ClientID%>'), enbale);
            ValidatorEnable(document.getElementById('<%=cmptxtReplaceAreaCode.ClientID%>'), enbale);
            
//            if(document.getElementById('pnlAreaCode').style.display == "none")
//            {
//                ValidatorEnable(document.getElementById('<%=reqLocalAreaCode.ClientID%>'), false);
//                ValidatorEnable(document.getElementById('<%=reqLocalPrefix.ClientID%>'), false);
//            }
        }
        
        function enableAreaCodeValidators()
        {
            ValidatorEnable(document.getElementById('<%=reqLocalAreaCode.ClientID%>'), true);
            ValidatorEnable(document.getElementById('<%=reqLocalPrefix.ClientID%>'), true);
        }
        
        function disableAreaCodeValidators()
        {
            ValidatorEnable(document.getElementById('<%=reqLocalAreaCode.ClientID%>'), false);
            ValidatorEnable(document.getElementById('<%=reqLocalPrefix.ClientID%>'), false);
        }
       
    </script>

</body>
</html>
