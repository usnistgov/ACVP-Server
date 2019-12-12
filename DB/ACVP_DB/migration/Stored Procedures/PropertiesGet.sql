CREATE PROCEDURE [migration].[PropertiesGet]

AS
	SET NOCOUNT ON

	SELECT	 P.algorithm_id
			,P.name
			,P.id as PropertyID
	FROM ref.CRYPTO_ALGORITHM_PROPERTY_PROTOCOL_ACV_VERSION_LINK X
		INNER JOIN
		ref.CRYPTO_ALGORITHM_PROPERTY_PROTOCOL C ON C.id = X.protocol_id
												AND X.acv_version_id = 5
												AND C.capability_field_name is not null
		INNER JOIN
		ref.CRYPTO_ALGORITHM_PROPERTY P ON P.id = C.property_id


