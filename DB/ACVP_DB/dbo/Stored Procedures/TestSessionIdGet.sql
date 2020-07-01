CREATE PROCEDURE [dbo].[TestSessionIdGet]

	@VectorSetId bigint

AS

SET NOCOUNT ON

SELECT TOP 1 TestSessionId
FROM dbo.VectorSets
WHERE VectorSetId = @VectorSetId