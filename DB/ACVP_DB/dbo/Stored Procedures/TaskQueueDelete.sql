-- Removes the completed row from the TASK_QUEUE
CREATE PROCEDURE [dbo].[TaskQueueDelete]

    @TaskId BIGINT

AS
    SET NOCOUNT ON

    DELETE
    FROM [dbo].[TaskQueue]
    WHERE TaskId = @TaskId

GO
