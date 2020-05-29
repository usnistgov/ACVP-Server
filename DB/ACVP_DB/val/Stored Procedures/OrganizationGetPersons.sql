CREATE PROCEDURE [val].[OrganizationGetPersons]
	
	@OrganizationID bigint

AS

SET NOCOUNT ON

SELECT	 id AS Id
		,full_name AS FullName
FROM val.PERSON
WHERE org_id = @OrganizationID
ORDER BY full_name