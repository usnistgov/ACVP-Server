CREATE PROCEDURE [dbo].[AddressGet]

	@AddressId bigint

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
WHERE AddressId = @AddressId

