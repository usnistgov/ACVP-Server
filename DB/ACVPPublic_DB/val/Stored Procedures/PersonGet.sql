CREATE PROCEDURE [val].[PersonGet]

	@PersonID bigint

AS

SET NOCOUNT ON

SELECT	 id AS ID
        ,full_name AS Name
        ,org_id AS OrganizationID
FROM val.PERSON
WHERE id = @PersonID