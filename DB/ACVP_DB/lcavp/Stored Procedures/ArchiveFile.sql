

CREATE PROCEDURE [lcavp].[ArchiveFile]

	@SubmissionLogID int

AS

SET NOCOUNT ON

UPDATE lcavp.SubmissionLog
SET Archived = 1
WHERE SubmissionLogID = @SubmissionLogID

