CREATE PROCEDURE [acvp].[TestSessionsExpire]
	
	@AgeInDays int

AS

SET NOCOUNT ON

UPDATE acvp.TEST_SESSION
SET TestSessionStatusId = 7
WHERE TestSessionStatusId NOT IN (1,6,7)	-- Not cancelled, published, or already expired
  AND LastTouched < DATEADD(DAY, -@AgeInDays, CURRENT_TIMESTAMP)