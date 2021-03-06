USE [master]
GO
/****** Object:  Database [RainmakerMaster]    Script Date: 07/25/2011 15:45:29 ******/
CREATE DATABASE [RainmakerMaster] ON  PRIMARY 
( NAME = N'RainmakerMaster', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\RainmakerMaster.mdf' , SIZE = 2240KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'RainmakerMaster_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL.1\MSSQL\DATA\RainmakerMaster_log.LDF' , SIZE = 768KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [RainmakerMaster] SET COMPATIBILITY_LEVEL = 90
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [RainmakerMaster].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [RainmakerMaster] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [RainmakerMaster] SET ANSI_NULLS OFF
GO
ALTER DATABASE [RainmakerMaster] SET ANSI_PADDING OFF
GO
ALTER DATABASE [RainmakerMaster] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [RainmakerMaster] SET ARITHABORT OFF
GO
ALTER DATABASE [RainmakerMaster] SET AUTO_CLOSE ON
GO
ALTER DATABASE [RainmakerMaster] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [RainmakerMaster] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [RainmakerMaster] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [RainmakerMaster] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [RainmakerMaster] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [RainmakerMaster] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [RainmakerMaster] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [RainmakerMaster] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [RainmakerMaster] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [RainmakerMaster] SET  ENABLE_BROKER
GO
ALTER DATABASE [RainmakerMaster] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [RainmakerMaster] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [RainmakerMaster] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [RainmakerMaster] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [RainmakerMaster] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [RainmakerMaster] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [RainmakerMaster] SET  READ_WRITE
GO
ALTER DATABASE [RainmakerMaster] SET RECOVERY SIMPLE
GO
ALTER DATABASE [RainmakerMaster] SET  MULTI_USER
GO
ALTER DATABASE [RainmakerMaster] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [RainmakerMaster] SET DB_CHAINING OFF
GO
USE [RainmakerMaster]
GO
/****** Object:  Table [dbo].[DataManagerViews]    Script Date: 07/25/2011 15:45:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DataManagerViews](
	[ViewID] [bigint] IDENTITY(1,1) NOT NULL,
	[AgentID] [bigint] NOT NULL,
	[ViewName] [varchar](50) NOT NULL,
	[FieldList] [varchar](1000) NOT NULL,
	[RecordsPerPage] [int] NOT NULL,
 CONSTRAINT [PK_DataManagerViews] PRIMARY KEY CLUSTERED 
(
	[ViewID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AreaCode]    Script Date: 07/25/2011 15:45:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AreaCode](
	[AreaCodeID] [bigint] IDENTITY(1,1) NOT NULL,
	[AreaCode] [varchar](50) NOT NULL,
	[Prefix] [varchar](10) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_AreaCode] PRIMARY KEY CLUSTERED 
(
	[AreaCodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DialerActivity]    Script Date: 07/25/2011 15:45:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DialerActivity](
	[DialerActivityID] [bigint] IDENTITY(1,1) NOT NULL,
	[ConnectTime] [datetime] NOT NULL,
	[DisconnectTime] [datetime] NULL,
	[DialerStartTime] [datetime] NULL,
	[DialerStopTime] [datetime] NULL,
 CONSTRAINT [PK_DialerActivity] PRIMARY KEY CLUSTERED 
(
	[DialerActivityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FieldTypes]    Script Date: 07/25/2011 15:45:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FieldTypes](
	[FieldTypeID] [bigint] IDENTITY(1,1) NOT NULL,
	[FieldType] [varchar](50) NOT NULL,
	[DBFieldType] [varchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_FieldTypes] PRIMARY KEY CLUSTERED 
(
	[FieldTypeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[GlobalDialingParameters]    Script Date: 07/25/2011 15:45:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[GlobalDialingParameters](
	[GlobalDialingID] [bigint] IDENTITY(1,1) NOT NULL,
	[Prefix] [varchar](50) NULL,
	[Suffix] [varchar](50) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_GlobalDialingParamters] PRIMARY KEY CLUSTERED 
(
	[GlobalDialingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Status]    Script Date: 07/25/2011 15:45:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Status](
	[StatusID] [bigint] IDENTITY(1,1) NOT NULL,
	[StatusName] [varchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Status] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TimeZone]    Script Date: 07/25/2011 15:45:30 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TimeZone](
	[TimeZoneID] [bigint] IDENTITY(1,1) NOT NULL,
	[TimeZone] [varchar](255) NOT NULL,
	[Offset] [numeric](18, 2) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_TimeZone] PRIMARY KEY CLUSTERED 
(
	[TimeZoneID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[ShutdownAllCampaigns]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Gregg Williamson	
-- Create date: 05.08.11
-- Description:	Sets all running campiagns to flush idle for system wide shutdown
-- =============================================
CREATE PROCEDURE [dbo].[ShutdownAllCampaigns] 
AS
BEGIN
	DECLARE @RunningCampaign bigint 
	DECLARE CsrRunningCampaign CURSOR FOR 
	SELECT CampaignID FROM Campaign WHERE StatusID = 2

	OPEN CsrRunningCampaign
		
	FETCH NEXT FROM CsrRunningCampaign INTO @RunningCampaign
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--PRINT @alterSQL
		UPDATE Campaign SET StatusID = 5 WHERE CampaignID = @RunningCampaign
		
		FETCH NEXT FROM CsrRunningCampaign INTO @RunningCampaign
	END
	CLOSE CsrRunningCampaign 
	DEALLOCATE CsrRunningCampaign 
END
GO
/****** Object:  Table [dbo].[AdminRequests]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AdminRequests](
	[RequestID] [bigint] IDENTITY(1,1) NOT NULL,
	[RequestType] [int] NOT NULL,
	[DateTimeSubmitted] [datetime] NOT NULL,
	[RequestData] [varchar](255) NULL,
	[RequestStatus] [int] NOT NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Campaign]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Campaign](
	[CampaignID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[ShortDescription] [varchar](50) NOT NULL,
	[FundRaiserDataTracking] [bit] NOT NULL,
	[RecordLevelCallHistory] [bit] NOT NULL,
	[OnsiteTransfer] [bit] NOT NULL,
	[EnableAgentTraining] [bit] NULL,
	[AllowDuplicatePhones] [bit] NOT NULL,
	[CampaignDBConnString] [varchar](255) NOT NULL,
	[FlushCallQueueOnIdle] [bit] NOT NULL,
	[StatusID] [bigint] NULL,
	[DialAllNumbers] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[OutboundCallerID] [varchar](20) NOT NULL,
	[StartTime] [datetime] NULL,
	[StopTime] [datetime] NULL,
	[DuplicateRule] [char](1) NOT NULL,
	[Allow7DigitNums] [bit] NOT NULL,
	[Allow10DigitNums] [bit] NOT NULL,
 CONSTRAINT [PK_Campaign] PRIMARY KEY CLUSTERED 
(
	[CampaignID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Campaign] ON [dbo].[Campaign] 
(
	[ShortDescription] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[CLONE_CAMPAIGN]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CLONE_CAMPAIGN]
	@ParentCampaignID bigint,
	@DBName VARCHAR(20),
	@IncludeResultCodes bit,
	@IncludeQueries bit,
	@IncludeOptions bit,
	@IncludeFields bit,
	@IncludeData bit,
	@IncludeScripts bit,
	@FullCopy bit
AS
 
DECLARE @ParentDBName VARCHAR(20)
DECLARE @SQLQuery NVARCHAR(4000)

SELECT @ParentDBName = (CASE WHEN 
		CHARINDEX('Campaign_', CampaignDBConnString) > 1 
	THEN
		'[Campaign_' + Convert(VARCHAR(20), CampaignID) + ']'
	ELSE
		'[' + ShortDescription + ']' END) 
FROM Campaign WHERE CampaignID =  @ParentCampaignID

PRINT @ParentDBName

IF (@IncludeResultCodes = 1 OR @FullCopy = 1)
BEGIN
	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[ResultCode] ON ' + CHAR(10) +
										
					'INSERT INTO ['+@DBName+'].[dbo].[ResultCode] (ResultCodeID,
					Description, Presentation, Redialable, RecycleInDays,
					Lead, Printable, NeverCall, VerifyOnly, DialThroughAll, CountAsLiveContact,
					ShowDeletedResultCodes, DateDeleted, DateCreated, DateModified)
					SELECT * FROM '+@ParentDBName+'.[dbo].[ResultCode] '
	--PRINT @SQLQuery
	EXECUTE sp_executesql @SQLQuery
END

IF (@IncludeQueries  = 1 OR @FullCopy = 1)
BEGIN

	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[Query] ON ' + CHAR(10) + 
										
					'INSERT INTO ['+@DBName+'].[dbo].[Query] (QueryID,
					QueryName, QueryCondition, DateCreated, DateModified)
					SELECT * FROM '+@ParentDBName+'.[dbo].[Query]'
	--PRINT @SQLQuery
	EXECUTE sp_executesql @SQLQuery

	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[QueryDetail] ON ' + CHAR(10) + 
										
					'INSERT INTO ['+@DBName+'].[dbo].[QueryDetail] (QueryDetailID,
					QueryID, SearchCriteria, SearchOperator, SearchString,
					LogicalOperator, LogicalOrder, SubQueryID, DateCreated, DateModified, SubsetID, SubsetName, SubsetLevel,
					ParentSubsetID, TreeNodeID, ParentTreeNodeID, SubsetLogicalOrder, ElementText)
					SELECT * FROM '+@ParentDBName+'.[dbo].[QueryDetail]' 
	--PRINT @SQLQuery
	EXECUTE sp_executesql @SQLQuery
	IF @FullCopy = 0
	BEGIN
		SET @SQLQuery =  'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[CampaignQueryStatus] ON ' + CHAR(10) + 
											
						'INSERT INTO ['+@DBName+'].[dbo].[CampaignQueryStatus] (CampaignQueryID, QueryID,IsActive,Total,Available,
						Dials,Talks,AnsweringMachine,NoAnswer,Busy,OpInt,Drops,Failed,DateCreated,DateModified)
						SELECT CampaignQueryID, QueryID, IsActive,0,0,
						0,0,0,0,0,0,0,0,DateCreated,DateModified FROM '+@ParentDBName+'.[dbo].[CampaignQueryStatus]' 
		--PRINT @SQLQuery
		EXECUTE sp_executesql @SQLQuery
	END
END

IF (@IncludeScripts = 1 OR @FullCopy = 1)
BEGIN

	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[Script] ON ' + CHAR(10) +
										
					'INSERT INTO ['+@DBName+'].[dbo].[Script] (ScriptID,
					ScriptName,ScriptHeader,ScriptSubHeader,ScriptBody,ParentScriptID,ScriptGuid,DateCreated,DateModified)
					SELECT * FROM '+@ParentDBName+'.[dbo].[Script] '
	--PRINT @SQLQuery
	EXECUTE sp_executesql @SQLQuery

END

IF (@IncludeOptions = 1 OR @FullCopy = 1)
BEGIN
	
	
	SET @SQLQuery = 'DELETE FROM ['+@DBName+'].[dbo].[DialingParameter]  ' + CHAR(10) +
					'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[DialingParameter] ON ' + CHAR(10) +
										
					'INSERT INTO ['+@DBName+'].[dbo].[DialingParameter] (DailingParameterID,PhoneLineCount,
					DropRatePercent,RingSeconds,MinimumDelayBetweenCalls,DialingMode,
					AnsweringMachineDetection,MessageRecordingTool,ColdCallScriptID,
					VerificationScriptID,InboundScriptID,AMCallTimes,PMCallTimes,
					WeekendCallTimes,AMDialingStart,AMDialingStop,PMDialingStart,PMDialingStop,
					AnsMachDetect,DateCreated,DateModified,ErrorRedialLapse,BusyRedialLapse,NoAnswerRedialLapse,ChannelsPerAgent,DefaultCallLapse,
					HumanMessageEnable,HumanMessage,SilentCallMessageEnable,SilentCallMessage,
					SevenDigitPrefix, TenDigitPrefix, SevenDigitSuffix, TenDigitSuffix)
					SELECT * FROM '+@ParentDBName+'.[dbo].[DialingParameter] '

	--PRINT @SQLQuery
	EXECUTE sp_executesql @SQLQuery

	IF @IncludeScripts = 0
	BEGIN
		SET @SQLQuery = 'UPDATE ['+@DBName+'].[dbo].[DialingParameter] SET ColdCallScriptID = 1,
					VerificationScriptID = 1,InboundScriptID = 1 '
		EXECUTE sp_executesql @SQLQuery
	END

	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[DigitalizedRecording] ON ' + CHAR(10) +
										
					'INSERT INTO ['+@DBName+'].[dbo].[DigitalizedRecording] (DigitalizedRecordingID,
					EnableRecording, EnableWithABeep,StartWithABeep,RecordToWave,
					HighQualityRecording,RecordingFilePath,FileNaming,DateCreated,DateModified)
					SELECT * FROM '+@ParentDBName+'.[dbo].[DigitalizedRecording] '
	--PRINT @SQLQuery
	EXECUTE sp_executesql @SQLQuery

	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[OtherParameter] ON ' + CHAR(10) +
										
					'INSERT INTO ['+@DBName+'].[dbo].[OtherParameter] (OtherParameterID,
					CallTransfer,StaticOffsiteNumber,TransferMessage,AutoPlayMessage,
					HoldMessage,AllowManualDial,StartingLine,EndingLine,AllowCallBacks,
					AlertSupervisorOnCallbacks,QueryStatisticsInPercent,DateCreated,DateModified )
					SELECT * FROM '+@ParentDBName+'.[dbo].[OtherParameter] '
	--PRINT @SQLQuery
	EXECUTE sp_executesql @SQLQuery

END

IF (@IncludeFields = 1 OR @FullCopy = 1)
BEGIN
	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[CampaignFields] ON ' + CHAR(10) +
										
					'INSERT INTO ['+@DBName+'].[dbo].[CampaignFields] (FieldID,
					FieldName,FieldTypeID,Value,DateCreated,DateModified,IsDefault,ReadOnly)
					SELECT * FROM '+@ParentDBName+'.[dbo].[CampaignFields] where IsDefault = 0 '
	
	--PRINT @SQLQuery
	EXECUTE sp_executesql @SQLQuery
	
	CREATE TABLE #ALTERCAMPAIGN(ALTERSQL NVARCHAR(2000))
	DECLARE @alterSQL NVARCHAR(2000)
	SET @SQLQuery = 'INSERT INTO #ALTERCAMPAIGN (ALTERSQL)
	SELECT ''ALTER TABLE ['+@DBName+'].[dbo].[Campaign] ADD '' + FieldName + '' '' +
	CASE WHEN Value IS NULL THEN ft.DBFieldType ELSE ft.DBFieldType + ''('' + CONVERT(VARCHAR(10), Value) + '') NULL'' END as atlersql
	FROM ['+@DBName+'].[dbo].[CampaignFields]  
	INNER JOIN [RainmakerMaster].[dbo].[FieldTypes] ft ON ft.FieldTypeID = ['+@DBName+'].[dbo].[CampaignFields].FieldTypeID
	where IsDefault = 0 '
	--PRINT @SQLQuery
	EXECUTE sp_executesql @SQLQuery

	DECLARE CAMPAINGNCUSTOMFLDS CURSOR FOR 
	select ALTERSQL from #ALTERCAMPAIGN 

	OPEN CAMPAINGNCUSTOMFLDS
		
	FETCH NEXT FROM CAMPAINGNCUSTOMFLDS INTO @alterSQL
	WHILE @@FETCH_STATUS = 0
	BEGIN
		--PRINT @alterSQL
		EXECUTE sp_executesql @alterSQL
		
		FETCH NEXT FROM CAMPAINGNCUSTOMFLDS INTO @alterSQL
	END
	CLOSE CAMPAINGNCUSTOMFLDS
	DEALLOCATE CAMPAINGNCUSTOMFLDS
END

IF (@IncludeData = 1 AND @FullCopy = 0)
BEGIN
	-- Step 1: Load all custom field names from parent DB into temp table 
	CREATE TABLE #INSERTFIELDS(INSFIELDNAME NVARCHAR(2000))
	DECLARE @InsFieldName NVARCHAR(2000)
	SET @SQLQuery = 'INSERT INTO #INSERTFIELDS (INSFIELDNAME)
	SELECT FieldName FROM ['+@DBName+'].[dbo].[CampaignFields]'
	
	EXECUTE sp_executesql @SQLQuery
	
	DECLARE @InsertSQL NVARCHAR(2000)
	
	-- Step 2: Build insert query starting with standard fields
	SET @SQLQuery = 'INSERT INTO ['+@DBName+'].[dbo].[Campaign] (Campaign,
					PhoneNum,DBCompany,NeverCallFlag,'
					
	SET @InsertSQL = 'SELECT Campaign,PhoneNum,DBCompany,NeverCallFlag,' 
	
	DECLARE CAMPAINGNCUSTOMFLDS CURSOR FOR 
	select INSFIELDNAME from #INSERTFIELDS

	OPEN CAMPAINGNCUSTOMFLDS
		
	FETCH NEXT FROM CAMPAINGNCUSTOMFLDS INTO @InsFieldName
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Added trap  by GW 11.04.10 for bug found in cloning
		IF @InsFieldName <> 'PHONENUM'
		BEGIN 
			SET @SQLQuery = @SQLQuery + @InsFieldName + ','
			SET @InsertSQL = @InsertSQL + @InsFieldName + ','
		END
		
		FETCH NEXT FROM CAMPAINGNCUSTOMFLDS INTO @InsFieldName
	END
	CLOSE CAMPAINGNCUSTOMFLDS
	DEALLOCATE CAMPAINGNCUSTOMFLDS
	
	SET @SQLQuery = LEFT(@SQLQuery, LEN(@SQLQuery) - 1)
	SET @InsertSQL = LEFT(@InsertSQL, LEN(@InsertSQL) - 1)
	
	SET @InsertSQL = @InsertSQL + ' FROM '+@ParentDBName+'.[dbo].[Campaign]'
	
	SET @SQLQuery = @SQLQuery + ') ' + @InsertSQL
	
	EXECUTE sp_executesql @SQLQuery
	
END

IF @FullCopy = 1
BEGIN
	-- Added for full clone copy feature.
	CREATE TABLE #INSERTFIELDSCOPY(INSFIELDNAMECOPY NVARCHAR(2000))
	DECLARE @InsFieldNameCopy NVARCHAR(2000)
	SET @SQLQuery = 'INSERT INTO #INSERTFIELDSCOPY (INSFIELDNAMECOPY)
	SELECT FieldName FROM ['+@DBName+'].[dbo].[CampaignFields]'
	
	EXECUTE sp_executesql @SQLQuery
	
	DECLARE @InsertSQLCopy NVARCHAR(2000)
		
	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[Campaign] ON ' + CHAR(10) +
						'INSERT INTO ['+@DBName+'].[dbo].[Campaign] (
						UniqueKey, Campaign, PhoneNum, OffsiteTransferNumber, DBCompany, NeverCallFlag, AgentName, AgentID, VerificationAgentID,
						CallResultCode, DateTimeofCall, CallDuration, CallSenttoDialTime, CalltoAgentTime, CallHangupTime, CallCompletionTime,
						CallWrapUpStartTime, CallWrapUpStopTime, ResultCodeSetTime, TotalNumAttempts, NumAttemptsAM, NumAttemptsWkEnd, NumAttemptsPM,
						LeadProcessed, FullQueryPassCount, scheduledate, schedulenotes, isManualDial, DateTimeofImport, '
					
	SET @InsertSQLCopy = 'SELECT UniqueKey, Campaign, PhoneNum, OffsiteTransferNumber, DBCompany, NeverCallFlag, AgentName, AgentID, VerificationAgentID,
						CallResultCode, DateTimeofCall, CallDuration, CallSenttoDialTime, CalltoAgentTime, CallHangupTime, CallCompletionTime,
						CallWrapUpStartTime, CallWrapUpStopTime, ResultCodeSetTime, TotalNumAttempts, NumAttemptsAM, NumAttemptsWkEnd, NumAttemptsPM,
						LeadProcessed, FullQueryPassCount, scheduledate, schedulenotes, isManualDial, DateTimeofImport, '
	
	DECLARE CAMPAINGNCUSTOMFLDS CURSOR FOR 
	select INSFIELDNAMECOPY from #INSERTFIELDSCOPY

	OPEN CAMPAINGNCUSTOMFLDS
		
	FETCH NEXT FROM CAMPAINGNCUSTOMFLDS INTO @InsFieldNameCopy
	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Added trap  by GW 11.04.10 for bug found in cloning
		IF @InsFieldNameCopy <> 'PHONENUM'
		BEGIN 
			SET @SQLQuery = @SQLQuery + @InsFieldNameCopy + ','
			SET @InsertSQLCopy = @InsertSQLCopy + @InsFieldNameCopy + ','
		END
		
		FETCH NEXT FROM CAMPAINGNCUSTOMFLDS INTO @InsFieldNameCopy
	END
	CLOSE CAMPAINGNCUSTOMFLDS
	DEALLOCATE CAMPAINGNCUSTOMFLDS
	
	SET @SQLQuery = LEFT(@SQLQuery, LEN(@SQLQuery) - 1)
	SET @InsertSQLCopy = LEFT(@InsertSQLCopy, LEN(@InsertSQLCopy) - 1)
	
	SET @InsertSQLCopy = @InsertSQLCopy + ' FROM '+@ParentDBName+'.[dbo].[Campaign]'
	
	SET @SQLQuery = @SQLQuery + ') ' + @InsertSQLCopy
	
	EXECUTE sp_executesql @SQLQuery
										
					--'INSERT INTO ['+@DBName+'].[dbo].[ResultCode] (ResultCodeID,
					--Description, Presentation, Redialable, RecycleInDays,
					--Lead, Printable, NeverCall, VerifyOnly, DialThroughAll, CountAsLiveContact,
					--ShowDeletedResultCodes, DateDeleted, DateCreated, DateModified)
					--SELECT * FROM '+@ParentDBName+'.[dbo].[ResultCode] '

	
	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[CampaignQueryStatus] ON ' + CHAR(10) +
						'INSERT INTO ['+@DBName+'].[dbo].[CampaignQueryStatus] (CampaignQueryID, QueryID, IsActive, Total, Available,
							Dials, Talks, AnsweringMachine, NoAnswer, Busy, OpInt, Drops, Failed, DateCreated, DateModified, IsCurrent,
							ShowMessage, Priority) SELECT CampaignQueryID, QueryID, IsActive, Total, Available,
							Dials, Talks, AnsweringMachine, NoAnswer, Busy, OpInt, Drops, Failed, DateCreated, DateModified, IsCurrent,
							ShowMessage, Priority FROM '+@ParentDBName+'.[dbo].[CampaignQueryStatus]' 
					
	EXECUTE sp_executesql @SQLQuery
	
	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[CallList] ON ' + CHAR(10) +
						'INSERT INTO ['+@DBName+'].[dbo].[CallList] (CallListID, AgentID, AgentName, ResultCodeID, VerificationAgentID,
							PhoneNumber, OffsiteTransferNumber, CallDate, CallTime, CallDuration, CallCompletionTime, CallWrapTime,
							IsBlocked, DateCreated, DateModified, UniqueKey, QueryID, IsManualDial)
							SELECT CallListID, AgentID, AgentName, ResultCodeID, VerificationAgentID,
							PhoneNumber, OffsiteTransferNumber, CallDate, CallTime, CallDuration, CallCompletionTime, CallWrapTime,
							IsBlocked, DateCreated, DateModified, UniqueKey, QueryID, IsManualDial FROM '+@ParentDBName+'.[dbo].[CallList]'
	EXECUTE sp_executesql @SQLQuery
	
	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[SilentCallList] ON ' + CHAR(10) +
						'INSERT INTO ['+@DBName+'].[dbo].[SilentCallList] (SilentCallID, UniqueKey, DateTimeofCall) 
							SELECT SilentCallID, UniqueKey, DateTimeofCall FROM '+@ParentDBName+'.[dbo].[SilentCallList]'
	EXECUTE sp_executesql @SQLQuery
	
	SET @SQLQuery = 'SET IDENTITY_INSERT ['+@DBName+'].[dbo].[AgentStat] ON ' + CHAR(10) +
						'INSERT INTO ['+@DBName+'].[dbo].[AgentStat] (StatID, AgentID, AgentName, StatusID, LeadsSales, Presentations,
							Calls, LeadSalesRatio, TalkTime, WaitingTime, PauseTime, WrapTime, LoginDate, LoginTime, LogOffDate,
							LastResultCodeID, DateCreated, DateModified, TimeModified) 
						SELECT StatID, AgentID, AgentName, StatusID, LeadsSales, Presentations,
							Calls, LeadSalesRatio, TalkTime, WaitingTime, PauseTime, WrapTime, LoginDate, LoginTime, LogOffDate,
							LastResultCodeID, DateCreated, DateModified, TimeModified FROM '+@ParentDBName+'.[dbo].[AgentStat]'
	EXECUTE sp_executesql @SQLQuery
	
END
GO
/****** Object:  StoredProcedure [dbo].[DEL_Campaign]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DEL_Campaign]
	@CampaignID bigint
AS
UPDATE dbo.Campaign
SET
	IsDeleted = 1,
	ShortDescription = ShortDescription + ' - ' +  convert(varchar(24),getdate(),121)
WHERE
	CampaignID = @CampaignID
GO
/****** Object:  StoredProcedure [dbo].[FetchUDColums]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchUDColums]
AS
BEGIN

	SELECT DISTINCT Syscolumns.Name FROM syscolumns  
	INNER JOIN
		sysobjects ON sysobjects.id = syscolumns.id
	INNER JOIN
		sysproperties ON sysproperties.id = sysobjects.id
	WHERE
		sysobjects.xtype ='u'
	AND
		sysobjects.name = 'Agent'
	AND
		LEN(CAST(sysproperties.value AS VARCHAR)) >0
END
GO
/****** Object:  StoredProcedure [dbo].[FetchUDColumsList]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FetchUDColumsList] AS
BEGIN
SELECT   Objname AS [Column]
FROM   ::fn_listextendedproperty (NULL, 'user', 'dbo', 'table', 'agent', 'column', default)
where value <>''
END
GO
/****** Object:  StoredProcedure [dbo].[GenerateInsUpdateScript]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*  
----------------------------------------------------------------------------------------------------------------------------------------  
 Project : Inventive Labs
 Client  : Rainmaker  
 Author  : Prasad Bhogadi
 Name of the Procedure : GenerateInsUpdateScript
----------------------------------------------------------------------------------------------------------------------------------------  
 Purpose :This Procedure is used generate Insert Update scripts for a tablet  
----------------------------------------------------------------------------------------------------------------------------------------  
 Stored Procedures Referenced  : --  
 Tables Referenced   : 
----------------------------------------------------------------------------------------------------------------------------------------  
 Input Parameters :
  
 Expected Output  : Generate script for Insert Update Stored procedure for a given table
---------------------------------------------------------------------------------------------------------------------------------------- */  

CREATE PROCEDURE [dbo].[GenerateInsUpdateScript]
	@objname nvarchar(776) = NULL		-- object name we're after
as
SET NOCOUNT ON
	declare @objid int
	declare @sysobj_type char(2)
	select @objid = id, @sysobj_type = xtype from sysobjects where id = object_id(@objname)
	declare @colname sysname
			
	select @colname = name from syscolumns where id = @objid and colstat & 1 = 1



	-- DISPLAY COLUMN IF TABLE / VIEW
	if @sysobj_type in ('S ','U ','V ','TF','IF')
	begin

		-- SET UP NUMERIC TYPES: THESE WILL HAVE NON-BLANK PREC/SCALE
		declare @numtypes nvarchar(80)
		declare @avoidlength nvarchar(80)
		select @numtypes = N'decimalreal,money,float,numeric,smallmoney'
		select @avoidlength = N'int,smallint,datatime,smalldatetime,text,bit,bigint'
		-- INFO FOR EACH COLUMN
		CREATE TABLE #MyProc
		(pkey int NOT NULL IDENTITY (1, 1),
		ID INT ,
		MyStatement NVARCHAR(4000))

		INSERT INTO #MyProc (ID, MyStatement)
		SELECT 1, 'CREATE PROCEDURE InsUpd_' + @objname + ' ' 

		INSERT INTO #MyProc (ID, MyStatement)
		select
			2,  '		@' + name + ' ' + 
					  type_name(xusertype) + ' ' 
			                  + case when charindex(type_name(xtype),@avoidlength) > 0
				          then ''
					  else
					  case when charindex(type_name(xtype), @numtypes) <= 0 then '(' + convert(varchar(10), length) + ')' else '(' +
					  case when charindex(type_name(xtype), @numtypes) > 0
					  then convert(varchar(5),ColumnProperty(id, name, 'precision'))
					  else '' end + case when charindex(type_name(xtype), @numtypes) > 0 then ',' else ' ' end + 
					  case when charindex(type_name(xtype), @numtypes) > 0
					  then convert(varchar(5),OdbcScale(xtype,xscale))
					  else '' end + ')' end end + ', '

		from syscolumns where id = @objid and number = 0 order by colid

	update #MyProc set MyStatement = Replace(MyStatement,', ',' ') where 
	pkey = (select max(pkey) from #MyProc)

	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 3, 'AS 
	BEGIN 
	IF @' + @colname + ' <= 0 
		BEGIN'
	
	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 3, '			INSERT INTO dbo.' + @objname + ' ('
		
	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 4, '				' +  name + ','
		from syscolumns where id = @objid and number = 0 order by colid

	DELETE FROM #MyProc 
	WHERE ID = 4 and MyStatement like '%' + @colname + '%'

	update #MyProc set MyStatement = Replace(MyStatement,',','') where 
	pkey = (select max(pkey) from #MyProc)

	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 5, ' 				)'

	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 6, ' 			VALUES ('

	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 7, '				@' +  name + ','
		from syscolumns where id = @objid and number = 0 order by colid

	DELETE FROM #MyProc 
	WHERE ID = 7 and MyStatement like '%' + @colname + '%'

	update #MyProc set MyStatement = Replace(MyStatement,'@DateCreated,','GETDATE(),')  where 
	ID = 7 AND MyStatement like '%@DateCreated,'

	update #MyProc set MyStatement = Replace(MyStatement,'@DateModified,','GETDATE(),')  where 
	ID = 7 AND MyStatement like '%@DateModified,'

	update #MyProc set MyStatement = Replace(MyStatement,',','') where 
	pkey = (select max(pkey) from #MyProc)

	

	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 8, '		 		)

		SET @' +  @colname + ' = @@IDENTITY 

		SELECT @' +   @colname + ' AS '  +  @colname + '

	END
	
	ELSE
		BEGIN'
	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 9, ' '

	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 10, '			UPDATE dbo.' +  @objname +  ' 
			SET '  
		
		INSERT INTO #MyProc (ID, MyStatement)
	SELECT 11, '				' +  name + ' = @' + name + ','
		from syscolumns where id = @objid and number = 0 order by colid

	DELETE FROM #MyProc 
	WHERE ID = 11 and MyStatement like '%' + @colname + '%'

	DELETE FROM #MyProc 
	WHERE ID = 11 and MyStatement like '%DateCreated %'

	update #MyProc set MyStatement = Replace(MyStatement,'@DateModified,','GETDATE(),')  where 
	ID = 11 AND MyStatement like '%@DateModified,'

	update #MyProc set MyStatement = Replace(MyStatement,',','') where 
	pkey = (select max(pkey) from #MyProc)

	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 12, '			WHERE 	
				'  + @colname + '= @' + @colname +  ' 	

			SELECT @' +   @colname + ' AS ' + @colname + '
		END
	END'

	SELECT MyStatement from #MyProc ORDER BY ID 
	
end
GO
/****** Object:  StoredProcedure [dbo].[GenerateSelectDtlScript]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GenerateSelectDtlScript]
	@objname nvarchar(776) = NULL		-- object name we're after
as
SET NOCOUNT ON
	declare @objid int
	declare @sysobj_type char(2)
	select @objid = id, @sysobj_type = xtype from sysobjects where id = object_id(@objname)
	declare @colname sysname
			
	select @colname = name from syscolumns where id = @objid and colstat & 1 = 1



	-- DISPLAY COLUMN IF TABLE / VIEW
	if @sysobj_type in ('S ','U ','V ','TF','IF')
	begin

		-- SET UP NUMERIC TYPES: THESE WILL HAVE NON-BLANK PREC/SCALE
		declare @numtypes nvarchar(80)
		declare @avoidlength nvarchar(80)
		select @numtypes = N'decimalreal,money,float,numeric,smallmoney'
		select @avoidlength = N'int,smallint,datatime,smalldatetime,text,bit,bigint'
		-- INFO FOR EACH COLUMN
		CREATE TABLE #MyProc
		(pkey int NOT NULL IDENTITY (1, 1),
		ID INT ,
		MyStatement NVARCHAR(4000))

		INSERT INTO #MyProc (ID, MyStatement)
		SELECT 1, 'CREATE PROCEDURE Sel_' + @objname + '_Dtl ' 
		
		INSERT INTO #MyProc (ID, MyStatement)
		select
			2,  ' 	@' + @colname + ' (18,0) 
AS 
BEGIN
				' 
		
		INSERT INTO #MyProc (ID, MyStatement)
		select
			3,  '	SELECT'

		INSERT INTO #MyProc (ID, MyStatement)
		select
			4,  '		' + name +  ', '

		from syscolumns where id = @objid and number = 0 order by colid

	update #MyProc set MyStatement = Replace(MyStatement,', ',' ') where 
	pkey = (select max(pkey) from #MyProc)

	
	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 5, '	FROM  
		dbo.' + @objname 
			INSERT INTO #MyProc (ID, MyStatement)
	SELECT 6, '	WHERE 	
		'  + @colname + ' = @' + @colname + '
		
END'

	SELECT MyStatement from #MyProc ORDER BY ID 
	
end
GO
/****** Object:  StoredProcedure [dbo].[GenerateSelectScript]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GenerateSelectScript]
	@objname nvarchar(776) = NULL		-- object name we're after
as
SET NOCOUNT ON
	declare @objid int
	declare @sysobj_type char(2)
	select @objid = id, @sysobj_type = xtype from sysobjects where id = object_id(@objname)
	declare @colname sysname
			
	select @colname = name from syscolumns where id = @objid and colstat & 1 = 1



	-- DISPLAY COLUMN IF TABLE / VIEW
	if @sysobj_type in ('S ','U ','V ','TF','IF')
	begin

		-- SET UP NUMERIC TYPES: THESE WILL HAVE NON-BLANK PREC/SCALE
		declare @numtypes nvarchar(80)
		declare @avoidlength nvarchar(80)
		select @numtypes = N'decimalreal,money,float,numeric,smallmoney'
		select @avoidlength = N'int,smallint,datatime,smalldatetime,text,bit,bigint'
		-- INFO FOR EACH COLUMN
		CREATE TABLE #MyProc
		(pkey int NOT NULL IDENTITY (1, 1),
		ID INT ,
		MyStatement NVARCHAR(4000))

		INSERT INTO #MyProc (ID, MyStatement)
		SELECT 1, 'CREATE PROCEDURE Sel_' + @objname + '_List ' 
		
		INSERT INTO #MyProc (ID, MyStatement)
		select
			2,  ' 	 
AS 
BEGIN
				' 
		
		INSERT INTO #MyProc (ID, MyStatement)
		select
			3,  '	SELECT'

		INSERT INTO #MyProc (ID, MyStatement)
		select
			4,  '		' + name +  ', '

		from syscolumns where id = @objid and number = 0 order by colid

	update #MyProc set MyStatement = Replace(MyStatement,', ',' ') where 
	pkey = (select max(pkey) from #MyProc)

	
	INSERT INTO #MyProc (ID, MyStatement)
	SELECT 5, '	FROM  
		dbo.' + @objname 
			INSERT INTO #MyProc (ID, MyStatement)
	SELECT 6, '	
		
END'

	SELECT MyStatement from #MyProc ORDER BY ID 
	
end
GO
/****** Object:  StoredProcedure [dbo].[SearchForStringInSPs]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 	TITLE 	: PROCEDURE WHICH RETURNS LIST OF ALL THE USER DEFINED STORED PROCEDURES WHICH CONTAIN A SPECIFIC STRING.
	AUTHOR 	: PRASAD BHOGADI
	ORGANIZATION : INFORAISE TECHNOLOGIES PVT LTD, INDIA.
*/


CREATE  PROCEDURE [dbo].[SearchForStringInSPs] @searchfor VARCHAR(100)
AS
DECLARE @spcode varchar(8000),
	@spname varchar(100),
	@occurance int,
	@rowcount int
set nocount on
CREATE TABLE #SPNAMES 
(SPNAME varchar(100))


		DECLARE GETSPCODE CURSOR FOR

			SELECT 	syscomments.text,
				sysobjects.name

			FROM 
				sysobjects,syscomments

			WHERE 
				sysobjects.id = syscomments.id

			AND 
				sysobjects.type IN ('P','V')
			AND 
				sysobjects.category=0

		OPEN GETSPCODE 
		FETCH  NEXT FROM GETSPCODE into @spcode,@spname
		WHILE @@FETCH_STATUS =0
			BEGIN
				SET @occurance = (SELECT CHARINDEX(@searchfor,@spcode))
				IF @occurance > 0
					BEGIN
						INSERT INTO #SPNAMES(SPNAME ) VALUES(@spname)	
					END
		FETCH  NEXT FROM GETSPCODE into @spcode,@spname
		SET @rowcount=@rowcount-1
		END
		CLOSE GETSPCODE
		DEALLOCATE GETSPCODE


SELECT DISTINCT(LTRIM(RTRIM(SPNAME))) FROM #SPNAMES
GO
/****** Object:  StoredProcedure [dbo].[Sel_ActiveCampaign_List]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Sel_ActiveCampaign_List] 
AS 
BEGIN
	SELECT
		CampaignID, 
		[Description], 
		ShortDescription, 
		FundRaiserDataTracking, 
		RecordLevelCallHistory, 
		OnsiteTransfer, 
		AllowDuplicatePhones, 
		CampaignDBConnString,
		StatusID, 
		DateCreated, 
		DateModified,
		FlushCallQueueOnIdle,
		DialAllNumbers,
		OutboundCallerID,
		DuplicateRule
	FROM  
		dbo.Campaign WITH (NOLOCK)

	WHERE
		IsDeleted=0 AND StatusID=2
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_Campaign_List]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_Campaign_List] 
AS 
BEGIN
	SELECT
		CampaignID, 
		[Description], 
		ShortDescription, 
		FundRaiserDataTracking, 
		RecordLevelCallHistory, 
		OnsiteTransfer, 
		AllowDuplicatePhones, 
		CampaignDBConnString,
		StatusID, 
		DateCreated, 
		DateModified,
		FlushCallQueueOnIdle,
		DialAllNumbers,
		OutboundCallerID,
		DuplicateRule
	FROM  
		dbo.Campaign WITH (NOLOCK)

		WHERE
		IsDeleted=0
		
	
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_Campaign_ListByCampaignID]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_Campaign_ListByCampaignID] 
	@CampaignID bigint
AS 

BEGIN

	SELECT
		CampaignID, 
		[Description], 
		ShortDescription, 
		FundRaiserDataTracking, 
		RecordLevelCallHistory, 
		OnsiteTransfer, 
		AllowDuplicatePhones, 
		CampaignDBConnString,
		StatusID,
		IsDeleted,
		DateCreated, 
		DateModified ,
		OutboundCallerID,
		DuplicateRule
	FROM  
		dbo.Campaign WITH (NOLOCK)
	where
		CampaignID =@CampaignID

END
GO
/****** Object:  StoredProcedure [dbo].[Sel_LoginStatus_List]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Sel_LoginStatus_List]  	 
AS 
BEGIN				
	SELECT
		LoginStatusID, 
		Status,
		DateCreated, 
		DateModified 
	FROM  
		dbo.LoginStatus		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_PhoneLinesInUse]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_PhoneLinesInUse] 
	@CampaignID BIGINT 
AS 
BEGIN
	
	DECLARE @LineCount INT 
	CREATE TABLE #LINECOUNT(LineCount INT)

	DECLARE @ID BIGINT
	DECLARE @ShortDesc VARCHAR(50)

	DECLARE CampaignDetails  CURSOR
	FOR	
	SELECT CampaignID, ShortDescription FROM campaign WHERE (statusid = 2 AND isdeleted = 0) OR CampaignID = @CampaignID
	
	OPEN CampaignDetails
		
	FETCH NEXT FROM CampaignDetails INTO @ID,  @ShortDesc
	
	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE @SQL VARCHAR(5000)
		SET @SQL = ' INSERT INTO #LINECOUNT SELECT PhoneLineCount FROM ['+@ShortDesc+'].[dbo].DialingParameter'
		PRINT @SQL
		EXEC(@SQL)
		FETCH NEXT FROM CampaignDetails INTO @ID,  @ShortDesc
	END
	CLOSE CampaignDetails
	DEALLOCATE CampaignDetails
	
	Select SUM(LineCount) FROM #LINECOUNT as LineCountInUse
END
GO
/****** Object:  StoredProcedure [dbo].[GetCampaignStatus]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[GetCampaignStatus] 
	@CampaignID bigint
AS
BEGIN 
	SELECT StatusID
	FROM 
		Campaign
	WHERE 
		CampaignID = @CampaignID
END
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignStatus]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Upd_CampaignStatus]
		@CampaignID bigint,		
		@StatusID bigint	
AS
BEGIN 	
	UPDATE dbo.Campaign
	SET		
		StatusID = @StatusID	
	WHERE 	
		CampaignID= @CampaignID 
END
GO
/****** Object:  StoredProcedure [dbo].[ResetCampaignsToIdle]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ResetCampaignsToIdle]
AS
BEGIN 	
	UPDATE dbo.Campaign
	SET		
		StatusID = 1	
END
GO
/****** Object:  Table [dbo].[Agent]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Agent](
	[AgentID] [bigint] IDENTITY(1,1) NOT NULL,
	[AgentName] [varchar](50) NOT NULL,
	[LoginName] [varchar](50) NOT NULL,
	[Password] [varchar](255) NOT NULL,
	[IsAdministrator] [bit] NOT NULL,
	[AllowManualDial] [bit] NOT NULL,
	[VerificationAgent] [bit] NOT NULL,
	[InBoundAgent] [bit] NOT NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[IsReset] [bit] NULL,
 CONSTRAINT [PK_Agent] PRIMARY KEY CLUSTERED 
(
	[AgentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_Agent_1] UNIQUE NONCLUSTERED 
(
	[LoginName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DataManagerOption]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DataManagerOption](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CampaignId] [bigint] NULL,
	[QueryId] [bigint] NULL,
	[RowLimit] [bigint] NULL,
	[SortColumn] [bigint] NULL,
	[SortActive] [bigint] NULL,
	[SortDirection] [bigint] NULL,
	[Description] [varchar](100) NULL,
	[ShowCsvHeaders] [int] NULL,
	[IsNamedQuery] [int] NULL,
	[Name] [varchar](100) NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DataManagerColumn]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DataManagerColumn](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[DataManagerOptionId] [bigint] NULL,
	[Name] [varchar](100) NULL,
	[Width] [bigint] NULL,
	[Hidden] [bigint] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AgentCampaignMap]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AgentCampaignMap](
	[AgentCampaignMapID] [bigint] IDENTITY(1,1) NOT NULL,
	[AgentID] [bigint] NOT NULL,
	[CampaignID] [bigint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_AgentCampaignMap] PRIMARY KEY CLUSTERED 
(
	[AgentCampaignMapID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AgentStation]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AgentStation](
	[StationID] [bigint] IDENTITY(1,1) NOT NULL,
	[StationIP] [varchar](255) NULL,
	[StationNumber] [varchar](50) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[AllwaysOffHook] [bit] NOT NULL,
 CONSTRAINT [PK_AgentStation] PRIMARY KEY CLUSTERED 
(
	[StationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AgentStation] ON [dbo].[AgentStation] 
(
	[StationIP] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_AgentStation_1] ON [dbo].[AgentStation] 
(
	[StationNumber] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AgentStatus]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AgentStatus](
	[AgentStatusID] [bigint] IDENTITY(1,1) NOT NULL,
	[Status] [varchar](50) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_LoginStatus] PRIMARY KEY CLUSTERED 
(
	[AgentStatusID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[Upd_AgentLogOut]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_AgentLogOut]
	@AgentID bigint,	
	@AgentActivityID bigint
AS 
BEGIN

UPDATE AgentActivity SET	
	 LogoutTime = GETDATE() 
WHERE 
	AgentActivityID = @AgentActivityID AND AgentID = @AgentID AND LogoutTime IS NULL

END
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignQuery_Status]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Upd_CampaignQuery_Status] 
		@CampaignID bigint,		
		@StatusID bigint,
		@FlushCallQueueOnIdle bit	
AS 	
	UPDATE dbo.Campaign 
	SET	
		StartTime = (CASE WHEN @StatusID = 2 THEN GETDATE() ELSE StartTime END),
		StopTime = (CASE WHEN @StatusID = 2 THEN NULL 
				ELSE (CASE WHEN StatusID = 2 THEN GETDATE() ELSE StopTime END) END),
		StatusID = @StatusID,	
		FlushCallQueueOnIdle = @FlushCallQueueOnIdle,	
		DateModified = GETDATE()
	WHERE 	
		CampaignID= @CampaignID 	
	
	SELECT @CampaignID as CampaignID
GO
/****** Object:  StoredProcedure [dbo].[Upd_Campaign_DialStatus]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_Campaign_DialStatus] 
		@CampaignID bigint,		
		@DialAllNumbers bit	
AS 	
	UPDATE dbo.Campaign 
	SET		
		DialAllNumbers = @DialAllNumbers,	
		DateModified = GETDATE()
	WHERE 	
		CampaignID= @CampaignID 	
	
	SELECT @CampaignID as CampaignID
GO
/****** Object:  Table [dbo].[AgentActivity]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AgentActivity](
	[AgentActivityID] [bigint] IDENTITY(1,1) NOT NULL,
	[AgentID] [bigint] NOT NULL,
	[AgentStatusID] [bigint] NOT NULL,
	[AgentReceiptModeID] [bigint] NULL,
	[CampaignID] [bigint] NULL,
	[LoginTime] [datetime] NOT NULL,
	[LogoutTime] [datetime] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[StationIP] [varchar](255) NULL,
	[StationHostName] [varchar](255) NOT NULL,
 CONSTRAINT [PK_AgentActivity] PRIMARY KEY CLUSTERED 
(
	[AgentActivityID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[AreaCodeRule]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AreaCodeRule](
	[AreaCodeRuleID] [bigint] IDENTITY(1,1) NOT NULL,
	[AgentID] [bigint] NOT NULL,
	[AreaCodeID] [bigint] NULL,
	[LikeDialing] [bit] NOT NULL,
	[LikeDialingOption] [bit] NULL,
	[CustomeDialing] [bit] NULL,
	[IsSevenDigit] [bit] NULL,
	[IsTenDigit] [bit] NULL,
	[IntraLataDialing] [bit] NULL,
	[IntraLataDialingAreaCode] [varchar](5) NULL,
	[ILDIsTenDigit] [bit] NULL,
	[ILDElevenDigit] [bit] NULL,
	[ReplaceAreaCode] [varchar](5) NULL,
	[LongDistanceDialing] [bit] NULL,
	[IsDeleted] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_AreaCodeRule] PRIMARY KEY CLUSTERED 
(
	[AreaCodeRuleID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_AreaCode]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_AreaCode] 
		@AreaCodeID bigint , 
		@AreaCode varchar (50), 
		@Prefix varchar (10), 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 
	IF @AreaCodeID <= 0 
		BEGIN
			INSERT INTO dbo.AreaCode (
				AreaCode,
				Prefix,
				DateCreated,
				DateModified
 				)
 			VALUES (
				@AreaCode,
				@Prefix,
				GETDATE(),
				GETDATE()
		 		)

		SET @AreaCodeID = @@IDENTITY 

		SELECT @AreaCodeID AS AreaCodeID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.AreaCode 
			SET 
				AreaCode = @AreaCode,
				Prefix = @Prefix,
				DateModified = GETDATE()
			WHERE 	
				AreaCodeID= @AreaCodeID 	

			SELECT @AreaCodeID AS AreaCodeID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[DEL_AreaCode]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
create PROCEDURE [dbo].[DEL_AreaCode] 
	@AreaCodeID bigint
AS
DELETE FROM dbo.AreaCode
WHERE
	AreaCodeID = @AreaCodeID
GO
/****** Object:  StoredProcedure [dbo].[Sel_AreaCode]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Sel_AreaCode] 	 
AS 
BEGIN				
	SELECT
		AreaCodeID, 
		AreaCode, 
		Prefix,
		DateCreated, 
		DateModified 
	FROM  
		dbo.AreaCode		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_AreaCodeRule_ByAgentID]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_AreaCodeRule_ByAgentID] 
	@AgentID BIGINT 	 
AS 
BEGIN
				
	SELECT
		AreaCodeRuleID, 
		AgentID, 
		AreaCodeID, 
		LikeDialing, 
		LikeDialingOption, 
		CustomeDialing, 
		IsSevenDigit, 
		IsTenDigit, 
		IntraLataDialing, 
		IntraLataDialingAreaCode, 
		ILDIsTenDigit, 
		ILDElevenDigit, 
		ReplaceAreaCode, 
		LongDistanceDialing, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.AreaCodeRule
	WHERE
		AgentID = @AgentID
	AND
		IsDeleted = 0
	
		
END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_AreaCodeRule]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_AreaCodeRule] 
		@AreaCodeRuleID bigint , 
		@AgentID bigint , 
		@AreaCodeID bigint , 
		@LikeDialing bit , 
		@LikeDialingOption bit , 
		@CustomeDialing bit , 
		@IsSevenDigit bit , 
		@IsTenDigit bit , 
		@IntraLataDialing bit , 
		@IntraLataDialingAreaCode varchar(5) , 
		@ILDIsTenDigit bit , 
		@ILDElevenDigit bit , 
		@ReplaceAreaCode varchar(5) , 
		@LongDistanceDialing bit , 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 
	IF @AreaCodeRuleID <= 0 
		BEGIN
			INSERT INTO dbo.AreaCodeRule (
				AgentID,
				AreaCodeID,
				LikeDialing,
				LikeDialingOption,
				CustomeDialing,
				IsSevenDigit,
				IsTenDigit,
				IntraLataDialing,
				IntraLataDialingAreaCode,
				ILDIsTenDigit,
				ILDElevenDigit,
				ReplaceAreaCode,
				LongDistanceDialing,
				DateCreated,
				DateModified
 				)
 			VALUES (
				@AgentID,
				@AreaCodeID,
				@LikeDialing,
				@LikeDialingOption,
				@CustomeDialing,
				@IsSevenDigit,
				@IsTenDigit,
				@IntraLataDialing,
				@IntraLataDialingAreaCode,
				@ILDIsTenDigit,
				@ILDElevenDigit,
				@ReplaceAreaCode,
				@LongDistanceDialing,
				GETDATE(),
				GETDATE()
		 		)

		SET @AreaCodeRuleID = @@IDENTITY 

		SELECT @AreaCodeRuleID AS AreaCodeRuleID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.AreaCodeRule 
			SET 
				AgentID = @AgentID,
				AreaCodeID = @AreaCodeID,
				LikeDialing = @LikeDialing,
				LikeDialingOption = @LikeDialingOption,
				CustomeDialing = @CustomeDialing,
				IsSevenDigit = @IsSevenDigit,
				IsTenDigit = @IsTenDigit,
				IntraLataDialing = @IntraLataDialing,
				IntraLataDialingAreaCode = @IntraLataDialingAreaCode,
				ILDIsTenDigit = @ILDIsTenDigit,
				ILDElevenDigit = @ILDElevenDigit,
				ReplaceAreaCode = @ReplaceAreaCode,
				LongDistanceDialing = @LongDistanceDialing,
				DateModified = GETDATE()
			WHERE 	
				AreaCodeRuleID= @AreaCodeRuleID 	

			SELECT @AreaCodeRuleID AS AreaCodeRuleID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[DEL_Agent]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DEL_Agent] 
	@AgentID bigint
AS

UPDATE dbo.AreaCodeRule
SET
	IsDeleted = 1
WHERE
	AgentID = @AgentID

UPDATE dbo.AgentActivity
SET
	IsDeleted = 1
	
WHERE
	AgentID = @AgentID


UPDATE dbo.Agent

SET
	
        IsDeleted =1,

	LoginName = LoginName + '-' + CAST(AgentID as VARCHAR),

	PhoneNumber = PhoneNumber + '-' + CAST(AgentID as VARCHAR)

WHERE
	AgentID = @AgentID
GO
/****** Object:  StoredProcedure [dbo].[InsGet_AgentActivity]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[InsGet_AgentActivity] 
	@AgentID bigint,
	@StationIP varchar(255),
	@StationHostName varchar(255) = ''
AS 
BEGIN

UPDATE AgentActivity SET LogoutTime = GETDATE() WHERE
	AgentID = @AgentID AND LogoutTime IS NULL

INSERT INTO AgentActivity VALUES
	(@AgentID,
	2,
	0,
	null,
	getdate(),
	null,
	0,
	getdate(),
	getdate(),
	@StationIP,
	@StationHostName)

DECLARE @allwaysOffHook bit
DECLARE @StationNumber VARCHAR(50)

SELECT @allwaysOffHook = AllwaysOffHook,@StationNumber = StationNumber 
		FROM agentstation WHERE (stationip = @StationHostName or stationip = @StationIP) and isdeleted = 0

SELECT AgentActivityID,
	AgentID,
	AgentStatusID,
	AgentReceiptModeID,
	CampaignID,
	LoginTime,
	StationHostName,
	ISNULL(@allwaysOffHook,0) as AllwaysOffHook,
	ISNULL(@StationNumber,'') as StationNumber
FROM 
	AgentActivity
WHERE 
	LogoutTime IS NULL 
AND 
	AgentID = @AgentID
AND 
	IsDeleted = 0	

END
GO
/****** Object:  StoredProcedure [dbo].[Sel_LoggedInAgents]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Sel_LoggedInAgents]
AS
Select Agent.AgentID,
	AgentName,
	PhoneNumber,
	AgentStatusID,
	AgentReceiptModeID,
	Campaign.ShortDescription,
	Campaign.CampaignDBConnString,
	AgentActivity.CampaignID,
	Campaign.OutboundCallerID,
	ISNULL(AgentStation.StationNumber,PhoneNumber) as StationNumber,
	ISNULL(AgentStation.AllwaysOffHook, 0) as AllwaysOffHook,
	Agent.AllowManualDial,
	Agent.VerificationAgent
FROM 
	Agent
INNER JOIN 
	AgentActivity
ON 
	AgentActivity.AgentID = Agent.AgentID
INNER JOIN 
	Campaign
ON 
	Campaign.CampaignID = AgentActivity.CampaignID
LEFT OUTER JOIN 
	AgentStation
ON 
	((AgentActivity.StationIP = AgentStation.StationIP 
	OR
	AgentActivity.StationHostName = AgentStation.StationIP)
	AND AgentStation.IsDeleted = 0)
	
WHERE 
	LogoutTime IS NULL AND AgentActivity.CampaignID IS NOT NULL
	AND AgentActivity.ISDeleted = 0
GO
/****** Object:  StoredProcedure [dbo].[Upd_AgentActivity]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Upd_AgentActivity]
	@AgentID bigint,
	@CampaignID bigint,
	@AgentStatusID bigint,
	@AgentReceiptModeID bigint,
	@AgentActivityID bigint,
	@IsDeleted bit
AS 
BEGIN

UPDATE AgentActivity SET	
	AgentStatusID = @AgentStatusID,
	AgentReceiptModeID = @AgentReceiptModeID,
	CampaignID = @CampaignID,
	IsDeleted = @IsDeleted	
WHERE 
	AgentActivityID = @AgentActivityID AND AgentID = @AgentID

END
GO
/****** Object:  StoredProcedure [dbo].[Upd_DialerStart]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_DialerStart]
	@DialerActivityID bigint,
	@DialerStartTime datetime
AS
	UPDATE DialerActivity SET 
		DialerStartTime = @DialerStartTime 
	WHERE DialerActivityID = @DialerActivityID
GO
/****** Object:  StoredProcedure [dbo].[Upd_DialerStop]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_DialerStop]
	@DialerActivityID bigint,
	@DialerStopTime datetime
AS
	UPDATE DialerActivity SET 
		DialerStopTime = @DialerStopTime,
		DisconnectTime = @DialerStopTime
	WHERE DialerActivityID = @DialerActivityID
GO
/****** Object:  StoredProcedure [dbo].[Ins_DialerActivity]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Ins_DialerActivity]
	@ConnectTime DateTime
AS
	DECLARE @DialerActivityID as bigint

	UPDATE DialerActivity SET DialerStopTime = getdate(), DisconnectTime = getdate() 
	WHERE  DialerStopTime IS NULL and DialerStartTime IS NOT NULL
		
	INSERT INTO DialerActivity(ConnectTime) VALUES(@ConnectTime)
	SET @DialerActivityID = @@IDENTITY
	
	SELECT @DialerActivityID as DialerActivityID
GO
/****** Object:  StoredProcedure [dbo].[IsDialerRunning]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[IsDialerRunning] 
AS 
BEGIN
	SELECT 
		COUNT(DialerActivityID) as IsRunning 
	FROM 
		[RainMakerMaster].[dbo].[DialerActivity]
	WHERE 
		DialerStartTime IS NOT NULL AND DialerStopTime IS NULL

END
GO
/****** Object:  StoredProcedure [dbo].[Sel_FieldTypes_List]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_FieldTypes_List]
AS
BEGIN
	SELECT 
		FieldTypeID,
		FieldType,
		DBFieldType,
		DateCreated,
		DateModified
	FROM
		dbo.FieldTypes
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_GlobalDialingParameters_List]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_GlobalDialingParameters_List] 
 	 
AS 
BEGIN
				
	SELECT
		GlobalDialingID, 
		Prefix, 
		Suffix, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.GlobalDialingParameters
	
		
END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_GlobalDialingParameters]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_GlobalDialingParameters] 
		@GlobalDialingID bigint , 
		@Prefix varchar (50), 
		@Suffix varchar (50), 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 
	IF @GlobalDialingID <= 0 
		BEGIN
			INSERT INTO dbo.GlobalDialingParameters (
				Prefix,
				Suffix,
				DateCreated,
				DateModified
 				)
 			VALUES (
				@Prefix,
				@Suffix,
				GETDATE(),
				GETDATE()
		 		)

		SET @GlobalDialingID = @@IDENTITY 

		SELECT @GlobalDialingID AS GlobalDialingID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.GlobalDialingParameters 
			SET 
				Prefix = @Prefix,
				Suffix = @Suffix,
				DateModified = GETDATE()
			WHERE 	
				GlobalDialingID= @GlobalDialingID 	

			SELECT @GlobalDialingID AS GlobalDialingID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[Ins_AdminRequest]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Ins_AdminRequest]
	@RequestType INT,
	@RequestData VARCHAR(200)
AS
BEGIN
	DECLARE @RequestID as bigint
	
	INSERT INTO AdminRequests(RequestType, DateTimeSubmitted, RequestData, RequestStatus) 
		VALUES(@RequestType, GETDATE(), @RequestData, 1)
	SET @RequestID = @@IDENTITY
	
	SELECT @RequestID as RequestID
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_AdminRequests]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Sel_AdminRequests] 	 
AS 
BEGIN				
	SELECT
		RequestID, 
		RequestType, 
		DateTimeSubmitted,
		RequestData, 
		RequestStatus 
	FROM  
		dbo.AdminRequests
	WHERE
		RequestStatus = 1
	ORDER BY
		DateTimeSubmitted ASC 			
END
GO
/****** Object:  StoredProcedure [dbo].[Upd_AdminRequestStatus]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Upd_AdminRequestStatus]
	@RequestID BIGINT,
	@RequestStatus VARCHAR(200) 	 
AS 
BEGIN 
	UPDATE dbo.AdminRequests 
	SET 
		RequestStatus = @RequestStatus
	WHERE 	
		RequestID = @RequestID 	
		
	SELECT @RequestID AS RequestID
END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_Campaign]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_Campaign] 
		@CampaignID bigint , 
		@Description varchar (255), 
		@ShortDescription varchar (8), 
		@FundRaiserDataTracking bit , 
		@RecordLevelCallHistory bit , 
		@OnsiteTransfer bit ,
		@EnableAgentTraining bit, 
		@AllowDuplicatePhones bit ,
		@Allow7DigitNums bit,
		@Allow10DigitNums bit, 
		@CampaignDBConnString varchar (255), 
		@StatusID bigint,
		@IsDeleted bit,		
		@FlushCallQueueOnIdle bit,
		@DateCreated datetime , 
		@DateModified datetime ,
		@OutboundCallerID varchar(20) = '',
		@DuplicateRule CHAR(1) = 'I'
AS 
	BEGIN 
	IF @CampaignID <= 0 
		BEGIN
			INSERT INTO dbo.Campaign (
				Description,
				ShortDescription,
				FundRaiserDataTracking,
				RecordLevelCallHistory,
				OnsiteTransfer,
				EnableAgentTraining,
				AllowDuplicatePhones,
				CampaignDBConnString,	
				StatusID,
				FlushCallQueueOnIdle,
				DialAllNumbers,
				IsDeleted,
				OutboundCallerID,
				DateCreated,
				DateModified,
				DuplicateRule,
				Allow7DigitNums,
				Allow10DigitNums			
 				)
 			VALUES (
				@Description,
				@ShortDescription,
				@FundRaiserDataTracking,
				@RecordLevelCallHistory,
				@OnsiteTransfer,
				@EnableAgentTraining,
				@AllowDuplicatePhones,
				@CampaignDBConnString,
				@StatusID,
				@FlushCallQueueOnIdle,
				0,
				@IsDeleted,
				@OutboundCallerID,
				GETDATE(),
				GETDATE(),
		 		@DuplicateRule,
		 		@Allow7DigitNums,
		 		@Allow10DigitNums)

		SET @CampaignID = @@IDENTITY 
	END	
	ELSE
	BEGIN 
			UPDATE dbo.Campaign 
			SET 
				Description = @Description,
				ShortDescription = @ShortDescription,
				FundRaiserDataTracking = @FundRaiserDataTracking,
				RecordLevelCallHistory = @RecordLevelCallHistory,
				OnsiteTransfer = @OnsiteTransfer,
				EnableAgentTraining = @EnableAgentTraining,
				AllowDuplicatePhones = @AllowDuplicatePhones,
				CampaignDBConnString = @CampaignDBConnString,
				OutboundCallerID = @OutboundCallerID,
				--FlushCallQueueOnIdle = @FlushCallQueueOnIdle	
				--StatusID = @StatusID,
				IsDeleted = @IsDeleted,
				DateModified = GETDATE(),
				DuplicateRule = @DuplicateRule,
				Allow7DigitNums = @Allow7DigitNums,
				Allow10DigitNums = @Allow10DigitNums 
			WHERE 	
				CampaignID= @CampaignID 	

			
		END

		--UPDATE dbo.Campaign SET 
		--	CampaignDBConnString = CampaignDBConnString + CONVERT(VARCHAR(10), @CampaignID) 
		--WHERE CampaignID = @CampaignID

		SELECT @CampaignID AS CampaignID
	END
GO
/****** Object:  StoredProcedure [dbo].[Sel_Campaign_Dtl]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_Campaign_Dtl] 
 	@CampaignID bigint
AS 
BEGIN
				
	SELECT
		CampaignID, 
		Description, 
		ShortDescription, 
		FundRaiserDataTracking, 
		RecordLevelCallHistory, 
		OnsiteTransfer,
		EnableAgentTraining, 
		AllowDuplicatePhones,
		Allow7DigitNums,
		Allow10DigitNums, 
		CampaignDBConnString, 
		FlushCallQueueOnIdle, 
		StatusID, 
		IsDeleted, 
		DateCreated, 
		DateModified,
		DialAllNumbers,
		OutboundCallerID,
		StartTime,
		StopTime,
		DuplicateRule
	FROM  
		dbo.Campaign
	WHERE 	
		CampaignID = @CampaignID
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_Agent_Dtl]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Sel_Agent_Dtl] 
 	@AgentID bigint 
AS 
BEGIN
				
	SELECT
		AgentID, 
		AgentName, 
		LoginName, 
		Password, 
		IsAdministrator, 
		AllowManualDial, 
		VerificationAgent, 
		InBoundAgent, 
		PhoneNumber, 
		DateCreated, 
		DateModified,
		IsReset 
	FROM  
		dbo.Agent
	WHERE 	
		AgentID = @AgentID
	AND	
		IsDeleted=0
	
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_Agent_List]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Sel_Agent_List]  	 
AS 
BEGIN				
	SELECT
		AgentID, 
		AgentName, 
		LoginName, 
		Password, 
		IsAdministrator, 
		AllowManualDial, 
		VerificationAgent, 
		InBoundAgent, 
		PhoneNumber,
		IsDefault, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.Agent	
	WHERE
		IsDeleted = 0
	ORDER BY
		AgentName ASC	
END
GO
/****** Object:  StoredProcedure [dbo].[p_AgentInfoByLoginNameGet]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
------------------------------------------
--OBJECTNAME: p_AgentInfoByLoginNameGet
--AUTHOR: Nagasree Mendu
--DESCRIPTION: Returns Agent Info based
--on the Login User Name
--------------------------------------
CREATE PROCEDURE [dbo].[p_AgentInfoByLoginNameGet] 
	@LoginName varchar(8)
AS
	SELECT AgentID,
		AgentName,
		LoginName,
		Password,
		IsAdministrator,
		AllowManualDial,
		VerificationAgent,
		InBoundAgent,
		PhoneNumber,
		IsDefault,
		DateCreated,
		DateModified
	FROM 
		Agent WITH (NOLOCK)
	WHERE 
		LoginName = @LoginName
	AND 
		IsDeleted=0
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_Agent]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_Agent] 
		@AgentID bigint , 
		@AgentName varchar (50), 
		@LoginName varchar (8), 
		@Password varchar (255), 
		@IsAdministrator bit , 
		@AllowManualDial bit , 
		@VerificationAgent bit , 
		@InBoundAgent bit , 
		@PhoneNumber varchar (20), 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 
	IF @AgentID <= 0 
		BEGIN
			INSERT INTO dbo.Agent (
				AgentName,
				LoginName,
				Password,
				IsAdministrator,
				AllowManualDial,
				VerificationAgent,
				InBoundAgent,
				PhoneNumber,
				DateCreated,
				DateModified,
				IsReset
 				)
 			VALUES (
				@AgentName,
				@LoginName,
				@Password,
				@IsAdministrator,
				@AllowManualDial,
				@VerificationAgent,
				@InBoundAgent,
				@PhoneNumber,
				GETDATE(),
				GETDATE(),
				0
		 		)

		SET @AgentID = @@IDENTITY 

		SELECT @AgentID AS AgentID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.Agent 
			SET 
				AgentName = @AgentName,
				LoginName = @LoginName,
				Password = @Password,
				IsAdministrator = @IsAdministrator,
				AllowManualDial = @AllowManualDial,
				VerificationAgent = @VerificationAgent,
				InBoundAgent = @InBoundAgent,
				PhoneNumber = @PhoneNumber,
				DateModified = GETDATE()
			WHERE 	
				AgentID= @AgentID 	

			SELECT @AgentID AS AgentID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[ToggleAgentReset]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ToggleAgentReset] 
 	@AgentID bigint,
 	@ResetSwitch bit 
AS 
BEGIN
				
	UPDATE dbo.Agent
	SET IsReset = @ResetSwitch
	WHERE 	
		AgentID = @AgentID
	
END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_AgentCampaignMap]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[InsUpd_AgentCampaignMap]
		@AgentCampaignMapID bigint, 
		@AgentID bigint, 
		@CampaignID bigint, 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 
	IF @AgentCampaignMapID <= 0 
		BEGIN
			INSERT INTO dbo.AgentCampaignMap (
				AgentID,
				CampaignID,
				DateCreated,
				DateModified
 				)
 			VALUES (
				@AgentID,
				@CampaignID,
				GETDATE(),
				GETDATE()
		 		)

		SET @AgentCampaignMapID = @@IDENTITY 

		SELECT @AgentCampaignMapID AS AgentCampaignMapID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.AgentCampaignMap 
			SET 
				AgentID = @AgentID,
				CampaignID = @CampaignID,
				DateModified = GETDATE()
			WHERE 	
				AgentCampaignMapID = @AgentCampaignMapID 	

			SELECT @AgentCampaignMapID AS AgentCampaignMapID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_AgentCampaignMap2]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[InsUpd_AgentCampaignMap2]
		@AgentCampaignMapID bigint, 
		@AgentID bigint, 
		@CampaignID bigint, 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 
	IF @AgentCampaignMapID <= 0 
		BEGIN
			INSERT INTO dbo.AgentCampaignMap (
				AgentID,
				CampaignID,
				DateCreated,
				DateModified
 				)
 			VALUES (
				@AgentID,
				@CampaignID,
				GETDATE(),
				GETDATE()
		 		)

		SET @AgentCampaignMapID = @@IDENTITY 

		SELECT @AgentCampaignMapID AS AgentCampaignMapID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.AgentCampaignMap 
			SET 
				AgentID = @AgentID,
				CampaignID = @CampaignID,
				DateModified = GETDATE()
			WHERE 	
				AgentCampaignMapID = @AgentCampaignMapID 	

			SELECT @AgentCampaignMapID AS AgentCampaignMapID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[Sel_AgentCampaignMap_Dtl]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Sel_AgentCampaignMap_Dtl] 
 	@AgentID bigint 
AS 
BEGIN			
	SELECT
		AgentCampaignMapID,
		AgentID, 
		CampaignID,
		DateCreated, 
		DateModified 
	FROM  
		dbo.AgentCampaignMap
	WHERE 	
		AgentID = @AgentID
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_Station_Dtl]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_Station_Dtl]  
	@StationID bigint	 
AS 
BEGIN
	SELECT
		StationID,
		StationIP,
		StationNumber,
		AllwaysOffHook
	FROM  
		dbo.AgentStation
	WHERE
		StationID = @StationID
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_Station_List]    Script Date: 07/25/2011 15:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_Station_List]  	 
AS 
BEGIN
	SELECT
		StationID,
		StationIP,
		StationNumber,
		AllwaysOffHook
	FROM  
		dbo.AgentStation
	WHERE
		IsDeleted = 0
END
GO
/****** Object:  UserDefinedFunction [dbo].[CampaignAgentStationInfo]    Script Date: 07/25/2011 15:45:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[CampaignAgentStationInfo](@CampaignID BIGINT, @AgentID BIGINT) RETURNS VARCHAR(255)
BEGIN
	DECLARE @StationIP VARCHAR(255)

	SET @StationIP = (SELECT 
		TOP 1 AST.StationIP	
	FROM 
		AgentActivity AA
	LEFT OUTER JOIN 
		AgentStation AST
	ON 
		((AA.StationIP = AST.StationIP 
		OR
		AA.StationHostName = AST.StationIP)
		AND AST.IsDeleted = 0)	
	WHERE AA.AgentID = @AgentID AND
		AA.LogoutTime IS NULL AND AA.CampaignID = @CampaignID
		AND AA.ISDeleted = 0 ORDER BY AA.LoginTime DESC)
	RETURN @StationIP

END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_AgentStation]    Script Date: 07/25/2011 15:45:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_AgentStation]
	@StationID bigint,
	@StationIP VARCHAR(255),
	@StationNumber VARCHAR(50),
	@AllwaysOffHook bit = 0
AS 
BEGIN
	IF @StationID <= 0 
		BEGIN		
			INSERT INTO AgentStation (
				StationIP,
				StationNumber,
				AllwaysOffHook)
			VALUES (
				@StationIP,
				@StationNumber,
				@AllwaysOffHook)
			
			SET @StationID = @@IDENTITY 

			SELECT @StationID AS StationID
		END
		ELSE
		BEGIN
			UPDATE AgentStation SET
				StationIP = @StationIP,
				StationNumber = @StationNumber,
				AllwaysOffHook = @AllwaysOffHook
			WHERE StationID = @StationID

			SELECT @StationID AS StationID
		END
END
GO
/****** Object:  StoredProcedure [dbo].[Get_AgentActivity_ById]    Script Date: 07/25/2011 15:45:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[Get_AgentActivity_ById] 
	@AgentActivityID bigint,
	@StationIP varchar(255),
	@StationHostName varchar(255) = ''
AS 
BEGIN
DECLARE @allwaysOffHook bit
DECLARE @StationNumber VARCHAR(50)
SELECT @allwaysOffHook = AllwaysOffHook,@StationNumber = StationNumber 
		FROM agentstation WHERE (stationip = @StationHostName or stationip = @StationIP) and isdeleted = 0
SELECT AgentActivityID,
	AgentID,
	AgentStatusID,
	CampaignID,
	LoginTime,
	StationHostName,
	ISNULL(@allwaysOffHook,0) as AllwaysOffHook,
	ISNULL(@StationNumber,'') as StationNumber
FROM 
	AgentActivity
WHERE 
	AgentActivityID = @AgentActivityID
END
GO
/****** Object:  StoredProcedure [dbo].[DEL_AgentStation]    Script Date: 07/25/2011 15:45:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[DEL_AgentStation]
	@StationID bigint
AS 
BEGIN
	
	UPDATE AgentStation SET
		IsDeleted = 1,
		StationIP = StationIP + '-' + CAST(StationID as VARCHAR),
		StationNumber = StationNumber + '-' + CAST(StationID as VARCHAR)
	WHERE StationID = @StationID
		
END
GO
/****** Object:  Default [DF_AreaCode_IsDeleted]    Script Date: 07/25/2011 15:45:30 ******/
ALTER TABLE [dbo].[AreaCode] ADD  CONSTRAINT [DF_AreaCode_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_AreaCode_DateCreated]    Script Date: 07/25/2011 15:45:30 ******/
ALTER TABLE [dbo].[AreaCode] ADD  CONSTRAINT [DF_AreaCode_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_AreaCode_DateModified]    Script Date: 07/25/2011 15:45:30 ******/
ALTER TABLE [dbo].[AreaCode] ADD  CONSTRAINT [DF_AreaCode_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_FieldTypes_DateCreated]    Script Date: 07/25/2011 15:45:30 ******/
ALTER TABLE [dbo].[FieldTypes] ADD  CONSTRAINT [DF_FieldTypes_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_FieldTypes_DateModified]    Script Date: 07/25/2011 15:45:30 ******/
ALTER TABLE [dbo].[FieldTypes] ADD  CONSTRAINT [DF_FieldTypes_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_GlobalDialingParamters_DateCreated]    Script Date: 07/25/2011 15:45:30 ******/
ALTER TABLE [dbo].[GlobalDialingParameters] ADD  CONSTRAINT [DF_GlobalDialingParamters_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_GlobalDialingParamters_DateModified]    Script Date: 07/25/2011 15:45:30 ******/
ALTER TABLE [dbo].[GlobalDialingParameters] ADD  CONSTRAINT [DF_GlobalDialingParamters_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_Status_DateCreated]    Script Date: 07/25/2011 15:45:30 ******/
ALTER TABLE [dbo].[Status] ADD  CONSTRAINT [DF_Status_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_Status_DateModified]    Script Date: 07/25/2011 15:45:30 ******/
ALTER TABLE [dbo].[Status] ADD  CONSTRAINT [DF_Status_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_TimeZone_DateCreated]    Script Date: 07/25/2011 15:45:30 ******/
ALTER TABLE [dbo].[TimeZone] ADD  CONSTRAINT [DF_TimeZone_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_TimeZone_DateModified]    Script Date: 07/25/2011 15:45:30 ******/
ALTER TABLE [dbo].[TimeZone] ADD  CONSTRAINT [DF_TimeZone_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_Campaign_FundRaiserDataTracking]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_FundRaiserDataTracking]  DEFAULT ((0)) FOR [FundRaiserDataTracking]
GO
/****** Object:  Default [DF_Campaign_RecordLevelCallHistory]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_RecordLevelCallHistory]  DEFAULT ((0)) FOR [RecordLevelCallHistory]
GO
/****** Object:  Default [DF_Campaign_OnsiteTransfer]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_OnsiteTransfer]  DEFAULT ((0)) FOR [OnsiteTransfer]
GO
/****** Object:  Default [DF_Campaign_AllowDuplicatePhones]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_AllowDuplicatePhones]  DEFAULT ((0)) FOR [AllowDuplicatePhones]
GO
/****** Object:  Default [DF_Campaign_FlushCallQueueOnIdel]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_FlushCallQueueOnIdel]  DEFAULT ((0)) FOR [FlushCallQueueOnIdle]
GO
/****** Object:  Default [DF_Campaign_DialAllNumbers]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_DialAllNumbers]  DEFAULT ((0)) FOR [DialAllNumbers]
GO
/****** Object:  Default [DF_Campaign_IsDeleted]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_Campaign_DateCreated]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_Campaign_DateModified]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_Campaign_OutboundCallerID]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_OutboundCallerID]  DEFAULT ((7202838475.)) FOR [OutboundCallerID]
GO
/****** Object:  Default [DF_Campaign_DuplicateRule]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_DuplicateRule]  DEFAULT ('I') FOR [DuplicateRule]
GO
/****** Object:  Default [DF_Campaign_Allow7DigitNums]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_Allow7DigitNums]  DEFAULT ((0)) FOR [Allow7DigitNums]
GO
/****** Object:  Default [DF_Campaign_Allow10DigitNums]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_Allow10DigitNums]  DEFAULT ((1)) FOR [Allow10DigitNums]
GO
/****** Object:  Default [DF_Agent_IsAdministrator]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_IsAdministrator]  DEFAULT ((0)) FOR [IsAdministrator]
GO
/****** Object:  Default [DF_Agent_AllowManualDial]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_AllowManualDial]  DEFAULT ((0)) FOR [AllowManualDial]
GO
/****** Object:  Default [DF_Agent_VerificationAgent]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_VerificationAgent]  DEFAULT ((0)) FOR [VerificationAgent]
GO
/****** Object:  Default [DF_Agent_InBoundAgent]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_InBoundAgent]  DEFAULT ((0)) FOR [InBoundAgent]
GO
/****** Object:  Default [DF_Agent_PhoneNumber]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_PhoneNumber]  DEFAULT ((123456)) FOR [PhoneNumber]
GO
/****** Object:  Default [DF_Agent_IsDefault]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_IsDefault]  DEFAULT ((0)) FOR [IsDefault]
GO
/****** Object:  Default [DF_Agent_IsDeleted]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_Agent_DateCreated]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_Agent_DateModified]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_Agent_IsReset]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[Agent] ADD  CONSTRAINT [DF_Agent_IsReset]  DEFAULT ((0)) FOR [IsReset]
GO
/****** Object:  Default [DF_AgentCampaignMap_DateCreated]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentCampaignMap] ADD  CONSTRAINT [DF_AgentCampaignMap_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_AgentCampaignMap_DateModified]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentCampaignMap] ADD  CONSTRAINT [DF_AgentCampaignMap_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_AgentStation_IsDeleted]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentStation] ADD  CONSTRAINT [DF_AgentStation_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_AgentStation_AllwaysOffHook]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentStation] ADD  CONSTRAINT [DF_AgentStation_AllwaysOffHook]  DEFAULT ((0)) FOR [AllwaysOffHook]
GO
/****** Object:  Default [DF_LoginStatus_DateCreated]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentStatus] ADD  CONSTRAINT [DF_LoginStatus_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_LoginStatus_DateModified]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentStatus] ADD  CONSTRAINT [DF_LoginStatus_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_AgentActivity_IsDeleted]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentActivity] ADD  CONSTRAINT [DF_AgentActivity_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_AgentActivity_DateCreated]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentActivity] ADD  CONSTRAINT [DF_AgentActivity_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_AgentActivity_DateModified]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentActivity] ADD  CONSTRAINT [DF_AgentActivity_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_AgentActivity_StationIP]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentActivity] ADD  CONSTRAINT [DF_AgentActivity_StationIP]  DEFAULT ('') FOR [StationIP]
GO
/****** Object:  Default [DF_AgentActivity_StationHostName]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentActivity] ADD  CONSTRAINT [DF_AgentActivity_StationHostName]  DEFAULT ('') FOR [StationHostName]
GO
/****** Object:  Default [DF_AreaCodeRule_IsDeleted]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AreaCodeRule] ADD  CONSTRAINT [DF_AreaCodeRule_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]
GO
/****** Object:  Default [DF_AreaCodeRule_DateCreated]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AreaCodeRule] ADD  CONSTRAINT [DF_AreaCodeRule_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_AreaCodeRule_DateModified]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AreaCodeRule] ADD  CONSTRAINT [DF_AreaCodeRule_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  ForeignKey [FK_AgentActivity_Agent]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentActivity]  WITH CHECK ADD  CONSTRAINT [FK_AgentActivity_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[AgentActivity] CHECK CONSTRAINT [FK_AgentActivity_Agent]
GO
/****** Object:  ForeignKey [FK_AgentActivity_Campaign]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentActivity]  WITH CHECK ADD  CONSTRAINT [FK_AgentActivity_Campaign] FOREIGN KEY([CampaignID])
REFERENCES [dbo].[Campaign] ([CampaignID])
GO
ALTER TABLE [dbo].[AgentActivity] CHECK CONSTRAINT [FK_AgentActivity_Campaign]
GO
/****** Object:  ForeignKey [FK_AgentActivity_LoginStatus]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AgentActivity]  WITH CHECK ADD  CONSTRAINT [FK_AgentActivity_LoginStatus] FOREIGN KEY([AgentStatusID])
REFERENCES [dbo].[AgentStatus] ([AgentStatusID])
GO
ALTER TABLE [dbo].[AgentActivity] CHECK CONSTRAINT [FK_AgentActivity_LoginStatus]
GO
/****** Object:  ForeignKey [FK_AreaCodeRule_Agent]    Script Date: 07/25/2011 15:45:39 ******/
ALTER TABLE [dbo].[AreaCodeRule]  WITH CHECK ADD  CONSTRAINT [FK_AreaCodeRule_Agent] FOREIGN KEY([AgentID])
REFERENCES [dbo].[Agent] ([AgentID])
GO
ALTER TABLE [dbo].[AreaCodeRule] CHECK CONSTRAINT [FK_AreaCodeRule_Agent]
GO
