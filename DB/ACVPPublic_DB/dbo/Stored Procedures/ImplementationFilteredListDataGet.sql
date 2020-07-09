CREATE PROCEDURE [dbo].[ImplementationFilteredListDataGet]
	
	@IDs varchar(MAX)

AS

SET NOCOUNT ON

-- Get the implementation level data
SELECT	 I.ImplementationId
		,I.ImplementationName
		,I.ImplementationVersion
		,I.ImplementationTypeId
		,I.[Url]
		,I.ImplementationDescription
		,I.OrganizationId
		,I.AddressId
FROM dbo.Implementations I
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = I.ImplementationId

-- Get the contacts for all of these
SELECT	 IC.ImplementationId
		,IC.PersonId
		,IC.OrderIndex
FROM dbo.ImplementationContacts IC
	INNER JOIN
	dbo.DelimitedListToTable(@IDs, ',') X ON X.[Value] = IC.ImplementationId
