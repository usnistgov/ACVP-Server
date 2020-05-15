CREATE PROCEDURE [val].[ImplementationIsUsed]

	@ImplementationID BIGINT,
	@inUse BIT OUTPUT

AS

SET NOCOUNT ON

SET @inUse = CASE WHEN EXISTS (
	SELECT	1
	FROM	val.VALIDATION_RECORD v
	WHERE	product_information_id = @ImplementationID
) THEN 1 ELSE 0 END