CREATE TABLE [dbo].[ValidationSources] (
    [ValidationSourceId] INT           IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [Name]               NVARCHAR (32) NOT NULL,
    [Prefix]             NVARCHAR (16) NULL,
    CONSTRAINT [PK_ValidationSources] PRIMARY KEY CLUSTERED ([ValidationSourceId] ASC)
);

