
CREATE PROCEDURE [lcavp].[PersonPhoneNumbersGet]

	@PersonID bigint

AS

SET NOCOUNT ON

SELECT	 OrderIndex
		,PhoneNumber
		,PhoneNumberType
FROM dbo.PersonPhoneNumbers
WHERE PersonId = @PersonID
ORDER BY OrderIndex

