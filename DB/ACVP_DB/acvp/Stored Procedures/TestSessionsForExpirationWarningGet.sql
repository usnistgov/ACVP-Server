CREATE PROCEDURE [acvp].[TestSessionsForExpirationWarningGet]
	
	@AgeInDaysForWarning int

AS

SET NOCOUNT ON

SELECT	 TS.id AS TestSessionID
		,TS.[user_id] AS UserID
		,U.person_id AS PersonID
FROM acvp.TEST_SESSION TS
	INNER JOIN
	acvp.ACVP_USER U ON U.id = TS.[user_id]
WHERE TS.TestSessionStatusId NOT IN (1,5,6,7)	-- Not cancelled, submitted for approval, published, or already expired
  AND DATEDIFF(DAY, TS.LastTouched, CURRENT_TIMESTAMP) = @AgeInDaysForWarning