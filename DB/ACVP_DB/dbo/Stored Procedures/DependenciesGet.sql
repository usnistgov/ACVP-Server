CREATE PROCEDURE [dbo].[DependenciesGet]

	@PageSize INT,
	@PageNumber INT,
	@DependencyId BIGINT = NULL,
	@Name NVARCHAR(1024) = NULL,
	@Type NVARCHAR(1024) = NULL,
	@Description NVARCHAR(2048) = NULL,
	@TotalRecords BIGINT OUTPUT

AS

SET NOCOUNT ON

SELECT	@TotalRecords = COUNT_BIG(1)
FROM dbo.Dependencies
WHERE	(@DependencyId IS NULL OR DependencyId = @DependencyId)
	AND (@Name IS NULL OR [Name] LIKE '%' + @Name + '%')
	AND (@Type IS NULL OR [DependencyType] LIKE '%' + @Type + '%')
	AND (@Description IS NULL OR [Description] LIKE '%' + @Description + '%')

SELECT	 DependencyId AS [ID]
		,[Name]
		,DependencyType AS [Type]
		,[Description]
FROM dbo.Dependencies
WHERE	(@DependencyId IS NULL OR DependencyId = @DependencyId)
	AND (@Name IS NULL OR [Name] LIKE '%' + @Name + '%')
	AND (@Type IS NULL OR [DependencyType] LIKE '%' + @Type + '%')
	AND (@Description IS NULL OR [Description] LIKE '%' + @Description + '%')
ORDER BY DependencyId
OFFSET @PageSize * (@PageNumber - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY;