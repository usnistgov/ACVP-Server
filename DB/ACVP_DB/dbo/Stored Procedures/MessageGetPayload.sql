
CREATE PROCEDURE [dbo].[MessageGetPayload]

	@MessageId uniqueidentifier

WITH EXECUTE AS OWNER

AS

SET NOCOUNT ON

OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;

SELECT	dbo.DecryptNvarchar(Payload) AS Payload
FROM dbo.MessageQueue
WHERE MessageId = @MessageId

CLOSE SYMMETRIC KEY ACVPKey


