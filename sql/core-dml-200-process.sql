USE [Core]
GO

PRINT 'Running DML Process...'
GO

DECLARE
  @createdBy       AS VARCHAR(100)        = 'sferguson'
, @createdDate     AS DATETIME            = CURRENT_TIMESTAMP

INSERT INTO [dbo].[Organization] ([Name], [Description], [Code], [DisplayName], [DisplayOrder], [IsActive], [CreatedBy], [CreatedDate])
VALUES ('Admin', 'Administration account', 'ADMN', 'Admin', 1, 1, @createdBy, @createdDate)

GO

PRINT 'Complete'
GO