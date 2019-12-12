SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

-- Set in-progress tasks back to un-started
-- I think 1 is not-processed ? maybe?
CREATE PROCEDURE [acvp].[TaskQueueRestart]
AS
    UPDATE t
    SET task_status = 1
    FROM common.TASK_QUEUE t
    WHERE task_status = 2
GO
