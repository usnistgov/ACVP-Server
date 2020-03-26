CREATE PROCEDURE [val].[WorkflowItemGetById]
	
	@WorkflowItemId BIGINT

AS

BEGIN
	SET NOCOUNT ON;

    SELECT	 id AS Id
			,APIActionID AS APIActionId
			,json_blob AS JsonBlob
			,[status] AS [Status]
	FROM	val.WORKFLOW
	WHERE	id = @WorkflowItemId

END