CREATE PROCEDURE [external].[SecretKeyValuePairSet] 
	@ConfigKey VARCHAR(256)
	,@ConfigValue NVARCHAR(MAX)
AS
BEGIN
	
	SET NOCOUNT ON;
	    
	OPEN SYMMETRIC KEY ACVPKey DECRYPTION BY CERTIFICATE ACVPKeyProtectionCert;
	
	IF EXISTS (
		SELECT	1
		FROM	[external].[SecretKeyValuePairs] s
		WHERE	s.ConfigKey = @ConfigKey
	)
		BEGIN
			UPDATE	[external].[SecretKeyValuePairs]
			SET		ConfigValue = dbo.EncryptNVarchar(Key_Guid('ACVPKey'), @ConfigValue),
					UpdatedOn = CURRENT_TIMESTAMP
			WHERE	ConfigKey = @ConfigKey
		END
	ELSE
		BEGIN
		INSERT INTO [external].[SecretKeyValuePairs] (ConfigKey, ConfigValue, UpdatedOn)
		VALUES (@configKey, dbo.EncryptNVarchar(Key_Guid('ACVPKey'), @ConfigValue), CURRENT_TIMESTAMP)
	END

	CLOSE SYMMETRIC KEY ACVPKey

END