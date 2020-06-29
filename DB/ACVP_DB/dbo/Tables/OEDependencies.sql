CREATE TABLE [dbo].[OEDependencies] (
    [OEId] BIGINT NOT NULL,
    [DependencyId]    BIGINT NOT NULL,
    CONSTRAINT [PK_OEDependencies] PRIMARY KEY CLUSTERED ([OEId] ASC, [DependencyId] ASC),
    CONSTRAINT [FK_OEDependencies_DependencyId] FOREIGN KEY ([DependencyId]) REFERENCES [dbo].[Dependencies] ([DependencyId]),
    CONSTRAINT [FK_OEDependencies_OEId] FOREIGN KEY ([OEId]) REFERENCES [dbo].[OEs] ([OEId])
);

