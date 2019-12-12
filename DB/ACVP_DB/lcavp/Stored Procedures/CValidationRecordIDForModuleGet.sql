CREATE PROCEDURE [lcavp].[CValidationRecordIDForModuleGet]

	@ModuleID bigint
	
AS
	SET NOCOUNT ON

	SELECT VR.id AS ValidationRecordID
	FROM val.VALIDATION_RECORD VR
	WHERE source_id = 18 -- LCAVP
	  AND product_information_id = @ModuleID



