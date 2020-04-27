CREATE PROCEDURE [val].[PersonPhonesGet]

	@PersonID bigint

AS

SET NOCOUNT ON

SELECT	phone_number AS PhoneNumber
        ,phone_number_type AS PhoneType
        ,order_index AS OrderIndex
FROM val.PERSON_PHONE
WHERE person_id = @PersonID
ORDER BY OrderIndex