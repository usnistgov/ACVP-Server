CREATE PROCEDURE [lcavp].[ValidationIDForSubmissionIDGet]

	@SubmissionID nvarchar(100)
	
AS
	SET NOCOUNT ON

	SELECT TOP 1 ValidationID
	FROM lcavp.SubmissionLog
	WHERE SubmissionID = @SubmissionID


