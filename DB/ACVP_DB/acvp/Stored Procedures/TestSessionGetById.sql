CREATE PROCEDURE [acvp].[TestSessionGetById]

	@testSessionId bigint

AS

SET NOCOUNT ON

SELECT	 ts.id
		,ts.created_on
		,ts.passed_date
		,ts.publishable
		,ts.published
		,ts.[sample]
		,P.id AS UserId
		,P.full_name AS UserName
FROM	acvp.TEST_SESSION ts
		LEFT OUTER JOIN
		acvp.ACVP_USER U ON U.id = ts.[user_id]
		LEFT OUTER JOIN
		val.PERSON P ON P.id = U.person_id
WHERE	ts.id = @testSessionId