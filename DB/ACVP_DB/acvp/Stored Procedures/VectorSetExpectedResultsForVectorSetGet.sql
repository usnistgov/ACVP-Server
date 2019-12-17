CREATE PROCEDURE [acvp].[VectorSetExpectedResultsForVectorSetGet]

	@VectorSetID bigint

AS

SET NOCOUNT ON

SELECT	 capabilities
		,prompt
		,expected_results
		,submitted_results
		,validation_results
		,internal_projection
FROM acvp.VECTOR_SET_EXPECTED_RESULTS
WHERE vector_set_id = @VectorSetID
