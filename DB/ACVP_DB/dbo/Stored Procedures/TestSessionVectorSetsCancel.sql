CREATE PROCEDURE [dbo].[TestSessionVectorSetsCancel]

	@TestSessionId bigint

AS

SET NOCOUNT ON

UPDATE dbo.VectorSet
SET VectorSetStatusId = 5
WHERE TestSessionId = @TestSessionId
