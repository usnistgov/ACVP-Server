CREATE PROCEDURE [acvp].[VectorSetGetJsonFileTypes]
    
    @vectorSetId BIGINT

AS
BEGIN
    SET NOCOUNT ON;

    SELECT  vst.FileType AS id, 
            jft.FileType AS fileType,
            vst.CreatedOn AS createdOn
    FROM    acvp.VectorSetJson vst
    INNER   JOIN common.JsonFileType jft ON vst.FileType = jft.Id
    WHERE   vst.VsId = @vectorSetId
END
