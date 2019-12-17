CREATE PROCEDURE [lcavp].[OEIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	,@OEname nvarchar(MAX)
AS
	SET NOCOUNT ON

	SELECT L.validation_oe_id AS OEID
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		val.VALIDATION_RECORD VR ON VR.source_id = S.id
								AND S.prefix = @Algorithm
								AND VR.validation_id = @ValidationNumber
		INNER JOIN
		val.VALIDATION_SCENARIO R ON R.record_id = VR.id
		INNER JOIN
		val.VALIDATION_SCENARIO_OE_LINK L ON L.scenario_id = R.id
		INNER JOIN
		val.VALIDATION_OE OE ON OE.id = L.validation_oe_id
							AND OE.[name] = @OEname


