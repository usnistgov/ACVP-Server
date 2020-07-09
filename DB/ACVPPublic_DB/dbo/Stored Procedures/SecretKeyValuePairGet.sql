CREATE PROCEDURE [dbo].[SecretKeyValuePairGet] 

	 @ConfigKey VARCHAR(256)
	,@ConfigValue NVARCHAR(MAX) OUTPUT

WITH EXECUTE AS OWNER

AS
BEGIN
	
	SET NOCOUNT ON;
	    
	OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;

	SET		@ConfigValue = (SELECT dbo.DecryptNvarchar(ConfigValue)
	FROM	dbo.ExternalSecretKeyValuePairs
	WHERE	ConfigKey = @ConfigKey)

	CLOSE SYMMETRIC KEY ACVPKey

END