CREATE PROCEDURE [acvp].[RequestInsert]

	 @RequestID bigint
	,@WorkflowID bigint
	,@UserID bigint

AS

SET NOCOUNT ON

INSERT INTO acvp.REQUEST(id, workflow_id, [user_id])
VALUES (@RequestID, @WorkflowID, @UserID)
