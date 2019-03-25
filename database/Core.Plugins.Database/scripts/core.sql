--PRINT 'Running DDL Pre-Process...'
--GO

--/*
-- *  ###########################################################
-- *    Create the database
-- *  ###########################################################
-- */

--IF EXISTS (SELECT * FROM sys.databases WHERE name = N'Core')
--BEGIN
--    DROP DATABASE [Core]
--END
--GO

--CREATE DATABASE [Core]
--GO

--ALTER DATABASE [Core] SET READ_COMMITTED_SNAPSHOT ON;
--GO

--USE [Core]
--GO


--/*
-- *  ###########################################################
-- *    Create a procedure to assist with the build script
-- *  ###########################################################
-- */

--CREATE PROCEDURE [dbo].[spDropTable] (@schemaName VARCHAR(255), @tableName VARCHAR(255))
--AS
--BEGIN

--    DECLARE @SQL VARCHAR(MAX);

--    IF OBJECT_ID(@schemaName + '.' + @tableName, 'U') IS NOT NULL
--    BEGIN
--        SET @SQL = 'DROP TABLE [' + @schemaName + '].[' + @tableName + ']'

--        EXEC (@SQL);

--        PRINT 'SUCCESS: Dropped Table [' + @schemaName + '].[' + @tableName + ']'
--    END
--    ELSE
--    BEGIN
--        PRINT 'INFO: Table [' + @schemaName + '].[' + @tableName + '] not found. Skipping Drop statement'
--    END

--END
--GO

--PRINT 'Complete'
--GO

--USE [Core]
--GO

--PRINT 'Running DDL Process...'
--GO

--/*
-- *  ###########################################################
-- *    Drop tables if they exist
-- *  ###########################################################
-- */

--EXEC [dbo].[spDropTable] 'dbo', 'Organization'

--GO


--/*
-- *  ###########################################################
-- *    Settings
-- *  ###########################################################
-- */

--SET ANSI_NULLS ON

--SET QUOTED_IDENTIFIER ON

--SET ANSI_PADDING ON

--GO


--/*
-- *  ###########################################################
-- *    CREATE - Organization
-- *  ###########################################################
-- */

--CREATE TABLE [dbo].[Organization](
--    [ID] [bigint] IDENTITY(1,1) NOT NULL,
--    [Name] [varchar](100) NOT NULL,
--    [Description] [varchar](4000) NOT NULL,
--    [Code] [varchar](50) NOT NULL,
--    [DisplayName] [varchar](100) NOT NULL,
--    [DisplayOrder] [int] NULL,
--    [IsActive] [bit] NOT NULL,
--    [CreatedBy] [varchar](100) NOT NULL,
--    [CreatedDate] [datetime] NOT NULL,
--    [ModifiedBy] [varchar](100) NULL,
--    [ModifiedDate] [datetime] NULL,
--    [RowVersion] [rowversion] NOT NULL,
--    CONSTRAINT [PKC__Organization__ID] PRIMARY KEY CLUSTERED 
--    (
--        [ID]
--    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
--    CONSTRAINT [UKC__Organization__Name] UNIQUE NONCLUSTERED 
--    (
--        [Name]
--    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
--    CONSTRAINT [UKC__Organization__Code] UNIQUE NONCLUSTERED 
--    (
--        [Code]
--    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
--    ) ON [PRIMARY]

--GO

--ALTER TABLE [dbo].[Organization] ADD CONSTRAINT [DF__Organization__CreatedBy]
--DEFAULT (suser_sname()) FOR [CreatedBy]
--GO

--ALTER TABLE [dbo].[Organization] ADD CONSTRAINT [DF__Organization__CreatedDate]
--DEFAULT (getutcdate()) FOR [CreatedDate]
--GO

--ALTER TABLE [dbo].[Organization] ADD CONSTRAINT [DF__Organization__IsActive]
--DEFAULT 1 FOR [IsActive]
--GO

--CREATE NONCLUSTERED INDEX IX__Organization__Name
--ON [dbo].[Organization] (Name)
--GO

--CREATE NONCLUSTERED INDEX IX__Organization__Code
--ON [dbo].[Organization] (Code)
--GO

--PRINT 'Complete'
--GO

--USE [Core]
--GO

--PRINT 'Running DDL Post-Process...'
--GO

--CREATE VIEW [dbo].[vwOrganization]
--AS
--    SELECT *
--    FROM [dbo].[Organization] (NOLOCK)
--GO

--PRINT 'Complete'
--GO

--USE [Core]
--GO

--PRINT 'Running DML Pre-Process...'
--GO

--PRINT 'Complete'
--GO

--USE [Core]
--GO

--PRINT 'Running DML Process...'
--GO

--DECLARE
--  @createdBy       AS VARCHAR(100)        = 'sferguson'
--, @createdDate     AS DATETIME            = CURRENT_TIMESTAMP

--INSERT INTO [dbo].[Organization] ([Name], [Description], [Code], [DisplayName], [DisplayOrder], [IsActive], [CreatedBy], [CreatedDate])
--VALUES ('Admin', 'Administration account', 'ADMN', 'Admin', 1, 1, @createdBy, @createdDate)

--GO

--PRINT 'Complete'
--GO

--USE [Core]
--GO

--PRINT 'Running DML Post-Process...'
--GO

--SELECT * 
--FROM [dbo].[Organization]

--SELECT * 
--FROM [dbo].[vwOrganization]

--PRINT 'Complete'
--GO