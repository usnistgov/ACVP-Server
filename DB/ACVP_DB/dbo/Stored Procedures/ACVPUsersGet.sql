CREATE PROCEDURE [dbo].[ACVPUsersGet]
	
	@PageSize BIGINT,
	@PageNumber BIGINT,
	@ACVPUserId BIGINT = NULL,
	@PersonId BIGINT = NULL,
	@OrganizationName NVARCHAR(1024) = NULL,
	@PersonName NVARCHAR(1024) = NULL,
	@TotalRecords BIGINT OUTPUT

AS
BEGIN
	SET NOCOUNT ON;

	SELECT	@TotalRecords = COUNT_BIG(1)
	FROM	dbo.ACVPUsers au
	INNER	JOIN dbo.People p ON au.PersonId = p.PersonId
	INNER	JOIN dbo.Organizations o ON p.OrganizationId = o.OrganizationId
	WHERE	1=1
		AND (@ACVPUserId IS NULL OR au.ACVPUserId = @ACVPUserId)
		AND (@PersonId IS NULL OR p.PersonId = @PersonId)
		AND (@OrganizationName IS NULL OR o.OrganizationName LIKE '%' + @OrganizationName + '%')
		AND (@PersonName IS NULL OR p.FullName LIKE '%' + @PersonName + '%')

	SELECT	 ACVPUserId
			,p.PersonId
			,p.FullName
			,o.OrganizationName
			,o.OrganizationId
			,au.ExpiresOn
	FROM	dbo.ACVPUsers au
	INNER	JOIN dbo.People p ON au.PersonId = p.PersonId
	INNER	JOIN dbo.Organizations o ON p.OrganizationId = o.OrganizationId
	WHERE	1=1
AND (@ACVPUserId IS NULL OR au.ACVPUserId = @ACVPUserId)
		AND (@PersonId IS NULL OR p.PersonId = @PersonId)
		AND (@OrganizationName IS NULL OR o.OrganizationName LIKE '%' + @OrganizationName + '%')
		AND (@PersonName IS NULL OR p.FullName LIKE '%' + @PersonName + '%')
	ORDER BY au.ExpiresOn
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;

END