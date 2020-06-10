<%@ Control Language="C#" AutoEventWireup="true" Codebehind="AgentToolbar.ascx.cs"
    Inherits="Rainmaker.Web.common.controls.AgentToolbar" %>

    <table border="0" cellpadding="4" cellspacing="0" width="100%">
        <tr>
            <td>
                <asp:LinkButton ID="lbtnReady" OnClick="lbtnPause_Click" runat="server" ><img src="../../images/pause.jpg" alt="Pause" /></asp:LinkButton>
                    
            </td>
        </tr>
         <tr>
            <td>
                <asp:LinkButton ID="lbtnPause" OnClick="lbtnPause_Click" runat="server" ><img src="../../images/pause.jpg" alt="Pause" /></asp:LinkButton>
                    
            </td>
        </tr>
        <tr>
            <td>
               <asp:LinkButton ID="lbtnCallBack"  runat="server" ><img src="../../images/callback.jpg" alt="callback" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
               <asp:LinkButton ID="lbtnschedule"  runat="server" ><img src="../../images/schedule.jpg" alt="schedule" id="imgSchedule" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="lbtnRecord"  runat="server" ><img src="../../images/record.jpg" alt="record" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="lbtnconference"  runat="server" ><img src="../../images/conference.jpg" alt="conference" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
               <asp:LinkButton ID="lbtnTransfer"  runat="server" ><img src="../../images/transfer.jpg" alt="transfer" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="lbtnHangup"  runat="server" ><img src="../../images/hangup.jpg" alt="hangup" /></asp:LinkButton>
            </td>
        </tr>
        <tr>
            <td>
                <asp:LinkButton ID="lbtnDispose"  runat="server" ><img src="../../images/dispose.jpg" alt="dispose" /></asp:LinkButton></td>
        </tr>
    </table>

