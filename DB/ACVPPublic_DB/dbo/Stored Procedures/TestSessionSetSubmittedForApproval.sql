CREATE PROCEDURE [dbo].[TestSessionSetSubmittedForApproval]
	
	@TestSessionId BIGINT

AS
BEGIN
	SET NOCOUNT ON;

    UPDATE	dbo.TestSessions
	SET		TestSessionStatusId = 5
	WHERE	TestSessionId = @TestSessionId

END