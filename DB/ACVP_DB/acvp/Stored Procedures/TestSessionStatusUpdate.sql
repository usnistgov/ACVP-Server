CREATE PROCEDURE [acvp].[TestSessionStatusUpdate]
	
	 @TestSessionId bigint
	,@TestSessionStatusId tinyint
AS

SET NOCOUNT ON

UPDATE acvp.TEST_SESSION
SET TestSessionStatusId = @TestSessionStatusId
WHERE id = @TestSessionId