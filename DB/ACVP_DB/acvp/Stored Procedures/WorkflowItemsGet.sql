CREATE PROCEDURE [acvp].[WorkflowItemsGet]
	
	@PageSize INT,
	@Page INT,
	@WorkflowItemId BIGINT = NULL,
	@APIActionId TINYINT = NULL,
	@RequestId BIGINT = NULL,
	@TotalRecords BIGINT OUTPUT

AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT	@TotalRecords = COUNT_BIG(1)
	FROM	val.WORKFLOW w
	INNER	JOIN acvp.REQUEST r on w.id = r.workflow_id
	WHERE	1=1
		AND (@WorkflowItemId IS NULL OR CAST(w.id as varchar) LIKE '%' + CAST(@WorkflowItemId as varchar) + '%')
		AND	(@APIActionId IS NULL OR w.APIActionID = @APIActionId)
		AND	(@RequestId IS NULL OR CAST(r.id as varchar) LIKE '%' + CAST(@RequestId as varchar) + '%')

    SELECT	w.id as WorkflowItemId
			, r.id as RequestId
			, created_on as Submitted
			, ISNULL(lab_name, 'NIST') as Submitter
			, CONCAT('ACVP ', r.id) as SubmissionId 
			, ISNULL(APIActionID, 0) as ApiAction
			, w.status as Status
	FROM	val.WORKFLOW w
	INNER	JOIN acvp.REQUEST r on w.id = r.workflow_id
	WHERE	1=1
		AND (@WorkflowItemId IS NULL OR CAST(w.id as varchar) LIKE '%' + CAST(@WorkflowItemId as varchar) + '%')
		AND	(@APIActionId IS NULL OR w.ApiActionID = @APIActionId)
		AND	(@RequestId IS NULL OR CAST(r.id as varchar) LIKE '%' + CAST(@RequestId as varchar) + '%')
	ORDER	BY w.id DESC
	OFFSET (@Page - 1) * @PageSize ROWS
	FETCH NEXT @PageSize ROWS ONLY

END