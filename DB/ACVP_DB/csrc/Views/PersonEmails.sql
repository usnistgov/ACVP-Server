
CREATE VIEW [csrc].[PersonEmails]
WITH SCHEMABINDING
AS

SELECT PersonId, OrderIndex, EmailAddress
FROM dbo.PersonEmails


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_PersonEmails]
    ON [csrc].[PersonEmails]([PersonId] ASC, [EmailAddress] ASC);

