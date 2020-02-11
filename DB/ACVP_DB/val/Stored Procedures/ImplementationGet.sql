CREATE PROCEDURE [val].[ImplementationGet]
	@implementationID int = 0
AS
	SELECT  PRODUCT.id as 'product_id',
			PRODUCT.product_url as 'product_url',
			PRODUCT.module_name as 'module_name',
			MODULE_TYPE.name as 'module_type',
			PRODUCT.module_version as 'module_version',
			PRODUCT.implementation_description as 'module_description',
			PRODUCT.itar as 'product_itar',
			ORGANIZATION.id as 'organization_id',
			ORGANIZATION.name as 'organization_name',
			ORGANIZATION.organization_url as 'organization_url',
			ORGANIZATION.fax_number as 'organization_fax_number',
			ORGANIZATION.voice_number as 'organization_voice_number',
			ORGANIZATION.parent_organization_id as 'organization_parent_id',
			ADDRESS.id as 'address_id',
			ADDRESS.address_street1,
			ADDRESS.address_street2,
			ADDRESS.address_street3,
			ADDRESS.address_locality,
			ADDRESS.address_region,
			ADDRESS.address_postal_code,
			ADDRESS.address_country
	FROM [val].[PRODUCT_INFORMATION] AS PRODUCT
		JOIN [val].[ORGANIZATION] AS ORGANIZATION
		ON PRODUCT.vendor_id = ORGANIZATION.id
		JOIN [val].[ADDRESS] as ADDRESS
		ON PRODUCT.address_id = ADDRESS.id
		JOIN [ref].[MODULE_TYPE] AS MODULE_TYPE
		ON MODULE_TYPE.id = PRODUCT.module_type
	WHERE PRODUCT.id = @implementationID