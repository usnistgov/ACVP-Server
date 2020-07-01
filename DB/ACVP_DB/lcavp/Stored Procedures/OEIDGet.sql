CREATE PROCEDURE [lcavp].[OEIDGet]

	 @Algorithm varchar(50)
	,@ValidationNumber int
	,@OEname nvarchar(MAX)
AS
	SET NOCOUNT ON

	SELECT DISTINCT A.OEId
	FROM dbo.ValidationSources S
		INNER JOIN
		dbo.Validations V ON V.ValidationSourceId = S.ValidationSourceId
								AND S.Prefix = @Algorithm
								AND V.ValidationNumber = @ValidationNumber
		INNER JOIN
		dbo.ValidationOEAlgorithms A ON A.ValidationId = V.ValidationId
		INNER JOIN
		dbo.OEs OE ON OE.OEId = A.OEId
				  AND OE.[Name] = @OEname


