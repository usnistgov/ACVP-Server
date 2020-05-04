


-- With new public, should be named acvp.Request as the replicated table name
CREATE VIEW [acvp].[REQUEST_WORKFLOW]
WITH SCHEMABINDING
AS

SELECT	request.id AS RequestID
		, request.user_id AS UserID
		, workflow.created_on AS Created
		, workflow.status AS [Status]
		, workflow.accept_id AS ApprovedID
		, workflow.APIActionID AS APIAction
FROM acvp.REQUEST request
	INNER JOIN val.WORKFLOW workflow ON workflow.id = request.workflow_id

