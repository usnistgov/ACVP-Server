CREATE PROCEDURE [dbo].[ValidationsForImplementationGet]

	@ImplementationId bigint

AS

SET NOCOUNT ON

SELECT	 ValidationId
		,ValidationSourceId
FROM dbo.Validations
WHERE ImplementationId = @ImplementationId
