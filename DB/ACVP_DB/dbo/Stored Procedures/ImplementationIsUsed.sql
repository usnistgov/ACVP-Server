CREATE PROCEDURE [dbo].[ImplementationIsUsed]

	@ImplementationId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
			WHEN EXISTS(SELECT NULL
						FROM dbo.Validations
						WHERE ImplementationId = @ImplementationId) THEN 1
			ELSE 0
		   END AS bit) AS IsUsed