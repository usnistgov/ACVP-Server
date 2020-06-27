-- Set in-progress tasks back to un-started
CREATE PROCEDURE [dbo].[TaskQueueRestart]

AS

SET NOCOUNT ON

    UPDATE dbo.TaskQueue
    SET [Status] = 0
    WHERE [Status] = 1
GO
