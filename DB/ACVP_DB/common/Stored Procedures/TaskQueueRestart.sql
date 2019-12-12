SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

-- Set in-progress tasks back to un-started
CREATE PROCEDURE [acvp].[TaskQueueRestart]
AS
    UPDATE t
    SET Status = 0
    FROM common.TaskQueue t
    WHERE Status = 1

GO
