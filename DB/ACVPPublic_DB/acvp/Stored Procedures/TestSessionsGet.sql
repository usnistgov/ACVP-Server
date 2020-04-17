CREATE PROCEDURE [acvp].[TestSessionsGet]
    @UserID BIGINT
    
AS

SELECT id AS ID
    ,created_on AS CreatedOn
    ,disposition AS Disposition
    ,passed_date AS PassedDate
    ,published AS Published
    ,[sample] AS [Sample]
    ,publishable AS Publishable
FROM [acvp].[TestSession]
WHERE user_id = @UserID
ORDER BY created_on