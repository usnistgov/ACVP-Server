CREATE PROCEDURE [dbo].[ValidationOEsGet]
	
	@ValidationId bigint

AS

SET NOCOUNT ON

SELECT DISTINCT OEId
FROM dbo.ValidationOEAlgorithms
WHERE ValidationId = @ValidationId
  AND Active = 1
ORDER BY OEId
