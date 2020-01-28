
CREATE PROCEDURE [common].[MessageQueueGetNext]

WITH EXECUTE AS OWNER

AS

SET NOCOUNT ON

OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;

SELECT	 TOP(1) id
		,message_type
		,message_status
		,created_on
		,dbo.DecryptNvarchar(message_payload) AS payload
FROM common.MESSAGE_QUEUE WITH (UPDLOCK, HOLDLOCK)
WHERE message_status = 0
ORDER BY created_on ASC

CLOSE SYMMETRIC KEY ACVPKey


