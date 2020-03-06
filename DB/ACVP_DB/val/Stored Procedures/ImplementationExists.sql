CREATE PROCEDURE [val].[ImplementationExists]
	
	@ImplementationId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
		WHEN EXISTS (SELECT NULL
					 FROM val.PRODUCT_INFORMATION
					 WHERE id = @ImplementationId) THEN 1
		ELSE 0
		END AS bit) AS [Exists]