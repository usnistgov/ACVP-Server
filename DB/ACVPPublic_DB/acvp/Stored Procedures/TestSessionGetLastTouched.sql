CREATE PROCEDURE [acvp].[TestSessionGetLastTouched]

    @TestSessionId BIGINT
	
AS

SET NOCOUNT ON

SELECT LastTouched
FROM [acvp].[TEST_SESSION]
WHERE id = @TestSessionId