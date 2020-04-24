CREATE PROCEDURE [val].[PersonExists]
	
	@personId BIGINT,
	@exists BIT OUTPUT

AS

SET NOCOUNT ON

SET @exists = CASE WHEN EXISTS (
	SELECT	1
	FROM	val.PERSON p
	WHERE	id = @personId
) THEN 1 ELSE 0 END