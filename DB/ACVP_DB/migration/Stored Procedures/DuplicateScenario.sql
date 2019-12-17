CREATE PROCEDURE [migration].[DuplicateScenario]
	 @ScenarioID bigint
	,@NewScenarioID bigint OUTPUT
AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_SCENARIO (record_id)
SELECT record_id
FROM val.VALIDATION_SCENARIO
WHERE id = @ScenarioID

SET @NewScenarioID = SCOPE_IDENTITY()

INSERT INTO val.VALIDATION_SCENARIO_OE_LINK (scenario_id, validation_oe_id)
SELECT @NewScenarioID, validation_oe_id
FROM val.VALIDATION_SCENARIO_OE_LINK
WHERE scenario_id = @ScenarioID


