-- Set in-progress tasks back to un-started
CREATE PROCEDURE [common].[TaskQueueRestart]

AS

SET NOCOUNT ON

    UPDATE t
    SET Status = 0
    FROM common.TaskQueue t
    WHERE Status = 1
GO
