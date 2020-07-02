CREATE PROCEDURE [dbo].[PersonDelete]

	@PersonId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.People
WHERE PersonId = @PersonId