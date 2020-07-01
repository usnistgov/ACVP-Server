CREATE PROCEDURE [dbo].[OrganizationDelete]

	@OrganizationId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.Organizations
WHERE OrganizationId = @OrganizationId