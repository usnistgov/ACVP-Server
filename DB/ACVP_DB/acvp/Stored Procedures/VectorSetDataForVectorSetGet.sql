CREATE PROCEDURE [acvp].[VectorSetDataForVectorSetGet]

	@VectorSetID bigint

AS

SET NOCOUNT ON

SELECT	 created_on
		,data_type
		,vector_set_data
		,iv_value
FROM acvp.VECTOR_SET_DATA
WHERE vector_set_id = @VectorSetID
