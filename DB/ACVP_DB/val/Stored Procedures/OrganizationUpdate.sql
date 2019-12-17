CREATE PROCEDURE [val].[OrganizationUpdate]

	 @OrganizationID bigint
	,@Name nvarchar(1024)
	,@Website nvarchar(1024)
	,@VoiceNumber nvarchar(64)
	,@FaxNumber nvarchar(64)
	,@ParentOrganizationID bigint
	,@NameUpdated bit
	,@WebsiteUpdated bit
	,@PhoneNumbersUpdated bit
	,@ParentOrganizationIDUpdated bit

AS

SET NOCOUNT ON

UPDATE val.ORGANIZATION
SET	 [name] = CASE @NameUpdated WHEN 1 THEN @Name ELSE [name] END
	,organization_url = CASE @WebsiteUpdated WHEN 1 THEN @Website ELSE organization_url END
	,voice_number = CASE @PhoneNumbersUpdated WHEN 1 THEN @VoiceNumber ELSE voice_number END
	,fax_number = CASE @PhoneNumbersUpdated WHEN 1 THEN @FaxNumber ELSE fax_number END
	,parent_organization_id = CASE @ParentOrganizationIDUpdated WHEN 1 THEN @ParentOrganizationID ELSE parent_organization_id END
WHERE id = @OrganizationID

