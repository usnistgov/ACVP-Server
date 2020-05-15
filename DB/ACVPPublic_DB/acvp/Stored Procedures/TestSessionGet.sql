CREATE PROCEDURE [acvp].[TestSessionGet]
    @ID BIGINT
	
AS

SELECT id AS ID
    ,created_on as CreatedOn
    ,[sample] as [Sample]
    ,TestSessionStatusId
--FROM [acvp].[TestSession]
FROM [acvp].[TEST_SESSION]
WHERE @ID = id
ORDER BY created_on