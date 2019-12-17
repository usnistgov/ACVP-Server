CREATE PROCEDURE [acvp].[VectorSetUpdateStatusAndMessage]

	 @VectorSetID bigint
	,@Status int
	,@ErrorMessage nvarchar(MAX)

AS

SET NOCOUNT ON

UPDATE acvp.VECTOR_SET
SET [status] = @Status
	,vector_error_message = @ErrorMessage
WHERE id = @VectorSetID
