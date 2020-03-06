CREATE PROCEDURE [val].[PrerequisitesForScenarioAlgorithmDelete]

	@ScenarioAlgorithmId bigint

AS
	
SET NOCOUNT ON

DELETE
FROM val.VALIDATION_PREREQUISITE
WHERE scenario_algorithm_id = @ScenarioAlgorithmId
