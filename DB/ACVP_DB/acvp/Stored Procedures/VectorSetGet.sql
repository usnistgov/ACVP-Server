CREATE PROCEDURE [acvp].[VectorSetGet]
	
	@vectorSetId bigint

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT	vs.id as vectorSetId, test_session_id as testSessionId, generator_version as generatorVersion, algorithm_id as algorithmId, ca.display_name as algorithmName, status
	FROM	acvp.VECTOR_SET vs
	INNER	JOIN ref.CRYPTO_ALGORITHM ca on vs.algorithm_id = ca.id
	WHERE	vs.id = @vectorSetId

END