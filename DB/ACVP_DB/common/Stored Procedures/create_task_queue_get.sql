SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

-- Grabs oldest unprocessed of common.TASK_QUEUE
-- I think 1 is not-processed ? maybe?
CREATE PROCEDURE [acvp].[TaskQueueGet]
AS
    UPDATE t
    SET task_status = 2
    OUTPUT inserted.id, inserted.task_type, inserted.task_payload
    FROM common.TASK_QUEUE t
    INNER JOIN (
        SELECT TOP 1 id
        FROM common.TASK_QUEUE
        WHERE task_status = 1
        ORDER BY created_on
    ) subQuery ON t.id = subQuery.id
GO

