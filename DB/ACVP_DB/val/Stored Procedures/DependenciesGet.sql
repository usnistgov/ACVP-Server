CREATE PROCEDURE [val].[DependenciesGet]

	@PageSize bigint,
	@PageNumber bigint

AS

SET NOCOUNT ON

SELECT DEPENDENCY.id,
       DEPENDENCY.name,
	   DEPENDENCY.dependency_type,
	   DEPENDENCY.description
FROM [val].[VALIDATION_OE_DEPENDENCY] AS DEPENDENCY
ORDER BY DEPENDENCY.id
OFFSET @PageSize * (@PageNumber - 1) ROWS
FETCH NEXT @PageSize ROWS ONLY;