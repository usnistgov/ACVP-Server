CREATE PROCEDURE [acvp].[RequestGetFromUser]
	@UserID bigint
AS

SET NOCOUNT ON

SELECT RequestID
    ,APIAction
    ,[Status]
    ,ApprovedID
	,Created
FROM [acvp].[Request]
WHERE UserID = @UserID
ORDER BY Created