CREATE PROCEDURE [acvp].[TestSessionIdGet]

	@VectorSetId bigint

AS

SET NOCOUNT ON

SELECT	TOP 1 test_session_id AS TestSessionId
FROM acvp.VECTOR_SET
WHERE id = @VectorSetId