CREATE PROCEDURE [dbo].[OrganizationEmailsGet]

	@OrganizationId bigint

AS

SET NOCOUNT ON

SELECT	 EmailAddress
		,OrderIndex	 
FROM dbo.OrganizationEmails
WHERE OrganizationId = @OrganizationId
ORDER BY OrderIndex