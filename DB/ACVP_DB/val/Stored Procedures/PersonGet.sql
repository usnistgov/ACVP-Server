CREATE PROCEDURE [val].[PersonGet]
	@PersonID bigint
AS
	SELECT  PERSON.id as 'person_id',
			PERSON.full_name as 'person_name',
			ORGANIZATION.id as 'organization_id',
			ORGANIZATION.name as 'organization_name',
			ORGANIZATION.organization_url as 'organization_url',
			ORGANIZATION.voice_number as 'organization_voice',
			ORGANIZATION.fax_number as 'organization_fax'
	FROM [val].[PERSON] AS PERSON
		JOIN [val].[ORGANIZATION] AS ORGANIZATION
		ON PERSON.org_id = ORGANIZATION.id
	WHERE PERSON.id = @PersonID