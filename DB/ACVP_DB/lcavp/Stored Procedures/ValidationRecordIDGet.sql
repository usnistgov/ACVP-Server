CREATE PROCEDURE [lcavp].[ValidationRecordIDGet]

	@Algorithm varchar(50)
	,@ValidationNumber int
	
AS
	SET NOCOUNT ON

	SELECT VR.id AS ValidationRecordID
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		val.VALIDATION_RECORD VR ON VR.source_id = S.id
								AND S.prefix = @Algorithm
								AND VR.validation_id = @ValidationNumber


