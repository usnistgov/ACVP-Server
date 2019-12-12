CREATE PROCEDURE [val].[DependencyDelete]

	@DependencyID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.VALIDATION_OE_DEPENDENCY
WHERE id = @DependencyID