CREATE PROCEDURE [dbo].[WorkflowItemGetById]
	
	@WorkflowItemId BIGINT

AS

BEGIN
	SET NOCOUNT ON;

    SELECT	 w.WorkflowItemId
			,w.APIActionID
			,w.JsonBlob
			,w.WorkflowStatusId
			,r.RequestId
			,w.AcceptId
	FROM dbo.WorkflowItems w
		INNER JOIN
		dbo.Requests r ON w.WorkflowItemId = r.WorkflowItemId
					  AND w.WorkflowItemId = @WorkflowItemId

END