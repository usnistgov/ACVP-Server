CREATE PROCEDURE [acvp].[TestSessionCancel]

	@id bigint

AS

SET NOCOUNT ON

UPDATE acvp.TEST_SESSION
SET	 disposition = 0
	,publishable = 0
WHERE id = @id
