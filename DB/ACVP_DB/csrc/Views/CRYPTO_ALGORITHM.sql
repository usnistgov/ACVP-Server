
CREATE VIEW [csrc].[CRYPTO_ALGORITHM]
WITH SCHEMABINDING
AS

SELECT id, name, primitive_id, mode, revision, alias, display_name, historical
FROM ref.CRYPTO_ALGORITHM


