CREATE PROCEDURE [dbo].[VectorSetGetFromTestSession]

    @TestSessionId BIGINT

AS

SET NOCOUNT ON

SELECT VectorSetId
FROM dbo.VectorSets
WHERE TestSessionId = @TestSessionId