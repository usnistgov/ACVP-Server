CREATE TABLE [dbo].[ImplementationContacts] (
    [ImplementationContactId] BIGINT NOT NULL,
    [ImplementationId]        BIGINT NOT NULL,
    [PersonId]                BIGINT NOT NULL,
    [OrderIndex]              INT    NOT NULL
);


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_VALIDATION_CONTACT]
    ON [dbo].[ImplementationContacts]([ImplementationContactId] ASC);

