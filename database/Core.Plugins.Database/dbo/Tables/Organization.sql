CREATE TABLE [dbo].[Organization] (
    [ID]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Name]         VARCHAR (100)  NOT NULL,
    [Description]  VARCHAR (4000) NOT NULL,
    [Code]         VARCHAR (50)   NOT NULL,
    [DisplayName]  VARCHAR (100)  NOT NULL,
    [DisplayOrder] INT            NULL,
    [IsActive]     BIT            CONSTRAINT [DF__Organization__IsActive] DEFAULT ((1)) NOT NULL,
    [CreatedBy]    VARCHAR (100)  CONSTRAINT [DF__Organization__CreatedBy] DEFAULT (suser_sname()) NOT NULL,
    [CreatedDate]  DATETIME       CONSTRAINT [DF__Organization__CreatedDate] DEFAULT (getdate()) NOT NULL,
    [ModifiedBy]   VARCHAR (100)  NULL,
    [ModifiedDate] DATETIME       NULL,
    [RowVersion]   ROWVERSION     NOT NULL,
    CONSTRAINT [PKC__Organization__ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [UKC__Organization__Code] UNIQUE NONCLUSTERED ([Code] ASC),
    CONSTRAINT [UKC__Organization__Name] UNIQUE NONCLUSTERED ([Name] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX__Organization__Name]
    ON [dbo].[Organization]([Name] ASC);


GO
CREATE NONCLUSTERED INDEX [IX__Organization__Code]
    ON [dbo].[Organization]([Code] ASC);

