CREATE PROCEDURE [val].[AddressExists]
	
	@AddressId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
		WHEN EXISTS (SELECT NULL
					 FROM val.[ADDRESS]
					 WHERE id = @AddressId) THEN 1
		ELSE 0
		END AS bit) AS [Exists]