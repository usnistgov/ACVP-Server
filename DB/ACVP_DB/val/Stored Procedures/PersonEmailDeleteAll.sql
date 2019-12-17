CREATE PROCEDURE [val].[PersonEmailDeleteAll]

	@PersonID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.PERSON_EMAIL
WHERE person_id = @PersonID