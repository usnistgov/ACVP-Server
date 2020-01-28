CREATE PROCEDURE [common].[MessageQueueDelete]

	@id uniqueidentifier

AS

SET NOCOUNT ON

DELETE FROM common.MESSAGE_QUEUE
WHERE id = @id
