CREATE PROCEDURE [acvp].[RequestGet]
	@RequestID bigint
AS

SET NOCOUNT ON

SELECT RequestID
    ,APIAction
    ,[Status]
    ,ApprovedID
	,Created
FROM [acvp].[Request]
WHERE RequestID = @RequestID