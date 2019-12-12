CREATE PROCEDURE [acvp].[VectorSetExpectedResultsInsertWithCapabilities]

	 @VectorSetID bigint
	,@Capabilities varbinary(MAX)

AS

SET NOCOUNT ON

INSERT INTO acvp.VECTOR_SET_EXPECTED_RESULTS(
	 vector_set_id
	,capabilities
	)
VALUES (
	 @VectorSetID
	,@Capabilities
	)