<%@ Page Language="C#" AutoEventWireup="true" Codebehind="AudioPlay.aspx.cs" Inherits="Rainmaker.Web.campaign.AudioPlay" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Rainmaker Dialer - Play Audio</title>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="1" border="0" width="300px" class="tdHeader">
            <tr>
                <td valign="top">
                    <table cellpadding="0" cellspacing="0" border="0" width="100%" class="tdWhite">
                        <tr>
                            <td>
                                <table>
                                    <tr>
                                        <td align="left">
                                            <table cellpadding="0" border="0" cellspacing="0" width="100%">
                                                <tr>
                                                    Playing file: &nbsp; <%=Audio %>
                                                    <asp:HiddenField ID="hdnFileToPlay" runat="server" />
                                                </tr>
                                                <tr>
                                                    <td width="100%">
                                                     <script type="text/javascript">
                                                         if (-1 != navigator.userAgent.indexOf("MSIE")) {
                                                             document.write('<OBJECT id="Player"');
                                                             document.write(' classid="clsid:6BF52A52-394A-11d3-B153-00C04F79FAA6"');
                                                             document.write(' width=300 height=200></OBJECT>');
                                                         }
                                                         else if (-1 != navigator.userAgent.indexOf("Firefox")) {
                                                             document.write(' <object width="100%" height="100%" type="application/x-ms-wmp" url="<%=Audio %>" data="<%=Audio %>"');
                                                             document.write(' classid="CLSID:6BF52A52-394A-11d3-B153-00C04F79FAA6">');
                                                             document.write(' <param name="url" value="<%=Audio %>">');
                                                             document.write(' <param name="filename" value="<%=Audio %>">');
                                                             document.write(' <param name="autostart" value="1">');
                                                             document.write(' <param name="uiMode" value="full"><param name="autosize" value="1">');
                                                             document.write(' <param name="playcount" value="1">');
                                                             document.write(' <embed type="application/x-ms-wmp" src="<%=Audio %>" width="100%" height="100%" autostart="true" showcontrols="true" pluginspage="http://www.microsoft.com/Windows/MediaPlayer/">');
                                                             document.write(' </embed>');
                                                             document.write(' </object>');

                                                         }         
                                                     
                                                     </script>
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
    </form>
</body>
</html>
