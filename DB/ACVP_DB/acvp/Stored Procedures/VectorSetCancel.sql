CREATE PROCEDURE [acvp].[VectorSetCancel]

	@id bigint

AS

SET NOCOUNT ON

UPDATE acvp.VECTOR_SET
SET status = 5
WHERE id = @id
