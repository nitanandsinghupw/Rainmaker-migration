<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManualDial.aspx.cs" Inherits="Rainmaker.Web.agent.ManualDial"
    ValidateRequest="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Toolbar" Src="~/common/controls/AgentToolbar.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Manual Dial</title>

    <script type="text/javascript">
    <!--
        var cbrowser = navigator.userAgent;
        if (cbrowser.toLowerCase().indexOf("firefox") > 0) {
            document.write('<link rel="stylesheet" href="../css/RainmakerNS.css" type="text/css" />');
        } else {
            document.write('<link rel="stylesheet" href="../css/Rainmaker.css" type="text/css" />');
        }
    //-->    
    </script>

    <script type="text/javascript" src="../js/jquery.js"></script>

    <script type="text/javascript" src="../js/json2.js"></script>

    <script type="text/javascript" src="../js/jquery-ui-1.9.1.custom.min.js"></script>

    <script type="text/javascript" src="../js/jquery-ui-timepicker-addon.js"></script>

    <script type="text/javascript" src="../js/jquery-ui-sliderAccess.js"></script>

    <script type="text/javascript" src="../js/jquery.caret.1.02.js"></script>

    <script type="text/javascript" src="../js/Rainmaker.js"></script>

    <script type="text/javascript" src="../js/dialer.js"></script>

    <link rel="stylesheet" media="all" type="text/css" href="../css/jquery-ui.css" />
    <link rel="stylesheet" media="all" type="text/css" href="../css/jquery-ui-timepicker-addon.css" />
    <link rel="stylesheet" href="../css/jqueryuidialogstyle.css" />

    <script type="text/javascript">

        function doGetCaretPosition(oField) {

            // Initialize
            var iCaretPos = 0;

            // IE Support
            if (document.selection) {

                // Set focus on the element
                oField.focus();

                // To get cursor position, get empty selection range
                var oSel = document.selection.createRange();

                // Move selection start to 0 position
                oSel.moveStart('character', -oField.value.length);

                // The caret position is selection length
                iCaretPos = oSel.text.length;
            }

            // Firefox support
            else if (oField.selectionStart || oField.selectionStart == '0')
                iCaretPos = oField.selectionStart;

            // Return results
            return (iCaretPos);
        }

        //Override default prototype to allow functions to work in IE    
        function setSelectionRange(input, selectionStart, selectionEnd) {
            if ((doGetCaretPosition(input) != selectionStart) && (doGetCaretPosition(input) != selectionEnd)) {
                if (input.setSelectionRange) {

                    input.setSelectionRange(selectionStart, selectionStart);

                }
                else if (input.createTextRange) {
                    var range = input.createTextRange();
                    range.collapse(true);
                    range.moveEnd('character', selectionEnd);
                    range.moveStart('character', selectionStart);
                    range.select();
                }
            }
        }

        function moveCaretToEnd(el) {

            if (typeof el.selectionStart == "number") {
                el.selectionStart = el.selectionEnd = el.value.length;
            } else if (typeof el.createTextRange != "undefined") {
                el.focus();
                var range = el.createTextRange();
                range.collapse(false);
                range.select();
            }
        }

        function setPhoneNumberCaret() {

            var elem = document.getElementById("txtPhoneNumber");
            if (elem != null) {

                var caretpos = $("#<%=HiddenCaretPosition.ClientID%>").val();
                var numberofchars = $('#txtPhoneNumber').val().length;
                var icaretpos = parseInt(caretpos);
                if (!isNaN(icaretpos)) {
                    if ((caretpos != "") && (numberofchars >= icaretpos)) {
                        setSelectionRange(elem, caretpos, caretpos);
                    }
                }
            }
        }

        function jqueryfunctions() {

            $(document).ready(function() {

                $.ajaxSetup({ cache: false });
                $('input[data-type=dtpicker]').live().each(function() {
                    $(this).datetimepicker({
                        timeFormat: 'HH:mm:ss'
                    });
                });
                $('input').live().bind('blur', function() {

                    try {
                        var ctrlName = '';
                        var ctrlValue = '';
                        ctrlName = $(this).attr('name');
                        ctrlValue = $(this).val();

                        $('input').each(function() {

                            if ($(this).attr('name') == ctrlName) {
                                $(this).attr('value', ctrlValue);
                            }
                        });
                    } catch (ex) { }
                });
                $('select').live().bind('change', function() {

                    try {
                        SelectDropDown($(this).attr('data-name'), $(this).val())
                    } catch (ex) { }
                });
                $('#phonenum').attr('disabled', 'true');
            });
        }
        jqueryfunctions();

        function cursorfix() {

            $("#txtPhoneNumber").bind("select keyup click", function() {
                $("#<%=HiddenCaretPosition.ClientID%>").val($(this).caret().start);
                //$("#TextBox1").val($(this).caret().start);
            });

            var pagerequestmanager = Sys.WebForms.PageRequestManager.getInstance();
            pagerequestmanager.add_endRequest(EndRequestHandler);
            pagerequestmanager.add_beginRequest(BeginRequestHandler);
            function BeginRequestHandler(sender, args) {

                $("#txtPhoneNumber").bind("select keyup click", function() {
                    $("#<%=HiddenCaretPosition.ClientID%>").val($(this).caret().start);
                    //$("#TextBox1").val($(this).caret().start);
                });

            }
            function EndRequestHandler(sender, args) {
                if (args.get_error() == undefined) {
                    $("#txtPhoneNumber").bind("select keyup click", function() {
                        $("#<%=HiddenCaretPosition.ClientID%>").val($(this).caret().start);
                        //$("#TextBox1").val($(this).caret().start);
                        //$("#txtPhoneNumber").focus();
                    });
                    jqueryfunctions();
                }
            }
        }

        function ShowInvalidCampaign() {
            alert("The campaign has been paused. Please select a different campaign.");
            window.location.href = "Campaigns.aspx";
        }

        function LoadScript(scriptid) {
            saveCampaign(null, scriptid);
        }

        function RenderScript(scriptid) {
            //alert(scriptid);
            $('#hndScriptID').val('');
            var isHeader = "true";
            var isInvalidScript = false;
            var newDate = new Date();
            var timeStamp = newDate.getMonth() + "" + newDate.getDate() + "" + newDate.getYear()
	                                    + "" + newDate.getHours() + "" + newDate.getMinutes() + "" + newDate.getSeconds() + "" + newDate.getMilliseconds();
            $.get("GetScript.aspx", {
                ScriptId: scriptid,
                IsHeader: isHeader,
                ts: timeStamp,
                cache: false
            },
            function(data) {
                if (data.indexOf("#invalidscript#") >= 0) {
                    isInvalidScript = true;
                    alert("Invalid Script");
                    return false;
                }
                else {

                    document.getElementById("divScriptHeader").innerHTML = data;
                    //scroll(0,0);
                }
            });

            if (isInvalidScript == true)
                return false;
            isHeader = "false";
            $.get("GetScript.aspx", {
                ScriptId: scriptid,
                IsHeader: isHeader,
                ts: timeStamp,
                cache: false
            },
            function(data) {

                if (data.indexOf("#invalidscript#") >= 0) {
                    isInvalidScript = true;
                    return false;
                }
                else {
                    scroll(0, 0);
                    jqueryfunctions();
                    $('#hndScriptID').val(scriptid); $('#hdnScriptBody').val(data); __doPostBack('ltrlScriptbody', data);
                    return true;
                }
            });
        }

        function saveCampaign(scriptaction, scriptid) {

            var isValid = true;

            var campaignCall = new CampaignCall
            (
                new CampaignForm
                (
                    {
                        fieldInfo: filledArray,
                        isScript: true,
                        pageName: "ManualDial",
                        $form: $("#frmManDial"),
                        $validationDialog: $("#validationDialog")
                    }
                ),
                new CampaignData(campaignId)
            );

            campaignCall.save(function(data) {
                if (data.error) {
                    campaignCall.showValidationError(data);
                    isValid = false;
                }
                else {
                    if (scriptid != null) {
                        RenderScript(scriptid);
                    }
                    else if (scriptaction != null) {
                        switch (scriptaction) {

                            case "Dispose":
                                ShowPopUp('Disposition', 'ManualDial');
                                break;
                            case "DisposeLookup":
                                ShowPopUp('Disposition', 'ManualDialLookup');
                                break;
                            case "Schedule":
                                ShowPopUp('Schedule', 'ManualDial');
                                break;
                            case "ScheduleLookup":
                                ShowPopUp('Schedule', 'ManualDial');
                                break;
                        }
                    }
                }
            });

            return isValid;
        }

        function SelectDropDown(id, value) {

            value = value.replace("&apos;", "'");
            //var elements = document.getElementsByName(id);
            var elements = document.getElementsByTagName("select");
            if (elements != null) {

                for (var j = 0; j < elements.length; j++) {

                    if (elements[j].getAttribute('data-name') == id) {
                        //alert('data-name is : ' + elements[j].getAttribute('data-name'));
                        //if (elements[j].nodeName == 'SELECT') {
                        elements[j].value = value;
                    }
                }
            }
        }

        function CheckRadioButton(id, value) {

            var radio = document.getElementsByName(id);
            if (radio != null) {
                //alert(id+":"+value+":"+radio.length);
                for (var j = 0; j < radio.length; j++) {
                    if (radio[j].type == "radio" && radio[j].value.toLowerCase() == value.toLowerCase()) {
                        radio[j].checked = true;
                        break;
                    }
                }
            }
        }

        function SelectCheckbox(id) {
            if (document.getElementById(id) != null)
                document.getElementById(id).checked = true;
        }

        function DisposeCall(resultcodename, _confirm) {

            if (_confirm !== false) {
                _confirm = confirm("Are you sure you want to dispose this call as " + resultcodename + "?");
            } else _confirm = true;

            if (_confirm) {

                var campaignCall = new CampaignCall
                (
                    new CampaignForm
                    (
                        {
                            fieldInfo: filledArray,
                            isScript: true,
                            pageName: "ManualDial",
                            $form: $("#frmManDial"),
                            $validationDialog: $("#validationDialog")
                        }
                    ),
                    new CampaignData(campaignId)
                );

                campaignCall.setDisposition(resultcodename);

                campaignCall.saveAndDispose(function(data) {
                    if (data.error) {
                        campaignCall.showValidationError(data);
                    }
                    else {
                        campaignCall.campaignForm.setValue("hdnDispose", "IsDispose");
                        campaignCall.campaignForm.submit();
                    }
                });
            }
        }

        function HangupCall() {

            if (document.getElementById("lbtnHangup").disabled == false || document.getElementById("lbtnHangup").disabled == undefined) {

                try {
                    document.getElementById("imgHangup").src = "../images/hangup_grey_red.jpg";
                    document.getElementById("lbtnHangup").disabled = true;

                }
                catch (e) {
                }
                var newDate = new Date();
                var timeStamp = newDate.getMonth() + "" + newDate.getDate() + "" + newDate.getYear()
	                                        + "" + newDate.getHours() + "" + newDate.getMinutes() + "" + newDate.getSeconds() + "" + newDate.getMilliseconds();
                $.get("Hangup.aspx", {
                    ts: timeStamp
                },
                function(data) {

                    if (data == "failed") {
                        return true;
                    }
                });
            }
            return false;
        }

        function SetResultCode(resultcode) {
            if (document.frmManDial.cboResultCode != null && document.frmManDial.cboResultCode.value != "-1") {
                var ddlOptr = document.frmManDial.cboResultCode;
                var l = 0;
                if (ddlOptr.options != null && ddlOptr.options.length != null) {
                    var listLength = ddlOptr.options.length;
                    for (l = listLength - 1; l > -1; l--) {
                        //alert(ddlOptr.options[l].value + " : "+resultcode);
                        if (ddlOptr.options[l].value == resultcode) {
                            document.frmManDial.cboResultCode.value = resultcode;
                        }
                    }
                }
            }
            //alert(resultcode);
        }

        function SetFocusToPhoneNumber() {

            try {
                //alert('g');
                setTimeout(function() {
                    if (document.frmManDial.txtPhoneNumber) {
                        document.frmManDial.txtPhoneNumber.focus();
                    }
                }, 1);

            }
            catch (e)
            { }
        }

        function SetEnd(TB) {

            if (TB.createTextRange) {

                var FieldRange = TB.createTextRange();
                FieldRange.moveStart('character', TB.value.length);
                FieldRange.collapse();
                FieldRange.select();
            }
        }
        function forcedial(isLookup) {

            if (isLookup) {

                document.getElementById("chkLookup").checked = true;
            } else {
                document.getElementById("chkLookup").checked = false;
            }


            document.getElementById("lbtnDial").click();
        }

    </script>

    <script type="text/javascript" src="../js/scriptEditor.js"></script>

    <script type="text/javascript">
        function doKey(e) {
            evt = e || window.event; // compliant with ie6
            if (evt.keyCode == 13) {
                evt.keyCode = 9;
            }
            else if (evt.keyCode == 116) {
                evt.keyCode = 9;
            }
        }
        
    </script>

    <style>
        .floatingbuttons
        {
            position: fixed;
            top: 10px;
            left: 88%;
        }
    </style>
</head>
<body onload="ShowPageMessage();" onkeydown="doKey(event)">
    <div id="dialog-modal" title="Rainmaker Dialing Systems">
    </div>
    <form id="frmManDial" runat="server" defaultbutton="lbtnDial" defaultfocus="txtPhoneNumber">
    <asp:ModalPopupExtender ID="MPE_Modal1" runat="server" TargetControlID="HiddenField1"
        PopupControlID="pnlpopup" Enabled="True" BackgroundCssClass="modalBackground" />
    <asp:ModalPopupExtender ID="MPE_Modal2" runat="server" TargetControlID="HiddenField2"
        PopupControlID="pnlCallback" Enabled="True" BackgroundCssClass="modalBackground" />
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
    </asp:ScriptManager>
    <asp:SqlDataSource ID="dsCallBack" runat="server"></asp:SqlDataSource>
    <asp:Timer ID="Timer2" OnTick="Timer2_Tick" runat="server">
    </asp:Timer>
    <div>
        <table cellpadding="0" cellspacing="1" border="0" width="100%" class="tdHeader">
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                        <tr>
                            <td>
                                <!-- Header -->
                                <div style="height: 170px">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td>
                                                <img alt="" src="../images/LeadSweepBannerLarge.jpg">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center>
                                                    <label id="statuslabel" name="statuslabel">
                                                    </label>
                                                </center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdSpacer" height="1px">
                                                <img src="../images/spacer.gif" height="1px">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <!-- Header -->
                                <!-- Body -->
                                <div id="main">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td width="90%" class="tdSideBar" align="center">
                                                <asp:HiddenField ID="HiddenField1" runat="server" />
                                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                                <asp:HiddenField ID="HiddenField3" runat="server" />
                                                <asp:HiddenField ID="hndScriptID" runat="server" />
                                                <asp:HiddenField ID="HiddenCaretPosition" runat="server" />
                                                <asp:HiddenField ID="hdnScriptBody" runat="server" />
                                                <asp:HiddenField ID="hdnDisposeDialog" runat="server" />
                                                <asp:Panel ID="pnlpopup" runat="server" Style="display: none">
                                                    <asp:UpdatePanel ID="UpdatePanelAdmin" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="ModalPanel" runat="server" Width="349px" Height="126px" ForeColor="Black"
                                                                BackColor="White" BorderStyle="Solid">
                                                                <br />
                                                                You have been reset by the administrator. You will be redirected to the login page.<br />
                                                                <br />
                                                                <asp:Button ID="OKButton" runat="server" Text="Ok" OnClick="OKButton_Click" Style="height: 26px"
                                                                    Height="22px" Width="61px" CausesValidation="False" />
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="OkButton" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlCallback" runat="server" Style="display: none;">
                                                    <asp:UpdatePanel ID="UpdatePanelCallback" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <asp:HiddenField ID="hdnCallBackKey" runat="server" />
                                                            <asp:Panel ID="UpdatePanelCallBackInner" runat="server" Width="549px" Height="349px"
                                                                ForeColor="Black" BackColor="White" BorderStyle="Solid">
                                                                <br />
                                                                <asp:Label ID="lblCallback" runat="server" Text="You have a scheduled callback [datetime] for [number]"></asp:Label>
                                                                <br />
                                                                <br />
                                                                <asp:TextBox ID="txtCallbackNotes" runat="server" Height="207px" Width="418px" TextMode="MultiLine"></asp:TextBox>
                                                                <br />
                                                                <br />
                                                                <br />
                                                                <asp:Button ID="btnCallbackDial" runat="server" Text="Dial" Style="height: 26px"
                                                                    Height="22px" Width="61px" CausesValidation="False" OnClick="btnCallbackDial_Click" />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="btnCallbackLookup" runat="server" Text="Lookup" Style="height: 26px"
                                                                    Height="22px" Width="61px" CausesValidation="False" OnClick="btnCallbackLookup_Click" />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <asp:Button ID="btnCallbackCancel" runat="server" Text="Cancel" Style="height: 26px"
                                                                    Height="22px" Width="61px" CausesValidation="False" OnClick="btnCallbackCancel_Click" />
                                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnCallbackDial" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnCallbackLookup" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnCallbackCancel" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                                <asp:UpdatePanel ID="updatePanelAQ" runat="server">
                                                    <Triggers>
                                                        <asp:AsyncPostBackTrigger ControlID="Timer2" EventName="Tick" />
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        <asp:Panel ID="pnlManualDial" runat="server">
                                                            <table cellpadding="0" cellspacing="1" border="0" width="100%">
                                                                <tr>
                                                                    <td class="tdWaiting" width="100%" align="center" valign="top">
                                                                        Manual Dial
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <table cellpadding="0" cellspacing="1" border="0" width="35%">
                                                                <tr>
                                                                    <td width="100%" align="center">
                                                                        <!-- Content Begin -->
                                                                        <table cellpadding="3" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                            <tr>
                                                                                <td align="right" valign="middle">
                                                                                    <b>Select Campaign</b>&nbsp;:&nbsp;&nbsp;
                                                                                </td>
                                                                                <td align="left" valign="middle">
                                                                                    <asp:DropDownList ID="ddlCompaign" runat="server" CssClass="dropDownList" AutoPostBack="true"
                                                                                        OnSelectedIndexChanged="ddlCompaign_SelectedIndexChanged">
                                                                                        <%--<asp:ListItem Text="Circulation Newspaper" Value="Circulation Newspaper"></asp:ListItem>--%>
                                                                                    </asp:DropDownList>
                                                                                    &nbsp;<asp:CompareValidator ID="cmpCompaign" runat="server" ControlToValidate="ddlCompaign"
                                                                                        ErrorMessage="Please select a campaign" ValueToCompare="0" Operator="GreaterThan"
                                                                                        Type="Integer" Text="*" Display="static" SetFocusOnError="true"></asp:CompareValidator>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" valign="middle" nowrap>
                                                                                    <b>Phone Number to Dial</b>&nbsp;:&nbsp;&nbsp;
                                                                                </td>
                                                                                <td align="left" valign="middle" nowrap>
                                                                                    <asp:TextBox ID="txtPhoneNumber" runat="server" CssClass="txtnormal"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                                                                        ID="reqPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" Text="*"
                                                                                        ErrorMessage="Please enter phone number" Display="static" SetFocusOnError="true"></asp:RequiredFieldValidator><%--<asp:CompareValidator
                                                                                            ID="cmpPhoneNumber" runat="server" ControlToValidate="txtPhoneNumber" Operator="dataTypeCheck"
                                                                                            Type="double" SetFocusOnError="true" Display="static" ErrorMessage="Enter numeric values">*</asp:CompareValidator>--%>
                                                                                    <asp:ValidationSummary ID="valsumManualDial" runat="server" ShowMessageBox="true"
                                                                                        ShowSummary="false" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" colspan="2">
                                                                                    <asp:CheckBox ID="chkLookup" runat="server" />Record Lookup Only&nbsp;&nbsp;&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td align="right" colspan="2" valign="middle">
                                                                                    <asp:LinkButton ID="lbtnDial" runat="server" OnClick="lbtnDial_Click"><img src="../images/Dial.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                                        ID="lbtnLookUp" runat="server" CausesValidation="false" OnClick="lbtnLookUp_Click"
                                                                                        Visible="false"><img src="../images/Lookup.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                                                            ID="lbtnCancel" runat="server" CausesValidation="false" OnClick="lbtnCancel_Click"><img src="../images/Cancel.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;&nbsp;<%--<asp:LinkButton
                                                                                                ID="lbtnClose" OnClick="lbtnclose_Click" runat="server" CausesValidation="false"><img src="../images/close.jpg" border="0" /></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;--%>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlPause" runat="server" Visible="false">
                                                            <!-- Progress -->
                                                            <table cellpadding="0" cellspacing="1" border="0" width="200px" height="50px" class="tdHeader">
                                                                <tr>
                                                                    <td>
                                                                        <table cellpadding="4" cellspacing="0" border="0" width="100%" height="50px" class="tdWhite">
                                                                            <tr>
                                                                                <td class="tdPause" align="center" valign="middle">
                                                                                    Paused
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <!-- Progress -->
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlScript" runat="server" Visible="false">
                                                            <table cellpadding="4" cellspacing="1" border="0" width="100%" class="tdWhite">
                                                                <tr>
                                                                    <td width="90%" align="left" valign="top">
                                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                            <tr>
                                                                                <td class="tdWaiting" align="left">
                                                                                    <div id="divScriptHeader">
                                                                                        <asp:Literal ID="ltrlScripthdr" runat="server"></asp:Literal>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <table cellpadding="4" cellspacing="0" border="0" width="100%">
                                                                            <tr>
                                                                                <td class="tdWaiting" align="left">
                                                                                    <div id="divScriptBody">
                                                                                        <asp:Literal ID="ltrlScriptbody" runat="server"></asp:Literal>
                                                                                    </div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left">
                                                                        <table cellpadding="4" cellspacing="0" border="0" width="100%">
                                                                            <tr>
                                                                                <td height="1px">
                                                                                    <img src="../images/spacer.gif" height="20px">
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                            <td width="10%" class="tdSideBar" valign="top">
                                                <asp:UpdatePanel ID="updatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <asp:HiddenField ID="hdnIsDial" runat="server" />
                                                        <asp:HiddenField ID="hdnPageFrom" runat="server" />
                                                        <asp:HiddenField ID="hdnDispose" runat="server" />
                                                        <div id="floatingsidebar1" class="floatingbuttons">
                                                            <asp:Panel runat="server" ID="pnlToolbar">
                                                                <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lbtnReady" Enabled="false" OnClick="lbtnReady_Click" CausesValidation="false"
                                                                                runat="server"><img src="../images/ready.jpg" alt="Ready" border="0" /></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lbtnPause" Enabled="false" OnClick="lbtnPause_Click" CausesValidation="false"
                                                                                runat="server"><img src="../images/pause_grey.jpg" alt="Pause" border="0" /></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <%--  <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="lbtnCallBack" Enabled="false" runat="server"><img src="../images/callback.jpg" alt="callback" border="0" /></asp:LinkButton></td>
                                                            </tr>--%>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lbtnSchedule" Enabled="false" runat="server"><img src="../images/schedule_grey.jpg" alt="schedule" id="imgSchedule" border="0" /></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lbtnHangup" Enabled="false" runat="server" OnClick="lbtnHangup_Click"
                                                                                OnClientClick="return HangupCall();"><img src="../images/hangup.jpg" alt="hangup" border="0" id="imgHangup"/></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lbtnDispose" Enabled="false" runat="server"><img src="../images/dispose.jpg" alt="dispose" border="0" /></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lbtnlogoff" OnClick="lbtnLogoff_Click" CausesValidation="false"
                                                                                runat="server"><img src="../images/logoff.jpg" border="0" /></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lbtnclose" OnClick="lbtnclose_Click" CausesValidation="false"
                                                                                runat="server"><img src="../images/close.jpg" border="0" /></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </div>
                                                        <div id="floatingsidebar2" class="floatingbuttons">
                                                            <asp:Panel runat="server" ID="pnlLookupButtons" Visible="false">
                                                                <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lbtnScheduleLookup" runat="server">LinkButton</asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lbtnSave" CausesValidation="false" runat="server" OnClientClick="return saveCampaign(null,null)"
                                                                                OnClick="lbtnSave_Click"><img src="../images/save.jpg" alt="save" border="0" /></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lbtnDisposeLookup" Enabled="false" runat="server"><img src="../images/dispose.jpg" alt="dispose" border="0" /></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:LinkButton ID="lbtnCloseLookup" OnClick="lbtnclose_Click" CausesValidation="false"
                                                                                runat="server"><img src="../images/close.jpg" border="0" /></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                        </div>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <!-- Body -->
                                <!-- Footer -->
                                <div id="pagefooter">
                                    <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                                </div>
                                <!-- Footer -->
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
    <div id="validationDialog">
    </div>
    <div id="dispositionDialog">
        <select id="dispositions" name="dispositions" size="10">
            <asp:Literal ID="dispositionOptions" runat="server" />
        </select>
    </div>
</body>
</html>
