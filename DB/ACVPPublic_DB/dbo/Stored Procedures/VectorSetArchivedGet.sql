CREATE PROCEDURE [dbo].[VectorSetArchivedGet]

    @VectorSetId BIGINT

AS

SELECT TOP 1 Archived
FROM dbo.VectorSets
WHERE VectorSetId = @VectorSetId