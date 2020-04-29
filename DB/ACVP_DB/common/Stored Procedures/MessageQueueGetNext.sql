
CREATE PROCEDURE [common].[MessageQueueGetNext]

WITH EXECUTE AS OWNER

AS

SET NOCOUNT ON

OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;

SELECT	 TOP(1) id AS ID
		,message_type AS MessageType
		,userId AS UserID
		,dbo.DecryptNvarchar(message_payload) AS Payload
FROM common.MESSAGE_QUEUE
WHERE message_status = 0
ORDER BY created_on ASC

CLOSE SYMMETRIC KEY ACVPKey


