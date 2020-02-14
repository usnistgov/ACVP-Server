
CREATE PROCEDURE [acvp].[WorkflowItemsGetByStatus]
	
	@status int

AS
BEGIN
	
	SET NOCOUNT ON;

    SELECT	id as workflowItemId
			, created_on as submitted
			, ISNULL(lab_name, 'NIST') as submitter
			, '' as submissionId 
			, ISNULL(APIActionID, 0) as apiAction
	FROM	val.WORKFLOW w
	WHERE	status = @status

END