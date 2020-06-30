CREATE PROCEDURE [dbo].[AddressDeleteAllForOrganization]

	@OrganizationId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.Addresses
WHERE OrganizationId = @OrganizationId