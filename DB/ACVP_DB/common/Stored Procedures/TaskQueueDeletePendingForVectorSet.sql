CREATE PROCEDURE [common].[TaskQueueDeletePendingForVectorSet]

	@VectorSetId BIGINT

AS
	SET NOCOUNT ON

	DELETE
	FROM [common].[TaskQueue]
	WHERE VsId = @VectorSetId
	  AND [Status] = 0

GO
