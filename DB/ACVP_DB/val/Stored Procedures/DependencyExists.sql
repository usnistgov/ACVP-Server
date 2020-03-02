CREATE PROCEDURE [val].[DependencyExists]
	
	@DependencyId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
		WHEN EXISTS (SELECT NULL
					 FROM val.VALIDATION_OE_DEPENDENCY
					 WHERE id = @DependencyId) THEN 1
		ELSE 0
		END AS bit) AS [Exists]