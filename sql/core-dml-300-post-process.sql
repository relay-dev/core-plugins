USE [Core]
GO

PRINT 'Running DML Post-Process...'
GO

SELECT * 
FROM [dbo].[Organization]

SELECT * 
FROM [dbo].[vwOrganization]

PRINT 'Complete'
GO