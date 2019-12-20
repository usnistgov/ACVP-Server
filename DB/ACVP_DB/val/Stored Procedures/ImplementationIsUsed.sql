CREATE PROCEDURE [val].[ImplementationIsUsed]

	@ImplementationID bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
			WHEN EXISTS(SELECT NULL
						FROM val.VALIDATION_RECORD
						WHERE product_information_id = @ImplementationID) THEN 1
			ELSE 0
		   END AS bit) AS IsUsed