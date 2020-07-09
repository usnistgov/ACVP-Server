CREATE PROCEDURE [dbo].[PersonFilteredListDataGet]
	
	 @IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the person level data
SELECT	 P.PersonId
		,P.OrganizationId
		,P.FullName
FROM dbo.People P
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.Value = P.PersonId

-- Get the email addresses for all of these
SELECT	 E.PersonId
		,E.EmailAddress
		,E.OrderIndex
FROM dbo.PersonEmails E
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.Value = E.PersonId

-- Get the phone numbers for all of these
SELECT	 P.PersonId
		,P.PhoneNumber
		,P.PhoneNumberType
		,P.OrderIndex
FROM dbo.PersonPhoneNumbers P
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.Value = P.PersonId
