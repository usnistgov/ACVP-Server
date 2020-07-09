CREATE PROCEDURE [dbo].[PersonPhonesGet]

	@PersonId bigint

AS

SET NOCOUNT ON

SELECT	 PhoneNumber
        ,PhoneNumberType
        ,OrderIndex
FROM dbo.PersonPhoneNumbers
WHERE PersonId = @PersonId
ORDER BY OrderIndex