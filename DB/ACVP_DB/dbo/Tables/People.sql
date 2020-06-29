CREATE TABLE [dbo].[People] (
    [PersonId]        BIGINT          IDENTITY (1, 1)  NOT NULL,
    [FullName] NVARCHAR (1024) NOT NULL,
    [OrganizationId]    BIGINT          NULL,
    CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED ([PersonId] ASC),
    CONSTRAINT [FK_People_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([OrganizationId])
);

