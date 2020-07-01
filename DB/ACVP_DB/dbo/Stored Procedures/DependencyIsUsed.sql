CREATE PROCEDURE [dbo].[DependencyIsUsed]

	@DependencyId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
			WHEN EXISTS(SELECT NULL
						FROM dbo.OEDependencies
						WHERE DependencyId = @DependencyId) THEN 1
			ELSE 0
		   END AS bit) AS IsUsed