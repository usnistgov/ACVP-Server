CREATE PROCEDURE [common].[MessageQueueInsert]

	 @MessageType int
	,@userId bigint
	,@Payload nvarchar(MAX)

WITH EXECUTE AS OWNER

AS

SET NOCOUNT ON

declare @id uniqueidentifier
set @id = NEWID()

-- Insert a copy of the data into an unencrypted version of the table
--INSERT INTO common.MESSAGE_QUEUE_UNENCRYPTED (id, message_type, message_payload, created_on, userId)
--VALUES (@id, @MessageType, @Payload, CURRENT_TIMESTAMP, @userId)

OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;

INSERT INTO common.MESSAGE_QUEUE (id, message_type, message_payload, created_on, userId)
VALUES (@id, @MessageType, dbo.EncryptNVarchar(Key_Guid('ACVPKey'), @Payload), CURRENT_TIMESTAMP, @userId)

CLOSE SYMMETRIC KEY ACVPKey
