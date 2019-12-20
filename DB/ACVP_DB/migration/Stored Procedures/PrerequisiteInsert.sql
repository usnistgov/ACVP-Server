CREATE PROCEDURE [migration].[PrerequisiteInsert]
	 @ScenarioAlgorithmID bigint
	,@ValidationNumber bigint
	,@FamilyID bigint
AS
	SET NOCOUNT ON

	INSERT INTO val.VALIDATION_PREREQUISITE (scenario_algorithm_id, record_id, requirement)
	SELECT @ScenarioAlgorithmID, VR.id, S.prefix
	FROM val.VALIDATION_RECORD VR
		INNER JOIN
		val.VALIDATION_SOURCE S ON S.id = VR.source_id
								AND VR.validation_id = @ValidationNumber
		INNER JOIN
		ref.LEGACY_CRYPTO_ALGORITHM_FAMILY F ON F.name = S.prefix
											AND F.id = @FamilyID


