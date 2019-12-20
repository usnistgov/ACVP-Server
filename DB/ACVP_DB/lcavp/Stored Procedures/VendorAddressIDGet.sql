CREATE PROCEDURE [lcavp].[VendorAddressIDGet]

	 @VendorID bigint
	,@Street1 nvarchar(1024)
	,@Street2 nvarchar(1024)
	,@Street3 nvarchar(1024)
	,@Locality nvarchar(1024)
	,@Region nvarchar(1024)
	,@PostalCode nvarchar(128)
	,@Country nvarchar(128)

AS
	SET NOCOUNT ON

	SELECT id AS AddressID
	FROM val.[ADDRESS]
	WHERE organization_id = @VendorID
	  AND (@Street1 IS NULL OR @Street1 = address_street1)
	  AND (@Street2 IS NULL OR @Street2 = address_street2)
	  AND (@Street3 IS NULL OR @Street3 = address_street3)
	  AND (@Locality IS NULL OR @Locality = address_locality)
	  AND (@Region IS NULL OR @Region = address_region)
	  AND (@PostalCode IS NULL OR @PostalCode = address_postal_code)
	  AND (@Country IS NULL OR @Country = address_country)


