CREATE PROCEDURE [dbo].[ValidationOEAlgorithmsForOEGetActive]
	
	 @ValidationId bigint
	,@OEId bigint

AS

SET NOCOUNT ON

SELECT	 ValidationOEAlgorithmId
		,AlgorithmId
FROM dbo.ValidationOEAlgorithms
WHERE ValidationId = @ValidationId
  AND OEId = @OEId
  AND Active = 1