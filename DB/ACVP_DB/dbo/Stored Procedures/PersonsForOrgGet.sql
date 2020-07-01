CREATE PROCEDURE [dbo].[PersonsForOrgGet]
	
	@OrganizationId bigint

AS

SET NOCOUNT ON

SELECT	 PersonId
		,FullName
FROM dbo.People
WHERE OrganizationId = @OrganizationId
ORDER BY FullName