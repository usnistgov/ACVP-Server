
-- Removes the completed row from the TASK_QUEUE
CREATE PROCEDURE [acvp].[TaskQueueDelete]
    @TaskID BIGINT
AS
    DELETE
    FROM [common].[TaskQueue]
    WHERE TaskID = @TaskID