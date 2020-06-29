
CREATE PROCEDURE [dbo].[VectorSetJsonGetAll]
   
    @VectorSetId bigint

AS

SET NOCOUNT ON

SELECT   VectorSetJsonFileTypeId
        ,Content
        ,CreatedOn
FROM dbo.VectorSetJson
WHERE VectorSetId = @VectorSetId
ORDER BY VectorSetJsonFileTypeId