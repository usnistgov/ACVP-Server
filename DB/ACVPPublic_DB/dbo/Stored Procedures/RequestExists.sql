CREATE PROCEDURE [dbo].[RequestExists]
	
	@RequestId BIGINT,
	@Exists BIT OUTPUT

AS

SET NOCOUNT ON

SET @Exists = CASE WHEN EXISTS (
	SELECT	1
	FROM	dbo.ExternalRequests
	WHERE	RequestId = @RequestId
) THEN 1 ELSE 0 END