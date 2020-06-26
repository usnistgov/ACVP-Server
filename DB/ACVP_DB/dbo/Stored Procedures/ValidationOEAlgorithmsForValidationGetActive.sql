CREATE PROCEDURE [dbo].[ValidationOEAlgorithmsForValidationGetActive]
	
	 @ValidationId bigint

AS

SET NOCOUNT ON

SELECT	 VOA.ValidationOEAlgorithmId
		,VOA.AlgorithmId
		,VOA.OEId
		,VOA.CreatedOn
		,A.DisplayName AS AlgorithmDisplayName
		,OE.[name] AS OEName
FROM dbo.ValidationOEAlgorithms VOA
	INNER JOIN
	dbo.Algorithms A ON A.AlgorithmId = VOA.AlgorithmId
					AND VOA.ValidationId = @ValidationId
					AND VOA.Active = 1
	INNER JOIN
	val.VALIDATION_OE OE ON OE.id = VOA.OEId
ORDER BY OEName, AlgorithmDisplayName, VOA.ValidationOEAlgorithmId