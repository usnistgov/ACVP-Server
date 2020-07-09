CREATE PROCEDURE [dbo].[ImplementationIsUsed]

	@ImplementationId BIGINT,
	@InUse BIT OUTPUT

AS

SET NOCOUNT ON

SET @InUse = CASE WHEN EXISTS (
	SELECT	1
	FROM	dbo.Validations
	WHERE	ImplementationId = @ImplementationId
) THEN 1 ELSE 0 END