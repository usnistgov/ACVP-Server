CREATE PROCEDURE [dbo].[ImplementationDelete]

	@ImplementationId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.Implementations
WHERE ImplementationId = @ImplementationId