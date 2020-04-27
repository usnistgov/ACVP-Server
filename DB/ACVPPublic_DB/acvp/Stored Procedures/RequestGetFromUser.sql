CREATE PROCEDURE [acvp].[RequestGetFromUser]
	@UserID bigint
AS

SET NOCOUNT ON

SELECT id AS ID
    ,APIActionID AS APIActionID
    ,[Status] AS [Status]
    ,AcceptID AS AcceptID
FROM [acvp].[Request]
WHERE UserID = @UserID
ORDER BY CreatedOn