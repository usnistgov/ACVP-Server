CREATE PROCEDURE [dbo].[OrganizationContactsFilteredListDataGet]
	
	@IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the contacts level data
SELECT	 P.PersonId
		,P.FullName
		,P.OrganizationId
FROM dbo.People P
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = P.PersonId

-- Get the email addresses for these contacts
SELECT	 E.PersonId
		,E.EmailAddress
		,E.OrderIndex
FROM dbo.PersonEmails E
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = E.PersonId

-- Get the phone numbers for these contacts
SELECT	 P.PersonId
		,P.PhoneNumber
		,P.PhoneNumberType
		,P.OrderIndex
FROM dbo.PersonPhoneNumbers P
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = P.PersonId
