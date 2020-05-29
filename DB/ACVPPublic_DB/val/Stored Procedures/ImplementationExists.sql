CREATE PROCEDURE [val].[ImplementationExists]
	
	@implementationId BIGINT,
	@exists BIT OUTPUT

AS

SET NOCOUNT ON

SET @exists = CASE WHEN EXISTS (
	SELECT	1
	FROM	val.PRODUCT_INFORMATION p
	WHERE	id = @implementationId
) THEN 1 ELSE 0 END