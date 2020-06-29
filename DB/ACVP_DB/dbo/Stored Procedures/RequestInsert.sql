CREATE PROCEDURE [dbo].[RequestInsert]

	 @RequestId bigint
	,@WorkflowItemId bigint
	,@ACVPUserId bigint

AS

SET NOCOUNT ON

INSERT INTO dbo.Requests(RequestId, WorkflowItemId, ACVPUserId)
VALUES (@RequestId, @WorkflowItemId, @ACVPUserId)
