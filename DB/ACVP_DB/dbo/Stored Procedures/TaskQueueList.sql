
CREATE PROCEDURE [dbo].[TaskQueueList]

AS

SET NOCOUNT ON

SELECT	 TaskID
		,TaskTypeId
		,VectorSetId
		,IsSample
		,ShowExpected
		,[Status]
		,CreatedOn
FROM dbo.TaskQueue
ORDER BY CreatedOn ASC



