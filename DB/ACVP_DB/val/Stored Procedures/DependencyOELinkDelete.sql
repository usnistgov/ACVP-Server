CREATE PROCEDURE [val].[DependencyOELinkDelete]

	 @DependencyID bigint
	,@OEID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.VALIDATION_OE_DEPENDENCY_LINK
WHERE dependency_id = @DependencyID
  AND validation_oe_id = @OEID