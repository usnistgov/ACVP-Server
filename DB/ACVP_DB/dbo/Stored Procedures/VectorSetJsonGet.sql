
CREATE PROCEDURE [dbo].[VectorSetJsonGet]

     @VectorSetId BIGINT
    ,@VectorSetJsonFileTypeId BIGINT

AS

SET NOCOUNT ON

SELECT   VectorSetId
        ,VectorSetJsonFileTypeId
        ,Content
        ,CreatedOn
FROM [dbo].[VectorSetJson]
WHERE VectorSetId = @VectorSetId
 AND VectorSetJsonFileTypeId = @VectorSetJsonFileTypeId