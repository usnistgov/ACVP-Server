CREATE PROCEDURE [lcavp].[CValidationIDForImplementationGet]

	@ImplementationId bigint
	
AS
	SET NOCOUNT ON

	SELECT ValidationId
	FROM dbo.Validations
	WHERE ValidationSourceId = 18
	  AND ImplementationId = @ImplementationId



