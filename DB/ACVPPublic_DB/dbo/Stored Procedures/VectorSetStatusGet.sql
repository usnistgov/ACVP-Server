CREATE PROCEDURE [dbo].[VectorSetStatusGet]

    @VectorSetId BIGINT

AS

SELECT VectorSetStatusId
FROM dbo.VectorSets
WHERE VectorSetId = @VectorSetId