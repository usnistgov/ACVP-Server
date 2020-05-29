CREATE PROCEDURE [common].[TaskQueueSetStatus] 
	
	@TaskID BIGINT
	,@Status INT

AS
BEGIN
	SET NOCOUNT ON;

    UPDATE	common.TaskQueue
	SET		[status] = @Status
	WHERE	TaskID = @TaskID

END