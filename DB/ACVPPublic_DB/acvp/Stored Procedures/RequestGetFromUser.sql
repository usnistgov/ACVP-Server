CREATE PROCEDURE [acvp].[RequestGetFromUser]
	@UserID bigint
	,@Offset bigint
	,@Limit bigint
	,@TotalRecords bigint output
AS

SET NOCOUNT ON

SELECT	@TotalRecords = COUNT_BIG(1)
FROM	[acvp].[Request]
WHERE	UserID = @UserID

SELECT RequestID
    ,APIAction
    ,[Status]
    ,ApprovedID
	,Created
FROM [acvp].[Request]
WHERE UserID = @UserID
ORDER BY Created
OFFSET @Offset ROWS
FETCH NEXT @Limit ROWS ONLY;