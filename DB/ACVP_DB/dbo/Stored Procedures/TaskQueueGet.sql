CREATE PROCEDURE [dbo].[TaskQueueGet]

AS

SET NOCOUNT ON

UPDATE t
SET [Status] = 1
OUTPUT inserted.TaskId, inserted.TaskTypeId, inserted.VectorSetId, inserted.IsSample, inserted.ShowExpected, inserted.[Status], inserted.CreatedOn
FROM dbo.TaskQueue t
INNER JOIN (
    SELECT TOP 1 TaskId
    FROM dbo.TaskQueue
    WHERE [Status] = 0
    ORDER BY CreatedOn
) subQuery ON t.TaskId = subQuery.TaskId

GO
