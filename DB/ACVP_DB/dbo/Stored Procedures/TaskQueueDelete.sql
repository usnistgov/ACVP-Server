-- Removes the completed row from the TASK_QUEUE
CREATE PROCEDURE [dbo].[TaskQueueDelete]

    @TaskID BIGINT

AS
    SET NOCOUNT ON

    DELETE
    FROM [dbo].[TaskQueue]
    WHERE TaskID = @TaskID

GO
