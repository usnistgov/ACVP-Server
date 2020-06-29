CREATE TABLE [dbo].[Dependencies] (
    [DependencyId]              BIGINT          IDENTITY (1, 1) NOT NULL,
    [DependencyType] NVARCHAR (1024) NOT NULL,
    [Name]            NVARCHAR (1024) NOT NULL,
    [Description]     NVARCHAR (2048) NULL,
    CONSTRAINT [PK_Dependencies] PRIMARY KEY CLUSTERED ([DependencyId] ASC)
);

