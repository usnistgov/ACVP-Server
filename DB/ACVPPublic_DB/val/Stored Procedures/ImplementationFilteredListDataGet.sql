
CREATE PROCEDURE [val].[ImplementationFilteredListDataGet]

	@IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the implementation level data
SELECT	 P.id AS Id
		,P.module_name AS [Name]
		,P.module_version AS [Version]
		,P.module_type AS [Type]
		,P.product_url AS Website
		,P.implementation_description AS [Description]
		,P.vendor_id AS OrganizationId
		,P.address_id AS AddressId
FROM val.PRODUCT_INFORMATION P
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = P.id

-- Get the contacts for all of these
SELECT	 VC.product_information_id AS ImplementationId
		,VC.person_id AS PersonId
		,VC.order_index AS OrderIndex
FROM val.VALIDATION_CONTACT VC
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = VC.product_information_id