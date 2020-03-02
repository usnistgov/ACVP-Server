CREATE PROCEDURE [val].[OrganizationExists]
	
	@OrganizationId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
		WHEN EXISTS (SELECT NULL
					 FROM val.ORGANIZATION
					 WHERE id = @OrganizationId) THEN 1
		ELSE 0
		END AS bit) AS [Exists]