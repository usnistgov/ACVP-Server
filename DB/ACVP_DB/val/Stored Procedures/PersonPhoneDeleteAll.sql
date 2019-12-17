CREATE PROCEDURE [val].[PersonPhoneDeleteAll]

	@PersonID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.PERSON_PHONE
WHERE person_id = @PersonID