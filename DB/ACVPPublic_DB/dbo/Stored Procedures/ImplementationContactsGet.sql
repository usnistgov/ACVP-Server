CREATE PROCEDURE [dbo].[ImplementationContactsGet]

    @ImplementationId BIGINT

AS

SET NOCOUNT ON

SELECT PersonId
FROM dbo.ImplementationContacts
WHERE ImplementationId = @ImplementationId
ORDER BY OrderIndex