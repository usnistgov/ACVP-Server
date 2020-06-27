
CREATE PROCEDURE [dbo].[MessageQueueList]

AS

SET NOCOUNT ON

SELECT	 MessageId AS ID
		,MessageStatus AS [Status]
		,APIActionId AS APIAction
		,DATALENGTH(Payload) AS [Length]
		,CreatedOn
FROM dbo.MessageQueue
ORDER BY CreatedOn ASC



