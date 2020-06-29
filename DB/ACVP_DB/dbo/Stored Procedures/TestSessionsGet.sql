CREATE PROCEDURE [dbo].[TestSessionsGet]

	@PageSize INT,
	@Page INT,
	@TestSessionId BIGINT = NULL,
	@TestSessionStatusId TINYINT = NULL,
	@VectorSetId BIGINT = NULL,
	@TotalRecords BIGINT OUTPUT

AS

SET NOCOUNT ON

SELECT	@TotalRecords = COUNT_BIG(1)
FROM	dbo.TestSessions ts
WHERE	(@TestSessionStatusId IS NULL OR ts.TestSessionStatusId = @TestSessionStatusId)
	AND (@TestSessionId IS NULL OR ts.TestSessionId = @TestSessionId)
	AND	(@VectorSetId IS NULL OR EXISTS (SELECT 1 FROM acvp.VECTOR_SET vs WHERE vs.test_session_id = ts.id AND vs.id = @VectorSetId))

SELECT	TestSessionId, CreatedOn AS Created, TestSessionStatusId as [Status]
FROM	dbo.TestSessions ts
WHERE	(@TestSessionStatusId IS NULL OR ts.TestSessionStatusId = @TestSessionStatusId)
	AND (@TestSessionId IS NULL OR ts.TestSessionId = @TestSessionId)
	AND	(@VectorSetId IS NULL OR EXISTS (SELECT 1 FROM acvp.VECTOR_SET vs WHERE vs.test_session_id = ts.id AND vs.id = @VectorSetId))
ORDER BY TestSessionId DESC
OFFSET (@Page - 1) * @PageSize ROWS
FETCH NEXT @PageSize ROWS ONLY

OPTION (RECOMPILE)