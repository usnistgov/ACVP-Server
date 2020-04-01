CREATE PROCEDURE [common].[MessageQueueInsert]

	 @MessageType int
	,@Payload nvarchar(MAX)

WITH EXECUTE AS OWNER

AS

SET NOCOUNT ON

OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;

INSERT INTO common.MESSAGE_QUEUE (message_type, message_payload, created_on)
VALUES (@MessageType, dbo.EncryptNVarchar(Key_Guid('ACVPKey'), @Payload), CURRENT_TIMESTAMP)

CLOSE SYMMETRIC KEY ACVPKey
