CREATE PROCEDURE [acvp].[TestSessionGetById]

	@testSessionId bigint

AS

SET NOCOUNT ON

SELECT	ts.id, ts.created_on, ts.passed_date, ts.publishable, ts.published, ts.[sample]
FROM	acvp.TEST_SESSION ts
WHERE	id = @testSessionId