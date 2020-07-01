CREATE PROCEDURE [dbo].[TestSessionsForExpirationWarningGet]
	
	@AgeInDaysForWarning int

AS

SET NOCOUNT ON

SELECT	 TS.TestSessionId
		,U.PersonId
FROM dbo.TestSessions TS
	INNER JOIN
	dbo.ACVPUsers U ON U.ACVPUserId = TS.AcvpUserId
WHERE TS.TestSessionStatusId NOT IN (1,5,6,7)	-- Not cancelled, submitted for approval, published, or already expired
  AND DATEDIFF(DAY, TS.LastTouched, CURRENT_TIMESTAMP) = @AgeInDaysForWarning