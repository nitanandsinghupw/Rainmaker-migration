var isCloseWindow = null;
var newWindow = null;

function ShowPageMessage()
{
    if(eval(document.getElementById("PageMessage"))){
        var messageText = document.getElementById("PageMessage").value;
        if(messageText != '')
        {
            alert(messageText);
        }
    }
    if (eval(document.getElementById("ConfirmMessage"))) {
        var confirmText = document.getElementById("ConfirmMessage").value;
        if (confirmText != '') {
            var conf = confirm(confirmText);
            if (conf == true) {
                document.getElementById("hdnAction").value = "add";
                __doPostBack('action','add');
            } else {
            document.getElementById("hdnPhone").value = "";
            document.getElementById("txtPhoneNumber").select();
            }
        }
    }
    if (eval(document.getElementById("ConfirmMessage2"))) {
        var confirmText = document.getElementById("ConfirmMessage2").value;
        if (confirmText != '') {
            var conf = confirm(confirmText);
            if (conf == true) {
                document.getElementById("hdnAction").value = "delete";
                __doPostBack('action','delete')
            } else {
                document.getElementById("hdnPhoneDelete").value = "";
                document.getElementById("txtPhoneNumber").select();
            }
        }
    }
}


function trimData(strMessage)
{
		var strResult;
		var charTemp;
		var i;
		strResult="";
		//remove the left space
		for(i=0;i<strMessage.length;i++){
			charTemp=strMessage.charAt(i);
			if(charTemp!=" "){
				strResult=strMessage.substring(i);
				break;
			}
		}
		//remove the right space
		for(i=strResult.length-1;i>=0;i=i-1){
			charTemp=strResult.charAt(i);
			if(charTemp!=" " ){
				strResult=strResult.substring(0,i+1);
				break;
			}
		}
		return(strResult);

}
function ShowPopUp(value, page) {
    var newDate = new Date();
    var timeStamp = newDate.getMonth() + "" + newDate.getDate() + "" + newDate.getYear()
	+ "" + newDate.getHours() + "" + newDate.getMinutes() + "" + newDate.getSeconds() + "" + newDate.getMilliseconds();

    if (value == 'Disposition') {
        
        var args = new Object;
        args.window = window; 
        var retValue = window.showModalDialog('../Agent/CallDisposition.aspx?pagefrom=' + page + '&ts=' + timeStamp, args);
        if ((retValue != undefined) && (retValue != '') && (retValue != false)) {
            $("#hdnDispose").val(retValue);
            __doPostBack('hdnDispose', '');

        }
        /*var dialogResults = window.showModalDialog(targetURL, null, dialogFeatures);

        if ((dialogResults != undefined) && (dialogResults != '') && (dialogResults != false)) {
            document.getElementById('someTextBox').value = dialogResults;
        }*/
        
        
        //window.open('../Agent/CallDisposition.aspx?pagefrom=' + page + '&ts=' + timeStamp, 'Agent', 'modal=yes,width=450,height=400,left=250,top=80,scrollbars=yes');
        return false;
    }

    if (value == 'Schedule') {
        //window.open('../Agent/ScheduleCallback.aspx?ts=' + timeStamp, 'Agent', 'width=450,height=350,left=250,top=120,scrollbars=no');
        var args = new Object;
        args.window = window;
        var retValue = window.showModalDialog('../Agent/ScheduleCallback.aspx?pagefrom=' + page + '&ts=' + timeStamp, args);
        
        if ((retValue != undefined) && (retValue != '') && (retValue != false)) {
            $("#hdnDispose").val(retValue);
            __doPostBack('hdnDispose', '');
        }    
        return false;
    }
}
function OpenAudioFile(AudioFile,FileUpload,Message,MultiBox) {

    
    isCloseWindow = FileUpload;
    var strAudioFileName = document.getElementById(AudioFile).value;
    
    if(document.getElementById(FileUpload).value != "") {
    
        strAudioFileName = document.getElementById(FileUpload).value;
        //newWindow = window.open('../Campaign/AudioPlay.aspx?IsBrowseAudioFileName=' + strAudioFileName,'AudioFile','width=350,height=350,left=((screen.width-795)/2),top=((screen.height)/2),scrollbars=yes');
        return strAudioFileName ;
    }
    if(strAudioFileName != "")
    {
        MultiBoxValue = document.getElementById(MultiBox).value;
        if (MultiBoxValue != "") {
            
            strAudioFileName = MultiBoxValue + strAudioFileName.split("\\").pop();
            strAudioFileName = strAudioFileName.replace(/"/, /\"/);
            strAudioFileName = strAudioFileName.replace("\\", "\\\\");
        }
        var thefile = strAudioFileName ;
        return thefile;
        //newWindow = window.open('../Campaign/AudioPlay.aspx?IsEditAudioFileName=' + strAudioFileName,'AudioFile','width=350,height=350,left=((screen.width-795)/2),top=((screen.height)/2),scrollbars=yes');
       
    }
    alert(Message);
    return "error";
    
}
    
function CloseWindow(ID)
{
    if(isCloseWindow == ID)
    {
        if(newWindow != null)
        {
	        if (navigator.appName.indexOf("Netscape") != -1)
	        {
		        if(newWindow.window)
		        {
	                newWindow.close();
			        return false;
                }
	        }   
            else
	        {
		        newWindow.close();
		        return false;
	        }
	        isCloseWindow = null;
            newWindow = null
            return false;
        }
    }
    return false;
}

function fileChange(inputFileId, extensionStr, allowedFileMessage)
{

	var txtval = document.getElementById(inputFileId).value;	
	txtval = txtval.substring(txtval.lastIndexOf(".")+1, txtval.length);
	if(trimData(extensionStr) != ""){
		var extensions = extensionStr.split(",");
		var fileMatch = false;
		for(i=0;i < extensions.length; i++){
			if(txtval.toLowerCase() == extensions[i]){
				fileMatch = true;
				break;					
			}
		}
		if(!fileMatch){
			alert(allowedFileMessage);
			clearFileUploadContent(inputFileId);
			return false;
        }
        switch (inputFileId) {

            case "FileUploadMachineToPlay":
                eval($('#lbtnUploadMachine').attr('href'));
                break;
            case "FileUploadHumanToPlay":
                eval($('#lbtnUploadHuman').attr('href'));
                break;
            case "FileUploadSilentCallToPlay":
                eval($('#lbtnUploadDrop').attr('href'));
                break;
        }
        disableRadioButtons();
        
	}
	return true;
}

function clearFileUploadContent(control)
{
    var isSafari=false;
    if(!isSafari)
    {
        var who=document.getElementsByName(control)[0];    
        var who2= who.cloneNode(false);    
        who2.onchange= who.onchange;    
        who.parentNode.replaceChild(who2,who);
    }
    else
    {
        // we need to provide one td for every file upload control, id must be equal to td + file upload control id
        document.getElementById("td"+control).innerHTML = document.getElementById("td"+control).innerHTML;
    }
}

function GetPopupData(value, page)
{  
	var newDate = new Date();
	var timeStamp = newDate.getMonth()+""+newDate.getDate()+""+newDate.getYear()
	+""+newDate.getHours()+""+newDate.getMinutes()+""+newDate.getSeconds()+""+newDate.getMilliseconds();

    if(value=='Disposition') {
    
        return ("../Agent/CallDisposition.aspx?pagefrom=" + page + "&ts=" + timeStamp);
        
    }

    if (value == 'Schedule') {

        return ("../Agent/ScheduleCallback.aspx?pagefrom=" + page + "&ts=" + timeStamp);
        
    }
}

var Cal_PopUp_CalWindowObject;
var Cal_PopUp_dateObject;
var CDateFormat = "MM/dd/yyyy";
var SDateFormat = "MM/dd/yyyy";

function CloseWindow(){
	if(newWindow != null)
	{
		if (navigator.appName.indexOf("Netscape")!=-1)
		{
			if(newWindow.window)
				newWindow.close();
		}else
			newWindow.close();

		newWindow = null
	}
}

function Cal_PopUp_DoNothing()
{
	return;
}
function Lessthantodaydate(Currentdate,fieldvalue,field){
    
    var todaydt = Currentdate;
     // The following variable(TrimedDate) is used to trim the Date value.
    var trimedDate = trimData(fieldvalue)

      var gettoday = Currentdate; 
      var date =Currentdate.split("/")[1];
      var month = Currentdate.split("/")[0];
      var year = Currentdate.split("/")[2];
      
     var fday = parseFloat(trimedDate.split("/")[1]);
     var fmonth = parseFloat(trimedDate.split("/")[0]);
     var fyear = parseFloat(trimedDate.split("/")[2]);
	 var errmsg = "";
 
	if(fyear < year){
	    errmsg = 'Date Cannot be Lesser than Current Date.';
	}else if(fmonth < month ){
	    if(fyear == year){
	        errmsg = 'Date Cannot be Lesser than Current Date.';
	     }
	}else if(fyear == year){
	        if(fmonth == month){
	        
	            if(fday < date){
	                errmsg = 'Date Cannot be Lesser than Current Date.';   
	            }
            }
	}
	
	if(errmsg != ''){
	    alert(field +errmsg);
	    return true;
	}
	else{
	    return false;
	}
}

function CurrentDateFormat(field,cdf,sdf){
    
	var arr1 = new Array("","",""); //holds MM, dd, yyyy
	var strSeparatorArray = new Array(". ","-"," ","/",".");
	var intElementNr;
	var splitStr = ""
	var CDateArray;
	var strDateArray;
	var SDateSplitStr="";
	var SDateArray;
	for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) 
	{
		if(cdf.indexOf(strSeparatorArray[intElementNr]) != -1 && splitStr==""){
			splitStr = strSeparatorArray[intElementNr];
			CDateArray = cdf.split(splitStr)
		}
		if(sdf.indexOf(strSeparatorArray[intElementNr]) != -1 && SDateSplitStr==""){
			SDateSplitStr = strSeparatorArray[intElementNr];
			SDateArray = sdf.split(SDateSplitStr)
			//alert(SDateArray +" "+SDateSplitStr)
		}
	}
	intElementNr = 0
	for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) 
	{
		if (field.value.indexOf(strSeparatorArray[intElementNr]) != -1){
			strDateArray = field.value.split(strSeparatorArray[intElementNr]);
			for (intEltNr = 0; intEltNr < CDateArray.length; intEltNr++){
				if(CDateArray[intEltNr].indexOf("y")!=-1){
					arr1[2] = strDateArray[intEltNr];					
				}else if(CDateArray[intEltNr].indexOf("M")!=-1){
					arr1[0] = strDateArray[intEltNr];													
				}else if(CDateArray[intEltNr].indexOf("d")!=-1){
					arr1[1] = strDateArray[intEltNr];
					
					if (arr1[1].length>2){							
						arr1[1] = arr1[1].substring(0,2)
					}
					//alert("################" + arr1[1])
				}
			}	
			//field.value = arr1.join("/")	
		}
	}
	
	var fdate =  new Array ("","","")//CDateArray.clone();
	for (intElementNr = 0; intElementNr < SDateArray.length; intElementNr++) 
	{
		if (intElementNr < fdate.length){
			if(SDateArray[intElementNr].indexOf("y")!=-1){
				//if (SDateArray[intElementNr].length > 2 && arr1[2].length < 3)
				//fdate[intElementNr] = "20" + arr1[2];
				//else
				fdate[intElementNr] = arr1[2]				
			}else if(SDateArray[intElementNr].indexOf("M")!=-1){
				//if (SDateArray[intElementNr].length<2 || arr1[0]*1 > 10)
					fdate[intElementNr] = arr1[0]
				//else
				//	fdate[intElementNr] = "0"+arr1[0]
										
			}else if(SDateArray[intElementNr].indexOf("d")!=-1){
				//if (SDateArray[intElementNr].length<2 || arr1[1]*1 > 10)
					fdate[intElementNr] = arr1[1]
				//else{
				//	fdate[intElementNr] = "0"+arr1[1]
				//}				
			}
		}
	}
	//alert(field.value + " = " + fdate.join(SDateSplitStr))
	field.value = fdate.join(SDateSplitStr)
	
}


function SetCurrentDateFormat(field,cdf,sdf){
	var arr1 = new Array("","",""); //holds MM, dd, yyyy
	var strSeparatorArray = new Array(". ","-"," ","/",".");
	var intElementNr;
	var splitStr = ""
	var CDateArray;
	var strDateArray;
	var SDateSplitStr="";
	var SDateArray;
	for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) 
	{
		if(cdf.indexOf(strSeparatorArray[intElementNr]) != -1 && splitStr==""){
			splitStr = strSeparatorArray[intElementNr];
			CDateArray = cdf.split(splitStr)
		}
		if(sdf.indexOf(strSeparatorArray[intElementNr]) != -1 && SDateSplitStr==""){
			SDateSplitStr = strSeparatorArray[intElementNr];
			SDateArray = sdf.split(SDateSplitStr)
			//alert(SDateArray +" "+SDateSplitStr)
		}
	}
	intElementNr = 0
	for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) 
	{
		if (field.indexOf(strSeparatorArray[intElementNr]) != -1){
			strDateArray = field.split(strSeparatorArray[intElementNr]);
			for (intEltNr = 0; intEltNr < CDateArray.length; intEltNr++){
				if(CDateArray[intEltNr].indexOf("y")!=-1){
					arr1[2] = strDateArray[intEltNr];					
				}else if(CDateArray[intEltNr].indexOf("M")!=-1){
					arr1[0] = strDateArray[intEltNr];													
				}else if(CDateArray[intEltNr].indexOf("d")!=-1){
					arr1[1] = strDateArray[intEltNr];
					
					if (arr1[1].length>2){							
						arr1[1] = arr1[1].substring(0,2)
					}
					//alert("################" + arr1[1])
				}
			}	
			//field.value = arr1.join("/")	
		}
	}
	
	var fdate =  new Array ("","","")//CDateArray.clone();
	for (intElementNr = 0; intElementNr < SDateArray.length; intElementNr++) 
	{
		if (intElementNr < fdate.length){
			if(SDateArray[intElementNr].indexOf("y")!=-1){
				//if (SDateArray[intElementNr].length > 2 && arr1[2].length < 3)
				//fdate[intElementNr] = "20" + arr1[2];
				//else
				fdate[intElementNr] = arr1[2]				
			}else if(SDateArray[intElementNr].indexOf("M")!=-1){
				//if (SDateArray[intElementNr].length<2 || arr1[0]*1 > 10)
					fdate[intElementNr] = arr1[0]
				//else
				//	fdate[intElementNr] = "0"+arr1[0]
										
			}else if(SDateArray[intElementNr].indexOf("d")!=-1){
				//if (SDateArray[intElementNr].length<2 || arr1[1]*1 > 10)
					fdate[intElementNr] = arr1[1]
				//else{
				//	fdate[intElementNr] = "0"+arr1[1]
				//}				
			}
		}
	}
	//alert(field.value + " = " + fdate.join(SDateSplitStr))
	//field.value = fdate.join(SDateSplitStr)
	return fdate.join(SDateSplitStr);
	
}



function checkDate(objName){

	if(!validDate(objName)){
		alert("Date should be in \"" + CDateFormat + "\" format and \nbetween \""+ SetCurrentDateFormat("01/01/1900",SDateFormat,CDateFormat) +"\" and \""+SetCurrentDateFormat("12/31/9999",SDateFormat,CDateFormat)+"\"")
		objName.focus();
		objName.select();
		return false;
	}
	return true;
}

function validDate(objName)
{
    
	var strDatestyle = "US"; //United States date style
	var strDate;
	var strDateArray;
	var strDay;
	var strMonth;
	var strYear;
	var intday;
	var intMonth;
	var intYear;
	var booFound = false;
	var datefield = objName;
	var strSeparatorArray = new Array(". ","-"," ","/",".");
	var intElementNr;
	var err = 0;
	var splitStr = ""
	var CDateArray;

	var strMonthArray = new Array(12);
	strMonthArray[0] = "Jan";
	strMonthArray[1] = "Feb";
	strMonthArray[2] = "Mar";
	strMonthArray[3] = "Apr";
	strMonthArray[4] = "May";
	strMonthArray[5] = "Jun";
	strMonthArray[6] = "Jul";
	strMonthArray[7] = "Aug";
	strMonthArray[8] = "Sep";
	strMonthArray[9] = "Oct";
	strMonthArray[10] = "Nov";
	strMonthArray[11] = "Dec";

	datefield.value = trimData(datefield.value);
	strDate = datefield.value;
	
	var x=new RegExp("[^. 0-9/-]");
		
	if (strDate.length < 1){
			return true;
	}if(strDate.search(x)!=-1){
		return false;
	}
	for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) 
	{
		if(CDateFormat.indexOf(strSeparatorArray[intElementNr]) != -1 && splitStr==""){
			splitStr = strSeparatorArray[intElementNr];
			CDateArray = CDateFormat.split(splitStr)
		}
	}
	for (intElementNr = 0; intElementNr < strSeparatorArray.length; intElementNr++) 
	{
		if (strDate.indexOf(strSeparatorArray[intElementNr]) != -1) 
		{
			strDateArray = strDate.split(strSeparatorArray[intElementNr]);
			//alert(strDateArray)
			if (strDateArray.length != 3){
				err = 1;
				return false;
			}else{
				for (intEltNr = 0; intEltNr < CDateArray.length; intEltNr++){
					if(CDateArray[intEltNr].indexOf("y")!=-1){
						strYear = strDateArray[intEltNr];					
					}else if(CDateArray[intEltNr].indexOf("M")!=-1){
						strMonth = strDateArray[intEltNr];													
					}else if(CDateArray[intEltNr].indexOf("d")!=-1){
						strDay = strDateArray[intEltNr];
						if (strDateArray[intEltNr].length>2){							
							strDay = strDateArray[intEltNr].substring(0,2)
						}
					}
				}
				//alert("###############Date["+strDay+"]Month["+strMonth+"]Year["+strYear+"]")
					/*strDay = strDateArray[0];
					strMonth = strDateArray[1];
					strYear = strDateArray[2];*/
			}
			booFound = true;
		 }
	}
	
	if (booFound == false){
		if ((strDate.length>5)&&(strDate.length<9)&&(strDate.length!=7)){
			var tempInt = 0 ;
			//alert(CDateArray)
			for (intEltNr = 0; intEltNr < CDateArray.length; intEltNr++) 
			{
				if(CDateArray[intEltNr].indexOf("y")!=-1){
					if(strDate.length == 6){
						strYear =  strDate.substr(tempInt,2);			
						tempInt = tempInt + 2
					}else{
						strYear =  strDate.substr(tempInt,4);		
						tempInt = tempInt + 4
					}
				}else if(CDateArray[intEltNr].indexOf("M")!=-1){
					strMonth = strDate.substr(tempInt, 2);
					tempInt = tempInt + 2
				}else if(CDateArray[intEltNr].indexOf("d")!=-1){
					strDay = strDate.substr(tempInt, 2);
					tempInt = tempInt + 2
				}
			}
			//alert(">>>>>>>Date["+strDay+"]Month["+strMonth+"]Year["+strYear+"]")
		}else{
			return false;
		}
	}
	if (strYear.length == 2){
			strYear = '20' + strYear;
	}

	// US style
	/*	if (strDatestyle == "US")
		{
				strTemp = strDay;
				strDay = strMonth;
				strMonth = strTemp;
		}
	*/
	intday = parseInt(strDay, 10);
	if (isNaN(intday)){
		err = 2;
		return false;
	}
	intMonth = parseInt(strMonth, 10);
	if (isNaN(intMonth))
	{
		for (i = 0;i<12;i++)
		{
			if (strMonth.toUpperCase() == strMonthArray[i].toUpperCase()){
					intMonth = i+1;
					strMonth = strMonthArray[i];
					i = 12;
			}
		}if (isNaN(intMonth)){
				err = 3;
				return false;
		}
	}
	intYear = parseInt(strYear, 10);
	if (isNaN(intYear)){
		err = 4;
		return false;
	}
	if (intMonth>12 || intMonth<1) {
			err = 5;
			return false;
	}
	if ((intMonth == 1 || intMonth == 3 || intMonth == 5 || intMonth == 7 || intMonth == 8 || intMonth == 10 || intMonth == 12) && (intday > 31 || intday < 1)) {
			err = 6;
			//alert("This is not a valid calender date \n This month contains only 31 days.");
			return false;
	}
	if ((intMonth == 4 || intMonth == 6 || intMonth == 9 || intMonth == 11) && (intday > 30 || intday < 1)){
			err = 7;
			//alert("This is not a valid calender date \n This month contains only 30 days.");
			return false;
	}
	if (intMonth == 2){
		if (intday < 1){
				err = 8;
				return false;
		}
		if (LeapYear(intYear) == true){
			if (intday > 29){
				err = 9;
				//alert("This is not a valid calender date \n This month contains only 29 days.");
				return false;
			}
		}else{
			if (intday > 28){
				err = 10;
				//alert("This is not a valid calender date \n This month contains only 28 days.");
				return false;
			}
		}
	}

	var fdate =  new Array ("","","")//CDateArray.clone();
	if (strDatestyle == "US"){
		for (intElementNr = 0; intElementNr < CDateArray.length; intElementNr++) 
		{
			if (intElementNr < fdate.length){
				if(CDateArray[intElementNr].indexOf("y")!=-1){
					if (CDateArray[intElementNr].length>2)
						fdate[intElementNr] = strYear
					else
						fdate[intElementNr] = strYear.substring(2)
					
				}else if(CDateArray[intElementNr].indexOf("M")!=-1){
					if (CDateArray[intElementNr].length<2 || intMonth >= 10)
						fdate[intElementNr] = intMonth
					else
						fdate[intElementNr] = "0"+intMonth
											
				}else if(CDateArray[intElementNr].indexOf("d")!=-1){
					if (CDateArray[intElementNr].length<2 || intday >= 10)
						fdate[intElementNr] = intday
					else{
						fdate[intElementNr] = "0"+intday
					}

					if (CDateArray[intElementNr].length==3)
						fdate[intElementNr] = fdate[intElementNr] + ((CDateArray[intElementNr]).substring(2))
					else if (CDateArray.length==4)
						fdate[intElementNr] = fdate[intElementNr] + splitStr
				}
			}
		}
		datefield.value = fdate.join(splitStr)
	}else{
		datefield.value = intday + " " + strMonthArray[intMonth-1] + " " + strYear;
	}
	return true;
}

function LeapYear(intYear)
{
	if (intYear % 100 == 0)
	{
			if (intYear % 400 == 0)
			{
					return true;
			}
	}
	else
	{
			if ((intYear % 4) == 0)
			{
					return true;
			}
	}
	return false;
}


        var timerID = 0;   
        function displayLoading()
		{
		    //timerID = setTimeout("showProgress()",100);
		    window.frames['iframeProgress'].window.startProgress();
		}
		
		//var progressVal = 10;
		var screenHeight = Math.floor(window.screen.availHeight * 86 / 100);
		function showProgress(progressVal)
		{
		    var height = document.body.clientHeight;
		    if(height < screenHeight)
		        height = screenHeight;
		    if(document.getElementById('loading').style.visibility == "hidden"){
		        document.getElementById('loading').style.height = height + 'px';					
		        document.getElementById('loading').style.width = document.body.clientWidth + 'px';
			    document.getElementById('loading').innerHTML = '<table width="100%" height="100%" align="center"><tr><td align="center"><span style="color:#ffffff">PLEASE WAIT....</span><TABLE cellspacing=0 cellpadding=0 border=1 style="width:400px; height:13px" ID="Table1"><TR><TD bgcolor=#008800 width="'+ progressVal + '%"></TD><TD bgcolor=#FFFFFF></TD></TR></TABLE></td></tr></table>';
			    document.getElementById('loading').style.visibility = 'visible';
			}else{
			    document.getElementById('loading').innerHTML = '<table width="100%" height="100%" align="center"><tr><td align="center"><span style="color:#ffffff">PLEASE WAIT....</span><TABLE cellspacing=0 cellpadding=0 border=1 style="width:400px; height:13px" ID="Table1"><TR><TD bgcolor=#008800 width="'+ progressVal + '%"></TD><TD bgcolor=#FFFFFF></TD></TR></TABLE></td></tr></table>';
			}
		}
		
		function showExport(progressVal)
		{
		    var height = document.body.clientHeight;
		    if(height < screenHeight)
		        height = screenHeight;
		    if(document.getElementById('loading').style.visibility == "hidden"){
		        document.getElementById('loading').style.height = '195px';					
		        document.getElementById('loading').style.width = '410px';
			    document.getElementById('loading').innerHTML = '<table width="100%" height="100%" align="center"><tr><td align="center"><span style="color:#ffffff">PLEASE WAIT....</span><TABLE cellspacing=0 cellpadding=0 border=1 style="width:400px; height:13px" ID="Table1"><TR><TD bgcolor=#008800 width="'+ progressVal + '%"></TD><TD bgcolor=#FFFFFF></TD></TR></TABLE></td></tr></table>';
			    document.getElementById('loading').style.visibility = 'visible';
			}else{
			    document.getElementById('loading').innerHTML = '<table width="100%" height="100%" align="center"><tr><td align="center"><span style="color:#ffffff">PLEASE WAIT....</span><TABLE cellspacing=0 cellpadding=0 border=1 style="width:400px; height:13px" ID="Table1"><TR><TD bgcolor=#008800 width="'+ progressVal + '%"></TD><TD bgcolor=#FFFFFF></TD></TR></TABLE></td></tr></table>';
			}
		}
		
		//var progressVal = 10;
		var screenHeight = Math.floor(window.screen.availHeight * 86 / 100);
		function showProgress(progressVal)
		{
		    var height = document.body.clientHeight;
		    if(height < screenHeight)
		        height = screenHeight;
		    if(document.getElementById('loading').style.visibility == "hidden"){
		        document.getElementById('loading').style.height = height + 'px';					
		        document.getElementById('loading').style.width = document.body.clientWidth + 'px';
			    document.getElementById('loading').innerHTML = '<table width="100%" height="100%" align="center"><tr><td align="center"><span style="color:#ffffff">PLEASE WAIT....</span><TABLE cellspacing=0 cellpadding=0 border=1 style="width:400px; height:13px" ID="Table1"><TR><TD bgcolor=#008800 width="'+ progressVal + '%"></TD><TD bgcolor=#FFFFFF></TD></TR></TABLE></td></tr></table>';
			    document.getElementById('loading').style.visibility = 'visible';
			}else{
			    document.getElementById('loading').innerHTML = '<table width="100%" height="100%" align="center"><tr><td align="center"><span style="color:#ffffff">PLEASE WAIT....</span><TABLE cellspacing=0 cellpadding=0 border=1 style="width:400px; height:13px" ID="Table1"><TR><TD bgcolor=#008800 width="'+ progressVal + '%"></TD><TD bgcolor=#FFFFFF></TD></TR></TABLE></td></tr></table>';
			}
		}

		function hideLoading()
		{
		    document.getElementById('loading').innerHTML = '';
			document.getElementById('loading').style.visibility = 'hidden';
			try
			{
			    clearTimeout(timerID);
			}
			catch (e) {
                //alert('error');
            }
		}

        function IsNumeric(sText)
        {
           var ValidChars = "0123456789.,";
           var IsNumber=true;
           var Char;
           if(sText.indexOf(".,") > -1){
		        IsNumber = false;
	        }
        	
           if(sText.split(".").length > 2){
	        IsNumber = false;
           }
           if(sText.split(".").length > 1){
		        var commval = sText.split(".")[1];
		        if(commval.indexOf(".,") > -1){
			        IsNumber = false;
		        }
           }
           if(sText.split(",").length > 3){
	        IsNumber = false;
           }
           sText = sText.split(",").join("");
           for (ic = 0; ic < sText.length && IsNumber == true; ic++) 
           { 
              Char = sText.charAt(ic); 
              if (ValidChars.indexOf(Char) == -1) 
              {
                 IsNumber = false;
              }
           }
           
           if (sText < 0 ){
	           IsNumber = false;
	       }
        	  
           return IsNumber;
       }


    /* check for Phone and Zip Characters */
    function IsNumber(sText)
    {
       var ValidChars = "0123456789";
       var IsNum=true;
       var Char;

       for (ic = 0; ic < sText.length && IsNum == true; ic++) 
       { 
          Char = sText.charAt(ic); 
          if (ValidChars.indexOf(Char) == -1) 
          {
             IsNum = false;
          }
       }
       return IsNum;
     }
       
     function checkDateWithTime(objName)
     {
            var value =  objName.value;
            var dateAndTime = objName.value.split(" ");
            
            var  newObj = objName;
            newObj.value = dateAndTime[0];
            var time = "";
            if(dateAndTime.length > 1)
                time = dateAndTime[1];
            if(trimData(time) == "")
            {
                time = "00:00:00";
            }
            
	        if(!validDate(objName) || !checktime(time))
	        {
		        alert("Date should be in \"" + CDateFormat + " hh:mm:ss" + "\" format")
		        objName.focus();
		        objName.select();
		        objName.value = value;
		        return false;
	        }
	        objName.value = value;
	        return true;
     }
     
     function checktime(thetime) 
     {
        var a,b,c,f,x,y,err=0;
        a=thetime;
        if (a.length != 8) err=1;
        b = a.substring(0, 2);
        c = a.substring(2, 3); 
        f = a.substring(3, 5);
        x = a.substring(5, 6);
        y = a.substring(6, 8);
        if (/\D/g.test(b)) err=1; //not a number
        if (/\D/g.test(f)) err=1; 
        if (b<0 || b>23) err=1;
        if (f<0 || f>59) err=1;
        if (y<0 || y>59) err=1;
        if (c != ':' || x != ':') err=1;
        if (err==1) {
            return false
        }
        return true;
     }
       
     function TruncateData(control, length)
     {
        try
        {
            var len = parseInt(length);
            if(len > 0 && control.value.length > len)
            {
                control.value = control.value.substring(0, len-1);
            }
        }
	    catch (e)
	    {
		    return null ;
	    }
     }