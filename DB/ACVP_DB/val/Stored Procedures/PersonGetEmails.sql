CREATE PROCEDURE [val].[PersonGetEmails]
	@PersonID bigint
AS
	SELECT  order_index,
			email_address
	FROM [val].[PERSON_EMAIL] AS EMAIL
	WHERE EMAIL.person_id = @PersonID
	ORDER BY EMAIL.order_index