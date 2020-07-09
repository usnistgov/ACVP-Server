CREATE PROCEDURE [dbo].[VectorSetJsonGet]

     @VectorSetId BIGINT
    ,@VectorSetJsonFileTypeId INT

AS

SET NOCOUNT ON

SELECT Content
FROM dbo.VectorSetJson
WHERE VectorSetId = @VectorSetId
  AND VectorSetJsonFileTypeId = @VectorSetJsonFileTypeId