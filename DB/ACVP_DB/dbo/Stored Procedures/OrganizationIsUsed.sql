CREATE PROCEDURE [dbo].[OrganizationIsUsed]

	@OrganizationId bigint

AS

SET NOCOUNT ON

SELECT CAST(CASE
			WHEN EXISTS(SELECT NULL
						FROM dbo.Implementations
						WHERE OrganizationId = @OrganizationId) THEN 1
			WHEN EXISTS(SELECT NULL
						FROM dbo.People
						WHERE OrganizationId = @OrganizationId) THEN 1
			WHEN EXISTS(SELECT NULL
						FROM dbo.Organizations
						WHERE ParentOrganizationId = @OrganizationId) THEN 1
			ELSE 0
		   END AS bit) AS IsUsed