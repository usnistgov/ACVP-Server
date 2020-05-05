CREATE PROCEDURE [acvp].[TestSessionGet]
    @ID BIGINT
	
AS

SELECT id AS ID
    ,created_on as CreatedOn
    ,isNull(disposition, 0) as Disposition
    ,passed_date as PassedDate
    ,published as Published
    ,[sample] as [Sample]
    ,publishable as Publishable
--FROM [acvp].[TestSession]
FROM [acvp].[TEST_SESSION]
WHERE @ID = id
ORDER BY created_on