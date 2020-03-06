CREATE PROCEDURE [val].[ScenarioAlgorithmInsert]

	 @ScenarioId bigint
	,@AlgorithmId bigint

AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_SCENARIO_ALGORITHM (algorithm_id, scenario_id)
VALUES (@AlgorithmId, @ScenarioId)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS ScenarioAlgorithmId

