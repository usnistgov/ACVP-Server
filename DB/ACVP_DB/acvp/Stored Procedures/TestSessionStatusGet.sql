CREATE PROCEDURE [acvp].[TestSessionStatusGet]
	
	@TestSessionId bigint

AS

SET NOCOUNT ON

SELECT TestSessionStatusId
FROM acvp.TEST_SESSION
WHERE id = @TestSessionId