CREATE PROCEDURE [acvp].[VectorSetArchive]

	@VectorSetId bigint

AS

SET NOCOUNT ON

-- TODO - This first delete can be removed after the Public rewrite
DELETE
FROM acvp.VECTOR_SET_DATA
WHERE vector_set_id = @VectorSetId

DELETE
FROM acvp.VectorSetJson
WHERE VsId = @VectorSetId

UPDATE acvp.VECTOR_SET
SET Archived = 1
WHERE id = @VectorSetId


