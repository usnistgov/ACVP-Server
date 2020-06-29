CREATE PROCEDURE [dbo].[TestSessionExists]

	@TestSessionId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
		WHEN EXISTS (SELECT NULL
					 FROM dbo.TestSessions
					 WHERE TestSessionId = @TestSessionId) THEN 1
		ELSE 0
		END AS bit) AS [Exists]
