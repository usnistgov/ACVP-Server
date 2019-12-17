CREATE PROCEDURE [val].[AddressIsUsedOtherThanOrg]

	@AddressID bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
			WHEN EXISTS(SELECT NULL
						FROM val.PRODUCT_INFORMATION
						WHERE address_id = @AddressID) THEN 1
			ELSE 0
		   END AS bit) AS IsUsed