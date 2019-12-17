
CREATE VIEW [csrc].[PERSON_EMAIL]
WITH SCHEMABINDING
AS

SELECT person_id, order_index, email_address
FROM val.PERSON_EMAIL

