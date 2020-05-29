CREATE PROCEDURE [external].[RequestExists]
	
	@requestId BIGINT,
	@exists BIT OUTPUT

AS

SET NOCOUNT ON

SET @exists = CASE WHEN EXISTS (
	SELECT	1
	FROM	[external].[REQUEST] r
	WHERE	r.id = @requestId
) THEN 1 ELSE 0 END