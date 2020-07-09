CREATE PROCEDURE [dbo].[RequestGetFromUser]

	 @ACVPUserId bigint
	,@Offset bigint
	,@Limit bigint
	,@TotalRecords bigint output

AS

SET NOCOUNT ON

SELECT	@TotalRecords = COUNT_BIG(1)
FROM	dbo.RequestWorkflow
WHERE	ACVPUserId = @ACVPUserId

SELECT	 RequestId AS RequestID
		,APIActionId AS APIAction
		,WorkflowStatusId AS [Status]
		,AcceptId AS ApprovedID
		,CreatedOn
FROM dbo.RequestWorkflow
WHERE ACVPUserId = @ACVPUserId
ORDER BY CreatedOn
OFFSET @Offset ROWS
FETCH NEXT @Limit ROWS ONLY;