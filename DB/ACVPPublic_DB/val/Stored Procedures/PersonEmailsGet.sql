CREATE PROCEDURE [val].[PersonEmailsGet]

	@PersonID bigint

AS

SET NOCOUNT ON

SELECT	email_address AS EmailAddress
        ,order_index AS OrderIndex
FROM val.PERSON_EMAIL
WHERE person_id = @PersonID
ORDER BY OrderIndex