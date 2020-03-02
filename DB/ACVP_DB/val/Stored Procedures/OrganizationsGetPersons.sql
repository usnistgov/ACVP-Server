CREATE PROCEDURE [val].[OrganizationGetPersons]
	@OrganizationID bigint
AS
	SELECT * FROM val.PERSON AS PERSON
	WHERE PERSON.org_id = @OrganizationID