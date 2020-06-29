-- With new public, should be named acvp.Request as the replicated table name
CREATE VIEW [dbo].[RequestWorkflow]
WITH SCHEMABINDING
AS

SELECT	 R.RequestId
		,R.ACVPUserId
		,W.CreatedOn
		,W.WorkflowStatusId
		,W.AcceptId
		,W.APIActionID
FROM dbo.Requests R
	INNER JOIN 
	dbo.WorkflowItems W ON W.WorkflowItemId = R.WorkflowItemId

GO

CREATE UNIQUE CLUSTERED INDEX [PK_ACVP_REQUEST_WORKFLOW] ON [dbo].[RequestWorkflow]
(
	[RequestId] ASC
)
GO