CREATE PROCEDURE [dbo].[ValidationsGet]
	
	@PageSize BIGINT,
	@PageNumber BIGINT,
	@ValidationId BIGINT = NULL,
	@ValidationLabel NVARCHAR(1024) = NULL,
	@ImplementationName NVARCHAR(1024) = NULL,
	@TotalRecords BIGINT OUTPUT

AS

BEGIN
	SET NOCOUNT ON;

	SELECT	@TotalRecords = COUNT_BIG(1)
	FROM	dbo.Validations V
	INNER	JOIN val.VALIDATION_SOURCE VS on V.ValidationSourceId = VS.id
	INNER	JOIN val.PRODUCT_INFORMATION P on V.ImplementationId = P.id
	WHERE	1=1
		AND (@ValidationId IS NULL OR V.ValidationId = @ValidationId)
		AND (@ValidationLabel IS NULL OR CONCAT(VS.prefix, V.ValidationNumber) LIKE '%' + @ValidationLabel + '%')
		AND (@ImplementationName IS NULL OR P.module_name LIKE '%' + @ImplementationName + '%')

	SELECT	 V.ValidationId
			,CONCAT(VS.prefix, V.ValidationNumber) AS ValidationLabel
			,P.module_name as ImplementationName
			,V.CreatedOn
	FROM	dbo.Validations V
	INNER	JOIN val.VALIDATION_SOURCE VS on V.ValidationSourceId = VS.id
	INNER	JOIN val.PRODUCT_INFORMATION P on V.ImplementationId = P.id
	WHERE	1=1
		AND (@ValidationId IS NULL OR V.ValidationId = @ValidationId)
		AND (@ValidationLabel IS NULL OR CONCAT(VS.prefix, V.ValidationNumber) LIKE '%' + @ValidationLabel + '%')
		AND (@ImplementationName IS NULL OR P.module_name LIKE '%' + @ImplementationName + '%')
	ORDER	BY CreatedOn DESC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;

END