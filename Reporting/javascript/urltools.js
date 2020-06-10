// Function borrowed from: http://rockmanx.wordpress.com/2008/10/03/get-url-parameters-using-javascript/
//
// Lets you get the url parameters from a get request
function gup( name )
{
  name = name.replace(/[\[]/,"\\\[").replace(/[\]]/,"\\\]");
  var regexS = "[\\?&]"+name+"=([^&#]*)";
  var regex = new RegExp( regexS );
  var results = regex.exec( window.location.href );
  if( results == null )
    return "";
  else
    return results[1];
}

