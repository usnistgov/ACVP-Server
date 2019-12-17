CREATE PROCEDURE [val].[DependencyAttributeDelete]

	@DependencyAttributeID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.VALIDATION_OE_DEPENDENCY_ATTRIBUTE
WHERE id = @DependencyAttributeID