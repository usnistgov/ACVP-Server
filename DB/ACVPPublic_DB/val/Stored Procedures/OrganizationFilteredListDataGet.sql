CREATE PROCEDURE val.[OrganizationFilteredListDataGet]

	@IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the organization level data
SELECT	 O.id AS Id
		,O.[name] AS [Name]
		,O.organization_url AS Website
		,O.voice_number AS VoiceNumber
		,O.fax_number AS FaxNumber
FROM val.ORGANIZATION O
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = O.id

-- Get the addresses for these orgs
SELECT	 A.id AS Id
		,A.organization_id AS OrganizationId
		,A.order_index AS OrderIndex
		,A.address_street1 AS Street1
		,A.address_street2 AS Street2
		,A.address_street3 AS Street3
		,A.address_locality AS Locality
		,A.address_region AS Region
		,A.address_country AS Country
		,A.address_postal_code AS PostalCode
FROM val.[ADDRESS] A
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = A.organization_id

-- Get the email addresses for these orgs
SELECT	 E.organization_id AS OrganizationId
		,E.email_address AS EmailAddress
		,E.order_index AS OrderIndex
FROM val.ORGANIZATION_EMAIL E
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = E.organization_id