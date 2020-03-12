CREATE PROCEDURE [acvp].[AcvpUsersGet]
	
	@PageSize BIGINT,
	@PageNumber BIGINT,
	@AcvpUserId BIGINT = NULL,
	@PersonId BIGINT = NULL,
	@CompanyName NVARCHAR(1024) = NULL,
	@PersonName NVARCHAR(1024) = NULL,
	@TotalRecords BIGINT OUTPUT

AS
BEGIN
	SET NOCOUNT ON;

	SELECT	@TotalRecords = COUNT_BIG(1)
	FROM	acvp.ACVP_USER au
	INNER	JOIN val.PERSON p ON au.person_id = p.id
	INNER	JOIN val.ORGANIZATION o ON p.org_id = o.id
	WHERE	1=1
		AND (@AcvpUserId IS NULL OR au.id = @AcvpUserId)
		AND (@PersonId IS NULL OR p.id = @PersonId)
		AND (@CompanyName IS NULL OR o.[name] LIKE '%' + @CompanyName + '%')
		AND (@PersonName IS NULL OR p.full_name LIKE '%' + @PersonName + '%')

	SELECT	au.id AS acvpUserId
			, p.id AS personId
			, p.full_name AS fullName
			, o.[name] AS companyName
			, o.id AS companyId
	FROM	acvp.ACVP_USER au
	INNER	JOIN val.PERSON p ON au.person_id = p.id
	INNER	JOIN val.ORGANIZATION o ON p.org_id = o.id
	WHERE	1=1
		AND (@AcvpUserId IS NULL OR au.id = @AcvpUserId)
		AND (@PersonId IS NULL OR p.id = @PersonId)
		AND (@CompanyName IS NULL OR o.[name] LIKE '%' + @CompanyName + '%')
		AND (@PersonName IS NULL OR p.full_name LIKE '%' + @PersonName + '%')
	ORDER BY au.id
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;

END