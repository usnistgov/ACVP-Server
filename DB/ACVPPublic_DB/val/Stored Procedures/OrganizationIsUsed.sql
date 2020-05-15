CREATE PROCEDURE [val].[OrganizationIsUsed]

	@OrganizationId BIGINT,
	@isUsed BIT OUTPUT

AS

SET NOCOUNT ON

SET @isUsed = 
	CASE
		WHEN EXISTS(SELECT NULL
					FROM val.PRODUCT_INFORMATION
					WHERE vendor_id = @OrganizationID) THEN 1
		WHEN EXISTS(SELECT NULL
					FROM val.PERSON
					WHERE org_id = @OrganizationID) THEN 1
		WHEN EXISTS(SELECT NULL
					FROM val.ORGANIZATION
					WHERE parent_organization_id = @OrganizationId) THEN 1
		ELSE 0 END