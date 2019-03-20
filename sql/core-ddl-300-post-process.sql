USE [Core]
GO

PRINT 'Running DDL Post-Process...'
GO

CREATE VIEW [dbo].[vwOrganization]
AS
    SELECT *
    FROM [dbo].[Organization] (NOLOCK)
GO

PRINT 'Complete'
GO