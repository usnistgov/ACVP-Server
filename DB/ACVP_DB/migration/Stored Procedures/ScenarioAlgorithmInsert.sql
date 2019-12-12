CREATE PROCEDURE [migration].[ScenarioAlgorithmInsert]
	 @ScenarioID bigint
	,@AlgorithmID bigint
	,@ScenarioAlgorithmID bigint OUTPUT
AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_SCENARIO_ALGORITHM (algorithm_id, scenario_id)
VALUES (@AlgorithmID, @ScenarioID)

SET @ScenarioAlgorithmID = SCOPE_IDENTITY()


