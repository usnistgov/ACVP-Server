CREATE PROCEDURE [val].[OrganizationEmailsGet]

	@OrganizationID bigint

AS

SET NOCOUNT ON

SELECT	 email_address AS EmailAddress
		,order_index AS OrderIndex	 
FROM val.ORGANIZATION_EMAIL
WHERE organization_id = @OrganizationID
ORDER BY order_index