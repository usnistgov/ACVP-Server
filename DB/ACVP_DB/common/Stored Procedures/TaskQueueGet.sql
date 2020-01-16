SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

-- Grabs oldest unprocessed of common.TASK_QUEUE
-- I think 1 is not-processed ? maybe?
CREATE PROCEDURE [acvp].[TaskQueueGet]
AS
    UPDATE t
    SET Status = 1
    OUTPUT inserted.TaskID, inserted.TaskType, inserted.VsID, inserted.IsSample, inserted.ShowExpected
    FROM common.TaskQueue t
    INNER JOIN (
        SELECT TOP 1 TaskID
        FROM common.TaskQueue
        WHERE Status = 0
        ORDER BY CreatedOn
    ) subQuery ON t.TaskID = subQuery.TaskID

GO
