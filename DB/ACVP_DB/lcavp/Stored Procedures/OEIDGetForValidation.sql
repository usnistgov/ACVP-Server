CREATE PROCEDURE [lcavp].[OEIDGetForValidation]

	 @ValidationId bigint
	,@OEname nvarchar(MAX)

AS

	SET NOCOUNT ON

	SELECT DISTINCT A.OEId
	FROM dbo.ValidationOEAlgorithms A
		INNER JOIN
		dbo.OEs OE ON OE.OEId = A.OEId
				  AND OE.[Name] = @OEname
				  AND A.ValidationId = @ValidationId

