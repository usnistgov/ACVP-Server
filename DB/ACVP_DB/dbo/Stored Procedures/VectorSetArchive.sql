CREATE PROCEDURE [dbo].[VectorSetArchive]

	@VectorSetId bigint

AS

SET NOCOUNT ON

DELETE
FROM dbo.VectorSetJson
WHERE VectorSetId = @VectorSetId

UPDATE dbo.VectorSets
SET Archived = 1
WHERE VectorSetId = @VectorSetId


