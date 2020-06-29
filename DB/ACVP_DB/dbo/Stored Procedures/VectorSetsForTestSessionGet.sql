CREATE PROCEDURE [dbo].[VectorSetsForTestSessionGet]
	
	@TestSessionId bigint

AS

SET NOCOUNT ON

SELECT	 VectorSetId
		,AlgorithmId
		,VectorSetStatusId
		,ErrorMessage
FROM dbo.VectorSets
WHERE TestSessionId = @TestSessionId
