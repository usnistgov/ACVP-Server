
CREATE PROCEDURE [acvp].[VectorSetJsonGetAll]
   
    @VectorSetId bigint

AS

SET NOCOUNT ON

SELECT   FileType
        ,Content
        ,CreatedOn
FROM acvp.VectorSetJson
WHERE VsId = @VectorSetId
ORDER BY FileType