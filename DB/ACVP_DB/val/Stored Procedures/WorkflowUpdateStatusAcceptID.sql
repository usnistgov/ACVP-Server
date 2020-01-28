CREATE PROCEDURE [val].[WorkflowUpdateStatusAcceptID]

	 @WorkflowItemID bigint
	,@Status int
	,@AcceptID bigint

AS

SET NOCOUNT ON

UPDATE val.WORKFLOW
SET	 [status] = @Status
	,accept_id = @AcceptID
	,LastUpdatedDate = CURRENT_TIMESTAMP
WHERE id = @WorkflowItemID