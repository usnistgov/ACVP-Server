CREATE PROCEDURE [lcavp].[SubmissionRegistrationInsert]
	 @SubmissionID int
	,@Status tinyint
	,@WorkflowTypeID int
	,@RegistrationJson nvarchar(MAX)
	,@ErrorJson nvarchar(MAX)
AS

SET NOCOUNT ON

INSERT INTO lcavp.SubmissionRegistrations (SubmissionID, [Status], RegistrationJson, ErrorJson, WorkflowTypeID)
VALUES (@SubmissionID, @Status, @RegistrationJson, @ErrorJson, @WorkflowTypeID)


