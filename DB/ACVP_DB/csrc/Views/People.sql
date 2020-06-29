

CREATE VIEW [csrc].[People]
WITH SCHEMABINDING
AS

SELECT PersonId, FullName, OrganizationId
FROM dbo.People


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_People]
    ON [csrc].[People]([PersonId] ASC);

