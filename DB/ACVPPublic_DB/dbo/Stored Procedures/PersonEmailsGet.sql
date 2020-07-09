CREATE PROCEDURE [dbo].[PersonEmailsGet]

	@PersonId bigint

AS

SET NOCOUNT ON

SELECT	 EmailAddress
        ,OrderIndex
FROM dbo.PersonEmails
WHERE PersonId = @PersonId
ORDER BY OrderIndex