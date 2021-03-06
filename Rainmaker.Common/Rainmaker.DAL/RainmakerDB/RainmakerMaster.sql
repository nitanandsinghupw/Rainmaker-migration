/****** Object:  Database RainmakerMaster    Script Date: 11/28/2011 3:26:39 PM ******/
IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'RainmakerMaster')
	DROP DATABASE [RainmakerMaster]
GO

CREATE DATABASE [RainmakerMaster]  ON (NAME = N'RainmakerMaster', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\RainmakerMaster.mdf' , SIZE = 4, FILEGROWTH = 10%) LOG ON (NAME = N'RainmakerMaster_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL\data\RainmakerMaster_log.LDF' , SIZE = 99, FILEGROWTH = 10%)
 COLLATE SQL_Latin1_General_CP1_CI_AS
GO

exec sp_dboption N'RainmakerMaster', N'autoclose', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'bulkcopy', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'trunc. log', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'torn page detection', N'true'
GO

exec sp_dboption N'RainmakerMaster', N'read only', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'dbo use', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'single', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'autoshrink', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'ANSI null default', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'recursive triggers', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'ANSI nulls', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'concat null yields null', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'cursor close on commit', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'default to local cursor', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'quoted identifier', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'ANSI warnings', N'false'
GO

exec sp_dboption N'RainmakerMaster', N'auto create statistics', N'true'
GO

exec sp_dboption N'RainmakerMaster', N'auto update statistics', N'true'
GO

use [RainmakerMaster]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_AgentActivity_Agent]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[AgentActivity] DROP CONSTRAINT FK_AgentActivity_Agent
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_AreaCodeRule_Agent]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[AreaCodeRule] DROP CONSTRAINT FK_AreaCodeRule_Agent
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_AgentActivity_LoginStatus]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[AgentActivity] DROP CONSTRAINT FK_AgentActivity_LoginStatus
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_AgentActivity_Campaign]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[AgentActivity] DROP CONSTRAINT FK_AgentActivity_Campaign
GO

/****** Object:  User Defined Function dbo.CampaignAgentStationInfo    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CampaignAgentStationInfo]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[CampaignAgentStationInfo]
GO

/****** Object:  Stored Procedure dbo.DEL_Agent    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DEL_Agent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DEL_Agent]
GO

/****** Object:  Stored Procedure dbo.Get_AgentActivity_ById    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Get_AgentActivity_ById]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Get_AgentActivity_ById]
GO

/****** Object:  Stored Procedure dbo.InsGet_AgentActivity    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsGet_AgentActivity]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsGet_AgentActivity]
GO

/****** Object:  Stored Procedure dbo.InsUpd_AreaCodeRule    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsUpd_AreaCodeRule]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsUpd_AreaCodeRule]
GO

/****** Object:  Stored Procedure dbo.Sel_AreaCodeRule_ByAgentID    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_AreaCodeRule_ByAgentID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_AreaCodeRule_ByAgentID]
GO

/****** Object:  Stored Procedure dbo.Sel_LoggedInAgents    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_LoggedInAgents]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_LoggedInAgents]
GO

/****** Object:  Stored Procedure dbo.Upd_AgentActivity    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Upd_AgentActivity]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Upd_AgentActivity]
GO

/****** Object:  Stored Procedure dbo.Upd_AgentLogOut    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Upd_AgentLogOut]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Upd_AgentLogOut]
GO

/****** Object:  Stored Procedure dbo.CLONE_CAMPAIGN    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CLONE_CAMPAIGN]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CLONE_CAMPAIGN]
GO

/****** Object:  Stored Procedure dbo.DEL_AgentStation    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DEL_AgentStation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DEL_AgentStation]
GO

/****** Object:  Stored Procedure dbo.DEL_AreaCode    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DEL_AreaCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DEL_AreaCode]
GO

/****** Object:  Stored Procedure dbo.DEL_Campaign    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DEL_Campaign]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DEL_Campaign]
GO

/****** Object:  Stored Procedure dbo.GetCampaignStatus    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCampaignStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCampaignStatus]
GO

/****** Object:  Stored Procedure dbo.InsUpd_Agent    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsUpd_Agent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsUpd_Agent]
GO

/****** Object:  Stored Procedure dbo.InsUpd_AgentCampaignMap    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsUpd_AgentCampaignMap]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsUpd_AgentCampaignMap]
GO

/****** Object:  Stored Procedure dbo.InsUpd_AgentCampaignMap2    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsUpd_AgentCampaignMap2]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsUpd_AgentCampaignMap2]
GO

/****** Object:  Stored Procedure dbo.InsUpd_AgentStation    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsUpd_AgentStation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsUpd_AgentStation]
GO

/****** Object:  Stored Procedure dbo.InsUpd_AreaCode    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsUpd_AreaCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsUpd_AreaCode]
GO

/****** Object:  Stored Procedure dbo.InsUpd_Campaign    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsUpd_Campaign]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsUpd_Campaign]
GO

/****** Object:  Stored Procedure dbo.InsUpd_GlobalDialingParameters    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsUpd_GlobalDialingParameters]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsUpd_GlobalDialingParameters]
GO

/****** Object:  Stored Procedure dbo.Ins_AdminRequest    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ins_AdminRequest]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Ins_AdminRequest]
GO

/****** Object:  Stored Procedure dbo.Ins_DialerActivity    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ins_DialerActivity]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Ins_DialerActivity]
GO

/****** Object:  Stored Procedure dbo.IsDialerRunning    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IsDialerRunning]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[IsDialerRunning]
GO

/****** Object:  Stored Procedure dbo.ResetCampaignsToIdle    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ResetCampaignsToIdle]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ResetCampaignsToIdle]
GO

/****** Object:  Stored Procedure dbo.Sel_ActiveCampaign_List    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_ActiveCampaign_List]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_ActiveCampaign_List]
GO

/****** Object:  Stored Procedure dbo.Sel_AdminRequests    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_AdminRequests]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_AdminRequests]
GO

/****** Object:  Stored Procedure dbo.Sel_AgentCampaignMap_Dtl    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_AgentCampaignMap_Dtl]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_AgentCampaignMap_Dtl]
GO

/****** Object:  Stored Procedure dbo.Sel_Agent_Dtl    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_Agent_Dtl]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_Agent_Dtl]
GO

/****** Object:  Stored Procedure dbo.Sel_Agent_List    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_Agent_List]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_Agent_List]
GO

/****** Object:  Stored Procedure dbo.Sel_AreaCode    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_AreaCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_AreaCode]
GO

/****** Object:  Stored Procedure dbo.Sel_Campaign_Dtl    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_Campaign_Dtl]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_Campaign_Dtl]
GO

/****** Object:  Stored Procedure dbo.Sel_Campaign_List    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_Campaign_List]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_Campaign_List]
GO

/****** Object:  Stored Procedure dbo.Sel_Campaign_ListByCampaignID    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_Campaign_ListByCampaignID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_Campaign_ListByCampaignID]
GO

/****** Object:  Stored Procedure dbo.Sel_FieldTypes_List    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_FieldTypes_List]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_FieldTypes_List]
GO

/****** Object:  Stored Procedure dbo.Sel_GlobalDialingParameters_List    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_GlobalDialingParameters_List]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_GlobalDialingParameters_List]
GO

/****** Object:  Stored Procedure dbo.Sel_PhoneLinesInUse    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_PhoneLinesInUse]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_PhoneLinesInUse]
GO

/****** Object:  Stored Procedure dbo.Sel_Station_Dtl    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_Station_Dtl]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_Station_Dtl]
GO

/****** Object:  Stored Procedure dbo.Sel_Station_List    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_Station_List]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_Station_List]
GO

/****** Object:  Stored Procedure dbo.ShutdownAllCampaigns    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ShutdownAllCampaigns]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ShutdownAllCampaigns]
GO

/****** Object:  Stored Procedure dbo.ToggleAgentReset    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ToggleAgentReset]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ToggleAgentReset]
GO

/****** Object:  Stored Procedure dbo.Upd_AdminRequestStatus    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Upd_AdminRequestStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Upd_AdminRequestStatus]
GO

/****** Object:  Stored Procedure dbo.Upd_CampaignQuery_Status    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Upd_CampaignQuery_Status]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Upd_CampaignQuery_Status]
GO

/****** Object:  Stored Procedure dbo.Upd_CampaignStatus    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Upd_CampaignStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Upd_CampaignStatus]
GO

/****** Object:  Stored Procedure dbo.Upd_Campaign_DialStatus    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Upd_Campaign_DialStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Upd_Campaign_DialStatus]
GO

/****** Object:  Stored Procedure dbo.Upd_DialerStart    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Upd_DialerStart]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Upd_DialerStart]
GO

/****** Object:  Stored Procedure dbo.Upd_DialerStop    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Upd_DialerStop]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Upd_DialerStop]
GO

/****** Object:  Stored Procedure dbo.p_AgentInfoByLoginNameGet    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[p_AgentInfoByLoginNameGet]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[p_AgentInfoByLoginNameGet]
GO

/****** Object:  Stored Procedure dbo.FetchUDColums    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FetchUDColums]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[FetchUDColums]
GO

/****** Object:  Stored Procedure dbo.FetchUDColumsList    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FetchUDColumsList]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[FetchUDColumsList]
GO

/****** Object:  Stored Procedure dbo.GenerateInsUpdateScript    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GenerateInsUpdateScript]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GenerateInsUpdateScript]
GO

/****** Object:  Stored Procedure dbo.GenerateSelectDtlScript    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GenerateSelectDtlScript]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GenerateSelectDtlScript]
GO

/****** Object:  Stored Procedure dbo.GenerateSelectScript    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GenerateSelectScript]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GenerateSelectScript]
GO

/****** Object:  Stored Procedure dbo.SearchForStringInSPs    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SearchForStringInSPs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SearchForStringInSPs]
GO

/****** Object:  Stored Procedure dbo.Sel_LoginStatus_List    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Sel_LoginStatus_List]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Sel_LoginStatus_List]
GO

/****** Object:  Table [dbo].[AgentActivity]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AgentActivity]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AgentActivity]
GO

/****** Object:  Table [dbo].[AreaCodeRule]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AreaCodeRule]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AreaCodeRule]
GO

/****** Object:  Table [dbo].[AdminRequests]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AdminRequests]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AdminRequests]
GO

/****** Object:  Table [dbo].[Agent]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Agent]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Agent]
GO

/****** Object:  Table [dbo].[AgentCampaignMap]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AgentCampaignMap]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AgentCampaignMap]
GO

/****** Object:  Table [dbo].[AgentStation]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AgentStation]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AgentStation]
GO

/****** Object:  Table [dbo].[AgentStatus]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AgentStatus]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AgentStatus]
GO

/****** Object:  Table [dbo].[AreaCode]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AreaCode]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AreaCode]
GO

/****** Object:  Table [dbo].[Campaign]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Campaign]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Campaign]
GO

/****** Object:  Table [dbo].[DataManagerColumn]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DataManagerColumn]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[DataManagerColumn]
GO

/****** Object:  Table [dbo].[DataManagerOption]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DataManagerOption]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[DataManagerOption]
GO

/****** Object:  Table [dbo].[DataManagerViews]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DataManagerViews]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[DataManagerViews]
GO

/****** Object:  Table [dbo].[DialerActivity]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DialerActivity]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[DialerActivity]
GO

/****** Object:  Table [dbo].[FieldTypes]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FieldTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[FieldTypes]
GO

/****** Object:  Table [dbo].[GlobalDialingParameters]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GlobalDialingParameters]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[GlobalDialingParameters]
GO

/****** Object:  Table [dbo].[Status]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Status]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Status]
GO

/****** Object:  Table [dbo].[TimeZone]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[TimeZone]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[TimeZone]
GO

/****** Object:  Table [dbo].[evcategories]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[evcategories]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[evcategories]
GO

/****** Object:  Table [dbo].[evevents]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[evevents]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[evevents]
GO

/****** Object:  Table [dbo].[evtellfriend]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[evtellfriend]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[evtellfriend]
GO

/****** Object:  Table [dbo].[evusers]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[evusers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[evusers]
GO

/****** Object:  Table [dbo].[jobsjobs]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[jobsjobs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[jobsjobs]
GO

/****** Object:  Table [dbo].[jobsjobtype]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[jobsjobtype]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[jobsjobtype]
GO

/****** Object:  Table [dbo].[jobsstate]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[jobsstate]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[jobsstate]
GO

/****** Object:  Table [dbo].[jobsusers]    Script Date: 11/28/2011 3:26:45 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[jobsusers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[jobsusers]
GO

/****** Object:  User dbo    Script Date: 11/28/2011 3:26:39 PM ******/
/****** Object:  Table [dbo].[AdminRequests]    Script Date: 11/28/2011 3:26:47 PM ******/
CREATE TABLE [dbo].[AdminRequests] (
	[RequestID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[RequestType] [int] NOT NULL ,
	[DateTimeSubmitted] [datetime] NOT NULL ,
	[RequestData] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[RequestStatus] [int] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Agent]    Script Date: 11/28/2011 3:26:48 PM ******/
CREATE TABLE [dbo].[Agent] (
	[AgentID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[AgentName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[LoginName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Password] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[IsAdministrator] [bit] NOT NULL ,
	[AllowManualDial] [bit] NOT NULL ,
	[VerificationAgent] [bit] NOT NULL ,
	[InBoundAgent] [bit] NOT NULL ,
	[PhoneNumber] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[IsDefault] [bit] NOT NULL ,
	[IsDeleted] [bit] NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[DateModified] [datetime] NOT NULL ,
	[IsReset] [bit] NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AgentCampaignMap]    Script Date: 11/28/2011 3:26:48 PM ******/
CREATE TABLE [dbo].[AgentCampaignMap] (
	[AgentCampaignMapID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[AgentID] [bigint] NOT NULL ,
	[CampaignID] [bigint] NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[DateModified] [datetime] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AgentStation]    Script Date: 11/28/2011 3:26:48 PM ******/
CREATE TABLE [dbo].[AgentStation] (
	[StationID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[StationIP] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[StationNumber] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[IsDeleted] [bit] NOT NULL ,
	[AllwaysOffHook] [bit] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AgentStatus]    Script Date: 11/28/2011 3:26:49 PM ******/
CREATE TABLE [dbo].[AgentStatus] (
	[AgentStatusID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[Status] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[DateModified] [datetime] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AreaCode]    Script Date: 11/28/2011 3:26:49 PM ******/
CREATE TABLE [dbo].[AreaCode] (
	[AreaCodeID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[AreaCode] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Prefix] [varchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[IsDeleted] [bit] NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[DateModified] [datetime] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Campaign]    Script Date: 11/28/2011 3:26:49 PM ******/
CREATE TABLE [dbo].[Campaign] (
	[CampaignID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[Description] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[ShortDescription] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[FundRaiserDataTracking] [bit] NOT NULL ,
	[RecordLevelCallHistory] [bit] NOT NULL ,
	[OnsiteTransfer] [bit] NOT NULL ,
	[EnableAgentTraining] [bit] NULL ,
	[AllowDuplicatePhones] [bit] NOT NULL ,
	[CampaignDBConnString] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[FlushCallQueueOnIdle] [bit] NOT NULL ,
	[StatusID] [bigint] NULL ,
	[DialAllNumbers] [bit] NOT NULL ,
	[IsDeleted] [bit] NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[DateModified] [datetime] NOT NULL ,
	[OutboundCallerID] [varchar] (20) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[StartTime] [datetime] NULL ,
	[StopTime] [datetime] NULL ,
	[DuplicateRule] [char] (1) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Allow7DigitNums] [bit] NOT NULL ,
	[Allow10DigitNums] [bit] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DataManagerColumn]    Script Date: 11/28/2011 3:26:49 PM ******/
CREATE TABLE [dbo].[DataManagerColumn] (
	[width] [bigint] NOT NULL ,
	[name] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[hidden] [bigint] NOT NULL ,
	[DataManagerOptionId] [bigint] NOT NULL ,
	[id] [bigint] IDENTITY (1, 1) NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DataManagerOption]    Script Date: 11/28/2011 3:26:49 PM ******/
CREATE TABLE [dbo].[DataManagerOption] (
	[campaignid] [bigint] NOT NULL ,
	[queryid] [bigint] NOT NULL ,
	[rowlimit] [bigint] NOT NULL ,
	[sortcolumn] [bigint] NOT NULL ,
	[sortactive] [bigint] NOT NULL ,
	[description] [varchar] (500) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[sortdirection] [bigint] NOT NULL ,
	[id] [bigint] IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
	[showcsvheaders] [int] NOT NULL ,
	[IsNamedQuery] [int] NOT NULL ,
	[name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DataManagerViews]    Script Date: 11/28/2011 3:26:49 PM ******/
CREATE TABLE [dbo].[DataManagerViews] (
	[ViewID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[AgentID] [bigint] NOT NULL ,
	[ViewName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[FieldList] [varchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[RecordsPerPage] [int] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[DialerActivity]    Script Date: 11/28/2011 3:26:49 PM ******/
CREATE TABLE [dbo].[DialerActivity] (
	[DialerActivityID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[ConnectTime] [datetime] NOT NULL ,
	[DisconnectTime] [datetime] NULL ,
	[DialerStartTime] [datetime] NULL ,
	[DialerStopTime] [datetime] NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[FieldTypes]    Script Date: 11/28/2011 3:26:50 PM ******/
CREATE TABLE [dbo].[FieldTypes] (
	[FieldTypeID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[FieldType] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DBFieldType] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[DateModified] [datetime] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[GlobalDialingParameters]    Script Date: 11/28/2011 3:26:50 PM ******/
CREATE TABLE [dbo].[GlobalDialingParameters] (
	[GlobalDialingID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[Prefix] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Suffix] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[DateModified] [datetime] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Status]    Script Date: 11/28/2011 3:26:50 PM ******/
CREATE TABLE [dbo].[Status] (
	[StatusID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[StatusName] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[DateModified] [datetime] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[TimeZone]    Script Date: 11/28/2011 3:26:50 PM ******/
CREATE TABLE [dbo].[TimeZone] (
	[TimeZoneID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[TimeZone] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Offset] [numeric](18, 2) NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[DateModified] [datetime] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[evcategories]    Script Date: 11/28/2011 3:26:50 PM ******/
CREATE TABLE [dbo].[evcategories] (
	[Category] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[evevents]    Script Date: 11/28/2011 3:26:50 PM ******/
CREATE TABLE [dbo].[evevents] (
	[EventID] [int] IDENTITY (1, 1) NOT NULL ,
	[Category] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Subject] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Description] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[FromDate] [datetime] NULL ,
	[ToDate] [datetime] NULL ,
	[FromTime] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ToTime] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Location] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Price] [money] NULL ,
	[Map] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Note] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Image] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[evtellfriend]    Script Date: 11/28/2011 3:26:50 PM ******/
CREATE TABLE [dbo].[evtellfriend] (
	[FromName] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[EmailAddress1] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[EmailAddress2] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[EmailAddress3] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[EmailAddress4] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[EmailAddress5] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[EmailSubject] [nvarchar] (250) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[EmailBody] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[evusers]    Script Date: 11/28/2011 3:26:50 PM ******/
CREATE TABLE [dbo].[evusers] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Group] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[jobsjobs]    Script Date: 11/28/2011 3:26:50 PM ******/
CREATE TABLE [dbo].[jobsjobs] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Company] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Title] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[TypeID] [int] NULL ,
	[Description] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[HowToApply] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Zip] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[City] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[state] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Country] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Website] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[LogoURL] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[DateAdded] [datetime] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[jobsjobtype]    Script Date: 11/28/2011 3:26:51 PM ******/
CREATE TABLE [dbo].[jobsjobtype] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[sType] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[jobsstate]    Script Date: 11/28/2011 3:26:51 PM ******/
CREATE TABLE [dbo].[jobsstate] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[State] [nvarchar] (10) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[jobsusers]    Script Date: 11/28/2011 3:26:51 PM ******/
CREATE TABLE [dbo].[jobsusers] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Username] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Password] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Group] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Email] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AgentActivity]    Script Date: 11/28/2011 3:26:51 PM ******/
CREATE TABLE [dbo].[AgentActivity] (
	[AgentActivityID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[AgentID] [bigint] NOT NULL ,
	[AgentStatusID] [bigint] NOT NULL ,
	[AgentReceiptModeID] [bigint] NULL ,
	[CampaignID] [bigint] NULL ,
	[LoginTime] [datetime] NOT NULL ,
	[LogoutTime] [datetime] NULL ,
	[IsDeleted] [bit] NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[DateModified] [datetime] NOT NULL ,
	[StationIP] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[StationHostName] [varchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[AreaCodeRule]    Script Date: 11/28/2011 3:26:51 PM ******/
CREATE TABLE [dbo].[AreaCodeRule] (
	[AreaCodeRuleID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[AgentID] [bigint] NOT NULL ,
	[AreaCodeID] [bigint] NULL ,
	[LikeDialing] [bit] NOT NULL ,
	[LikeDialingOption] [bit] NULL ,
	[CustomeDialing] [bit] NULL ,
	[IsSevenDigit] [bit] NULL ,
	[IsTenDigit] [bit] NULL ,
	[IntraLataDialing] [bit] NULL ,
	[IntraLataDialingAreaCode] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ILDIsTenDigit] [bit] NULL ,
	[ILDElevenDigit] [bit] NULL ,
	[ReplaceAreaCode] [varchar] (5) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[LongDistanceDialing] [bit] NULL ,
	[IsDeleted] [bit] NOT NULL ,
	[DateCreated] [datetime] NOT NULL ,
	[DateModified] [datetime] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Agent] WITH NOCHECK ADD 
	CONSTRAINT [PK_Agent] PRIMARY KEY  CLUSTERED 
	(
		[AgentID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[AgentCampaignMap] WITH NOCHECK ADD 
	CONSTRAINT [PK_AgentCampaignMap] PRIMARY KEY  CLUSTERED 
	(
		[AgentCampaignMapID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[AgentStation] WITH NOCHECK ADD 
	CONSTRAINT [PK_AgentStation] PRIMARY KEY  CLUSTERED 
	(
		[StationID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[AgentStatus] WITH NOCHECK ADD 
	CONSTRAINT [PK_LoginStatus] PRIMARY KEY  CLUSTERED 
	(
		[AgentStatusID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[AreaCode] WITH NOCHECK ADD 
	CONSTRAINT [PK_AreaCode] PRIMARY KEY  CLUSTERED 
	(
		[AreaCodeID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Campaign] WITH NOCHECK ADD 
	CONSTRAINT [PK_Campaign] PRIMARY KEY  CLUSTERED 
	(
		[CampaignID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[DataManagerColumn] WITH NOCHECK ADD 
	CONSTRAINT [PK_DataManagerColumn] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[DataManagerOption] WITH NOCHECK ADD 
	CONSTRAINT [PK_DataManagerOption] PRIMARY KEY  CLUSTERED 
	(
		[id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[DataManagerViews] WITH NOCHECK ADD 
	CONSTRAINT [PK_DataManagerViews] PRIMARY KEY  CLUSTERED 
	(
		[ViewID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[DialerActivity] WITH NOCHECK ADD 
	CONSTRAINT [PK_DialerActivity] PRIMARY KEY  CLUSTERED 
	(
		[DialerActivityID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[FieldTypes] WITH NOCHECK ADD 
	CONSTRAINT [PK_FieldTypes] PRIMARY KEY  CLUSTERED 
	(
		[FieldTypeID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[GlobalDialingParameters] WITH NOCHECK ADD 
	CONSTRAINT [PK_GlobalDialingParamters] PRIMARY KEY  CLUSTERED 
	(
		[GlobalDialingID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Status] WITH NOCHECK ADD 
	CONSTRAINT [PK_Status] PRIMARY KEY  CLUSTERED 
	(
		[StatusID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[TimeZone] WITH NOCHECK ADD 
	CONSTRAINT [PK_TimeZone] PRIMARY KEY  CLUSTERED 
	(
		[TimeZoneID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[AgentActivity] WITH NOCHECK ADD 
	CONSTRAINT [PK_AgentActivity] PRIMARY KEY  CLUSTERED 
	(
		[AgentActivityID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[AreaCodeRule] WITH NOCHECK ADD 
	CONSTRAINT [PK_AreaCodeRule] PRIMARY KEY  CLUSTERED 
	(
		[AreaCodeRuleID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Agent] WITH NOCHECK ADD 
	CONSTRAINT [DF_Agent_IsAdministrator] DEFAULT (0) FOR [IsAdministrator],
	CONSTRAINT [DF_Agent_AllowManualDial] DEFAULT (0) FOR [AllowManualDial],
	CONSTRAINT [DF_Agent_VerificationAgent] DEFAULT (0) FOR [VerificationAgent],
	CONSTRAINT [DF_Agent_InBoundAgent] DEFAULT (0) FOR [InBoundAgent],
	CONSTRAINT [DF_Agent_PhoneNumber] DEFAULT (123456) FOR [PhoneNumber],
	CONSTRAINT [DF_Agent_IsDefault] DEFAULT (0) FOR [IsDefault],
	CONSTRAINT [DF_Agent_IsDeleted] DEFAULT (0) FOR [IsDeleted],
	CONSTRAINT [DF_Agent_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_Agent_DateModified] DEFAULT (getdate()) FOR [DateModified],
	CONSTRAINT [DF_Agent_IsReset] DEFAULT (0) FOR [IsReset],
	CONSTRAINT [IX_Agent_1] UNIQUE  NONCLUSTERED 
	(
		[LoginName]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[AgentCampaignMap] WITH NOCHECK ADD 
	CONSTRAINT [DF_AgentCampaignMap_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_AgentCampaignMap_DateModified] DEFAULT (getdate()) FOR [DateModified]
GO

ALTER TABLE [dbo].[AgentStation] WITH NOCHECK ADD 
	CONSTRAINT [DF_AgentStation_IsDeleted] DEFAULT (0) FOR [IsDeleted],
	CONSTRAINT [DF_AgentStation_AllwaysOffHook] DEFAULT (0) FOR [AllwaysOffHook]
GO

ALTER TABLE [dbo].[AgentStatus] WITH NOCHECK ADD 
	CONSTRAINT [DF_LoginStatus_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_LoginStatus_DateModified] DEFAULT (getdate()) FOR [DateModified]
GO

ALTER TABLE [dbo].[AreaCode] WITH NOCHECK ADD 
	CONSTRAINT [DF_AreaCode_IsDeleted] DEFAULT (0) FOR [IsDeleted],
	CONSTRAINT [DF_AreaCode_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_AreaCode_DateModified] DEFAULT (getdate()) FOR [DateModified]
GO

ALTER TABLE [dbo].[Campaign] WITH NOCHECK ADD 
	CONSTRAINT [DF_Campaign_FundRaiserDataTracking] DEFAULT (0) FOR [FundRaiserDataTracking],
	CONSTRAINT [DF_Campaign_RecordLevelCallHistory] DEFAULT (0) FOR [RecordLevelCallHistory],
	CONSTRAINT [DF_Campaign_OnsiteTransfer] DEFAULT (0) FOR [OnsiteTransfer],
	CONSTRAINT [DF_Campaign_AllowDuplicatePhones] DEFAULT (0) FOR [AllowDuplicatePhones],
	CONSTRAINT [DF_Campaign_FlushCallQueueOnIdel] DEFAULT (0) FOR [FlushCallQueueOnIdle],
	CONSTRAINT [DF_Campaign_DialAllNumbers] DEFAULT (0) FOR [DialAllNumbers],
	CONSTRAINT [DF_Campaign_IsDeleted] DEFAULT (0) FOR [IsDeleted],
	CONSTRAINT [DF_Campaign_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_Campaign_DateModified] DEFAULT (getdate()) FOR [DateModified],
	CONSTRAINT [DF_Campaign_OutboundCallerID] DEFAULT (7202838475) FOR [OutboundCallerID],
	CONSTRAINT [DF_Campaign_DuplicateRule] DEFAULT ('I') FOR [DuplicateRule],
	CONSTRAINT [DF_Campaign_Allow7DigitNums] DEFAULT (0) FOR [Allow7DigitNums],
	CONSTRAINT [DF_Campaign_Allow10DigitNums] DEFAULT (1) FOR [Allow10DigitNums]
GO

ALTER TABLE [dbo].[DataManagerOption] WITH NOCHECK ADD 
	CONSTRAINT [DF_DataManagerOption_showcsvheaders] DEFAULT (0) FOR [showcsvheaders],
	CONSTRAINT [DF_DataManagerOption_is_named_query] DEFAULT (0) FOR [IsNamedQuery],
	CONSTRAINT [DF_DataManagerOption_name] DEFAULT ('') FOR [name]
GO

ALTER TABLE [dbo].[FieldTypes] WITH NOCHECK ADD 
	CONSTRAINT [DF_FieldTypes_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_FieldTypes_DateModified] DEFAULT (getdate()) FOR [DateModified]
GO

ALTER TABLE [dbo].[GlobalDialingParameters] WITH NOCHECK ADD 
	CONSTRAINT [DF_GlobalDialingParamters_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_GlobalDialingParamters_DateModified] DEFAULT (getdate()) FOR [DateModified]
GO

ALTER TABLE [dbo].[Status] WITH NOCHECK ADD 
	CONSTRAINT [DF_Status_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_Status_DateModified] DEFAULT (getdate()) FOR [DateModified]
GO

ALTER TABLE [dbo].[TimeZone] WITH NOCHECK ADD 
	CONSTRAINT [DF_TimeZone_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_TimeZone_DateModified] DEFAULT (getdate()) FOR [DateModified]
GO

ALTER TABLE [dbo].[evevents] WITH NOCHECK ADD 
	CONSTRAINT [DF__evevents__Price__1AD3FDA4] DEFAULT (0) FOR [Price],
	 PRIMARY KEY  NONCLUSTERED 
	(
		[EventID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[evusers] WITH NOCHECK ADD 
	 PRIMARY KEY  NONCLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[jobsjobs] WITH NOCHECK ADD 
	CONSTRAINT [DF__jobsjobs__TypeID__2180FB33] DEFAULT (0) FOR [TypeID],
	 PRIMARY KEY  NONCLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[jobsjobtype] WITH NOCHECK ADD 
	 PRIMARY KEY  NONCLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[jobsstate] WITH NOCHECK ADD 
	 PRIMARY KEY  NONCLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[jobsusers] WITH NOCHECK ADD 
	 PRIMARY KEY  NONCLUSTERED 
	(
		[ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[AgentActivity] WITH NOCHECK ADD 
	CONSTRAINT [DF_AgentActivity_IsDeleted] DEFAULT (0) FOR [IsDeleted],
	CONSTRAINT [DF_AgentActivity_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_AgentActivity_DateModified] DEFAULT (getdate()) FOR [DateModified],
	CONSTRAINT [DF_AgentActivity_StationIP] DEFAULT ('') FOR [StationIP],
	CONSTRAINT [DF_AgentActivity_StationHostName] DEFAULT ('') FOR [StationHostName]
GO

ALTER TABLE [dbo].[AreaCodeRule] WITH NOCHECK ADD 
	CONSTRAINT [DF_AreaCodeRule_IsDeleted] DEFAULT (0) FOR [IsDeleted],
	CONSTRAINT [DF_AreaCodeRule_DateCreated] DEFAULT (getdate()) FOR [DateCreated],
	CONSTRAINT [DF_AreaCodeRule_DateModified] DEFAULT (getdate()) FOR [DateModified]
GO

 CREATE  UNIQUE  INDEX [IX_AgentStation] ON [dbo].[AgentStation]([StationIP]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [IX_AgentStation_1] ON [dbo].[AgentStation]([StationNumber]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [IX_Campaign] ON [dbo].[Campaign]([ShortDescription]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [EventID] ON [dbo].[evevents]([EventID]) ON [PRIMARY]
GO

 CREATE  INDEX [ID] ON [dbo].[evusers]([ID]) ON [PRIMARY]
GO

 CREATE  INDEX [ID] ON [dbo].[jobsjobs]([ID]) ON [PRIMARY]
GO

 CREATE  UNIQUE  INDEX [ID] ON [dbo].[jobsjobtype]([ID]) ON [PRIMARY]
GO

 CREATE  INDEX [ID] ON [dbo].[jobsusers]([ID]) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgentActivity] ADD 
	CONSTRAINT [FK_AgentActivity_Agent] FOREIGN KEY 
	(
		[AgentID]
	) REFERENCES [dbo].[Agent] (
		[AgentID]
	),
	CONSTRAINT [FK_AgentActivity_Campaign] FOREIGN KEY 
	(
		[CampaignID]
	) REFERENCES [dbo].[Campaign] (
		[CampaignID]
	),
	CONSTRAINT [FK_AgentActivity_LoginStatus] FOREIGN KEY 
	(
		[AgentStatusID]
	) REFERENCES [dbo].[AgentStatus] (
		[AgentStatusID]
	)
GO

ALTER TABLE [dbo].[AreaCodeRule] ADD 
	CONSTRAINT [FK_AreaCodeRule_Agent] FOREIGN KEY 
	(
		[AgentID]
	) REFERENCES [dbo].[Agent] (
		[AgentID]
	)
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.FetchUDColums    Script Date: 11/28/2011 3:26:51 PM ******/

/****** Object:  Stored Procedure dbo.FetchUDColums    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE FetchUDColums
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.FetchUDColumsList    Script Date: 11/28/2011 3:26:51 PM ******/

/****** Object:  Stored Procedure dbo.FetchUDColumsList    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE FetchUDColumsList AS
BEGIN
SELECT   Objname AS [Column]
FROM   ::fn_listextendedproperty (NULL, 'user', 'dbo', 'table', 'agent', 'column', default)
where value <>''
END




GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.GenerateInsUpdateScript    Script Date: 11/28/2011 3:26:51 PM ******/

/****** Object:  Stored Procedure dbo.GenerateInsUpdateScript    Script Date: 11/28/2011 9:36:11 AM ******/

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

CREATE PROCEDURE GenerateInsUpdateScript
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.GenerateSelectDtlScript    Script Date: 11/28/2011 3:26:51 PM ******/

/****** Object:  Stored Procedure dbo.GenerateSelectDtlScript    Script Date: 11/28/2011 9:36:11 AM ******/


CREATE PROCEDURE GenerateSelectDtlScript
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.GenerateSelectScript    Script Date: 11/28/2011 3:26:51 PM ******/

/****** Object:  Stored Procedure dbo.GenerateSelectScript    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE GenerateSelectScript
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.SearchForStringInSPs    Script Date: 11/28/2011 3:26:51 PM ******/

/****** Object:  Stored Procedure dbo.SearchForStringInSPs    Script Date: 11/28/2011 9:36:11 AM ******/

/* 	TITLE 	: PROCEDURE WHICH RETURNS LIST OF ALL THE USER DEFINED STORED PROCEDURES WHICH CONTAIN A SPECIFIC STRING.
	AUTHOR 	: PRASAD BHOGADI
	ORGANIZATION : INFORAISE TECHNOLOGIES PVT LTD, INDIA.
*/


CREATE  PROCEDURE SearchForStringInSPs @searchfor VARCHAR(100)
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_LoginStatus_List    Script Date: 11/28/2011 3:26:51 PM ******/

/****** Object:  Stored Procedure dbo.Sel_LoginStatus_List    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE Sel_LoginStatus_List  	 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.CLONE_CAMPAIGN    Script Date: 11/28/2011 3:26:51 PM ******/

/****** Object:  Stored Procedure dbo.CLONE_CAMPAIGN    Script Date: 11/28/2011 9:36:11 AM ******/

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
											
						'INSERT INTO ['+@DBName+'].[dbo].[CampaignQueryStatus] (CampaignQueryID, QueryID,IsActive,IsStandby,Total,Available,
						Dials,Talks,AnsweringMachine,NoAnswer,Busy,OpInt,Drops,Failed,DateCreated,DateModified)
						SELECT CampaignQueryID, QueryID, IsActive,IsStandby,0,0,
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
					AnsweringMachineDetection,ColdCallScriptID,
					VerificationScriptID,InboundScriptID,AMCallTimes,PMCallTimes,
					WeekendCallTimes,AMDialingStart,AMDialingStop,PMDialingStart,PMDialingStop,
					AnsMachDetect,DateCreated,DateModified,ErrorRedialLapse,BusyRedialLapse,NoAnswerRedialLapse,ChannelsPerAgent,DefaultCallLapse,
					AnsweringMachineMessage,HumanMessageEnable,HumanMessage,SilentCallMessageEnable,SilentCallMessage,
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
					SELECT * FROM '+@ParentDBName+'.[dbo].[CampaignFields] where IsDefault = 0 OR FieldName = ''PledgeAmount'' '
	
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
						'INSERT INTO ['+@DBName+'].[dbo].[CampaignQueryStatus] (CampaignQueryID, QueryID, IsActive, IsStandby, Total, Available,
							Dials, Talks, AnsweringMachine, NoAnswer, Busy, OpInt, Drops, Failed, DateCreated, DateModified, IsCurrent,
							ShowMessage, Priority) SELECT CampaignQueryID, QueryID, IsActive, IsStandby, Total, Available,
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
						'INSERT INTO ['+@DBName+'].[dbo].[AgentStat] (StatID, AgentID, AgentName, StatusID, LeadsSales, PledgeAmount, Presentations,
							Calls, LeadSalesRatio, TalkTime, WaitingTime, PauseTime, WrapTime, LoginDate, LoginTime, LogOffDate,
							LastResultCodeID, DateCreated, DateModified, TimeModified) 
						SELECT StatID, AgentID, AgentName, StatusID, LeadsSales, PledgeAmount, Presentations,
							Calls, LeadSalesRatio, TalkTime, WaitingTime, PauseTime, WrapTime, LoginDate, LoginTime, LogOffDate,
							LastResultCodeID, DateCreated, DateModified, TimeModified FROM '+@ParentDBName+'.[dbo].[AgentStat]'
	EXECUTE sp_executesql @SQLQuery
	
END










GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.DEL_AgentStation    Script Date: 11/28/2011 3:26:51 PM ******/

/****** Object:  Stored Procedure dbo.DEL_AgentStation    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE DEL_AgentStation
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.DEL_AreaCode    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.DEL_AreaCode    Script Date: 11/28/2011 9:36:11 AM ******/

create PROCEDURE DEL_AreaCode 
	@AreaCodeID bigint
AS
DELETE FROM dbo.AreaCode
WHERE
	AreaCodeID = @AreaCodeID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.DEL_Campaign    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.DEL_Campaign    Script Date: 11/28/2011 9:36:11 AM ******/



CREATE PROCEDURE DEL_Campaign
	@CampaignID bigint
AS
UPDATE dbo.Campaign
SET
	IsDeleted = 1,
	ShortDescription = ShortDescription + ' - ' +  convert(varchar(24),getdate(),121)
WHERE
	CampaignID = @CampaignID





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.GetCampaignStatus    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.GetCampaignStatus    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE GetCampaignStatus 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.InsUpd_Agent    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.InsUpd_Agent    Script Date: 11/28/2011 9:36:11 AM ******/

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.InsUpd_AgentCampaignMap    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.InsUpd_AgentCampaignMap    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE InsUpd_AgentCampaignMap
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.InsUpd_AgentCampaignMap2    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.InsUpd_AgentCampaignMap2    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE InsUpd_AgentCampaignMap2
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.InsUpd_AgentStation    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.InsUpd_AgentStation    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE InsUpd_AgentStation
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.InsUpd_AreaCode    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.InsUpd_AreaCode    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE InsUpd_AreaCode 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.InsUpd_Campaign    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.InsUpd_Campaign    Script Date: 11/28/2011 9:36:11 AM ******/

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.InsUpd_GlobalDialingParameters    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.InsUpd_GlobalDialingParameters    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE InsUpd_GlobalDialingParameters 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Ins_AdminRequest    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Ins_AdminRequest    Script Date: 11/28/2011 9:36:11 AM ******/

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Ins_DialerActivity    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Ins_DialerActivity    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE Ins_DialerActivity
	@ConnectTime DateTime
AS
	DECLARE @DialerActivityID as bigint

	UPDATE DialerActivity SET DialerStopTime = getdate(), DisconnectTime = getdate() 
	WHERE  DialerStopTime IS NULL and DialerStartTime IS NOT NULL
		
	INSERT INTO DialerActivity(ConnectTime) VALUES(@ConnectTime)
	SET @DialerActivityID = @@IDENTITY
	
	SELECT @DialerActivityID as DialerActivityID



GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.IsDialerRunning    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.IsDialerRunning    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE IsDialerRunning 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.ResetCampaignsToIdle    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.ResetCampaignsToIdle    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE [dbo].[ResetCampaignsToIdle]
AS
BEGIN 	
	UPDATE dbo.Campaign
	SET		
		StatusID = 1	
END


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_ActiveCampaign_List    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_ActiveCampaign_List    Script Date: 11/28/2011 9:36:11 AM ******/



CREATE PROCEDURE Sel_ActiveCampaign_List 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_AdminRequests    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_AdminRequests    Script Date: 11/28/2011 9:36:11 AM ******/
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_AgentCampaignMap_Dtl    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_AgentCampaignMap_Dtl    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE Sel_AgentCampaignMap_Dtl 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_Agent_Dtl    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_Agent_Dtl    Script Date: 11/28/2011 9:36:11 AM ******/

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_Agent_List    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_Agent_List    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE Sel_Agent_List  	 
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
END





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_AreaCode    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_AreaCode    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE Sel_AreaCode 	 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_Campaign_Dtl    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_Campaign_Dtl    Script Date: 11/28/2011 9:36:11 AM ******/

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_Campaign_List    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_Campaign_List    Script Date: 11/28/2011 9:36:11 AM ******/



CREATE PROCEDURE Sel_Campaign_List 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_Campaign_ListByCampaignID    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_Campaign_ListByCampaignID    Script Date: 11/28/2011 9:36:11 AM ******/


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
		Allow7DigitNums,
		Allow10DigitNums, 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_FieldTypes_List    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_FieldTypes_List    Script Date: 11/28/2011 9:36:12 AM ******/

CREATE PROCEDURE Sel_FieldTypes_List
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_GlobalDialingParameters_List    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_GlobalDialingParameters_List    Script Date: 11/28/2011 9:36:12 AM ******/

CREATE PROCEDURE Sel_GlobalDialingParameters_List 
 	 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_PhoneLinesInUse    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_PhoneLinesInUse    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE Sel_PhoneLinesInUse 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_Station_Dtl    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_Station_Dtl    Script Date: 11/28/2011 9:36:12 AM ******/

CREATE PROCEDURE Sel_Station_Dtl  
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_Station_List    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_Station_List    Script Date: 11/28/2011 9:36:12 AM ******/

CREATE PROCEDURE Sel_Station_List  	 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.ShutdownAllCampaigns    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.ShutdownAllCampaigns    Script Date: 11/28/2011 9:36:11 AM ******/
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.ToggleAgentReset    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.ToggleAgentReset    Script Date: 11/28/2011 9:36:12 AM ******/

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Upd_AdminRequestStatus    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Upd_AdminRequestStatus    Script Date: 11/28/2011 9:36:12 AM ******/

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Upd_CampaignQuery_Status    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Upd_CampaignQuery_Status    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE Upd_CampaignQuery_Status 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Upd_CampaignStatus    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Upd_CampaignStatus    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE Upd_CampaignStatus
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Upd_Campaign_DialStatus    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Upd_Campaign_DialStatus    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE Upd_Campaign_DialStatus 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Upd_DialerStart    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Upd_DialerStart    Script Date: 11/28/2011 9:36:12 AM ******/

CREATE PROCEDURE Upd_DialerStart
	@DialerActivityID bigint,
	@DialerStartTime datetime
AS
	UPDATE DialerActivity SET 
		DialerStartTime = @DialerStartTime 
	WHERE DialerActivityID = @DialerActivityID


GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Upd_DialerStop    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Upd_DialerStop    Script Date: 11/28/2011 9:36:12 AM ******/

CREATE PROCEDURE Upd_DialerStop
	@DialerActivityID bigint,
	@DialerStopTime datetime
AS
	UPDATE DialerActivity SET 
		DialerStopTime = @DialerStopTime,
		DisconnectTime = @DialerStopTime
	WHERE DialerActivityID = @DialerActivityID





GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.p_AgentInfoByLoginNameGet    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.p_AgentInfoByLoginNameGet    Script Date: 11/28/2011 9:36:12 AM ******/

------------------------------------------
--OBJECTNAME: p_AgentInfoByLoginNameGet
--AUTHOR: Nagasree Mendu
--DESCRIPTION: Returns Agent Info based
--on the Login User Name
--------------------------------------
CREATE PROCEDURE p_AgentInfoByLoginNameGet 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.DEL_Agent    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.DEL_Agent    Script Date: 11/28/2011 9:36:12 AM ******/

CREATE PROCEDURE DEL_Agent 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Get_AgentActivity_ById    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Get_AgentActivity_ById    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE Get_AgentActivity_ById 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.InsGet_AgentActivity    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.InsGet_AgentActivity    Script Date: 11/28/2011 9:36:12 AM ******/

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.InsUpd_AreaCodeRule    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.InsUpd_AreaCodeRule    Script Date: 11/28/2011 9:36:12 AM ******/

CREATE PROCEDURE InsUpd_AreaCodeRule 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_AreaCodeRule_ByAgentID    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_AreaCodeRule_ByAgentID    Script Date: 11/28/2011 9:36:12 AM ******/

CREATE PROCEDURE Sel_AreaCodeRule_ByAgentID 
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Sel_LoggedInAgents    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Sel_LoggedInAgents    Script Date: 11/28/2011 9:36:12 AM ******/

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Upd_AgentActivity    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Upd_AgentActivity    Script Date: 11/28/2011 9:36:12 AM ******/

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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.Upd_AgentLogOut    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  Stored Procedure dbo.Upd_AgentLogOut    Script Date: 11/28/2011 9:36:11 AM ******/

CREATE PROCEDURE Upd_AgentLogOut
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

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  User Defined Function dbo.CampaignAgentStationInfo    Script Date: 11/28/2011 3:26:52 PM ******/

/****** Object:  User Defined Function dbo.CampaignAgentStationInfo    Script Date: 11/28/2011 9:36:12 AM ******/
CREATE FUNCTION CampaignAgentStationInfo(@CampaignID BIGINT, @AgentID BIGINT) RETURNS VARCHAR(255)
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

GO
SET ANSI_NULLS ON 
GO
INSERT INTO agent VALUES  ('Amcat Admin', 'admin', '05199deca16614131327f2c3fea9031c', 1, 1, 1, 0, '530-897-5201', 1, 0, GETDATE(), GETDATE(),0)

GO
INSERT INTO Status VALUES ('Idle',GETDATE(),GETDATE())
INSERT INTO Status VALUES ('Run',GETDATE(),GETDATE())
INSERT INTO Status VALUES ('Pause',GETDATE(),GETDATE())
INSERT INTO Status VALUES ('Completed',GETDATE(),GETDATE())
GO
INSERT INTO AgentStatus VALUES ('Ready', GETDATE(),GETDATE())
INSERT INTO AgentStatus VALUES ('Pause', GETDATE(),GETDATE())
INSERT INTO AgentStatus VALUES ('Busy', GETDATE(),GETDATE())
GO
GO
INSERT INTO TimeZone VALUES ('AK', 9, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('AZ', 7, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('AR', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('CA', 8, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('CO', 7, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('CT', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('DE', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('DC', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('FL', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('GA', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('HI', 10, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('ID', 7, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('IL', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('IN', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('IA', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('KS', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('LA', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('MA', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('MD', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('ME', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('MI', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('MN', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('MO', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('MS', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('MT', 7, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('NE', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('NV', 8, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('NH', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('NJ', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('NM', 7, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('NY', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('NC', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('ND', 0, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('OH', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('OK', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('OR', 8, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('PA', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('RI', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('SC', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('SD', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('TN', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('TX', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('UT', 7, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('VT', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('VA', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('WA', 8, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('WV', 5, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('WI', 6, GETDATE(), GETDATE()) 
INSERT INTO TimeZone VALUES ('WY', 7, GETDATE(), GETDATE()) 
GO

insert into FieldTypes values('String','Varchar',Getdate(),Getdate())
insert into FieldTypes values('Integer','int',Getdate(),Getdate())
insert into FieldTypes values('Decimal','float',Getdate(),Getdate())
insert into FieldTypes values('Money','money',Getdate(),Getdate())
insert into FieldTypes values('Date','datetime',Getdate(),Getdate())
insert into FieldTypes values('Boolean','bit',Getdate(),Getdate())

GO


