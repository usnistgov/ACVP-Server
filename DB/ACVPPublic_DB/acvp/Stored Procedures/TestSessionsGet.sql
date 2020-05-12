CREATE PROCEDURE [acvp].[TestSessionsGet]
    @UserID BIGINT
    
AS

SELECT id AS ID
    ,created_on AS CreatedOn
    ,[sample] AS [Sample]
    ,TestSessionStatusId
--FROM [acvp].[TestSession]
FROM [acvp].[TEST_SESSION]
WHERE user_id = @UserID
ORDER BY created_on