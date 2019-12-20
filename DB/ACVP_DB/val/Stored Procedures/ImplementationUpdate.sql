CREATE PROCEDURE [val].[ImplementationUpdate]

	 @ImplementationID bigint
	,@Name nvarchar(1024)
	,@Description nvarchar(MAX)
	,@Type int
	,@Version nvarchar(128)
	,@Website nvarchar(1024)
	,@OrganizationID bigint
	,@AddressID bigint
	,@NameUpdated bit
	,@DescriptionUpdated bit
	,@TypeUpdated bit
	,@VersionUpdated bit
	,@WebsiteUpdated bit
	,@OrganizationIDUpdated bit
	,@AddressIDUpdated bit

AS

SET NOCOUNT ON

UPDATE val.PRODUCT_INFORMATION
SET	 vendor_id = CASE @OrganizationIDUpdated WHEN 1 THEN @OrganizationID ELSE vendor_id END
	,address_id = CASE @AddressIDUpdated WHEN 1 THEN @AddressID ELSE address_id END
	,product_url = CASE @WebsiteUpdated WHEN 1 THEN @Website ELSE product_url END
	,module_name = CASE @NameUpdated WHEN 1 THEN @Name ELSE module_name END
	,module_type = CASE @TypeUpdated WHEN 1 THEN @Type ELSE module_type END
	,module_version = CASE @VersionUpdated WHEN 1 THEN @Version ELSE module_version END
	,implementation_description = CASE @DescriptionUpdated WHEN 1 THEN @Description ELSE implementation_description END
WHERE id = @ImplementationID