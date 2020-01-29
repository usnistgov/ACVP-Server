CREATE PROCEDURE [val].[ValidationsForImplementationGet]

	@ImplementationId bigint

AS

SET NOCOUNT ON

SELECT	 id AS ValidationId
		,source_id AS SourceId
FROM val.VALIDATION_RECORD
WHERE product_information_id = @ImplementationId
