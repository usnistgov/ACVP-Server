CREATE PROCEDURE [val].[DependencyAttributeDeleteAll]

	@DependencyID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.VALIDATION_OE_DEPENDENCY_ATTRIBUTE
WHERE validation_oe_dependency_id = @DependencyID