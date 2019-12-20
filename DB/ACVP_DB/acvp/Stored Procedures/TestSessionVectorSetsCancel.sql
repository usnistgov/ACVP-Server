CREATE PROCEDURE [acvp].[TestSessionVectorSetsCancel]

	@id bigint

AS

SET NOCOUNT ON

UPDATE acvp.VECTOR_SET
SET status = 5
WHERE test_session_id = @id
