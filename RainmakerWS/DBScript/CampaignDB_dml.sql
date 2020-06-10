insert into resultcode 
	values ('Answering Machine', 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,null,getdate(),getdate())
insert into resultcode 
	values ('Busy', 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,null,getdate(),getdate())
insert into resultcode 
	values ('Operator Intercept', 0, 0, 0, 0, 0, 1, 1, 0, 0, 0,null,getdate(),getdate())
insert into resultcode 
	values ('Dropped', 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,null,getdate(),getdate())
insert into resultcode 
	values ('No Answer', 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,null,getdate(),getdate())
insert into resultcode 
	values ('Scheduled Callback', 0, 1, 0, 0, 0, 0, 0, 0, 0, 0,null,getdate(),getdate())
insert into resultcode 
	values ('Transferred to Agent', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null,getdate(),getdate())
insert into resultcode 
	values ('Transferred to Dialer', 0, 1, 0, 0, 0, 0, 0, 0, 0, 0, null,getdate(),getdate())
insert into resultcode 
	values ('Unmanned Live Contact', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null,getdate(),getdate())
insert into resultcode 
	values ('Inbound Abandoned by Agent', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null,getdate(),getdate())
insert into resultcode 
	values ('Inbound abandoned by Caller', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null,getdate(),getdate())
insert into resultcode 
	values ('Failed', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,null,getdate(),getdate())
insert into resultcode 
	values ('Unmanned Transferred to Answering Machine', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null,getdate(),getdate())
insert into resultcode 
values ('Transferred Offsite', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, null,getdate(),getdate())
GO

INSERT INTO DialingParameter 
(
	PhoneLineCount, 
	DropRatePercent, 
	RingSeconds, 
	MinimumDelayBetweenCalls, 
	DialingMode,
	AnsweringMachineDetection,
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
	SilentCallMessage, 
	SevenDigitPrefix,
	TenDigitPrefix,
	SevenDigitSuffix,
	TenDigitSuffix
)  
VALUES 
(
	5,3,23,0,1,0,1,1,1,5,5,5,
	'2012-01-01 09:00:00.000', 
	'2012-01-01 12:00:00.000', 
	'2012-01-01 12:00:00.000', 
	'2012-01-01 21:00:00.000',
	0, getdate(), getdate(),
	0,0,0,2,
	40, null, 0,null,0,null,
	'', '', '', ''
)
GO

INSERT INTO CampaignFields VALUES('FIRSTNAME',1,255,getdate(),getdate(),1, 0)
INSERT INTO CampaignFields VALUES('LASTNAME',1,255,getdate(),getdate(),1, 0)
INSERT INTO CampaignFields VALUES('ADDRESS',1,255,getdate(),getdate(),1, 0)
INSERT INTO CampaignFields VALUES('ADDRESS2',1,255,getdate(),getdate(),1, 0)
INSERT INTO CampaignFields VALUES('CITY',1,255,getdate(),getdate(),1, 0)
INSERT INTO CampaignFields VALUES('STATE',1,255,getdate(),getdate(),1, 0)
INSERT INTO CampaignFields VALUES('ZIP',1,255,getdate(),getdate(),1, 0)
INSERT INTO CampaignFields VALUES('PHONENUM',1,10,getdate(),getdate(),1, 0)


GO

exec CreateDefaultQueries
GO
