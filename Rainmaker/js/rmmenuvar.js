//Menu Width, Height, FontFace, FontSize, FontColor, FontHighlite, 
//MenuBgcolor, MenuBghiglite, MenuBorder, MenuItemBorder Spec

//Specify the root dir.
//var root="../";
var MenuHeight=20;
var FontFace="verdana,arial,helvetica";
var FontSize = 12;
var MenuBorder=1;
var MenuItemBorder=1;


// Menu 1 Spec
var Menu01Width=166;
var Menu01FontColor="#000000";
var Menu01FontHighliteColor="#FFFFFF";
var Menu01Bgcolor="#D1D7DF";
var Menu01BgHighlite="#395676";
var Menu01BorderColor="#395676";
var Menu01ItemBorderColor="#395676";
var Menu01LiteBgcolor="#FFFFFF";


// Menu 
	window.Menu01 = new Menu("Menu01", Menu01Width, MenuHeight, FontFace, FontSize, Menu01FontColor, Menu01FontHighliteColor, Menu01Bgcolor, Menu01BgHighlite, MenuBorder, MenuItemBorder, Menu01BorderColor, Menu01ItemBorderColor, Menu01LiteBgcolor);
	Menu01.addMenuItem("Agents","../agent/AgentList.aspx");
	Menu01.addMenuItem("Agent Stations","../agent/StationList.aspx");
	Menu01.addMenuItem("'Call Me' - Prompt Recorder","../campaign/CallMeRecorder.aspx");
	Menu01.addMenuItem("Global Dialing Prefix/Suffix","../campaign/GlobalDialingParams.aspx");
	Menu01.hideOnMouseOut=true;	
	
	
// This line must come after all the configurations
	Menu01.writeMenus();
	
