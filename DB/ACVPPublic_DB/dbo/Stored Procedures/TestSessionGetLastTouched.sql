CREATE PROCEDURE [dbo].[TestSessionGetLastTouched]
	
	@TestSessionId BIGINT
	
AS

SET NOCOUNT ON

SELECT LastTouched
FROM dbo.TestSessions
WHERE TestSessionId = @TestSessionId