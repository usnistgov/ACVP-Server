CREATE PROCEDURE [val].[AddressGet]

	@AddressID bigint

AS

SET NOCOUNT ON

SELECT	 id AS ID
        ,organization_id AS OrganizationID
		,address_street1 AS Street1
		,address_street2 AS Street2
		,address_street3 AS Street3
		,address_locality AS Locality
		,address_region AS Region
		,address_postal_code AS PostalCode
		,address_country AS Country
FROM val.[ADDRESS]
WHERE id = @AddressID