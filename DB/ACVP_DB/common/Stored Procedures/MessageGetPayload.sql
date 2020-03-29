
CREATE PROCEDURE [common].[MessageGetPayload]

	@MessageId uniqueidentifier

WITH EXECUTE AS OWNER

AS

SET NOCOUNT ON

OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;

SELECT	dbo.DecryptNvarchar(message_payload) AS Payload
FROM common.MESSAGE_QUEUE
WHERE id = @MessageId

CLOSE SYMMETRIC KEY ACVPKey


