CREATE PROCEDURE [migration].[PropertyStringValueMappingsGet]

AS
	SET NOCOUNT ON

	SELECT	 property_id
			,cavp_value
			,acvp_value
	FROM migration.PROPERTY_STRING_VALUE_MAPPING


