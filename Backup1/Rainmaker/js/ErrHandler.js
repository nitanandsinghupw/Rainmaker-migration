// Added 1.16.11 for ignoring random SOAP errors, may need to add logging

// Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

function EndRequestHandler(sender, args)
{
  if (args.get_error() != undefined)
   {
//                   var errorMessage;
//                   //alert("SOAP error trap firing, error code " + args.get_response().get_statusCode());
//                   if (args.get_response().get_statusCode() == '12029')
//                   {
//                    args.set_errorHandled(true);
//                   }
//                   else
//                   {
//                       // not my error so let the default behavior happen       
//                   }
       $get('Error').style.visibility = "visible"; // Let the framework know that the error is handled, // so it doesn't throw the JavaScript alert. 
       args.set_errorHandled(true);

   }
}
