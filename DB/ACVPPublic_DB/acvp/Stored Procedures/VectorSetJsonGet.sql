CREATE PROCEDURE [acvp].[VectorSetJsonGet]
    @VsID BIGINT,
    @FileType INT
AS

--SELECT content AS Content
--FROM [acvp].[VectorSetJson]
--WHERE VsId = @VsID AND FileType = @FileType

SELECT vector_set_data as Content
FROM [acvp].[VECTOR_SET_DATA]
WHERE vector_set_id = @VsID AND data_type = @FileType