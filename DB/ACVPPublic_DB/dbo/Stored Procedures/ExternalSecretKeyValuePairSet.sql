CREATE PROCEDURE [dbo].[ExternalSecretKeyValuePairSet] 

	 @ConfigKey VARCHAR(256)
	,@ConfigValue NVARCHAR(MAX)

AS
BEGIN
	
	SET NOCOUNT ON;
	    
	OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;
	
	IF EXISTS (
		SELECT	1
		FROM	dbo.ExternalSecretKeyValuePairs
		WHERE	ConfigKey = @ConfigKey
	)
		BEGIN
			UPDATE	dbo.ExternalSecretKeyValuePairs
			SET		ConfigValue = dbo.EncryptNVarchar(Key_Guid('ACVPKey'), @ConfigValue),
					UpdatedOn = CURRENT_TIMESTAMP
			WHERE	ConfigKey = @ConfigKey
		END
	ELSE
		BEGIN
		INSERT INTO dbo.ExternalSecretKeyValuePairs (ConfigKey, ConfigValue, UpdatedOn)
		VALUES (@ConfigKey, dbo.EncryptNVarchar(Key_Guid('ACVPKey'), @ConfigValue), CURRENT_TIMESTAMP)
	END

	CLOSE SYMMETRIC KEY ACVPKey

END