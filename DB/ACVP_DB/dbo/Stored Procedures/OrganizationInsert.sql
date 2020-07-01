CREATE PROCEDURE [dbo].[OrganizationInsert]

	 @Name nvarchar(1024)
	,@Website nvarchar(1024)
	,@VoiceNumber nvarchar(64)
	,@FaxNumber nvarchar(64)
	,@ParentOrganizationId bigint

AS

SET NOCOUNT ON

INSERT INTO dbo.Organizations (
	 OrganizationName
	,OrganizationUrl
	,VoiceNumber
	,FaxNumber
	,ParentOrganizationId
)
VALUES (
	 @Name
	,@Website
	,@VoiceNumber
	,@FaxNumber
	,@ParentOrganizationId
)

SELECT CAST(SCOPE_IDENTITY() AS bigint) AS OrganizationId