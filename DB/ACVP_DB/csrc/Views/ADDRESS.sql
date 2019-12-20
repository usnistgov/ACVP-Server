
CREATE VIEW [csrc].[ADDRESS]
WITH SCHEMABINDING
AS

SELECT id, organization_id, order_index, address_street1, address_street2, address_street3, address_locality, address_region, address_country, address_postal_code
FROM val.ADDRESS

