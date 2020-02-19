CREATE PROCEDURE [val].[PersonExists]
	
	@PersonId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
		WHEN EXISTS (SELECT NULL
					 FROM val.PERSON
					 WHERE id = @PersonId) THEN 1
		ELSE 0
		END AS bit) AS [Exists]