CREATE PROCEDURE [dbo].[TestSessionGet]

    @TestSessionId BIGINT
	
AS

SET NOCOUNT ON

SELECT   TestSessionId
        ,CreatedOn
        ,IsSample
        ,TestSessionStatusId
        ,LastTouched
FROM dbo.TestSessions
WHERE TestSessionId = @TestSessionId
ORDER BY CreatedOn