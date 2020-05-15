CREATE PROCEDURE [acvp].[TestSessionSetPublished]
	
	@testSessionid BIGINT

AS
BEGIN
	SET NOCOUNT ON;

    UPDATE	acvp.TEST_SESSION
	--UPDATE	acvp.TestSession
	SET		TestSessionStatusId = 6
	WHERE	id = @testSessionid

END