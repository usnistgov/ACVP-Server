
CREATE PROCEDURE [dbo].[MessageQueueGetNext]

WITH EXECUTE AS OWNER

AS

SET NOCOUNT ON

OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;

SELECT	 TOP(1) MessageId AS ID
		,APIActionId AS APIAction
		,ACVPUserId AS UserID
		,dbo.DecryptNvarchar(Payload) AS Payload
FROM dbo.MessageQueue
WHERE MessageStatus = 0
ORDER BY CreatedOn ASC

CLOSE SYMMETRIC KEY ACVPKey


