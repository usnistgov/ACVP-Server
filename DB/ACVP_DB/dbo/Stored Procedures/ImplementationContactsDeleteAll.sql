CREATE PROCEDURE [dbo].[ImplementationContactsDeleteAll]

	@ImplementationId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.ImplementationContacts
WHERE ImplementationId = @ImplementationId