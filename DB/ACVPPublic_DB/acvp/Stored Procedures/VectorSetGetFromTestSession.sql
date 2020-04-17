CREATE PROCEDURE [acvp].[VectorSetGetFromTestSession]
    @TestSessionID BIGINT
AS

SELECT id AS ID
FROM [acvp].[VectorSet]
WHERE test_session_id = @TestSessionID