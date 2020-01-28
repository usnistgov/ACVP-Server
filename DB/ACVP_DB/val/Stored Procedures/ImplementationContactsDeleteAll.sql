CREATE PROCEDURE [val].[ImplementationContactsDeleteAll]

	@ImplementationID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.VALIDATION_CONTACT
WHERE product_information_id = @ImplementationID