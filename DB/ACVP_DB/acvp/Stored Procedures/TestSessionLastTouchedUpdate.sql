CREATE PROCEDURE [acvp].[TestSessionLastTouchedUpdate]
	
	 @TestSessionId bigint

AS

SET NOCOUNT ON

UPDATE acvp.TEST_SESSION
SET LastTouched = CURRENT_TIMESTAMP
WHERE id = @TestSessionId