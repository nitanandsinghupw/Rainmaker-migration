<%@ Page Language="C#" AutoEventWireup="true" Codebehind="WaitingforCall.aspx.cs"
    Inherits="Rainmaker.Web.agent.WaitingforCall" ValidateRequest="false"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Waiting for Call</title>
    
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
    
        var timerId =  setTimeout("UpdateProgressBar();",1000);
		var progress = 1;
		var strprogress = "";
		function UpdateProgressBar()
		{
			strprogress = strprogress + ".";
			progress=progress+1;
			if(progress >=30){
				progress = 1;
				strprogress = ".";
			}
			if(document.getElementById("tdProgressBar")!=null)
			{
			document.getElementById("tdProgressBar").innerHTML = strprogress
			timerId =  setTimeout("UpdateProgressBar();",1000);
			}
		}
		
		function GetOffsiteNumber()
        {
            window.showModalDialog('../campaign/GetTransferNum.aspx?'+ ( new Date() ).getTime(),'GetTransferNum','dialogWidth:455px;dialogHeight:155px;edge:Raised;center:Yes;resizable:No;status:No');
        }
		
        function Validate()
        {
            if(document.frmWaitingForCall.cboResultCode!=null)
            {
           
                if(document.frmWaitingForCall.cboResultCode.value!="-1")
                {
                    document.getElementById("hdnresultcode").value=document.frmWaitingForCall.cboResultCode.value;
                    return true;
                }
                else
                {
                    alert("Select Result Code"); 
                    return false;
                }
                return false;
            }
        }
        
        function LoadScript(scriptid){
            saveCampaign(null, scriptid);				  
		  }
        
        function RenderScript(scriptid){
            $('#hndScriptID').val('');
            var isHeader = "true";
            var isInvalidScript = false;
            var newDate = new Date();
	        var timeStamp = newDate.getMonth()+""+newDate.getDate()+""+newDate.getYear()
	                                    +""+newDate.getHours()+""+newDate.getMinutes()+""+newDate.getSeconds()+""+newDate.getMilliseconds();
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

         function saveCampaign(scriptaction, scriptid) 
         {
            isValid = true;

            var campaignCall = new CampaignCall
                (
                    new CampaignForm
                    (
                        {
                            fieldInfo: filledArray,
                            isScript: true,
                            pageName: "WaitingForCall",
                            $form: $("#frmWaitingForCall"),
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
                                ShowPopUp('Disposition', 'WaitingForCall');
                                break;

                            case "Schedule":
                                ShowPopUp('Schedule', 'WaitingForCall');
                                break;
                        }
                    }
                }
            });

            return isValid;
        }
		 
		function CheckRadioButton(id, value) {

		    
            //alert(id);
            var radio = document.getElementsByName(id);
            if(radio != null)
            {
                //alert(id+":"+value+":"+radio.length);
                for (var j = 0; j < radio.length; j++)
                {
                    if(radio[j].type == "radio" && radio[j].value.toLowerCase() == value.toLowerCase())
                    {
                        radio[j].checked = true;
                        break;
                    }    
                }
            }
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
         function SelectCheckbox(id)
         {
            //alert("Sel:"+id);
            if(document.getElementById(id)!=null) 
            {
                document.getElementById(id).checked = true;
            }
         }

     </script>
     
    <script language="javascript" type="text/javascript">
        function partialpostbacks() {

            var pagerequestmanager = Sys.WebForms.PageRequestManager.getInstance();
            pagerequestmanager.add_endRequest(EndRequestHandler);
            
            function EndRequestHandler(sender, args) {
                if (args.get_error() == undefined) {
                    jqueryfunctions();
                }
            }
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
                            pageName: "WaitingForCall",
                            $form: $("#frmWaitingForCall"),
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
        
         
        function HangupCall()
        {
            if(document.getElementById("lbtnHangup").disabled == false || document.getElementById("lbtnHangup").disabled==undefined) {
                    
                try
                {
                    document.getElementById("imgHangup").src = "../images/hangup_grey_red.jpg";
                    document.getElementById("lbtnHangup").disabled = true;
                    
                    document.getElementById("imgTransfer").src = "../images/transfer_grey.jpg";
                    document.getElementById("lbtnTransfer").disabled = true;
                }
                catch (e) { }
                
                var newDate = new Date();
                var timeStamp = newDate.getMonth()+""+newDate.getDate()+""+newDate.getYear()
                                            +""+newDate.getHours()+""+newDate.getMinutes()+""+newDate.getSeconds()+""+newDate.getMilliseconds();
                $.get("Hangup.aspx", {  
					ts:timeStamp
				},
				function(data){ 

					if(data == "failed") {
					
						return true;
					}
				}); 
            }
            return false;
        }

        function TrnsferCall()
        {
            if(document.getElementById("lbtnTransfer").disabled == false || document.getElementById("lbtnTransfer").disabled==undefined)
                return saveCampaign(null,null);
            else
                return false;
            }
            function jqueryfunctions() {
        
                $(document).ready(function() {
                    
                    $.ajaxSetup({ cache: false });
                    $('input[data-type=dtpicker]').each(function() {
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
    </script>
	<script type="text/javascript" src="../js/scriptEditor.js"></script>
    <script language="javascript" type="text/javascript">
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
    .floatingbuttons {
        position: fixed;
        top: 10px;
        left: 88%;
    }
    
    </style>

</head>

<body onload="ShowPageMessage();" onkeydown="doKey(event)">
    
    <form id="frmWaitingForCall" runat="server">
        <asp:ModalPopupExtender ID="MPE_Modal1" runat="server" 
            TargetControlID="HiddenField1" PopupControlID="pnlpopup" 
            BackgroundCssClass="modalBackground" />
        <asp:ModalPopupExtender ID="MPE_Modal2" runat="server" 
            TargetControlID="HiddenField2" PopupControlID="pnlCallback" Enabled="True" 
            BackgroundCssClass="modalBackground" />
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
        
        <asp:SqlDataSource ID="dsCallBack" runat="server"></asp:SqlDataSource>
        
        <asp:Timer ID="Timer1" OnTick="Timer1_Tick" runat="server">
        </asp:Timer>
        <asp:Timer ID="Timer2" OnTick="Timer2_Tick" runat="server">
        </asp:Timer>
        <%--<asp:Timer ID="TrainingTimer" OnTick="TrainingTimer_Tick" runat="server">
        </asp:Timer>--%>
        <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>
        <div>
            <table id="tblMain" cellpadding="0" cellspacing="1" border="0" width="100%" class="tdHeader">
                <tr>
                    <td>
                        <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                            <tr>
                                <td>
                                    <!-- Header -->
                                    <div style="height:210px">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%" style="height:210px">
                                        <tr>
                                            <td>
                                                <img src="../images/LeadSweepBannerLarge.jpg"><asp:HiddenField ID="hdnresultcode" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <center><label id="statuslabel" name="statuslabel"></label></center>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdHeader" height="1px">
                                                <img src="../images/spacer.gif" height="1px"></td>
                                        </tr>
                                    </table>
                                    </div>
                                    <!-- Header -->
                                    <!-- Body -->
                                    <div id="main">
                                    <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                        <tr>
                                            <td width="90%" align="center">
                                                <asp:HiddenField ID="HiddenField1" runat="server" />
												
                                                <asp:HiddenField ID="HiddenField2" runat="server" />
                                                <asp:HiddenField ID="hdnactionphone" runat="server" />
												
												<asp:HiddenField ID="hndScriptID" runat="server" />
                                                
                                                <asp:HiddenField ID="hdnScriptBody" runat="server" />
                                                <asp:Panel ID="pnlpopup" runat="server" Style="display:none">
                                                    <asp:UpdatePanel ID="UpdatePanelAdmin" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <asp:Panel ID="ModalPanel" runat="server" Width="349px" Height="126px" 
                                                                ForeColor="Black" BackColor="White" BorderStyle="Solid">
                                                                <br />
                                                 You have been reset by the administrator.  You will be redirected to the login page.<br />
                                                                <br />
                                                            <asp:Button ID="OKButton" runat="server" Text="Ok" onclick="OKButton_Click" 
                                                                    style="height: 26px" Height="22px" Width="61px" CausesValidation="False" />
                                                            </asp:Panel>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="OkButton" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </asp:Panel>
                                                <asp:Panel ID="pnlCallback" runat="server" Style="display:none;">
                                                    <asp:UpdatePanel ID="UpdatePanelCallback" runat="server" UpdateMode="Always">
                                                        <ContentTemplate>
                                                            <asp:HiddenField ID="hdnCallBackKey" runat="server" />
                                                            <asp:Panel ID="UpdatePanelCallBackInner" runat="server" Width="549px" Height="349px" 
                                                                ForeColor="Black" BackColor="White" BorderStyle="Solid">
                                                                <br />
                                                                <asp:Label ID="lblCallback" runat="server" 
                                                                    Text="You have a scheduled callback [datetime] for [number]"></asp:Label>
                                                                <br />
                                                                <br />
                                                                <asp:TextBox ID="txtCallbackNotes" runat="server" Height="207px" Width="418px" 
                                                                    TextMode="MultiLine"></asp:TextBox>
                                                                <br />
                                                                <br />
                                                                <br />
                                                            <asp:Button ID="btnCallbackDial" runat="server" Text="Dial" 
                                                                    style="height: 26px" Height="22px" Width="61px" CausesValidation="False" onclick="btnCallbackDial_Click" />
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnCallbackLookup" runat="server" Text="Lookup" 
                                                                    style="height: 26px" Height="22px" Width="61px" CausesValidation="False" onclick="btnCallbackLookup_Click" 
                                                                    />
                                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                            <asp:Button ID="btnCallbackCancel" runat="server" Text="Cancel" 
                                                                    style="height: 26px" Height="22px" Width="61px" CausesValidation="False" onclick="btnCallbackCancel_Click" 
                                                                     />
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
                                                        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />
                                                        <asp:AsyncPostBackTrigger ControlID="Timer2" EventName="Tick" />
                                                        <%--<asp:AsyncPostBackTrigger ControlID="TrainingTimer" EventName="Tick" />--%>
                                                    </Triggers>
                                                    <ContentTemplate>
                                                        
                                                        <asp:Panel ID="pnlWaitingforcall" runat="server">
                                                            <table cellpadding="0" cellspacing="1" border="0" width="200px" height="50px" class="tdHeader">
                                                                <tr>
                                                                    <td>
                                                                        <table cellpadding="4" cellspacing="0" border="0" width="100%" height="50px" class="tdWhite">
                                                                            <tr>
                                                                                <td class="tdWaiting" align="center" valign="top">
                                                                                    Waiting For Call</td>
                                                                            </tr>
                                                                            <tr valign="top">
                                                                                <td class="tdProgress" id="tdProgressBar" align="center" valign="top">
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
                                                                                    Paused</td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <!-- Progress -->
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlScript" runat="server">
                                                            <table cellpadding="4" cellspacing="1" border="0" width="100%" class="tdWhite">
                                                                <tr>
                                                                    <td width="90%" align="left" valign="top">
                                                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                                                            <tr>
                                                                                <td class="tdWaiting">
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
                                                                                <td class="tdWaiting">
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
                                                                                    <img src="../images/spacer.gif" height="20px"></td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                        <asp:Panel ID="pnlTrainingPage" runat="server">
                                                        <table cellpadding="0" cellspacing="5" border="0" width="100%" class="tdHeader">
                                                            <tr>
                                                                <td>
                                                                    <table cellpadding="4" cellspacing="1" border="0" width="100%" class="tdWhite">
                                                                        <tr>
                                                                            <td align="left">
                                                                                <table cellpadding="4" cellspacing="0" border="0" width="100%">
                                                                                    <tr>
                                                                                        <%--<td class="tdWaiting">--%>
                                                                                        <td>
                                                                                            <div id="divTrainingPageContent">
                                                                                                <asp:Literal ID="ltrTrainingPageContent" runat="server"></asp:Literal>
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
                                                                                            <img src="../images/spacer.gif" height="20px"></td>
                                                                                    </tr>
                                                                                </table>
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
                                                <div id="floatingsidebar1" class="floatingbuttons">
                                                <asp:UpdatePanel ID="updatePanel1" runat="server">
                                                    <ContentTemplate>
                                                        <table border="0" cellpadding="4" cellspacing="0" width="100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="lbtnReady" OnClick="lbtnReady_Click" runat="server"><img src="../images/ready.jpg" alt="Ready" border="0" /></asp:LinkButton></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="lbtnPause" OnClick="lbtnPause_Click" runat="server"><img src="../images/pause.jpg" alt="Pause" border="0" /></asp:LinkButton></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="lbtnSchedule" Enabled="false" runat="server"><img src="../images/schedule.jpg" alt="schedule" id="imgSchedule" border="0" /></asp:LinkButton></td>
                                                            </tr>
                                                            <tr id="trTransfer" runat="server">
                                                                <td>
                                                                    <asp:LinkButton ID="lbtnOffTransfer" runat="server" OnClientClick="return GetOffsiteNumber();"><img src="../images/transfer.jpg" alt="transfer" border="0" /></asp:LinkButton>
                                                                    <asp:LinkButton ID="lbtnTransfer" runat="server" OnClick="lbtnTransfer_Click"><img src="../images/transfer.jpg" alt="transfer" border="0" /></asp:LinkButton></td>
                                                            </tr>
                                                             <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="lbtnHangup" Enabled="false" runat="server" OnClick="lbtnHangup_Click" OnClientClick="return HangupCall();"><img src="../images/hangup.jpg" alt="hangup" border="0" id="imgHangup"/></asp:LinkButton></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="lbtnDispose" Enabled="false" runat="server"><img src="../images/dispose.jpg" alt="dispose" border="0" /></asp:LinkButton></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="lbtnlogoff" OnClick="lbtnLogoff_Click" runat="server"><img src="../images/logoff.jpg" border="0"/></asp:LinkButton></td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:LinkButton ID="lbtnclose" OnClick="lbtnclose_Click" runat="server"><img src="../images/close.jpg" border="0" /></asp:LinkButton>
                                                                    <asp:HiddenField ID="hdnDispose" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                    </div>
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
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        function EndRequestHandler(sender, args)
        {
           if (args.get_error() != undefined)
           {
               //alert("dfg"+args.get_response().get_statusCode());
               if ((args.get_response().get_statusCode() == '12007') || (args.get_response().get_statusCode() == '12029'))
               {
                //Show a Message like 'Please make sure you are connected to internet';
                //alert('Please make sure you are connected to internet');
                args.set_errorHandled(true); 
               }
           }
        }
    
    </script>
    <div id="validationDialog">
    </div>
    <div id="dispositionDialog">
        <select id="dispositions" name="dispositions" size="10">
            <asp:Literal ID="dispositionOptions" runat="server" />
        </select>
    </div>
</body>
</html>
