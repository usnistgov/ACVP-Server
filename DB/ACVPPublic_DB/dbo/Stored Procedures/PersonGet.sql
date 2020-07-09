CREATE PROCEDURE [dbo].[PersonGet]

	@PersonId bigint

AS

SET NOCOUNT ON

SELECT	 PersonId
        ,FullName
        ,OrganizationId
FROM dbo.People
WHERE PersonId = @PersonId