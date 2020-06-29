CREATE PROCEDURE [dbo].[VectorSetsToArchiveGet]

AS

SET NOCOUNT ON

-- Everything tied to cancelled, published, or expired test sessions
SELECT VS.VectorSetId
FROM dbo.TestSessions TS
	INNER JOIN
	dbo.VectorSets VS ON VS.TestSessionId = TS.TestSessionId
					  AND VS.Archived = 0
					  AND TS.TestSessionStatusId IN (1, 6, 7)		-- Cancelled, published, or expired

UNION

-- Individual vector set cancellations
SELECT VectorSetId
FROM dbo.VectorSets
WHERE VectorSetStatusId = 5		-- Cancelled
  AND Archived = 0