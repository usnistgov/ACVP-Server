CREATE PROCEDURE [val].[ImplementationInsert]

	 @Name nvarchar(1024)
	,@Description nvarchar(MAX)
	,@Type int
	,@Version nvarchar(128)
	,@Website nvarchar(1024)
	,@OrganizationID bigint
	,@AddressID bigint
	,@IsITAR bit

AS

SET NOCOUNT ON

INSERT INTO val.PRODUCT_INFORMATION(
	 vendor_id
	,address_id
	,product_url
	,module_name
	,module_type
	,module_version
	,implementation_description
	,itar
)
VALUES (
	 @OrganizationID
	,@AddressID
	,@Website
	,@Name
	,@Type
	,@Version
	,@Description
	,@IsITAR
)

SELECT SCOPE_IDENTITY() AS ImplementationID

