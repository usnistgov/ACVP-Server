CREATE PROCEDURE [dbo].[VectorSetGetJsonFileTypes]
    
    @VectorSetId BIGINT

AS
BEGIN
    SET NOCOUNT ON;

    SELECT   VectorSetJsonFileTypeId 
            ,CreatedOn
    FROM dbo.VectorSetJson
    WHERE VectorSetId = @VectorSetId
END
