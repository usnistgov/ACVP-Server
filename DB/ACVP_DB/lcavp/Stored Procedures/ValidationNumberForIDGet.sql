CREATE PROCEDURE [lcavp].[ValidationNumberForIDGet]

	@ValidationId bigint
	
AS
	SET NOCOUNT ON

	SELECT TOP 1 validation_id AS ValidationNumber
	FROM val.VALIDATION_RECORD
	WHERE id = @ValidationId


