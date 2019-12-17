CREATE PROCEDURE [val].[OEDependencyLinksGet]

	@OEID bigint

AS

SET NOCOUNT ON

SELECT dependency_id AS DependencyID
FROM val.VALIDATION_OE_DEPENDENCY_LINK
WHERE validation_oe_id = @OEID