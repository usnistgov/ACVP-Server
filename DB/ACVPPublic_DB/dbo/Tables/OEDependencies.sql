CREATE TABLE [dbo].[OEDependencies] (
    [OEId]         BIGINT NOT NULL,
    [DependencyId] BIGINT NOT NULL,
    CONSTRAINT [PK_OEDependencies] PRIMARY KEY CLUSTERED ([OEId] ASC, [DependencyId] ASC)
);

