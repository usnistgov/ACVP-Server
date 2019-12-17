CREATE PROCEDURE [acvp].[RequestInsert]

	 @RequestID bigint
	,@ActionID int
	,@WorkflowID bigint
	,@UserID bigint

AS

SET NOCOUNT ON

INSERT INTO acvp.REQUEST(id, action_id, workflow_id, [user_id])
VALUES (@RequestID, @ActionID, @WorkflowID, @UserID)
