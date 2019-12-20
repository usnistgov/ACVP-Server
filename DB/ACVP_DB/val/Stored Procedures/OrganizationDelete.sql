CREATE PROCEDURE [val].[OrganizationDelete]

	@OrganizationID bigint

AS

SET NOCOUNT ON

DELETE 
FROM val.ORGANIZATION
WHERE id = @OrganizationID