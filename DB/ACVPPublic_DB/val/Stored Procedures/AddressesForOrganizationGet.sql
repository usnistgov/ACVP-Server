CREATE PROCEDURE [val].[AddressesForOrganizationGet]

	@OrganizationID bigint

AS

SET NOCOUNT ON

SELECT	 id AS ID
		,@OrganizationID AS OrganizationID
		,address_street1 AS Street1
		,address_street2 AS Street2
		,address_street3 AS Street3
		,address_locality AS Locality
		,address_region AS Region
		,address_postal_code AS PostalCode
		,address_country AS Country
		,order_index AS OrderIndex
FROM val.[ADDRESS]
WHERE organization_id = @OrganizationID
ORDER BY order_index

