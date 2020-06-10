
var WindowWidth=getWindowWidth();
var a=a1()

function a1()
{
if (((document.body.clientWidth-750)/2) > 0)
a=((document.body.clientWidth-750)/2);
else
a=0;
return a
}

//alert(WindowWidth/2);
//var Menu01Left= a+185;
var Menu01Left= 85;

var arrSelectElements = new Array();

//var Menu02Left=244;
var MenuTop=120;

function getWindowWidth(){
	if (ie4){
		WindowWidth=document.body.clientWidth;
	}
	if (ns4){
		WindowWidth=window.innerWidth;
		WindowWidth-=45;
	}
	if (ie5){
		WindowWidth=document.body.clientWidth;
		WindowWidth-=30;
	}
	return WindowWidth;
}


function hideSelectElements(x,y,w,h){ 
	var selx,sely,selw,selh,i 
	var sel=document.getElementsByTagName("SELECT") 
	for(i=0;i<sel.length;i++){ 
		selx=0; sely=0; var selp; 
		if(sel[i].offsetParent){ 
			selp=sel[i]; 
			while(selp.offsetParent){ 
				selp=selp.offsetParent; 
				selx+=selp.offsetLeft; 
				sely+=selp.offsetTop; 
			} 
		} 
		selx+=sel[i].offsetLeft; 
		sely+=sel[i].offsetTop; 
		selw=sel[i].offsetWidth; 
		selh=sel[i].offsetHeight; 
		if(selx+selw>x && selx<x+w && sely+selh>y && sely<y+h){
			if(sel[i].style.visibility!="hidden"){ 
				sel[i].style.visibility="hidden";
				arrSelectElements[arrSelectElements.length] = sel[i];
			}
		}
	} 
} 

function showSelectElements(){
	for(i=0;i<arrSelectElements.length;i++){
		arrSelectElements[i].style.visibility="visible";
	}
	arrSelectElements = new Array();
}