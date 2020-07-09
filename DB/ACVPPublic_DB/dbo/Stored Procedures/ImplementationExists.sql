CREATE PROCEDURE [dbo].[ImplementationExists]
	
	@ImplementationId BIGINT,
	@Exists BIT OUTPUT

AS

SET NOCOUNT ON

SET @Exists = CASE WHEN EXISTS (
	SELECT	1
	FROM	dbo.Implementations
	WHERE	ImplementationId = @ImplementationId
) THEN 1 ELSE 0 END