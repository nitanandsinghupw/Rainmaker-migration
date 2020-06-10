<%@ Page Language="C#" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional/EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html>
<head>
<title>Rainmaker Data Manager</title>

<link rel="stylesheet" type="text/css" href="styles/queryviewer.css"/>
<script type="text/javascript" src="javascript/prototype.js"></script>
<script type="text/javascript">

function loadSettings()
	{
	$('optionsCtrl').setStyle('display: none');	
	if($('nameSettingsList').getValue() == 0) return;
	var url = 'ReportHandler.ashx?actionType=15&nameSettingId=' + $('nameSettingsList').getValue() + "&campaignName=" + $('campaignList').getValue()  + "&queryID=" + $('queryList').getValue();
	new Ajax.Request
			(
			url,
				{
				method: 'GET',
				parameters: {},
				onSuccess: function(transport)
					{
					runQuery();
					},
				onFailure: function(){ alert('Something went wrong...') }
        }
      );
	}

function saveSettings()
	{
	$('optionsCtrl').setStyle('display: none');	
	if($('nameSettingsList').getValue() == 0) return;
	var url = 'ReportHandler.ashx?actionType=14&nameSettingId=' + $('nameSettingsList').getValue() + "&campaignName=" + $('campaignList').getValue()  + "&queryID=" + $('queryList').getValue();
	new Ajax.Request
			(
			url,
				{
				method: 'GET',
				parameters: {},
				onSuccess: function(transport)
					{
					return; // nothing needs done
					},
				onFailure: function(){ alert('Something went wrong...') }
        }
      );
	}

// Asks the user which named settings value they want to delete
function deleteNameSettingPrompt()
	{
	$('optionsCtrl').setStyle('display: none');	
	var viewport = document.viewport.getDimensions();
	var scrollAmount = document.viewport.getScrollOffsets();
	var scrnHorizCenter = viewport.width/2;
	var scrnVertCenter = viewport.height/3;
	var newStyle = "";
	newStyle += 'left: ' + (scrollAmount.left + scrnHorizCenter-175) + 'px';
	newStyle += '; top: ' + (scrollAmount.top + scrnVertCenter-45) + 'px';
	newStyle += '; display: block; border: 4px solid #DDDDDD; width: 350px; height: 90px';
	$('namedSettingsDeletePrompt').setStyle(newStyle);	
	}

function deleteNameSetting()
	{
	$('namedSettingsDeletePrompt').setStyle('display: none');	
	if($('nameSettingsDeleteList').getValue() == 0) return;
	var url = 'ReportHandler.ashx?actionType=13&nameSettingId=' + $('nameSettingsDeleteList').getValue() + "&campaignName=" + $('campaignList').getValue();
	new Ajax.Request
			(
			url,
				{
				method: 'GET',
				parameters: {},
				onSuccess: function(transport)
					{
					var response = eval("(" + transport.responseText + ")");
					$('nameSettingsList').update(response.contents);
					$('nameSettingsDeleteList').update(response.contents);
					},
				onFailure: function(){ alert('Something went wrong...') }
        }
      );
	}

// Asks the user for a name for the new named settings
function newNameSettingPrompt()
	{
	$('optionsCtrl').setStyle('display: none');	
	var viewport = document.viewport.getDimensions();
	var scrollAmount = document.viewport.getScrollOffsets();
	var scrnHorizCenter = viewport.width/2;
	var scrnVertCenter = viewport.height/2;
	var newStyle = "";
	newStyle += 'left: ' + (scrollAmount.left + scrnHorizCenter-150) + 'px';
	newStyle += '; top: ' + (scrollAmount.top + scrnVertCenter-45) + 'px';
	newStyle += '; display: block; border: 4px solid #DDDDDD; width: 300px; height: 90px';
	$('namedSettingsPrompt').setStyle(newStyle);	
	}

function newNameSettingSave()
	{
	$('namedSettingsPrompt').setStyle('display: none');
	var url = 'ReportHandler.ashx?actionType=12&name=' + $('namedSettingsName').getValue() + "&campaignName=" + $('campaignList').getValue() + "&queryID=" + $('queryList').getValue();
	new Ajax.Request
			(
			url,
				{
				method: 'GET',
				parameters: {},
				onSuccess: function(transport)
					{
					var response = eval("(" + transport.responseText + ")");
					$('nameSettingsList').update(response.contents);
					$('nameSettingsDeleteList').update(response.contents);
					},
				onFailure: function(){ alert('Something went wrong...') }
        }
      );
	$('namedSettingsName').setValue('');
	}

function getSettingsList(campaignID)
	{
	var url = 'ReportHandler.ashx?actionType=11&campaignName=' + campaignID;
	new Ajax.Request(
			url,  
				{
				method: 'GET',
				parameters: {},
				onSuccess: function(transport)
					{
					var response = eval("(" + transport.responseText + ")");
					$('nameSettingsList').update(response.contents);
					$('nameSettingsDeleteList').update(response.contents);
					$('nameSettingsList').setValue(0);
					$('nameSettingsDeleteListList').setValue(0);
					},
				onFailure: function(){ alert('Something went wrong...') }
        });
	}

// These define how each of the column headers should appear
var tableCols = Array();
tableCols.push({width: 0, direction: 'dsc', name: 'unknown'});

// Affect column sorting
var curSortColumn;
var curSortDirection;
var sortOn = 0;

// The UniqueKey values for each row should be stored in here
var rowKeys = Array();
function optionsChange(reload)
	{
	if(!reload) reload = false;
	var rowLimit = parseInt($('rowLimit').getValue());
	if(!rowLimit) rowLimit = 20;
	var url = 'ReportHandler.ashx?actionType=7&';
	url += "campaignName=" + $('campaignList').getValue();
	url += "&queryID=" + $('queryList').getValue();
	url += "&rowLimit=" + rowLimit;
	var showCSVHeaders = ($('showCSVHeaders').getValue() == 'on') ? "1" : "0";
	url += ("&showCSVHeaders=" + showCSVHeaders);
	url += "&sortOn=" + sortOn.toString();
	url += "&sortColumn=" + (parseInt(curSortColumn)+1);
	url += "&sortDirection=" + ((curSortDirection == 'dsc') ? '1' : '0');
	new Ajax.Request
			(
			url,
				{
				method: 'GET',
				parameters: {},
				onSuccess: function(transport)
					{
					if(reload) runQuery();
					},
				onFailure: function(){ alert('Something went wrong...') }
        }
      );
	}

//
/// Code for managing the help window
//
function showHelp()
	{
	var viewport = document.viewport.getDimensions();
	var scrollAmount = document.viewport.getScrollOffsets();
	var scrnHorizCenter = viewport.width/2;
	var scrnVertCenter = 250;
	var newStyle = "";
	newStyle += 'left: ' + (scrollAmount.left + scrnHorizCenter-350) + 'px';
	newStyle += '; top: ' + (scrollAmount.top + scrnVertCenter-150) + 'px';
	newStyle += '; display: block; border: 4px solid #DDDDDD; width: 700px; height: 300px';
	$('helpWindow').setStyle(newStyle);
	}

// Intercepts mouse clicks in the option control and keeps them
// from propogating up the DOM.
function helpSaver(event)
	{
	event.stop();
	}

//
/// Code of managing and responding to the 'options' region
//
function resetOptions()
	{
	$('optionsCtrl').setStyle('display: none');	
	var url = 'ReportHandler.ashx?actionType=10&';
	url += "campaignName=" + $('campaignList').getValue();
	url += "&queryID=" + $('queryList').getValue();
	new Ajax.Request
			(
			url,
				{
				method: 'GET',
				parameters: {},
				onSuccess: function(transport)
					{
					runQuery();
					},
				onFailure: function(){ alert('Something went wrong...') }
        }
      );
	}

function showOptions()
	{
	var position = $('options').positionedOffset();
	var newStyle = 'left: ' + position.left + 'px';
	newStyle += '; top: ' + (position.top + 25) + 'px';
	newStyle += '; display: block; border: 4px solid #DDDDDD;';
	$('optionsCtrl').setStyle(newStyle);
	}

function saveOption()
	{	
	$('optionsCtrl').setStyle('display: none');	
	}

// Intercepts mouse clicks in the option control and keeps them
// from propogating up the DOM.
function optionSaver(event)
	{
	event.stop();
	}

function hideColumn(columnName)
	{
	var url = 'ReportHandler.ashx?actionType=8&';
	url += "campaignName=" + $('campaignList').getValue();
	url += "&queryID=" + $('queryList').getValue();
	url += "&columnName=" + columnName;
	url += "&hidden=1";
	new Ajax.Request
			(
			url,
				{
				method: 'GET',
				parameters: {},
				onSuccess: function(transport)
					{
					runQuery();
					},
				onFailure: function(){ alert('Something went wrong...') }
        }
      );
	}

//
/// Code for interacting with the rainmaker server
//
// Loads the current list of campaigns from the server shows it in every campaignList
function pickCampaign()
	{
	if(isLoading) return;
	var campaignID = $('campaignList').getValue();
	if(campaignID == 0)
		{
		$('queryList').disabled = true;
		$('runQuery').disabled = true;
		$('getCSV').disabled = true;
		$('options').disabled = true;
		$('rowLimit').disabled = true;
		}
	else
		{
		$('queryList').disabled = false;
		$('runQuery').disabled = false;
		$('options').disabled = false;
		getQueryList(campaignID);
		getSettingsList(campaignID);
		}
	}
	
function getCampaignList(allCampaigns)
	{
	var url = 'ReportHandler.ashx?actionType=0&';
	if(allCampaigns)
		url += "allCampaignsList=1";
	else
		url += "allCampaignsList=0";
	new Ajax.Request(
			url,  
				{
				method:'GET',
				parameters: {},
				onSuccess: function(transport)
					{
					var response = eval("(" + transport.responseText + ")");
					$('campaignList').update(response.contents);
					},
				onFailure: function(){ alert('Something went wrong...') }
        });
	}

function getCSV()
	{
	$('confirmDelete').setStyle('display: none');	
	if(isLoading) return;
	var queryID = $('queryList').getValue();
	var campaignID = $('campaignList').getValue();
	var deleteDownload = $('deleteDownload').getValue();
	window.open("ReportHandler.ashx?actionType=6&queryID=" + queryID + "&campaignName=" + campaignID + "&deleteDownload=" + deleteDownload);
// Seems to cause server problems?
//runQuery();
	}

function showConfirmDelete()
	{
	var viewport = document.viewport.getDimensions();
	var scrollAmount = document.viewport.getScrollOffsets();
	var scrnHorizCenter = viewport.width/2;
	var scrnVertCenter = 250;
	var newStyle = "";
	newStyle += 'left: ' + (scrollAmount.left + scrnHorizCenter-250) + 'px';
	newStyle += '; top: ' + (scrollAmount.top + scrnVertCenter-50) + 'px';
	newStyle += '; display: block; border: 4px solid #DDDDDD; width: 500px; height: 100px';
	$('confirmDelete').setStyle(newStyle);
	}

function runQuery()
	{
	showLoadingMessage();
	var queryID = $('queryList').getValue();
	var campaignID = $('campaignList').getValue();
	var url = 'ReportHandler.ashx?actionType=4';
	url += "&queryID=" + queryID;
	url += "&campaignName=" + campaignID;
	url += "&sortOn=0";
	$('dataView').update('');
	new Ajax.Request(
			url,  
				{
				method:'GET',
				parameters: {},
				onSuccess: handleQueryResponse,
				onFailure: function(){ alert('Something went wrong...') }
        });
	}

function handleQueryResponse(transport)
	{
	var response = eval("(" + transport.responseText + ")");
	curCellValue = selectedCell = cursorX = cursorY = null;
	tablePixelWidth = parseInt(response.tablePixelWidth);
	tableWidth = parseInt(response.tableWidth);
	tableHeight = parseInt(response.tableHeight);
	sortOn = parseInt(response.sortOn);
	eval(response.tableColumns);
	eval(response.rowKeys);
	$('rowLimit').setValue(parseInt(response.rowLimit));
	$('showCSVHeaders').setValue(response.showCSVHeaders);
	$('dataView').update(response.contents);
	$('dataView').setStyle('width: ' + tablePixelWidth + 'px');
	$('getCSV').disabled = false;
	$('controlPanel').setStyle('width: ' + tablePixelWidth + 'px');
	$('nameSettingsList').setValue(0);
	$('nameSettingsDeleteList').setValue(0);
	$('recordCount').update('Records: ' + parseInt(response.rowCount));
	$('querySample').update('Query Options: ' + response.sqlOptions);
	hideLoadingMessage();
	}

// Loads all the queries associated with the currently choosen campaign
function getQueryList(campaignID)
	{
	var url = 'ReportHandler.ashx?actionType=3&';
	url += "allCampaignsList=1&campaignName=" + campaignID;
	new Ajax.Request(
			url,  
				{
				method:'GET',
				parameters: {},
				onSuccess: function(transport)
					{
					$('rowLimit').disabled = false;
					var response = eval("(" + transport.responseText + ")");
					$('queryList').update(response.contents);
					},
				onFailure: function(){ alert('Something went wrong...') }
        });
	}

//
/// Code to keep track of the currently selected cell
//
// Name of the currently selected cell
var selectedCell = null;
// X and Y position of the currently selected cell
var cursorX = null;
var cursorY = null;
// Size of the Grid
var tableWidth = 4;
var tableHeight = 2;
var curCellValue = null;

function chooseCell(x, y)
	{
	if(isLoading) return;
	var cellName = x + 'x' + y;
	if(selectedCell == cellName) return;
	deselectCell();
	cursorX = x;
	cursorY = y;
	selectedCell = cellName;
	curCellValue = $(cellName).innerHTML;
	$(cellName).update("<input id='cursor' type='text'  value='" + curCellValue.trim() + "'/>");
	Event.observe($('cursor'), 'keypress', tableCtrlKeyHandler);
	$('cursor').focus();
	}

function tableCtrlKeyHandler(evt)
	{
	if(isLoading) return;
	if(evt.keyCode == Event.KEY_RETURN)
		{
		deselectCell();
		evt.stop();
		}
	var key = String.fromCharCode(evt.charCode);
	if(evt.ctrlKey && evt.keyCode == Event.KEY_RIGHT) moveCellRight(evt);
	if(evt.ctrlKey && evt.keyCode == Event.KEY_LEFT)  moveCellLeft(evt);
	if(evt.ctrlKey && evt.keyCode == Event.KEY_UP) moveCellUp(evt);
	if(evt.ctrlKey && evt.keyCode == Event.KEY_DOWN) moveCellDown(evt);
	}

// will deselect any currently selected cells, if there content has changed it will
// be sent back to the server.
function deselectCell()
	{
	// Hide the option box if it is showing
	$('optionsCtrl').setStyle('display: none');	
	if(isLoading) return;
	// Read the change the user made
	if(selectedCell == null) return;
	if($('cursor') == null) return;
	var userInput = $('cursor').getValue();
	$(selectedCell).update(userInput);
	if(curCellValue.trim() == userInput.trim())
		{
		selectedCell = cursorX = cursorY = null;
		return; // nothing to do
		}
	undoUserInput = curCellValue;
	undoCell = selectedCell;	
	var colType = $(selectedCell).readAttribute('dataType');
	var colName = tableCols[cursorX].name;
	var uniqueKey = rowKeys[(cursorY-1)].id;
	selectedCell = cursorX = cursorY = null;
	var campaignID = $('campaignList').getValue();
	saveValue(campaignID, uniqueKey, colName, colType, userInput);
	}

function saveValue(campaignID, uniqueKey, colName, colType, userInput)
	{
	// First store the undo information
	undoCampaignID = campaignID;
	undoUniqueKey = uniqueKey;
	undoColName = colName;
	undoColType = colType;
	// Now send the change to the server
	var url = 'ReportHandler.ashx?actionType=5&';
	url += "UniqueKey=" + uniqueKey + "&colName=" + colName + "&value=" + userInput + "&campaignName=" + campaignID + "&dataType=" + colType;
	new Ajax.Request(
			url,  
				{
				method: 'POST',
				parameters: {},
				onSuccess: function(transport) {},
				onFailure: function(){ alert('Could not store value...') }
        });
	}

function moveCellRight(evt)
	{
	if(isLoading) return;
	if(selectedCell == null) return;
	if(cursorX < (tableWidth-1))
		{
		cursorX = findNextCellRight(cursorX);
		chooseCell(cursorX, cursorY);
		}
	evt.stop();
	}

function moveCellLeft(evt)
	{
	if(isLoading) return;
	if(selectedCell == null) return;
	if(cursorX > 2)
		{
		cursorX = findNextCellLeft(cursorX);
		chooseCell(cursorX, cursorY);
		}
	evt.stop();
	}

// Because rows can be hidden we may have to skip a few rows when looking to
// the right before another one is encountered. This will do the skips
// until we find a row or run out of possibilities.
function findNextCellRight(curX)
	{
	var start = curX;
	do
		{
		curX++;
		var cellName = curX + 'x' + cursorY;
		if(curX > (tableWidth-1)) 
			{
			curX = start;
			break;
			}
		}
	while($(cellName) == null)
	return curX;
	}

function findNextCellLeft(curX)
	{
	var start = curX;
	do
		{
		curX--;
		var cellName = curX + 'x' + cursorY;
		if(curX < 2)
			{
			curX = start;
			break;
			}
		}
	while($(cellName) == null)
	return curX;
	}

function moveCellDown(evt)
	{
	if(isLoading) return;
	if(selectedCell == null) return;
	if(cursorY < (tableHeight-1))
		{
		cursorY++;
		chooseCell(cursorX, cursorY);
		}
	evt.stop();
	}

function moveCellUp(evt)
	{
	if(isLoading) return;
	if(selectedCell == null) return;
	if(cursorY > 1)
		{
		cursorY--;
		chooseCell(cursorX, cursorY);
		}
	evt.stop();
	}

// Will hit the server and fetch the table data stored on the current column
//
// value - the 0-based index of the column to sort on
// direction - either 'asc' or 'dsc'
function sortByColumn(sortColumn)
	{
	sortOn = 1;
	curSortColumn = sortColumn;
	curSortDirection = tableCols[sortColumn].direction;
	optionsChange();
	var queryID = $('queryList').getValue();
	var campaignID = $('campaignList').getValue();
	var url = 'ReportHandler.ashx?actionType=4';
	url += "&queryID=" + queryID;
	url += "&campaignName=" + campaignID;
	showLoadingMessage();
	new Ajax.Request(
			url,  
				{
				method:'GET',
				parameters: {},
				onSuccess: handleSortQueryResponse,
				onFailure: function(){ alert('Something went wrong...') }
        });
	}


function handleSortQueryResponse(transport)
	{
	handleQueryResponse(transport);	
	if(curSortDirection == 'dsc')
		{
		tableCols[curSortColumn].direction = 'asc';		
		toggle = false;
		}
	else
		{
		tableCols[curSortColumn].direction = 'dsc';		
		toggle = true;
		}	
	}

//
// Used to turn off text selection on the grid so things act as expected
//
function disableSelection(target)
	{
	if(isLoading) return;
	target.onselectstart = function() 
		{
		return false;
		};
	target.unselectable = "on";
	target.style.MozUserSelect = "none";
	target.style.cursor = "default";
	}

//
/// Code for resizing the columns
//
/// I learned a few things while creating this code:
///
/// If you use 'auto' for the margins of the table you will discover the mouse 
/// does not track with the column movment. This is because the table is being 
/// repositioned on the screen. So, you're going to have to achore the table to
/// a fixed position.
///
/// If you add things like padding or borders to the table cells the mouse will
/// not track with the column movement. This is because the width of the columns
/// does not take these values into account. The solution is to apply this kind
/// of thing to the content within the cell.
//
var lastMouseX = 0;
var isTrackingMove = false;
var ctrlName = null;
var nextCtrlName = null;
var resizeColumnIdx = null;
var nextColIdx = null;

function resizeColumnStart(event, idxA, idxB)
	{
	if(isLoading) return;
	resizeColumnIdx = idxA;
	nextColIdx = findNextHeaderRight(idxA);
	if(nextColIdx == idxA) return;
	ctrlName = 'sizer' + idxA;
	nextCtrlName = null;
	tableCols[resizeColumnIdx].width = $(ctrlName).getWidth();
	if(nextColIdx != null)
		{
		nextCtrlName = 'sizer' + nextColIdx;
		tableCols[nextColIdx].width = $(nextCtrlName).getWidth();
		}
	lastMouseX = event.pointerX();
	Event.observe(window, 'mousemove', resizeColumnMove);
	Event.observe(window, 'mouseup', resizeColumnStop);
	}

function findNextHeaderRight(curX)
	{
	var start = curX;
	do
		{
		curX++;
		var headerName = 'sizer' + curX;
		if(curX > (tableWidth-1)) 
			{
			curX = start;
			break;
			}
		}
	while($(headerName) == null)
	return curX;
	}

function resizeColumnMove(event)
	{
	// Check if trying to resize last column, if yes, don't allow
	if(nextCtrlName == null) return;
	// Go ahead and resize the column
	var changeX = event.pointerX() - lastMouseX;
	lastMouseX = event.pointerX();
	tableCols[resizeColumnIdx].width = $(ctrlName).getWidth() + changeX;
	tableCols[nextColIdx].width = $(nextCtrlName).getWidth() - changeX;;
	$(ctrlName).setStyle('width: ' + tableCols[resizeColumnIdx].width + 'px');
	$(nextCtrlName).setStyle('width: ' + tableCols[nextColIdx].width + 'px');
	}

function resizeColumnStop(event)
	{
	Event.stopObserving(window, 'mousemove');
	Event.stopObserving(window, 'mouseup');
	updateColumnWidth(tableCols[resizeColumnIdx].name, tableCols[resizeColumnIdx].width);
	updateColumnWidth(tableCols[nextColIdx].name, tableCols[nextColIdx].width);
	}

function updateColumnWidth(colName, width)
	{
	var url = 'ReportHandler.ashx?actionType=9&';
	url += "campaignName=" + $('campaignList').getValue();
	url += "&queryID=" + $('queryList').getValue();
	url += "&columnName=" + colName;
	url += "&width=" + width;
	new Ajax.Request
			(
			url,
				{
				method: 'GET',
				parameters: {},
				onSuccess: function(transport)
					{
					// nothing needs to be done
					},
				onFailure: function(){ alert('Something went wrong...') }
        }
      );
	}

// Controls should be locked while this is true
var isLoading = false;

function showLoadingMessage()
	{
	$('campaignList').disabled = true;
	$('queryList').disabled = true;
	$('runQuery').disabled = true;
	$('getCSV').disabled = true;
	isLoading = true;
	var viewport = document.viewport.getDimensions();
	var scrollAmount = document.viewport.getScrollOffsets();
	var scrnHorizCenter = viewport.width/2;
	var scrnVertCenter = viewport.height/2;
	var newStyle = "";
	newStyle += 'left: ' + (scrollAmount.left + scrnHorizCenter-200) + 'px';
	newStyle += '; top: ' + (scrollAmount.top + scrnVertCenter-45) + 'px';
	newStyle += '; display: block; border: 4px solid #DDDDDD; width: 400px; height: 90px';
	$('msgBox').setStyle(newStyle);
	}
	
function hideLoadingMessage()
	{
	$('campaignList').disabled = false;
	$('queryList').disabled = false;
	$('runQuery').disabled = false;
	$('getCSV').disabled = false;
	isLoading = false;
	$('msgBox').setStyle('display: none');
	}

//
/// Code to handle undo operations
//
var undoCampaignID, undoUniqueKey, undoColName, undoColType, undoUserInput, undoCell;
function undoLastChange()
	{
	if(undoCell == null) return;
	deselectCell();
	saveValue(undoCampaignID, undoUniqueKey, undoColName, undoColType, undoUserInput);
	$(undoCell).update(undoUserInput);
	undoCampaignID = undoUniqueKey = undoColName = undoColType = undoUserInput = undoCell = null;
	}

//
// Page Setup Code
//
Event.observe(window, 'load', 
	function() 
		{
		getCampaignList(true);
		disableSelection($('dataView'));
		Event.observe(window, 'mousedown', deselectCell);
		Event.observe('optionsCtrl', 'mousedown', optionSaver);
		Event.observe('helpWindow', 'mousedown', helpSaver);
		Event.observe(window, 'keypress', 
			function(evt) 
				{				
				var key = String.fromCharCode(evt.charCode);
				if(key == 'z' && evt.ctrlKey) undoLastChange();
				});
		});
</script>
</head>

<body id="dataManager">
<div style="margin: 0px">
<table id="controlPanel" class="layout" cellpadding="0" cellspacing="0">
<tr>
  <td class="header">
  <table class="controls" style="width: 98%" cellpadding="0" cellspacing="0">
	<tr>
		<td>
		Campaign List
		<select id="campaignList" style="margin-left: 10px; margin-right: 10px; width: 200px" onchange="pickCampaign()">
			<option value='0'>Choose Campaign</option>
		</select>
		Query List
		<select id="queryList" style="margin-left: 10px; width: 200px" disabled>
			<option value='0'>Choose Query</option>
		</select>
		<input type="button" id="runQuery" style="margin-left: 10px" value="Run Query" disabled onclick="runQuery()"/>
		<input type="button" id="getCSV" style="margin-left: 10px" value="Download" disabled onclick="showConfirmDelete()"/>
		<input type="button" id="options" style="margin-left: 10px; margin-right: 10px" value="Options" disabled onclick="showOptions()"/>
		Row Limit<input type="text" id="rowLimit" style="margin-left: 10px" disabled onchange="optionsChange(true);" value="20"/>
		</td>
		<td style="text-align: right">
		<img src="resources/question.png" width="50px" alt="Question" onclick="showHelp()"/>
		</td>
	</tr>
  </table>
  </td>
</tr>
<!-- This Row Contains Runtime information -->
<tr>
	<td class="header" style="height: 15px">
	<table style="width: 98%; margin-left: 17px" cellpadding="0" cellspacing="0">
	<tr>
		<td id="recordCount" style="width: 200px; padding: 8px">
		Records: 0
		</td>
		<td id="querySample">
		Query Options: Not Ready
		</td>
	</tr>
	</table>
	</td>
</tr>
</table>
</div>

<!-- This is where the table will end up going -->
<table id="dataView" class="dataView" style="margin: 0px;"></table>

<!-- This is used a loading dialog box -->
<div id="msgBox" style="display: none; position: absolute; background-color: white; z-index: 100; padding: 20px">
	<h2>Talking to the server, please wait...</h2><br/>
	<img alt="progress message" src="resources/progress.gif" width="350px" height="15px"/>
</div>

<!-- Confirms if user wants to delete query on server  whe D/Ling data-->
<div id="confirmDelete" style="display: none; position: absolute; background-color: white; z-index: 100; padding: 20px; width: 150px; height: 70px">
	<table style="width: 100%" cellspacing="0" cellpadding="5">
	<tr>
		<td style="height: 80px">
		Type DELETE to delete data after it is downloaded:
		<input id="deleteDownload" type="text" style="margin-left: 10px; width: 60px" />
		</td>
	</tr>
	<tr>
		<td style="text-align: right; vertical-align: bottom">
		<input type="button" value="Canel" onclick="$('deleteDownload').setValue(''); $('confirmDelete').setStyle('display: none');" />
		<input type="button" value="Download" onclick="getCSV();" />
		</td>
	</tr>
	</table>
</div>

<!-- Dialog box to ask user for name of named settings -->
<div id="namedSettingsPrompt" style="display: none; position: absolute; background-color: white; z-index: 100; padding: 20px; width: 200px; height: 70px">
	<table style="width: 100%" cellspacing="0" cellpadding="5">
	<tr>
		<td style="height: 80px">
		What do you want to call the named settings?
		<input id="namedSettingsName" type="text" style="margin-top: 10px; width: 300px" maxlength="50" />
		</td>
	</tr>
	<tr>
		<td style="text-align: right; vertical-align: bottom">
		<input type="button" value="Cancel" onclick="$('namedSettingsName').setValue(''); $('namedSettingsPrompt').setStyle('display: none');" />
		<input type="button" value="Create" onclick="newNameSettingSave()" />
		</td>
	</tr>
	</table>
</div>

<!-- Dialog to ask user which named settings to delete -->
<div id="namedSettingsDeletePrompt" style="display: none; position: absolute; background-color: white; z-index: 100; padding: 20px; width: 200px; height: 70px">
	<table style="width: 100%" cellspacing="0" cellpadding="5">
	<tr>
		<td style="height: 80px">
		Which named settings value do you want to delete?
			<select id="nameSettingsDeleteList" style="width: 340px; margin-top: 5px">
				<option value="0">Select</option>
			</select>
		</td>
	</tr>
	<tr>
		<td style="text-align: right; vertical-align: bottom">
		<input type="button" value="Cancel" onclick="$('nameSettingsDeleteList').setValue(0); $('namedSettingsDeletePrompt').setStyle('display: none');" />
		<input type="button" value="Delete" onclick="deleteNameSetting()" />
		</td>
	</tr>
	</table>
</div>

<!-- Contains The Applications Options Panel -->
<div id="optionsCtrl" style="display: none; position: absolute; background-color: white; z-index: 100; padding: 20px; width: 250px; height: 140px">
	<table style="width: 100%" cellspacing="0" cellpadding="5">
	<tr>
		<td style="height: 35px">CSV Headers<input id="showCSVHeaders" type="checkbox" style="margin-left: 10px" onchange="optionsChange(false)"/></td>
	</tr>
	<tr>
		<td style="height: 35px">
		
		<table style="border: 1px solid black; width: 100%">
		<tr>
			<td colspan="3">Named Settings</td>
		</tr>
		<tr>
			<td colspan="3" style="height: 30px; vertical-align: middle">
				<select id="nameSettingsList" style="width: 237px" onchange="loadSettings()">
					<option value="0">Select</option>
				</select>
			</td>
		</tr>
		<tr>
			<td>
				<input type="button" value="New" style="width: 75px" onclick="newNameSettingPrompt()"/>
			</td>
			<td>
				<input type="button" value="Delete" style="width: 75px" onclick="deleteNameSettingPrompt()";/>
			</td>
			<td>
				<input type="button" value="Save" style="width: 75px" onclick="saveSettings()"/>
			</td>
		</tr>
		</table>		
		</td>
	</tr>
	<tr>
		<td style="height: 35px">
			<input type="button" value="Reset Settings" style="width: 250px" onclick="resetOptions()"/>
		</td>
	</tr>
	</table>
</div>

<!-- Contains and information about using the Data Manager -->
<div id="helpWindow" style="display: none; position: absolute; background-color: white; z-index: 100; padding: 10px; width: 700px; height: 300px">
<table style="width: 100%; height: 100%">
<tr>
	<td style="vertical-align: top">
	<div style="overflow: auto; height: 250px">
	<p>
	<b>Basic Usage:</b><br /><br />
	
	Select a campaign from the list, pick a query and press 'Run Query'. The system will 
	display the first 10 rows of data returned by the query.<br /><br />
	
	If you want to see all of the data returned by the query check the 'show all' option. 
	If there is a lot of data it may take a few minutes for the results to be returned
	from the server. Use the 'show all' option cautiously.<br /><br />
	
	You can edit most of the data returned from the server by clicking on a cell 
	and typing. As soon as you leave the cell the data will be saved. If the data
	cannot be saved because it formated improperly you will be warned and the data will
	be returned to it original value.<br /><br />
	
	When you have a cell selected you can move around the spread sheet by holding
	<b><i>ctrl</i></b> while pressing the arrow keys. If you do not hold <b><i>ctrl</i></b>
	the cursor will move around within the current cell.<br /><br />
	
	The following keyboard shortcuts are available when working with data:<br />
	Press <b><i>ctrl-a</i></b> to select all text in cell.<br />
	Press <b><i>ctrl-c</i></b> to copy selected text.<br />
	Press <b><i>ctrl-v</i></b> to paste into cell.<br />
	Press <b><i>ctrl-z</i></b> to undo the last change you made. You can only undo the last change made!<br /><br />
	
	To download a CSV version of the data press 'download'. The CSV file
	will contain all of the data returned by the query not just the first 10 rows
	displayed by default.<br /><br />
	
	You can hide columns by selecting the check next to their name then pressing 'options' 
	and finally pressing 'Hide Selected'. If you want to show columns that have been 
	hidden press 'options' and 'Unhide All'. The columns that have been hidden will not
	show up in CSV files that you download<br /><br />
	
	You can save all of the options associated with the campaign and query by pressing
	'options' and then pressing 'Save Options'. The options you save will only be 
	applied to the current campaign and query. Each time your return to the campaign
	and select the same query the options will be reloaded. 
	</p>
	</div>
	</td>
</tr>
<tr>
	<td style="text-align: right; vertical-align: bottom">
	<input type="button" value="Close" onclick="$('helpWindow').setStyle('display: none');" />
	</td>
</tr>
</table>
</div>

</body>
</html>