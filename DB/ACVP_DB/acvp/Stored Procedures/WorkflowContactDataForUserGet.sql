CREATE PROCEDURE [acvp].[WorkflowContactDataForUserGet]
	
	@ACVPUserID bigint

AS
	
SET NOCOUNT ON

-- Since a person may have multiple emails, only want to get the first one. Doing outer joins just in case the user has no org or email, which doesn't make sense, but strange things happen...
SELECT TOP 1 P.full_name AS [Name]
			,O.[name] AS Lab
			,E.email_address AS EmailAddress
FROM acvp.ACVP_USER U
	INNER JOIN
	val.PERSON P ON P.id = U.person_id
				AND U.id = @ACVPUserID
	LEFT OUTER JOIN
	val.ORGANIZATION O ON O.id = P.org_id
	LEFT OUTER JOIN
	val.PERSON_EMAIL E ON E.person_id = P.id
ORDER BY E.order_index