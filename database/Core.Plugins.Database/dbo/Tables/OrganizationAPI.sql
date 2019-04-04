CREATE TABLE [dbo].[OrganizationAPI] (
	[ID]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [OrganizationID]    BIGINT         NOT NULL,
    [APIKeyEncrypted]   VARCHAR (4000) NOT NULL,
    [StartDate]         DATETIME       CONSTRAINT [DF__Token__StartDate] DEFAULT (getdate()) NOT NULL, 
    [EndDate]           DATETIME       CONSTRAINT [DF__Token__EndDate] DEFAULT '12/31/9999' NOT NULL, 
    [CreatedBy]         VARCHAR (100)  CONSTRAINT [DF__Token__CreatedBy] DEFAULT (suser_sname()) NOT NULL, 
    [CreatedDate]       DATETIME       CONSTRAINT [DF__Token__CreatedDate] DEFAULT (getdate()) NOT NULL, 
    [ModifiedBy]        VARCHAR (100)  NULL,
    [ModifiedDate]      DATETIME       NULL, 
    [RowVersion]        ROWVERSION     NOT NULL,
    CONSTRAINT [PKC__OrganizationApiToken__ID] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [FKN__OrganizationApiToken__OrganizationID] FOREIGN KEY ([OrganizationID]) REFERENCES [dbo].[Organization] ([ID])
);


GO
CREATE NONCLUSTERED INDEX [IX__Token__TokenValue]
    ON [dbo].[OrganizationAPI]([APIKeyEncrypted] ASC);

