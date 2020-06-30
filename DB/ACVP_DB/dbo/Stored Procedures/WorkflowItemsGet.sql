CREATE PROCEDURE [dbo].[WorkflowItemsGet]
	
	@PageSize INT,
	@Page INT,
	@WorkflowItemId BIGINT = NULL,
	@APIActionID TINYINT = NULL,
	@RequestId BIGINT = NULL,
	@WorkflowStatusId TINYINT = NULL,
	@TotalRecords BIGINT OUTPUT

AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT	@TotalRecords = COUNT_BIG(1)
	FROM dbo.WorkflowItems w
		INNER JOIN
		dbo.Requests r on r.WorkflowItemId = w.WorkflowItemId
	WHERE	1=1
		AND (@WorkflowStatusId IS NULL OR w.WorkflowStatusId = @WorkflowStatusId)
		AND (@WorkflowItemId IS NULL OR CAST(w.WorkflowItemId as varchar) LIKE '%' + CAST(@WorkflowItemId as varchar) + '%')
		AND	(@APIActionId IS NULL OR w.APIActionID = @APIActionId)
		AND	(@RequestId IS NULL OR CAST(r.RequestId as varchar) LIKE '%' + CAST(@RequestId as varchar) + '%')

    SELECT	 w.WorkflowItemId
			,r.RequestId
			,ISNULL(APIActionID, 0) as APIAction
			,CONCAT('ACVP ', r.RequestId) as SubmissionId
			,ISNULL(w.LabName, 'NIST') as Submitter 
			,w.CreatedOn AS Submitted
			,w.WorkflowStatusId AS [Status]
	FROM dbo.WorkflowItems w
		INNER JOIN
		dbo.Requests r on r.WorkflowItemId = w.WorkflowItemId
	WHERE	1=1
		AND (@WorkflowStatusId IS NULL OR w.WorkflowStatusId = @WorkflowStatusId)
		AND (@WorkflowItemId IS NULL OR CAST(w.WorkflowItemId as varchar) LIKE '%' + CAST(@WorkflowItemId as varchar) + '%')
		AND	(@APIActionId IS NULL OR w.APIActionID = @APIActionId)
		AND	(@RequestId IS NULL OR CAST(r.RequestId as varchar) LIKE '%' + CAST(@RequestId as varchar) + '%')
	ORDER BY w.WorkflowItemId DESC
	OFFSET (@Page - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY

END