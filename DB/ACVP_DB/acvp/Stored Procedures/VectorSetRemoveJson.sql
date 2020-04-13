CREATE PROCEDURE [acvp].[VectorSetRemoveJson]
	
	@vectorSetId BIGINT
	, @fileTypeId BIGINT

AS
BEGIN
	SET NOCOUNT ON;

    DELETE	v
	FROM	acvp.VectorSetJson v
	WHERE	v.VsId = @vectorSetId
		AND v.FileType = @fileTypeId

END