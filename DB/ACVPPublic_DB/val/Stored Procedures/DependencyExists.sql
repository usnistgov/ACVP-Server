CREATE PROCEDURE [val].[DependencyExists]
	
	@dependencyId BIGINT,
	@exists BIT OUTPUT

AS

SET NOCOUNT ON

SET @exists = CASE WHEN EXISTS (
	SELECT	1
	FROM	val.VALIDATION_OE_DEPENDENCY v
	WHERE	id = @dependencyId
) THEN 1 ELSE 0 END