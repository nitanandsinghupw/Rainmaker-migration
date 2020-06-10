<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional/EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>Rainmaker Report</title>

<link rel="stylesheet" type="text/css" href="styles/reportmaker.css"/> 
<style type="text/css">
    /* Replace imported body style so we don't get a background and
     * other junk.
     **************************************************************/
    body {
	    margin: 0px;
        margin-top: 10px;
        margin-left: 15px;
        width: auto;	
        background: none;
    }
</style>
<script type="text/javascript" src="javascript/jquery.js"></script>
<script type="text/javascript">

// Sends a request to the server to create a report using properties read
// from the URL.
function createReport() {
    
    var url = "ReportHandler.ashx" + window.location.search;
    $.ajax({
        url: url,
        type: "GET",
        data: {},
        success: function(data) {
            var response = eval("(" + data + ")");
            $('#content').html(response.contents);
        },
        failure: function() { alert('getCampaignList error') }
    });
}
</script>
</head>

<body id="content" onload="createReport();">
</body>
</html>
