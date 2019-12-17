CREATE PROCEDURE [acvp].[VectorSetUpdateSubmittedResults]

	 @VectorSetID bigint
	,@Results varbinary(MAX)
AS

SET NOCOUNT ON

UPDATE acvp.VECTOR_SET_EXPECTED_RESULTS
SET submitted_results = @Results
WHERE vector_set_id = @VectorSetID
