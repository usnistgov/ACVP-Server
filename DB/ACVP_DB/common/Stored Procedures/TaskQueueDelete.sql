-- Removes the completed row from the TASK_QUEUE
CREATE PROCEDURE [common].[TaskQueueDelete]

    @TaskID BIGINT

AS
    SET NOCOUNT ON

    DELETE
    FROM [common].[TaskQueue]
    WHERE TaskID = @TaskID

GO
