
CREATE PROCEDURE [lcavp].[UpdateValidationIDForSubmissionLogID]

	 @SubmissionLogID int
	,@ValidationId bigint

AS

SET NOCOUNT ON

UPDATE lcavp.SubmissionLog
SET ValidationId = @ValidationId
WHERE SubmissionLogID = @SubmissionLogID

