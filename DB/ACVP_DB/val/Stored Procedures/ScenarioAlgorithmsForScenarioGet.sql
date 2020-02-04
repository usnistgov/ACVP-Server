CREATE PROCEDURE [val].[ScenarioAlgorithmsForScenarioGet]
	
	@ScenarioId bigint

AS

SET NOCOUNT ON

SELECT	 id AS ScenarioAlgorithmId
		,algorithm_id AS AlgorithmId
FROM val.VALIDATION_SCENARIO_ALGORITHM
WHERE scenario_id = @ScenarioId
