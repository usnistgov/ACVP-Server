CREATE PROCEDURE [dbo].[PersonsGet]

	@PageSize BIGINT,
	@PageNumber BIGINT,
	@PersonId BIGINT = NULL,
	@Name NVARCHAR(1024) = NULL,
	@OrganizationName NVARCHAR(1024) = NULL,
	@TotalRecords BIGINT OUTPUT

AS

	SELECT	@TotalRecords = COUNT_BIG(1)
	FROM dbo.People P
		INNER JOIN
		dbo.Organizations O ON O.OrganizationId = P.OrganizationId
	WHERE	(@PersonId IS NULL OR P.PersonId = @PersonId)
		AND	(@Name IS NULL OR P.FullName LIKE '%' + @Name + '%')
		AND	(@OrganizationName IS NULL OR O.OrganizationName LIKE '%' + @OrganizationName + '%')

	SELECT	 P.PersonId AS ID
			,P.FullName AS [Name]
			,O.OrganizationName
	FROM dbo.People P
		INNER JOIN
		dbo.Organizations O ON O.OrganizationId = P.OrganizationId
	WHERE	(@PersonId IS NULL OR P.PersonId = @PersonId)
		AND	(@Name IS NULL OR P.FullName LIKE '%' + @Name + '%')
		AND	(@OrganizationName IS NULL OR O.OrganizationName LIKE '%' + @OrganizationName + '%')
	ORDER BY P.PersonId
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;