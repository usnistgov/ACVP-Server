
CREATE VIEW [csrc].[PERSON_EMAIL]
WITH SCHEMABINDING
AS

SELECT person_id, order_index, email_address
FROM val.PERSON_EMAIL


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_PERSON_EMAIL]
    ON [csrc].[PERSON_EMAIL]([person_id] ASC, [email_address] ASC);

