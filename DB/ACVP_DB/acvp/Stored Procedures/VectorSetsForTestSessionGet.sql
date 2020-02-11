CREATE PROCEDURE [acvp].[VectorSetsForTestSessionGet]
	
	@TestSessionId bigint

AS

SET NOCOUNT ON

SELECT	 id AS VectorSetId
		,algorithm_id AS AlgorithmId
		,[status] AS VectorSetStatusId
		,vector_error_message AS ErrorMessage
FROM acvp.VECTOR_SET
WHERE test_session_id = @TestSessionId
