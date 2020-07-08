-- With new public, should be named acvp.Request as the replicated table name
CREATE VIEW [dbo].[RequestWorkflow]
WITH SCHEMABINDING
AS

SELECT	 R.RequestId
		,R.ACVPUserId
		,W.CreatedOn
		,W.WorkflowStatusId
		,W.AcceptId
		,W.APIActionId
FROM dbo.Requests R
	INNER JOIN 
	dbo.WorkflowItems W ON W.WorkflowItemId = R.WorkflowItemId

GO

CREATE UNIQUE CLUSTERED INDEX [PK_RequestWorkflow] ON [dbo].[RequestWorkflow]
(
	[RequestId] ASC
)
GO