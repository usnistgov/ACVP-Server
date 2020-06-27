CREATE PROCEDURE [dbo].[TaskQueueDeletePendingForVectorSet]

	@VectorSetId BIGINT

AS
	SET NOCOUNT ON

	DELETE
	FROM [dbo].[TaskQueue]
	WHERE VectorSetId = @VectorSetId
	  AND [Status] = 0

GO
