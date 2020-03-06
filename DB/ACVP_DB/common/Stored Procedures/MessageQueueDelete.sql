CREATE PROCEDURE [common].[MessageQueueDelete]

	@MessageId uniqueidentifier

AS

SET NOCOUNT ON

DELETE FROM common.MESSAGE_QUEUE
WHERE id = @MessageId
