CREATE PROCEDURE [dbo].[AddressUpdate]
	 
	 @AddressId bigint
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

UPDATE dbo.Addresses
SET  OrderIndex = @OrderIndex
	,Street1 = CASE @Street1Updated WHEN 1 THEN @Street1 ELSE Street1 END
	,Street2 = CASE @Street2Updated WHEN 1 THEN @Street2 ELSE Street2 END
	,Street3 = CASE @Street3Updated WHEN 1 THEN @Street3 ELSE Street3 END
	,Locality = CASE @LocalityUpdated WHEN 1 THEN @Locality ELSE Locality END
	,Region = CASE @RegionUpdated WHEN 1 THEN @Region ELSE Region END
	,Country = CASE @CountryUpdated WHEN 1 THEN @Country ELSE Country END
	,PostalCode = CASE @PostalCodeUpdated WHEN 1 THEN @PostalCode ELSE PostalCode END
WHERE AddressId = @AddressId