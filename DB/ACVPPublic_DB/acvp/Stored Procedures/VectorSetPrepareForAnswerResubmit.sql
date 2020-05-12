CREATE PROCEDURE [acvp].[VectorSetPrepareForAnswerResubmit]
	
	@VectorSetID BIGINT

AS
BEGIN
	SET NOCOUNT ON;

    -- Set the vector set status to processing, as to not allow for a second put to the API
	UPDATE	acvp.VECTOR_SET
	SET		[status] = 2 -- "KATReceived"
	WHERE	id = @VectorSetID

	-- Remove the validation json file, so the GET request on the validation endpoint will return a retry, rather than the old validation file.
	DELETE	v
	FROM	acvp.VectorSetJson v
	WHERE	v.FileType = 6 -- "validation" file
		and v.vsId = @VectorSetId

END