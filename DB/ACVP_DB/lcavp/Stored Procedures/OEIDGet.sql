CREATE PROCEDURE [lcavp].[OEIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	,@OEname nvarchar(MAX)
AS
	SET NOCOUNT ON

	SELECT DISTINCT A.OEId
	FROM val.VALIDATION_SOURCE S
		INNER JOIN
		dbo.Validations V ON V.ValidationSourceId = S.id
								AND S.prefix = @Algorithm
								AND V.ValidationNumber = @ValidationNumber
		INNER JOIN
		dbo.ValidationOEAlgorithms A ON A.ValidationId = V.ValidationId
		INNER JOIN
		val.VALIDATION_OE OE ON OE.id = A.OEId
							AND OE.[name] = @OEname


