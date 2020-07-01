CREATE PROCEDURE [dbo].[OrganizationEmailDeleteAll]

	@OrganizationId bigint

AS

SET NOCOUNT ON

DELETE 
FROM dbo.OrganizationEmails
WHERE OrganizationId = @OrganizationId