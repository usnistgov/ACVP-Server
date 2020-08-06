CREATE VIEW [dbo].[FilteredImplementations]
WITH SCHEMABINDING
AS

SELECT ImplementationId, OrganizationId, AddressId, [Url], ImplementationName, ImplementationTypeId, ImplementationVersion, ImplementationDescription, ITAR
FROM dbo.Implementations
WHERE ITAR = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_FilteredImplementations]
    ON [dbo].[FilteredImplementations]([ImplementationId] ASC);

