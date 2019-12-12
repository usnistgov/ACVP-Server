
CREATE PROCEDURE [lcavp].[PersonPhoneNumbersGet]

	@PersonID bigint

AS

SET NOCOUNT ON

SELECT	 order_index AS OrderIndex
		,phone_number AS PhoneNumber
		,phone_number_type AS [Type]
FROM val.PERSON_PHONE
WHERE person_id = @PersonID
ORDER BY order_index

