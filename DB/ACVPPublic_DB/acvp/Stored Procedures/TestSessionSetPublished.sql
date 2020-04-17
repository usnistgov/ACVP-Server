CREATE PROCEDURE [acvp].[TestSessionSetPublished]
	
	@testSessionid BIGINT

AS
BEGIN
	SET NOCOUNT ON;

    UPDATE	acvp.TestSession
	SET		published = 1
	WHERE	id = @testSessionid

END