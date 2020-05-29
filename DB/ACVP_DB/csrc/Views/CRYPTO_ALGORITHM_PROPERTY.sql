

CREATE VIEW [csrc].[CRYPTO_ALGORITHM_PROPERTY]
WITH SCHEMABINDING
AS

SELECT id, algorithm_id, name, capability_type, vector_type, group_type, value_type, default_value, in_certificate, certificate_name, order_index, historical, required, units_label
FROM ref.CRYPTO_ALGORITHM_PROPERTY


GO
CREATE UNIQUE CLUSTERED INDEX [PK_CSRC_CRYPTO_ALGORITHM_PROPERTY]
    ON [csrc].[CRYPTO_ALGORITHM_PROPERTY]([id] ASC);

