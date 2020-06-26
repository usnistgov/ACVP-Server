
CREATE PROCEDURE [val].[OrganizationContactsFilteredListDataGet]
	
	@IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the contacts level data
SELECT	 p.id AS PersonId
		,p.full_name AS FullName
		,p.org_id AS OrganizationId
FROM val.PERSON P
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = p.id

-- Get the email addresses for these contacts
SELECT	 E.person_id AS PersonId
		,E.email_address AS EmailAddress
		,E.order_index AS OrderIndex
FROM val.PERSON_EMAIL E
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = E.person_id

-- Get the phone numbers for these contacts
SELECT	 p.person_id AS PersonId
		,p.phone_number AS PhoneNumber
		,p.phone_number_type AS PhoneNumberType
		,p.order_index AS OrderIndex
FROM val.PERSON_PHONE p
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = p.person_id