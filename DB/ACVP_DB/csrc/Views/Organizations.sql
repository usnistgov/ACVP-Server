
CREATE VIEW [csrc].[Organizations]
WITH SCHEMABINDING
AS

SELECT OrganizationId, ParentOrganizationId, OrganizationName, OrganizationUrl, VoiceNumber, FaxNumber
FROM dbo.Organizations



GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_Organizations]
    ON [csrc].[Organizations]([OrganizationId] ASC);

