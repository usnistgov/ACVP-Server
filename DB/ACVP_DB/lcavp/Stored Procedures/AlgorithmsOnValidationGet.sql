
CREATE PROCEDURE [lcavp].[AlgorithmsOnValidationGet]

	@ValidationId bigint

AS

SET NOCOUNT ON

SELECT DISTINCT AlgorithmId
FROM dbo.ValidationOEAlgorithms
WHERE ValidationId = @ValidationId


