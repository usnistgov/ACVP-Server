CREATE PROCEDURE [dbo].[DependencyExists]
	
	@DependencyId BIGINT,
	@Exists BIT OUTPUT

AS

SET NOCOUNT ON

SET @Exists = CASE WHEN EXISTS (
	SELECT	1
	FROM	dbo.Dependencies
	WHERE	DependencyId = @DependencyId
) THEN 1 ELSE 0 END