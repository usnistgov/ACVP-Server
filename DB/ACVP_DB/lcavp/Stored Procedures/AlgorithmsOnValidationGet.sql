
CREATE PROCEDURE [lcavp].[AlgorithmsOnValidationGet]

	@ValidationRecordID bigint

AS

SET NOCOUNT ON

SELECT SA.algorithm_id
FROM val.VALIDATION_RECORD VR
	INNER JOIN
	val.VALIDATION_SCENARIO S ON S.record_id = VR.id
							 AND VR.id = @ValidationRecordID
	INNER JOIN
	val.VALIDATION_SCENARIO_ALGORITHM SA ON SA.scenario_id = S.id

