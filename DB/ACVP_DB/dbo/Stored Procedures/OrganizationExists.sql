CREATE PROCEDURE [dbo].[OrganizationExists]
	
	@OrganizationId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
		WHEN EXISTS (SELECT NULL
					 FROM dbo.Organizations
					 WHERE OrganizationId = @OrganizationId) THEN 1
		ELSE 0
		END AS bit) AS [Exists]