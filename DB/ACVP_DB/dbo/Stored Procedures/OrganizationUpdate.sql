CREATE PROCEDURE [dbo].[OrganizationUpdate]

	 @OrganizationId bigint
	,@Name nvarchar(1024)
	,@Website nvarchar(1024)
	,@VoiceNumber nvarchar(64)
	,@FaxNumber nvarchar(64)
	,@ParentOrganizationId bigint
	,@NameUpdated bit
	,@WebsiteUpdated bit
	,@VoiceNumberUpdated bit
	,@FaxNumberUpdated bit
	,@ParentOrganizationIdUpdated bit

AS

SET NOCOUNT ON

UPDATE dbo.Organizations
SET	 OrganizationName = CASE @NameUpdated WHEN 1 THEN @Name ELSE OrganizationName END
	,OrganizationUrl = CASE @WebsiteUpdated WHEN 1 THEN @Website ELSE OrganizationUrl END
	,VoiceNumber = CASE @VoiceNumberUpdated WHEN 1 THEN @VoiceNumber ELSE VoiceNumber END
	,FaxNumber = CASE @FaxNumberUpdated WHEN 1 THEN @FaxNumber ELSE FaxNumber END
	,ParentOrganizationId = CASE @ParentOrganizationIdUpdated WHEN 1 THEN @ParentOrganizationId ELSE ParentOrganizationId END
WHERE OrganizationId = @OrganizationId

