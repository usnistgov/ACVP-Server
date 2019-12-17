CREATE PROCEDURE [val].[PersonIsUsed]

	@PersonID bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
			WHEN EXISTS(SELECT NULL
						FROM val.VALIDATION_CONTACT
						WHERE person_id = @PersonID) THEN 1
			WHEN EXISTS(SELECT NULL
						FROM acvp.ACVP_USER
						WHERE person_id = @PersonID) THEN 1
			ELSE 0
		   END AS bit) AS IsUsed