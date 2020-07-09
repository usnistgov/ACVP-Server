CREATE PROCEDURE [dbo].[VectorSetStatusSet]
	
	 @VectorSetId BIGINT
	,@VectorSetStatusId INT

AS
BEGIN
	SET NOCOUNT ON;

    -- Set the vector set status to processing, as to not allow for a second put to the API
	UPDATE	dbo.VectorSets
	SET		VectorSetStatusId = @VectorSetStatusId
	WHERE	VectorSetId = @VectorSetId

END