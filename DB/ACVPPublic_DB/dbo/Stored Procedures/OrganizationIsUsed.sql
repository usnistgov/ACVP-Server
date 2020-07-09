CREATE PROCEDURE [dbo].[OrganizationIsUsed]

	@OrganizationId BIGINT,
	@IsUsed BIT OUTPUT

AS

SET NOCOUNT ON

SET @IsUsed = 
	CASE
		WHEN EXISTS(SELECT NULL
					FROM dbo.Implementations
					WHERE OrganizationId = @OrganizationId) THEN 1
		WHEN EXISTS(SELECT NULL
					FROM dbo.People
					WHERE OrganizationId = @OrganizationId) THEN 1
		WHEN EXISTS(SELECT NULL
					FROM dbo.Organizations
					WHERE ParentOrganizationId = @OrganizationId) THEN 1
		ELSE 0 END