<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ImportMappings.aspx.cs"
    Inherits="Rainmaker.Web.campaign.ImportMappings" %>

<%@ Register TagPrefix="RainMaker" TagName="Footer" Src="~/common/controls/Footer.ascx" %>
<%@ Register TagPrefix="RainMaker" TagName="Header" Src="~/common/controls/CampaignHeader.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rainmaker Dialer - Import Mappings</title>
    <style type="text/css">
        #loading
        {
            z-index: 1000000000;
            background-image: url(../images/greyout.png);
            visibility: hidden;
            background-repeat: repeat-x;
            position: absolute;
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

    <script type="text/javascript" src="../js/jquery.js"></script>

    <script type="text/javascript">

var curImportMapID = <asp:Literal runat="server" id="curImportMapIDValue"/>;
var curImportMap = Array(<asp:Literal runat="server" id="curImportMap"/>);

$(document).ready
	(
	function() 
		{
		$("#templateList").change(importMapPicked);
		$("#saveImportMap").click(saveImportMap);
		$("#createImportMap").click(createImportMap);
		$("#deleteImportMap").click(deleteImportMap);
		$("#renameImportMap").click(renameImportMap);
		loadImportMap();
		}
	);

var mapValue;
var curMapField = 0;
function loadImportMap()
	{
	if(curImportMapID < 1) return;
	curMapField = 0;
	var fields = $(".campaignColumn").each(reloadingCampaignField);
	}

function reloadingCampaignField()
	{
	if(curMapField >= curImportMap.length) return;
	$(this).children("option:selected").removeAttr('selected');
	mapValue = curImportMap[curMapField++];
	$(this).children().each(displayMe);
	}

function displayMe()
	{
	if($(this).attr('value') == mapValue.value)
		{
		$(this).attr('selected', 'true');
		}
	}

var campaignColumnCnt = 0;
var campaignColumnMap = "";
function saveImportMap()
	{
	if(curImportMapID == 0)
		{
		alert('You must create or choose an import map in order to save!');
		return;
		}
	campaignColumnCnt = 0;
	campaignColumnMap = "";
	$(".campaignColumn").each(readCampainColumnVal);
	var path = window.location.pathname;
	path += "?action=save_import_map&curImportMapID=" + curImportMapID + campaignColumnMap;
	window.location = path;
	}

function readCampainColumnVal()
	{
	campaignColumnMap += "&";
	campaignColumnMap += (campaignColumnCnt + "=" + $(this).val());
	campaignColumnCnt++;
	}

function renameImportMap()
	{
	if(curImportMapID == 0)
		{
		alert('You cannot rename the select message!');
		return;
		}
	var newName = prompt("Map Name: ");
	if(newName == null) return;
	var path = window.location.pathname;
	path += "?action=rename_import_map&curImportMapID=" + curImportMapID + "&name=" + newName;
	window.location = path;
	}

function createImportMap()
	{
	var path = window.location.pathname;
	path += "?action=create_import_map";
	window.location = path;
	}

function deleteImportMap()
	{
	if(curImportMapID == 0)
		{
		alert('You cannot delete the select message!');
		return;
		}
	var path = window.location.pathname;
	path += "?action=delete_import_map&curImportMapID=" + curImportMapID;
	window.location = path;
	}

function importMapPicked()
	{
	var path = window.location.pathname;
	path += "?action=load_import_map&curImportMapID=" + $("#templateList").val();
	window.location = path;
	}
    </script>

</head>
<body onload="ShowPageMessage();hideLoading();">
    <div id="loading">
    </div>
    <form id="frmImportMappings" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>

    <script language="javascript" type="text/javascript" src="../js/ErrHandler.js"></script>

    <table cellpadding="0" cellspacing="1" border="0" width="992px" class="tdHeader">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                    <tr>
                        <td>
                            <RainMaker:Header ID="campaignHeader" runat="server"></RainMaker:Header>
                            <!-- Body -->
                            <table cellpadding="0" cellspacing="0" height="500px" border="0" width="100%">
                                <tr>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                            <tr>
                                                <td valign="top">
                                                    <table cellpadding="0" cellspacing="1" border="0" width="100%">
                                                        <tr>
                                                            <td align="left" width="100%">
                                                                <table cellpadding="1" cellspacing="1" width="80%" border="0">
                                                                    <tr>
                                                                        <td align="left" width="100%">
                                                                            <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                                                                <tr>
                                                                                    <td valign="bottom" align="left">
                                                                                        <a href="Home.aspx" class="aHome" runat="server" id="anchHome">
                                                                                            <asp:Label ID="lblCampaign" runat="server" Text="(CompaignName)"></asp:Label>
                                                                                        </a>&nbsp;&nbsp;<img src="../images/arrowright.gif" alt="" />&nbsp;&nbsp;<b>Import</b>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <!-- Jeff: Added this to provide for templates -->
                                                                    <tr>
                                                                        <td style="padding: 5px; padding-left: 15px;">
                                                                            Import Map
                                                                            <select id="templateList" style="margin-left: 10px; width: 360px">
                                                                                <asp:Literal runat="server" ID="importMapList" />
                                                                            </select>
                                                                            <input id="createImportMap" type="button" value="Create" style="width: 70px" />
                                                                            <input id="renameImportMap" type="button" value="Rename" style="width: 70px" />
                                                                            <input id="deleteImportMap" type="button" value="Delete" style="width: 70px" />
                                                                            <input id="saveImportMap" type="button" value="Save" style="width: 70px" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr valign="bottom">
                                                                        <td align="left" valign="bottom">
                                                                            &nbsp;&nbsp;<asp:LinkButton ID="lbtnBack1" runat="server" PostBackUrl="~/campaign/Import.aspx" Text="Back" CssClass="button blue small"></asp:LinkButton>
                                                                            &nbsp;&nbsp;<asp:LinkButton ID="lbtnUpload1" runat="server" OnClick="lbtnUpload_Click"
                                                                                OnClientClick="displayLoading();" Text="Upload" CssClass="button blue small"></asp:LinkButton>
                                                                            &nbsp;&nbsp;<asp:LinkButton ID="lbtnClose1" runat="server" CausesValidation="false"
                                                                                PostBackUrl="~/campaign/home.aspx" Text="Close" CssClass="button blue small"></asp:LinkButton>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="middle" align="center" width="100%">
                                                                            <table cellspacing="5" cellpadding="5" width="100%" border="0">
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <asp:GridView runat="server" AutoGenerateColumns="False" ID="grdMappingList" Width="100%"
                                                                                            CellPadding="3" CellSpacing="1" BorderWidth="0" CssClass="tablecontentBlack"
                                                                                            OnRowDataBound="grdMappingList_RowDataBound">
                                                                                            <HeaderStyle CssClass="tableHdr" />
                                                                                            <RowStyle CssClass="tableRow" />
                                                                                            <Columns>
                                                                                                <asp:TemplateField HeaderText="Table Column">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:DropDownList ID="campaignColumns" runat="server" CssClass="dropDownList campaignColumn">
                                                                                                        </asp:DropDownList>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:TemplateField HeaderText="Record Header">
                                                                                                    <ItemTemplate>
                                                                                                        <asp:DropDownList ID="sourceColumns" runat="server" CssClass="dropDownList">
                                                                                                        </asp:DropDownList>
                                                                                                    </ItemTemplate>
                                                                                                </asp:TemplateField>
                                                                                                <asp:BoundField DataField="SampleData" HeaderText="Sample Data" />
                                                                                            </Columns>
                                                                                        </asp:GridView>
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
                                    <td align="left" valign="bottom">
                                        &nbsp;&nbsp;<asp:LinkButton ID="lbtnBack" runat="server" PostBackUrl="~/campaign/Import.aspx"><img src="" class="myButton" alt="Back" border="0" alt=""/></asp:LinkButton>
                                        &nbsp;&nbsp;<asp:LinkButton ID="lbtnUpload" runat="server" OnClick="lbtnUpload_Click"
                                            OnClientClick="displayLoading();"><img src="" class="myButton" alt="Upload" border="0" alt=""/></asp:LinkButton>
                                        &nbsp;&nbsp;<asp:LinkButton ID="lbtnClose" runat="server" CausesValidation="false"
                                            PostBackUrl="~/campaign/home.aspx"><img src="" class="myButton" alt="Close" border="0" alt=""/></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                            <!-- Body -->
                            <iframe id="iframeProgress" src="progressbar.html" width="0" height="0" frameborder="0">
                            </iframe>
                            <!-- Footer -->
                            <RainMaker:Footer ID="Footer" runat="server"></RainMaker:Footer>
                            <!-- Footer -->
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
