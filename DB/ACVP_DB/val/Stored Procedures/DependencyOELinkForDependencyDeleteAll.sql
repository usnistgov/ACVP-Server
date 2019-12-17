CREATE PROCEDURE [val].[DependencyOELinkForDependencyDeleteAll]

	@DependencyID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.VALIDATION_OE_DEPENDENCY_LINK
WHERE dependency_id = @DependencyID