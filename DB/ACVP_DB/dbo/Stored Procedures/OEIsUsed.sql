CREATE PROCEDURE [dbo].[OEIsUsed]

	@OEId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
			WHEN EXISTS(SELECT NULL
						FROM dbo.ValidationOEAlgorithms
						WHERE OEId = @OEId) THEN 1
			ELSE 0
		   END AS bit) AS IsUsed