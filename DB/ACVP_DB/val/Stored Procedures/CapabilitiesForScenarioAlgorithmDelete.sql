CREATE PROCEDURE [val].[CapabilitiesForScenarioAlgorithmDelete]

	@ScenarioAlgorithmId bigint

AS
	
SET NOCOUNT ON

DELETE
FROM val.VALIDATION_CAPABILITY
WHERE scenario_algorithm_id = @ScenarioAlgorithmId
