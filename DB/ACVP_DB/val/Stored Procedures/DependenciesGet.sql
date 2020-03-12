CREATE PROCEDURE [val].[DependenciesGet]

	@PageSize INT,
	@PageNumber INT,
	@Id BIGINT = NULL,
	@Name NVARCHAR(1024) = NULL,
	@Type NVARCHAR(1024) = NULL,
	@Description NVARCHAR(2048) = NULL,
	@TotalRecords BIGINT OUTPUT

AS

SET NOCOUNT ON

SELECT	@TotalRecords = COUNT_BIG(1)
FROM [val].[VALIDATION_OE_DEPENDENCY] AS d
WHERE	1=1
	AND (@Id IS NULL OR d.id = @Id)
	AND (@Name IS NULL OR d.name LIKE '%' + @Name + '%')
	AND (@Type IS NULL OR d.dependency_type LIKE '%' + @Type + '%')
	AND (@Description IS NULL OR d.description LIKE '%' + @Description + '%')

SELECT d.id AS Id,
       d.name AS [Name],
	   d.dependency_type AS DependencyType,
	   d.description AS [Description]
FROM [val].[VALIDATION_OE_DEPENDENCY] AS d
WHERE	1=1
	AND (@Id IS NULL OR d.id = @Id)
	AND (@Name IS NULL OR d.name LIKE '%' + @Name + '%')
	AND (@Type IS NULL OR d.dependency_type LIKE '%' + @Type + '%')
	AND (@Description IS NULL OR d.description LIKE '%' + @Description + '%')
ORDER BY d.id
OFFSET @PageSize * (@PageNumber - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY;