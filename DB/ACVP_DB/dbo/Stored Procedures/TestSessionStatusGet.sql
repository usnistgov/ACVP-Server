CREATE PROCEDURE [dbo].[TestSessionStatusGet]
	
	@TestSessionId bigint

AS

SET NOCOUNT ON

SELECT TOP 1 TestSessionStatusId
FROM dbo.TestSessions
WHERE TestSessionId = @TestSessionId