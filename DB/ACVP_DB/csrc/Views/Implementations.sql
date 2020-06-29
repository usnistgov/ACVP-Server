CREATE VIEW [csrc].[Implementations]
WITH SCHEMABINDING
AS

SELECT ImplementationId, VendorId, AddressId, [Url], ImplementationName, ImplementationTypeId, ImplementationVersion, ImplementationDescription, ITAR
FROM dbo.Implementations
WHERE ITAR = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_Implementations]
    ON [csrc].[Implementations]([ImplementationId] ASC);

