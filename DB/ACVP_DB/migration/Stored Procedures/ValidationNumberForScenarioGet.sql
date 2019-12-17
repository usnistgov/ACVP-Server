CREATE PROCEDURE [migration].[ValidationNumberForScenarioGet]
	@ScenarioID bigint,
	@ValidationNumber int OUTPUT
AS

SET NOCOUNT ON

SELECT top 1 @ValidationNumber = VR.validation_id
FROM val.VALIDATION_SCENARIO S
	INNER JOIN
	val.VALIDATION_RECORD VR ON VR.id = S.record_id
							AND S.id = @ScenarioID


