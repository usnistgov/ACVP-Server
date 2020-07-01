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

	SELECT AddressId
	FROM dbo.Addresses
	WHERE OrganizationId = @VendorID
	  AND (@Street1 IS NULL OR @Street1 = Street1)
	  AND (@Street2 IS NULL OR @Street2 = Street2)
	  AND (@Street3 IS NULL OR @Street3 = Street3)
	  AND (@Locality IS NULL OR @Locality = Locality)
	  AND (@Region IS NULL OR @Region = Region)
	  AND (@PostalCode IS NULL OR @PostalCode = PostalCode)
	  AND (@Country IS NULL OR @Country = Country)


