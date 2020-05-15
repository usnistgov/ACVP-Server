CREATE PROCEDURE [external].[VectorSetGetNextID]
    @TestSessionID as BIGINT
AS

INSERT INTO [external].[VECTOR_SET] (test_session_id)
VALUES (@TestSessionID)
SELECT SCOPE_IDENTITY() as ID