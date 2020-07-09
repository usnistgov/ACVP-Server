CREATE PROCEDURE [dbo].[TestSessionsGet]

    @ACVPUserId BIGINT
    
AS

SET NOCOUNT ON

SELECT   TestSessionId
        ,CreatedOn
        ,IsSample
        ,TestSessionStatusId
        ,LastTouched
FROM dbo.TestSessions
WHERE ACVPUserId = @ACVPUserId
ORDER BY CreatedOn