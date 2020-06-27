CREATE PROCEDURE [dbo].[MessageQueueUpdateStatus]

	 @MessageId uniqueidentifier
	,@StatusId int

AS

SET NOCOUNT ON

UPDATE dbo.MessageQueue
SET MessageStatus = @StatusId
WHERE MessageId = @MessageId
