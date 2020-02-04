CREATE PROCEDURE [acvp].[VectorSetGetByTestSessionId]

	@testSessionId bigint

AS

SET NOCOUNT ON

SELECT	vs.id, generator_version, algorithm_id, ca.display_name, [status]
FROM	acvp.VECTOR_SET vs
INNER	JOIN ref.CRYPTO_ALGORITHM ca on vs.algorithm_id = ca.id
WHERE	test_session_id = @testSessionId