CREATE PROCEDURE [val].[AddressInsert]
	 
	 @OrganizationID bigint
	,@OrderIndex int
	,@Street1 nvarchar(1024)
	,@Street2 nvarchar(1024)
	,@Street3 nvarchar(1024)
	,@Locality nvarchar(1024)
	,@Region nvarchar(1024)
	,@Country nvarchar(128)
	,@PostalCode nvarchar(128)

AS

SET NOCOUNT ON

INSERT INTO val.[ADDRESS] (
	 organization_id
	,order_index
	,address_street1
	,address_street2
	,address_street3
	,address_locality
	,address_region
	,address_country
	,address_postal_code
)
VALUES (
	 @OrganizationID
	,@OrderIndex
	,@Street1
	,@Street2
	,@Street3
	,@Locality
	,@Region
	,@Country
	,@PostalCode
)

SELECT SCOPE_IDENTITY() AS ID
