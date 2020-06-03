CREATE PROCEDURE [lcavp].[OEIDGetForValidation]

	 @ValidationId bigint
	,@OEname nvarchar(MAX)

AS

	SET NOCOUNT ON

	SELECT DISTINCT A.OEId
	FROM dbo.ValidationOEAlgorithms A
		INNER JOIN
		val.VALIDATION_OE OE ON OE.id = A.OEId
							AND OE.[name] = @OEname
							AND A.ValidationId = @ValidationId

