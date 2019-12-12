CREATE PROCEDURE [lcavp].[ConfigurationSettingsGet]

AS
	SET NOCOUNT ON

	SELECT	name AS SettingName
			,[value] AS Value
	FROM common.APPLICATION_PROPERTIES
	WHERE type = 'lcavp'


