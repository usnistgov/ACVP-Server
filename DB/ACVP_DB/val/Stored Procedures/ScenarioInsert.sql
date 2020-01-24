CREATE PROCEDURE [val].[ScenarioInsert]

	@ValidationId bigint

AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_SCENARIO (record_id) VALUES (@ValidationId)

SELECT SCOPE_IDENTITY() AS ScenarioId
