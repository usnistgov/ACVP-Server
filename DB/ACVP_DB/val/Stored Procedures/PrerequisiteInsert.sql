CREATE PROCEDURE [val].[PrerequisiteInsert]

	 @ScenarioAlgorithmId bigint
	,@ValidationId bigint
	,@Requirement nvarchar(2048)

AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_PREREQUISITE(
	 scenario_algorithm_id
	,record_id
	,requirement
)
VALUES (
	 @ScenarioAlgorithmId
	,@ValidationId
	,@Requirement
)

SELECT SCOPE_IDENTITY() AS PrerequisiteId

