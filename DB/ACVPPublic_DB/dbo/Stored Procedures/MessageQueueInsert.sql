CREATE PROCEDURE [dbo].[MessageQueueInsert]

	 @APIActionId int
	,@ACVPUserId bigint
	,@Payload nvarchar(MAX)

WITH EXECUTE AS OWNER

AS

SET NOCOUNT ON

OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;

INSERT INTO dbo.MessageQueue(APIActionId, Payload, CreatedOn, ACVPUserId)
VALUES (@APIActionId, dbo.EncryptNvarchar(Key_Guid('ACVPKey'), @Payload), CURRENT_TIMESTAMP, @ACVPUserId)

CLOSE SYMMETRIC KEY ACVPKey
