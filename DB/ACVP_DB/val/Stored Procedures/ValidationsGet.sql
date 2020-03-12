CREATE PROCEDURE [val].[ValidationsGet]
	
	@PageSize BIGINT,
	@PageNumber BIGINT,
	@ValidationId BIGINT = NULL,
	@ValidationLabel NVARCHAR(1024) = NULL,
	@ProductName NVARCHAR(1024) = NULL,
	@TotalRecords BIGINT OUTPUT

AS
BEGIN
	SET NOCOUNT ON;

	SELECT	@TotalRecords = COUNT_BIG(1)
	FROM	val.VALIDATION_RECORD v
	INNER	JOIN val.VALIDATION_SOURCE vs on v.source_id = vs.id
	INNER	JOIN val.PRODUCT_INFORMATION p on v.product_information_id = p.id
	WHERE	1=1
		AND (@ValidationId IS NULL OR v.id = @ValidationId)
		AND (@ValidationLabel IS NULL OR CONCAT(vs.prefix, v.validation_id) LIKE '%' + @ValidationLabel + '%')
		AND (@ProductName IS NULL OR p.module_name LIKE '%' + @ProductName + '%')

	SELECT	v.id AS validationId
		, v.product_information_id AS productId
		, v.source_id AS sourceId
		, CONCAT(vs.prefix, v.validation_id) AS validationLabel
		, p.module_name as productName
		, v.created_on AS created
	FROM	val.VALIDATION_RECORD v
	INNER	JOIN val.VALIDATION_SOURCE vs on v.source_id = vs.id
	INNER	JOIN val.PRODUCT_INFORMATION p on v.product_information_id = p.id
	WHERE	1=1
		AND (@ValidationId IS NULL OR v.id = @ValidationId)
		AND (@ValidationLabel IS NULL OR CONCAT(vs.prefix, v.validation_id) LIKE '%' + @ValidationLabel + '%')
		AND (@ProductName IS NULL OR p.module_name LIKE '%' + @ProductName + '%')
	ORDER	BY created_on DESC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;

END