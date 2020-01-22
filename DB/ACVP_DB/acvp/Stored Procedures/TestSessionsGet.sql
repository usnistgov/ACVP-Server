CREATE PROCEDURE [acvp].[TestSessionsGet]
AS

SET NOCOUNT ON

SELECT	id, created_on, TestSessionStatusId
FROM	acvp.TEST_SESSION
ORDER	BY created_on DESC