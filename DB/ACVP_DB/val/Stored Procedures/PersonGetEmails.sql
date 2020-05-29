CREATE PROCEDURE [val].[PersonGetEmails]

	@PersonID bigint

AS

SET NOCOUNT ON

SELECT  order_index AS OrderIndex,
		email_address AS EmailAddress
FROM [val].[PERSON_EMAIL]
WHERE person_id = @PersonID
ORDER BY order_index