CREATE PROCEDURE [val].[PersonDelete]

	@PersonID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.PERSON
WHERE id = @PersonID