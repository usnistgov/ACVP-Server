CREATE PROCEDURE [val].[OrganizationGet]

	@OrganizationID bigint

AS

SET NOCOUNT ON

SELECT	 id AS Id
		,parent_organization_id AS ParentOrganizationId
		,[name] AS [Name]
		,organization_url AS Website
		,voice_number AS VoiceNumber
		,fax_number AS FaxNumber
FROM val.ORGANIZATION
WHERE id = @OrganizationID