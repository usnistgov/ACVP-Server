CREATE PROCEDURE [dbo].[ImplementationExists]
	
	@ImplementationId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
		WHEN EXISTS (SELECT NULL
					 FROM dbo.Implementations
					 WHERE ImplementationId = @ImplementationId) THEN 1
		ELSE 0
		END AS bit) AS [Exists]