CREATE PROCEDURE [dbo].[OEsGet]

	@PageSize INT,
	@PageNumber INT,
	@OEId BIGINT = NULL,
	@Name NVARCHAR(2048) = NULL,
	@TotalRecords BIGINT OUTPUT
AS

SET NOCOUNT ON

SELECT	@TotalRecords = COUNT_BIG(1)
FROM dbo.OEs
WHERE	(@OEId IS NULL OR OEid = @OEId)
	AND	(@Name IS NULL OR [Name] LIKE '%' + @Name + '%')

SELECT	 OEId AS ID
		,[Name]
FROM dbo.OEs
WHERE	(@OEId IS NULL OR OEid = @OEId)
	AND	(@Name IS NULL OR [Name] LIKE '%' + @Name + '%')
ORDER BY OEId
OFFSET @PageSize * (@PageNumber - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY;