CREATE PROCEDURE [dbo].[TestSessionStatusUpdate]
	
	 @TestSessionId bigint
	,@TestSessionStatusId tinyint

AS

SET NOCOUNT ON

UPDATE dbo.TestSessions
SET TestSessionStatusId = @TestSessionStatusId
WHERE TestSessionId = @TestSessionId