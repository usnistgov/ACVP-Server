CREATE PROCEDURE [val].[ScenarioAlgorithmDelete]

	@ScenarioAlgorithmId bigint

AS

SET NOCOUNT ON

DELETE
FROM val.VALIDATION_SCENARIO_ALGORITHM
WHERE id = @ScenarioAlgorithmId
