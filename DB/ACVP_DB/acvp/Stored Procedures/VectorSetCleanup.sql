CREATE PROCEDURE [acvp].[VectorSetCleanup]

	@VectorSetID bigint

AS

SET NOCOUNT ON

DELETE
FROM acvp.VECTOR_SET_DATA
WHERE vector_set_id = @VectorSetID


DELETE
FROM acvp.VECTOR_SET_EXPECTED_RESULTS
WHERE vector_set_id = @VectorSetID


-- TODO - uncomment this once I can add an archived flag to the table
--UPDATE acvp.VECTOR_SET
--SET archived = 1
--WHERE id = @VectorSetID


