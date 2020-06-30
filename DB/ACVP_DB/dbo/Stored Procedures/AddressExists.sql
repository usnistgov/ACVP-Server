CREATE PROCEDURE [dbo].[AddressExists]
	
	@AddressId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
		WHEN EXISTS (SELECT NULL
					 FROM dbo.Addresses
					 WHERE AddressId = @AddressId) THEN 1
		ELSE 0
		END AS bit) AS [Exists]