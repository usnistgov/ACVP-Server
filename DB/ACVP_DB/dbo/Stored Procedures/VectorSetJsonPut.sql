
CREATE PROCEDURE [dbo].[VectorSetJsonPut]

     @VectorSetId BIGINT
    ,@VectorSetJsonFileTypeId BIGINT
    ,@Content VARCHAR(max)

AS

SET NOCOUNT ON

INSERT INTO [dbo].[VectorSetJson] (VectorSetId, VectorSetJsonFileTypeId, Content, CreatedOn)
VALUES (@VectorSetId, @VectorSetJsonFileTypeId, @Content, CURRENT_TIMESTAMP)