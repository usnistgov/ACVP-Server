﻿
CREATE VIEW [csrc].[ORGANIZATION_EMAIL]
WITH SCHEMABINDING
AS

SELECT organization_id, order_index, email_address
FROM val.ORGANIZATION_EMAIL


