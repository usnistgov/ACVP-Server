CREATE TABLE [dbo].[DependencyAttributes] (
    [DependencyAttributeId] BIGINT          IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DependencyId]          BIGINT          NOT NULL,
    [Name]                  NVARCHAR (1024) NOT NULL,
    [Value]                 NVARCHAR (1024) NOT NULL,
    CONSTRAINT [PK_DependencyAttributes] PRIMARY KEY CLUSTERED ([DependencyAttributeId] ASC)
);

