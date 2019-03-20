USE [Core]
GO

PRINT 'Running DDL Process...'
GO

/*
 *  ###########################################################
 *    Drop tables if they exist
 *  ###########################################################
 */

EXEC [dbo].[spDropTable] 'dbo', 'Organization'

GO


/*
 *  ###########################################################
 *    Settings
 *  ###########################################################
 */

SET ANSI_NULLS ON

SET QUOTED_IDENTIFIER ON

SET ANSI_PADDING ON

GO


/*
 *  ###########################################################
 *    CREATE - Organization
 *  ###########################################################
 */

CREATE TABLE [dbo].[Organization](
    [ID] [bigint] IDENTITY(1,1) NOT NULL,
    [Name] [varchar](100) NOT NULL,
    [Description] [varchar](4000) NOT NULL,
    [Code] [varchar](50) NOT NULL,
    [DisplayName] [varchar](100) NOT NULL,
    [DisplayOrder] [int] NULL,
    [IsActive] [bit] NOT NULL,
    [CreatedBy] [varchar](100) NOT NULL,
    [CreatedDate] [datetime] NOT NULL,
    [ModifiedBy] [varchar](100) NULL,
    [ModifiedDate] [datetime] NULL,
    [RowVersion] [rowversion] NOT NULL,
    CONSTRAINT [PKC__Organization__ID] PRIMARY KEY CLUSTERED 
    (
        [ID]
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    CONSTRAINT [UKC__Organization__Name] UNIQUE NONCLUSTERED 
    (
        [Name]
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    CONSTRAINT [UKC__Organization__Code] UNIQUE NONCLUSTERED 
    (
        [Code]
    ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Organization] ADD CONSTRAINT [DF__Organization__CreatedBy]
DEFAULT (suser_sname()) FOR [CreatedBy]
GO

ALTER TABLE [dbo].[Organization] ADD CONSTRAINT [DF__Organization__CreatedDate]
DEFAULT (getutcdate()) FOR [CreatedDate]
GO

ALTER TABLE [dbo].[Organization] ADD CONSTRAINT [DF__Organization__IsActive]
DEFAULT 1 FOR [IsActive]
GO

CREATE NONCLUSTERED INDEX IX__Organization__Name
ON [dbo].[Organization] (Name)
GO

CREATE NONCLUSTERED INDEX IX__Organization__Code
ON [dbo].[Organization] (Code)
GO

PRINT 'Complete'
GO