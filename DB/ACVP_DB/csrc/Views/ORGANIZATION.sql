
CREATE VIEW [csrc].[ORGANIZATION]
WITH SCHEMABINDING
AS

SELECT id, parent_organization_id, name, organization_url, voice_number, fax_number
FROM val.ORGANIZATION


