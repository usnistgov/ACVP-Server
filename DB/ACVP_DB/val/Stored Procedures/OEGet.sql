CREATE PROCEDURE [val].[OEGet]

	@OEID int = 0

AS

SET NOCOUNT ON

SELECT	 id AS Id
		,[name] as [Name]
FROM val.VALIDATION_OE
WHERE id = @OEID