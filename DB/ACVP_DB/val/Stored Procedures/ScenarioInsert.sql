CREATE PROCEDURE [val].[ScenarioInsert]

	@ValidationId bigint

AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_SCENARIO (record_id) VALUES (@ValidationId)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS ScenarioId
