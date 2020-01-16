CREATE PROCEDURE [val].[DependencyAttributesGet]

	@DependencyID bigint

AS

SET NOCOUNT ON

SELECT	id AS [ID]
		,[name] AS [Name]
		,[value] AS [Value]
FROM val.VALIDATION_OE_DEPENDENCY_ATTRIBUTE
WHERE validation_oe_dependency_id = @DependencyID