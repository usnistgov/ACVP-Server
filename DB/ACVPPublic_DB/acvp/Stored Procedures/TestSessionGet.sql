CREATE PROCEDURE [acvp].[TestSessionGet]
    @UserID BIGINT,
    @ID BIGINT
    
AS

SELECT id AS ID
    ,created_on as CreatedOn
    ,disposition as Disposition
    ,passed_date as PassedDate
    ,published as Published
    ,[sample] as [Sample]
    ,publishable as Publishable
--FROM [acvp].[TestSession]
FROM [acvp].[TEST_SESSION]
WHERE user_id = @UserID AND @ID = id