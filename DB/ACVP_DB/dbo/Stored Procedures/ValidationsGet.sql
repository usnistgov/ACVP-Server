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
	FROM dbo.Validations V
		INNER JOIN
		dbo.ValidationSources VS ON V.ValidationSourceId = VS.ValidationSourceId
		INNER JOIN
		dbo.Implementations I ON I.ImplementationId = V.ImplementationId
	WHERE	(@ValidationId IS NULL OR V.ValidationId = @ValidationId)
		AND (@ValidationLabel IS NULL OR CONCAT(VS.Prefix, V.ValidationNumber) LIKE '%' + @ValidationLabel + '%')
		AND (@ImplementationName IS NULL OR I.ImplementationName LIKE '%' + @ImplementationName + '%')

	SELECT	 V.ValidationId
			,CONCAT(VS.Prefix, V.ValidationNumber) AS ValidationLabel
			,I.ImplementationName
			,V.CreatedOn
	FROM dbo.Validations V
		INNER JOIN
		dbo.ValidationSources VS ON V.ValidationSourceId = VS.ValidationSourceId
		INNER JOIN
		dbo.Implementations I ON I.ImplementationId = V.ImplementationId
	WHERE	(@ValidationId IS NULL OR V.ValidationId = @ValidationId)
		AND (@ValidationLabel IS NULL OR CONCAT(VS.Prefix, V.ValidationNumber) LIKE '%' + @ValidationLabel + '%')
		AND (@ImplementationName IS NULL OR I.ImplementationName LIKE '%' + @ImplementationName + '%')
	ORDER BY V.CreatedOn DESC
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;

END