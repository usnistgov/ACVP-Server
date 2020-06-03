
CREATE PROCEDURE [val].[ValidationGetById]
	
	@validationId bigint

AS
BEGIN
	SET NOCOUNT ON;

    SELECT	V.ValidationId
		, V.ImplementationId
		, CONCAT(vs.prefix, V.ValidationNumber) AS ValidationLabel
		, P.module_name AS ImplementationName
		, V.CreatedOn
		, V.LastUpdated
		, p.vendor_id AS VendorId
	FROM	dbo.Validations V
	INNER	JOIN val.VALIDATION_SOURCE vs on V.ValidationSourceId = vs.id
	INNER	JOIN val.PRODUCT_INFORMATION p on V.ImplementationId = p.id
	WHERE	V.ValidationId = @validationId
END