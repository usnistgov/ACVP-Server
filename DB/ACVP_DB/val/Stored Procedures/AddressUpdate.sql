CREATE PROCEDURE [val].[AddressUpdate]
	 
	 @AddressID bigint
	,@OrderIndex int
	,@Street1 nvarchar(1024)
	,@Street2 nvarchar(1024)
	,@Street3 nvarchar(1024)
	,@Locality nvarchar(1024)
	,@Region nvarchar(1024)
	,@Country nvarchar(128)
	,@PostalCode nvarchar(128)
	,@Street1Updated bit
	,@Street2Updated bit
	,@Street3Updated bit
	,@LocalityUpdated bit
	,@RegionUpdated bit
	,@PostalCodeUpdated bit
	,@CountryUpdated bit

AS

SET NOCOUNT ON

UPDATE val.[ADDRESS]
SET  order_index = @OrderIndex
	,address_street1 = CASE @Street1Updated WHEN 1 THEN @Street1 ELSE address_street1 END
	,address_street2 = CASE @Street2Updated WHEN 1 THEN @Street2 ELSE address_street2 END
	,address_street3 = CASE @Street3Updated WHEN 1 THEN @Street3 ELSE address_street3 END
	,address_locality = CASE @LocalityUpdated WHEN 1 THEN @Locality ELSE address_locality END
	,address_region = CASE @RegionUpdated WHEN 1 THEN @Region ELSE address_region END
	,address_country = CASE @CountryUpdated WHEN 1 THEN @Country ELSE address_country END
	,address_postal_code = CASE @PostalCodeUpdated WHEN 1 THEN @PostalCode ELSE address_postal_code END
WHERE id = @AddressID