CREATE PROCEDURE [val].[OrganizationsGet]

	@PageSize INT,
	@PageNumber INT,
	@Id BIGINT = NULL,
	@Name NVARCHAR(1024) = NULL,
	@TotalRecords BIGINT OUTPUT

AS

SET NOCOUNT ON

SELECT	@TotalRecords = COUNT_BIG(1)
FROM [val].[ORGANIZATION] AS org
WHERE	1=1
	AND (@Id IS NULL OR org.id = @Id)
	AND (@Name IS NULL OR org.name LIKE '%' + @Name + '%')

SELECT org.id AS Id,
       org.name AS [Name],
	   org.organization_url AS [URL]
FROM [val].[ORGANIZATION] AS org
WHERE	1=1
	AND (@Id IS NULL OR org.id = @Id)
	AND (@Name IS NULL OR org.name LIKE '%' + @Name + '%')
ORDER BY org.id
OFFSET @PageSize * (@PageNumber - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY;