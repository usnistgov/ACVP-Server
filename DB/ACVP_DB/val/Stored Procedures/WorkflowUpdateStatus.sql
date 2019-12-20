CREATE PROCEDURE [val].[WorkflowUpdateStatus]

	 @WorkflowItemID bigint
	,@Status int

AS

SET NOCOUNT ON

UPDATE val.WORKFLOW
SET	 [status] = @Status
	,LastUpdatedDate = CURRENT_TIMESTAMP
WHERE id = @WorkflowItemID