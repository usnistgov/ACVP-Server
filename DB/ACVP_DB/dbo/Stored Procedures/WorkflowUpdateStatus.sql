CREATE PROCEDURE [dbo].[WorkflowUpdateStatus]

	 @WorkflowItemId bigint
	,@WorkflowStatusId int

AS

SET NOCOUNT ON

UPDATE dbo.WorkflowItems
SET	 WorkflowStatusId = @WorkflowStatusId
	,LastUpdatedDate = CURRENT_TIMESTAMP
WHERE WorkflowItemId = @WorkflowItemId