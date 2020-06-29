CREATE PROCEDURE [dbo].[VectorSetCancel]

	@VectorSetId bigint

AS

SET NOCOUNT ON

UPDATE dbo.VectorSets
SET VectorSetStatusId = 5
WHERE VectorSetId = @VectorSetId
