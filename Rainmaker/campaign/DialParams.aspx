<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DialParams.aspx.cs" Inherits="Rainmaker.Web.campaign.DialParams" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Dialing Parameters</title>

    <script language="javascript" type="text/javascript">
    
        var cbrowser = navigator.userAgent;
        if (cbrowser.toLowerCase().indexOf("firefox") > 0) {
            document.write('<link rel="stylesheet" href="../css/RainmakerNS.css" type="text/css" />');
        } else {
            document.write('<link rel="stylesheet" href="../css/Rainmaker.css" type="text/css" />');
        }
        
    </script>

    <script language="javascript" type="text/javascript" src="../js/RainMaker.js"></script>
    
    <script language="javascript" type="text/javascript" src="../js/jquery.js"></script>

    <script language="javascript" type="text/javascript">
        function CheckDialingMode() {
            var dialingMode = document.form1.ddlDialingMode.options[document.form1.ddlDialingMode.selectedIndex].text;
            var dialingModeValue = document.form1.ddlDialingMode.value;
            if (dialingModeValue == "2" || dialingModeValue == "3") {
                alert("'" + dialingMode + "' feature not implemented.");
                document.form1.ddlDialingMode.selectedIndex = 0;
            }
        }
       
    </script>
    
         <link rel="stylesheet" href="http://code.jquery.com/ui/1.9.1/themes/base/jquery-ui.css" />
     
     <script language="javascript" type="text/javascript" src="../js/jquery-ui-1.9.1.custom.min.js"></script>
    <link rel="stylesheet" href="../css/jqueryuidialogstyle.css" />
    
    <script language="javascript" type="text/javascript">
        function setDialogCode() {
            //<span class="ui-icon" style="display:block"><img src="../images/Phone.jpg" id="myNewImage" /></span>
            $("#dialog-modal").dialog({
                title: 'Rainmaker Dialer Systems',
                autoOpen: false,
                height: 290,
                width: 340,
                modal: true
            });

            $("#lbtnPlayMachine").click(function() {
                if (!document.getElementById('chkAnswerMachine').checked)
                    return false;

                var filetoopen = OpenAudioFile('hdnPlayAudioFile', 'FileUploadMachineToPlay', 'Select Message to Play', 'hdnServerPath');

                if (filetoopen == "error") {
                    return false;
                }
                if (filetoopen != "Select Message to Play") {
                    //var thehtml;
                    //$.get("AudioPlay.htm", function(data) {

                    //$("#Player").filename = filetoopen;

                    //$("#Player").data = filetoopen;
                    //thehtml = data.replace(/#Audio#/g, filetoopen);
                    //$("#dialog-modal").html(thehtml);
                    filetoopen = filetoopen.replace("\\\\\\", "\\\\");
                    Player.url = filetoopen;
                    Player.src = filetoopen;
                    alert(filetoopen);


                    var modalhtml = $("#audiotitle").html();

                    modalhtml = modalhtml.replace("#Audio#", $("#lblMessage").text());
                    $("#audiotitle").html(modalhtml);
                    $("#dialog-modal").dialog("open");


                    //});
                }
                //$("#dialog-modal").load('AudioPlay.htm', function() {
                //    $("#dialog-modal").dialog("open");
                //});

                return false;
            });

        }
        function disableRadioButtons() {

            $("#lbtnUploadMachine").hide();
            $("#lbtnUploadHuman").hide();
            $("#lbtnUploadDrop").hide();
            
            if (!document.getElementById('chkAnswerMachine').checked) {

                $("#FileUploadMachineToPlay").attr("disabled", true);
                document.getElementById('lblMessage').innerHTML = "";

            }
            else {

                if ($("#lblMessage").text() != "") {
                    
                    $("#FileUploadMachineToPlay").removeAttr("disabled");
                } else {
                    
                    $("#FileUploadMachineToPlay").removeAttr("disabled");
                    $("#FileUploadMachineToPlay").show();
                    
                }

            }

            if (!document.getElementById('chkHumanMessageEnable').checked) {

                $("#FileUploadHumanToPlay").attr("disabled", true);
                document.getElementById('lblMessageH').innerHTML = "";
                

                

            }
            else {

                if ($("#lblMessageH").text() != "") {

                    $("#FileUploadHumanToPlay").removeAttr("disabled");
                } else {

                    $("#FileUploadHumanToPlay").removeAttr("disabled");
                    $("#FileUploadHumanToPlay").show();

                }
            }

            if (!document.getElementById('chkSilentCallMessageEnable').checked) {

                $("#FileUploadSilentCallToPlay").attr("disabled", true);
                document.getElementById('lblMessageS').innerHTML = "";

            }
            else {

                if ($("#lblMessageH").text() != "") {

                    $("#FileUploadSilentCallToPlay").removeAttr("disabled");
                } else {

                    $("#FileUploadSilentCallToPlay").removeAttr("disabled");
                    $("#FileUploadSilentCallToPlay").show();

                }
            }
        }
        
    </script>
     
</head>
<body onload="ShowPageMessage(); CheckDropRate();">
    <!-- <div id="dialog-modal"  name="dialog-modal" style="position:absolute;left:-1000px;top:-1000px" title="Rainmaker Dialing Systems"> -->
    
        <div id="dialog-modal" title="Rainmaker Dialing Systems" style="display:none;">
    
            <table cellpadding="0" border="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                    <p id="audiotitle">Playing file: &nbsp; #Audio#</p>
                    </td>
                </tr>
                <tr>
                    <td width="100%">
                     <script language="javascript" type="text/javascript">

                         if (-1 != navigator.userAgent.indexOf("MSIE")) {

                             document.write('<OBJECT id="Player"');
                             document.write(' classid="clsid:6BF52A52-394A-11d3-B153-00C04F79FAA6"');
                             document.write(' width=300 height=200>');
                            document.write(' </OBJECT>');
                         }
                         else if (-1 != navigator.userAgent.indexOf("Firefox")) {
                         var firefoxobject = '<embed id="Player" width="300" height="300" stretchtofit="1" playcount="1" rate="1" volume="100" displaysize="1" showtracker="1" showpositioncontrols="1" animationatstart="1" transparentatstart="1" autosize="1" showdisplay="1" autorewind="1" autostart="1" showstatusbar="1" showcontrols="1" name="WMP" pluginspage="http://www.microsoft.com/Windows/MediaPlayer/" type="application/x-mplayer2" /> ';
                             document.write(firefoxobject);
                         }         
                     
                     </script>
                    </td>
                </tr>
            </table>
        </div>
    
    <form id="form1" runat="server" defaultbutton="lbtnSave" defaultfocus="ddlPhoneCount">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>

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
                                    <table cellpadding="0" cellspacing="0" height="400px" border="0" width="100%">
                                        <tr>
                                            <td valign="bottom" align="right">
                                                <asp:LinkButton ID="LinkButton1" OnClick="lbtnSave_Click" runat="server" CssClass="button blue small">Save</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                    ID="LinkButton2" OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"
                                                    OnClick="lbtnCancel_Click" CausesValidation="false" runat="server" CssClass="button blue small">Cancel</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                        ID="LinkButton3" PostBackUrl="~/campaign/home.aspx" CausesValidation="false"
                                                        runat="server" CssClass="button blue small">Close</asp:LinkButton>
      
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                <table cellpadding="2" cellspacing="0" border="0" width="100%">
                                                    <tr>
                                                        <td align="left" width="100%" colspan="2">
                                                            <table cellpadding="2" cellspacing="1" width="100%" border="0">
                                                                <tr>
                                                                    <td valign="middle" width="35%" align="left">
                                                                        <a href="Home.aspx" class="aHome" runat="server" id="anchHome">Campaign</a>&nbsp;&nbsp;<img
                                                                            src="../images/arrowright.gif">&nbsp;&nbsp;<b>Dialing Parameters</b>
                                                                        <asp:HiddenField ID="hdnValidate" runat="server" />
                                                                        <asp:HiddenField ID="hdnServerPath" runat="server" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" width="50%">
                                                            <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                                                <tr>
                                                                    <td width="50%" align="right">
                                                                        <b>Phone Line Count&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlPhoneCount" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        &nbsp;<asp:RequiredFieldValidator ID="reqPLCount" runat="server" SetFocusOnError="true"
                                                                            ControlToValidate="ddlPhoneCount" ErrorMessage="Select Phone Line Count" Text="*"
                                                                            Display="Static"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>Drop Rate Percentage&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlDropRate" runat="server" CssClass="select1" onchange="CheckDropRate();">
                                                                        </asp:DropDownList>
                                                                        &nbsp;<asp:RequiredFieldValidator ID="reqDRPercentage" runat="server" SetFocusOnError="true"
                                                                            ControlToValidate="ddlDropRate" ErrorMessage="Select Drop Rate Percentage" Text="*"
                                                                            Display="Static"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr id="trDroprateMessage" valign="top">
                                                                    <td style="color: Red" colspan="2" valign="top">
                                                                        &nbsp;&nbsp;&nbsp;(FTC requires you drop no more than 3% of your calls, are you
                                                                        sure?)
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>Seconds to Ring Calls&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlRingSeconds" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        &nbsp;<asp:RequiredFieldValidator ID="reqRingSeconds" runat="server" SetFocusOnError="true"
                                                                            ControlToValidate="ddlRingSeconds" ErrorMessage="Seconds to Ring Calls" Text="*"
                                                                            Display="Static"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>Force Dialing to Delay Between Calls (secs)&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlAnalyzeDelayFreq" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        &nbsp;<%--<asp:RequiredFieldValidator ID="reqADF" runat="server" SetFocusOnError="true"
                                                                                ControlToValidate="ddlAnalyzeDelayFreq" ErrorMessage="Select Analyze Delay Freq.(in seconds)"
                                                                                Text="*" Display="Static"></asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>Dialing Mode&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlDialingMode" runat="server" CssClass="dropDownList" OnSelectedIndexChanged="ddlDialingMode_Change"
                                                                            AutoPostBack="true">
                                                                            <asp:ListItem Text="Outbound" Value="1"></asp:ListItem>
                                                                            <%--<asp:ListItem Text="Inbound/Outbound" Value="2"></asp:ListItem>
                                                                                <asp:ListItem Text="Inbound Only" Value="3"></asp:ListItem>--%>
                                                                            <asp:ListItem Text="Power Dial" Value="4"></asp:ListItem>
                                                                            <asp:ListItem Text="Manual Dial" Value="5"></asp:ListItem>
                                                                            <asp:ListItem Text="Unmanned" Value="6"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="middle" style="font-weight: bold;" colspan="2">
                                                                        &nbsp;&nbsp;<asp:CheckBox ID="chkAnswerMachine" Checked="true" runat="server" Text="Answering Machine Detection"
                                                                            onclick="disableRadioButtons();" />
                                                                    </td>
                                                                </tr>
                                                                <%-- COmmented out by GW 10.17.10 to remove quick and complete AMD, which no longer affect the dialer
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <img src="../images/spacer.gif" width="40px" height="1px">
                                                                            <asp:RadioButton ID="rbtnQuick" GroupName="rdgp1" runat="server" CausesValidation="false"
                                                                                Checked="true" Text="Quick" />
                                                                            &nbsp;&nbsp;<asp:RadioButton ID="rbtnComplete" GroupName="rdgp1" runat="server" Text="Complete" />
                                                                        </td>
                                                                    </tr>--%>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table cellpadding="0" cellspacing="1" border="0" width="95%" align="right" class="tdHeader">
                                                                            <tr>
                                                                                <td>
                                                                                    <table cellpadding="2" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                                        <tr>
                                                                                            <td align="left" class="tdSetting" valign="middle">
                                                                                                Message to Play&nbsp;<asp:Label ID="lblMessage" name="lblMessage" runat="server" onchange="disableRadioButtons();"></asp:Label>
                                                                                                <asp:HiddenField ID="hdnPlayAudioFile" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:FileUpload ID="FileUploadMachineToPlay" onchange="fileChange('FileUploadMachineToPlay','wav','Please select a sound file..');"
                                                                                                    runat="server" CssClass="fileUpload" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:LinkButton ID="lbtnUploadMachine" runat="server" CausesValidation="False" OnClick="lbtnUploadMachine_Click"
                                                                                                    ToolTip="Upload selected file to server and save."><img src="" class="myButton" alt="Upload" border="0"/></asp:LinkButton>&nbsp;&nbsp;
                                                                                                &nbsp;&nbsp;
                                                                                                
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="middle" style="font-weight: bold;" colspan="2">
                                                                        &nbsp;&nbsp;<asp:CheckBox ID="chkHumanMessageEnable" runat="server" Text="Play Message On Human Detection"
                                                                            onclick="disableRadioButtons();" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table cellpadding="0" cellspacing="1" border="0" width="95%" align="right" class="tdHeader">
                                                                            <tr>
                                                                                <td>
                                                                                    <table cellpadding="2" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                                        <tr>
                                                                                            <td align="left" class="tdSetting" valign="middle">
                                                                                                Message to Play&nbsp;<asp:Label ID="lblMessageH" runat="server"></asp:Label>
                                                                                                <asp:HiddenField ID="hdnPlayAudioFileH" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:FileUpload ID="FileUploadHumanToPlay" onchange="fileChange('FileUploadHumanToPlay','wav','Please select a sound file..');"
                                                                                                    runat="server" CssClass="fileUpload" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:LinkButton ID="lbtnUploadHuman" runat="server" CausesValidation="false" OnClick="lbtnUploadHuman_Click"
                                                                                                    ToolTip="Upload selected file to server and save."><img src="" class="myButton" alt="Upload" border="0" /></asp:LinkButton>&nbsp;&nbsp;
                                                                                                &nbsp;&nbsp;
                                                                                                &nbsp;
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="left" valign="middle" style="font-weight: bold;" colspan="2">
                                                                        &nbsp;&nbsp;<asp:CheckBox ID="chkSilentCallMessageEnable" runat="server" Text="Dropped Calls Message"
                                                                            onclick="disableRadioButtons();" />
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        <table cellpadding="0" cellspacing="1" border="0" width="95%" align="right" class="tdHeader">
                                                                            <tr>
                                                                                <td>
                                                                                    <table cellpadding="2" cellspacing="0" border="0" width="100%" class="tdWhite">
                                                                                        <tr>
                                                                                            <td align="left" class="tdSetting" valign="middle">
                                                                                                Message to Play&nbsp;<asp:Label ID="lblMessageS" runat="server"></asp:Label>
                                                                                                <asp:HiddenField ID="hdnPlayAudioFileS" runat="server" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:FileUpload ID="FileUploadSilentCallToPlay" onchange="fileChange('FileUploadSilentCallToPlay','wav','Please select a sound file..');"
                                                                                                    runat="server" CssClass="fileUpload" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="center">
                                                                                                <asp:LinkButton ID="lbtnUploadDrop" runat="server" CausesValidation="false" OnClick="lbtnUploadDrop_Click"
                                                                                                    ToolTip="Upload selected file to server and save."><img src="" class="myButton" alt="Upload" border="0" /></asp:LinkButton>&nbsp;&nbsp;
                                                                                                &nbsp;&nbsp;
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
                                                        <td valign="top" width="50%">
                                                            <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>Lines to Dial per Agent&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="dddInitDials" runat="server" CssClass="select1" Width="48px">
                                                                            <asp:ListItem Text="1" Value="1.00"></asp:ListItem>
                                                                            <asp:ListItem Text="1.5" Value="1.50"></asp:ListItem>
                                                                            <asp:ListItem Text="2" Value="2.00" Selected="True"></asp:ListItem>
                                                                            <asp:ListItem Text="2.5" Value="2.50"></asp:ListItem>
                                                                            <asp:ListItem Text="3" Value="3.00"></asp:ListItem>
                                                                            <asp:ListItem Text="3.5" Value="3.50"></asp:ListItem>
                                                                            <asp:ListItem Text="4" Value="4.00"></asp:ListItem>
                                                                            <asp:ListItem Text="4.5" Value="4.50"></asp:ListItem>
                                                                            <asp:ListItem Text="5" Value="5.00"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>Default Call Lapse(in seconds)&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlDefaultcallLapse" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        &nbsp;<asp:RequiredFieldValidator ID="reqDCL" runat="server" SetFocusOnError="true"
                                                                            ControlToValidate="ddlDefaultcallLapse" ErrorMessage="Select Default Call Lapse(in seconds)"
                                                                            Text="*" Display="Static"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>7 Digit Dialing Prefix&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txt7DigPrefix" runat="server" MaxLength="5" CssClass="txtmedium"></asp:TextBox>&nbsp;<%--<asp:RequiredFieldValidator
                                                                                ID="reqLocalDialingPrefix" runat="server" ControlToValidate="txtLDP" Text="*"
                                                                                ErrorMessage="Enter Local Dialing Prefix" Display="static" SetFocusOnError="true"></asp:RequiredFieldValidator>--%><asp:CompareValidator
                                                                                    ID="cmp7DigPrefix" runat="server" ControlToValidate="txt7DigPrefix" Type="integer"
                                                                                    Operator="DataTypeCheck" SetFocusOnError="true" ErrorMessage="Please enter valid prefix containing only numbers."
                                                                                    Display="static">*</asp:CompareValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>7 Digit Dialing Suffix&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txt7DigSuffix" runat="server" MaxLength="5" CssClass="txtmedium"></asp:TextBox>&nbsp;<%--<asp:RequiredFieldValidator
                                                                                ID="reqtxtILDP" runat="server" ControlToValidate="txtILDP" Text="*" ErrorMessage="Enter Intra Local Dialing Prefix"
                                                                                Display="static" SetFocusOnError="true"></asp:RequiredFieldValidator>--%><asp:CompareValidator
                                                                                    ID="cmp7DigSuffix" runat="server" ControlToValidate="txt7DigSuffix" Operator="datatypecheck"
                                                                                    Type="integer" SetFocusOnError="true" ErrorMessage="Please enter valid suffix containing only numbers."
                                                                                    Display="static">*</asp:CompareValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>10 Digit Dialing Prefix&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txt10DigPrefix" runat="server" MaxLength="5" CssClass="txtmedium"></asp:TextBox>&nbsp;<%--<asp:RequiredFieldValidator
                                                                                ID="reqLongDistanceDialingPrefix" runat="server" ControlToValidate="txtLDDP"
                                                                                Text="*" ErrorMessage="Enter Long Distance Dialing Prefix" Display="static" SetFocusOnError="true"></asp:RequiredFieldValidator>--%><asp:CompareValidator
                                                                                    ID="cmp10DigPrefix" runat="server" ControlToValidate="txt10DigPrefix" Operator="datatypecheck"
                                                                                    Type="integer" SetFocusOnError="true" ErrorMessage="lease enter valid prefix containing only numbers."
                                                                                    Display="static">*</asp:CompareValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>10 Digit Dialing Suffix&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:TextBox ID="txt10DigSuffix" runat="server" MaxLength="5" CssClass="txtmedium"></asp:TextBox>&nbsp;<%--<asp:RequiredFieldValidator
                                                                                ID="reqLongDistanceDialingPrefix" runat="server" ControlToValidate="txtLDDP"
                                                                                Text="*" ErrorMessage="Enter Long Distance Dialing Prefix" Display="static" SetFocusOnError="true"></asp:RequiredFieldValidator>--%><asp:CompareValidator
                                                                                    ID="cmp10DigSuffix" runat="server" ControlToValidate="txt10DigSuffix" Operator="datatypecheck"
                                                                                    Type="integer" SetFocusOnError="true" ErrorMessage="Please enter valid suffix containing only numbers."
                                                                                    Display="static">*</asp:CompareValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td width="60%" align="right">
                                                                        <b>Cold Call Script&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlCallScript" runat="server" CssClass="dropDownList">
                                                                        </asp:DropDownList>
                                                                        &nbsp;<asp:CompareValidator ID="cmpCCS" runat="server" SetFocusOnError="true" ControlToValidate="ddlCallScript"
                                                                            ErrorMessage="Select Cold Call Script" ValueToCompare="0" Operator="GreaterThan"
                                                                            Type="Integer" Text="*" Display="static"></asp:CompareValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>Verification Script&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlVerScript" runat="server" CssClass="dropDownList">
                                                                        </asp:DropDownList>
                                                                        &nbsp;<asp:CompareValidator ID="cmpVS" runat="server" SetFocusOnError="true" ControlToValidate="ddlVerScript"
                                                                            ErrorMessage="Select Verification Script" ValueToCompare="0" Operator="GreaterThan"
                                                                            Type="Integer" Text="*" Display="static"></asp:CompareValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>Inbound Script&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlInboundScript" runat="server" CssClass="dropDownList">
                                                                        </asp:DropDownList>
                                                                        &nbsp;<asp:CompareValidator ID="cmpIS" runat="server" SetFocusOnError="true" ControlToValidate="ddlInboundScript"
                                                                            ErrorMessage="Select Inbound Script" ValueToCompare="0" Operator="GreaterThan"
                                                                            Type="Integer" Text="*" Display="static"></asp:CompareValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>AM Call # Times&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlAMCall" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        &nbsp;<%--<asp:RequiredFieldValidator ID="reqAMCT" runat="server" ControlToValidate="ddlAMCall"
                                                                                ErrorMessage="Select AM Call # Times" Text="*" Display="Static"></asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>PM Call # Times&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlPMCall" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        &nbsp;<%--<asp:RequiredFieldValidator ID="reqPMCT" runat="server" ControlToValidate="ddlPMCall"
                                                                                ErrorMessage="Select PM Call # Times" Text="*" Display="Static"></asp:RequiredFieldValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <b>Weekend Call # Times&nbsp;:&nbsp;&nbsp;</b>
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlWCall" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        &nbsp;<asp:RequiredFieldValidator ID="reqWCT" runat="server" SetFocusOnError="true"
                                                                            ControlToValidate="ddlWCall" ErrorMessage="Select Weekend Call # Times" Text="*"
                                                                            Display="Static"></asp:RequiredFieldValidator>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="LBL_AMStartTime" runat="server" Text="AM Dialing Start Time : " Visible="False"
                                                                            Font-Bold="True" />&nbsp;
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlAMDailingHrs" runat="server" CssClass="select1" Visible="False">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlAMDailingMinutes" runat="server" CssClass="select1" Visible="False">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlAMDailing" runat="server" CssClass="select1" Visible="False">
                                                                            <asp:ListItem Text="AM"></asp:ListItem>
                                                                            <asp:ListItem Text="PM"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        &nbsp;&nbsp;
                                                                        <%--&nbsp;<asp:CompareValidator ID="cmpAMDailingST" runat="server" ControlToValidate="ddlAMDailingST"
                                                                                ErrorMessage="Select AM Dialing Start Time" ValueToCompare="0" Operator="GreaterThan"
                                                                                Type="Integer" Text="*" Display="static"></asp:CompareValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="LBL_AMStopTime" runat="server" Text="AM Dialing Stop Time : " Visible="False"
                                                                            Font-Bold="True" />&nbsp;
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlAMDailingSTHrs" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlAMDailingSTMinutes" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlAMDailingST" runat="server" CssClass="select1">
                                                                            <asp:ListItem Text="AM"></asp:ListItem>
                                                                            <asp:ListItem Text="PM"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        &nbsp;&nbsp;
                                                                        <%--&nbsp;<asp:CompareValidator ID="cmpAMDailingST" runat="server" ControlToValidate="ddlAMDailingST"
                                                                                ErrorMessage="Select AM Dialing Start Time" ValueToCompare="0" Operator="GreaterThan"
                                                                                Type="Integer" Text="*" Display="static"></asp:CompareValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="LBL_PMStartTime" runat="server" Text="PM Dialing Start Time : " Visible="False"
                                                                            Font-Bold="True" />&nbsp;
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlPMDailingHrs" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlPMDailingMinutes" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlPMDailing" runat="server" CssClass="select1">
                                                                            <asp:ListItem Text="AM"></asp:ListItem>
                                                                            <asp:ListItem Text="PM"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        &nbsp;&nbsp;<%--&nbsp;<asp:CompareValidator ID="cmpPMDailingST" runat="server" ControlToValidate="ddlPMDailingST"
                                                                                ErrorMessage="Select PM Dialing Start Time" ValueToCompare="0" Operator="GreaterThan"
                                                                                Type="string"  Text="*" Display="static"></asp:CompareValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td align="right">
                                                                        <asp:Label ID="LBL_PMStopTime" runat="server" Text="PM Dialing Stop Time : " Visible="False"
                                                                            Font-Bold="True" />&nbsp;
                                                                    </td>
                                                                    <td align="left">
                                                                        <asp:DropDownList ID="ddlPMDailingSTHrs" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlPMDailingSTMinutes" runat="server" CssClass="select1">
                                                                        </asp:DropDownList>
                                                                        <asp:DropDownList ID="ddlPMDailingST" runat="server" CssClass="select1">
                                                                            <asp:ListItem Text="AM"></asp:ListItem>
                                                                            <asp:ListItem Text="PM"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        &nbsp;&nbsp;<%--&nbsp;<asp:CompareValidator ID="cmpPMDailingET" runat="server" ControlToValidate="ddlPMDailingET"
                                                                                ErrorMessage="Select PM Dialing Stop Time" ValueToCompare="0" Operator="GreaterThan"
                                                                                Type="Integer" Text="*" Display="static"></asp:CompareValidator>--%>
                                                                    </td>
                                                                </tr>
                                                                <%--<td align="right">
                                                                            <b>Local Dialing Suffix&nbsp;:&nbsp;&nbsp;</b></td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtLDS" runat="server" MaxLength="5" CssClass="txtmedium"></asp:TextBox>&nbsp;<%--<asp:RequiredFieldValidator
                                                                                ID="reqLocalDialingSuffix" runat="server" ControlToValidate="txtLDS" Text="*"
                                                                                ErrorMessage="Enter Local Dialing Suffix" Display="static" SetFocusOnError="true"></asp:RequiredFieldValidator><asp:CompareValidator
                                                                                    ID="cmpLocalDialingSuffix" runat="server" ControlToValidate="txtLDS" Operator="datatypecheck"
                                                                                    Type="integer" SetFocusOnError="true" ErrorMessage="Please enter valid suffix"
                                                                                    Display="static">*</asp:CompareValidator>
                                                                        </td>--%>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <%--<td align="right">
                                                                            <b>Intra Lata Dialing Suffix&nbsp;:&nbsp;&nbsp;</b></td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtILDS" runat="server" MaxLength="5" CssClass="txtmedium"></asp:TextBox>&nbsp;<%--<asp:RequiredFieldValidator
                                                                                ID="reqIntraLataDialingSuffix" runat="server" ControlToValidate="txtILDS" Text="*"
                                                                                ErrorMessage="Enter Intra Lata Dialing Suffix" Display="static" SetFocusOnError="true"></asp:RequiredFieldValidator><asp:CompareValidator
                                                                                    ID="cmpIntraLataDialingSuffix" runat="server" ControlToValidate="txtILDS" Operator="datatypecheck"
                                                                                    Type="integer" SetFocusOnError="true" ErrorMessage="Please enter valid suffix"
                                                                                    Display="static">*</asp:CompareValidator>
                                                                        </td>--%>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <%--<td align="right">
                                                                            <b>Long Distance Dialing Suffix&nbsp;:&nbsp;&nbsp;</b></td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtLDDS" runat="server" MaxLength="5" CssClass="txtmedium"></asp:TextBox>&nbsp;<%--<asp:RequiredFieldValidator
                                                                                ID="reqLongDistanceDialingSuffix" runat="server" ControlToValidate="txtLDDS"
                                                                                Text="*" ErrorMessage="Enter Long Distance Dialing Suffix" Display="static" SetFocusOnError="true"></asp:RequiredFieldValidator><asp:CompareValidator
                                                                                    ID="cmpLongDistanceDialingSuffix" runat="server" ControlToValidate="txtLDDS"
                                                                                    Operator="datatypecheck" Type="integer" SetFocusOnError="true" ErrorMessage="Please enter valid suffix"
                                                                                    Display="static">*</asp:CompareValidator>
                                                                        </td>--%>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <%--<td align="right">
                                                                            <b>Error Redial Lapse(in minutes)&nbsp;:&nbsp;&nbsp;</b></td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtErrorRedialLapse" runat="server" MaxLength="5" CssClass="txtmini"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                                                                ID="reqtxtErrorRedialLapse" runat="server" ControlToValidate="txtErrorRedialLapse"
                                                                                Text="*" ErrorMessage="Enter Error Redial Lapse(in minutes)" Display="static"
                                                                                SetFocusOnError="true"></asp:RequiredFieldValidator><asp:CompareValidator ID="cmptxtErrorRedialLapse"
                                                                                    runat="server" ControlToValidate="txtErrorRedialLapse" Operator="datatypecheck"
                                                                                    Type="integer" SetFocusOnError="true" ErrorMessage="Please enter valid Error Redial Lapse(in minutes)"
                                                                                    Display="static">*</asp:CompareValidator>
                                                                        </td>--%>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <%-- <td align="right">
                                                                            <b>Busy Redial Lapse(in minutes)&nbsp;:&nbsp;&nbsp;</b></td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtBusyRedialLapse" runat="server" MaxLength="5" CssClass="txtmini"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                                                                ID="reqtxtBusyRedialLapse" runat="server" ControlToValidate="txtBusyRedialLapse"
                                                                                Text="*" ErrorMessage="Enter Busy Redial Lapse(in minutes)" Display="static"
                                                                                SetFocusOnError="true"></asp:RequiredFieldValidator><asp:CompareValidator ID="cmptxtBusyRedialLapse"
                                                                                    runat="server" ControlToValidate="txtBusyRedialLapse" Operator="datatypecheck"
                                                                                    Type="integer" SetFocusOnError="true" ErrorMessage="Please enter valid Busy Redial Lapse(in minutes)"
                                                                                    Display="static">*</asp:CompareValidator>
                                                                        </td>--%>
                                                    </tr>
                                                    <tr style="display: none">
                                                        <%--<td align="right">
                                                                            <b>No Answer Redial Lapse(in minutes)&nbsp;:&nbsp;&nbsp;</b></td>
                                                                        <td align="left">
                                                                            <asp:TextBox ID="txtNoAnswerRedialLapse" runat="server" MaxLength="5" CssClass="txtmini"></asp:TextBox>&nbsp;<asp:RequiredFieldValidator
                                                                                ID="reqNoAnswerRedialLapse" runat="server" ControlToValidate="txtNoAnswerRedialLapse"
                                                                                Text="*" ErrorMessage="Enter NoAnswer Redial Lapse(in minutes)" Display="static"
                                                                                SetFocusOnError="true"></asp:RequiredFieldValidator><asp:CompareValidator ID="cmpNoAnswerRedialLapse"
                                                                                    runat="server" ControlToValidate="txtNoAnswerRedialLapse" Operator="datatypecheck"
                                                                                    Type="integer" SetFocusOnError="true" ErrorMessage="Please enter valid NoAnswer Redial Lapse(in minutes)"
                                                                                    Display="static">*</asp:CompareValidator>
                                                                        </td>--%>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr valign="bottom">
                                <td valign="bottom" align="right" colspan="2">
                                    <table cellpadding="4" cellspacing="1" border="0" width="100%">
                                        <tr>
                                            <td valign="bottom" align="right">
                                                <asp:LinkButton ID="lbtnSave" OnClick="lbtnSave_Click" runat="server" CssClass="button blue small">Save</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                    ID="lbtnCancel" OnClientClick="javascript:return confirm('Do you want to cancel the changes?');"
                                                    OnClick="lbtnCancel_Click" CausesValidation="false" runat="server" CssClass="button blue small">Cancel</asp:LinkButton>&nbsp;&nbsp;<asp:LinkButton
                                                        ID="lbtnClose" PostBackUrl="~/campaign/home.aspx" CausesValidation="false" runat="server" CssClass="button blue small">Close</asp:LinkButton>
                                                <asp:ValidationSummary ID="valsumDialParams" runat="server" ShowMessageBox="true"
                                                    ShowSummary="false" />
                                                <asp:HiddenField ID="hdnMachineFile" runat="server" />
                                                <asp:HiddenField ID="hdnDropFile" runat="server" />
                                                <asp:HiddenField ID="hdnHumanFile" runat="server" />
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
            </td> </tr> </table>
        </div>
        <div style="height:800px">divdivdiv</div>
    </form>

    <script language="javascript" type="text/javascript">


        /* function ValidateData() {
        if (!document.getElementById('chkAnswerMachine').checked
        || (document.getElementById('FileUploadMachineToPlay').value == "")) {
        if (document.getElementById('ddlDialingMode').value == "1" || document.getElementById('ddlDialingMode').value == "6" || document.getElementById('ddlDialingMode').value == "4") {
        if (!confirm('FTC requires a message be played for Answering Machines. \nAre you sure you wish to change?')) {
        return false;
        }
        }
        }
        if (!document.getElementById('chkHumanMessageEnable').checked
        || (document.getElementById('FileUploadHumanToPlay').value == "")) {
        if (document.getElementById('ddlDialingMode').value == "1" || document.getElementById('ddlDialingMode').value == "6" || document.getElementById('ddlDialingMode').value == "4") {
        if (!confirm('FTC requires message identifying caller within 2 seconds, \nAre you sure you wish to turn off?')) {
        return false;
        }
        }
        }
        if (!document.getElementById('chkSilentCallMessageEnable').checked
        || (document.getElementById('FileUploadSilentCallToPlay').value == "")) {
        if (document.getElementById('ddlDialingMode').value == "1" || document.getElementById('ddlDialingMode').value == "6" || document.getElementById('ddlDialingMode').value == "4") {
        if (!confirm('FTC requires a message be played for ALL dropped calls. \nAre you sure you wish to change?')) {
        return false;
        }
        }
        } 
        }*/

        CheckDropRate();
        function CheckDropRate() {
            if (parseInt(document.getElementById('ddlDropRate').value, 10) > 3 && (document.getElementById('ddlDialingMode').value == "1" || document.getElementById('ddlDialingMode').value == "6")) {
                document.getElementById('trDroprateMessage').style.display = "";
            }
            else {
                document.getElementById('trDroprateMessage').style.display = "none";
            }
        }
    </script>

</body>
</html>
