CREATE PROCEDURE [dbo].[ACVPUserGetById]
	
	@ACVPUserId BIGINT

AS
BEGIN
	SET NOCOUNT ON;

	SELECT	 au.ACVPUserId
			,p.PersonId
			,p.FullName
			,o.OrganizationName
			,o.OrganizationId
			,au.Seed
			,au.[Certificate]
			,au.CommonName
	FROM dbo.ACVPUsers au
		INNER JOIN
		dbo.People p ON au.PersonId = p.PersonId
					AND au.ACVPUserId = @ACVPUserId
		INNER JOIN
		dbo.Organizations o ON p.OrganizationId = o.OrganizationId

END