<table class="shiftReport">
<!--
	== Show the report header/information
	-->
<tr>
	<td style="border-bottom: solid black 4px; padding-bottom: 10px;">
	<table width="100%">
	<tr style="height: 40px; vertical-align: top">
		<td id="shiftReportHeader" colspan="3">Shift Report</td>
	</tr>
	<tr>
		<td style="vertical-align: top">
			<span class="shiftSecHeader" style="margin-left: 100px; font-size: large; text-decoration: none">
			For Campaign: &nbsp&nbsp&nbsp $campaign.Description$
			</span>
		</td>
		<td align="right" style="vertical-align: top">
			<table style="font-weight: bold">
			<tr><td>Start Date: $startDate$</td></tr>
			<tr><td>End Date: $endDate$</td></tr>
			<tr><td><p style="text-decoration: underline">Between Times</p></td></tr>
			<tr><td>Start Time: $startTime$</td></tr>
			<tr><td>End Time: $endTime$</td></tr>
			</table>
		</td>
	</tr>
	</table>
	</td>
</tr>
			
<!--
  == Summary Section
  -->
<tr>
	<td class="shiftSecHeader" style="padding-top: 10px; padding-bottom: 10px">Productivity</td>
</tr>
  
<tr>
	<td class="shiftSubSectHeader">Summary</td>
</tr>
	
<tr>
	<td style="padding-left: 40px">
		Total Man-Hours: $manHours$ <br/>
		Total Dialing-Hours: $dialingHours$ <br/>
		Total Pause Time: $pauseTime$
	</td>
</tr>

<tr>
	<td class="shiftSubSectHeader" style="padding-left: 40px; padding-top: 10px">Leads/Sales</td>
</tr>
<tr>
	<td>
	<table style="width: 100%">
	<tr>
		<td style="padding-left: 40px; width: 33%">Total: $leads$</td>
		<td style="width: 33%">Leads/Hour: $leadsPerHour$</td>
		<td style="width: 33%">Leads/Man-Hour: $leadsPerManHour$</td>
	</tr>
	</table>
	</td>
</tr>


<tr>
	<td class="shiftSubSectHeader" style="padding-left: 40px; padding-top: 10px">Presentations</td>
</tr>
<tr>
	<td>
	<table style="width: 100%">
	<tr>
		<td style="padding-left: 40px; width: 33%">Total: $totalPresentations$</td>
		<td style="width: 33%">Presentations/Hour: $presentationsPerHour$</td>
		<td style="width: 33%">Presentations/Man-Hour: $presentationsPerManHour$</td>
	</tr>
	</table>
	</td>
</tr>
		
		
<tr>
	<td class="shiftSubSectHeader" style="padding-left: 40px; padding-top: 10px">Calls</td>
</tr>
<tr>
	<td style="padding-bottom: 50px">
	<table style="width: 100%">
	<tr>
		<td style="padding-left: 40px; width: 40%">Total: $totalCalls$</td>
		<td style="width: 60%">Avg. Call Time (All): $averageCallTime$</td>
	</tr>
	<tr>
		<td style="padding-left: 40px; width: 40%">Calls/Hour: $callsPerHour$</td>
		<td style="width: 60%">Avg. Call Time (Presentations): $avgCallTimePresentation$</td>
	</tr>
	<tr>
		<td style="padding-left: 40px; width: 40%">Calls/Man-Hour: $callsPerManHour$</td>
		<td style="width: 60%">Avg. Call Time (Lead): $avgCallTimePerLead$</td>
	</tr>
	<tr>
		<td style="padding-left: 40px; width: 40%">Avg. Time Between-Calls: $aveWaitTime$</td>
		<td style="width: 60%">Avg. Wrap Time (Lead): $aveWrapTimeLeads$</td>
	</tr>
	</table>
	</td>
</tr>
			
	
<!--
  == Summary Section
  -->
<tr>
	<td class="shiftSubSectHeader" style="padding-top: 10px; padding-bottom: 10px">Hourly Agent Productivity</td>
</tr>
<tr>
	<td style="padding-bottom: 50px">
	<table id="shiftProductTable" cellpadding="0" cellspacing="0">
	<!-- Write the Table Header -->
	<tr>
		<td class="shiftProductTableHeader" style="width: 130px; padding: 5px; text-align: left">Name</td>
		<td class="shiftProductTableHeader" style="width: 70px; padding: 5px;">Leads</td>
		<td class="shiftProductTableHeader" style="width: 70px; padding: 5px;">Lead/Hr</td>
		<td class="shiftProductTableHeader" style="width: 70px; padding: 5px;">Total Calls</td>
		<td class="shiftProductTableHeader" style="width: 70px; padding: 5px;">Calls/Hr</td>
		<td class="shiftProductTableHeader" style="width: 70px; padding: 5px;">Presents</td>
		<td class="shiftProductTableHeader" style="width: 90px; padding: 5px;">Presents/Hr</td>
		<td class="shiftProductTableHeader" style="width: 70px; padding: 5px;">Dail Hrs</td>
		<td class="shiftProductTableHeader" style="width: 70px; padding: 5px;">Pause</td>
		<td class="shiftProductTableHeader" style="padding: 5px;">Talk %</td>
	</tr>

	<!-- OK, write out the agent information -->
	$summary.Agents:
	{
  <tr>
    <td style="padding-left: 5px; text-align: left">$it.AgentName$</td>
    <td style="text-align: center">$it.Leads$</td>
    <td style="text-align: center">$it.FormatedLeadsPerHour$</td>
    <td style="text-align: center">$it.TotalCalls$</td>
    <td style="text-align: center">$it.FormatedCallsPerHour$</td>
    <td style="text-align: center">$it.Presentations$</td>
    <td style="text-align: center">$it.FormatedPresentationsPerHour$</td>
    <td style="text-align: center">$it.FormatedDialingHours$</td>
    <td style="text-align: center">$it.FormatedPauseTime$</td>
    <td style="text-align: center">$it.FormatedTalkPercentage$</td>
  </tr>
	}$
	
	<!-- Finally, display the footer with the data -->
	<tr>
		<td class="shiftProductTableFooter">Totals:</td>
		<td class="shiftProductTableFooter">$leads$</td>
		<td class="shiftProductTableFooter">$leadsPerHour$</td>
		<td class="shiftProductTableFooter">$totalCalls$</td>
		<td class="shiftProductTableFooter">$callsPerHour$</td>
		<td class="shiftProductTableFooter">$totalPresentations$</td>
		<td class="shiftProductTableFooter">$presentationsPerHour$</td>
		<td class="shiftProductTableFooter">$dialingHours$</td>
		<td class="shiftProductTableFooter">$pauseTime$</td>
		<td class="shiftProductTableFooter">$talkPercentage$</td>
	</tr>		
	</table>
	</td>
</tr>

<!--
  == Write out the Dialing Results
  -->
<tr>
	<td class="shiftSecHeader" style="padding-top: 10px; padding-bottom: 10px">Dialing Results</td>
</tr>
  
<tr>
	<td class="shiftSubSectHeader" style="padding-left: 20px">Dialing History</td>
</tr>
<tr>
	<td style="padding-bottom: 50px; padding-left: 20px">
	<table style="width: 100%" cellspacing="0">
	<tr>
		<td style="padding-left: 20px; width: 10%; border-top: solid black 2px; border-bottom: solid black 2px">Dials</td>
		<td style="width: 10%; text-align: center; border-top: solid black 2px; border-bottom: solid black 2px">Connects</td>
		<td style="width: 10%; text-align: center; border-top: solid black 2px; border-bottom: solid black 2px">AnsMach</td>
		<td style="width: 10%; text-align: center; border-top: solid black 2px; border-bottom: solid black 2px">NoAns</td>
		<td style="width: 10%; text-align: center; border-top: solid black 2px; border-bottom: solid black 2px">Busy</td>
		<td style="width: 10%; text-align: center; border-top: solid black 2px; border-bottom: solid black 2px">OpInt</td>
		<td style="width: 10%; text-align: center; border-top: solid black 2px; border-bottom: solid black 2px">Drop</td>
		<td style="width: 10%; text-align: center; border-top: solid black 2px; border-bottom: solid black 2px">Connect%</td>
		<td style="width: 10%; text-align: center; border-top: solid black 2px; border-bottom: solid black 2px">Drop%</td>
	</tr>
	<tr>	
		<td style="padding-left: 20px; width: 10%;">$dials$</td>
		<td style="width: 10%; text-align: center;">$totalCalls$</td>
		<td style="width: 10%; text-align: center;">$answeringMachines$</td>
		<td style="width: 10%; text-align: center;">$noAnswers$</td>
		<td style="width: 10%; text-align: center;">$busys$</td>
		<td style="width: 10%; text-align: center;">$operatorIntercept$</td>
		<td style="width: 10%; text-align: center;">$drops$</td>
		<td style="width: 10%; text-align: center;">$connectPercentage$</td>
		<td style="width: 10%; text-align: center;">$dropPercentage$</td>
	</tr>
	</table>
	</td>
</tr>

<!-- Show the information for the summary agent -->
<tr>
	<td style="padding-bottom: 50px">
	<table style="width: 100%; padding-left: 20px">
	<tr>
		<td>
		<p style="font-weight: bold;">Agent Name: <span style="margin-left: 8px; font-weight: normal">$summaryAgent.AgentName$</span></p>
		</td>
	</tr>
	
	<tr>
		<td>
			<table width="100%">
			<tr>
				<td style="text-align: center; width: 16%">Total Calls</td>
				<td style="text-align: center; width: 16%">Leads(Sale)</td>
				<td style="text-align: center; width: 16%">Presentations</td>
				<td style="text-align: center; width: 16%">Leads(Sales) %</td>
				<td style="text-align: center; width: 16%">Presentation %</td>
				<td style="text-align: center; width: 16%">Lead/Present. %</td>
			</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td>
			<table width="100%">
			<tr>
				<td style="text-align: center; width: 16%">$summaryAgent.TotalCalls$</td>
				<td style="text-align: center; width: 16%">$summaryAgent.Leads$</td>
				<td style="text-align: center; width: 16%">$summaryAgent.Presentations$</td>
				<td style="text-align: center; width: 16%">$summaryAgent.FormatedLeadsPerCall$</td>
				<td style="text-align: center; width: 16%">$summaryAgent.FormatedPresentationsPerCall$</td>
				<td style="text-align: center; width: 16%">$summaryAgent.FormatedLeadsPerPresentation$</td>
			</tr>
			</table>
		</td>
	</tr>	
	
	<tr>
		<td>
		<table width="100%" cellspacing="0">
			<tr>
				<td style="width: 30%; border-top: solid black 2px; border-bottom: solid black 2px">Result Code</td>
				<td style="text-align: center; width: 15%; border-top: solid black 2px; border-bottom: solid black 2px">Count</td>
				<td style="text-align: center; width: 15%; border-top: solid black 2px; border-bottom: solid black 2px">% of Total</td>
				<td style="text-align: center; width: 40%; border-top: solid black 2px; border-bottom: solid black 2px">&nbsp</td>
			</tr>
		</table>
		</td>
	</tr>
	<tr>
		<td style="padding-bottom: 50px">
		<table style="width: 100%">
		$summaryAgent.ResultCodes:
			{
			<tr>
				<td style="width: 30%;">$it.Description$</td>
				<td style="text-align: center; width: 15%;">$it.Count$</td>
				<td style="text-align: center; width: 15%;">$it.FormatedPercentageOfTotal$</td>
				<td style="text-align: center; width: 40%;">&nbsp</td>
			</tr>
			}$
		</table>
		</td>
	</tr>
	</table>
	</td>
</tr>

<!-- Start showing the summary information agent-by-agent -->
<tr>
	<td style="padding-bottom: 50px">
	<table style="width: 100%; padding-left: 20px">
	$summary.Agents:
	{
	<tr>
		<td>
		<p style="font-weight: bold;">Agent Name: <span style="margin-left: 8px; font-weight: normal">$it.AgentName$</span></p>
		</td>
	</tr>
	
	<tr>
		<td>
			<table width="100%">
			<tr>
				<td style="text-align: center; width: 16%">Total Calls</td>
				<td style="text-align: center; width: 16%">Leads(Sale)</td>
				<td style="text-align: center; width: 16%">Presentations</td>
				<td style="text-align: center; width: 16%">Leads(Sales) %</td>
				<td style="text-align: center; width: 16%">Presentation %</td>
				<td style="text-align: center; width: 16%">Lead/Present. %</td>
			</tr>
			</table>
		</td>
	</tr>
	<tr>
		<td>
			<table width="100%">
			<tr>
				<td style="text-align: center; width: 16%">$it.TotalCalls$</td>
				<td style="text-align: center; width: 16%">$it.Leads$</td>
				<td style="text-align: center; width: 16%">$it.Presentations$</td>
				<td style="text-align: center; width: 16%">$it.FormatedLeadsPerCall$</td>
				<td style="text-align: center; width: 16%">$it.FormatedPresentationsPerCall$</td>
				<td style="text-align: center; width: 16%">$it.FormatedLeadsPerPresentation$</td>
			</tr>
			</table>
		</td>
	</tr>	
	
	<tr>
		<td>
		<table width="100%" cellspacing="0">
			<tr>
				<td style="width: 30%; border-top: solid black 2px; border-bottom: solid black 2px">Result Code</td>
				<td style="text-align: center; width: 15%; border-top: solid black 2px; border-bottom: solid black 2px">Count</td>
				<td style="text-align: center; width: 15%; border-top: solid black 2px; border-bottom: solid black 2px">% of Total</td>
				<td style="text-align: center; width: 40%; border-top: solid black 2px; border-bottom: solid black 2px">&nbsp</td>
			</tr>
		</table>
		</td>
	</tr>
	<tr>
		<td style="padding-bottom: 50px">
		<table style="width: 100%">
		$it.ResultCodes:
			{
			<tr>
				<td style="width: 30%;">$it.Description$</td>
				<td style="text-align: center; width: 15%;">$it.Count$</td>
				<td style="text-align: center; width: 15%;">$it.FormatedPercentageOfTotal$</td>
				<td style="text-align: center; width: 40%;">&nbsp</td>
			</tr>
			}$
		</table>
		</td>
	</tr>
	}$
	</table>
	</td>
</tr>

<!--
  == Write out the Dialing Results
  -->
<tr>
	<td class="shiftSecHeader" style="padding-top: 10px; padding-bottom: 10px">Time Statistics</td>
</tr>
  
<tr>
	<td class="shiftSubSectHeader" style="padding-left: 20px">Summary</td>
</tr>

<tr>
	<td style="padding-left: 20px">
	<table id="shiftTimeStatTable" cellpadding="0" cellspacing="0" style="width: 800px">
	<tr>
		<td class="shiftTimeStatTableHeader" colspan="2">Call Times</td>
		<td class="shiftTimeStatTableHeader" colspan="2">Totals</td>
	</tr>
	<tr>
		<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Avg. Call Time:</td>
		<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$summaryAgent.FormatedAveCallTime$</td>
		<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Total Talk Time:</td>
		<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$summaryAgent.FormatedTotalTalkTime$</td>
	</tr>
	<tr>
		<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Avg. Call Time (Presentations):</td>
		<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$summaryAgent.FormatedAveCallTimePresentation$</td>
		<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Total Wait Time:</td>
		<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$summaryAgent.FormatedWaitTime$</td>
	</tr>
	<tr>
		<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Avg. Call Time (Leads):</td>
		<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$summaryAgent.FormatedAveCallTimeLead$</td>
		<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Total Pause Time:</td>
		<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$summaryAgent.FormatedPauseTime$</td>
	</tr>
	<tr>
		<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Avg. Wrap-Up Time (All):</td>
		<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$summaryAgent.FormatedAveWrapTimeAll$</td>
		<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Total Wrap Time:</td>
		<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$summaryAgent.FormatedWrapTime$</td>
	</tr>
	<tr>
		<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Ave. Wrap-Up Time (Leads):</td>
		<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$summaryAgent.FormatedAveWrapTimeLeads$</td>
		<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Total Calls:</td>
		<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$summaryAgent.TotalCalls$</td>
	</tr>
	
	<tr>
		<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Avg. Time Between Calls:</td>
		<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$summaryAgent.FormatedAveWaitTime$</td>
		<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Log-In Time:</td>
		<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$summaryAgent.FormatedManHours$</td>
	</tr>
	</table>
	</td>
</tr>
			
<tr>
	<td class="shiftSubSectHeader" style="padding-left: 20px">Agents</td>
</tr>

<tr>
	<td style="padding-bottom: 50px">
	<table style="width: 100%; padding-left: 20px">
	$summary.Agents:
	{
	<tr>
		<td class="shiftSubSectHeader" style="padding-left: 40px">$it.AgentName$</td>
	</tr>
	<tr>
		<td style="padding-left: 80px">
		<table id="shiftTimeStatTable" cellpadding="0" cellspacing="0" style="width: 800px">
		<tr>
			<td class="shiftTimeStatTableHeader" colspan="2">Call Times</td>
			<td class="shiftTimeStatTableHeader" colspan="2">Totals</td>
		</tr>
		<tr>
			<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Avg. Call Time:</td>
			<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$it.FormatedAveCallTime$</td>
			<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Total Talk Time:</td>
			<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$it.FormatedTotalTalkTime$</td>
		</tr>
		<tr>
			<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Avg. Call Time (Presentations):</td>
			<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$it.FormatedAveCallTimePresentation$</td>
			<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Total Wait Time:</td>
			<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$it.FormatedWaitTime$</td>
		</tr>
		<tr>
			<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Avg. Call Time (Leads):</td>
			<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$it.FormatedAveCallTimeLead$</td>
			<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Total Pause Time:</td>
			<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$it.FormatedPauseTime$</td>
		</tr>
		<tr>
			<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Avg. Wrap-Up Time (All):</td>
			<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$it.FormatedAveWrapTimeAll$</td>
			<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Total Wrap Time:</td>
			<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$it.FormatedWrapTime$</td>
		</tr>
		<tr>
			<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Ave. Wrap-Up Time (Leads):</td>
			<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$it.FormatedAveWrapTimeLeads$</td>
			<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Total Calls:</td>
			<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$it.TotalCalls$</td>
		</tr>
		
		<tr>
			<td class="shiftTimeStatTableRow" style="width: 240px; text-align: right; padding-right: 30px">Avg. Time Between Calls:</td>
			<td class="shiftTimeStatTableRow" style="width: 90px; text-align: left">$it.FormatedAveWaitTime$</td>
			<td class="shiftTimeStatTableRow" style="width: 260px; text-align: right; padding-right: 30px">Log-In Time:</td>
			<td class="shiftTimeStatTableRow" style="width: 110px; text-align: left">$it.FormatedManHours$</td>
		</tr>
		</table>
		</td>
	</tr>
	}$
	</table>
	</td>
</tr>


</table>