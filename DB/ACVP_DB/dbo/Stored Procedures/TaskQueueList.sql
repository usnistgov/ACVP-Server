
CREATE PROCEDURE [dbo].[TaskQueueList]

AS

SET NOCOUNT ON

SELECT	 TaskId
		,TaskTypeId
		,VectorSetId
		,IsSample
		,ShowExpected
		,[Status]
		,CreatedOn
FROM dbo.TaskQueue
ORDER BY CreatedOn ASC



