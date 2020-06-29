CREATE PROCEDURE [dbo].[TestSessionGetById]

	@TestSessionId bigint

AS

SET NOCOUNT ON

SELECT	 ts.TestSessionId
		,ts.CreatedOn
		,ts.TestSessionStatusId
		,ts.IsSample
		,P.id AS PersonId
		,P.full_name AS UserName
FROM	dbo.TestSessions ts
		LEFT OUTER JOIN
		acvp.ACVP_USER U ON U.id = ts.AcvpUserId
		LEFT OUTER JOIN
		val.PERSON P ON P.id = U.person_id
WHERE	ts.TestSessionId = @TestSessionId