CREATE PROCEDURE [acvp].[VectorSetsToArchiveGet]

AS

SET NOCOUNT ON

-- Everything tied to cancelled, published, or expired test sessions
SELECT VS.id AS VectorSetId
FROM acvp.TEST_SESSION TS
	INNER JOIN
	acvp.VECTOR_SET VS ON VS.test_session_id = TS.id
					  AND VS.Archived = 0
					  AND TS.TestSessionStatusId IN (1, 6, 7)		-- Cancelled, published, or expired

UNION

-- Individual vector set cancellations
SELECT id AS VectorSetId
FROM acvp.VECTOR_SET
WHERE [status] = 5		-- Cancelled
  AND Archived = 0