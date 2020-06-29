CREATE PROCEDURE [dbo].[VectorSetUpdateStatusAndMessage]

	 @VectorSetId bigint
	,@VectorSetStatusId int
	,@ErrorMessage nvarchar(MAX)

AS

SET NOCOUNT ON

UPDATE dbo.VectorSets
SET @VectorSetStatusId = @VectorSetStatusId
	,ErrorMessage = @ErrorMessage
WHERE VectorSetId = @VectorSetId
