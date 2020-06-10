<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html>
<head>
<title>Rainmaker Report Maker V2</title>
<link rel="stylesheet" type="text/css" href="styles/reportmaker.css"/> 
<link rel="stylesheet" media="all" type="text/css" href="css/jquery-ui.css" />
<link rel="stylesheet" media="all" type="text/css" href="css/jquery-ui-timepicker-addon.css" />

<script type="text/javascript" src="javascript/jquery.js"></script>
<script type="text/javascript" src="javascript/jquery.print.js"></script>
<script type="text/javascript" src="javascript/time.js"></script>
<script type="text/javascript" src="javascript/jquery-ui-1.9.1.custom.min.js"></script>
<script type="text/javascript" src="javascript/jquery-ui-timepicker-addon.js"></script>
<script type="text/javascript">
//
/// Global script variables
//
var lastReportQry = "";

// Hides the controls for all report types
function hideReportControls() {
		$('#reportctrls').css({display: 'none' });
		$('#altreportctrls').css({display: 'none' });
		$('#agentNameTitleRegion').css({ display: 'none' });
		$('#agentNameSelectRegion').css({display: 'none' });
		$('#phoneNumberTitleRegion').css({display: 'none' });
		$('#phoneNumberSelectRegion').css({display: 'none' });
}

// Called each time the choose decides to make a different kind of report
function reportChoosen() {

		var rType = $('#reportType :selected').val();
		hideReportControls();
		switch(rType) {
		case "0": // Shift Report    r
		
				$('#reportctrls').css({display: 'block' });
				break;
		
		case "4": // Agents Dialer Results
		
				$('#reportctrls').css({display: 'block' });
				$('#agentNameTitleRegion').css({display: 'inline' });
				$('#agentNameSelectRegion').css({display: 'inline' });
				getCampaignList(true);
				getAgentList(false);
				break;
        
        case "1": // Call History Report By Phone
		
				$('#reportctrls').css({display: 'block' });
				$('#phoneNumberTitleRegion').css({display: 'inline' });
				$('#phoneNumberSelectRegion').css({display: 'inline' });
				break;

        case "3": // Summarized Agents Dialer Results
        case "5": // Call History Report By Agent
		
				$('#reportctrls').css({display: 'block' });
				$('#agentNameTitleRegion').css({display: 'inline' });
				$('#agentNameSelectRegion').css({display: 'inline' });
				getAgentList((rType=="3") ? true : false);
				break;
		}
}

// Sends a request to the server to create a report.
function createReport() {

		var lastReportQry = '?actionType=1&reportType=';
		lastReportQry += $('#reportType :selected').val();
		lastReportQry += createReportParams();
		var url = 'ReportHandler.ashx' + lastReportQry;

		$.ajax({
		    url: url,
		    type: "GET",
		    cache:false,
		    data: {},
		    success: function(data) {

		    var response = eval("(" + data + ")");
		    $('#reportregion').html(response.contents);
		    $('#reportregion').css({ display: 'block' });
		    $('#topctrls').css({ display: 'block' });
		    $('#bottomctrls').css({ display: 'block' });
		    },
		    failure: function() { alert('Something went wrong...') }
		});
}

// Gets the parameters required for the currently selected report type
function createReportParams() {

    var reportType = $('#reportType :selected').val();
		switch(reportType) {
		case "4": // Agent Dialer Results Report
				var params = getReportParams();
				params += "&agentid=" + $('#agentNameSelect').val();
				return params;
        case "0":
            var params = getReportParams();
            return params;
            
		case "1":
				var params = getReportParams();
				params += "&phonenumber=" + $('#phoneNumber').val();
				return params;
		case "3":
		case "5":
				var params = getReportParams();
				params += "&agentid=" + $('#agentNameSelect').val();
				return params;
		//case "1":
		//    break;
		};
}
Date.prototype.getMonthName = function(lang) {
    lang = lang && (lang in Date.locale) ? lang : 'en';
    return Date.locale[lang].month_names[this.getMonth()];
};

Date.prototype.getMonthNameShort = function(lang) {
    lang = lang && (lang in Date.locale) ? lang : 'en';
    return Date.locale[lang].month_names_short[this.getMonth()];
};

Date.locale = {
    en: {
        month_names: ['January', 'February', 'March', 'April', 'May', 'June', 'July', 'August', 'September', 'October', 'November', 'December'],
        month_names_short: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
    }
};
function getReportParams() {

    var params = "";

    var thedate = new Date($('#shiftReportStartDate').val());


    var mm = thedate.getMonthNameShort(); //January is 0!
    var dd = thedate.getDate();
    var yyyy = thedate.getFullYear();
    params += "&startDate=" + dd + "-" + mm + "-" + yyyy;


    thedate.getHours();
    var hh = thedate.getHours();
    var mm = thedate.getMinutes() < 10 ? "0" + thedate.getMinutes() : thedate.getMinutes();
    var ss = thedate.getSeconds() < 10 ? "0" + thedate.getSeconds() : thedate.getSeconds();
    params += "&startTime=" + hh + ":" + mm + ":" + ss;

    
    
    thedate = null;
    thedate = new Date($('#shiftReportEndDate').val());

    var mm = thedate.getMonthNameShort(); //January is 0!
    var dd = thedate.getDate();
    var yyyy = thedate.getFullYear();
    params += "&endDate=" + dd + "-" + mm + "-" + yyyy;


    thedate.getHours();
    var hh = thedate.getHours();
    var mm = thedate.getMinutes() < 10 ? "0" + thedate.getMinutes() : thedate.getMinutes();
    var ss = thedate.getSeconds() < 10 ? "0" + thedate.getSeconds() : thedate.getSeconds();
    params += "&endTime=" + hh + ":" + mm + ":" + ss;

    params += "&campaignName=" + $('#shiftReportCampaign').val();
    return params;
    
}

// Reset the data/time value for the selected report
function resetDateTime() {

		//findTime();
		var reportType = $('#reportType :selected').val();
		switch(reportType) {
		    case "0": // Shift-report

		        var today = new Date();
		        var month = today.getMonth() + 1; //January is 0!
		        var dd = today.getDate();
		        var yyyy = today.getFullYear();
		        var hh = today.getHours();
		        //var mm = today.getMinutes() < 10 ? "0" + today.getMinutes() : today.getMinutes();
		        //var ss = today.getSeconds() < 10 ? "0" + today.getSeconds() : today.getSeconds();

		        var today = month + '/' + dd + '/' + yyyy; // +" " + hh + ":" + mm + ":" + ss;
		        $('#shiftReportStartDate').val(today + " 00:00:01");
		        $('#shiftReportEndDate').val(today + " 23:59:59");
		        break;
		}
}

// Loads the current list of campaigns from the server shows it in every campaignList
function getCampaignList(allCampaigns)
{
		var url = 'ReportHandler.ashx?actionType=0&';
		if(allCampaigns)
				url += "allCampaignsList=1";
		else
				url += "allCampaignsList=0";
		$.ajax({
		    url: url,
		    type: "GET",
		    cache: false,
			data: {},
			success: function(data)
					{
					    var response = eval("(" + data + ")");
							$('#shiftReportCampaign').html(response.contents);
					},
			failure: function(){ alert('getCampaignList error') }
		});
}

// Loads the current list of agents from the server shows
function getAgentList(allAgents) {
		
	var url = 'ReportHandler.ashx?actionType=2&';
	if(allAgents)
        url += "allAgentsList=1";
	else
	    url += "allAgentsList=0";
	$.ajax({
	    url: url,
	    type: "GET",
	    cache: false,
	    data: {},
	    success: function(data) {
	        var response = eval("(" + data + ")");
	        $('#agentNameSelect').html(response.contents);
	    },
	    failure: function() { alert('getAgentList error.') }
	});				
				

}

// Shows a printable version of the current report.
function showPrintable() {

    //window.open("PrintReport.aspx" + lastReportQry, "printReportWin");
    
    $('#reportregion').print();
	
}

// Sets up all of the options required when the Report Maker is first loaded...
function reportMakerStart() {

		$('#shiftReportStartHour').html(create12HourList());
		$('#shiftReportStartMin').html(create5MinList());
		$('#shiftReportEndHour').html(create12HourList());
		$('#shiftReportEndMin').html(create5MinList());
		resetDateTime();
		getCampaignList(false);
		reportChoosen();
		
}

function jqueryfunctions() {

    $(document).ready(function() {

        $.ajaxSetup({ cache: false });
        $('input[data-type=dtpicker]').live().each(function() {
        $(this).datetimepicker({
                showSecond: true,
                timeFormat: 'HH:mm:ss',
                changeMonth: true, changeYear: true
            });
        });
    });
}
jqueryfunctions();


</script>




</head>

<body onload="reportMakerStart();">

<div id="report" class="reportStyle">

<!--
	== Controls that allow you to pick the type of report that you want 
	== to create.
	-->
    <div id="details" class="reportChooserCtrls">
        <select id="reportType" style="width: 250px" onchange="reportChoosen();" 
                name="reportType">
		        <option value="0">Shift Report</option>
		        <option value="1">Call History Report By Telephone</option>
		        <option value="5">Call History Report By Agent</option>
		        <option value="3">Summarized Results Report</option>
		        <option value="4">Agent Dialer Results Report</option>
        </select>
        <input type="button" value="Create Report" onclick="createReport();"/>
    </div>

    <!-- 
	    == "Shift Report" controls 
	    ==
	    == NOTE: these controls appear when creating a "Shift Report"
	    -->
    <div id="reportctrls" class="reportCtrls">
	    <div class="reportCtrlsRegion">
	        <span>Campaign Name</span>
	       
		        <select id="shiftReportCampaign" style="width: 200px">
				        <option value="0">Campaign Name</option>
		        </select>
	        
	        <span id="agentNameTitleRegion" style="position: absolute; left: 400px; display: none;">Agent Name</span>
	        <span id="agentNameSelectRegion" style="position: absolute; left: 500px; display: none;">
		        <select id="agentNameSelect" style="width: 200px">
				        <option value="0">Campaign Name</option>
		        </select>
	        </span>
	        <span id="phoneNumberTitleRegion" style="position: absolute; left: 400px; display: none;">Phone Number</span>
	        <span id="phoneNumberSelectRegion" style="position: absolute; left: 520px; display: none;">
		        <input id="phoneNumber" maxlength="10" style="width: 100px"  type="text" />
	        </span>
	    </div>
	   <br />
	    <div class="reportCtrlsRegion">
		    <span style="width: 100px">Start Date&nbsp;&nbsp;</span>
		    
		        <input id="shiftReportStartDate" style="width: 120px" data-type="dtpicker" />
		    <span style="width: 100px">End Date&nbsp;&nbsp;</span>
		   
			    <input id="shiftReportEndDate" style="width: 120px" data-type="dtpicker" />
		    
		    
	    </div>
	    
    </div>
    <!--
	    ==
	    == If I end needing a customized set of controls, this is where they will go...
	    ==
	    -->
    <div id="altreportctrls" class="reportCtrls">
    More controls...
    </div>


    <!-- Controls of D/Ling and managing report (At top of report -->
    <div id="topctrls" style="display: none" class="reportTools">
        <input type="button" value="Print" style="width: 150px" onclick="showPrintable();"/>
    </div>

    <!-- This is where the rendered report actually appears -->
    <div id="reportregion" class="reportPrintRegion"></div>

    <!-- Controls of D/Ling and managing report (At bottom of report -->
    <div id="bottomctrls" style="display: none" class="reportTools">
        <input type="button" value="Print" style="width: 150px"/>
    </div>
</div>

</body>
</html>
