SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

-- Removes the completed row from the TASK_QUEUE
CREATE PROCEDURE [acvp].[TaskQueueDelete]
    @TaskID BIGINT
AS
    DELETE
    FROM [common].[TaskQueue]
    WHERE TaskID = @TaskID

GO
