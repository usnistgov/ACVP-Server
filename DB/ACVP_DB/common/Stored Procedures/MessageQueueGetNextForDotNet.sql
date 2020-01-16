
CREATE PROCEDURE [common].[MessageQueueGetNextForDotNet]

WITH EXECUTE AS OWNER

AS

SET NOCOUNT ON

OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;

SELECT	 TOP(1) id AS ID
		,message_type AS MessageType
		,dbo.DecryptNvarchar(message_payload) AS Payload
FROM common.MESSAGE_QUEUE WITH (UPDLOCK, HOLDLOCK)
WHERE message_status = 0
  AND UseJava IS NULL
ORDER BY created_on ASC

CLOSE SYMMETRIC KEY ACVPKey


