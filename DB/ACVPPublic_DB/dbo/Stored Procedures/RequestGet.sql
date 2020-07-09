CREATE PROCEDURE [dbo].[RequestGet]

	@RequestId bigint

AS

SET NOCOUNT ON

SELECT	 RequestId AS RequestID
		,APIActionId AS APIAction
		,WorkflowStatusId AS [Status]
		,AcceptId AS ApprovedID
		,CreatedOn
FROM dbo.RequestWorkflow
WHERE RequestId = @RequestId