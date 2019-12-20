
CREATE PROCEDURE [lcavp].[FilesToArchiveGet]

AS

SET NOCOUNT ON

SELECT	 SubmissionLogID
		,ZipFileName
FROM lcavp.SubmissionLog
WHERE ISNULL(Archived, 0) = 0
  AND DATEDIFF(MINUTE, ProcessedDate, CURRENT_TIMESTAMP) > 60
ORDER BY SubmissionLogID

