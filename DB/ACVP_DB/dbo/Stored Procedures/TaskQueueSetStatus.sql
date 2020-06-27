CREATE PROCEDURE [dbo].[TaskQueueSetStatus] 
	
	@TaskID BIGINT
	,@Status INT

AS
BEGIN
	SET NOCOUNT ON;

    UPDATE	dbo.TaskQueue
	SET		[status] = @Status
	WHERE	TaskID = @TaskID

END