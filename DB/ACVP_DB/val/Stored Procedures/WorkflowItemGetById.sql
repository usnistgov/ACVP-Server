CREATE PROCEDURE [val].[WorkflowItemGetById]
	
	@WorkflowItemId BIGINT

AS

BEGIN
	SET NOCOUNT ON;

    SELECT	 w.id AS Id
			,w.APIActionID AS APIActionId
			,json_blob AS JsonBlob
			,w.[status] AS [Status]
			,r.id as RequestId
	FROM	val.WORKFLOW w
	INNER	JOIN acvp.REQUEST r on w.id = r.workflow_id
	WHERE	w.id = @WorkflowItemId

END