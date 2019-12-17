
-- 3. Create View
CREATE VIEW [acvp].[REQUEST_WORKFLOW]
WITH SCHEMABINDING
AS

SELECT request.id, request.action_id, request.user_id, workflow.created_on, workflow.type, workflow.status, workflow.accept_id
FROM acvp.REQUEST request
	INNER JOIN val.WORKFLOW workflow ON workflow.id = request.workflow_id

