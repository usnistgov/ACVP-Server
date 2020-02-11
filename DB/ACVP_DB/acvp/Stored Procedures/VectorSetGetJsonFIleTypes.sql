CREATE PROCEDURE [acvp].[VectorSetGetJsonFIleTypes]
	
	@vectorSetId BIGINT

AS
BEGIN
	SET NOCOUNT ON;

    SELECT	vst.FileType AS id, jft.FileType as fileType
	FROM	acvp.VectorSetJson vst
	INNER	JOIN common.JsonFileType jft ON vst.FileType = jft.Id
	WHERE	vst.VsId = @vectorSetId
END