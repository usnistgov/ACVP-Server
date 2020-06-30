CREATE PROCEDURE [dbo].[PersonPhoneDeleteAll]

	@PersonId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.PersonPhoneNumbers
WHERE PersonId = @PersonId