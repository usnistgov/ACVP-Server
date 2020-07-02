CREATE PROCEDURE [dbo].[WorkflowUpdateStatusAcceptID]

	 @WorkflowItemId bigint
	,@WorkflowStatusId int
	,@AcceptId bigint

AS

SET NOCOUNT ON

UPDATE dbo.WorkflowItems
SET	 WorkflowStatusId = @WorkflowStatusId
	,AcceptId = @AcceptId
	,LastUpdatedDate = CURRENT_TIMESTAMP
WHERE WorkflowItemId = @WorkflowItemID