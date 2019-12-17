
CREATE VIEW [csrc].[PERSON_PHONE]
WITH SCHEMABINDING
AS

SELECT id, person_id, order_index, phone_number, phone_number_type
FROM val.PERSON_PHONE

