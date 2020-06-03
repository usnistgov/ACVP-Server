CREATE PROCEDURE [lcavp].[ValidationNumberForIDGet]

	@ValidationId bigint
	
AS
	SET NOCOUNT ON

	SELECT TOP 1 ValidationNumber
	FROM dbo.Validations
	WHERE ValidationId = @ValidationId


