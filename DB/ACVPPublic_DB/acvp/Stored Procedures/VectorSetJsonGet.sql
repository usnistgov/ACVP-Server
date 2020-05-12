CREATE PROCEDURE [acvp].[VectorSetJsonGet]
    @VsID BIGINT,
    @FileType INT
AS

SET NOCOUNT ON

SELECT Content
FROM [acvp].[VectorSetJson]
WHERE VsId = @VsID AND FileType = @FileType