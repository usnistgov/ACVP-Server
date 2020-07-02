CREATE PROCEDURE [dbo].[WorkflowContactDataForUserGet]
	
	@ACVPUserId bigint

AS
	
SET NOCOUNT ON

-- Since a person may have multiple emails, only want to get the first one. Doing outer joins just in case the user has no org or email, which doesn't make sense, but strange things happen...
SELECT TOP 1 P.FullName
			,O.OrganizationName
			,E.EmailAddress
FROM dbo.ACVPUsers U
	INNER JOIN
	dbo.People P ON P.PersonId = U.PersonId
				AND U.ACVPUserId = @ACVPUserId
	LEFT OUTER JOIN
	dbo.Organizations O ON O.OrganizationId = P.OrganizationId
	LEFT OUTER JOIN
	dbo.PersonEmails E ON E.PersonId = P.PersonId
ORDER BY E.OrderIndex