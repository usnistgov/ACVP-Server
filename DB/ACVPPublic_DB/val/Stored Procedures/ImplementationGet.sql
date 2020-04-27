CREATE PROCEDURE [val].[ImplementationGet]
    @ID BIGINT
AS

SELECT id AS ID
    ,vendor_id AS OrganizationID
    ,address_id AS AddressID
    ,product_url AS Website
    ,module_name AS ImplementationName
    ,module_type AS ImplementationType
    ,module_version AS ImplementationVersion
    ,implementation_description AS ImplementationDescription
FROM [val].[PRODUCT_INFORMATION]
WHERE id = @ID AND itar = 0