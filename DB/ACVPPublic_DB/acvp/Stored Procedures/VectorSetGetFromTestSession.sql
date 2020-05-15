CREATE PROCEDURE [acvp].[VectorSetGetFromTestSession]
    @TestSessionID BIGINT
AS

SELECT id AS ID
--FROM [acvp].[VectorSet]
FROM [acvp].[VECTOR_SET]
WHERE test_session_id = @TestSessionID