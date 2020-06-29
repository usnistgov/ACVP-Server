CREATE PROCEDURE [dbo].[TestSessionLastTouchedUpdate]
	
	 @TestSessionId bigint

AS

SET NOCOUNT ON

UPDATE dbo.TestSessions
SET LastTouched = CURRENT_TIMESTAMP
WHERE TestSessionId = @TestSessionId