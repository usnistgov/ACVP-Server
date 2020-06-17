
CREATE PROCEDURE [val].[PersonFilteredListDataGet]
	
	 @IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the person level data
SELECT	 P.id AS Id
		,P.org_id AS OrganizationId
		,P.full_name AS [Name]
FROM val.PERSON P
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.Value = P.id

-- Get the email addresses for all of these
SELECT	 P.person_id AS PersonId
		,P.email_address AS EmailAddress
		,P.order_index AS OrderIndex
FROM val.PERSON_EMAIL P
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.Value = P.person_id

-- Get the phone numbers for all of these
SELECT	 P.person_id AS PersonId
		,P.phone_number AS PhoneNumber
		,P.phone_number_type AS PhoneNumberType
		,P.order_index AS OrderIndex
FROM val.PERSON_PHONE P
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.Value = P.person_id