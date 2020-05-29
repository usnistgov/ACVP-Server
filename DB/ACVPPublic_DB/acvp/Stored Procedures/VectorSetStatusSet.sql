CREATE PROCEDURE [acvp].[VectorSetStatusSet]
	
	@VectorSetID BIGINT
	,@status INT

AS
BEGIN
	SET NOCOUNT ON;

    -- Set the vector set status to processing, as to not allow for a second put to the API
	UPDATE	acvp.VECTOR_SET
	SET		[status] = @status
	WHERE	id = @VectorSetID

END