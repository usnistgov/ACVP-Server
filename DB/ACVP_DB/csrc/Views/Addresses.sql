
CREATE VIEW [csrc].[Addresses]
WITH SCHEMABINDING
AS

SELECT AddressId, OrganizationId, OrderIndex, Street1, Street2, Street3, Locality, Region, Country, PostalCode
FROM dbo.Addresses


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_Addresses]
    ON [csrc].[Addresses]([AddressId] ASC);

