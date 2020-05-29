
CREATE PROCEDURE [common].[MessageQueueList]

AS

SET NOCOUNT ON

SELECT	 id AS ID
		,message_status AS [Status]
		,message_type AS MessageType
		,DATALENGTH(message_payload) AS [Length]
		,created_on AS CreatedOn
FROM common.MESSAGE_QUEUE
ORDER BY created_on ASC



