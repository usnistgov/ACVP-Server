CREATE PROCEDURE [dbo].[OEsForValidationGet]

	@ValidationId bigint

AS

SET NOCOUNT ON

SELECT	 DISTINCT OE.OEId AS ID
		,OE.[Name]
FROM dbo.ValidationOEAlgorithms VOA
	INNER JOIN
	dbo.OEs OE ON OE.OEId = VOA.OEId
ORDER BY OE.[Name]