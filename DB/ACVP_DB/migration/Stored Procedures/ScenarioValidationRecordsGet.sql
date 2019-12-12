CREATE PROCEDURE [migration].[ScenarioValidationRecordsGet]

AS

SET NOCOUNT ON

SELECT	 S.id AS ScenarioID
		--,S.record_id AS ValidationRecordID
		,Mapping.Old_Id AS OldValidationRecordID
FROM val.VALIDATION_SCENARIO S
	INNER JOIN
	val.VALIDATION_RECORD NewVR ON NewVR.id = S.record_id
	INNER JOIN
	migration.VR_VR Mapping ON Mapping.New_Id = NewVR.id


