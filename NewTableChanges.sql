


CREATE TABLE [dbo].[TblCityNew](
	[Id] [bigint] IDENTITY(1,1) NOT NULL primary key,
	[TenantId] [uniqueidentifier] NOT NULL,
	[Code] [varchar](25) NULL,
	[StateId] [bigint] NULL,
	[CityName] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[Status] [bit] NULL)



	CREATE TABLE [dbo].[TblCountryNew](
	[CountryId] [bigint] IDENTITY(1,1) NOT NULL primary key,
	[TenantId] [uniqueidentifier] NOT NULL,
	[Code] [varchar](25) NULL,
	[CountryName] [varchar](50) NULL,
	[MobileCode] [varchar](10) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[Status] [bit] NULL)

	CREATE TABLE [dbo].[TblEmpHierarchyNew](
	[Id] [bigint] IDENTITY(1,1) NOT NULL primary key,
    [TenantId] [uniqueidentifier] NOT NULL,
	[Code] [varchar](25) NULL,
	[EDescription] [varchar](1000) NULL,
	[ParentId] [bigint] NULL)
	
	CREATE TABLE [dbo].[TblLanguageNew](
	[Id] [bigint] IDENTITY(1,1) NOT NULL primary key,
	[TenantId] [uniqueidentifier] NOT NULL,
	[Code] [varchar](25) NULL,
	[LanguageName] [varchar](100) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[Status] [bit] NULL)


	CREATE TABLE [dbo].[TblMileStoneStageNew](
	[Id] [bigint] IDENTITY(1,1) NOT NULL primary key,
	[TenantId] [uniqueidentifier] NOT NULL,
	[MileStoneStage] [varchar](50) NOT NULL,
	[Roles] [varchar](50) NOT NULL)

	CREATE TABLE [dbo].[TblProductTypeNew](
	[Id] [bigint] IDENTITY(1,1) NOT NULL primary key,
	[TenantId] [uniqueidentifier] NOT NULL,
	[ProductType] [varchar](200) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[Status] [bit] NULL)


	CREATE TABLE [dbo].[TblStateNew](
	[StateId] [bigint] IDENTITY(1,1) NOT NULL primary key,
	[TenantId] [uniqueidentifier] NOT NULL,
	[Code] [varchar](25) NULL,
	[CountryId] [bigint] NULL,
	[StateName] [varchar](50) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[Status] [bit] NULL)



	CREATE TABLE [dbo].[TblTransEmployeeTargetNew](
	[Id] [bigint] IDENTITY(1,1) NOT NULL primary key,
	[TenantId] [uniqueidentifier] NOT NULL,
	[EmpId] [bigint] NULL,
	[EmployeeRoleId] [bigint] NULL,
	[FinancialYearId] [bigint] NULL,
	[TargetHike] [decimal](18, 0) NULL,
	[IsAutomatic] [bit] NULL,
	[Budget] [decimal](18, 0) NULL,
	[QuarterlyTarget] [decimal](18, 0) NULL,
	[MonthlyTarget] [decimal](18, 0) NULL,
	[WeeklyTarget] [decimal](18, 0) NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedDate] [datetime] NULL,
	[Status] [bit] NULL)



 