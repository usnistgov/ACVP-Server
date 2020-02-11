CREATE PROCEDURE [acvp].[VectorSetExpectedResultsCapabilitiesGet]

	@VectorSetId bigint

AS

SELECT capabilities AS Capabilities
FROM acvp.VECTOR_SET_EXPECTED_RESULTS
WHERE vector_set_id = @VectorSetId