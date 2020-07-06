CREATE TABLE [dbo].[DependencyAttributes] (
    [DependencyAttributeId]                          BIGINT          IDENTITY (1, 1) NOT NULL,
    [DependencyId] BIGINT          NOT NULL,
    [Name]                        NVARCHAR (1024) NOT NULL,
    [Value]                       NVARCHAR (1024) NOT NULL,
    CONSTRAINT [PK_DependencyAttributes] PRIMARY KEY CLUSTERED ([DependencyAttributeId] ASC),
    CONSTRAINT [FK_DependencyAttributes_Dependencies] FOREIGN KEY ([DependencyId]) REFERENCES [dbo].[Dependencies] ([DependencyId])
);

