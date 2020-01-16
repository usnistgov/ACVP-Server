CREATE PROCEDURE [val].[OrganizationIsUsed]

	@OrganizationID bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
			WHEN EXISTS(SELECT NULL
						FROM val.PRODUCT_INFORMATION
						WHERE vendor_id = @OrganizationID) THEN 1
			WHEN EXISTS(SELECT NULL
						FROM val.PERSON
						WHERE org_id = @OrganizationID) THEN 1
			WHEN EXISTS(SELECT NULL
						FROM val.ORGANIZATION
						WHERE parent_organization_id = @OrganizationID) THEN 1
			ELSE 0
		   END AS bit) AS IsUsed