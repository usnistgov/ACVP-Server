CREATE PROCEDURE [dbo].[AddressesForOrganizationGet]

	@OrganizationId bigint

AS

SET NOCOUNT ON

SELECT	 AddressId
		,OrganizationId
		,Street1
		,Street2
		,Street3
		,Locality
		,Region
		,PostalCode
		,Country
		,OrderIndex
FROM dbo.Addresses
WHERE OrganizationId = @OrganizationId
ORDER BY OrderIndex

