CREATE TABLE [dbo].[ImplementationContacts] (
    [ImplementationContactId]                     BIGINT IDENTITY (1, 1) NOT NULL,
    [ImplementationId] BIGINT NOT NULL,
    [PersonId]              BIGINT NOT NULL,
    [OrderIndex]            INT    NOT NULL,
    CONSTRAINT [PK_ImplementationContacts] PRIMARY KEY CLUSTERED ([ImplementationContactId] ASC),
    CONSTRAINT [FK_ImplementationContacts_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [dbo].[People] ([PersonId]),
    CONSTRAINT [FK_ImplementationContacts_ImplementationId] FOREIGN KEY ([ImplementationId]) REFERENCES [dbo].[Implementations] ([ImplementationId])
);

