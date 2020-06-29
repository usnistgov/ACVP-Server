CREATE PROCEDURE [dbo].[VectorSetRemoveJson]
	
	 @VectorSetId BIGINT
	,@VectorSetJsonFileTypeId BIGINT

AS
BEGIN
	SET NOCOUNT ON;

    DELETE
	FROM dbo.VectorSetJson
	WHERE VectorSetId = @VectorSetId
	  AND VectorSetJsonFileTypeId = @VectorSetJsonFileTypeId

END