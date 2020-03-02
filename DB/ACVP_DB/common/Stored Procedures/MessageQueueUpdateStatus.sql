CREATE PROCEDURE [common].[MessageQueueUpdateStatus]

	 @MessageId uniqueidentifier
	,@StatusId int

AS

SET NOCOUNT ON

UPDATE common.MESSAGE_QUEUE
SET message_status = @StatusId
WHERE id = @MessageId
