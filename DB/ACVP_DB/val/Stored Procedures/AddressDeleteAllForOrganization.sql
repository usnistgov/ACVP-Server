CREATE PROCEDURE [val].[AddressDeleteAllForOrganization]

	@OrganizationID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.[ADDRESS]
WHERE organization_id = @OrganizationID