CREATE PROCEDURE [val].[DependencyIsUsed]

	@DependencyID bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
			WHEN EXISTS(SELECT NULL
						FROM val.VALIDATION_OE_DEPENDENCY_LINK
						WHERE dependency_id = @DependencyID) THEN 1
			ELSE 0
		   END AS bit) AS IsUsed