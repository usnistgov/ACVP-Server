CREATE VIEW [csrc].[PRODUCT_INFORMATION]
WITH SCHEMABINDING
AS

SELECT id, vendor_id, address_id, product_url, module_name, module_type, module_version, implementation_description, itar
FROM val.PRODUCT_INFORMATION
WHERE itar = 0


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_PRODUCT_INFORMATION]
    ON [csrc].[PRODUCT_INFORMATION]([id] ASC);

