/****** Object:  Table [dbo].[QueryDetail]    Script Date: 01/21/2013 16:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[QueryDetail](
	[QueryDetailID] [bigint] IDENTITY(1,1) NOT NULL,
	[QueryID] [bigint] NOT NULL,
	[SearchCriteria] [varchar](100) NULL,
	[SearchOperator] [varchar](50) NULL,
	[SearchString] [varchar](500) NULL,
	[LogicalOperator] [nchar](10) NULL,
	[LogicalOrder] [smallint] NOT NULL,
	[SubQueryID] [bigint] NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[SubsetID] [int] NULL,
	[SubsetName] [varchar](100) NULL,
	[SubsetLevel] [int] NULL,
	[ParentSubsetID] [int] NULL,
	[TreeNodeID] [int] NULL,
	[ParentTreeNodeID] [int] NULL,
	[SubsetLogicalOrder] [int] NULL,
	[ElementText] [varchar](100) NULL,
	[isDefault] [bit] NOT NULL,
	
 CONSTRAINT [PK_QueryDetail] PRIMARY KEY CLUSTERED 
(
	[QueryDetailID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Query]    Script Date: 01/21/2013 16:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Query](
	[QueryID] [bigint] IDENTITY(1,1) NOT NULL,
	[QueryName] [varchar](255) NOT NULL,
	[QueryCondition] [text] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[isDefault] [bit] NOT NULL,
	
 CONSTRAINT [PK_Query] PRIMARY KEY CLUSTERED 
(
	[QueryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Query] ON [dbo].[Query] 
(
	[QueryName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Script]    Script Date: 01/21/2013 16:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Script](
	[ScriptID] [bigint] IDENTITY(1,1) NOT NULL,
	[ScriptName] [varchar](255) NOT NULL,
	[ScriptHeader] [text] NULL,
	[ScriptSubHeader] [text] NULL,
	[ScriptBody] [text] NULL,
	[ParentScriptID] [bigint] NULL,
	[ScriptGuid] [varchar](255) NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_Script] PRIMARY KEY CLUSTERED 
(
	[ScriptID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Script] ON [dbo].[Script] 
(
	[ScriptGuid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Script_1] ON [dbo].[Script] 
(
	[ScriptName] ASC,
	[ParentScriptID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ResultCode]    Script Date: 01/21/2013 16:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ResultCode](
	[ResultCodeID] [bigint] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](255) NOT NULL,
	[Presentation] [bit] NOT NULL,
	[Redialable] [bit] NOT NULL,
	[RecycleInDays] [smallint] NOT NULL,
	[Lead] [bit] NOT NULL,
	[MasterDNC] [bit] NOT NULL,
	[NeverCall] [bit] NOT NULL,
	[VerifyOnly] [bit] NOT NULL,
	[CountAsLiveContact] [bit] NOT NULL,
	[DialThroughAll] [bit] NOT NULL,
	[ShowDeletedResultCodes] [bit] NOT NULL,
	[DateDeleted] [datetime] NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_ResultCode] PRIMARY KEY CLUSTERED 
(
	[ResultCodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[OtherParameter]    Script Date: 01/21/2013 16:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OtherParameter](
	[OtherParameterID] [bigint] IDENTITY(1,1) NOT NULL,
	[CallTransfer] [smallint] NOT NULL,
	[StaticOffsiteNumber] [varchar](20) NULL,
	[TransferMessage] [bit] NOT NULL,
	[AutoPlayMessage] [varchar](255) NULL,
	[HoldMessage] [varchar](255) NULL,
	[AllowManualDial] [bit] NOT NULL,
	[StartingLine] [smallint] NULL,
	[EndingLine] [smallint] NULL,
	[AllowCallBacks] [smallint] NULL,
	[AlertSupervisorOnCallbacks] [smallint] NULL,
	[QueryStatisticsInPercent] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_OtherParameter] PRIMARY KEY CLUSTERED 
(
	[OtherParameterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DigitalizedRecording]    Script Date: 01/21/2013 16:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DigitalizedRecording](
	[DigitalizedRecordingID] [bigint] IDENTITY(1,1) NOT NULL,
	[EnableRecording] [bit] NOT NULL,
	[EnableWithABeep] [bit] NOT NULL,
	[StartWithABeep] [bit] NOT NULL,
	[RecordToWave] [bit] NOT NULL,
	[HighQualityRecording] [bit] NOT NULL,
	[RecordingFilePath] [varchar](255) NULL,
	[FileNaming] [varchar](255) NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
 CONSTRAINT [PK_DigitalizedRecording] PRIMARY KEY CLUSTERED 
(
	[DigitalizedRecordingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[DialingParameter]    Script Date: 01/21/2013 16:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[DialingParameter](
	[DailingParameterID] [bigint] IDENTITY(1,1) NOT NULL,
	[PhoneLineCount] [smallint] NOT NULL,
	[DropRatePercent] [smallint] NOT NULL,
	[RingSeconds] [smallint] NOT NULL,
	[MinimumDelayBetweenCalls] [smallint] NOT NULL,
	[DialingMode] [smallint] NOT NULL,
	[AnsweringMachineDetection] [bit] NOT NULL,
	[ColdCallScriptID] [bigint] NOT NULL,
	[VerificationScriptID] [bigint] NOT NULL,
	[InboundScriptID] [bigint] NOT NULL,
	[AMCallTimes] [smallint] NOT NULL,
	[PMCallTimes] [smallint] NOT NULL,
	[WeekendCallTimes] [smallint] NOT NULL,
	[AMDialingStart] [datetime] NOT NULL,
	[AMDialingStop] [datetime] NOT NULL,
	[PMDialingStart] [datetime] NOT NULL,
	[PMDialingStop] [datetime] NOT NULL,
	[AnsMachDetect] [smallint] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[ErrorRedialLapse] [int] NOT NULL,
	[BusyRedialLapse] [int] NOT NULL,
	[NoAnswerRedialLapse] [int] NOT NULL,
	[ChannelsPerAgent] [numeric](5, 2) NOT NULL,
	[DefaultCallLapse] [int] NOT NULL,
	[AnsweringMachineMessage] [varchar](1000) NULL,
	[HumanMessageEnable] [bit] NULL,
	[HumanMessage] [varchar](1000) NULL,
	[SilentCallMessageEnable] [bit] NULL,
	[SilentCallMessage] [varchar](1000) NULL,
	[SevenDigitPrefix] [varchar](20) NULL,
	[TenDigitPrefix] [varchar](20) NULL,
	[SevenDigitSuffix] [varchar](20) NULL,
	[TenDigitSuffix] [varchar](20) NULL,
 CONSTRAINT [PK_DialingParameter] PRIMARY KEY CLUSTERED 
(
	[DailingParameterID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Campaign]    Script Date: 01/21/2013 16:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Campaign](
	[UniqueKey] [bigint] IDENTITY(1,1) NOT NULL,
	[Campaign] [int] NULL,
	[PhoneNum] [varchar](10) NULL,
	[OffsiteTransferNumber] [varchar](50) NULL,
	[DBCompany] [varchar](255) NULL,
	[NeverCallFlag] [int] NULL,
	[AgentName] [varchar](50) NULL,
	[AgentID] [varchar](255) NULL,
	[VerificationAgentID] [varchar](255) NULL,
	[CallResultCode] [int] NULL,
	[DateTimeofCall] [datetime] NULL,
	[CallDuration] [varchar](255) NULL,
	[CallSenttoDialTime] [datetime] NULL,
	[CalltoAgentTime] [datetime] NULL,
	[CallHangupTime] [datetime] NULL,
	[CallCompletionTime] [datetime] NULL,
	[CallWrapUpStartTime] [datetime] NULL,
	[CallWrapUpStopTime] [datetime] NULL,
	[ResultCodeSetTime] [datetime] NULL,
	[TotalNumAttempts] [varchar](255) NULL,
	[NumAttemptsAM] [varchar](255) NULL,
	[NumAttemptsWkEnd] [varchar](255) NULL,
	[NumAttemptsPM] [varchar](255) NULL,
	[LeadProcessed] [varchar](255) NULL,
	[FIRSTNAME] [varchar](255) NULL,
	[LASTNAME] [varchar](255) NULL,
	[ADDRESS] [varchar](255) NULL,
	[CITY] [varchar](255) NULL,
	[STATE] [varchar](255) NULL,
	[ZIP] [varchar](255) NULL,
	[ADDRESS2] [varchar](255) NULL,
	[FullQueryPassCount] [int] NULL,
	[scheduledate] [datetime] NULL,
	[schedulenotes] [text] NULL,
	[isManualDial] [bit] NOT NULL,
	[DateTimeofImport] [datetime] NULL,
	[PledgeAmount] [money] NULL,
	
 CONSTRAINT [PK_Campaign] PRIMARY KEY CLUSTERED 
(
	[UniqueKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_Campaign_PhoneNum] ON [dbo].[Campaign] 
(
	[PhoneNum] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CampaignFields]    Script Date: 01/21/2013 16:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CampaignFields](
	[FieldID] [bigint] IDENTITY(1,1) NOT NULL,
	[FieldName] [varchar](50) NOT NULL,
	[FieldTypeID] [bigint] NOT NULL,
	[Value] [int] NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[IsDefault] [bit] NOT NULL,
	[ReadOnly] [bit] NULL,
 CONSTRAINT [PK_CampaignFields] PRIMARY KEY CLUSTERED 
(
	[FieldID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CampaignFields] ON [dbo].[CampaignFields] 
(
	[FieldName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Sel_AgentLogin_Dtl]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_AgentLogin_Dtl] 
 	@AgentID bigint 
AS 
BEGIN			
	SELECT
		AgentLoginID,
		AgentID, 
		LoginStatusID,
		DateCreated, 
		DateModified 
	FROM  
		dbo.AgentLogin
	WHERE 	
		AgentID = @AgentID
END
GO
/****** Object:  Table [dbo].[TrainingSchemes]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TrainingSchemes](
	[TrainingSchemeID] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[ScoreboardFrequency] [int] NULL,
	[ScoreboardDisplayTime] [int] NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[TrainingPages]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[TrainingPages](
	[TrainingPageID] [bigint] IDENTITY(1,1) NOT NULL,
	[TrainingSchemeID] [bigint] NOT NULL,
	[TrainingPageName] [varchar](255) NOT NULL,
	[TrainingPageContent] [text] NULL,
	[DisplayTime] [int] NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[IsActive] [bit] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SilentCallList]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SilentCallList](
	[SilentCallID] [bigint] IDENTITY(1,1) NOT NULL,
	[UniqueKey] [bigint] NOT NULL,
	[DateTimeofCall] [datetime] NOT NULL,
 CONSTRAINT [PK_SilentCallList] PRIMARY KEY CLUSTERED 
(
	[SilentCallID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_AgentLogin]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_AgentLogin]
		@AgentLoginID bigint, 
		@AgentID bigint, 
		@LoginStatusID bigint, 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 
	IF @AgentLoginID <= 0 
		BEGIN
			INSERT INTO dbo.AgentLogin (
				AgentID,
				LoginStatusID,
				DateCreated,
				DateModified
 				)
 			VALUES (
				@AgentID,
				@LoginStatusID,
				GETDATE(),
				GETDATE()
		 		)

		SET @AgentLoginID = @@IDENTITY 

		SELECT @AgentLoginID AS AgentLoginID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.AgentLogin 
			SET 
				AgentID = @AgentID,
				LoginStatusID = @LoginStatusID,
				DateModified = GETDATE()
			WHERE 	
				AgentLoginID = @AgentLoginID 	

			SELECT @AgentLoginID AS AgentLoginID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[SearchForStringInSPs]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
/****** Object:  Table [dbo].[GlobalDialingParameters]    Script Date: 01/21/2013 16:45:50 ******/
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
 CONSTRAINT [PK_GlobalDialingParameters] PRIMARY KEY CLUSTERED 
(
	[GlobalDialingID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  StoredProcedure [dbo].[Sel_Script_List_By_ParentScriptID]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_Script_List_By_ParentScriptID]  
	@ParentScriptID bigint	 
AS 
BEGIN
				
	SELECT
		ScriptID, 
		ScriptName, 
		ScriptHeader, 
		ScriptSubHeader, 
		ScriptBody, 
		DateCreated, 
		DateModified,
		ParentScriptID,		
		ScriptGuid
	FROM  
		dbo.script	
	WHERE ParentScriptID = @ParentScriptID
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_Script_List]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_Script_List]  	 
AS 
BEGIN
				
	SELECT
		ScriptID, 
		ScriptName, 
		ScriptHeader, 
		ScriptSubHeader, 
		ScriptBody, 
		DateCreated, 
		DateModified,
		ParentScriptID,		
		ScriptGuid
	FROM  
		dbo.script	
	WHERE ParentScriptID IS NULL
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_script_Dtl_ByGUID]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_script_Dtl_ByGUID] 
 	@ScriptGuid varchar(255) 
AS 
BEGIN
				
	SELECT
		ScriptID, 
		ScriptName, 
		ScriptHeader, 
		ScriptSubHeader, 
		ScriptBody, 
		DateCreated, 
		DateModified,
		ParentScriptID,		
		ScriptGuid
	FROM  
		dbo.script 
	WHERE 	
		ScriptGuid = @ScriptGuid
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_script_Dtl]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_script_Dtl]
 	@ScriptID bigint 
AS 
BEGIN
				
	SELECT
		PAGE.ScriptID, 
		PAGE.ScriptName, 
		PAGE.ScriptHeader, 
		PAGE.ScriptSubHeader, 
		PAGE.ScriptBody, 
		PAGE.DateCreated, 
		PAGE.DateModified,
		PAGE.ParentScriptID,
		PARENT.ScriptName as ParentScriptName,
		PAGE.ScriptGuid
	FROM  
		dbo.script PAGE
	LEFT OUTER JOIN
		dbo.script PARENT  ON PAGE.ParentScriptID = PARENT.ScriptID
	WHERE 	
		PAGE.ScriptID = @ScriptID
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_ResultCode_List]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_ResultCode_List] 
 	 
AS 
BEGIN	
	SELECT
		ResultCodeID, 
		Description, 
		Presentation, 
		Redialable, 
		RecycleInDays, 
		Lead, 
		MasterDNC, 
		NeverCall, 
		VerifyOnly, 
		CountAsLiveContact, 
		DialThroughAll, 
		ShowDeletedResultCodes, 
		DateDeleted, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.ResultCode					
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_ResultCode_Dtl]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_ResultCode_Dtl] 
 	@ResultCodeID numeric(18,0) 
AS 
BEGIN
				
	SELECT
		ResultCodeID, 
		Description, 
		Presentation, 
		Redialable, 
		RecycleInDays, 
		Lead, 
		MasterDNC, 
		NeverCall, 
		VerifyOnly, 
		CountAsLiveContact,  
		DialThroughAll, 
		ShowDeletedResultCodes, 
		DateDeleted, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.ResultCode
	WHERE 	
		ResultCodeID = @ResultCodeID
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_QueryDetail_List]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_QueryDetail_List] 
 	 
AS 
BEGIN
				
	SELECT
		QueryDetailID, 
		QueryID, 
		SearchCriteria, 
		SearchOperator, 
		SearchString, 
		LogicalOperator, 
		LogicalOrder,
		SubQueryID, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.QueryDetail
	
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_QueryDetail_Dtl]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_QueryDetail_Dtl] 
 	@QueryDetailID numeric(18,0) 
AS 
BEGIN
				
	SELECT
		QueryDetailID, 
		QueryID, 
		SearchCriteria, 
		SearchOperator, 
		SearchString, 
		LogicalOperator, 
		LogicalOrder,
		SubQueryID, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.QueryDetail
	WHERE 	
		QueryDetailID = @QueryDetailID
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_QueryDetail_ByQueryID]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_QueryDetail_ByQueryID]
 	@QueryID BIGINT
AS 
BEGIN
				
	SELECT	QD.QueryDetailID, 
		QD.QueryID, 
		Q.QueryName,
		Q.QueryCondition,
		Q.DateCreated,
		Q.DateModified,
		QD.SearchCriteria, 
		QD.SearchOperator, 
		QD.SearchString, 
		QD.LogicalOperator, 
		QD.LogicalOrder,
		QD.SubQueryID, 
		QD.DateCreated, 
		QD.DateModified,
		QD.SubsetID,
		QD.SubsetName,
		QD.SubsetLevel,
		QD.ParentSubsetID,
		QD.SubsetLogicalOrder,
		QD.TreeNodeID,
		QD.ParentTreeNodeID,
		QD.ElementText 
	FROM  
		dbo.QueryDetail QD WITH (NOLOCK)
	INNER JOIN
		dbo.Query Q WITH (NOLOCK) ON Q.QueryID = QD.QueryID
	WHERE 	
		QD.QueryID = @QueryID
	ORDER BY
		QD.LogicalOrder ASC,
		QD.SubsetID ASC,
		QD.SubsetLogicalOrder ASC
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_Query_List]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_Query_List] 
 	 
AS 
BEGIN
				
	SELECT
		QueryID, 
		QueryName, 
		QueryCondition, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.Query
	ORDER BY
		QueryName ASC

END
GO
/****** Object:  StoredProcedure [dbo].[Sel_Query_Dtl]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_Query_Dtl] 
 	@QueryID numeric(18,0) 
AS 
BEGIN
				
	SELECT
		QueryID, 
		QueryName, 
		QueryCondition, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.Query
	WHERE 	
		QueryID = @QueryID
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_OtherParameter_List]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_OtherParameter_List] 
 	 
AS 
BEGIN
				
	SELECT
		OtherParameterID, 
		CallTransfer, 
		StaticOffsiteNumber, 
		TransferMessage, 
		AutoPlayMessage, 
		HoldMessage, 
		AllowManualDial, 
		StartingLine, 
		EndingLine, 
		AllowCallBacks, 
		AlertSupervisorOnCallbacks, 
		QueryStatisticsInPercent, 
		DateCreated, 
		DateModified		
	FROM  
		dbo.OtherParameter
	
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_OtherParameter_Dtl]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_OtherParameter_Dtl] 
 	@OtherParameterID BIGINT
AS 
BEGIN
				
	SELECT
		OtherParameterID, 
		CallTransfer, 
		StaticOffsiteNumber, 
		TransferMessage, 
		AutoPlayMessage, 
		HoldMessage, 
		AllowManualDial, 
		StartingLine, 
		EndingLine, 
		AllowCallBacks, 
		AlertSupervisorOnCallbacks, 
		QueryStatisticsInPercent, 
		DateCreated, 
		DateModified		
	FROM  
		dbo.OtherParameter
	WHERE 	
		OtherParameterID = @OtherParameterID
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_OffsiteTransferNumber]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_OffsiteTransferNumber]
	@UniqueKey bigint, 
	@PhoneNumber varchar(20)
AS
Select Campaign.OffsiteTransferNumber FROM Campaign 
	WHERE Campaign.PhoneNum = @PhoneNumber AND Campaign.UniqueKey = @UniqueKey
GO
/****** Object:  StoredProcedure [dbo].[Sel_DigitalizedRecording_List]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_DigitalizedRecording_List] 
 	 
AS 
BEGIN
				
	SELECT
		DigitalizedRecordingID, 
		EnableRecording, 
		EnableWithABeep, 
		StartWithABeep, 
		RecordToWave, 
		HighQualityRecording, 
		RecordingFilePath, 
		FileNaming, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.DigitalizedRecording
	
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_DigitalizedRecording_Dtl]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_DigitalizedRecording_Dtl] 
 	@DigitalizedRecordingID numeric(18,0) 
AS 
BEGIN
				
	SELECT
		DigitalizedRecordingID, 
		EnableRecording, 
		EnableWithABeep, 
		StartWithABeep, 
		RecordToWave, 
		HighQualityRecording, 
		RecordingFilePath, 
		FileNaming, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.DigitalizedRecording
	WHERE 	
		DigitalizedRecordingID = @DigitalizedRecordingID
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_DialingParameter_List]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_DialingParameter_List] 
AS 
BEGIN
				
	SELECT
		DailingParameterID, 
		PhoneLineCount, 
		DropRatePercent, 
		RingSeconds, 
		MinimumDelayBetweenCalls, 
		DialingMode, 
		AnsweringMachineDetection, 
		SevenDigitPrefix,
		TenDigitPrefix,
		SevenDigitSuffix,
		TenDigitSuffix,
		ColdCallScriptID, 
		VerificationScriptID, 
		InboundScriptID, 
		AMCallTimes, 
		PMCallTimes, 
		WeekendCallTimes, 
		AMDialingStart,
		AMDialingStop, 
		PMDialingStart, 
		PMDialingStop, 
		AnsMachDetect,
		DateCreated, 
		DateModified,
		ErrorRedialLapse, 
		BusyRedialLapse,
		NoAnswerRedialLapse,
		ChannelsPerAgent,
		DefaultCallLapse,
		AnsweringMachineMessage,
		HumanMessageEnable,
		HumanMessage,
		SilentCallMessageEnable,
		SilentCallMessage
	FROM  
		dbo.DialingParameter
	
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_DialingParameter_Dtl]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_DialingParameter_Dtl] 
 	@DailingParameterID bigint 
AS 
BEGIN
				
	SELECT
		DailingParameterID, 
		PhoneLineCount, 
		DropRatePercent, 
		RingSeconds, 
		MinimumDelayBetweenCalls, 
		DialingMode, 
		AnsweringMachineDetection, 
		SevenDigitPrefix,
		TenDigitPrefix,
		SevenDigitSuffix,
		TenDigitSuffix,
		ColdCallScriptID, 
		VerificationScriptID, 
		InboundScriptID, 
		AMCallTimes, 
		PMCallTimes, 
		WeekendCallTimes, 
		AMDialingStart,
		AMDialingStop, 
		PMDialingStart, 
		PMDialingStop, 
		AnsMachDetect,
		DateCreated, 
		DateModified,
		ErrorRedialLapse, 
		BusyRedialLapse,
		NoAnswerRedialLapse,
		ChannelsPerAgent,
		DefaultCallLapse,
		AnsweringMachineMessage,
		HumanMessageEnable,
		HumanMessage,
		SilentCallMessageEnable,
		SilentCallMessage
	FROM  
		dbo.DialingParameter
	WHERE 	
		DailingParameterID = @DailingParameterID
		
END
GO
/****** Object:  StoredProcedure [dbo].[Del_CampaignField]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Del_CampaignField]
	@FieldID bigint
AS
BEGIN
	DELETE FROM CampaignFields WHERE FieldID = @FieldID
END
GO
/****** Object:  StoredProcedure [dbo].[Del_CampaignColumn]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Del_CampaignColumn]
	@FieldID bigint
AS
BEGIN
	DECLARE @alterSQL VARCHAR(1000)	

	SET @alterSQL = (SELECT FieldName FROM CampaignFields WHERE FieldID = @FieldID)
	IF NOT @alterSQL IS NULL and @alterSQL != ''
	BEGIN
		SET @alterSQL = 'ALTER TABLE dbo.Campaign DROP COLUMN ' + @alterSQL
		EXEC (@alterSQL)
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Get_CampaignTransferCall]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_CampaignTransferCall]
	@UniqueKey bigint
AS
SELECT VerificationAgentID From CAMPAIGN
	WHERE UniqueKey = @UniqueKey
GO
/****** Object:  StoredProcedure [dbo].[Get_CampaignManualDial]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_CampaignManualDial]
	@AgentID bigint	
AS
BEGIN
	SELECT 
		UniqueKey, 		
		PhoneNum,		
		AgentID		
	FROM 
		CAMPAIGN 
	WHERE 
		CallResultCode IS NULL 
	AND
		DateTimeofCall IS NULL
	AND 
		IsManualDial = 1 
	AND 
		AgentID = @AgentID
END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_CampaignFields]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_CampaignFields]  
	@FieldID bigint,	
	@FieldName varchar(50),
	@FieldTypeID bigint,
	@IsDefault bit,		
	@Value int
AS 
BEGIN	
	--There will be no update 
	INSERT INTO CampaignFields(			
		FieldName,
		FieldTypeID,		
		Value,
		DateCreated,
		DateModified,
		IsDefault,
		ReadOnly)
	VALUES(			
		@FieldName,
		@FieldTypeID,		
		@Value,
		GetDate(),
		GetDate(),
		@IsDefault,
		0)	

	SET @FieldID = @@IDENTITY		
	SELECT @FieldID as FieldID
	
END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_Campaign]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_Campaign] 
		@UniqueKey bigint , 
		@Campaign int , 
		@PhoneNum varchar (20), 
		@DBCompany varchar (255), 
		@NeverCallFlag int , 
		@AgentID varchar (255), 
		@VerificationAgentID varchar (255), 
		@CallResultCode int , 
		@DateTimeofCall datetime , 
		@CallDuration varchar (255), 
		@CallSenttoDialTime datetime, 
		@CalltoAgentTime datetime, 
		@CallHangupTime datetime, 
		@CallCompletionTime datetime, 
		@CallWrapUpStartTime datetime, 
		@CallWrapUpStopTime datetime, 
		@ResultCodeSetTime datetime, 
		@TotalNumAttempts varchar (255), 
		@NumAttemptsAM varchar (255), 
		@NumAttemptsWkEnd varchar (255), 
		@NumAttemptsPM varchar (255), 
		@LeadProcessed varchar (255), 
		@FIRSTNAME varchar (255),
		@LASTNAME varchar (255), 
		@ADDRESS varchar (255), 
		@CITY varchar (255), 
		@STATE varchar (255), 
		@ZIP varchar (255), 
		@ADDRESS2 varchar (255), 
		--@COUNTRY varchar (255), 
		@FullQueryPassCount int , 
		--@APCR varchar (255), 
		--@APCRAgent varchar (255), 
		--@APCRDT varchar (255), 
		--@APCRMemo varchar (255), 
		--@APCR2 varchar (255), 
		--@APCRAgent2 varchar (255), 
		--@APCRDT2 varchar (255), 
		--@APCRMemo2 varchar (255), 
		--@APCR3 varchar (255), 
		--@APCRAgent3 varchar (255), 
		--@APCRDT3 varchar (255), 
		--@APCRMemo3 varchar (255), 
		--@APCR4 varchar (255), 
		--@APCRAgent4 varchar (255), 
		--@APCRDT4 varchar (255), 
		--@APCRMemo4 varchar (255), 
		--@APCR5 varchar (255), 
		--@APCRAgent5 varchar (255), 
		--@APCRDT5 varchar (255), 
		--@APCRMemo5 varchar (255), 
		--@APCR6 varchar (255), 
		--@APCRAgent6 varchar (255), 
		--@APCRDT6 varchar (255), 
		--@APCRMemo6 varchar (255),
		@AllowDuplicates bit = 1
AS
BEGIN

Declare @DupUniqueKey BIGINT
IF @AllowDuplicates = 0
BEGIN
	SET @DupUniqueKey = (select Top 1 UniqueKey from Campaign where PhoneNum = @PhoneNum)
END
SET @DupUniqueKey = ISNULL(@DupUniqueKey, 0) 
IF @DupUniqueKey <= 0
BEGIN
	IF @UniqueKey <= 0 
		BEGIN
			INSERT INTO dbo.Campaign (
				Campaign,
				PhoneNum,
				DBCompany,
				NeverCallFlag,
				AgentID,
				VerificationAgentID,
				CallResultCode,
				DateTimeofCall,
				CallDuration,
				CallSenttoDialTime,
				CalltoAgentTime,
				CallHangupTime,
				CallCompletionTime,
				CallWrapUpStartTime,
				CallWrapUpStopTime,
				ResultCodeSetTime,
				TotalNumAttempts,
				NumAttemptsAM,
				NumAttemptsWkEnd,
				NumAttemptsPM,
				LeadProcessed,
				FIRSTNAME,
				LASTNAME,
				ADDRESS,
				CITY,
				STATE,
				ZIP,
				ADDRESS2,
				--COUNTRY,
				FullQueryPassCount
				--APCR,
				--APCRAgent,
				--APCRDT,
				--APCRMemo,
				--APCR2,
				--APCRAgent2,
				--APCRDT2,
				--APCRMemo2,
				--APCR3,
				--APCRAgent3,
				--APCRDT3,
				--APCRMemo3,
				--APCR4,
				--APCRAgent4,
				--APCRDT4,
				--APCRMemo4,
				--APCR5,
				--APCRAgent5,
				--APCRDT5,
				--APCRMemo5,
				--APCR6,
				--APCRAgent6,
				--APCRDT6,
				--APCRMemo6
 				)
 			VALUES (
				@Campaign,
				@PhoneNum,
				@DBCompany,
				@NeverCallFlag,
				@AgentID,
				@VerificationAgentID,
				@CallResultCode,
				@DateTimeofCall,
				@CallDuration,
				@CallSenttoDialTime,
				@CalltoAgentTime,
				@CallHangupTime,
				@CallCompletionTime,
				@CallWrapUpStartTime,
				@CallWrapUpStopTime,
				@ResultCodeSetTime,
				@TotalNumAttempts,
				@NumAttemptsAM,
				@NumAttemptsWkEnd,
				@NumAttemptsPM,
				@LeadProcessed,
				@FIRSTNAME,
				@LASTNAME,
				@ADDRESS,
				@CITY,
				@STATE,
				@ZIP,
				@ADDRESS2,
				--@COUNTRY,
				@FullQueryPassCount
				--@APCR,
				--@APCRAgent,
				--@APCRDT,
				--@APCRMemo,
				--@APCR2,
				--@APCRAgent2,
				--@APCRDT2,
				--@APCRMemo2,
				--@APCR3,
				--@APCRAgent3,
				--@APCRDT3,
				--@APCRMemo3,
				--@APCR4,
				--@APCRAgent4,
				--@APCRDT4,
				--@APCRMemo4,
				--@APCR5,
				--@APCRAgent5,
				--@APCRDT5,
				--@APCRMemo5,
				--@APCR6,
				--@APCRAgent6,
				--@APCRDT6,
				--@APCRMemo6
				)
				
			SET @UniqueKey = @@IDENTITY 

			SELECT @UniqueKey AS UniqueKey
		END
		ELSE
		BEGIN
 
			UPDATE dbo.Campaign 
			SET				
				Campaign = @Campaign,
				PhoneNum = @PhoneNum,
				DBCompany = @DBCompany,
				NeverCallFlag = @NeverCallFlag,
				AgentID = @AgentID,
				VerificationAgentID = @VerificationAgentID,
				CallResultCode = @CallResultCode,
				DateTimeofCall = @DateTimeofCall,
				CallDuration = @CallDuration,
				CallSenttoDialTime = @CallSenttoDialTime,
				CalltoAgentTime = @CalltoAgentTime,
				CallHangupTime = @CallHangupTime,
				CallCompletionTime = @CallCompletionTime,
				CallWrapUpStartTime = @CallWrapUpStartTime,
				CallWrapUpStopTime = @CallWrapUpStopTime,
				ResultCodeSetTime = @ResultCodeSetTime,
				TotalNumAttempts = @TotalNumAttempts,
				NumAttemptsAM = @NumAttemptsAM,
				NumAttemptsWkEnd = @NumAttemptsWkEnd,
				NumAttemptsPM = @NumAttemptsPM,
				LeadProcessed = @LeadProcessed,
				FIRSTNAME = @FIRSTNAME,
				LASTNAME = @LASTNAME,
				ADDRESS = @ADDRESS,
				CITY = @CITY,
				STATE = @STATE,
				ZIP = @ZIP,
				ADDRESS2 = @ADDRESS2,
				--COUNTRY = @COUNTRY,
				FullQueryPassCount = @FullQueryPassCount
				--APCR = @APCR,
				--APCRAgent = @APCRAgent,
				--APCRDT = @APCRDT,
				--APCRMemo = @APCRMemo,
				--APCR2 = @APCR2,
				--APCRAgent2 = @APCRAgent2,
				--APCRDT2 = @APCRDT2,
				--APCRMemo2 = @APCRMemo2,
				--APCR3 = @APCR3,
				--APCRAgent3 = @APCRAgent3,
				--APCRDT3 = @APCRDT3,
				--APCRMemo3 = @APCRMemo3,
				--APCR4 = @APCR4,
				--APCRAgent4 = @APCRAgent4,
				--APCRDT4 = @APCRDT4,
				--APCRMemo4 = @APCRMemo4,
				--APCR5 = @APCR5,
				--APCRAgent5 = @APCRAgent5,
				--APCRDT5 = @APCRDT5,
				--APCRMemo5 = @APCRMemo5,
				--APCR6 = @APCR6,
				--APCRAgent6 = @APCRAgent6,
				--APCRDT6 = @APCRDT6,
				--APCRMemo6 = @APCRMemo6
			WHERE UniqueKey = @UniqueKey
			
			SELECT @UniqueKey AS UniqueKey
		END
	END
	ELSE
	BEGIN
		Select @DupUniqueKey as UniqueKey
	END
END
GO
/****** Object:  StoredProcedure [dbo].[ScrubDNC]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ScrubDNC] 
		
	
AS

	BEGIN

	UPDATE Campaign 
		SET 
			NeverCallFlag = 2  
		WHERE 
			Campaign.PhoneNum
				IN
					(SELECT RainmakerMaster.dbo.MasterDNC.PhoneNum 
						FROM RainmakerMaster.dbo.MasterDNC);

	END
GO
/****** Object:  StoredProcedure [dbo].[Ins_TrainingScheme]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Ins_TrainingScheme]
	@TrainingSchemeName varchar(255) 
AS 
BEGIN
	DECLARE @TrainingSchemeID bigint 

	INSERT INTO TrainingSchemes (Name, ScoreboardFrequency, ScoreboardDisplayTime, DateCreated, DateModified, IsActive) VALUES (@TrainingSchemeName, 5, 15, GETDATE(), GETDATE(), 1)
	
	SET @TrainingSchemeID = (SELECT TrainingSchemeID FROM TrainingSchemes WHERE Name = @TrainingSchemeName) 
	
	UPDATE TrainingSchemes SET IsActive=0 WHERE TrainingSchemeID <> @TrainingSchemeID
	
	SELECT
		@TrainingSchemeID AS TrainingSchemeID
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_TrainingScheme_List]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_TrainingScheme_List]
AS 
BEGIN
	SELECT
		TrainingSchemeID,
		Name,
		ScoreboardFrequency,
		ScoreboardDisplayTime,
		IsActive
	FROM  
		dbo.TrainingSchemes
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_TrainingScheme_Dtl]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_TrainingScheme_Dtl]
	@TrainingSchemeID bigint 
AS 
BEGIN
	DECLARE @PageCount int 
	SET @PageCount = (SELECT COUNT(TrainingPageID) FROM TrainingPages WHERE TrainingSchemeID = @TrainingSchemeID)

	SELECT
		@PageCount AS PageCount,
		Name,
		ScoreboardFrequency,
		ScoreboardDisplayTime,
		IsActive
	FROM  
		dbo.TrainingSchemes
	WHERE 	
		TrainingSchemeID = @TrainingSchemeID
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_TrainingPageActive_List]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_TrainingPageActive_List]
AS 
BEGIN
	SELECT
		TrainingPages.TrainingPageID AS TrainingPageID,
		TrainingPages.TrainingPageName AS TrainingPageName,
		TrainingPages.TrainingPageContent AS TrainingPageContent,
		TrainingPages.DisplayTime AS DisplayTime,
		TrainingPages.DateCreated AS DateCreated
	FROM 
		dbo.TrainingPages
	INNER JOIN dbo.TrainingSchemes ON TrainingPages.TrainingSchemeID = TrainingSchemes.TrainingSchemeID
	WHERE 	
		TrainingSchemes.IsActive = 1
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_TrainingPage_List]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_TrainingPage_List]
	@TrainingSchemeID bigint 
AS 
BEGIN
	SELECT
		TrainingPageID,
		TrainingPageName,
		TrainingPageContent,
		DisplayTime,
		DateCreated,
		DateModified,
		IsActive
	FROM  
		dbo.TrainingPages
	WHERE 	
		TrainingSchemeID = @TrainingSchemeID
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_TrainingPage_Dtl]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_TrainingPage_Dtl]
	@TrainingPageID bigint 
AS 
BEGIN
	SELECT
		TrainingPageID,
		TrainingPageName,
		TrainingPageContent,
		DisplayTime,
		DateCreated,
		DateModified,
		IsActive
	FROM  
		dbo.TrainingPages
	WHERE 	
		TrainingPageID = @TrainingPageID
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_AgentScript]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_AgentScript]
	@IsVericationScript bit = 0
AS 
BEGIN
	Declare @ScriptID bigint
	IF @IsVericationScript = 1 BEGIN
		Set @ScriptId=(SELECT VerificationScriptID  FROM  dbo.DialingParameter)
	END
	ELSE
	BEGIN
		Set @ScriptId=(SELECT ColdCallScriptID  FROM  dbo.DialingParameter)
	END
	IF @ScriptID >0
	BEGIN
		SELECT
			ScriptID, 
			ScriptName, 
			ScriptHeader, 
			ScriptSubHeader, 
			ScriptBody, 
			DateCreated, 
			DateModified 
		FROM  
			dbo.script
		WHERE 	
			ScriptID = @ScriptID
	END 	
END
GO
/****** Object:  StoredProcedure [dbo].[Get_Campaign_HangupTime]    Script Date: 01/21/2013 16:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_Campaign_HangupTime]
	@UniqueKey bigint
AS
SELECT CallHangupTime From CAMPAIGN
	WHERE UniqueKey = @UniqueKey
GO
/****** Object:  StoredProcedure [dbo].[Sel_Campaign_By_AgentID]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_Campaign_By_AgentID] 
	@AgentID bigint
AS
Select UniqueKey,
	Campaign,
	PhoneNum,
	DBCompany,
	NeverCallFlag,
	AgentID,
	VerificationAgentID,
	CallResultCode,
	DateTimeofCall,
	CallDuration,
	CallSenttoDialTime,
	CalltoAgentTime,
	CallHangupTime,
	CallCompletionTime,
	CallWrapUpStartTime,
	CallWrapUpStopTime,
	ResultCodeSetTime,
	TotalNumAttempts,
	NumAttemptsAM,
	NumAttemptsWkEnd,
	NumAttemptsPM,
	LeadProcessed,
	FIRSTNAME,
	LASTNAME,
	ADDRESS,
	CITY,
	STATE,
	ZIP,
	ADDRESS2,
	--COUNTRY,
	FullQueryPassCount
	--APCR,
	--APCRAgent,
	--APCRDT,
	--APCRMemo,
	--APCR2,
	--APCRAgent2,
	--APCRDT2,
	--APCRMemo2,
	--APCR3,
	--APCRAgent3,
	--APCRDT3,
	--APCRMemo3,
	--APCR4,
	--APCRAgent4,
	--APCRDT4,
	--APCRMemo4,
	--APCR5,
	--APCRAgent5,
	--APCRDT5,
	--APCRMemo5,
	--APCR6,
	--APCRAgent6,
	--APCRDT6,
	--APCRMemo6
FROM Campaign WHERE AgentID = @AgentID and CallResultCode IS NULL 
ORDER BY DateTimeofCall DESC
GO
/****** Object:  StoredProcedure [dbo].[Sel_CampaignFields_List]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_CampaignFields_List]  	 
AS 
BEGIN
				
	SELECT
		cf.FieldID,
		cf.FieldName,
		cf.FieldTypeID,
		ft.FieldType,
		cf.Value,
		cf.DateCreated,
		cf.DateModified,
		cf.IsDefault,
		cf.ReadOnly
	FROM  
		dbo.CampaignFields cf
	INNER JOIN 
		RainmakerMaster.dbo.FieldTypes ft ON cf.FieldTypeID = ft.FieldTypeID
	 
	
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_CampaignFields_Dtl]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_CampaignFields_Dtl]  
	@FieldID bigint	 
AS 
BEGIN				
	SELECT
		cf.FieldID,
		cf.FieldName,
		cf.FieldTypeID,
		ft.FieldType,
		cf.Value,
		cf.DateCreated,
		cf.DateModified,
		cf.ReadOnly
	FROM  
		dbo.CampaignFields cf
	INNER JOIN 
		RainmakerMaster.dbo.FieldTypes ft ON cf.FieldTypeID = ft.FieldTypeID 
	WHERE cf.FieldID = @FieldID 
	
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_CampaignDetails_By_PhoneNumber]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_CampaignDetails_By_PhoneNumber] 
	@PhoneNumber varchar(20)
AS
Select Campaign.* FROM Campaign 
	WHERE Campaign.PhoneNum = @PhoneNumber
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignHangup]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_CampaignHangup]
	@UniqueKey bigint
AS
UPDATE CAMPAIGN SET
	CallHangupTime = ISNULL(CallHangupTime, convert(varchar(23),GetDate(),121))
WHERE UniqueKey = @UniqueKey
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignTransferCall]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_CampaignTransferCall]
	@Uniquekey bigint,
	@OffsiteNumber varchar(50)
AS
UPDATE 
	Campaign 
SET
	VerificationAgentID='-1',
	OffsiteTransferNumber = @OffsiteNumber
WHERE
	Uniquekey = @Uniquekey
GO
/****** Object:  StoredProcedure [dbo].[Upd_AgentScript]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Upd_AgentScript] 
		@AgentID bigint,
		@CallResultCode int,
		@ResultCodeSetTime varchar(255)
	 
AS 
		BEGIN
			UPDATE dbo.Campaign 
			SET 
				CallResultCode = @CallResultCode,
				ResultCodeSetTime=@ResultCodeSetTime
			WHERE 	
				AgentID=@AgentID	
			
		END
GO
/****** Object:  StoredProcedure [dbo].[Upd_ActiveTrainingScheme]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_ActiveTrainingScheme]
	@TrainingSchemeID bigint 
AS 
BEGIN
	IF @TrainingSchemeID  < 0 
		BEGIN
			UPDATE TrainingSchemes SET IsActive=1
		END
	ELSE
		BEGIN
			IF @TrainingSchemeID > 0 
				BEGIN
					UPDATE TrainingSchemes SET IsActive=1 WHERE TrainingSchemeID = @TrainingSchemeID
					UPDATE TrainingSchemes SET IsActive=0 WHERE TrainingSchemeID <> @TrainingSchemeID
				END
			ELSE
				BEGIN
					UPDATE TrainingSchemes SET IsActive=0
				END
		END
END
SET ANSI_NULLS ON
GO
/****** Object:  Table [dbo].[CampaignQueryStatus]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CampaignQueryStatus](
	[CampaignQueryID] [bigint] IDENTITY(1,1) NOT NULL,
	[QueryID] [bigint] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IsStandby] [bit] NULL,
	[Total] [int] NOT NULL,
	[Available] [int] NOT NULL,
	[Dials] [int] NOT NULL,
	[Talks] [int] NOT NULL,
	[AnsweringMachine] [int] NOT NULL,
	[NoAnswer] [int] NOT NULL,
	[Busy] [int] NOT NULL,
	[OpInt] [int] NOT NULL,
	[Drops] [int] NOT NULL,
	[Failed] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[IsCurrent] [bit] NOT NULL,
	[ShowMessage] [bit] NOT NULL,
	[Priority] [int] NULL,
	[isDefault] [bit] NOT NULL,
	
 CONSTRAINT [PK_CampaignQueryStatus] PRIMARY KEY CLUSTERED 
(
	[CampaignQueryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CampaignQueryStatus] ON [dbo].[CampaignQueryStatus] 
(
	[QueryID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CampaignQueryStatus_1] ON [dbo].[CampaignQueryStatus] 
(
	[IsActive] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[AgentStat]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AgentStat](
	[StatID] [bigint] IDENTITY(1,1) NOT NULL,
	[AgentID] [bigint] NULL,
	[AgentName] [varchar](50) NULL,
	[StatusID] [bigint] NULL,
	[LeadsSales] [int] NULL,
	[Presentations] [int] NULL,
	[Calls] [int] NULL,
	[LeadSalesRatio] [numeric](5, 2) NULL,
	[PledgeAmount] [money] NULL,
	[TalkTime] [numeric](7, 2) NULL,
	[WaitingTime] [numeric](7, 2) NULL,
	[PauseTime] [numeric](7, 2) NULL,
	[WrapTime] [numeric](18, 2) NULL,
	[LoginDate] [datetime] NULL,
	[LoginTime] [datetime] NULL,
	[LogOffDate] [datetime] NULL,
	[LogOffTime] [datetime] NULL,
	[LastResultCodeID] [bigint] NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[TimeModified] [datetime] NOT NULL,
 CONSTRAINT [PK_AgentStat] PRIMARY KEY CLUSTERED 
(
	[StatID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[CallList]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[CallList](
	[CallListID] [bigint] IDENTITY(1,1) NOT NULL,
	[AgentID] [bigint] NOT NULL,
	[AgentName] [varchar](50) NULL,
	[ResultCodeID] [bigint] NOT NULL,
	[VerificationAgentID] [bigint] NULL,
	[PhoneNumber] [varchar](20) NOT NULL,
	[OffsiteTransferNumber] [varchar](50) NULL,
	[CallDate] [datetime] NOT NULL,
	[CallTime] [datetime] NOT NULL,
	[CallDuration] [numeric](5, 2) NOT NULL,
	[CallCompletionTime] [datetime] NOT NULL,
	[CallWrapTime] [datetime] NOT NULL,
	[IsBlocked] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL,
	[DateModified] [datetime] NOT NULL,
	[UniqueKey] [bigint] NOT NULL,
	[QueryID] [bigint] NOT NULL,
	[IsManualDial] [bit] NOT NULL,
 CONSTRAINT [PK_CallList] PRIMARY KEY CLUSTERED 
(
	[CallListID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_CallList] ON [dbo].[CallList] 
(
	[ResultCodeID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_CallList_1] ON [dbo].[CallList] 
(
	[AgentID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[Del_TrainingScheme]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Del_TrainingScheme] 
 	@TrainingSchemeID bigint 
AS 
BEGIN

	DELETE FROM dbo.TrainingSchemes
	WHERE
	TrainingSchemeID = @TrainingSchemeID	

	DELETE FROM dbo.TrainingPages
	WHERE
	TrainingSchemeID = @TrainingSchemeID	
		
END
GO
/****** Object:  StoredProcedure [dbo].[Del_Script]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Del_Script]
	@ScriptID bigint
AS
BEGIN
	DECLARE @ScriptCount INT

	SET @ScriptCount = (SELECT COUNT(DailingParameterID) 
		FROM 
			DialingParameter
		WHERE 
			InboundScriptID = @ScriptID 
		OR 
			VerificationScriptID = @ScriptID 
		OR 
			ColdCallScriptID = @ScriptID)

	IF @ScriptCount > 0
	BEGIN
		SELECT 'Script In Use'
		--RAISERROR('Script In Use',10,1)
	END
	ELSE
	BEGIN
		Delete From Script Where ParentScriptID = @ScriptID
		Delete From Script Where ScriptID = @ScriptID
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Del_QueryDetail]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[Del_QueryDetail] 

 	@QueryDetailID numeric(18,0) 

AS 

BEGIN


	DELETE FROM dbo.QueryDetail

	WHERE

	QueryDetailID = @QueryDetailID	

		

END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_TrainingPage]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_TrainingPage] 
		@TrainingPageID bigint , 
		@TrainingPageName varchar (255), 
		@TrainingPageContent text , 
		@TrainingSchemeID bigint,
		@DisplayTime int,
		@DateCreated datetime, 
		@DateModified datetime  
AS 
	BEGIN 
	IF @TrainingPageID <= 0 
		BEGIN
			INSERT INTO dbo.TrainingPages (
				TrainingPageName,
				TrainingPageContent,
				DateCreated,
				DateModified,
				DisplayTime,
				TrainingSchemeID
 				)
 			VALUES (
				@TrainingPageName,
				@TrainingPageContent,
				GETDATE(),
				GETDATE(),
				@DisplayTime,
				@TrainingSchemeID
		 		)

		SET @TrainingPageID = @@IDENTITY 

		SELECT @TrainingPageID AS TrainingPageID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.TrainingPages 
			SET
				TrainingPageName = @TrainingPageName,
				TrainingPageContent = @TrainingPageContent,
				DateModified = GETDATE(),
				DisplayTime = @DisplayTime,
				TrainingSchemeID = TrainingSchemeID
			WHERE 	
				TrainingPageID = @TrainingPageID	

			SELECT @TrainingPageID AS TrainingPageID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_SilentCallList]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_SilentCallList]
	@SilentCallID bigint,
	@UniqueKey bigint,
	@DateTimeofCall datetime
AS
BEGIN
	IF @SilentCallID <= 0 
	BEGIN
		INSERT INTO SilentCallList 
			(UniqueKey,
			DateTimeofCall)
		VALUES
			(@UniqueKey,
			@DateTimeofCall)
		SET @SilentCallID = @@IDENTITY
		SELECT @SilentCallID as SilentCallID
	END
	ELSE
	BEGIN
		UPDATE SilentCallList SET
			UniqueKey = @UniqueKey,
			DateTimeofCall = @DateTimeofCall
		WHERE SilentCallID = @SilentCallID
		SELECT @SilentCallID as SilentCallID
	END
END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_script]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_script] 
		@ScriptID bigint , 
		@ScriptName varchar (255), 
		@ScriptHeader text , 
		@ScriptSubHeader text , 
		@ScriptBody text , 
		@ParentScriptID bigint,
		@ScriptGuid varchar(255),
		@DateCreated datetime, 
		@DateModified datetime  
AS 
	BEGIN 
	IF @ScriptID <= 0 
		BEGIN
			INSERT INTO dbo.script (
				ScriptName,
				ScriptHeader,
				ScriptSubHeader,
				ScriptBody,
				DateCreated,
				DateModified,
				ParentScriptID,
				ScriptGuid
 				)
 			VALUES (
				@ScriptName,
				@ScriptHeader,
				@ScriptSubHeader,
				@ScriptBody,
				GETDATE(),
				GETDATE(),
				@ParentScriptID,
				@ScriptGuid
		 		)

		SET @ScriptID = @@IDENTITY 

		SELECT @ScriptID AS ScriptID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.script 
			SET 
				ScriptName = @ScriptName,
				ScriptHeader = @ScriptHeader,
				ScriptSubHeader = @ScriptSubHeader,
				ScriptBody = @ScriptBody,
				DateModified = GETDATE(),
				ParentScriptID = @ParentScriptID
				--ScriptGuid
			WHERE 	
				ScriptID= @ScriptID 	

			SELECT @ScriptID AS ScriptID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_ScheduledCampaign]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_ScheduledCampaign] 
	@UniqueKey bigint,
	@ScheduleDate datetime
AS
BEGIN
	INSERT INTO Campaign (Campaign,
		PhoneNum,
		DBCompany,
		NeverCallFlag,
		AgentID,
		VerificationAgentID,
		CallResultCode,
		DateTimeofCall,
		CallDuration,
		CallSenttoDialTime,
		CalltoAgentTime,
		CallHangupTime,
		CallCompletionTime,
		CallWrapUpStartTime,
		CallWrapUpStopTime,
		ResultCodeSetTime,
		TotalNumAttempts,
		NumAttemptsAM,
		NumAttemptsWkEnd,
		NumAttemptsPM,
		LeadProcessed,
		FIRSTNAME,
		LASTNAME,
		ADDRESS,
		CITY,
		STATE,
		ZIP,
		ADDRESS2,
		--COUNTRY,
		FullQueryPassCount,
		--APCR,
		--APCRAgent,
		--APCRDT,
		--APCRMemo,
		--APCR2,
		--APCRAgent2,
		--APCRDT2,
		--APCRMemo2,
		--APCR3,
		--APCRAgent3,
		--APCRDT3,
		--APCRMemo3,
		--APCR4,
		--APCRAgent4,
		--APCRDT4,
		--APCRMemo4,
		--APCR5,
		--APCRAgent5,
		--APCRDT5,
		--APCRMemo5,
		--APCR6,
		--APCRAgent6,
		--APCRDT6,
		--APCRMemo6,
		scheduledate)
	SELECT Campaign,
		PhoneNum,
		DBCompany,
		NeverCallFlag,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		null,
		FIRSTNAME,
		LASTNAME,
		ADDRESS,
		CITY,
		STATE,
		ZIP,
		ADDRESS2,
		--COUNTRY,
		null,
		--APCR,
		--APCRAgent,
		--APCRDT,
		--APCRMemo,
		--APCR2,
		--APCRAgent2,
		--APCRDT2,
		--APCRMemo2,
		--APCR3,
		--APCRAgent3,
		--APCRDT3,
		--APCRMemo3,
		--APCR4,
		--APCRAgent4,
		--APCRDT4,
		--APCRMemo4,
		--APCR5,
		--APCRAgent5,
		--APCRDT5,
		--APCRMemo5,
		--APCR6,
		--APCRAgent6,
		--APCRDT6,
		--APCRMemo6,
		@ScheduleDate
	FROM campaign WHERE UniqueKey = @UniqueKey
END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_ResultCode]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_ResultCode] 
		@ResultCodeID bigint , 
		@Description varchar (255), 
		@Presentation bit , 
		@Redialable bit , 
		@RecycleInDays smallint , 
		@Lead bit , 
		@MasterDNC bit , 
		@NeverCall bit , 
		@VerifyOnly bit ,
		@CountAsLiveContact bit,  
		@DialThroughAll bit , 
		@ShowDeletedResultCodes bit , 
		@DateDeleted datetime , 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 
	IF @ResultCodeID <= 0 
		BEGIN
			INSERT INTO dbo.ResultCode (
				Description,
				Presentation,
				Redialable,
				RecycleInDays,
				Lead,
				MasterDNC,
				NeverCall,
				VerifyOnly, 
				CountAsLiveContact, 
				DialThroughAll,
				ShowDeletedResultCodes,
				DateDeleted,
				DateCreated,
				DateModified
 				)
 			VALUES (
				@Description,
				@Presentation,
				@Redialable,
				@RecycleInDays,
				@Lead,
				@MasterDNC,
				@NeverCall,
				@VerifyOnly, 
				@CountAsLiveContact, 
				@DialThroughAll,
				@ShowDeletedResultCodes,
				@DateDeleted,
				GETDATE(),
				GETDATE()
		 		)

		SET @ResultCodeID = @@IDENTITY 

		SELECT @ResultCodeID AS ResultCodeID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.ResultCode 
			SET 
				Description = @Description,
				Presentation = @Presentation,
				Redialable = @Redialable,
				RecycleInDays = @RecycleInDays,
				Lead = @Lead,
				MasterDNC = @MasterDNC,
				NeverCall = @NeverCall,
				VerifyOnly = @VerifyOnly, 
				CountAsLiveContact = @CountAsLiveContact, 
				DialThroughAll = @DialThroughAll,
				ShowDeletedResultCodes = @ShowDeletedResultCodes,
				DateDeleted = @DateDeleted,
				DateModified = GETDATE()
			WHERE 	
				ResultCodeID= @ResultCodeID 	

			SELECT @ResultCodeID AS ResultCodeID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_QueryDetail]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_QueryDetail] 
		@QueryDetailID bigint , 
		@QueryID bigint , 
		@SearchCriteria varchar (100), 
		@SearchOperator varchar (50), 
		@SearchString varchar (500), 
		@LogicalOperator nchar (20), 
		@LogicalOrder smallint ,
		@SubQueryID bigint, 
		@DateCreated datetime , 
		@DateModified datetime,
		@SubsetID int,
		@SubsetName varchar (100),
		@SubsetLevel int,
		@ParentSubsetID int,
		@SubsetLogicalOrder int,
		@TreeNodeID int,
		@ParentTreeNodeID int,
		@ElementText varchar (100) 
AS 
	BEGIN 
	IF @QueryDetailID <= 0 
		BEGIN
			INSERT INTO dbo.QueryDetail (
				QueryID,
				SearchCriteria,
				SearchOperator,
				SearchString,
				LogicalOperator,
				LogicalOrder,
				SubQueryID,
				DateCreated,
				DateModified,
				SubsetID,
				SubsetName,
				SubsetLevel,
				ParentSubsetID,
				SubsetLogicalOrder,
				TreeNodeID,
				ParentTreeNodeID,
				ElementText
 				)
 			VALUES (
				@QueryID,
				@SearchCriteria,
				@SearchOperator,
				@SearchString,
				@LogicalOperator,
				@LogicalOrder,
				@SubQueryID,
				GETDATE(),
				GETDATE(),
				@SubsetID,
				@SubsetName,
				@SubsetLevel,
				@ParentSubsetID,
				@SubsetLogicalOrder,
				@TreeNodeID,
				@ParentTreeNodeID,
				@ElementText
		 		)

		SET @QueryDetailID = @@IDENTITY 

		SELECT @QueryDetailID AS QueryDetailID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.QueryDetail 
			SET 
				QueryID = @QueryID,
				SearchCriteria = @SearchCriteria,
				SearchOperator = @SearchOperator,
				SearchString = @SearchString,
				LogicalOperator = @LogicalOperator,
				LogicalOrder = @LogicalOrder,
				SubQueryID = @SubQueryID,
				DateModified = GETDATE(),
				SubsetID = @SubsetID,
				SubsetName = @SubsetName,
				SubsetLevel = @SubsetLevel,
				ParentSubsetID = @ParentSubsetID,
				SubsetLogicalOrder = @SubsetLogicalOrder,
				TreeNodeID = @TreeNodeID,
				ParentTreeNodeID = @ParentTreeNodeID,
				ElementText = @ElementText
			WHERE 	
				QueryDetailID= @QueryDetailID 	

			SELECT @QueryDetailID AS QueryDetailID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_Query]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_Query] 
		@QueryID bigint , 
		@QueryName varchar (255), 
		@QueryCondition varchar(3000) , 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 

	IF @QueryID <= 0 
		BEGIN
			INSERT INTO dbo.Query (
				QueryName,
				QueryCondition,
				DateCreated,
				DateModified
 				)
 			VALUES (
				@QueryName,
				@QueryCondition,
				GETDATE(),
				GETDATE()
		 		)

		SET @QueryID = @@IDENTITY 

		SELECT @QueryID AS QueryID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.Query 
			SET 
				QueryName = @QueryName,
				QueryCondition = @QueryCondition,
				DateModified = GETDATE()
			WHERE 	
				QueryID= @QueryID 	

			SELECT @QueryID AS QueryID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_OtherParameter]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_OtherParameter] 
		@OtherParameterID bigint , 
		@CallTransfer smallint , 
		@StaticOffsiteNumber varchar (20), 
		@TransferMessage bit , 
		@AutoPlayMessage varchar (255), 
		@HoldMessage varchar (255), 
		@AllowManualDial bit , 
		@StartingLine smallint , 
		@EndingLine smallint , 
		@AllowCallBacks smallint , 
		@AlertSupervisorOnCallbacks smallint , 
		@QueryStatisticsInPercent bit , 
		@DateCreated datetime , 
		@DateModified datetime
AS 
	BEGIN 
	IF @OtherParameterID <= 0 
		BEGIN
			INSERT INTO dbo.OtherParameter (
				CallTransfer,
				StaticOffsiteNumber,
				TransferMessage,
				AutoPlayMessage,
				HoldMessage,
				AllowManualDial,
				StartingLine,
				EndingLine,
				AllowCallBacks,
				AlertSupervisorOnCallbacks,
				QueryStatisticsInPercent,
				DateCreated,
				DateModified
 				)
 			VALUES (
				@CallTransfer,
				@StaticOffsiteNumber,
				@TransferMessage,
				@AutoPlayMessage,
				@HoldMessage,
				@AllowManualDial,
				@StartingLine,
				@EndingLine,
				@AllowCallBacks,
				@AlertSupervisorOnCallbacks,
				@QueryStatisticsInPercent,
				GETDATE(),
				GETDATE()
		 		)

		SET @OtherParameterID = @@IDENTITY 

		SELECT @OtherParameterID AS OtherParameterID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.OtherParameter 
			SET 
				CallTransfer = @CallTransfer,
				StaticOffsiteNumber = @StaticOffsiteNumber,
				TransferMessage = @TransferMessage,
				AutoPlayMessage = @AutoPlayMessage,
				HoldMessage = @HoldMessage,
				AllowManualDial = @AllowManualDial,
				StartingLine = @StartingLine,
				EndingLine = @EndingLine,
				AllowCallBacks = @AllowCallBacks,
				AlertSupervisorOnCallbacks = @AlertSupervisorOnCallbacks,
				QueryStatisticsInPercent = @QueryStatisticsInPercent,
				DateModified = GETDATE()
			WHERE 	
				OtherParameterID= @OtherParameterID 	

			SELECT @OtherParameterID AS OtherParameterID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_GlobalDialingParameters]    Script Date: 01/21/2013 16:45:51 ******/
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
/****** Object:  StoredProcedure [dbo].[InsUpd_DigitalizedRecording]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_DigitalizedRecording] 
		@DigitalizedRecordingID bigint , 
		@EnableRecording bit , 
		@EnableWithABeep bit , 
		@StartWithABeep bit , 
		@RecordToWave bit , 
		@HighQualityRecording bit , 
		@RecordingFilePath varchar (255), 
		@FileNaming varchar (255), 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 
	IF @DigitalizedRecordingID <= 0 
		BEGIN
			INSERT INTO dbo.DigitalizedRecording (
				EnableRecording,
				EnableWithABeep,
				StartWithABeep,
				RecordToWave,
				HighQualityRecording,
				RecordingFilePath,
				FileNaming,
				DateCreated,
				DateModified
 				)
 			VALUES (
				@EnableRecording,
				@EnableWithABeep,
				@StartWithABeep,
				@RecordToWave,
				@HighQualityRecording,
				@RecordingFilePath,
				@FileNaming,
				GETDATE(),
				GETDATE()
		 		)

		SET @DigitalizedRecordingID = @@IDENTITY 

		SELECT @DigitalizedRecordingID AS DigitalizedRecordingID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.DigitalizedRecording 
			SET 
				EnableRecording = @EnableRecording,
				EnableWithABeep = @EnableWithABeep,
				StartWithABeep = @StartWithABeep,
				RecordToWave = @RecordToWave,
				HighQualityRecording = @HighQualityRecording,
				RecordingFilePath = @RecordingFilePath,
				FileNaming = @FileNaming,
				DateModified = GETDATE()
			WHERE 	
				DigitalizedRecordingID= @DigitalizedRecordingID 	

			SELECT @DigitalizedRecordingID AS DigitalizedRecordingID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_DialingParameter]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_DialingParameter] 
		@DailingParameterID bigint , 
		@PhoneLineCount smallint , 
		@DropRatePercent smallint , 
		@RingSeconds smallint , 
		@MinimumDelayBetweenCalls smallint , 
		@DialingMode smallint , 
		@AnsweringMachineDetection bit , 
		@SevenDigitPrefix VARCHAR(20),
		@TenDigitPrefix VARCHAR(20),
		@SevenDigitSuffix VARCHAR(20),
		@TenDigitSuffix VARCHAR(20),
		@ColdCallScriptID bigint , 
		@VerificationScriptID bigint , 
		@InboundScriptID bigint , 
		@AMCallTimes smallint , 
		@PMCallTimes smallint , 
		@WeekendCallTimes smallint , 
		@AMDialingStart datetime ,
		@AMDialingStop datetime , 
		@PMDialingStart datetime , 
		@PMDialingStop datetime , 
		@AnsMachDetect smallint,
		@DateCreated datetime , 
		@DateModified datetime,
		@ErrorRedialLapse int = 5, 
		@BusyRedialLapse int = 5,
		@NoAnswerRedialLapse int = 5,
		@ChannelsPerAgent numeric(5,2) = 2,
		@DefaultCallLapse int = 40,
		@AnsweringMachineMessage varchar(1000) = '',
		@HumanMessageEnable bit = 0,
		@HumanMessage varchar(1000) = '',
		@SilentCallMessageEnable bit = 0,
		@SilentCallMessage varchar(1000) = ''
AS 
	BEGIN 
	IF @DailingParameterID <= 0 
		BEGIN
			INSERT INTO dbo.DialingParameter (
				PhoneLineCount,
				DropRatePercent,
				RingSeconds,
				MinimumDelayBetweenCalls,
				DialingMode,
				AnsweringMachineDetection,
				SevenDigitPrefix,
				TenDigitPrefix,
				SevenDigitSuffix,
				TenDigitSuffix,
				ColdCallScriptID,
				VerificationScriptID,
				InboundScriptID,
				AMCallTimes,
				PMCallTimes,
				WeekendCallTimes,
				AMDialingStart,
				AMDialingStop,
				PMDialingStart,
				PMDialingStop,
				AnsMachDetect,
				DateCreated,
				DateModified,
				ErrorRedialLapse, 
				BusyRedialLapse,
				NoAnswerRedialLapse,
				ChannelsPerAgent,
				DefaultCallLapse,
				AnsweringMachineMessage,
				HumanMessageEnable,
				HumanMessage,
				SilentCallMessageEnable,
				SilentCallMessage
 				)
 			VALUES (
				@PhoneLineCount,
				@DropRatePercent,
				@RingSeconds,
				@MinimumDelayBetweenCalls,
				@DialingMode,
				@AnsweringMachineDetection,
				@SevenDigitPrefix,
				@TenDigitPrefix,
				@SevenDigitSuffix,
				@TenDigitSuffix,
				@ColdCallScriptID,
				@VerificationScriptID,
				@InboundScriptID,
				@AMCallTimes,
				@PMCallTimes,
				@WeekendCallTimes,
				@AMDialingStart,
				@AMDialingStop,
				@PMDialingStart,
				@PMDialingStop,
				@AnsMachDetect,
				GETDATE(),
				GETDATE(),
				@ErrorRedialLapse, 
				@BusyRedialLapse,
				@NoAnswerRedialLapse,
				@ChannelsPerAgent,
				@DefaultCallLapse,
				@AnsweringMachineMessage,
				@HumanMessageEnable,
				@HumanMessage,
				@SilentCallMessageEnable,
				@SilentCallMessage
		 		)

		SET @DailingParameterID = @@IDENTITY 

		SELECT @DailingParameterID AS DailingParameterID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.DialingParameter 
			SET 
				PhoneLineCount = @PhoneLineCount,
				DropRatePercent = @DropRatePercent,
				RingSeconds= @RingSeconds,
				MinimumDelayBetweenCalls = @MinimumDelayBetweenCalls,
				DialingMode = @DialingMode,
				AnsweringMachineDetection = @AnsweringMachineDetection,
				SevenDigitPrefix = @SevenDigitPrefix,
				TenDigitPrefix = @TenDigitPrefix,
				SevenDigitSuffix = @SevenDigitSuffix,
				TenDigitSuffix = @TenDigitSuffix,
				ColdCallScriptID = @ColdCallScriptID,
				VerificationScriptID = @VerificationScriptID,
				InboundScriptID = @InboundScriptID,
				AMCallTimes = @AMCallTimes,
				PMCallTimes = @PMCallTimes,
				WeekendCallTimes = @WeekendCallTimes,
				AMDialingStart = @AMDialingStart,
				AMDialingStop = @AMDialingStop,
				PMDialingStart = @PMDialingStart,
				PMDialingStop = @PMDialingStop,
				AnsMachDetect = @AnsMachDetect,
				DateModified = GETDATE(),
				ErrorRedialLapse = @ErrorRedialLapse, 
				BusyRedialLapse = @BusyRedialLapse,
				NoAnswerRedialLapse = @NoAnswerRedialLapse,
				ChannelsPerAgent = @ChannelsPerAgent,
				DefaultCallLapse = @DefaultCallLapse,
				AnsweringMachineMessage = @AnsweringMachineMessage,
				HumanMessageEnable = @HumanMessageEnable,
				HumanMessage = @HumanMessage,
				SilentCallMessageEnable = @SilentCallMessageEnable,
				SilentCallMessage = @SilentCallMessage
			WHERE 	
				DailingParameterID= @DailingParameterID 	

			SELECT @DailingParameterID AS DailingParameterID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[ResetAgent]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ResetAgent]
	@AgentID bigint	
AS 
BEGIN

UPDATE AgentStat SET	
	 LogOffDate = ISNULL(LogOffDate, GETDATE()),
	 StatusID = 0 
WHERE 
	AgentID = @AgentID
END
GO
/****** Object:  StoredProcedure [dbo].[PrepareDialerQuery]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PrepareDialerQuery]
		@QueryCondition VARCHAR(4000),
		@QueryID bigint
AS
BEGIN

	DECLARE @NewSQLQuery NVARCHAR(4000)
	DECLARE @HigherPrioritySqlQuery NVARCHAR(4000)
	DECLARE @Priority INT
	SET @NewSQLQuery = @QueryCondition
	SET @Priority = (SELECT Priority 
		FROM  CampaignQueryStatus
		WHERE QueryID = @QueryID)
	
	 /* Create a table full of all active queries with higher priority */
	CREATE TABLE #ACTIVEQUERYLIST(SqlQuery NVARCHAR(4000))

		INSERT INTO #ACTIVEQUERYLIST
		SELECT 'SELECT DISTINCT UniqueKey FROM CAMPAIGN WHERE ' + SUBSTRING(CONVERT(varchar(4000),QueryCondition), CHARINDEX('DIALINGPARAMETER WHERE', CONVERT(varchar(4000),QueryCondition)) + 23,
				CHARINDEX('And ((DATEPART(hour, GETDATE()) < 13', CONVERT(varchar(4000),QueryCondition)) - CHARINDEX('DIALINGPARAMETER WHERE', 
				CONVERT(varchar(4000),QueryCondition)) - 23) + ')'
		FROM 
			Query
		INNER JOIN CampaignQueryStatus ON CampaignQueryStatus.QueryID = Query.QueryID 
		WHERE Priority < @Priority AND IsActive > 0 AND CampaignQueryStatus.QueryID <> @QueryID
		/* End Create */
		
		/* Loop Through and build a temp query for 'not in' higher queries */
		DECLARE CrsActiveQueries CURSOR FOR SELECT SqlQuery FROM #ACTIVEQUERYLIST
		OPEN CrsActiveQueries
		FETCH NEXT FROM CrsActiveQueries INTO @HigherPrioritySqlQuery
		WHILE @@FETCH_STATUS = 0 
		BEGIN
			SET @NewSQLQuery = @NewSQLQuery + ' AND UniqueKey NOT IN (' + @HigherPrioritySqlQuery + ')'
			FETCH NEXT FROM CrsActiveQueries INTO @HigherPrioritySqlQuery
		END
		/* end loop */	
	
	DROP TABLE #ACTIVEQUERYLIST
	CLOSE CrsActiveQueries
	DEALLOCATE CrsActiveQueries
	SELECT 'NewQueryCondition' = @NewSQLQuery
END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_CampaignQueryStatus]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_CampaignQueryStatus] 
		@CampaignQueryID bigint, 
		@QueryID bigint , 
		@IsActive bit , 
		@Total int , 
		@Available int , 
		@Dials int , 
		@Talks int , 
		@AnsweringMachine int , 
		@NoAnswer int , 
		@Busy int , 
		@OpInt int , 
		@Drops int ,
		@Failed int, 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 	
	
	Declare @tempCampaignQueryID bigint
	SET @CampaignQueryID = 0
	SET @tempCampaignQueryID = (select CampaignQueryID from CampaignQueryStatus Where QueryID = @QueryID)
	SET @CampaignQueryID = ISNULL(@tempCampaignQueryID, @CampaignQueryID)

	IF @CampaignQueryID <= 0 
		BEGIN
			INSERT INTO dbo.CampaignQueryStatus (
				QueryID,
				IsActive,
				IsStandby,
				Total,
				Available,
				Dials,
				Talks,
				AnsweringMachine,
				NoAnswer,
				Busy,
				OpInt,
				Drops,
				Failed,
				DateCreated,
				DateModified,
				IsCurrent
 				)
 			VALUES (
				@QueryID,
				0,
				0,
				@Total,
				@Available,
				@Dials,
				@Talks,
				@AnsweringMachine,
				@NoAnswer,
				@Busy,
				@OpInt,
				@Drops,
				@Failed,
				GETDATE(),
				GETDATE(),
				0
		 		)

		SET @CampaignQueryID = @@IDENTITY 

		SELECT @CampaignQueryID AS CampaignQueryID

	END	
	ELSE
	BEGIN 
		--IsActive shall be updated using Upd_CampaignQuery_Status
		UPDATE dbo.CampaignQueryStatus 
		SET 
			QueryID = @QueryID,
			--IsActive = @IsActive,
			Total = IsNull(@Total, Total), 
			Available = IsNull(@Available, Available),
			--Dials = @Dials,
			--Talks = @Talks,
			--AnsweringMachine = @AnsweringMachine,
			--NoAnswer = @NoAnswer,
			--Busy = @Busy,
			--OpInt = @OpInt,
			--Drops = @Drops,
			DateModified = GETDATE()
		WHERE 	
			CampaignQueryID= @CampaignQueryID 	

		SELECT @CampaignQueryID AS CampaignQueryID
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Del_QueryByName]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Del_QueryByName] 
 	@QueryName varchar(255) 
AS 
BEGIN
	DECLARE @QueryID numeric(18,0)
	
	SET @QueryID = (SELECT QueryID FROM Query WHERE QueryName = @QueryName)
	
	DELETE FROM dbo.CampaignQueryStatus
	WHERE
	QueryID = @QueryID	

	DELETE FROM dbo.QueryDetail
	WHERE
	QueryID = @QueryID	

	DELETE  dbo.Query
	WHERE
	QueryID = @QueryID		
END
GO
/****** Object:  StoredProcedure [dbo].[Del_Query]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Del_Query] 
 	@QueryID numeric(18,0) 
AS 
BEGIN

	DELETE FROM dbo.CampaignQueryStatus
	WHERE
	QueryID = @QueryID	

	DELETE FROM dbo.QueryDetail
	WHERE
	QueryID = @QueryID	

	DELETE  dbo.Query
	WHERE
	QueryID = @QueryID		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_CampaignQueryStatus_Dtl]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_CampaignQueryStatus_Dtl] 
 	@CampaignQueryID numeric(18,0) 
AS 
BEGIN
				
	SELECT
		CampaignQueryID, 
		QueryID, 
		IsActive,
		IsStandby, 
		Total, 
		Available, 
		Dials, 
		Talks, 
		AnsweringMachine, 
		NoAnswer, 
		Busy, 
		OpInt, 
		Drops,
		Failed, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.CampaignQueryStatus
	WHERE 	
		CampaignQueryID = @CampaignQueryID

END
GO
/****** Object:  StoredProcedure [dbo].[UPDATEQueryAvailableCounts_UsingPriority]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UPDATEQueryAvailableCounts_UsingPriority]
AS
BEGIN
	
	DECLARE @SQLQuery NVARCHAR(4000)
	DECLARE @SQLQueryTotal NVARCHAR(4000)
	DECLARE @HigherPrioritySqlQuery NVARCHAR(4000)
	DECLARE @QueryID bigint
	DECLARE @AvailableCount int
	DECLARE @TotalCount int
	DECLARE @QueryCondition VARCHAR(2000)
	DECLARE @Priority int
	DECLARE @SecondPriority int
	DECLARE @IsActive bit 
	DECLARE @LogEntry varchar(1000) 

	CREATE TABLE #CAMPAIGNQUERY(QueryID bigint, SqlQuery NVARCHAR(4000), Priority int, IsActive bit)

	INSERT INTO #CAMPAIGNQUERY
	SELECT	Query.QueryID, 
		'SELECT ISNULL(COUNT(UniqueKey),0) '+ SUBSTRING(CONVERT(varchar(4000),QueryCondition),
		CHARINDEX('FROM CAMPAIGN, DIALINGPARAMETER WHERE',CONVERT(varchar(4000),QueryCondition)), 
		LEN(CONVERT(varchar(4000),QueryCondition)) + 1 - CHARINDEX('FROM CAMPAIGN, DIALINGPARAMETER WHERE',CONVERT(varchar(4000),QueryCondition))),
		CampaignQueryStatus.Priority,
		CampaignQueryStatus.IsActive
	FROM 
		Query
	INNER JOIN CampaignQueryStatus ON CampaignQueryStatus.QueryID = Query.QueryID
	ORDER BY Priority ASC 

	DECLARE CAMPAIGNQUERIESS CURSOR FOR SELECT QueryID , SqlQuery, Priority, IsActive FROM #CAMPAIGNQUERY
	OPEN CAMPAIGNQUERIESS
	FETCH NEXT FROM CAMPAIGNQUERIESS INTO @QueryID, @SQLQuery, @Priority, @IsActive
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		IF @Priority < 1 OR @Priority IS NULL
		BEGIN
					
			DECLARE @MaxPriority int
			SET @MaxPriority = (SELECT MAX(Priority) FROM CampaignQueryStatus)
			UPDATE CampaignQueryStatus SET Priority = (@MaxPriority +1) WHERE CampaignQueryStatus.QueryID = @QueryID 
		
		END
		
		SET @SQLQueryTotal = @SqlQuery
		SET @SQLQueryTotal = SUBSTRING(@SQLQueryTotal, 0, CHARINDEX('And ((DATEPART(hour, GETDATE())', @SQLQueryTotal)) + ')'
		/* Append the current query with available conditions */
		IF @ISActive > 0
		BEGIN			
			set @QueryCondition = 'AND ( NeverCallFlag=0 or NeverCallFlag IS NULL ) 
								AND (ScheduleDate is null OR DATEDIFF(dd,getdate(),ScheduleDate) <= 0)
								AND ((DateTimeofCall is null AND (CallResultCode is null OR CallResultCode = 0))
									OR CallResultCode NOT IN (
								SELECT DISTINCT ResultCodeID FROM ResultCode  
								WHERE 
									(Redialable = 0 OR NeverCall = 1 OR NeverCall = 2 OR DATEDIFF(dd, Campaign.DateTimeofCall ,GETDATE()) < RecycleInDays)))'
			
			SET @SQLQuery = @SQLQuery + @QueryCondition
			 /* End Append */
			
			 /* Create a table full of all active queries with higher priority */
			CREATE TABLE #ACTIVEQUERYLIST(SqlQuery NVARCHAR(4000))
		
				INSERT INTO #ACTIVEQUERYLIST
				SELECT 'SELECT DISTINCT UniqueKey FROM CAMPAIGN WHERE ' + SUBSTRING(CONVERT(varchar(4000),QueryCondition), CHARINDEX('DIALINGPARAMETER WHERE', CONVERT(varchar(4000),QueryCondition)) + 23,
						CHARINDEX('And ((DATEPART(hour, GETDATE()) < 13', CONVERT(varchar(4000),QueryCondition)) - CHARINDEX('DIALINGPARAMETER WHERE', 
						CONVERT(varchar(4000),QueryCondition)) - 23) + ')'
				FROM 
					Query
				INNER JOIN CampaignQueryStatus ON CampaignQueryStatus.QueryID = Query.QueryID 
				WHERE Priority < @Priority AND IsActive > 0 AND CampaignQueryStatus.QueryID <> @QueryID
			
				/* End Create */
				
				/* Loop Through and build a temp query for 'not in' higher queries */
				DECLARE CrsActiveQueries CURSOR FOR SELECT SqlQuery FROM #ACTIVEQUERYLIST
				OPEN CrsActiveQueries
				FETCH NEXT FROM CrsActiveQueries INTO @HigherPrioritySqlQuery
				WHILE @@FETCH_STATUS = 0 
				BEGIN
					SET @SQLQuery = @SQLQuery + ' AND UniqueKey NOT IN (' + @HigherPrioritySqlQuery + ')'
					FETCH NEXT FROM CrsActiveQueries INTO @HigherPrioritySqlQuery
				END
				/* end loop */	
			
			DROP TABLE #ACTIVEQUERYLIST
			CLOSE CrsActiveQueries
			DEALLOCATE CrsActiveQueries
		END	
		CREATE TABLE #TempAvailable (AvailableCount INT)
		INSERT #TempAvailable
 			EXEC(@SQLQuery )
		SET @AvailableCount = ( SELECT TOP 1 AvailableCount FROM #TempAvailable )
		DROP TABLE #TempAvailable
		
		CREATE TABLE #TempTotal (TotalCount INT)
		INSERT #TempTotal
 			EXEC(@SQLQueryTotal )
		SET @TotalCount = ( SELECT TOP 1 TotalCount FROM #TempTotal )
					
		DROP TABLE #TempTotal
		
		/*SET @LogEntry = 'Query ' + CAST(@QueryID AS VARCHAR(10)) + ' Total Query: ' + @SQLQueryTotal
		INSERT INTO SPLog Values(GETDATE(), @LogEntry)*/
		
		IF @AvailableCount IS NULL SET @AvailableCount = 0
		IF @TotalCount IS NULL SET @TotalCount = 0
		
		IF @IsActive = 0 UPDATE CampaignQueryStatus SET Available = 0 WHERE QueryId = @QueryID
		ELSE UPDATE CampaignQueryStatus SET Available = @AvailableCount WHERE QueryId = @QueryID
		IF @IsActive <> 1 UPDATE CampaignQueryStatus SET Total = @TotalCount WHERE QueryId = @QueryID
		FETCH NEXT FROM CAMPAIGNQUERIESS INTO @QueryID, @SQLQuery, @Priority, @IsActive
	END

	CLOSE CAMPAIGNQUERIESS
	DEALLOCATE CAMPAIGNQUERIESS
	DROP TABLE #CAMPAIGNQUERY

END
GO
/****** Object:  StoredProcedure [dbo].[UPDATEQueryAvailableCounts]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UPDATEQueryAvailableCounts]
AS
BEGIN
	
	DECLARE @SQLQuery NVARCHAR(4000)
	DECLARE @SQLQueryTotal NVARCHAR(4000)
	DECLARE @EarlierDateModifiedSqlQuery NVARCHAR(4000)
	DECLARE @QueryID bigint
	DECLARE @AvailableCount int
	DECLARE @TotalCount int
	DECLARE @QueryCondition VARCHAR(2000)
	DECLARE @DateModified datetime
	DECLARE @SecondDateModified datetime
	DECLARE @IsActive bit 
	DECLARE @LogEntry varchar(1000) 

	CREATE TABLE #CAMPAIGNQUERY(QueryID bigint, SqlQuery NVARCHAR(4000), DateModified datetime, IsActive bit)

	INSERT INTO #CAMPAIGNQUERY
	SELECT	Query.QueryID, 
		'SELECT ISNULL(COUNT(UniqueKey),0) '+ SUBSTRING(CONVERT(varchar(4000),QueryCondition),
		CHARINDEX('FROM CAMPAIGN, DIALINGPARAMETER WHERE',CONVERT(varchar(4000),QueryCondition)), 
		LEN(CONVERT(varchar(4000),QueryCondition)) + 1 - CHARINDEX('FROM CAMPAIGN, DIALINGPARAMETER WHERE',CONVERT(varchar(4000),QueryCondition))),
		CampaignQueryStatus.DateModified,
		CampaignQueryStatus.IsActive
	FROM 
		Query
	INNER JOIN CampaignQueryStatus ON CampaignQueryStatus.QueryID = Query.QueryID
	ORDER BY CampaignQueryStatus.DateModified ASC 

	DECLARE CAMPAIGNQUERIESS CURSOR FOR SELECT QueryID , SqlQuery, DateModified, IsActive FROM #CAMPAIGNQUERY
	OPEN CAMPAIGNQUERIESS
	FETCH NEXT FROM CAMPAIGNQUERIESS INTO @QueryID, @SQLQuery, @DateModified, @IsActive
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		IF @DateModified IS NULL
		BEGIN
			UPDATE CampaignQueryStatus SET DateModified = GETDATE()
		END
		
		SET @SQLQueryTotal = @SqlQuery
		SET @SQLQueryTotal = SUBSTRING(@SQLQueryTotal, 0, CHARINDEX('And ((DATEPART(hour, GETDATE())', @SQLQueryTotal)) + ')'
		/* Append the current query with available conditions */
		IF @ISActive > 0
		BEGIN			
			set @QueryCondition = 'AND ( NeverCallFlag=0 or NeverCallFlag IS NULL ) 
								AND (ScheduleDate is null OR DATEDIFF(dd,getdate(),ScheduleDate) <= 0)
								AND ((DateTimeofCall is null AND (CallResultCode is null OR CallResultCode = 0))
									OR CallResultCode NOT IN (
								SELECT DISTINCT ResultCodeID FROM ResultCode  
								WHERE 
									(Redialable = 0 OR NeverCall = 1 OR NeverCall = 2 OR DATEDIFF(dd, Campaign.DateTimeofCall ,GETDATE()) < RecycleInDays)))'
			
			SET @SQLQuery = @SQLQuery + @QueryCondition
			 /* End Append */
			
			 /* Create a table full of all active queries with higher DateModified */
			CREATE TABLE #ACTIVEQUERYLIST(SqlQuery NVARCHAR(4000))
		
				INSERT INTO #ACTIVEQUERYLIST
				SELECT 'SELECT DISTINCT UniqueKey FROM CAMPAIGN WHERE ' + SUBSTRING(CONVERT(varchar(4000),QueryCondition), CHARINDEX('DIALINGPARAMETER WHERE', CONVERT(varchar(4000),QueryCondition)) + 23,
						CHARINDEX('And ((DATEPART(hour, GETDATE()) < 13', CONVERT(varchar(4000),QueryCondition)) - CHARINDEX('DIALINGPARAMETER WHERE', 
						CONVERT(varchar(4000),QueryCondition)) - 23) + ')'
				FROM 
					Query
				INNER JOIN CampaignQueryStatus ON CampaignQueryStatus.QueryID = Query.QueryID 
				WHERE CampaignQueryStatus.DateModified < @DateModified AND IsActive > 0 AND CampaignQueryStatus.QueryID <> @QueryID
			
				/* End Create */
				
				/* Loop Through and build a temp query for 'not in' higher queries */
				DECLARE CrsActiveQueries CURSOR FOR SELECT SqlQuery FROM #ACTIVEQUERYLIST
				OPEN CrsActiveQueries
				FETCH NEXT FROM CrsActiveQueries INTO @EarlierDateModifiedSqlQuery
				WHILE @@FETCH_STATUS = 0 
				BEGIN
					SET @SQLQuery = @SQLQuery + ' AND UniqueKey NOT IN (' + @EarlierDateModifiedSqlQuery + ')'
					FETCH NEXT FROM CrsActiveQueries INTO @EarlierDateModifiedSqlQuery
				END
				/* end loop */	
			
			DROP TABLE #ACTIVEQUERYLIST
			CLOSE CrsActiveQueries
			DEALLOCATE CrsActiveQueries
		END	
		CREATE TABLE #TempAvailable (AvailableCount INT)
		INSERT #TempAvailable
 			EXEC(@SQLQuery )
		SET @AvailableCount = ( SELECT TOP 1 AvailableCount FROM #TempAvailable )
		DROP TABLE #TempAvailable
		
		CREATE TABLE #TempTotal (TotalCount INT)
		INSERT #TempTotal
 			EXEC(@SQLQueryTotal )
		SET @TotalCount = ( SELECT TOP 1 TotalCount FROM #TempTotal )
					
		DROP TABLE #TempTotal
		
		/*SET @LogEntry = 'Query ' + CAST(@QueryID AS VARCHAR(10)) + ' Total Query: ' + @SQLQueryTotal
		INSERT INTO SPLog Values(GETDATE(), @LogEntry)*/
		
		IF @AvailableCount IS NULL SET @AvailableCount = 0
		IF @TotalCount IS NULL SET @TotalCount = 0
		
		IF @IsActive = 0 UPDATE CampaignQueryStatus SET Available = 0 WHERE QueryId = @QueryID
		ELSE UPDATE CampaignQueryStatus SET Available = @AvailableCount WHERE QueryId = @QueryID
		IF @IsActive <> 1 UPDATE CampaignQueryStatus SET Total = @TotalCount WHERE QueryId = @QueryID
		FETCH NEXT FROM CAMPAIGNQUERIESS INTO @QueryID, @SQLQuery, @DateModified, @IsActive
	END

	CLOSE CAMPAIGNQUERIESS
	DEALLOCATE CAMPAIGNQUERIESS
	DROP TABLE #CAMPAIGNQUERY

END
GO
/****** Object:  StoredProcedure [dbo].[Upd_Query_Status_FromDialer]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_Query_Status_FromDialer]
	@QueryID bigint,
	@IsActive bit,
	@IsStandby bit
AS
BEGIN
	UPDATE dbo.CampaignQueryStatus 
	SET
		IsActive = @IsActive,
		IsStandby = @IsStandby,				
		DateModified = GETDATE()
	WHERE 	
		QueryID= @QueryID 	
	SELECT @QueryID AS CampaignQueryID
END
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignVerificationAgent]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_CampaignVerificationAgent]
	@AgentID bigint,
	@AgentName varchar(50),
	@UniqueKey bigint
AS
UPDATE CAMPAIGN SET
	VerificationAgentID = @AgentID,
	CallToAgentTime = GetDate(), --convert(varchar(23),GetDate(),121),
	CallResultCode = null
WHERE UniqueKey = @UniqueKey

Declare @StatId BIGINT
SET @StatId = ( SELECT TOP 1 statid FROM AGENTSTAT where agentid=@AgentID AND logoffdate IS NULL order by statid desc )
IF @StatId IS NOT NULL
BEGIN
	UPDATE AGENTSTAT SET
		Calls = ISNULL(Calls, 0) + 1
	WHERE statid = @StatId
END
UPDATE [RainMakerMaster].[dbo].[AgentActivity] set AgentStatusID=3 where AgentID=@AgentID and LogoutTime IS NULL
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignSchedule]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_CampaignSchedule]
	@UniqueKey bigint,
	@AgentID bigint,
	@ScheduleDate datetime,
	@ScheduleNotes text,
	@IsFromAgent bit = 0
AS
BEGIN
	UPDATE CAMPAIGN SET
		AgentID = @AgentID,
		scheduledate = @ScheduleDate,
		ScheduleNotes = @ScheduleNotes,
		CallResultCode = 6,
		ResultCodeSetTime = getdate()
	WHERE UniqueKey = @UniqueKey

	IF @IsFromAgent = 1
	BEGIN
		UPDATE dbo.CampaignQueryStatus SET 
			Available = Available + 1
		WHERE	IsCurrent = 1
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignResultCode]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_CampaignResultCode]
		@UniqueKey bigint,
		@CallResultCode int,
		@AgentID bigint,
		@AgentName varchar(50),
		@QueryID bigint
	AS 
		BEGIN
			
			DECLARE @Lead bit
			DECLARE @Presentation bit
			DECLARE @MasterDNC bit
			DECLARE @NeverCall bit
			DECLARE @CallDuration varchar(255)
			DECLARE @CallWrapUpStartTime datetime
			DECLARE @CallWrapUpStopTime datetime
			DECLARE @AgentLoginTime datetime
			DECLARE @DateTimeofCall Datetime
			DECLARE @CurrentDate datetime
			DECLARE @PhoneNum varchar(255)

			SET @CurrentDate = GetDate() --convert(varchar(23),GetDate(),121)
			
			select 
				@Lead = Lead, 
				@Presentation = Presentation,
				@MasterDNC = MasterDNC,
				@NeverCall = NeverCall 
			from 
				RESULTCODE 
			where 
				ResultCodeID = @CallResultCode
			
			SELECT  @CallWrapUpStartTime = CallWrapUpStartTime,
				@DateTimeofCall = isnull(DateTimeofCall, GetDate()),
				@PhoneNum = REPLACE(REPLACE(REPLACE(REPLACE(PHONENUM,' ',''), '-', ''), '(', ''),')', '')
				FROM Campaign WHERE UniqueKey = @UniqueKey
			
			IF (@CallWrapUpStartTime IS NULL) OR (DATEDIFF(ss,@CallWrapUpStartTime,@DateTimeofCall) > 0)
			BEGIN
				SET @CallWrapUpStartTime = null
			END

			if @MasterDNC = 1
			begin
				set @NeverCall = 1
				
				if not exists 
				(
					select 
						md.phoneNum 
					from 
						RainmakerMaster.dbo.MasterDNC md (nolock) 
					where 
						md.PhoneNum = @PhoneNum
				)
				begin
					insert into RainmakerMaster.dbo.MasterDNC
						(PhoneNum)
					values
						(@PhoneNum)
				end
			end

			UPDATE Campaign 
			SET 
				CallResultCode = @CallResultCode,
				ResultCodeSetTime = @CurrentDate,
				CallWrapUpStartTime = isnull(@CallWrapUpStartTime, @CurrentDate),
				CallCompletionTime = isnull(@CallWrapUpStartTime, @CurrentDate),
				CallWrapUpStopTime = @CurrentDate,
				LeadProcessed = convert(varchar(2),isnull(@Lead,0)),
				NeverCallFlag = (Case when @NeverCall = 1 then 1 else 0 end)
			WHERE 	
				UniqueKey = @UniqueKey

			IF @NeverCall = 1
			BEGIN
				UPDATE Campaign 
				SET 
					AgentID = @AgentID,
					AgentName = @AgentName,
					CallResultCode = @CallResultCode,
					ResultCodeSetTime = @CurrentDate,
					--CallWrapUpStartTime = isnull(@CallWrapUpStartTime, @CurrentDate),
					--CallCompletionTime = isnull(@CallWrapUpStartTime, @CurrentDate),
					--CallWrapUpStopTime = @CurrentDate,
					LeadProcessed = convert(varchar(2),isnull(@Lead,0)),
					NeverCallFlag = (Case when @NeverCall = 1 then 1 else 0 end)
				WHERE
					REPLACE(REPLACE(REPLACE(REPLACE(PHONENUM,' ',''), '-', ''), '(', ''),')', '') = @PhoneNum
				AND CallResultCode IS NULL
			END
			
			--select convert(varchar(23),getdate(),121) -- convert(varchar(23),logindate,121)
			SELECT @AgentLoginTime = logindate FROM agentstat WHERE agentid = @AgentID AND logoffDate IS NULL

			SELECT @CallDuration=isnull(CallDuration,0),
			       @CallWrapUpStartTime=isnull(CallWrapUpStartTime, @CurrentDate),
			       @CallWrapUpStopTime=isnull(CallWrapUpStopTime, @CurrentDate) FROM Campaign
			where UniqueKey=@UniqueKey

			SET @CallWrapUpStartTime = (SELECT CASE 
					WHEN 
						DATEDIFF(ss,@CallWrapUpStartTime,@AgentLoginTime) > 0 
					THEN 
						@AgentLoginTime 
					ELSE @CallWrapUpStartTime END)
			
			UPDATE  AgentStat 
			SET 
				LeadsSales = isnull(LeadsSales,0) + (case when @Lead = 1 then 1 else 0 end),
				Presentations = isnull(Presentations,0) + (case when @Presentation = 1 then 1 else 0 end),
				LastResultCodeID = @CallResultCode,
				--TalkTime =IsNull(TalkTime,0) + CAST(@CallDuration as numeric(7,2)),
				WrapTime =IsNull(WrapTime,0) + DATEDIFF(ss,@CallWrapUpStartTime,@CallWrapUpStopTime) 

			WHERE 	
				AgentID = @AgentID AND LogOffDate is null

			UPDATE dbo.AgentStat 
			SET 
				LeadSalesRatio = convert(numeric(5,2), cast(isnull(LeadsSales,0) as decimal) / cast(case when (isnull(Calls,1)) = 0 then 1 else isnull(Calls,1) end  as decimal))
			WHERE 	
				AgentID = @AgentID AND LogOffDate is null

			IF @CallResultCode Is Not Null
			BEGIN
				Declare @CallListID BIGINT
				SET @CallListID = ( SELECT TOP 1 CallListID FROM CallList where 
				UniqueKey = @UniqueKey and QueryID = @QueryID order by CallListID desc )
				IF @CallListID IS NOT NULL
				BEGIN
					
					UPDATE CallList SET 
						ResultCodeID = @CallResultCode,
						CallCompletionTime = isnull(@CallWrapUpStartTime, @CurrentDate),
						CallWrapTime = @CurrentDate,
						DateModified = @CurrentDate 
					WHERE CallListID = @CallListID
				END
			END
		END
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignQueryComplete]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_CampaignQueryComplete]
	@QueryID bigint,
	@IsActive bit,
	@IsStandby bit,
	@ShowMessage bit
AS
BEGIN
	/* DECLARE @QueryCondition VARCHAR(2000)
	DECLARE @OtherQueryCondition VARCHAR (2000)
	DECLARE @NewQueryCondition VARCHAR(2000)
	DECLARE @CharIndex int
	DECLARE @OtherQueryID bigint
	--DECLARE @QueryID bigint
	
	Get the query who is changing's inactive's QID  
	DECLARE Cur CURSOR FOR SELECT QueryID FROM CampaignQueryStatus WHERE CampaignQueryID = @CampaignQueryID
	OPEN Cur
	FETCH NEXT from Cur INTO @QueryID
	CLOSE Cur
	DEALLOCATE Cur

	IF @IsActive = 0
		BEGIN
			--Inserted to update queries for Multiple Query unique records only 08.30.10 by GW, this will update all other queries, removing the
			duplicate limits set by query overlap
			-- Get the query who is going inactive's Q Condition
			Commented out 11.17.10 for issue 83

			DECLARE Cur CURSOR FOR SELECT QueryCondition FROM Query WHERE QueryID = @QueryID
			OPEN Cur
			FETCH NEXT from Cur INTO @QueryCondition
			CLOSE Cur
			DEALLOCATE Cur

			-- Strip all othe Q Conditions from query, down to basic cond. 
			SET @CharIndex = CHARINDEX(' AND UniqueKey NOT IN(', @QueryCondition)	
			IF (@CharIndex > 0) SET @QueryCondition = SUBSTRING(@QueryCondition, 0, @CharIndex) 
			SET @QueryCondition = REPLACE(@QueryCondition, ', PhoneNum, NumAttemptsAM, NumAttemptsWkEnd, NumAttemptsPM, ScheduleDate ', ' ')
			
			-- Pull all other queries that are curerntly active	
			DECLARE Cur2 CURSOR FOR SELECT QueryCondition, QueryID FROM dbo.Query WHERE (QueryID <> @QueryID)

			OPEN Cur2
			FETCH NEXT from Cur2 INTO @OtherQueryCondition, @OtherQueryID
			
			WHILE @@FETCH_STATUS = 0
			BEGIN
			
				SET @NewQueryCondition = REPLACE(CONVERT(VARCHAR(2000),@OtherQueryCondition), ' AND UniqueKey NOT IN(' + @QueryCondition + ')', '')
			
				UPDATE dbo.Query 
				SET
				QueryCondition = @NewQueryCondition
				WHERE
				QueryID = @OtherQueryID 
			
				FETCH NEXT from Cur2 INTO @OtherQueryCondition, @OtherQueryID
			
			END
			CLOSE Cur2
			DEALLOCATE Cur2
			End Multi Query Insert 
		END
	ELSE
		BEGIN
			-- Inserted to update queries for Multiple Query unique records only 08.29.10 by GW
			Commented out 11.17.10 for issue 83
			DECLARE Cur CURSOR FOR SELECT QueryCondition FROM Query WHERE QueryID = @QueryID
			OPEN Cur
			FETCH NEXT from Cur INTO @NewQueryCondition
			CLOSE Cur
			DEALLOCATE Cur 			
			
			SET @CharIndex = CHARINDEX(' AND UniqueKey NOT IN(', @NewQueryCondition)

			IF (@CharIndex > 0) SET @NewQueryCondition = SUBSTRING(@NewQueryCondition, 0, @CharIndex) 
			
			--  Pull all other queries that are active
			DECLARE Cur CURSOR FOR 	SELECT     Query.QueryCondition
									FROM         CampaignQueryStatus INNER JOIN
									Query ON CampaignQueryStatus.QueryID = Query.QueryID
									WHERE     (Query.QueryID <> @QueryID) AND (CampaignQueryStatus.IsActive = 1)
			OPEN Cur
			FETCH NEXT from Cur INTO @OtherQueryCondition
			
			WHILE @@FETCH_STATUS = 0
			BEGIN
				SET @CharIndex = CHARINDEX(' AND UniqueKey NOT IN(', @OtherQueryCondition)

				IF (@CharIndex > 0) SET @OtherQueryCondition = SUBSTRING(@OtherQueryCondition, 0, @CharIndex)
			
				-- Below line should be changed if additional fields are added to query selects.  Note that we are removing exactly all but the UniqueKey
				SET @NewQueryCondition = CONVERT(varchar(2000), @NewQueryCondition) + ' AND UniqueKey NOT IN('+ REPLACE(@OtherQueryCondition,
					', PhoneNum, NumAttemptsAM, NumAttemptsWkEnd, NumAttemptsPM, ScheduleDate ', ' ') + ')' 
			
				FETCH NEXT from Cur INTO @OtherQueryCondition
			
			END
			CLOSE Cur
			DEALLOCATE Cur
			
			UPDATE dbo.Query 
			SET
			QueryCondition = @NewQueryCondition
			WHERE
			QueryID = @QueryID 
			-- End Multi Query Insert 
		
		END*/

	UPDATE dbo.CampaignQueryStatus 
	SET
		IsActive = @IsActive,
		IsStandby = @IsStandby,				
		DateModified = GETDATE(),
		ShowMessage =  @ShowMessage
	WHERE 	
		QueryID= @QueryID 	

	SELECT @QueryID AS QueryID
END

SET QUOTED_IDENTIFIER OFF
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignQuery_Status_FromDialer]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_CampaignQuery_Status_FromDialer]
	@QueryID bigint,
	@Dials int = 0, 
	@Talks int = 0, 
	@AnsweringMachine int = 0 , 
	@NoAnswer int = 0 , 
	@Busy int = 0, 
	@OpInt int = 0, 
	@Drops int = 0,
	@Failed int = 0, 
	@ResultCodeId int = 0
AS
BEGIN
	-- Added for dynamic Count As Live Contact Feature 1.3.0.1 - 11/29/10
	DECLARE @TalksToAdd INT
	SET @TalksToAdd = 0
	
	IF @AnsweringMachine > 0	
	BEGIN
		IF (SELECT CountAsLiveContact FROM ResultCode WHERE Description = 'Answering Machine') > 0
		BEGIN
			SET @TalksToAdd = @TalksToAdd + @AnsweringMachine
		END
	END
	
	IF @NoAnswer > 0	
	BEGIN
		IF (SELECT CountAsLiveContact FROM ResultCode WHERE Description = 'No Answer') > 0
		BEGIN
			SET @TalksToAdd = @TalksToAdd + @NoAnswer
		END
	END
	
	IF @Busy > 0	
	BEGIN
		IF (SELECT CountAsLiveContact FROM ResultCode WHERE Description = 'Busy') > 0
		BEGIN
			SET @TalksToAdd = @TalksToAdd + @Busy
		END
	END
	
	IF @OpInt > 0	
	BEGIN
		IF (SELECT CountAsLiveContact FROM ResultCode WHERE Description = 'Operator Intercept') > 0
		BEGIN
			SET @TalksToAdd = @TalksToAdd + @OpInt
		END
	END
	
	IF @Drops > 0	
	BEGIN
		IF (SELECT CountAsLiveContact FROM ResultCode WHERE Description = 'Dropped') > 0
		BEGIN
			SET @TalksToAdd = @TalksToAdd + @Drops
		END
	END

	IF @ResultCodeId > 0	
	BEGIN
		IF (SELECT CountAsLiveContact FROM ResultCode WHERE ResultCodeID = @ResultCodeId) > 0
		BEGIN
			SET @TalksToAdd = @TalksToAdd + 1
		END
	END

	UPDATE dbo.CampaignQueryStatus set IsCurrent = 0 where QueryID != @QueryID

	UPDATE dbo.CampaignQueryStatus 
	SET
		Dials = Dials + @Dials,
		Talks = Talks + @TalksToAdd + @Talks,
		--Available = Available - @Talks,
		AnsweringMachine = AnsweringMachine + @AnsweringMachine,
		NoAnswer = NoAnswer + @NoAnswer,
		Busy = Busy + @Busy,
		OpInt = OpInt + @OpInt,
		Drops = Drops + @Drops,
		Failed = Failed + @Failed,
		IsCurrent = 1
	WHERE 	
		QueryID= @QueryID 	

	SELECT @QueryID AS QueryID
END
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignQuery_Status]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_CampaignQuery_Status]
	@CampaignQueryID bigint,
	@IsActive bit,
	@IsStandby bit,
	@ShowMessage bit,
	@ResetStats bit = 0
AS
BEGIN
	DECLARE @QueryCondition VARCHAR(2000)
	DECLARE @OtherQueryCondition VARCHAR (2000)
	DECLARE @NewQueryCondition VARCHAR(2000)
	DECLARE @CharIndex int
	DECLARE @OtherQueryID bigint
	DECLARE @QueryID bigint
	
	/* Get the query who is changing''s inactive''s QID */ 
	DECLARE Cur CURSOR FOR SELECT QueryID FROM CampaignQueryStatus WHERE CampaignQueryID = @CampaignQueryID
	OPEN Cur
	FETCH NEXT from Cur INTO @QueryID
	CLOSE Cur
	DEALLOCATE Cur
	DECLARE @MaxPriority int
	SET @MaxPriority = (SELECT MAX(Priority) FROM CampaignQueryStatus)

	if @ResetStats = 1
	begin
		UPDATE dbo.CampaignQueryStatus 
		SET
			IsActive = @IsActive,
			IsStandby = @IsStandby,				
			DateModified = GETDATE(),
			ShowMessage =  ISNULL(@ShowMessage,0),
			Priority = (@MaxPriority + 1),
			Dials				= 0,
			Talks				= 0,
			AnsweringMachine	= 0,
			NoAnswer			= 0,
			Busy				= 0,
			OpInt				= 0,
			Drops				= 0,
			Failed				= 0
		WHERE 	
			CampaignQueryID = @CampaignQueryID
	end
	else
	begin
		UPDATE dbo.CampaignQueryStatus 
		SET
			IsActive = @IsActive,
			IsStandby = @IsStandby,				
			DateModified = GETDATE(),
			ShowMessage =  ISNULL(@ShowMessage,0),
			Priority = (@MaxPriority + 1)
		WHERE 	
			CampaignQueryID = @CampaignQueryID
	end

	SELECT @CampaignQueryID AS CampaignQueryID
END
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignQuery_INDialerQueue]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_CampaignQuery_INDialerQueue]
	@QueryID bigint	
AS
BEGIN
	UPDATE dbo.CampaignQueryStatus set IsCurrent = 0 where QueryID != @QueryID

	UPDATE dbo.CampaignQueryStatus 
	SET		
		IsCurrent = 1
	WHERE 	
		QueryID= @QueryID 	

	SELECT @QueryID AS QueryID
END
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignCallCompletion]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_CampaignCallCompletion]
		@UniqueKey bigint,
		@CallDuration varchar (255)
AS 
		BEGIN
			DECLARE @AgentID varchar(255)
			DECLARE @CallResultCode int

			SELECT @AgentID = AgentID, @CallResultCode = CallResultCode FROM campaign WHERE UniqueKey=@UniqueKey	

			IF (@AgentID IS NOT NULL)
			BEGIN
				IF (@CallResultCode IS NULL) 
				BEGIN
					UPDATE dbo.Campaign 
					SET 
						CallCompletionTime = GetDate(), -- Convert(VARCHAR,GetDate(),21),
						CallWrapUpStartTime = GetDate(), -- Convert(VARCHAR,GetDate(),21),
						CallDuration = @CallDuration				
					WHERE 	
						UniqueKey=@UniqueKey	
				END
				ELSE
				BEGIN
					UPDATE dbo.Campaign 
					SET
						CallCompletionTime = GetDate(), -- Convert(VARCHAR,GetDate(),21),
						CallWrapUpStartTime = GetDate(), -- Convert(VARCHAR,GetDate(),21), 
						CallDuration = @CallDuration				
					WHERE 	
						UniqueKey=@UniqueKey
				END

				Update AgentStat
				SET
					TalkTime =IsNull(TalkTime,0) + CAST(@CallDuration as numeric(7,2))
				WHERE 				 	
					AgentID = (SELECT agentid FROM dbo.Campaign WHERE UniqueKey=@UniqueKey) 
				AND 
					LogOffDate is null

				Declare @CallListID BIGINT
				SET @CallListID = ( SELECT TOP 1 CallListID FROM CallList where UniqueKey = @UniqueKey order by CallListID desc )
				IF @CallListID IS NOT NULL
				BEGIN
					
					UPDATE CallList SET 
						CallDuration = CAST(@CallDuration as numeric(7,2)) 
					WHERE CallListID = @CallListID
				END
			END
		END
GO
/****** Object:  StoredProcedure [dbo].[Upd_CampaignAgent]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_CampaignAgent]
	@AgentID bigint,
	@AgentName varchar(50),
	@UniqueKey bigint
AS
UPDATE CAMPAIGN SET
	AgentID = @AgentID,
	AgentName = @AgentName,
	CallToAgentTime = GetDate(), --convert(varchar(23),GetDate(),121),
	CallResultCode = null
WHERE UniqueKey = @UniqueKey

Declare @StatId BIGINT
SET @StatId = ( SELECT TOP 1 statid FROM AGENTSTAT where agentid=@AgentID AND logoffdate IS NULL order by statid desc )
IF @StatId IS NOT NULL
BEGIN
	UPDATE AGENTSTAT SET
		Calls = ISNULL(Calls, 0) + 1
	WHERE statid = @StatId
END
UPDATE [RainMakerMaster].[dbo].[AgentActivity] set AgentStatusID=3 where AgentID=@AgentID and LogoutTime IS NULL
GO
/****** Object:  StoredProcedure [dbo].[Upd_Campaign]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_Campaign]
	@UniqueKey bigint,
	@CallType int = 0,
	@QueryID bigint
AS
UPDATE CAMPAIGN SET
	DateTimeofCall = GetDate(),
	NumAttemptsAM = ISNULL(NumAttemptsAM, 0) + (CASE WHEN @CallType = 0 THEN 1 ELSE 0 END),
	NumAttemptsPM = ISNULL(NumAttemptsPM, 0) + (CASE WHEN @CallType = 1 THEN 1 ELSE 0 END),
	NumAttemptsWkEnd = ISNULL(NumAttemptsWkEnd, 0) + (CASE WHEN @CallType = 2 THEN 1 ELSE 0 END),
	TotalNumAttempts = ISNULL(TotalNumAttempts, 0)  + 1,
	CallHangupTime = NULL,
	CallResultCode = NULL,
	AgentID = NULL
WHERE UniqueKey = @UniqueKey

DECLARE @ResultCodeID int
DECLARE @AgentID bigint
DECLARE @PhoneNumber varchar(50)
SELECT @ResultCodeID = ResultCodeID from ResultCode where Description = 'Transferred to Dialer'
SELECT @PhoneNumber = PhoneNum, @AgentID=cast(ISNULL(AgentID, 0) as bigint) from Campaign where UniqueKey = @UniqueKey
IF (@QueryID > 0)
BEGIN
	INSERT INTO CallList values(@AgentID, '', @ResultCodeID, NULL, @PhoneNumber, NULL, getdate(), getdate(),0,getdate(),getdate(),0, getdate(), getdate(), @UniqueKey, @QueryID, 0) 
END
GO
/****** Object:  StoredProcedure [dbo].[Upd_Calllist]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_Calllist]
	@UniqueKey bigint,
	@ResultDesc varchar(100),
	@AgentId bigint,
	@VerificationAgentID bigint,
	@OffsiteTransferNumber varchar(50)
AS

Declare @CallListID BIGINT
SET @CallListID = ( SELECT TOP 1 CallListID FROM CallList where UniqueKey = @UniqueKey order by CallListID desc )
IF @CallListID IS NOT NULL
BEGIN
	Declare @ResultCodeId BIGINT
	DECLARE @NeverCall bit
	--IF @ResultDesc = 'Failed' Call result add retur
	--BEGIN
	--	SET @ResultCodeId = -1
	--END
	--ELSE
	BEGIN
		SET @ResultCodeId = ( SELECT TOP 1 ResultCodeID FROM ResultCode where Description = @ResultDesc )
	END
	IF @ResultCodeId IS NOT NULL
	BEGIN
		UPDATE CallList SET 
			ResultCodeID = @ResultCodeId, 
			AgentId = (CASE WHEN @AgentId = 0 THEN AgentId ELSE @AgentId END),
			VerificationAgentID = (CASE WHEN @VerificationAgentId = 0 THEN VerificationAgentId ELSE @VerificationAgentId END),
			DateModified=GetDate(),
			OffsiteTransferNumber = @OffsiteTransferNumber  
		WHERE CallListID = @CallListID

		SELECT @NeverCall = NeverCall FROM RESULTCODE WHERE ResultCodeID = @ResultCodeId
		
		--IF @NeverCall = 1
		--BEGIN
			UPDATE Campaign 
			SET 
				NeverCallFlag = @NeverCall,
				CallResultCode = @ResultCodeId
			WHERE 	
				UniqueKey = @UniqueKey
		--END
	END
END
GO
/****** Object:  StoredProcedure [dbo].[Upd_AgentStatus]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_AgentStatus] 
		@AgentID bigint, 
		@StatusID bigint
AS 
BEGIN 
	UPDATE dbo.AgentStat 
	SET 
		AgentID = ISNULL(@AgentID,AgentID),
		StatusID = ISNULL(@StatusID,StatusID)
	WHERE 	
		AgentID= @AgentID	

	SELECT @AgentID AS AgentID
END

SET QUOTED_IDENTIFIER ON
GO
/****** Object:  StoredProcedure [dbo].[Upd_AgentStat]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Upd_AgentStat] 
		@AgentID bigint,
		@LeadProcessed varchar(255)
	AS 
BEGIN
	IF @LeadProcessed='Yes'
	BEGIN 
		UPDATE dbo.AgentStat 
		SET 
			LeadsSales = isnull(LeadsSales,0)+1
		WHERE 	
			AgentID = @AgentID AND LogOffDate is null
	END	

	UPDATE dbo.AgentStat 
	SET 
		Presentations = isnull(Presentations,0)+1,
		LeadSalesRatio = convert(numeric(5,2), cast(isnull(LeadsSales,0) as decimal) / cast(isnull(Calls,1) as decimal))
	WHERE 	
		AgentID = @AgentID AND LogOffDate is null
									
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_CampaignDetails_By_AgentID]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_CampaignDetails_By_AgentID]
	@AgentID bigint,
	@IsManualDial bit = 0
AS
/*Select *
FROM Campaign WHERE AgentID = @AgentID and CallResultCode IS NULL and IsManualDial=@IsManualDial
ORDER BY DateTimeofCall DESC */


Select Campaign.*, CL.QueryId
FROM Campaign 
INNER JOIN  calllist CL on CL.uniquekey = campaign.uniquekey
INNER JOIN 
	(SELECT MAX(calllistid) AS MaxCallListID FROM calllist GROUP BY uniquekey) MCL ON MCL.MaxCallListID = CL.CallListID  

	And CL.ResultCodeID in (Select ResultCodeId from resultcode where description in ('Transferred to Agent','Transferred to Verification'))
WHERE (CL.AgentID = @AgentID and CL.IsManualDial=@IsManualDial and (Campaign.VerificationAgentID IS NULL OR Campaign.VerificationAgentID = 0))
	OR (Campaign.VerificationAgentID = @AgentID)
ORDER BY DateTimeofCall DESC
GO
/****** Object:  StoredProcedure [dbo].[Sel_CallList_List]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_CallList_List] 
 	 
AS 
BEGIN
				
	SELECT
		CallListID, 
		AgentID, 
		ResultCodeID, 
		PhoneNumber, 
		CallDate, 
		CallTime, 
		CallDuration, 
		CallCompletionTime, 
		CallWrapTime, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.CallList
	
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_CallList_Dtl]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_CallList_Dtl] 
 	@CallListID numeric(18,0) 
AS 
BEGIN
				
	SELECT
		CallListID, 
		AgentID, 
		ResultCodeID, 
		PhoneNumber, 
		CallDate, 
		CallTime, 
		CallDuration, 
		CallCompletionTime, 
		CallWrapTime, 
		DateCreated, 
		DateModified 
	FROM  
		dbo.CallList
	WHERE 	
		CallListID = @CallListID
		
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_AgentStat_List]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_AgentStat_List]  
	@CampaignID bigint
AS 
BEGIN
				
	SELECT
		AST.StatID, 
		AST.AgentID, 
		A.[AgentName],
		AST.StatusID, 
		ATS.Status,
		AST.LeadsSales, 
		AST.Presentations, 
		AST.Calls, 
		AST.LeadSalesRatio,
		AST.PledgeAmount, 
		AST.TalkTime, 
		AST.WaitingTime, 
		AST.PauseTime, 
		AST.WrapTime, 
		AST.LoginDate, 
		AST.LogOffDate, 
		AST.LastResultCodeID, 
		AST.DateCreated, 
		AST.DateModified,
		RC.Description as ResultCodeDesc,
		A.PhoneNumber,
		AST.LoginTime,
		AST.LogOffTime,
		AST.TimeModified,
		ISNULL(RainmakerMaster.dbo.CampaignAgentStationInfo(@CampaignID,AST.AgentID), A.PhoneNumber) as StationIP
	FROM  
		dbo.AgentStat AST
	INNER JOIN
		RainmakerMaster.dbo.Agent A ON A.AgentID = AST.AgentID
	INNER JOIN
		RainmakerMaster.dbo.AgentStatus ATS ON ATS.AgentStatusID = AST.StatusID
	LEFT OUTER JOIN
		ResultCode RC ON RC.ResultCodeID = AST.LastResultCodeID
	WHERE   
		AST.LogOffDate is null
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_AgentStat_Dtl]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_AgentStat_Dtl]  	
	@StatID numeric(18,0) 
AS 
BEGIN
				
	SELECT
		StatID, 
		AgentID, 
		StatusID, 
		LeadsSales, 
		Presentations, 
		Calls, 
		LeadSalesRatio,
		PledgeAmount, 
		TalkTime, 
		WaitingTime, 
		PauseTime, 
		WrapTime, 
		LoginDate, 
		LogOffDate, 
		LastResultCodeID, 
		DateCreated, 
		DateModified,
		LoginTime,
		LogOffTime,
		TimeModified
	FROM  
		dbo.AgentStat
	WHERE 	
		StatID = @StatID	
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_AgentStat_By_CampaignID]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_AgentStat_By_CampaignID]
@CampaignID bigint
AS
BEGIN
SELECT TalkTime,WrapTime,AgentStat.AgentId 
FROM AgentStat,[RainmakerMaster].[dbo].[AgentActivity] AgentAct
WHERE 
LogOffDate is null and agentact.AgentId=AgentStat.AgentId 
and LogoutTime is null 
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_ActiveQuery_List]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_ActiveQuery_List] 
 	 
AS 
BEGIN
	SELECT
		dbo.Query.QueryID, 
		dbo.Query.QueryName, 
		dbo.Query.QueryCondition, 
		dbo.Query.DateCreated, 
		dbo.CampaignQueryStatus.DateModified 
	FROM  
		dbo.Query
	INNER JOIN
		dbo.CampaignQueryStatus ON dbo.CampaignQueryStatus.QueryID = dbo.Query.QueryID
	WHERE
		dbo.CampaignQueryStatus.IsActive=1
	ORDER BY
		dbo.CampaignQueryStatus.DateModified ASC 
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_StandbyQuery_List]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_StandbyQuery_List] 
 	 
AS 
BEGIN
	SELECT
		dbo.Query.QueryID, 
		dbo.Query.QueryName, 
		dbo.Query.QueryCondition, 
		dbo.Query.DateCreated, 
		dbo.CampaignQueryStatus.DateModified 
	FROM  
		dbo.Query
	INNER JOIN
		dbo.CampaignQueryStatus ON dbo.CampaignQueryStatus.QueryID = dbo.Query.QueryID
	WHERE
		dbo.CampaignQueryStatus.IsActive = 0 AND dbo.CampaignQueryStatus.IsStandby = 1
	ORDER BY
		dbo.CampaignQueryStatus.DateModified ASC 
END
GO
/****** Object:  StoredProcedure [dbo].[Ins_CampaignManualDial]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Ins_CampaignManualDial]
	@AgentID bigint,
	@AgentName varchar(50),
	@PhoneNum varchar(20)
AS
BEGIN
	DECLARE @UniqueKey bigint
	Declare @StatId BIGINT
	DECLARE @ResultCodeID int
	SET @UniqueKey = ( SELECT TOP 1 UniqueKey FROM CAMPAIGN where PhoneNum = REPLACE(REPLACE(REPLACE(REPLACE(@PhoneNum,' ',''), '-', ''), '(', ''),')', '') )
	

	IF @UniqueKey IS NULL
	BEGIN
		INSERT INTO Campaign (AgentID,AgentName,PhoneNum,IsManualDial)
			VALUES(@AgentID,@AgentName,@PhoneNum,1)

		
		SELECT @ResultCodeID = ResultCodeID from ResultCode where Description = 'Transferred to Agent'
		SELECT @UniqueKey = UniqueKey from CAMPAIGN where PhoneNum = @PhoneNum and AgentID=@AgentID and IsManualDial = 1
		INSERT INTO CallList values(@AgentID, @AgentName, @ResultCodeID, NULL, @PhoneNum, NULL, getdate(), getdate(),0,getdate(),getdate(),0, getdate(), getdate(), @UniqueKey, 0, 1)

		
		SET @StatId = ( SELECT TOP 1 statid FROM AGENTSTAT where agentid=@AgentID AND logoffdate IS NULL order by statid desc )
		IF @StatId IS NOT NULL
		BEGIN
			UPDATE AGENTSTAT SET  Calls = ISNULL(Calls, 0) + 1  WHERE statid = @StatId
		END

		SELECT 1
	END
	ELSE
	BEGIN
		DECLARE @IsNeverCall bit
		SET @IsNeverCall = ( SELECT NeverCallFlag FROM CAMPAIGN where UniqueKey = @UniqueKey )
		IF (@IsNeverCall != 1)
		BEGIN
			SET @IsNeverCall = (SELECT top 1 NeverCall from resultcode where ResultCodeID = 
				(SELECT top 1 CallResultCode from CAMPAIGN where UniqueKey = @UniqueKey))
		END
		IF @IsNeverCall IS NULL
		BEGIN
			SET @IsNeverCall = 0
		END
		IF (@IsNeverCall != 1)
		BEGIN
			-- update the record
			UPDATE Campaign set 
				CallResultCode = NULL ,DateTimeofCall = NULL, IsManualDial = 1, 
				AgentID = @AgentID, AgentName = @AgentName, CallHangupTime = null 
				WHERE UniqueKey = @UniqueKey


			SELECT @ResultCodeID = ResultCodeID from ResultCode where Description = 'Transferred to Agent'
			SELECT @UniqueKey = UniqueKey from CAMPAIGN where PhoneNum = @PhoneNum and AgentID=@AgentID and IsManualDial = 1
			INSERT INTO CallList values(@AgentID, @AgentName, @ResultCodeID, NULL, @PhoneNum, NULL, getdate(), getdate(),0,getdate(),getdate(),0, getdate(), getdate(), @UniqueKey, 0, 1)

			SET @StatId = ( SELECT TOP 1 statid FROM AGENTSTAT where agentid=@AgentID AND logoffdate IS NULL order by statid desc )
			IF @StatId IS NOT NULL
			BEGIN
				UPDATE AGENTSTAT SET  Calls = ISNULL(Calls, 0) + 1  WHERE statid = @StatId
			END

			SELECT 1
		END
		ELSE
			SELECT 0
	END
END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_CallList]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_CallList] 
		@CallListID bigint , 
		@AgentID bigint , 
		@ResultCodeID bigint ,
		@VerificationAgentID bigint , 
		@PhoneNumber varchar (20), 
		@CallDate datetime , 
		@CallTime datetime , 
		@CallDuration numeric (5,2), 
		@CallCompletionTime datetime , 
		@CallWrapTime datetime , 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 
	IF @CallListID <= 0 
		BEGIN
			INSERT INTO dbo.CallList (
				AgentID,
				ResultCodeID,
				VerificationAgentID,
				PhoneNumber,
				CallDate,
				CallTime,
				CallDuration,
				CallCompletionTime,
				CallWrapTime,
				DateCreated,
				DateModified
 				)
 			VALUES (
				@AgentID,
				@ResultCodeID,
				@VerificationAgentID,
				@PhoneNumber,
				@CallDate,
				@CallTime,
				@CallDuration,
				@CallCompletionTime,
				@CallWrapTime,
				GETDATE(),
				GETDATE()
		 		)

		SET @CallListID = @@IDENTITY 

		SELECT @CallListID AS CallListID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.CallList 
			SET 
				AgentID = @AgentID,
				ResultCodeID = @ResultCodeID,
				VerificationAgentID = @VerificationAgentID,
				PhoneNumber = @PhoneNumber,
				CallDate = @CallDate,
				CallTime = @CallTime,
				CallDuration = @CallDuration,
				CallCompletionTime = @CallCompletionTime,
				CallWrapTime = @CallWrapTime,
				DateModified = GETDATE()
			WHERE 	
				CallListID= @CallListID 	

			SELECT @CallListID AS CallListID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[InsUpd_AgentStat]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[InsUpd_AgentStat] 
		@StatID bigint, 
		@AgentID bigint, 
		@StatusID bigint, 
		@LeadsSales int , 
		@Presentations int , 
		@Calls int , 
		@LeadSalesRatio numeric (5,2),
		@PledgeAmount varchar(20), 
		@TalkTime numeric (7,2), 
		@WaitingTime numeric (7,2), 
		@PauseTime numeric (7,2), 
		@WrapTime numeric (18,2), 
		@LoginDate datetime, 
		@LogOffDate datetime, 
		@LoginTime datetime, 
		@LogOffTime datetime,
		@LastResultCodeID bigint , 
		@DateCreated datetime , 
		@DateModified datetime  
AS 
	BEGIN 
	IF @StatID <= 0 
		BEGIN

			UPDATE dbo.AgentStat SET 
				LogOffDate = GETDATE(),
				LogOffTime = '1/1/1901 ' + convert(varchar(8),getdate(),108)
			WHERE
				AgentID = @AgentID AND LogOffDate IS NULL


			INSERT INTO dbo.AgentStat (
				AgentID,
				StatusID,
				LeadsSales,
				Presentations,
				Calls,
				LeadSalesRatio,
				PledgeAmount,
				TalkTime,
				WaitingTime,
				PauseTime,
				WrapTime,
				LoginDate,
				LogOffDate,
				LastResultCodeID,
				DateCreated,
				DateModified,
				LoginTime,
				LogOffTime,
				TimeModified
 				)
 			VALUES (
				@AgentID,
				@StatusID,
				ISNULL(@LeadsSales,0),
				ISNULL(@Presentations,0),
				ISNULL(@Calls,0),
				ISNULL(@LeadSalesRatio,0),
				ISNULL(CONVERT(money,@PledgeAmount), 0),
				ISNULL(@TalkTime,0),
				ISNULL(@WaitingTime,0),
				ISNULL(@PauseTime,0),
				ISNULL(@WrapTime,0),
				@LoginDate,
				@LogOffDate,
				@LastResultCodeID,
				GETDATE(),
				GETDATE(),
				@LoginTime,
				@LogOffTime,
				'1/1/1901 ' + convert(varchar(8),getdate(),108)
		 		)

		SET @StatID = @@IDENTITY 

		SELECT @StatID AS StatID

	END
	
	ELSE
		BEGIN
 
			UPDATE dbo.AgentStat 
			SET 
				AgentID = ISNULL(@AgentID,AgentID),
				StatusID = ISNULL(@StatusID,StatusID),
				LeadsSales = ISNULL(@LeadsSales,LeadsSales),
				Presentations = ISNULL(@Presentations,Presentations),
				Calls = ISNULL(@Calls,Calls),
				LeadSalesRatio = ISNULL(@LeadSalesRatio,LeadSalesRatio),
				PledgeAmount = ISNULL(CONVERT(money, @PledgeAmount), PledgeAmount),
				TalkTime = ISNULL(@TalkTime,TalkTime),
				WaitingTime = ISNULL(@WaitingTime,WaitingTime),
				PauseTime = ISNULL(@PauseTime,PauseTime),
				WrapTime = ISNULL(@WrapTime,WrapTime),
				LoginDate = ISNULL(@LoginDate,LoginDate),
				LogOffDate = CASE WHEN @LogOffDate IS NOT NULL THEN GETDATE() ELSE LogOffDate End,
				LastResultCodeID = ISNULL(@LastResultCodeID,LastResultCodeID),
				DateModified = GETDATE(),
				LoginTime = ISNULL(@LoginTime,LoginTime),
				LogOffTime = ISNULL(@LogOffTime, LogOffTime),
				TimeModified = '1/1/1901 ' + convert(varchar(8),getdate(),108)
			WHERE 	
				StatID= @StatID 	

			SELECT @StatID AS StatID
		END
	END
GO
/****** Object:  StoredProcedure [dbo].[GET_QUERYCAMPAIGNLIST]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GET_QUERYCAMPAIGNLIST]
	@queryid bigint,
	@QueryCondition TEXT
AS
BEGIN

CREATE TABLE #CAMPAIGNLIST
	(UniqueKey bigint, PhoneNum varchar(20), 
	NumAttemptsAM varchar(255), 
	NumAttemptsWkEnd varchar(255), 
	NumAttemptsPM varchar(255), 
	ScheduleDate Datetime)

EXEC ('INSERT INTO #CAMPAIGNLIST ' + @QueryCondition + ' AND (NeverCallFlag IS NULL OR NeverCallFlag=0)')

SELECT DISTINCT UniqueKey, 
	PhoneNum, 
	NumAttemptsAM,
	NumAttemptsWkEnd, 
	NumAttemptsPM, 
	ScheduleDate,
	1 AS OrderIndex  
FROM 
	#CAMPAIGNLIST
WHERE UniqueKey NOT IN (SELECT UniqueKey FROM CallList WHERE QueryID = @QueryID)
AND (ScheduleDate is null OR DATEDIFF(dd,getdate(),ScheduleDate) <= 0)
UNION ALL
SELECT DISTINCT #CAMPAIGNLIST.UniqueKey, 
	PhoneNum, 
	NumAttemptsAM, 
	NumAttemptsWkEnd, 
	NumAttemptsPM, 
	ScheduleDate,
	2 AS OrderIndex  
FROM 
	CALLLIST CL
INNER JOIN (SELECT MAX(calllistid) AS MaxCallListID FROM calllist GROUP BY uniquekey) MCL 
	ON MCL.MaxCallListID = CL.CallListID 
INNER JOIN 
	#CAMPAIGNLIST ON CL.UniqueKey = #CAMPAIGNLIST.UniqueKey
INNER JOIN 
	ResultCode ON ResultCode.ResultCodeID = CL.ResultCodeID
WHERE 
	CL.IsManualDial = 0  AND
(CL.queryid = @queryid AND (ResultCode.Redialable = 1 AND ResultCode.NeverCall = 0 
	AND DATEDIFF(dd,CL.calldate,getdate()) >= RecycleInDays)) 
 	AND (ScheduleDate is null OR DATEDIFF(dd,getdate(),ScheduleDate) <= 0)

ORDER BY OrderIndex, #CAMPAIGNLIST.UniqueKey

END
GO
/****** Object:  StoredProcedure [dbo].[Get_CampaignActiveDialCount]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Get_CampaignActiveDialCount]
AS
BEGIN
	
	DECLARE @SQLQuery NVARCHAR(4000)
	DECLARE @QueryID bigint

	CREATE TABLE #CAMPAIGNQUERY(QueryID bigint, SqlQuery NVARCHAR(4000), reccount int)
	

	INSERT INTO #CAMPAIGNQUERY
	SELECT	QueryID, 'SELECT ISNULL(COUNT(UniqueKey),0)'+ SUBSTRING(CONVERT(varchar(4000),QueryCondition),
		CHARINDEX('FROM CAMPAIGN, DIALINGPARAMETER WHERE',CONVERT(varchar(4000),QueryCondition)), 
		LEN(CONVERT(varchar(4000),QueryCondition)) + 1 - CHARINDEX('FROM CAMPAIGN, DIALINGPARAMETER WHERE',CONVERT(varchar(4000),QueryCondition)) ),0
	FROM 
		Query
	WHERE 
		QueryID IN (SELECT QueryID FROM CampaignQueryStatus WHERE IsActive =1)

	DECLARE CAMPAIGNQUERIESS CURSOR FOR SELECT QueryID , SqlQuery FROM #CAMPAIGNQUERY
	OPEN CAMPAIGNQUERIESS
	FETCH NEXT FROM CAMPAIGNQUERIESS INTO @QueryID, @SQLQuery
		
	WHILE @@FETCH_STATUS = 0 
	BEGIN
		
		SET @SQLQuery = 'UPDATE #CAMPAIGNQUERY SET reccount = ('+@SQLQuery+') WHERE QueryID = ' + CONVERT(VARCHAR(10),@QueryID)
		EXEC(@SQLQuery)
		FETCH NEXT FROM CAMPAIGNQUERIESS INTO  @QueryID, @SQLQuery
	END

	CLOSE CAMPAIGNQUERIESS
	DEALLOCATE CAMPAIGNQUERIESS
	SELECT ISNULL(SUM(reccount),0) as CampaignRecCount FROM #CAMPAIGNQUERY
	DROP TABLE #CAMPAIGNQUERY

END
GO
/****** Object:  StoredProcedure [dbo].[CreateDefaultQueries]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CreateDefaultQueries] 
/* Creates 2 default queries to insert when building a campaign.  Added by GW 09.08.10 */
AS 
	BEGIN
		/* Create first query - All numbers, just checks for phonenum field null */			
		INSERT INTO dbo.Query (QueryName, QueryCondition, DateCreated, DateModified,isDefault)
		VALUES ('All Numbers', 
			'SELECT distinct UniqueKey, PhoneNum, NumAttemptsAM, NumAttemptsWkEnd, NumAttemptsPM, ScheduleDate  FROM CAMPAIGN, DIALINGPARAMETER WHERE (  PhoneNum Is Not Null And ((DATEPART(hour, GETDATE()) < 13 And (NumAttemptsAM is NULL OR NumAttemptsAM < DialingParameter.AMCallTimes)) Or (DATEPART(hour, GETDATE()) > 12 And (NumAttemptsPM is NULL OR NumAttemptsPM < DialingParameter.PMCallTimes))))',
			GETDATE(), GETDATE(),1)
			
		DECLARE @QueryID bigint
		
		
		SET @QueryID = @@IDENTITY
		
		DECLARE @SearchOperator1 varchar(30)
		SET @SearchOperator1  = CHAR(123) + '0' + CHAR(125) + ' Is Not Null ' + CHAR(123) + '1' + CHAR(125)
		
		DECLARE @SearchOperator2 varchar(30)
		SET @SearchOperator2  = CHAR(123) + '0' + CHAR(125) + ' Is Null ' + CHAR(123) + '1' + CHAR(125)
	
		INSERT INTO dbo.QueryDetail (QueryID, SearchCriteria, SearchOperator, SearchString, LogicalOperator, LogicalOrder, SubQueryID, DateCreated, DateModified, SubsetID, SubsetLevel, ParentSubsetID, TreeNodeId, ParentTreeNodeID, SubsetLogicalOrder,IsDefault)
		VALUES (@QueryID, 'PhoneNum', @SearchOperator1, '', 'And', '1', 0, GETDATE(), GETDATE(), 0, 0, 0, 1, 0, 0, 1)
		
		/* Update query status.  Set status feilds to default and set query to standby */
		INSERT INTO dbo.CampaignQueryStatus (
				QueryID,
				IsActive,
				IsStandby,
				Total,
				Available,
				Dials,
				Talks,
				AnsweringMachine,
				NoAnswer,
				Busy,
				OpInt,
				Drops,
				Failed,
				DateCreated,
				DateModified,
				IsCurrent,
				IsDefault
 				)
 			VALUES (
				@QueryID, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, GETDATE(), GETDATE(), 0, 1
		 		)
		
		/* Create first query - New numbers, checks for phonenum field null, Datetime of last call null and result code null */	
		INSERT INTO dbo.Query (QueryName, QueryCondition, DateCreated, DateModified,isDefault)
		VALUES ('New Numbers', 
			'SELECT distinct UniqueKey, PhoneNum, NumAttemptsAM, NumAttemptsWkEnd, NumAttemptsPM, ScheduleDate  FROM CAMPAIGN, DIALINGPARAMETER WHERE (  PhoneNum Is Not Null  And  DateTimeofCall Is Null  And  CallResultCode Is Null And ((DATEPART(hour, GETDATE()) < 13 And (NumAttemptsAM is NULL OR NumAttemptsAM < DialingParameter.AMCallTimes)) Or (DATEPART(hour, GETDATE()) > 12 And (NumAttemptsPM is NULL OR NumAttemptsPM < DialingParameter.PMCallTimes))))',
			GETDATE(), GETDATE(),1)
			
		DECLARE @QueryID2 bigint
		
		SET @QueryID2 = @@IDENTITY
	
		INSERT INTO dbo.QueryDetail (QueryID, SearchCriteria, SearchOperator, SearchString, LogicalOperator, LogicalOrder, SubQueryID, DateCreated, DateModified, SubsetID, SubsetLevel, ParentSubsetID, TreeNodeId, ParentTreeNodeID, SubsetLogicalOrder,IsDefault)
		VALUES (@QueryID2, 'PhoneNum', @SearchOperator1, '', 'And', '1', 0,GETDATE(),GETDATE(), 0, 0, 0, 1, 0, 0, 1)
		
		INSERT INTO dbo.QueryDetail (QueryID, SearchCriteria, SearchOperator, SearchString, LogicalOperator, LogicalOrder, SubQueryID, DateCreated, DateModified, SubsetID, SubsetLevel, ParentSubsetID, TreeNodeId, ParentTreeNodeID, SubsetLogicalOrder,IsDefault)
		VALUES (@QueryID2, 'DateTimeofCall', @SearchOperator2, '', 'And', '2', 0 ,GETDATE(),GETDATE(), 0, 0, 0, 2, 0, 1, 1)
		
		INSERT INTO dbo.QueryDetail (QueryID, SearchCriteria, SearchOperator, SearchString, LogicalOperator, LogicalOrder, SubQueryID, DateCreated, DateModified, SubsetID, SubsetLevel, ParentSubsetID, TreeNodeId, ParentTreeNodeID, SubsetLogicalOrder,IsDefault)
		VALUES (@QueryID2, 'CallResultCode', @SearchOperator2, '', 'And', '3', 0 ,GETDATE(),GETDATE(), 0, 0, 0, 3, 0, 2, 1)
		
				/* Update query status.  Set status feilds to default and set query to standby */
		INSERT INTO dbo.CampaignQueryStatus (
				QueryID,
				IsActive,
				IsStandby,
				Total,
				Available,
				Dials,
				Talks,
				AnsweringMachine,
				NoAnswer,
				Busy,
				OpInt,
				Drops,
				Failed,
				DateCreated,
				DateModified,
				IsCurrent,
				IsDefault
				
 				)
 			VALUES (
				@QueryID2, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, GETDATE(), GETDATE(), 0, 1
		 		)	
	END
GO
/****** Object:  StoredProcedure [dbo].[ClearCampaignQueryStats]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ClearCampaignQueryStats] 
		@CampaignQueryID bigint,
		@Total int , 
		@Available int 
AS 
	BEGIN 	
	UPDATE dbo.CampaignQueryStatus 
		SET 
			Total = @Total,
			Available = @Available,
			Dials = 0,
			Talks = 0,
			AnsweringMachine = 0,
			NoAnswer = 0,
			Busy = 0,
			OpInt = 0,
			Drops = 0,
			Failed = 0,
			DateModified = GETDATE()
		WHERE 	
			CampaignQueryID= @CampaignQueryID
	END
GO
/****** Object:  StoredProcedure [dbo].[SEL_CampaignScoreboardData]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SEL_CampaignScoreboardData]
AS
BEGIN
	DECLARE @ResultCodes INT
	DECLARE @DialingParameters INT
	DECLARE @OtherParameters INT
	DECLARE @DigitalizedRecording INT
	DECLARE @AgentStats INT
	DECLARE @QueryStatusActive INT
	DECLARE @QueryStatusInActive INT
	DECLARE @Scripts INT
	DECLARE @DilaerEngineStatus INT
	DECLARE @DilaerRecCount INT
	DECLARE @TrainingSchemeCount INT
	
	SELECT 
		@DilaerEngineStatus = COUNT(DialerActivityID) 
	FROM 
		[RainMakerMaster].[dbo].[DialerActivity]
	WHERE 
		DialerStartTime IS NOT NULL AND DialerStopTime IS NULL

	SET @ResultCodes = (SELECT ISNULL(COUNT(ResultCodeID),0) FROM ResultCode WHERE DateDeleted IS NULL)
	SET @DialingParameters = (SELECT ISNULL(COUNT(DailingParameterID),0) FROM DialingParameter)
	SET @OtherParameters = (SELECT ISNULL(COUNT(OtherParameterID),0) FROM OtherParameter)
	SET @DigitalizedRecording = (SELECT ISNULL(COUNT(DigitalizedRecordingID),0) FROM DigitalizedRecording)
	SET @AgentStats = (SELECT ISNULL(COUNT(StatID),0) FROM AgentStat WHERE LogOffDate is null)
	SET @QueryStatusActive = (SELECT ISNULL(COUNT(CampaignQueryID),0) FROM CampaignQueryStatus WHERE IsActive =1)
	SET @QueryStatusInActive = (SELECT ISNULL(COUNT(CampaignQueryID),0) FROM CampaignQueryStatus WHERE IsActive =0) 
	SET @DilaerRecCount = (SELECT SUM(ISNULL(Available,0)) FROM CampaignQueryStatus WHERE IsActive = 1)
	SET @Scripts = (SELECT ISNULL(COUNT(ScriptID),0) FROM Script WHERE ParentScriptID IS NULL)
	SET @TrainingSchemeCount = (SELECT ISNULL(COUNT(TrainingSchemeID),0) FROM TrainingSchemes)
	
	DECLARE @ScriptID INT
	
	SET @ScriptID = (SELECT TOP 1 ScriptID 
	FROM 
		Script, DialingParameter
	WHERE 
		ColdCallScriptID = ScriptID 
	OR 
		VerificationScriptID = ScriptID 
	OR 
		InboundScriptID = ScriptID)
	
	SELECT 	@ResultCodes AS ResultCodes,
		@DialingParameters AS DialingParameters,
		@OtherParameters AS OtherParameters,
		@DigitalizedRecording AS DigitalizedRecording,
		@AgentStats AS AgentStats,
		@QueryStatusActive AS ActiveQueries,
		@QueryStatusInActive AS StandByQueries,
		@Scripts AS Scripts,
		@TrainingSchemeCount AS TrainingSchemeCount,
		CASE WHEN ISNULL(@DilaerEngineStatus, 0) > 0 THEN 1 ELSE 0 END AS IsDialerRunning,
		CASE WHEN ISNULL(@ScriptID, 0) > 0 THEN 1 ELSE 0 END AS IsScriptAssigned,
		CASE WHEN ISNULL(@DilaerEngineStatus, 0) > 0 THEN @DilaerRecCount ELSE 0 END AS DialerRecInQueue
END
GO
/****** Object:  StoredProcedure [dbo].[Sel_CampaignQueryStatus_List]    Script Date: 01/21/2013 16:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Sel_CampaignQueryStatus_List]  	 
AS 
BEGIN		

	EXEC dbo.UPDATEQueryAvailableCounts
		
	SELECT
		CampaignQueryID,
		CampaignQueryStatus.QueryID, 
		Query.QueryName,
		IsActive,
		IsStandby, 
		Total, 
		Available, 
		Dials, 
		Talks, 
		AnsweringMachine, 
		NoAnswer, 
		Busy, 
		OpInt, 
		Drops, 
		Failed,
		dbo.Query.QueryCondition,
		dbo.Query.DateCreated, 
		dbo.CampaignQueryStatus.DateModified,
		ShowMessage,
		Priority
	FROM  
		dbo.CampaignQueryStatus
	INNER JOIN
		dbo.Query ON dbo.Query.QueryID = CampaignQueryStatus.QueryID	
		
END
GO
/****** Object:  Default [DF_QueryDetail_DateCreated]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[QueryDetail] ADD  CONSTRAINT [DF_QueryDetail_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_QueryDetail_DateModified]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[QueryDetail] ADD  CONSTRAINT [DF_QueryDetail_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_Script_DateCreated]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[Script] ADD  CONSTRAINT [DF_Script_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_Script_DateModified]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[Script] ADD  CONSTRAINT [DF_Script_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_ResultCode_Presentation]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_Presentation]  DEFAULT ((0)) FOR [Presentation]
GO
/****** Object:  Default [DF_ResultCode_Redialable]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_Redialable]  DEFAULT ((0)) FOR [Redialable]
GO
/****** Object:  Default [DF_ResultCode_RecycleInDays]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_RecycleInDays]  DEFAULT ((0)) FOR [RecycleInDays]
GO
/****** Object:  Default [DF_ResultCode_Lead]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_Lead]  DEFAULT ((0)) FOR [Lead]
GO
/****** Object:  Default [DF_ResultCode_MasterDNC]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_MasterDNC]  DEFAULT ((0)) FOR [MasterDNC]
GO
/****** Object:  Default [DF_ResultCode_NeverCall]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_NeverCall]  DEFAULT ((0)) FOR [NeverCall]
GO
/****** Object:  Default [DF_ResultCode_VerifyOnly]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_VerifyOnly]  DEFAULT ((0)) FOR [VerifyOnly]
GO
/****** Object:  Default [DF_ResultCode_CountAsLiveContact]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_CountAsLiveContact]  DEFAULT ((0)) FOR [CountAsLiveContact]
GO
/****** Object:  Default [DF_ResultCode_DialThroughAll]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_DialThroughAll]  DEFAULT ((0)) FOR [DialThroughAll]
GO
/****** Object:  Default [DF_ResultCode_ShowDeletedResultCodes]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_ShowDeletedResultCodes]  DEFAULT ((0)) FOR [ShowDeletedResultCodes]
GO
/****** Object:  Default [DF_ResultCode_DateCreated]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_ResultCode_DateModified]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[ResultCode] ADD  CONSTRAINT [DF_ResultCode_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_OtherParameter_AllowCallTransfer]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[OtherParameter] ADD  CONSTRAINT [DF_OtherParameter_AllowCallTransfer]  DEFAULT ((0)) FOR [CallTransfer]
GO
/****** Object:  Default [DF_OtherParameter_AllowManualDial]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[OtherParameter] ADD  CONSTRAINT [DF_OtherParameter_AllowManualDial]  DEFAULT ((0)) FOR [AllowManualDial]
GO
/****** Object:  Default [DF_OtherParameter_QueryStatisticsInPercent]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[OtherParameter] ADD  CONSTRAINT [DF_OtherParameter_QueryStatisticsInPercent]  DEFAULT ((1)) FOR [QueryStatisticsInPercent]
GO
/****** Object:  Default [DF_OtherParameter_DateCreated]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[OtherParameter] ADD  CONSTRAINT [DF_OtherParameter_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_OtherParameter_DateModified]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[OtherParameter] ADD  CONSTRAINT [DF_OtherParameter_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_DigitalizedRecording_EnableRecording]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DigitalizedRecording] ADD  CONSTRAINT [DF_DigitalizedRecording_EnableRecording]  DEFAULT ((1)) FOR [EnableRecording]
GO
/****** Object:  Default [DF_Table_1_StartWithABeep]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DigitalizedRecording] ADD  CONSTRAINT [DF_Table_1_StartWithABeep]  DEFAULT ((0)) FOR [EnableWithABeep]
GO
/****** Object:  Default [DF_DigitalizedRecording_StartWithABeep]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DigitalizedRecording] ADD  CONSTRAINT [DF_DigitalizedRecording_StartWithABeep]  DEFAULT ((0)) FOR [StartWithABeep]
GO
/****** Object:  Default [DF_DigitalizedRecording_HighQualityRecording]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DigitalizedRecording] ADD  CONSTRAINT [DF_DigitalizedRecording_HighQualityRecording]  DEFAULT ((0)) FOR [HighQualityRecording]
GO
/****** Object:  Default [DF_DigitalizedRecording_DateCreated]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DigitalizedRecording] ADD  CONSTRAINT [DF_DigitalizedRecording_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_DigitalizedRecording_DateModified]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DigitalizedRecording] ADD  CONSTRAINT [DF_DigitalizedRecording_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_DialingParameter_PhoneLineCount]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_PhoneLineCount]  DEFAULT ((0)) FOR [PhoneLineCount]
GO
/****** Object:  Default [DF_DialingParameter_AnsweringMachineDetection]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_AnsweringMachineDetection]  DEFAULT ((0)) FOR [AnsweringMachineDetection]
GO
/****** Object:  Default [DF_DialingParameter_AnsMachDetect]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_AnsMachDetect]  DEFAULT ((1)) FOR [AnsMachDetect]
GO
/****** Object:  Default [DF_DialingParameter_DateCreated]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_DialingParameter_DateModified]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_DialingParameter_ErrorRedialLapse]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_ErrorRedialLapse]  DEFAULT ((5)) FOR [ErrorRedialLapse]
GO
/****** Object:  Default [DF_DialingParameter_BusyRedialLapse]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_BusyRedialLapse]  DEFAULT ((5)) FOR [BusyRedialLapse]
GO
/****** Object:  Default [DF_DialingParameter_NoAnswerRedialLapse]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_NoAnswerRedialLapse]  DEFAULT ((5)) FOR [NoAnswerRedialLapse]
GO
/****** Object:  Default [DF_DialingParameter_ChannelsPerAgent]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_ChannelsPerAgent]  DEFAULT ((2)) FOR [ChannelsPerAgent]
GO
/****** Object:  Default [DF_DialingParameter_DefaultCallLapse]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_DefaultCallLapse]  DEFAULT ((40)) FOR [DefaultCallLapse]
GO
/****** Object:  Default [DF_DialingParameter_HumanMessageEnable]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_HumanMessageEnable]  DEFAULT ((0)) FOR [HumanMessageEnable]
GO
/****** Object:  Default [DF_DialingParameter_SilentCallMessageEnable]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[DialingParameter] ADD  CONSTRAINT [DF_DialingParameter_SilentCallMessageEnable]  DEFAULT ((0)) FOR [SilentCallMessageEnable]
GO
/****** Object:  Default [DF_Campaign_isManualDial]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_isManualDial]  DEFAULT ((0)) FOR [isManualDial]
GO
/****** Object:  Default [DF_Campaign_DateTimeofImport]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[Campaign] ADD  CONSTRAINT [DF_Campaign_DateTimeofImport]  DEFAULT (getdate()) FOR [DateTimeofImport]
GO
/****** Object:  Default [DF_Table_1_DateCreate]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[CampaignFields] ADD  CONSTRAINT [DF_Table_1_DateCreate]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_CampaignFields_DateModified]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[CampaignFields] ADD  CONSTRAINT [DF_CampaignFields_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_CampaignFields_IsDefault]    Script Date: 01/21/2013 16:45:49 ******/
ALTER TABLE [dbo].[CampaignFields] ADD  CONSTRAINT [DF_CampaignFields_IsDefault]  DEFAULT ((0)) FOR [IsDefault]
GO
/****** Object:  Default [DF_GlobalDialingParameters_DateCreated]    Script Date: 01/21/2013 16:45:50 ******/
ALTER TABLE [dbo].[GlobalDialingParameters] ADD  CONSTRAINT [DF_GlobalDialingParameters_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_GlobalDialingParameters_DateModified]    Script Date: 01/21/2013 16:45:50 ******/
ALTER TABLE [dbo].[GlobalDialingParameters] ADD  CONSTRAINT [DF_GlobalDialingParameters_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF_CampaignQueryStatus_IsActive]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CampaignQueryStatus] ADD  CONSTRAINT [DF_CampaignQueryStatus_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
/****** Object:  Default [DF_CampaignQueryStatus_Total]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CampaignQueryStatus] ADD  CONSTRAINT [DF_CampaignQueryStatus_Total]  DEFAULT ((0)) FOR [Total]
GO
/****** Object:  Default [DF_CampaignQueryStatus_Available]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CampaignQueryStatus] ADD  CONSTRAINT [DF_CampaignQueryStatus_Available]  DEFAULT ((0)) FOR [Available]
GO
/****** Object:  Default [DF_CampaignQueryStatus_Failed]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CampaignQueryStatus] ADD  CONSTRAINT [DF_CampaignQueryStatus_Failed]  DEFAULT ((0)) FOR [Failed]
GO
/****** Object:  Default [DF_CampaignQueryStatus_IsCurrent]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CampaignQueryStatus] ADD  CONSTRAINT [DF_CampaignQueryStatus_IsCurrent]  DEFAULT ((0)) FOR [IsCurrent]
GO
/****** Object:  Default [DF_CampaignQueryStatus_ShowMessage]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CampaignQueryStatus] ADD  CONSTRAINT [DF_CampaignQueryStatus_ShowMessage]  DEFAULT ((0)) FOR [ShowMessage]
GO
/****** Object:  Default [DF_AgentStat_TimeModified]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[AgentStat] ADD  CONSTRAINT [DF_AgentStat_TimeModified]  DEFAULT (((1)/(1))/(1900)) FOR [TimeModified]
GO
/****** Object:  Default [DF_CallList_IsBlocked]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CallList] ADD  CONSTRAINT [DF_CallList_IsBlocked]  DEFAULT ((0)) FOR [IsBlocked]
GO
/****** Object:  Default [DF_CallList_DateCreated]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CallList] ADD  CONSTRAINT [DF_CallList_DateCreated]  DEFAULT (getdate()) FOR [DateCreated]
GO
/****** Object:  Default [DF_CallList_DateModified]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CallList] ADD  CONSTRAINT [DF_CallList_DateModified]  DEFAULT (getdate()) FOR [DateModified]
GO
/****** Object:  Default [DF__CallList__Unique__3DB3258D]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CallList] ADD  CONSTRAINT [DF__CallList__Unique__3DB3258D]  DEFAULT ((0)) FOR [UniqueKey]
GO
/****** Object:  Default [DF_CallList_IsManualDial]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CallList] ADD  CONSTRAINT [DF_CallList_IsManualDial]  DEFAULT ((0)) FOR [IsManualDial]
GO
/****** Object:  ForeignKey [FK_CampaignQueryStatus_Query]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CampaignQueryStatus]  WITH CHECK ADD  CONSTRAINT [FK_CampaignQueryStatus_Query] FOREIGN KEY([QueryID])
REFERENCES [dbo].[Query] ([QueryID])
GO
ALTER TABLE [dbo].[CampaignQueryStatus] CHECK CONSTRAINT [FK_CampaignQueryStatus_Query]
GO
/****** Object:  ForeignKey [FK_AgentStat_ResultCode]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[AgentStat]  WITH CHECK ADD  CONSTRAINT [FK_AgentStat_ResultCode] FOREIGN KEY([LastResultCodeID])
REFERENCES [dbo].[ResultCode] ([ResultCodeID])
GO
ALTER TABLE [dbo].[AgentStat] CHECK CONSTRAINT [FK_AgentStat_ResultCode]
GO
/****** Object:  ForeignKey [FK_CallList_ResultCode]    Script Date: 01/21/2013 16:45:51 ******/
ALTER TABLE [dbo].[CallList]  WITH CHECK ADD  CONSTRAINT [FK_CallList_ResultCode] FOREIGN KEY([ResultCodeID])
REFERENCES [dbo].[ResultCode] ([ResultCodeID])
GO
ALTER TABLE [dbo].[CallList] CHECK CONSTRAINT [FK_CallList_ResultCode]
GO
ALTER TABLE [dbo].[Query] ADD  CONSTRAINT [DF_Query_isDefault]  DEFAULT ((0)) FOR [isDefault]
GO
ALTER TABLE [dbo].[QueryDetail] ADD  CONSTRAINT [DF_QueryDetail_isDefault]  DEFAULT ((0)) FOR [isDefault]
GO
ALTER TABLE [dbo].[CampaignQueryStatus] ADD  CONSTRAINT [DF_CampaignQueryStatus_isDefault]  DEFAULT ((0)) FOR [isDefault]
