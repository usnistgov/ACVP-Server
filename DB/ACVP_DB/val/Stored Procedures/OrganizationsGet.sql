CREATE PROCEDURE [val].[OrganizationGet]
	@OrganizationID bigint
AS
	SELECT * FROM val.ORGANIZATION AS ORG
	WHERE ORG.id = @OrganizationID