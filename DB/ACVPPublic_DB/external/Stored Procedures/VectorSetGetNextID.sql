CREATE PROCEDURE [external].[VectorSetGetNextID]
    @TestSessionID as BIGINT
AS

SET NOCOUNT ON

INSERT INTO [external].[VECTOR_SET] (test_session_id)
VALUES (@TestSessionID)

SELECT CAST(SCOPE_IDENTITY() AS bigint) as ID