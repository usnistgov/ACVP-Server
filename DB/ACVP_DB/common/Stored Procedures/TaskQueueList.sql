
CREATE PROCEDURE [common].[TaskQueueList]

AS

SET NOCOUNT ON

SELECT	 TaskID AS ID
		,TaskType AS TaskTypeText
		,VsId AS VectorSetID
		,IsSample
		,ShowExpected
		,[Status]
		,CreatedOn
FROM common.TaskQueue
ORDER BY CreatedOn ASC



