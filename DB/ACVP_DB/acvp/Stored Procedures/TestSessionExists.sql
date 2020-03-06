CREATE PROCEDURE [acvp].[TestSessionExists]

	@TestSessionId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
		WHEN EXISTS (SELECT NULL
					 FROM acvp.TEST_SESSION
					 WHERE id = @TestSessionId) THEN 1
		ELSE 0
		END AS bit) AS [Exists]
