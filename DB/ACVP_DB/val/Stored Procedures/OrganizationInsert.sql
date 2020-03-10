CREATE PROCEDURE [val].[OrganizationInsert]

	 @Name nvarchar(1024)
	,@Website nvarchar(1024)
	,@VoiceNumber nvarchar(64)
	,@FaxNumber nvarchar(64)
	,@ParentOrganizationID bigint

AS

SET NOCOUNT ON

INSERT INTO val.ORGANIZATION (
	 [name]
	,organization_url
	,voice_number
	,fax_number
	,parent_organization_id
)
VALUES (
	 @Name
	,@Website
	,@VoiceNumber
	,@FaxNumber
	,@ParentOrganizationID
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS OrganizationID

