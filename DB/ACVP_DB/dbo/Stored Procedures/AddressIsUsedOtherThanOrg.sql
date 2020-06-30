CREATE PROCEDURE [dbo].[AddressIsUsedOtherThanOrg]

	@AddressId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
			WHEN EXISTS(SELECT NULL
						FROM dbo.Implementations
						WHERE AddressId = @AddressId) THEN 1
			ELSE 0
		   END AS bit) AS IsUsed