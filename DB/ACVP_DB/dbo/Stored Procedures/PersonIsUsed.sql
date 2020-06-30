CREATE PROCEDURE [dbo].[PersonIsUsed]

	@PersonId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
			WHEN EXISTS(SELECT NULL
						FROM dbo.ImplementationContacts
						WHERE PersonId = @PersonId) THEN 1
			WHEN EXISTS(SELECT NULL
						FROM dbo.ACVPUsers
						WHERE PersonId = @PersonId) THEN 1
			ELSE 0
		   END AS bit) AS IsUsed