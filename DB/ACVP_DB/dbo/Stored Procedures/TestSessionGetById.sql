CREATE PROCEDURE [dbo].[TestSessionGetById]

	@TestSessionId bigint

AS

SET NOCOUNT ON

SELECT	 ts.TestSessionId
		,ts.CreatedOn
		,ts.TestSessionStatusId
		,ts.IsSample
		,P.PersonId
		,P.FullName
FROM	dbo.TestSessions ts
		LEFT OUTER JOIN
		dbo.ACVPUsers U ON U.ACVPUserId = ts.ACVPUserId
		LEFT OUTER JOIN
		dbo.People P ON P.PersonId = U.PersonId
WHERE	ts.TestSessionId = @TestSessionId