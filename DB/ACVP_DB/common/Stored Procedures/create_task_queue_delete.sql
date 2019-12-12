SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO

-- Removes the completed row from the TASK_QUEUE
CREATE PROCEDURE [acvp].[TaskQueueDelete]
    @id BIGINT
AS
    DELETE
    FROM [common].[TASK_QUEUE]
    WHERE id = @id
GO
