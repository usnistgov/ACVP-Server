CREATE PROCEDURE [dbo].[OrganizationsGet]

	@PageSize INT,
	@PageNumber INT,
	@OrganizationId BIGINT = NULL,
	@Name NVARCHAR(1024) = NULL,
	@TotalRecords BIGINT OUTPUT

AS

SET NOCOUNT ON

SELECT	@TotalRecords = COUNT_BIG(1)
FROM dbo.Organizations
WHERE (@OrganizationId IS NULL OR OrganizationId = @OrganizationId)
  AND (@Name IS NULL OR OrganizationName LIKE '%' + @Name + '%')

SELECT	 OrganizationId AS Id
		,OrganizationName AS [Name]
		,OrganizationUrl AS [URL]
FROM dbo.Organizations
WHERE (@OrganizationId IS NULL OR OrganizationId = @OrganizationId)
  AND (@Name IS NULL OR OrganizationName LIKE '%' + @Name + '%')
ORDER BY OrganizationId
OFFSET @PageSize * (@PageNumber - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY;