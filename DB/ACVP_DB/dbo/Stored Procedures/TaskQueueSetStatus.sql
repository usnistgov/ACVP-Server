CREATE PROCEDURE [dbo].[TaskQueueSetStatus] 
	
	 @TaskId BIGINT
	,@Status INT

AS
BEGIN
	SET NOCOUNT ON;

    UPDATE	dbo.TaskQueue
	SET		[Status] = @Status
	WHERE	TaskId = @TaskId

END