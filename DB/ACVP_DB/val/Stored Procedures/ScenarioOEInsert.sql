CREATE PROCEDURE [val].[ScenarioOEInsert]

	 @ScenarioId bigint
	,@OEId bigint

AS

SET NOCOUNT ON

INSERT INTO val.VALIDATION_SCENARIO_OE_LINK (scenario_id, validation_oe_id)
VALUES (@ScenarioId, @OEId)
