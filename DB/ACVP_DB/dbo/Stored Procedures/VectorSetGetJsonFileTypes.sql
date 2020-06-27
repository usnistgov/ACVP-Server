CREATE PROCEDURE [dbo].[VectorSetGetJsonFileTypes]
    
    @VectorSetId BIGINT

AS
BEGIN
    SET NOCOUNT ON;

    SELECT  vst.FileType AS id, 
            ft.FileType AS fileType,
            vst.CreatedOn AS createdOn
    FROM    acvp.VectorSetJson vst
    INNER   JOIN dbo.VectorSetJsonFileTypes ft ON vst.FileType = ft.VectorSetJsonFileTypeId
    WHERE   vst.VsId = @VectorSetId
END
