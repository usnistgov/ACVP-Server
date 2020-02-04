CREATE PROCEDURE [val].[ScenariosForValidationOEGet]
	
	 @ValidationId bigint
	,@OEId bigint

AS

SET NOCOUNT ON

SELECT S.id AS ScenarioId
FROM val.VALIDATION_SCENARIO S
	INNER JOIN
	val.VALIDATION_SCENARIO_OE_LINK L ON L.scenario_id = S.id
									 AND S.record_id = @ValidationId
									 AND L.validation_oe_id = @OEId