CREATE PROCEDURE [val].[OEsGet]

	@PageSize INT,
	@PageNumber INT,
	@Id BIGINT = NULL,
	@Name NVARCHAR(2048) = NULL,
	@TotalRecords BIGINT OUTPUT
AS

SET NOCOUNT ON

SELECT	@TotalRecords = COUNT_BIG(1)
FROM [val].[VALIDATION_OE] AS o
WHERE	1=1
	AND ((@Id IS NULL OR o.id = @Id)
	OR	(@Name IS NULL OR o.[name] LIKE '%' + @Name + '%'))

SELECT	o.id,
		o.[name]
FROM [val].[VALIDATION_OE] AS o
WHERE	1=1
	AND ((@Id IS NULL OR o.id = @Id)
	OR	(@Name IS NULL OR o.[name] LIKE '%' + @Name + '%'))
ORDER BY o.id
OFFSET @PageSize * (@PageNumber - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY;