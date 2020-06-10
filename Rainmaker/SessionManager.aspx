<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SessionManager.aspx.cs" Inherits="Rainmaker.Web.SessionManager" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
    <script language="javascript" type="text/javascript">
    function closewindow()
    {
    alert('I should be closing');
    return;
        if(navigator.appName=="Microsoft Internet Explorer") 
        {
            // this.focus();self.opener = this;self.close(); 
            
            //var etc="channelmode=0,dependent=0,directories=0,fullscreen=0,location=0,menubar=0,resizable=0,scrollbars=1,status=1,toolbar=0"
            window.open('','_self','');
            //window.opener=null;
            window.close();
        }
        else 
        { 
            window.open('','_parent',''); window.close(); 
        }

    }
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
<asp:Literal id="msg" Text="My Literal" runat="server" />
<asp:TextBox ID="tb" Text="Empty" runat="server" />
    </div>
    </form>
</body>
</html>
