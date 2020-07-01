CREATE PROCEDURE [dbo].[OrganizationGet]

	@OrganizationId bigint

AS

SET NOCOUNT ON

SELECT	 OrganizationId
		,ParentOrganizationId
		,OrganizationName
		,OrganizationUrl
		,VoiceNumber
		,FaxNumber
FROM dbo.Organizations
WHERE OrganizationId = @OrganizationId