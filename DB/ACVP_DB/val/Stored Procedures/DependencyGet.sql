CREATE PROCEDURE [val].[DependencyGet]

	@DependencyID bigint

AS

SET NOCOUNT ON

SELECT	 dependency_type AS [Type]
		,[name] AS [Name]
		,[description] AS [Description]
FROM val.VALIDATION_OE_DEPENDENCY
WHERE id = @DependencyID