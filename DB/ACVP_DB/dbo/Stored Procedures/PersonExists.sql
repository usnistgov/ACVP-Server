CREATE PROCEDURE [dbo].[PersonExists]
	
	@PersonId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
		WHEN EXISTS (SELECT NULL
					 FROM dbo.People
					 WHERE PersonId = @PersonId) THEN 1
		ELSE 0
		END AS bit) AS [Exists]