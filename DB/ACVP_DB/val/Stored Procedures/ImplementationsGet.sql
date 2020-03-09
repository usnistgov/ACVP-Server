CREATE PROCEDURE [val].[ImplementationsGet]
	@PageSize BIGINT,
	@PageNumber BIGINT,
	@Id BIGINT = NULL,
	@Name NVARCHAR(1024) = NULL,
	@Description NVARCHAR(MAX) = NULL,
	@TotalRecords BIGINT OUTPUT
AS

	SELECT  @TotalRecords = COUNT_BIG(1)
	FROM [val].[PRODUCT_INFORMATION] AS p
		JOIN [val].[ORGANIZATION] AS o
		ON p.vendor_id = o.id
		JOIN [val].[ADDRESS] as a
		ON p.address_id = a.id
		JOIN [ref].[MODULE_TYPE] AS m
		ON m.id = p.module_type
	WHERE	1=1
		AND ((@Id IS NULL OR p.id = @Id)
		OR (@Name IS NULL OR p.module_name LIKE '%' + @Name + '%')
		OR (@Name IS NULL OR p.implementation_description LIKE '%' + @Description + '%'))

	SELECT  p.id as 'product_id',
			p.product_url as 'product_url',
			p.module_name as 'module_name',
			m.name as 'module_type',
			p.module_version as 'module_version',
			p.implementation_description as 'module_description',
			p.itar as 'product_itar',
			o.id as 'organization_id',
			o.name as 'organization_name',
			o.organization_url as 'organization_url',
			o.fax_number as 'organization_fax_number',
			o.voice_number as 'organization_voice_number',
			o.parent_organization_id as 'organization_parent_id',
			a.id as 'address_id',
			a.address_street1,
			a.address_street2,
			a.address_street3,
			a.address_locality,
			a.address_region,
			a.address_postal_code,
			a.address_country
	FROM [val].[PRODUCT_INFORMATION] AS p
		JOIN [val].[ORGANIZATION] AS o
		ON p.vendor_id = o.id
		JOIN [val].[ADDRESS] as a
		ON p.address_id = a.id
		JOIN [ref].[MODULE_TYPE] AS m
		ON m.id = p.module_type
	WHERE	1=1
		AND ((@Id IS NULL OR p.id = @Id)
		OR (@Name IS NULL OR p.module_name LIKE '%' + @Name + '%')
		OR (@Name IS NULL OR p.implementation_description LIKE '%' + @Description + '%'))
	ORDER BY p.id
	OFFSET @PageSize * (@PageNumber - 1) ROWS
	FETCH NEXT @PageSize ROWS ONLY;