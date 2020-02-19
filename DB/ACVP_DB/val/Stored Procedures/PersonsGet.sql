CREATE PROCEDURE [val].[PersonsGet]
	@PageSize bigint,
	@PageNumber bigint
AS
	SELECT PERSON.id,
			PERSON.full_name,
			ORGANIZATION.name as org_name
	FROM [val].[PERSON] AS PERSON
		JOIN [VAL].[ORGANIZATION] AS ORGANIZATION
		ON PERSON.org_id = ORGANIZATION.id
	ORDER BY PERSON.id
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;