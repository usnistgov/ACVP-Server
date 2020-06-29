
CREATE VIEW [csrc].[OrganizationEmails]
WITH SCHEMABINDING
AS

SELECT OrganizationId, OrderIndex, EmailAddress
FROM dbo.OrganizationEmails



GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_OrganizationEmails]
    ON [csrc].[OrganizationEmails]([OrganizationId] ASC, [EmailAddress] ASC);

