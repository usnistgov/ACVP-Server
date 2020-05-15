CREATE PROCEDURE [acvp].[VectorSetArchive]

	@VectorSetId bigint

AS

SET NOCOUNT ON

DELETE
FROM acvp.VectorSetJson
WHERE VsId = @VectorSetId

UPDATE acvp.VECTOR_SET
SET Archived = 1
WHERE id = @VectorSetId


