CREATE PROCEDURE [acvp].[WorkflowItemGetById]
	
	@workflowItemId BIGINT

AS
BEGIN
	SET NOCOUNT ON;

    SELECT	id
			, APIActionID as apiActionId
			, json_blob as jsonBlob
	FROM	val.WORKFLOW w
	WHERE	w.id = @workflowItemId

END