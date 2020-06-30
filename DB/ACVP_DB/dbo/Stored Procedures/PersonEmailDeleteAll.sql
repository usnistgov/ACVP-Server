CREATE PROCEDURE [dbo].[PersonEmailDeleteAll]

	@PersonId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.PersonEmails
WHERE PersonId = @PersonId