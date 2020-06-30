CREATE PROCEDURE [dbo].[PersonGet]

	@PersonId bigint

AS

SET NOCOUNT ON

SELECT	 P.PersonId
		,P.FullName
		,O.OrganizationId
		,O.OrganizationName
		,O.OrganizationUrl
		,O.VoiceNumber
		,O.FaxNumber
FROM dbo.People P
	INNER JOIN
	dbo.Organizations O ON O.OrganizationId = P.OrganizationId
						AND P.PersonId = @PersonId