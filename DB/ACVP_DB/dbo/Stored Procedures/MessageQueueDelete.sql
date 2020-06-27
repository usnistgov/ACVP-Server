CREATE PROCEDURE [dbo].[MessageQueueDelete]

	@MessageId uniqueidentifier

AS

SET NOCOUNT ON

DELETE FROM dbo.MessageQueue
WHERE MessageId = @MessageId
