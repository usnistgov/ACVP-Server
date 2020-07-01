CREATE PROCEDURE [dbo].[ImplementationGet]

	@ImplementationId int = 0

AS

SET NOCOUNT ON

SELECT	 I.ImplementationId 
		,I.[Url]
		,I.ImplementationName
		,I.ImplementationVersion
		,I.ImplementationDescription
		,I.ITAR
		,I.ImplementationTypeId
		,O.OrganizationId
		,O.OrganizationName
		,O.OrganizationUrl
		,O.FaxNumber
		,O.VoiceNumber
		,O.ParentOrganizationId
		,A.AddressId
		,A.Street1
		,A.Street2
		,A.Street3
		,A.Locality
		,A.Region
		,A.PostalCode
		,A.Country
FROM dbo.Implementations I
	INNER JOIN
	dbo.Organizations O ON O.OrganizationId = I.VendorId
						AND I.ImplementationId = @ImplementationId
	INNER JOIN
	dbo.Addresses A ON A.AddressId = I.AddressId