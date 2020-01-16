CREATE PROCEDURE [val].[ImplementationDelete]

	@ImplementationID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.PRODUCT_INFORMATION
WHERE id = @ImplementationID