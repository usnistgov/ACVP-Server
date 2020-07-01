CREATE PROCEDURE [dbo].[DependencyGet]

	@DependencyId bigint

AS

SET NOCOUNT ON

SELECT	 DependencyType
		,[Name]
		,[Description]
FROM dbo.Dependencies
WHERE DependencyId = @DependencyId