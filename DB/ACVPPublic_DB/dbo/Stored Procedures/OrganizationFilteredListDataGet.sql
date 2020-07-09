CREATE PROCEDURE [dbo].[OrganizationFilteredListDataGet]

	@IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the organization level data
SELECT	 O.OrganizationId
		,O.OrganizationName
		,O.OrganizationUrl
		,O.VoiceNumber
		,O.FaxNumber
		,O.ParentOrganizationId
FROM dbo.Organizations O
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = O.OrganizationId

-- Get the addresses for these orgs
SELECT	 A.AddressId
		,A.OrganizationId
		,A.OrderIndex
		,A.Street1
		,A.Street2
		,A.Street3
		,A.Locality
		,A.Region
		,A.Country
		,A.PostalCode
FROM dbo.Addresses A
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = A.OrganizationId

-- Get the email addresses for these orgs
SELECT	 E.OrganizationId
		,E.EmailAddress
		,E.OrderIndex
FROM dbo.OrganizationEmails E
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = E.OrganizationId
