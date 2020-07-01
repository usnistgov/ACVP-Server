CREATE PROCEDURE [dbo].[ImplementationUpdate]

	 @ImplementationId bigint
	,@Name nvarchar(1024)
	,@Description nvarchar(MAX)
	,@ImplementationTypeId int
	,@Version nvarchar(128)
	,@Website nvarchar(1024)
	,@OrganizationId bigint
	,@AddressId bigint
	,@NameUpdated bit
	,@DescriptionUpdated bit
	,@ImplementationTypeIdUpdated bit
	,@VersionUpdated bit
	,@WebsiteUpdated bit
	,@OrganizationIdUpdated bit
	,@AddressIdUpdated bit

AS

SET NOCOUNT ON

UPDATE dbo.Implementations
SET	 VendorId = CASE @OrganizationIdUpdated WHEN 1 THEN @OrganizationId ELSE VendorId END
	,AddressId = CASE @AddressIdUpdated WHEN 1 THEN @AddressId ELSE AddressId END
	,[Url] = CASE @WebsiteUpdated WHEN 1 THEN @Website ELSE [Url] END
	,ImplementationName = CASE @NameUpdated WHEN 1 THEN @Name ELSE ImplementationName END
	,ImplementationTypeId = CASE @ImplementationTypeIdUpdated WHEN 1 THEN @ImplementationTypeId ELSE ImplementationTypeId END
	,ImplementationVersion = CASE @VersionUpdated WHEN 1 THEN @Version ELSE ImplementationVersion END
	,ImplementationDescription = CASE @DescriptionUpdated WHEN 1 THEN @Description ELSE ImplementationDescription END
WHERE ImplementationId = @ImplementationId