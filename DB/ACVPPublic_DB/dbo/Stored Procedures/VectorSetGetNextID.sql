CREATE PROCEDURE [dbo].[VectorSetGetNextID]

    @TestSessionId BIGINT

AS

SET NOCOUNT ON

INSERT INTO dbo.ExternalVectorSets (TestSessionId)
VALUES (@TestSessionId)
SELECT CAST(SCOPE_IDENTITY() AS bigint) AS VectorSetId