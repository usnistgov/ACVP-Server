CREATE PROCEDURE [val].[OrganizationEmailDeleteAll]

	@OrganizationID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.ORGANIZATION_EMAIL
WHERE organization_id = @OrganizationID