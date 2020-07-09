CREATE PROCEDURE [dbo].[PersonExists]
	
	@PersonId BIGINT,
	@Exists BIT OUTPUT

AS

SET NOCOUNT ON

SET @Exists = CASE WHEN EXISTS (
	SELECT	1
	FROM	dbo.People
	WHERE	PersonId = @PersonId
) THEN 1 ELSE 0 END