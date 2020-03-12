CREATE PROCEDURE [val].[PersonsGet]

	@PageSize BIGINT,
	@PageNumber BIGINT,
	@Id BIGINT = NULL,
	@Name NVARCHAR(1024) = NULL,
	@OrganizationName NVARCHAR(1024) = NULL,
	@TotalRecords BIGINT OUTPUT

AS

	SELECT	@TotalRecords = COUNT_BIG(1)
	FROM [val].[PERSON] AS p
		INNER JOIN [VAL].[ORGANIZATION] AS o ON p.org_id = o.id
	WHERE	1=1
		AND ((@Id IS NULL OR p.id = @Id)
		AND	(@Name IS NULL OR p.full_name LIKE '%' + @Name + '%')
		AND	(@OrganizationName IS NULL OR o.[name] LIKE '%' + @OrganizationName + '%'))

	SELECT	p.id AS Id,
			p.full_name AS [Name],
			o.name AS OrganizationName
	FROM [val].[PERSON] AS p
		INNER JOIN [VAL].[ORGANIZATION] AS o ON p.org_id = o.id
	WHERE	1=1
		AND ((@Id IS NULL OR p.id = @Id)
		AND	(@Name IS NULL OR p.full_name LIKE '%' + @Name + '%')
		AND	(@OrganizationName IS NULL OR o.[name] LIKE '%' + @OrganizationName + '%'))
	ORDER BY p.id
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;