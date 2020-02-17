CREATE PROCEDURE [acvp].[ValidationsGet]
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT	v.id AS validationId
		, v.product_information_id AS productId
		, v.source_id AS sourceId
		, CONCAT(vs.prefix, v.validation_id) AS validationLabel
		, module_name as productName
		, v.created_on AS created
	FROM	val.VALIDATION_RECORD v
	INNER	JOIN val.VALIDATION_SOURCE vs on v.source_id = vs.id
	INNER	JOIN val.PRODUCT_INFORMATION p on v.product_information_id = p.id
	ORDER	BY created_on DESC

END