CREATE PROCEDURE val.PersonGetPhones

	@PersonID bigint

AS

SET NOCOUNT ON

SELECT	 phone_number
		,phone_number_type
		,order_index AS OrderIndex
FROM val.PERSON_PHONE
WHERE person_id = @PersonID
ORDER BY order_index