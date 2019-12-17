CREATE PROCEDURE [common].[MessageQueueMarkForJava]

	@id uniqueidentifier

AS

SET NOCOUNT ON

UPDATE common.MESSAGE_QUEUE
SET UseJava = 1
WHERE id = @id
