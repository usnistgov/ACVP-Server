CREATE PROCEDURE [dbo].[AlgorithmsGet]

AS

SET NOCOUNT ON

SELECT 	 AlgorithmId
		,[Name]
		,Mode
		,Revision
FROM dbo.Algorithms
WHERE Historical = 0
  AND SupportedByACVP = 1