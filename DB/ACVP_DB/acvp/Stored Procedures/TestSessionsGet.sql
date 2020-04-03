CREATE PROCEDURE [acvp].[TestSessionsGet]
	@PageSize INT,
	@Page INT,
	@TestSessionId BIGINT = NULL,
	@TestSessionStatus TINYINT = NULL,
	@VectorSetId BIGINT = NULL,
	@TotalRecords BIGINT OUTPUT
AS

SET NOCOUNT ON

SELECT	@TotalRecords = COUNT_BIG(1)
FROM	acvp.TEST_SESSION ts
WHERE	1=1
	AND (@TestSessionStatus IS NULL OR ts.TestSessionStatusId = @TestSessionStatus)
	AND (@TestSessionId IS NULL OR ts.id = @TestSessionId)
	AND	(@VectorSetId IS NULL OR EXISTS (SELECT 1 FROM acvp.VECTOR_SET vs WHERE vs.test_session_id = ts.id AND vs.id = @VectorSetId))

SELECT	id as TestSessionId, created_on AS Created, TestSessionStatusId as [Status]
FROM	acvp.TEST_SESSION ts
WHERE	1=1
	AND (@TestSessionStatus IS NULL OR ts.TestSessionStatusId = @TestSessionStatus)
	AND (@TestSessionId IS NULL OR ts.id = @TestSessionId)
	AND	(@VectorSetId IS NULL OR EXISTS (SELECT 1 FROM acvp.VECTOR_SET vs WHERE vs.test_session_id = ts.id AND vs.id = @VectorSetId))
ORDER	BY id DESC
OFFSET (@Page - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY

OPTION (RECOMPILE)