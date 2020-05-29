CREATE PROCEDURE [val].[OEsForValidationGet]

	@ValidationId bigint

AS

SET NOCOUNT ON

SELECT	 DISTINCT OE.id AS Id
		,OE.[name] AS [Name]
FROM val.VALIDATION_SCENARIO S 
	INNER JOIN
	val.VALIDATION_SCENARIO_OE_LINK L ON L.scenario_id = S.id
									 AND S.record_id = @ValidationId
	INNER JOIN
	val.VALIDATION_OE OE ON OE.id = L.validation_oe_id
ORDER BY OE.[name]