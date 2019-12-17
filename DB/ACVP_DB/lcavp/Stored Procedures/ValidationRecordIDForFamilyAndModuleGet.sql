
CREATE PROCEDURE [lcavp].[ValidationRecordIDForFamilyAndModuleGet]

	 @ModuleID bigint
	,@Algorithm varchar(50) 
	
AS
	SET NOCOUNT ON

	SELECT VR.id AS ValidationRecordID
	FROM val.VALIDATION_RECORD VR
		INNER JOIN
		val.VALIDATION_SOURCE S ON S.id = VR.source_id
								AND product_information_id = @ModuleID
								AND S.prefix = @Algorithm

